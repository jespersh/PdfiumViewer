using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PdfiumViewer
{
    internal class PdfFile : IDisposable
    {
        private static readonly Encoding FPDFEncoding = new UnicodeEncoding(false, false, false);

        private IntPtr _document;
        private IntPtr _form;
        private bool _disposed;
        private IPDF_JSPLATFORM _jsPlatform;
        private FPDF_FORMFILLINFO _formCallbacks;
        private IntPtr _formCallbacksHandle;
        private IntPtr _jsPlatformHandle;
        private readonly int _id;
        private Stream _stream;

        private PageData _currentPageData = null;
        private int _currentPageDataPageNumber = -1;

        public PdfFile(Stream stream, string password)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            PdfLibrary.EnsureLoaded();

            _stream = stream;
            _id = StreamManager.Register(stream);
            SynchronizationContext = SynchronizationContext.Current;

            var document = NativeMethods.FPDF_LoadCustomDocument(stream, password, _id);
            if (document == IntPtr.Zero)
                throw new PdfException((PdfError)NativeMethods.FPDF_GetLastError());

            LoadDocument(document);
        }

        public PdfBookmarkCollection Bookmarks { get; private set; }
        public SynchronizationContext SynchronizationContext { get; set; }

        public bool RenderPDFPageToDC(int pageNumber, IntPtr dc, int dpiX, int dpiY, int boundsOriginX, int boundsOriginY, int boundsWidth, int boundsHeight, NativeMethods.FPDF flags)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            NativeMethods.FPDF_RenderPage(dc, GetPageData(pageNumber).Page, boundsOriginX, boundsOriginY, boundsWidth, boundsHeight, 0, flags);

            return true;
        }

        public bool RenderPDFPageToBitmap(int pageNumber, IntPtr bitmapHandle, int dpiX, int dpiY, int boundsOriginX, int boundsOriginY, int boundsWidth, int boundsHeight, int rotate, NativeMethods.FPDF flags, bool renderFormFill)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            var pageData = GetPageData(pageNumber);

            NativeMethods.FPDF_RenderPageBitmap(bitmapHandle, pageData.Page, boundsOriginX, boundsOriginY, boundsWidth, boundsHeight, rotate, flags);

            if (renderFormFill)
            {
                NativeMethods.FPDF_FFLDraw(_form, bitmapHandle, pageData.Page, boundsOriginX, boundsOriginY, boundsWidth, boundsHeight, rotate, flags);
            }

            return true;
        }

        public PdfPageLinks GetPageLinks(int pageNumber, Size pageSize)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            var links = new List<PdfPageLink>();

            int link = 0;
            IntPtr annotation;

            while (NativeMethods.FPDFLink_Enumerate(GetPageData(pageNumber).Page, ref link, out annotation))
            {
                var destination = NativeMethods.FPDFLink_GetDest(_document, annotation);
                int? target = null;
                string uri = null;

                if (destination != IntPtr.Zero)
                    target = (int)NativeMethods.FPDFDest_GetDestPageIndex(_document, destination);

                var action = NativeMethods.FPDFLink_GetAction(annotation);
                if (action != IntPtr.Zero)
                {
                    const uint length = 1024;
                    var sb = new StringBuilder(1024);
                    NativeMethods.FPDFAction_GetURIPath(_document, action, sb, length);

                    uri = sb.ToString();
                }

                var rect = new NativeMethods.FS_RECTF();

                if (NativeMethods.FPDFLink_GetAnnotRect(annotation, rect) && (target.HasValue || uri != null))
                {
                    links.Add(new PdfPageLink(
                        new RectangleF(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top),
                        target,
                        uri
                    ));
                }
            }

            return new PdfPageLinks(links);
        }

        public List<SizeF> GetPDFDocInfo()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);

            int pageCount = NativeMethods.FPDF_GetPageCount(_document);
            var result = new List<SizeF>(pageCount);

            for (int i = 0; i < pageCount; i++)
            {
                result.Add(GetPDFDocInfo(i));
            }

            return result;
        }

        public bool MouseDownForForms(double pageX, double pageY)
        {
            return NativeMethods.FORM_OnLButtonDown(_form, _currentPageData.Page, 0, pageX, pageY);
        }

        public bool MouseUpForForms(double pageX, double pageY)
        {
            return NativeMethods.FORM_OnLButtonUp(_form, _currentPageData.Page, 0, pageX, pageY);
        }

        public bool OnKeyDown(System.Windows.Forms.Keys keyCode, KeyboardModifiers keyboardModifiers)
        {
            return NativeMethods.FORM_OnKeyDown(_form, _currentPageData.Page, keyCode, keyboardModifiers);
        }

        public int HasFormFieldAtPoint(double pageX, double pageY)
        {
            return NativeMethods.FPDFPage_HasFormFieldAtPoint(_form, _currentPageData.Page, pageX, pageY);
        }

        public SizeF GetPDFDocInfo(int pageNumber)
        {
            double height;
            double width;
            NativeMethods.FPDF_GetPageSizeByIndex(_document, pageNumber, out width, out height);

            return new SizeF((float)width, (float)height);
        }

        public void Save(Stream stream)
        {
            NativeMethods.FPDF_SaveAsCopy(_document, stream, NativeMethods.FPDF_SAVE_FLAGS.FPDF_NO_INCREMENTAL);
        }

        protected void LoadDocument(IntPtr document)
        {
            _document = document;

            NativeMethods.FPDF_GetDocPermissions(_document);

            _jsPlatform = new IPDF_JSPLATFORM();
            _jsPlatform.app_alert = new app_alert_callback(this.app_alert);
            _jsPlatform.app_beep = new app_beep_callback(this.app_beep);
            _jsPlatform.app_response = new app_response_callback(this.app_response);
            _jsPlatform.Doc_getFilePath = new Doc_getFilePath_callback(this.Doc_getFilePath);
            _jsPlatform.Doc_gotoPage = new Doc_gotoPage_callback(this.Doc_gotoPage);
            _jsPlatform.Doc_mail = new Doc_mail_callback(this.Doc_mail);
            _jsPlatform.Doc_print = new Doc_print_callback(this.Doc_print);
            _jsPlatform.Doc_submitForm = new Doc_submitForm_callback(this.Doc_submitForm);
            _jsPlatform.Field_browse = new Field_browse_callback(this.Field_Browse);
            _jsPlatform.Version = 1;

            _formCallbacks = new FPDF_FORMFILLINFO();
            _formCallbacks.Release = new ReleaseCallback(this.ReleaseCallback);
            _formCallbacks.FFI_Invalidate = new FFI_InvalidateCallback(this.FFI_InvalidateCallback);
            _formCallbacks.FFI_OutputSelectedRect = new FFI_OutputSelectedRectCallback(this.FFI_OutputSelectedRectCallback);
            _formCallbacks.FFI_SetCursor = new FFI_SetCursorCallback(this.FFI_SetCursorCallback);
            _formCallbacks.FFI_SetTimer = new FFI_SetTimerCallback(this.FFI_SetTimerCallback);
            _formCallbacks.FFI_KillTimer = new FFI_KillTimerCallback(this.FFI_KillTimerCallback);
            _formCallbacks.FFI_GetLocalTime = new FFI_GetLocalTimeCallback(this.FFI_GetLocalTimeCallback);
            _formCallbacks.FFI_OnChange = new FFI_OnChangeCallback(this.FFI_OnChangeCallback);
            _formCallbacks.FFI_GetPage = new FFI_GetPageCallback(this.FFI_GetPageCallback);
            _formCallbacks.FFI_GetCurrentPage = new FFI_GetCurrentPageCallback(this.FFI_GetCurrentPageCallback);
            _formCallbacks.FFI_GetRotation = new FFI_GetRotationCallback(this.FFI_GetRotationCallback);
            _formCallbacks.FFI_ExecuteNamedAction = new FFI_ExecuteNamedActionCallback(this.FFI_ExecuteNamedActionCallback);
            _formCallbacks.FFI_SetTextFieldFocus = new FFI_SetTextFieldFocusCallback(this.FFI_SetTextFieldFocusCallback);
            _formCallbacks.FFI_DoURIAction = new FFI_DoURIActionCallback(this.FFI_DoURIActionCallback);



            // PDF_ENABLE_V8
            //_formCallbacks.m_pJsPlatform = IntPtr.Zero;

            // PDF_ENABLE_XFA
            /*_formCallbacks.FFI_EmailTo = IntPtr.Zero;
            _formCallbacks.FFI_DisplayCaret = Marshal.GetFunctionPointerForDelegate(FFI_DisplayCaretCall);
            _formCallbacks.FFI_SetCurrentPage = IntPtr.Zero;
            _formCallbacks.FFI_GetCurrentPageIndex = IntPtr.Zero;
            _formCallbacks.FFI_GetPageViewRect = IntPtr.Zero;
            _formCallbacks.FFI_GetPlatform = IntPtr.Zero;
            _formCallbacks.FFI_PageEvent = IntPtr.Zero;
            _formCallbacks.FFI_PopupMenu = IntPtr.Zero;
            _formCallbacks.FFI_PostRequestURL = IntPtr.Zero;
            _formCallbacks.FFI_PutRequestURL = IntPtr.Zero;
            _formCallbacks.FFI_UploadTo = IntPtr.Zero;
            _formCallbacks.FFI_DownloadFromURL = IntPtr.Zero;
            _formCallbacks.FFI_OpenFile = IntPtr.Zero;
            _formCallbacks.FFI_GotoURL = IntPtr.Zero;
            _formCallbacks.FFI_GetLanguage = IntPtr.Zero;*/


            //_formCallbacksHandle = GCHandle.Alloc(_formCallbacks, GCHandleType.Pinned);

            _formCallbacks.version = 2;
            int cb = Marshal.SizeOf<FPDF_FORMFILLINFO>();
            _formCallbacksHandle = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr<FPDF_FORMFILLINFO>(_formCallbacks, _formCallbacksHandle, false);

            cb = Marshal.SizeOf<IPDF_JSPLATFORM>();
            _jsPlatformHandle = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr<IPDF_JSPLATFORM>(_jsPlatform, _jsPlatformHandle, false);

            // Depending on whether XFA support is built into the PDFium library, the version
            // needs to be 1 or 2 or 3. We don't really care, so we just try one or the other.
            // Version 3 is v8+xfa

            _form = NativeMethods.FPDFDOC_InitFormFillEnvironmentEx(_document, _formCallbacksHandle, _jsPlatformHandle);

            NativeMethods.FPDF_SetFormFieldHighlightColor(_form, 0, 0xFFE4DD);
            NativeMethods.FPDF_SetFormFieldHighlightAlpha(_form, 50);

            NativeMethods.FORM_DoDocumentJSAction(_form);
            NativeMethods.FORM_DoDocumentOpenAction(_form);

            Bookmarks = new PdfBookmarkCollection();

            LoadBookmarks(Bookmarks, NativeMethods.FPDF_BookmarkGetFirstChild(document, IntPtr.Zero));
        }

        private void LoadBookmarks(PdfBookmarkCollection bookmarks, IntPtr bookmark)
        {
            if (bookmark == IntPtr.Zero)
                return;

            bookmarks.Add(LoadBookmark(bookmark));
            while ((bookmark = NativeMethods.FPDF_BookmarkGetNextSibling(_document, bookmark)) != IntPtr.Zero)
                bookmarks.Add(LoadBookmark(bookmark));
        }

        private PdfBookmark LoadBookmark(IntPtr bookmark)
        {
            var result = new PdfBookmark
            {
                Title = GetBookmarkTitle(bookmark),
                PageIndex = (int)GetBookmarkPageIndex(bookmark)
            };

            //Action = NativeMethods.FPDF_BookmarkGetAction(_bookmark);
            //if (Action != IntPtr.Zero)
            //    ActionType = NativeMethods.FPDF_ActionGetType(Action);

            var child = NativeMethods.FPDF_BookmarkGetFirstChild(_document, bookmark);
            if (child != IntPtr.Zero)
                LoadBookmarks(result.Children, child);

            return result;
        }

        private string GetBookmarkTitle(IntPtr bookmark)
        {
            uint length = NativeMethods.FPDF_BookmarkGetTitle(bookmark, null, 0);
            byte[] buffer = new byte[length];
            NativeMethods.FPDF_BookmarkGetTitle(bookmark, buffer, length);

            string result = Encoding.Unicode.GetString(buffer);
            if (result.Length > 0 && result[result.Length - 1] == 0)
                result = result.Substring(0, result.Length - 1);

            return result;
        }

        private uint GetBookmarkPageIndex(IntPtr bookmark)
        {
            IntPtr dest = NativeMethods.FPDF_BookmarkGetDest(_document, bookmark);
            if (dest != IntPtr.Zero)
                return NativeMethods.FPDFDest_GetDestPageIndex(_document, dest);

            return 0;
        }

        public PdfMatches Search(string text, bool matchCase, bool wholeWord, int startPage, int endPage)
        {
            var matches = new List<PdfMatch>();

            if (String.IsNullOrEmpty(text))
                return new PdfMatches(startPage, endPage, matches);

            for (int page = startPage; page <= endPage; page++)
            {
                var pageData = GetPageData(page);

                NativeMethods.FPDF_SEARCH_FLAGS flags = 0;
                if (matchCase)
                    flags |= NativeMethods.FPDF_SEARCH_FLAGS.FPDF_MATCHCASE;
                if (wholeWord)
                    flags |= NativeMethods.FPDF_SEARCH_FLAGS.FPDF_MATCHWHOLEWORD;

                var handle = NativeMethods.FPDFText_FindStart(pageData.TextPage, FPDFEncoding.GetBytes(text), flags, 0);

                try
                {
                    while (NativeMethods.FPDFText_FindNext(handle))
                    {
                        int index = NativeMethods.FPDFText_GetSchResultIndex(handle);

                        int matchLength = NativeMethods.FPDFText_GetSchCount(handle);

                        var result = new byte[(matchLength + 1) * 2];
                        NativeMethods.FPDFText_GetText(pageData.TextPage, index, matchLength, result);
                        string match = FPDFEncoding.GetString(result, 0, matchLength * 2);

                        matches.Add(new PdfMatch(
                            match,
                            new PdfTextSpan(page, index, matchLength),
                            page
                        ));
                    }
                }
                finally
                {
                    NativeMethods.FPDFText_FindClose(handle);
                }
            }

            return new PdfMatches(startPage, endPage, matches);
        }

        public IList<PdfRectangle> GetTextBounds(PdfTextSpan textSpan)
        {
            return GetTextBounds(GetPageData(textSpan.Page).TextPage, textSpan.Page, textSpan.Offset, textSpan.Length);
        }

        public Point PointFromPdf(int page, PointF point)
        {
            var pageData = GetPageData(page);
            NativeMethods.FPDF_PageToDevice(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                point.X,
                point.Y,
                out var deviceX,
                out var deviceY
            );

            return new Point(deviceX, deviceY);
        }

        public Rectangle RectangleFromPdf(int page, RectangleF rect)
        {
            var pageData = GetPageData(page);
            NativeMethods.FPDF_PageToDevice(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                rect.Left,
                rect.Top,
                out var deviceX1,
                out var deviceY1
            );

            NativeMethods.FPDF_PageToDevice(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                rect.Right,
                rect.Bottom,
                out var deviceX2,
                out var deviceY2
            );

            return new Rectangle(
                deviceX1,
                deviceY1,
                deviceX2 - deviceX1,
                deviceY2 - deviceY1
            );
        }

        public PointF PointToPdf(int page, Point point)
        {
            var pageData = GetPageData(page);
            NativeMethods.FPDF_DeviceToPage(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                point.X,
                point.Y,
                out var deviceX,
                out var deviceY
            );

            return new PointF((float)deviceX, (float)deviceY);
        }

        public RectangleF RectangleToPdf(int page, Rectangle rect)
        {
            var pageData = GetPageData(page);
            NativeMethods.FPDF_DeviceToPage(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                rect.Left,
                rect.Top,
                out var deviceX1,
                out var deviceY1
            );

            NativeMethods.FPDF_DeviceToPage(
                pageData.Page,
                0,
                0,
                (int)pageData.Width,
                (int)pageData.Height,
                0,
                rect.Right,
                rect.Bottom,
                out var deviceX2,
                out var deviceY2
            );

            return new RectangleF(
                (float)deviceX1,
                (float)deviceY1,
                (float)(deviceX2 - deviceX1),
                (float)(deviceY2 - deviceY1)
            );
        }

        private IList<PdfRectangle> GetTextBounds(IntPtr textPage, int page, int index, int matchLength)
        {
            var result = new List<PdfRectangle>();
            RectangleF? lastBounds = null;

            for (int i = 0; i < matchLength; i++)
            {
                var bounds = GetBounds(textPage, index + i);

                if (bounds.Width == 0 || bounds.Height == 0)
                    continue;

                if (
                    lastBounds.HasValue &&
                    AreClose(lastBounds.Value.Right, bounds.Left) &&
                    AreClose(lastBounds.Value.Top, bounds.Top) &&
                    AreClose(lastBounds.Value.Bottom, bounds.Bottom)
                )
                {
                    float top = Math.Max(lastBounds.Value.Top, bounds.Top);
                    float bottom = Math.Min(lastBounds.Value.Bottom, bounds.Bottom);

                    lastBounds = new RectangleF(
                        lastBounds.Value.Left,
                        top,
                        bounds.Right - lastBounds.Value.Left,
                        bottom - top
                    );

                    result[result.Count - 1] = new PdfRectangle(page, lastBounds.Value);
                }
                else
                {
                    lastBounds = bounds;
                    result.Add(new PdfRectangle(page, bounds));
                }
            }

            return result;
        }

        private bool AreClose(float p1, float p2)
        {
            return Math.Abs(p1 - p2) < 4f;
        }

        private RectangleF GetBounds(IntPtr textPage, int index)
        {
            NativeMethods.FPDFText_GetCharBox(
                textPage,
                index,
                out var left,
                out var right,
                out var bottom,
                out var top
            );

            return new RectangleF(
                (float)left,
                (float)top,
                (float)(right - left),
                (float)(bottom - top)
            );
        }

        public string GetPdfText(int page)
        {
            var pageData = GetPageData(page);
            int length = NativeMethods.FPDFText_CountChars(pageData.TextPage);
            return GetPdfText(pageData, new PdfTextSpan(page, 0, length));
        }

        public string GetPdfText(PdfTextSpan textSpan)
        {
            return GetPdfText(GetPageData(textSpan.Page), textSpan);
        }

        private string GetPdfText(PageData pageData, PdfTextSpan textSpan)
        {
            // NOTE: The count parameter in FPDFText_GetText seems to include the null terminator, even though the documentation does not specify this.
            // So to read 40 characters, we need to allocate 82 bytes (2 for the terminator), and request 41 characters from GetText.
            // The return value also includes the terminator (which is documented)
            var result = new byte[(textSpan.Length + 1) * 2];
            int count = NativeMethods.FPDFText_GetText(pageData.TextPage, textSpan.Offset, textSpan.Length + 1, result);
            if (count <= 0)
                return string.Empty;
            return FPDFEncoding.GetString(result, 0, (count - 1) * 2);
        }

        public IList<PdfCharacterInformation> GetCharacterInformation(int page)
        {
            using (var pageData = new PageData(_document, _form, page))
            {
                var result = new List<PdfCharacterInformation>();
                int charCount = NativeMethods.FPDFText_CountChars(pageData.TextPage);
                var allChars = GetPdfText(pageData, new PdfTextSpan(page, 0, charCount)).ToCharArray();

                for (int i = 0; i < charCount; i++)
                {
                    var bounds = GetBounds(pageData.TextPage, i);
                    double fontSize = NativeMethods.FPDFText_GetFontSize(pageData.TextPage, i);
                    result.Add(new PdfCharacterInformation(page, i, allChars[i], fontSize, bounds));
                }

                return result;
            }
        }

        public int GetCharIndexAtPos(PdfPoint location, double xTolerance, double yTolerance)
        {
            return NativeMethods.FPDFText_GetCharIndexAtPos(
                GetPageData(location.Page).TextPage,
                location.Location.X,
                location.Location.Y,
                xTolerance,
                yTolerance
            );
        }

        public bool GetWordAtPosition(PdfPoint location, double xTolerance, double yTolerance, out PdfTextSpan span)
        {
            var index = GetCharIndexAtPos(location, xTolerance, yTolerance);
            if (index < 0)
            {
                span = default(PdfTextSpan);
                return false;
            }

            var baseCharacter = GetCharacter(location.Page, index);
            if (IsWordSeparator(baseCharacter))
            {
                span = default(PdfTextSpan);
                return false;
            }

            int start = index, end = index;

            for (int i = index - 1; i >= 0; i--)
            {
                var c = GetCharacter(location.Page, i);
                if (IsWordSeparator(c))
                    break;
                start = i;
            }

            var count = CountChars(location.Page);
            for (int i = index + 1; i < count; i++)
            {
                var c = GetCharacter(location.Page, i);
                if (IsWordSeparator(c))
                    break;
                end = i;
            }

            span = new PdfTextSpan(location.Page, start, end - start);
            return true;

            bool IsWordSeparator(char c)
            {
                return char.IsSeparator(c) || char.IsPunctuation(c) || char.IsControl(c) || char.IsWhiteSpace(c) || c == '\r' || c == '\n';
            }
        }

        public char GetCharacter(int page, int index)
        {
            return NativeMethods.FPDFText_GetUnicode(GetPageData(page).TextPage, index);
        }

        public int CountChars(int page)
        {
            return NativeMethods.FPDFText_CountChars(GetPageData(page).TextPage);
        }

        public List<PdfRectangle> GetTextRectangles(int page, int startIndex, int count)
        {
            return NativeMethods.FPDFText_GetRectangles(GetPageData(page).TextPage, page, startIndex, count);
        }

        public void DeletePage(int pageNumber)
        {
            NativeMethods.FPDFPage_Delete(_document, pageNumber);
        }

        public PdfRotation GetPageRotation(int pageNumber)
        {
            return NativeMethods.FPDFPage_GetRotation(GetPageData(pageNumber).Page);
        }

        public void RotatePage(int pageNumber, PdfRotation rotation)
        {
            NativeMethods.FPDFPage_SetRotation(GetPageData(pageNumber).Page, rotation);
        }

        public PdfInformation GetInformation()
        {
            var pdfInfo = new PdfInformation();

            pdfInfo.Creator = GetMetaText("Creator");
            pdfInfo.Title = GetMetaText("Title");
            pdfInfo.Author = GetMetaText("Author");
            pdfInfo.Subject = GetMetaText("Subject");
            pdfInfo.Keywords = GetMetaText("Keywords");
            pdfInfo.Producer = GetMetaText("Producer");
            pdfInfo.CreationDate = GetMetaTextAsDate("CreationDate");
            pdfInfo.ModificationDate = GetMetaTextAsDate("ModDate");

            return pdfInfo;
        }

        private string GetMetaText(string tag)
        {
            // Length includes a trailing \0.

            uint length = NativeMethods.FPDF_GetMetaText(_document, tag, null, 0);
            if (length <= 2)
                return string.Empty;

            byte[] buffer = new byte[length];
            NativeMethods.FPDF_GetMetaText(_document, tag, buffer, length);

            return Encoding.Unicode.GetString(buffer, 0, (int)(length - 2));
        }

        public DateTime? GetMetaTextAsDate(string tag)
        {
            string dt = GetMetaText(tag);

            if (string.IsNullOrEmpty(dt))
                return null;

            Regex dtRegex =
                new Regex(
                    @"(?:D:)(?<year>\d\d\d\d)(?<month>\d\d)(?<day>\d\d)(?<hour>\d\d)(?<minute>\d\d)(?<second>\d\d)(?<tz_offset>[+-zZ])?(?<tz_hour>\d\d)?'?(?<tz_minute>\d\d)?'?");

            Match match = dtRegex.Match(dt);

            if (match.Success)
            {
                var year = match.Groups["year"].Value;
                var month = match.Groups["month"].Value;
                var day = match.Groups["day"].Value;
                var hour = match.Groups["hour"].Value;
                var minute = match.Groups["minute"].Value;
                var second = match.Groups["second"].Value;
                var tzOffset = match.Groups["tz_offset"]?.Value;
                var tzHour = match.Groups["tz_hour"]?.Value;
                var tzMinute = match.Groups["tz_minute"]?.Value;

                string formattedDate = $"{year}-{month}-{day}T{hour}:{minute}:{second}.0000000";

                if (!string.IsNullOrEmpty(tzOffset))
                {
                    switch (tzOffset)
                    {
                        case "Z":
                        case "z":
                            formattedDate += "+0";
                            break;
                        case "+":
                        case "-":
                            formattedDate += $"{tzOffset}{tzHour}:{tzMinute}";
                            break;
                    }
                }

                try
                {
                    return DateTime.Parse(formattedDate);
                }
                catch (FormatException)
                {
                    return null;
                }
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                StreamManager.Unregister(_id);

                _currentPageData?.Dispose();
                _currentPageData = null;

                if (_form != IntPtr.Zero)
                {
                    NativeMethods.FORM_DoDocumentAAction(_form, NativeMethods.FPDFDOC_AACTION.WC);
                    NativeMethods.FPDFDOC_ExitFormFillEnvironment(_form);
                    _form = IntPtr.Zero;
                }

                if (_document != IntPtr.Zero)
                {
                    NativeMethods.FPDF_CloseDocument(_document);
                    _document = IntPtr.Zero;
                }

                /*if (_formCallbacksHandle.IsAllocated)
                    _formCallbacksHandle.Free();*/

                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }

                _disposed = true;
            }
        }

        private PageData GetPageData(int pageNumber)
        {
            if (_currentPageDataPageNumber != pageNumber)
            {
                _currentPageData?.Dispose();
                _currentPageData = new PageData(_document, _form, pageNumber);
                _currentPageDataPageNumber = pageNumber;
            }

            return _currentPageData;
        }

        #region FPDF_FORMFILLINFO callbacks

        private void ReleaseCallback(FPDF_FORMFILLINFO pThis)
        {
        }

        private void FFI_InvalidateCallback(FPDF_FORMFILLINFO pThis, IntPtr page, double left, double top, double right, double bottom)
        {
            FS_RECTF rect = new FS_RECTF(left, top, right, bottom);
            if (_currentPageData.Page == page)
            {
                InvalidatePage?.Invoke(this, new InvalidatePageEventArgs(_currentPageData, rect));
            }
        }
        public event EventHandler<InvalidatePageEventArgs> InvalidatePage;

        private void FFI_OutputSelectedRectCallback(FPDF_FORMFILLINFO pThis, IntPtr page, double left, double top, double right, double bottom)
        {
        }

        private void FFI_SetCursorCallback(FPDF_FORMFILLINFO pThis, CursorTypes nCursorType)
        {
        }

        private Dictionary<int, TimerEx> _timers = new Dictionary<int, TimerEx>();
        private static object _syncTimerId = new object();
        private static int __timerid = 0;
        private static int _timerid
        {
            get
            {
                object syncTimerId = PdfFile._syncTimerId;
                int result;
                lock (syncTimerId)
                {
                    result = ++PdfFile.__timerid;
                }
                return result;
            }
        }

        private int FFI_SetTimerCallback(FPDF_FORMFILLINFO pThis, int uElapse, TimerCallback lpTimerFunc)
        {
            TimerEx timerEx = new TimerEx(this.SynchronizationContext, uElapse, PdfFile._timerid, lpTimerFunc);
            this._timers.Add(timerEx.TimerId, timerEx);
            timerEx.Start();
            return timerEx.TimerId;
        }

        private void FFI_KillTimerCallback(FPDF_FORMFILLINFO pThis, int nTimerId)
        {
            if (_timers.ContainsKey(nTimerId))
            {
                _timers[nTimerId].Stop();
                _timers.Remove(nTimerId);
            }
        }

        private FPDF_SYSTEMTIME FFI_GetLocalTimeCallback(FPDF_FORMFILLINFO pThis)
        {
            return new FPDF_SYSTEMTIME();
        }

        private void FFI_OnChangeCallback(FPDF_FORMFILLINFO pThis)
        {
        }

        private IntPtr FFI_GetPageCallback(FPDF_FORMFILLINFO pThis, IntPtr document, int nPageIndex)
        {
            return IntPtr.Zero;
        }

        private IntPtr FFI_GetCurrentPageCallback(FPDF_FORMFILLINFO pThis, IntPtr document)
        {
            return IntPtr.Zero;
        }

        private PageRotation FFI_GetRotationCallback(FPDF_FORMFILLINFO pThis, IntPtr page)
        {
            return PageRotation.Rotate0;
        }

        private void FFI_ExecuteNamedActionCallback(FPDF_FORMFILLINFO pThis, string namedAction)
        {
        }

        private void FFI_SetTextFieldFocusCallback(FPDF_FORMFILLINFO pThis, string value, int valueLen, bool is_focus)
        {
        }

        private void FFI_DoURIActionCallback(FPDF_FORMFILLINFO pThis, string bsURI)
        {
        }

        private DialogResults app_alert(IPDF_JSPLATFORM pThis, string Msg, string Title, ButtonTypes Type, IconTypes Icon)
        {
            return DialogResults.Ok;
        }

        private void app_beep(IPDF_JSPLATFORM pThis, BeepTypes nType)
        {
        }

        private int app_response(IPDF_JSPLATFORM pThis, string Question, string Title, string Default, string cLabel, bool Password, IntPtr buffer, int buflen)
        {
            return 0;
        }

        private int Doc_getFilePath(IPDF_JSPLATFORM pThis, IntPtr filePath, int length)
        {
            return 0;
        }

        private void Doc_gotoPage(IPDF_JSPLATFORM pThis, int nPageNum)
        {
        }

        private void Doc_mail(IPDF_JSPLATFORM pThis, byte[] mailData, int length, bool bUI, string To, string Subject, string Cc, string Bcc, string Msg)
        {
        }

        private void Doc_print(IPDF_JSPLATFORM pThis, bool bUI, int nStart, int nEnd, bool bSilent, bool bShrinkToFit, bool bPrintAsImage, bool bReverse, bool bAnnotations)
        {
        }

        private void Doc_submitForm(IPDF_JSPLATFORM pThis, byte[] formData, int length, string Url)
        {
        }

        private int Field_Browse(IPDF_JSPLATFORM pThis, IntPtr filePath, int length)
        {
            return 0;
        }

        #endregion
    }
}
