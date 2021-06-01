using System.Runtime.InteropServices;

namespace PdfiumViewer
{
    /// <summary>
    /// Declares of a struct type to the local system time.
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_formfill.h#344
    /// </remarks>
    public struct FPDF_SYSTEMTIME
    {
        /// <summary>
        /// years since 1900
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort Year;

        /// <summary>
        /// months since January - [0,11]
        /// </summary>
        /// <remarks>
        /// Beware 0 index
        /// </remarks>
        [MarshalAs(UnmanagedType.I2)]
        public ushort Month;

        /// <summary>
        /// days since Sunday - [0,6]
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort DayOfWeek;

        /// <summary>
        /// day of the month - [1,31]
        /// </summary>
        /// <remarks>
        /// Seems a bit strange day of month is 1-31, when month is 0-11, it is it what it is.
        /// </remarks>
        [MarshalAs(UnmanagedType.I2)]
        public ushort wDay;

        /// <summary>
        /// hours since midnight - [0,23]
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort wHour;

        /// <summary>
        /// minutes after the hour - [0,59]
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort wMinute;

        /// <summary>
        /// seconds after the minute - [0,59]
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort wSecond;

        /// <summary>
        /// milliseconds after the second - [0,999]
        /// </summary>
        [MarshalAs(UnmanagedType.I2)]
        public ushort wMilliseconds;
    }
}