using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

#pragma warning disable 1591

namespace PdfiumViewer
{
    partial class NativeMethods
    {
        // Interned strings are cached over AppDomains. This means that when we
        // lock on this string, we actually lock over AppDomain's. The Pdfium
        // library is not thread safe, and this way of locking
        // guarantees that we don't access the Pdfium library from different
        // threads, even when there are multiple AppDomain's in play.
        private static readonly string LockString = String.Intern("e362349b-001d-4cb2-bf55-a71606a3e36f");

        public static void FPDF_InitLibrary()
        {
            lock (LockString)
            {
                Imports.FPDF_InitLibrary();
            }
        }

        public static void FPDF_InitEmbeddedLibraries()
        {
            string resPath = PdfiumResolver.GetPdfiumResourcesPath() ?? Path.Combine(Path.GetDirectoryName(typeof(NativeMethods).Assembly.Location), IntPtr.Size == 4 ? "x86" : "x64", "res");
            lock (LockString)
            {
                Imports.FPDF_InitEmbeddedLibraries(resPath);
            }
        }

        public static void FPDF_DestroyLibrary()
        {
            lock (LockString)
            {
                Imports.FPDF_DestroyLibrary();
            }
        }

        public static IntPtr FPDF_LoadMemDocument(SafeHandle data_buf, int size, string password)
        {
            lock (LockString)
            {
                return Imports.FPDF_LoadMemDocument(data_buf, size, password);
            }
        }

        public static IntPtr FPDF_LoadMemDocument(byte[] data_buf, int size, string password)
        {
            lock (LockString)
            {
                return Imports.FPDF_LoadMemDocument(data_buf, size, password);
            }
        }

        public static void FPDF_CloseDocument(IntPtr document)
        {
            lock (LockString)
            {
                Imports.FPDF_CloseDocument(document);
            }
        }

        public static int FPDF_GetPageCount(IntPtr document)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetPageCount(document);
            }
        }

        public static uint FPDF_GetDocPermissions(IntPtr document)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetDocPermissions(document);
            }
        }

        public static IntPtr FPDFDOC_InitFormFillEnvironmentEx(IntPtr document, IntPtr formInfo, IntPtr jsPlatform)
        {
            lock (LockString)
            {
                return Imports.FPDFDOC_InitFormFillEnvironmentEx(document, formInfo, jsPlatform);
            }
        }

        public static void FPDF_SetFormFieldHighlightColor(IntPtr hHandle, int fieldType, uint color)
        {
            lock (LockString)
            {
                Imports.FPDF_SetFormFieldHighlightColor(hHandle, fieldType, color);
            }
        }

        public static void FPDF_SetFormFieldHighlightAlpha(IntPtr hHandle, byte alpha)
        {
            lock (LockString)
            {
                Imports.FPDF_SetFormFieldHighlightAlpha(hHandle, alpha);
            }
        }

        public static void FORM_DoDocumentJSAction(IntPtr hHandle)
        {
            lock (LockString)
            {
                Imports.FORM_DoDocumentJSAction(hHandle);
            }
        }

        public static void FORM_DoDocumentOpenAction(IntPtr hHandle)
        {
            lock (LockString)
            {
                Imports.FORM_DoDocumentOpenAction(hHandle);
            }
        }

        public static void FPDFDOC_ExitFormFillEnvironment(IntPtr hHandle)
        {
            lock (LockString)
            {
                Imports.FPDFDOC_ExitFormFillEnvironment(hHandle);
            }
        }

        public static void FORM_DoDocumentAAction(IntPtr hHandle, FPDFDOC_AACTION aaType)
        {
            lock (LockString)
            {
                Imports.FORM_DoDocumentAAction(hHandle, aaType);
            }
        }

        public static IntPtr FPDF_LoadPage(IntPtr document, int page_index)
        {
            lock (LockString)
            {
                return Imports.FPDF_LoadPage(document, page_index);
            }
        }

        public static IntPtr FPDFText_LoadPage(IntPtr page)
        {
            lock (LockString)
            {
                return Imports.FPDFText_LoadPage(page);
            }
        }

        public static void FORM_OnAfterLoadPage(IntPtr page, IntPtr _form)
        {
            lock (LockString)
            {
                Imports.FORM_OnAfterLoadPage(page, _form);
            }
        }

        public static bool FORM_OnLButtonDown(IntPtr form, IntPtr page, int modifier, double page_x, double page_y)
        {
            lock (LockString)
            {
                return Imports.FORM_OnLButtonDown(form, page, modifier, page_x, page_y);
            }
        }

        public static bool FORM_OnLButtonUp(IntPtr form, IntPtr page, int modifier, double page_x, double page_y)
        {
            lock (LockString)
            {
                return Imports.FORM_OnLButtonUp(form, page, modifier, page_x, page_y);
            }
        }

        public static bool FORM_OnKeyDown(IntPtr form, IntPtr page, Keys keyCode, KeyboardModifiers modifier)
        {
            lock (LockString)
            {
                char character = KeyboardHelper.CodeToCharacter((int)keyCode);
                if (character == char.MinValue)
                {
                    return Imports.FORM_OnKeyDown(form, page, (int)keyCode, (int)modifier);
                }
                Imports.FORM_OnKeyDown(form, page, (int)keyCode, (int)modifier);
                return Imports.FORM_OnChar(form, page, character, (int)modifier);
            }
        }

        public static int FPDFPage_HasFormFieldAtPoint(IntPtr _form, IntPtr page, double page_x, double page_y)
        {
            lock (LockString)
            {
                return Imports.FPDFPage_HasFormFieldAtPoint(_form, page, page_x, page_y);
            }
        }

        public static void FORM_DoPageAAction(IntPtr page, IntPtr _form, FPDFPAGE_AACTION fPDFPAGE_AACTION)
        {
            lock (LockString)
            {
                Imports.FORM_DoPageAAction(page, _form, fPDFPAGE_AACTION);
            }
        }

        public static double FPDF_GetPageWidth(IntPtr page)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetPageWidth(page);
            }
        }

        public static double FPDF_GetPageHeight(IntPtr page)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetPageHeight(page);
            }
        }

        public static void FORM_OnBeforeClosePage(IntPtr page, IntPtr _form)
        {
            lock (LockString)
            {
                Imports.FORM_OnBeforeClosePage(page, _form);
            }
        }

        public static void FPDFText_ClosePage(IntPtr text_page)
        {
            lock (LockString)
            {
                Imports.FPDFText_ClosePage(text_page);
            }
        }

        public static void FPDF_ClosePage(IntPtr page)
        {
            lock (LockString)
            {
                Imports.FPDF_ClosePage(page);
            }
        }

        public static void FPDF_RenderPage(IntPtr dc, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags)
        {
            lock (LockString)
            {
                Imports.FPDF_RenderPage(dc, page, start_x, start_y, size_x, size_y, rotate, flags);
            }
        }

        public static void FPDF_RenderPageBitmap(IntPtr bitmapHandle, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags)
        {
            lock (LockString)
            {
                Imports.FPDF_RenderPageBitmap(bitmapHandle, page, start_x, start_y, size_x, size_y, rotate, flags);
            }
        }

        public static int FPDF_GetPageSizeByIndex(IntPtr document, int page_index, out double width, out double height)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetPageSizeByIndex(document, page_index, out width, out height);
            }
        }

        public static IntPtr FPDFBitmap_CreateEx(int width, int height, int format, IntPtr first_scan, int stride)
        {
            lock (LockString)
            {
                return Imports.FPDFBitmap_CreateEx(width, height, format, first_scan, stride);
            }
        }

        public static void FPDFBitmap_FillRect(IntPtr bitmapHandle, int left, int top, int width, int height, uint color)
        {
            lock (LockString)
            {
                Imports.FPDFBitmap_FillRect(bitmapHandle, left, top, width, height, color);
            }
        }

        public static IntPtr FPDFBitmap_Destroy(IntPtr bitmapHandle)
        {
            lock (LockString)
            {
                return Imports.FPDFBitmap_Destroy(bitmapHandle);
            }
        }

        public static IntPtr FPDFText_FindStart(IntPtr page, byte[] findWhat, FPDF_SEARCH_FLAGS flags, int start_index)
        {
            lock (LockString)
            {
                return Imports.FPDFText_FindStart(page, findWhat, flags, start_index);
            }
        }

        public static double FPDFText_GetFontSize(IntPtr page, int index)
        {
            lock (LockString)
            {
                return Imports.FPDFText_GetFontSize(page, index);
            }
        }

        public static int FPDFText_GetSchResultIndex(IntPtr handle)
        {
            lock (LockString)
            {
                return Imports.FPDFText_GetSchResultIndex(handle);
            }
        }

        public static int FPDFText_GetSchCount(IntPtr handle)
        {
            lock (LockString)
            {
                return Imports.FPDFText_GetSchCount(handle);
            }
        }

        public static int FPDFText_GetText(IntPtr page, int start_index, int count, byte[] result)
        {
            lock (LockString)
            {
                return Imports.FPDFText_GetText(page, start_index, count, result);
            }
        }

        public static void FPDFText_GetCharBox(IntPtr page, int index, out double left, out double right, out double bottom, out double top)
        {
            lock (LockString)
            {
                Imports.FPDFText_GetCharBox(page, index, out left, out right, out bottom, out top);
            }
        }

        public static int FPDFText_GetCharIndexAtPos(IntPtr page, double x, double y, double xTolerance, double yTolerance)
        {
            lock (LockString)
            {
                var idx = Imports.FPDFText_GetCharIndexAtPos(page, x, y, xTolerance, yTolerance);
                if (idx == -3)
                    throw new PdfException((PdfError)Imports.FPDF_GetLastError());
                return idx;
            }
        }

        public static int FPDFText_CountChars(IntPtr page)
        {
            lock (LockString)
            {
                return Imports.FPDFText_CountChars(page);
            }
        }

        public static List<PdfRectangle> FPDFText_GetRectangles(IntPtr page, int pageIndex, int startIndex, int characterCount)
        {
            lock (LockString)
            {
                // GetRect uses internal state set by CountRects, so we should call them within the same lock

                var count = Imports.FPDFText_CountRects(page, startIndex, characterCount);
                var rectangles = new List<PdfRectangle>(count);

                for (int i = 0; i < count; i++)
                {
                    if (!Imports.FPDFText_GetRect(page, i, out var left, out var top, out var right, out var bottom))
                        throw new PdfException((PdfError)Imports.FPDF_GetLastError());

                    rectangles.Add(new PdfRectangle(pageIndex, new RectangleF(
                        (float)left,
                        (float)top,
                        (float)(right - left),
                        (float)(bottom - top)
                    )));
                }

                return rectangles;
            }
        }

        public static char FPDFText_GetUnicode(IntPtr page, int index)
        {
            lock (LockString)
            {
                return Imports.FPDFText_GetUnicode(page, index);
            }
        }

        public static bool FPDFText_FindNext(IntPtr handle)
        {
            lock (LockString)
            {
                return Imports.FPDFText_FindNext(handle);
            }
        }

        public static void FPDFText_FindClose(IntPtr handle)
        {
            lock (LockString)
            {
                Imports.FPDFText_FindClose(handle);
            }
        }

        public static bool FPDFLink_Enumerate(IntPtr page, ref int startPos, out IntPtr linkAnnot)
        {
            lock (LockString)
            {
                return Imports.FPDFLink_Enumerate(page, ref startPos, out linkAnnot);
            }
        }

        public static IntPtr FPDFLink_GetDest(IntPtr document, IntPtr link)
        {
            lock (LockString)
            {
                return Imports.FPDFLink_GetDest(document, link);
            }
        }

        public static uint FPDFDest_GetDestPageIndex(IntPtr document, IntPtr dest)
        {
            lock (LockString)
            {
                return Imports.FPDFDest_GetDestPageIndex(document, dest);
            }
        }

        public static bool FPDFLink_GetAnnotRect(IntPtr linkAnnot, FS_RECTF rect)
        {
            lock (LockString)
            {
                return Imports.FPDFLink_GetAnnotRect(linkAnnot, rect);
            }
        }

        public static void FPDF_DeviceToPage(IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, int device_x, int device_y, out double page_x, out double page_y)
        {
            lock (LockString)
            {
                Imports.FPDF_DeviceToPage(page, start_x, start_y, size_x, size_y, rotate, device_x, device_y, out page_x, out page_y);
            }
        }

        public static void FPDF_PageToDevice(IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, double page_x, double page_y, out int device_x, out int device_y)
        {
            lock (LockString)
            {
                Imports.FPDF_PageToDevice(page, start_x, start_y, size_x, size_y, rotate, page_x, page_y, out device_x, out device_y);
            }
        }

        public static IntPtr FPDFLink_GetAction(IntPtr link)
        {
            lock (LockString)
            {
                return Imports.FPDFLink_GetAction(link);
            }
        }

        public static uint FPDFAction_GetURIPath(IntPtr document, IntPtr action, StringBuilder buffer, uint buflen)
        {
            lock (LockString)
            {
                return Imports.FPDFAction_GetURIPath(document, action, buffer, buflen);
            }
        }

        public static IntPtr FPDF_BookmarkGetFirstChild(IntPtr document, IntPtr bookmark)
        {
            lock (LockString)
                return Imports.FPDFBookmark_GetFirstChild(document, bookmark);
        }

        public static IntPtr FPDF_BookmarkGetNextSibling(IntPtr document, IntPtr bookmark)
        {
            lock (LockString)
                return Imports.FPDFBookmark_GetNextSibling(document, bookmark);
        }

        public static uint FPDF_BookmarkGetTitle(IntPtr bookmark, byte[] buffer, uint buflen)
        {
            lock (LockString)
                return Imports.FPDFBookmark_GetTitle(bookmark, buffer, buflen);
        }

        public static IntPtr FPDF_BookmarkGetAction(IntPtr bookmark)
        {
            lock (LockString)
                return Imports.FPDFBookmark_GetAction(bookmark);
        }

        public static IntPtr FPDF_BookmarkGetDest(IntPtr document, IntPtr bookmark)
        {
            lock (LockString)
                return Imports.FPDFBookmark_GetDest(document, bookmark);
        }

        public static uint FPDF_ActionGetType(IntPtr action)
        {
            lock (LockString)
                return Imports.FPDFAction_GetType(action);
        }

        /// <summary>
        /// Opens a document using a .NET Stream. Allows opening huge
        /// PDFs without loading them into memory first.
        /// </summary>
        /// <param name="input">The input Stream. Don't dispose prior to closing the pdf.</param>
        /// <param name="password">Password, if the PDF is protected. Can be null.</param>
        /// <param name="id">Retrieves an IntPtr to the COM object for the Stream. The caller must release this with Marshal.Release prior to Disposing the Stream.</param>
        /// <returns>An IntPtr to the FPDF_DOCUMENT object.</returns>
        public static IntPtr FPDF_LoadCustomDocument(Stream input, string password, int id)
        {
            var getBlock = Marshal.GetFunctionPointerForDelegate(_getBlockDelegate);

            var access = new FPDF_FILEACCESS
            {
                m_FileLen = (uint)input.Length,
                m_GetBlock = getBlock,
                m_Param = (IntPtr)id
            };

            lock (LockString)
            {
                return Imports.FPDF_LoadCustomDocument(access, password);
            }
        }

        public static FPDF_ERR FPDF_GetLastError()
        {
            lock (LockString)
            {
                return (FPDF_ERR)Imports.FPDF_GetLastError();
            }
        }

        public static uint FPDF_GetMetaText(IntPtr document, string tag, byte[] buffer, uint buflen)
        {
            lock (LockString)
            {
                return Imports.FPDF_GetMetaText(document, tag, buffer, buflen);
            }
        }

        #region Save / Edit Methods

        public static PdfRotation FPDFPage_GetRotation(IntPtr page)
        {
            lock (LockString)
            {
                return (PdfRotation)Imports.FPDFPage_GetRotation(page);
            }
        }

        public static void FPDFPage_SetRotation(IntPtr page, PdfRotation rotation)
        {
            lock (LockString)
            {
                Imports.FPDFPage_SetRotation(page, (int)rotation);
            }
        }

        public static bool FPDFPage_GenerateContent(IntPtr page)
        {
            lock (LockString)
            {
                return Imports.FPDFPage_GenerateContent(page);
            }
        }

        public static void FPDFPage_Delete(IntPtr doc, int page)
        {
            lock (LockString)
            {
                Imports.FPDFPage_Delete(doc, page);
            }
        }

        public static bool FPDF_SaveAsCopy(IntPtr doc, Stream output, FPDF_SAVE_FLAGS flags)
        {
            int id = StreamManager.Register(output);

            try
            {
                var write = new FPDF_FILEWRITE
                {
                    stream = (IntPtr)id,
                    version = 1,
                    WriteBlock = Marshal.GetFunctionPointerForDelegate(_saveBlockDelegate)
                };

                lock (LockString)
                {
                    return Imports.FPDF_SaveAsCopy(doc, write, flags);
                }
            }
            finally
            {
                StreamManager.Unregister(id);
            }
        }

        public static bool FPDF_SaveWithVersion(IntPtr doc, Stream output, FPDF_SAVE_FLAGS flags, int fileVersion)
        {
            int id = StreamManager.Register(output);

            try
            {
                var write = new FPDF_FILEWRITE
                {
                    stream = (IntPtr)id,
                    version = 1,
                    WriteBlock = Marshal.GetFunctionPointerForDelegate(_saveBlockDelegate)
                };

                lock (LockString)
                {
                    return Imports.FPDF_SaveWithVersion(doc, write, flags, fileVersion);
                }
            }
            finally
            {
                StreamManager.Unregister(id);
            }
        }

        public static void FPDF_FFLDraw(IntPtr form, IntPtr bitmap, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags)
        {
            lock (LockString)
            {
                Imports.FPDF_FFLDraw(form, bitmap, page, start_x, start_y, size_x, size_y, rotate, flags);
            }
        }
        #endregion

        #region Custom Load/Save Logic
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FPDF_GetBlockDelegate(IntPtr param, uint position, IntPtr buffer, uint size);

        private static readonly FPDF_GetBlockDelegate _getBlockDelegate = FPDF_GetBlock;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int FPDF_SaveBlockDelegate(IntPtr fileWrite, IntPtr data, uint size);

        private static readonly FPDF_SaveBlockDelegate _saveBlockDelegate = FPDF_SaveBlock;

        private static int FPDF_GetBlock(IntPtr param, uint position, IntPtr buffer, uint size)
        {
            var stream = StreamManager.Get((int)param);
            if (stream == null)
                return 0;
            byte[] managedBuffer = new byte[size];

            stream.Position = position;
            int read = stream.Read(managedBuffer, 0, (int)size);
            if (read != size)
                return 0;

            Marshal.Copy(managedBuffer, 0, buffer, (int)size);
            return 1;
        }

        private static int FPDF_SaveBlock(IntPtr fileWrite, IntPtr data, uint size)
        {
            var write = new FPDF_FILEWRITE();
            Marshal.PtrToStructure(fileWrite, write);

            var stream = StreamManager.Get((int)write.stream);
            if (stream == null)
                return 0;

            byte[] buffer = new byte[size];
            Marshal.Copy(data, buffer, 0, (int)size);

            try
            {
                stream.Write(buffer, 0, (int)size);
                return (int)size;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        private static class Imports
        {
            [DllImport("pdfium.dll")]
            public static extern void FPDF_InitLibrary();

            [DllImport("pdfium.dll")]
            public static extern void FPDF_InitEmbeddedLibraries(string resPath);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_DestroyLibrary();

            [DllImport("pdfium.dll", CharSet = CharSet.Ansi)]            
            public static extern IntPtr FPDF_LoadCustomDocument([MarshalAs(UnmanagedType.LPStruct)]FPDF_FILEACCESS access, string password);

            [DllImport("pdfium.dll", CharSet = CharSet.Ansi)]
            public static extern IntPtr FPDF_LoadMemDocument(SafeHandle data_buf, int size, string password);

            [DllImport("pdfium.dll", CharSet = CharSet.Ansi)]
            public static extern IntPtr FPDF_LoadMemDocument(byte[] data_buf, int size, string password);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_CloseDocument(IntPtr document);

            [DllImport("pdfium.dll")]
            public static extern int FPDF_GetPageCount(IntPtr document);

            [DllImport("pdfium.dll")]
            public static extern uint FPDF_GetDocPermissions(IntPtr document);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFDOC_InitFormFillEnvironmentEx(IntPtr document, IntPtr formInfo, IntPtr jsPlatform);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_SetFormFieldHighlightColor(IntPtr hHandle, int fieldType, uint color);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_SetFormFieldHighlightAlpha(IntPtr hHandle, byte alpha);

            [DllImport("pdfium.dll")]
            public static extern void FORM_DoDocumentJSAction(IntPtr hHandle);

            [DllImport("pdfium.dll")]
            public static extern void FORM_DoDocumentOpenAction(IntPtr hHandle);

            [DllImport("pdfium.dll")]
            public static extern void FPDFDOC_ExitFormFillEnvironment(IntPtr hHandle);

            [DllImport("pdfium.dll")]
            public static extern void FORM_DoDocumentAAction(IntPtr hHandle, FPDFDOC_AACTION aaType);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDF_LoadPage(IntPtr document, int page_index);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFText_LoadPage(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern void FORM_OnAfterLoadPage(IntPtr page, IntPtr _form);

            [DllImport("pdfium.dll")]
            public static extern bool FORM_OnLButtonDown(IntPtr _form, IntPtr page, int modifier, double page_x, double page_y);

            [DllImport("pdfium.dll")]
            public static extern bool FORM_OnLButtonUp(IntPtr _form, IntPtr page, int modifier, double page_x, double page_y);

            [DllImport("pdfium.dll")]
            public static extern bool FORM_OnKeyDown(IntPtr _form, IntPtr page, int keyCode, int modifier);

            [DllImport("pdfium.dll")]
            public static extern bool FORM_OnChar(IntPtr _form, IntPtr page, int keyCode, int modifier);

            [DllImport("pdfium.dll")]
            public static extern int FPDFPage_HasFormFieldAtPoint(IntPtr page, IntPtr _form, double page_x, double page_y);

            [DllImport("pdfium.dll")]
            public static extern void FORM_DoPageAAction(IntPtr page, IntPtr _form, FPDFPAGE_AACTION fPDFPAGE_AACTION);

            [DllImport("pdfium.dll")]
            public static extern double FPDF_GetPageWidth(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern double FPDF_GetPageHeight(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern void FORM_OnBeforeClosePage(IntPtr page, IntPtr _form);

            [DllImport("pdfium.dll")]
            public static extern void FPDFText_ClosePage(IntPtr text_page);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_ClosePage(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_RenderPage(IntPtr dc, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_RenderPageBitmap(IntPtr bitmapHandle, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags);

            [DllImport("pdfium.dll")]
            public static extern int FPDF_GetPageSizeByIndex(IntPtr document, int page_index, out double width, out double height);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBitmap_CreateEx(int width, int height, int format, IntPtr first_scan, int stride);

            [DllImport("pdfium.dll")]
            public static extern void FPDFBitmap_FillRect(IntPtr bitmapHandle, int left, int top, int width, int height, uint color);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBitmap_Destroy(IntPtr bitmapHandle);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFText_FindStart(IntPtr page, byte[] findWhat, FPDF_SEARCH_FLAGS flags, int start_index);

            [DllImport("pdfium.dll")]
            public static extern double FPDFText_GetFontSize(IntPtr page, int index);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_GetSchResultIndex(IntPtr handle);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_GetSchCount(IntPtr handle);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_GetText(IntPtr page, int start_index, int count, byte[] result);

            [DllImport("pdfium.dll")]
            public static extern void FPDFText_GetCharBox(IntPtr page, int index, out double left, out double right, out double bottom, out double top);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_GetCharIndexAtPos(IntPtr page, double x, double y, double xTolerance, double yTolerance);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_CountChars(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern int FPDFText_CountRects(IntPtr page, int startIndex, int count);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFText_GetRect(IntPtr page, int index, out double left, out double top, out double right, out double bottom);

            [DllImport("pdfium.dll", CharSet = CharSet.Unicode)]
            public static extern char FPDFText_GetUnicode(IntPtr page, int index);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFText_FindNext(IntPtr handle);

            [DllImport("pdfium.dll")]
            public static extern void FPDFText_FindClose(IntPtr handle);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFLink_Enumerate(IntPtr page, ref int startPos, out IntPtr linkAnnot);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFLink_GetDest(IntPtr document, IntPtr link);

            [DllImport("pdfium.dll")]
            public static extern uint FPDFDest_GetDestPageIndex(IntPtr document, IntPtr dest);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFLink_GetAnnotRect(IntPtr linkAnnot, FS_RECTF rect);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_DeviceToPage(IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, int device_x, int device_y, out double page_x, out double page_y);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_PageToDevice(IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, double page_x, double page_y, out int device_x, out int device_y);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFLink_GetAction(IntPtr link);

            [DllImport("pdfium.dll")]
            public static extern uint FPDFAction_GetURIPath(IntPtr document, IntPtr action, StringBuilder buffer, uint buflen);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBookmark_GetFirstChild(IntPtr document, IntPtr bookmark);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBookmark_GetNextSibling(IntPtr document, IntPtr bookmark);

            [DllImport("pdfium.dll")]
            public static extern uint FPDFBookmark_GetTitle(IntPtr bookmark, byte[] buffer, uint buflen);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBookmark_GetAction(IntPtr bookmark);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFBookmark_GetDest(IntPtr document, IntPtr bookmark);

            [DllImport("pdfium.dll")]
            public static extern uint FPDFAction_GetType(IntPtr action);

            [DllImport("pdfium.dll")]
            public static extern uint FPDF_GetLastError();

            [DllImport("pdfium.dll")]
            public static extern uint FPDF_GetMetaText(IntPtr document, string tag, byte[] buffer, uint buflen);

            #region Save/Edit APIs

            [DllImport("pdfium.dll")]
            public static extern bool FPDF_ImportPages(IntPtr destDoc, IntPtr srcDoc, [MarshalAs(UnmanagedType.LPStr)]string pageRange, int index);

            [DllImport("pdfium.dll")]
            public static extern bool FPDF_SaveAsCopy(IntPtr doc,
                [MarshalAs(UnmanagedType.LPStruct)]FPDF_FILEWRITE writer,
                [MarshalAs(UnmanagedType.I4)]FPDF_SAVE_FLAGS flag);

            [DllImport("pdfium.dll")]
            public static extern bool FPDF_SaveWithVersion(IntPtr doc,
                [MarshalAs(UnmanagedType.LPStruct)]FPDF_FILEWRITE writer,
                [MarshalAs(UnmanagedType.I4)]FPDF_SAVE_FLAGS flags,
                int fileVersion);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFPage_New(IntPtr doc, int index, double width, double height);

            [DllImport("pdfium.dll")]
            public static extern void FPDFPage_Delete(IntPtr doc, int index);

            [DllImport("pdfium.dll")]
            public static extern int FPDFPage_GetRotation(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern void FPDFPage_SetRotation(IntPtr page, int rotate);

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDF_CreateNewDocument();

            [DllImport("pdfium.dll")]
            public static extern IntPtr FPDFPageObj_NewImgeObj(IntPtr document);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFImageObj_SetBitmap(IntPtr pages, int count, IntPtr imageObject, IntPtr bitmap);

            [DllImport("pdfium.dll")]
            public static extern void FPDFPageObj_Transform(IntPtr page, double a, double b, double c, double d, double e, double f);

            [DllImport("pdfium.dll")]
            public static extern void FPDFPage_InsertObject(IntPtr page, IntPtr pageObject);

            [DllImport("pdfium.dll")]
            public static extern bool FPDFPage_GenerateContent(IntPtr page);

            [DllImport("pdfium.dll")]
            public static extern void FPDF_FFLDraw(IntPtr form, IntPtr bitmap, IntPtr page, int start_x, int start_y, int size_x, int size_y, int rotate, FPDF flags);

            #endregion
        }

        public enum FPDF_UNSP
        {
            DOC_XFAFORM = 1,
            DOC_PORTABLECOLLECTION = 2,
            DOC_ATTACHMENT = 3,
            DOC_SECURITY = 4,
            DOC_SHAREDREVIEW = 5,
            DOC_SHAREDFORM_ACROBAT = 6,
            DOC_SHAREDFORM_FILESYSTEM = 7,
            DOC_SHAREDFORM_EMAIL = 8,
            ANNOT_3DANNOT = 11,
            ANNOT_MOVIE = 12,
            ANNOT_SOUND = 13,
            ANNOT_SCREEN_MEDIA = 14,
            ANNOT_SCREEN_RICHMEDIA = 15,
            ANNOT_ATTACHMENT = 16,
            ANNOT_SIG = 17
        }

        public enum FPDFDOC_AACTION
        {
            WC = 0x10,
            WS = 0x11,
            DS = 0x12,
            WP = 0x13,
            DP = 0x14
        }

        public enum FPDFPAGE_AACTION
        {
            OPEN = 0,
            CLOSE = 1
        }

        [Flags]
        public enum FPDF
        {
            ANNOT = 0x01,
            LCD_TEXT = 0x02,
            NO_NATIVETEXT = 0x04,
            GRAYSCALE = 0x08,
            DEBUG_INFO = 0x80,
            NO_CATCH = 0x100,
            RENDER_LIMITEDIMAGECACHE = 0x200,
            RENDER_FORCEHALFTONE = 0x400,
            PRINTING = 0x800,
            REVERSE_BYTE_ORDER = 0x10
        }

        [Flags]
        public enum FPDF_SEARCH_FLAGS
        {
            FPDF_MATCHCASE = 1,
            FPDF_MATCHWHOLEWORD = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        public class FS_RECTF
        {
            public float left;
            public float top;
            public float right;
            public float bottom;
        }

        public enum FPDF_ERR : uint
        {
            FPDF_ERR_SUCCESS = 0,		// No error.
            FPDF_ERR_UNKNOWN = 1,		// Unknown error.
            FPDF_ERR_FILE = 2,		// File not found or could not be opened.
            FPDF_ERR_FORMAT = 3,		// File not in PDF format or corrupted.
            FPDF_ERR_PASSWORD = 4,		// Password required or incorrect password.
            FPDF_ERR_SECURITY = 5,		// Unsupported security scheme.
            FPDF_ERR_PAGE = 6		// Page not found or content error.
        }

        #region Save/Edit Structs and Flags
        [Flags]
        public enum FPDF_SAVE_FLAGS
        {
            FPDF_INCREMENTAL = 1,
            FPDF_NO_INCREMENTAL = 2,
            FPDF_REMOVE_SECURITY = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public class FPDF_FILEACCESS
        {
            public uint m_FileLen;
            public IntPtr m_GetBlock;
            public IntPtr m_Param;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class FPDF_FILEWRITE
        {
            public int version;
            public IntPtr WriteBlock;
            public IntPtr stream;
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FPDF_FORMFILLINFO
    {
        public int version;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public ReleaseCallback Release;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_InvalidateCallback FFI_Invalidate;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_OutputSelectedRectCallback FFI_OutputSelectedRect;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_SetCursorCallback FFI_SetCursor;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_SetTimerCallback FFI_SetTimer;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_KillTimerCallback FFI_KillTimer;

        public FFI_GetLocalTimeCallback FFI_GetLocalTime;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_OnChangeCallback FFI_OnChange;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_GetPageCallback FFI_GetPage;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_GetCurrentPageCallback FFI_GetCurrentPage;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_GetRotationCallback FFI_GetRotation;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_ExecuteNamedActionCallback FFI_ExecuteNamedAction;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_SetTextFieldFocusCallback FFI_SetTextFieldFocus;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_DoURIActionCallback FFI_DoURIAction;

        [MarshalAs(UnmanagedType.FunctionPtr)]
        public FFI_DoGoToActionCallback FFI_DoGoToAction;

        public IntPtr m_pJsPlatform;

        public int xfa_disabled = 0;

        public IntPtr FFI_DisplayCaret;
        public IntPtr FFI_GetCurrentPageIndex;
        public IntPtr FFI_SetCurrentPage;
        public IntPtr FFI_GotoURL;
        public IntPtr FFI_GetPageViewRect;
        public IntPtr FFI_PageEvent;
        public IntPtr FFI_PopupMenu;
        public IntPtr FFI_OpenFile;
        public IntPtr FFI_EmailTo;
        public IntPtr FFI_UploadTo;
        public IntPtr FFI_GetPlatform;
        public IntPtr FFI_GetLanguage;
        public IntPtr FFI_DownloadFromURL;
        public IntPtr FFI_PostRequestURL;
        public IntPtr FFI_PutRequestURL;
        public IntPtr FFI_OnFocusChange;
        public IntPtr FFI_DoURIActionWithKeyboardModifier;

    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_InvalidateCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr page, 
        [MarshalAs(UnmanagedType.R8)] double left, 
        [MarshalAs(UnmanagedType.R8)] double top, 
        [MarshalAs(UnmanagedType.R8)] double right, 
        [MarshalAs(UnmanagedType.R8)] double bottom);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_OutputSelectedRectCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr page, 
        [MarshalAs(12)] double left, 
        [MarshalAs(12)] double top, 
        [MarshalAs(12)] double right, 
        [MarshalAs(12)] double bottom);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_SetCursorCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, CursorTypes nCursorType);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int FFI_SetTimerCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, int uElapse, TimerCallback lptimerFunc);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void TimerCallback(int idEvent);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_KillTimerCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, int nTimerId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate FPDF_SYSTEMTIME FFI_GetLocalTimeCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_OnChangeCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr FFI_GetPageCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr document, int nPageIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr FFI_GetCurrentPageCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr document);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate PageRotation FFI_GetRotationCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, IntPtr page);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_ExecuteNamedActionCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, [MarshalAs(UnmanagedType.LPStr)] string namedAction);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_SetTextFieldFocusCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, [MarshalAs(UnmanagedType.LPWStr)] string value, 
        [MarshalAs(UnmanagedType.I4)] int valueLen, [MarshalAs(UnmanagedType.Bool)] bool is_focus);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_DoURIActionCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, [MarshalAs(UnmanagedType.LPStr)] string bsURI);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void FFI_DoGoToActionCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, int nPageIndex, ZoomModes zoomMode, 
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.R4, SizeParamIndex = 4)] float[] fPosArray, int sizeofArray);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate DialogResults app_alert_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, [MarshalAs(UnmanagedType.LPWStr)] string Msg, 
        [MarshalAs(UnmanagedType.LPWStr)] string Title, ButtonTypes Type, IconTypes Icon);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void app_beep_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, BeepTypes nType);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int app_response_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, [MarshalAs(UnmanagedType.LPWStr)] string Question, 
        [MarshalAs(UnmanagedType.LPWStr)] string Title, [MarshalAs(UnmanagedType.LPWStr)] string Default, [MarshalAs(UnmanagedType.LPWStr)] string cLabel, 
        [MarshalAs(UnmanagedType.Bool)] bool Password, IntPtr buffer, int buflen);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Doc_getFilePath_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, IntPtr filePath, int length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Doc_mail_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] mailData, 
        int length, [MarshalAs(UnmanagedType.Bool)] bool bUI, [MarshalAs(UnmanagedType.LPStr)] string To, [MarshalAs(UnmanagedType.LPStr)] string Subject, 
        [MarshalAs(UnmanagedType.LPStr)] string Cc, [MarshalAs(UnmanagedType.LPStr)] string Bcc, [MarshalAs(UnmanagedType.LPStr)] string Msg);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Doc_print_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, [MarshalAs(UnmanagedType.Bool)] bool bUI, int nStart, int nEnd, 
        [MarshalAs(UnmanagedType.Bool)] bool bSilent, [MarshalAs(UnmanagedType.Bool)] bool bShrinkToFit, [MarshalAs(UnmanagedType.Bool)] bool bPrintAsImage, 
        [MarshalAs(UnmanagedType.Bool)] bool bReverse, [MarshalAs(UnmanagedType.Bool)] bool bAnnotations);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Doc_submitForm_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] formData, 
        int length, [MarshalAs(UnmanagedType.LPWStr)] string Url);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Doc_gotoPage_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, int nPageNum);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Field_browse_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, IntPtr filePath, int length);

}
