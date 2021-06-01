using System;

namespace PdfiumViewer
{
    public class PageData : IDisposable
    {
        private readonly IntPtr _form;
        private bool _disposed;

        public IntPtr Page { get; private set; }

        public IntPtr TextPage { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public PageData(IntPtr document, IntPtr form, int pageNumber)
        {
            _form = form;

            Page = NativeMethods.FPDF_LoadPage(document, pageNumber);
            TextPage = NativeMethods.FPDFText_LoadPage(Page);
            NativeMethods.FORM_OnAfterLoadPage(Page, form);
            NativeMethods.FORM_DoPageAAction(Page, form, NativeMethods.FPDFPAGE_AACTION.OPEN);

            Width = NativeMethods.FPDF_GetPageWidth(Page);
            Height = NativeMethods.FPDF_GetPageHeight(Page);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                NativeMethods.FORM_DoPageAAction(Page, _form, NativeMethods.FPDFPAGE_AACTION.CLOSE);
                NativeMethods.FORM_OnBeforeClosePage(Page, _form);
                NativeMethods.FPDFText_ClosePage(TextPage);
                NativeMethods.FPDF_ClosePage(Page);

                _disposed = true;
            }
        }
    }
}