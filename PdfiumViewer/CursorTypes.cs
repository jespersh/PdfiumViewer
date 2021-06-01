namespace PdfiumViewer
{
    /// <summary>
    /// The type of cursor returned by FFI_SetCursorCallback
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/fpdfsdk/pwl/ipwl_systemhandler.h#20
    /// </remarks>
    public enum CursorTypes
    {
        /// <summary>
        /// kArrow
        /// </summary>
        Arrow,
        /// <summary>
        /// kNESW
        /// </summary>
        NorthEastSouthWest,
        /// <summary>
        /// kNWSE
        /// </summary>
        NorthWestSouthEast,
        /// <summary>
        /// kVBeam
        /// </summary>
        VerticalBeam,
        /// <summary>
        /// kHBeam
        /// </summary>
        HorizontalBeam,
        /// <summary>
        /// kHand
        /// </summary>
        Hand
    }
}