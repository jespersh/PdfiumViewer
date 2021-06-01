namespace PdfiumViewer
{
    /// <summary>
    /// Zoommode defined by FFI_DoGoToAction
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_formfill.h#707
    /// </remarks>
    public enum ZoomModes
    {
        None,
        XYZ,
        FitPage,
        FitHorizontal,
        FitVertical,
        FitRectangle,
        FitBoundingbox,
        FitBoundingboxWidth,
        FitBoundingboxHeight
    }
}