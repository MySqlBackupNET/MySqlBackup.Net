using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Backup_All_Databases
{
    internal static class EncryptionHelper
    {
        /// <summary>
        /// Loads a salt from a secure file, generating and saving a new one if it doesn't exist.
        /// </summary>
        /// <returns>A 32-byte salt for key derivation.</returns>
        private static byte[] LoadSalt()
        {
            const int saltSize = 32; // 32 bytes is a strong, standard size for a salt

            try
            {
                // Check if the salt file exists
                if (File.Exists(Program.saltFilePath))
                {
                    // Load the existing salt
                    byte[] salt = File.ReadAllBytes(Program.saltFilePath);
                    if (salt.Length != saltSize)
                    {
                        throw new Exception("Invalid salt size in stored file.");
                    }
                    return salt;
                }
                else
                {
                    // Generate a new random salt
                    byte[] salt = new byte[saltSize];
                    using (var rng = new RNGCryptoServiceProvider())
                    {
                        rng.GetBytes(salt);
                    }

                    // Save the salt to a file with restricted permissions
                    File.WriteAllBytes(Program.saltFilePath, salt);
                    Program.SetFilePermissions(Program.saltFilePath); // Reuse the existing method from Program.cs

                    return salt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load or generate salt: {ex.Message}");
            }
        }

        /// <summary>
        /// Generates a 256-bit key from a password using PBKDF2 with a stored salt.
        /// </summary>
        public static byte[] GenerateKey(string password)
        {
            byte[] salt = LoadSalt();
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return deriveBytes.GetBytes(32); // 256-bit key
            }
        }

        /// <summary>
        /// Encrypts data using AES with a provided key.
        /// </summary>
        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts data using AES with a provided key.
        /// </summary>
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[aes.IV.Length];
                Array.Copy(data, 0, iv, 0, iv.Length);
                aes.IV = iv;
                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, iv.Length, data.Length - iv.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Protects a key using DPAPI for the local machine.
        /// </summary>
        public static byte[] ProtectKey(byte[] key)
        {
            return ProtectedData.Protect(key, null, DataProtectionScope.LocalMachine);
        }

        /// <summary>
        /// Unprotects a key using DPAPI for the local machine.
        /// </summary>
        public static byte[] UnprotectKey(byte[] protectedKey)
        {
            return ProtectedData.Unprotect(protectedKey, null, DataProtectionScope.LocalMachine);
        }
    }
}
