namespace PdfiumViewer
{
    /// <summary>
    /// Rectangle area(float) in device or page coordination system.
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdfview.h#140
    /// </remarks>
    public struct FS_RECTF
    {
        /// <summary>
        /// The x-coordinate of the left-top corner.
        /// </summary>
        public double left;
        /// <summary>
        /// The y-coordinate of the left-top corner.
        /// </summary>
        public double top;
        /// <summary>
        /// The x-coordinate of the right-bottom corner.
        /// </summary>
        public double right;
        /// <summary>
        /// The y-coordinate of the right-bottom corner.
        /// </summary>
        public double bottom;

        public FS_RECTF(double left, double top, double right, double bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }
}
