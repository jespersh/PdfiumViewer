namespace PdfiumViewer
{
    public class InvalidatePageEventArgs
    {
        public InvalidatePageEventArgs(PageData pageData, FS_RECTF rect)
        {
            PageData = pageData;
            Rect = rect;
        }

        public PageData PageData { get; }
        public FS_RECTF Rect { get; }
    }
}