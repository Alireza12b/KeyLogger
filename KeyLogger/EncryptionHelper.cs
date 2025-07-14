using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyLogger
{
    public static class EncryptionHelper
    {
        private static readonly byte[] key = Encoding.UTF8.GetBytes("1234567890abcdef");
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("abcdef1234567890"); 

        public static byte[] EncryptString(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor();

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            }
        }
    }
}
