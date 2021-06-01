using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PdfiumViewer
{
    /// <summary>
    /// Javascript callbacks
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_formfill.h#52
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public class IPDF_JSPLATFORM
    {
        /// <summary>
        /// Version number of the interface. Currently must be 2.
        /// </summary>
        public int Version = 2;

        /// <summary>
        /// pop up a dialog to show warning or hint.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public app_alert_callback app_alert;

        /// <summary>
        /// Causes the system to play a sound. 
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public app_beep_callback app_beep;

        /// <summary>
        /// Displays a dialog box containing a question and an entry field for the user to reply to the question.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public app_response_callback app_response;

        /// <summary>
        /// Get the file path of the current document. 
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Doc_getFilePath_callback Doc_getFilePath;

        /// <summary>
        /// AMails the data buffer as an attachment to all recipients, with or without user interaction. 
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Doc_mail_callback Doc_mail;

        /// <summary>
        ///  Prints all or a specific number of pages of the document.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Doc_print_callback Doc_print;

        /// <summary>
        /// Send the form data to a specified URL.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Doc_submitForm_callback Doc_submitForm;

        /// <summary>
        /// Jump to a specified page.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Doc_gotoPage_callback Doc_gotoPage;

        /// <summary>
        /// Show a file selection dialog, and return the selected file path.
        /// </summary>
        /// <remarks>Required</remarks>
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public Field_browse_callback Field_browse;

        /// <summary>
        /// pointer to FPDF_FORMFILLINFO interface.
        /// </summary>
        private IntPtr m_pFormfillinfo;

        /* Version 2. */
        private IntPtr m_isolate; /* Unused in v3, retain for compatibility. */
        private uint m_v8EmbedderSlot; /* Unused in v3, retain for compatibility. */

        /* Version 3. */
        /* Version 3 moves m_Isolate and m_v8EmbedderSlot to FPDF_LIBRARY_CONFIG. */
    }
}
