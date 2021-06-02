using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PdfiumViewer.Demo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            PdfiumResolver.Resolve += PdfiumResolver_Resolve;
            PdfiumResolver.ResolveResources += PdfiumResolver_ResolveResources;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void PdfiumResolver_ResolveResources(object sender, PdfiumResolveEventArgs e)
        {
            if (IntPtr.Size == 4)
            {
                e.PdfiumFileName = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "runtimes", "win-x86", "natives");
            }
            else if (IntPtr.Size == 8)
            {
                e.PdfiumFileName = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "runtimes", "win-x64", "natives");
            }
        }

        private static void PdfiumResolver_Resolve(object sender, PdfiumResolveEventArgs e)
        {
            if (IntPtr.Size == 4)
            {
                e.PdfiumFileName = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "runtimes", "win-x86", "natives", "pdfium.dll");
            }
            else if(IntPtr.Size == 8)
            {
                e.PdfiumFileName = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "runtimes", "win-x64", "natives", "pdfium.dll");
            }
        }
    }
}
