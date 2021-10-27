using System;
using System.Collections.Generic;
using System.Text;

namespace PdfiumViewer
{
    /// <summary>
    /// Configures the print document.
    /// </summary>
    public class PdfPrintSettings
    {
        /// <summary>
        /// Gets the mode used to print margins.
        /// </summary>
        public PdfPrintMode Mode { get; }


        /// <summary>
        /// Gets configuration for printing multiple PDF pages on a single page.
        /// </summary>
        public PdfPrintMultiplePages MultiplePages { get; }

        /// <summary>
        /// Allows overriding the printer DpiX for out of memory exceptions
        /// </summary>
        public float DpiX { get; set; } = -1;

        /// <summary>
        /// Allows overriding the printer DpiY for out of memory exceptions
        /// </summary>
        public float DpiY { get; set; } = -1;

        /// <summary>
        /// Creates a new instance of the PdfPrintSettings class.
        /// </summary>
        /// <param name="mode">The mode used to print margins.</param>
        /// <param name="multiplePages">Configuration for printing multiple PDF
        /// pages on a single page.</param>
        public PdfPrintSettings(PdfPrintMode mode, PdfPrintMultiplePages multiplePages)
        {
            Mode = mode;
            MultiplePages = multiplePages;
        }
    }
}
