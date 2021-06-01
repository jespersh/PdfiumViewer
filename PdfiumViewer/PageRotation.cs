namespace PdfiumViewer
{
    /// <summary>
    /// Value used by FFI_GetRotation
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_formfill.h#615
    /// </remarks>
    public enum PageRotation
    {
        Rotate0,
        Rotate90,
        Rotate180,
        Rotate270
    }
}