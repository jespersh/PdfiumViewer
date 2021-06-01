namespace PdfiumViewer
{
	/// <summary>
	/// Result to be returned to app_alert
	/// </summary>
	/// <remarks>
	/// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_formfill.h#36
	/// </remarks>
	public enum DialogResults
	{
		Ok = 1,
		Cancel,
		No,
		Yes
	}
}