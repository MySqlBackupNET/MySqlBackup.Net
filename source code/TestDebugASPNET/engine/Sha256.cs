using System;
using System.IO;
using System.Security.Cryptography;

namespace System
{
    public class Sha256
    {
        public static string Compute(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = new BufferedStream(File.OpenRead(filePath), 1200000))
            {
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}