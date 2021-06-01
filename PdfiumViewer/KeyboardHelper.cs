using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PdfiumViewer
{
    /// <summary>
    /// Helper class to convert a keyboard scancode character to a unicode character
    /// </summary>
    public class KeyboardHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            Keys[] lpKeyState,
            byte[] pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll", ExactSpelling = true)]
        internal static extern bool GetKeyboardState(Keys[] keyStates);

        public static char CodeToCharacter(int scanCode)
        {
            Keys[] keyStates = new Keys[256];
            if (!GetKeyboardState(keyStates))
                return char.MinValue;

            byte[] chars = new byte[2];
            if(ToUnicode((uint)scanCode, 0, keyStates, chars, 2, 0) == 1)
            {
                return Encoding.Unicode.GetChars(chars, 0, 2)[0];
            }
            return char.MinValue;
        }
    }
}
