using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfiumViewer
{
    /// <summary>
    /// Key flags. FWL_EVENTFLAG
    /// </summary>
    /// <remarks>
    /// https://pdfium.googlesource.com/pdfium/+/refs/heads/chromium/4522/public/fpdf_fwlevent.h#17
    /// </remarks>
    public enum KeyboardModifiers
    {
        ShiftKey = 1 << 0,
        ControlKey = 1 << 1,
        AltKey = 1 << 2,
        MetaKey = 1 << 3,
        KeyPad = 1 << 4,
        AutoRepeat = 1 << 5,
        LeftButtonDown = 1 << 6,
        MiddleButtonDown = 1 << 7,
        RightButtonDown = 1 << 8,
    }
}
