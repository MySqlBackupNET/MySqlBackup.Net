using System;
using System.IO;
using System.Security.Cryptography;

namespace System
{
    public class AesHelper
    {
        private static readonly int KeySize = 256;
        private static readonly int SaltSize = 32;

        public static byte[] Encrypt(byte[] sourceBytes, byte[] keyBytes)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.Padding = PaddingMode.PKCS7;

                // Preparing random salt
                var salt = new byte[SaltSize];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }

                using (var deriveBytes = new Rfc2898DeriveBytes(keyBytes, salt, 1000))
                {
                    aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
                    aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);
                }

                using (var encryptor = aes.CreateEncryptor())
                using (var memoryStream = new MemoryStream())
                {
                    // Insert the salt to the first block
                    memoryStream.Write(salt, 0, salt.Length);

                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var binaryWriter = new BinaryWriter(cryptoStream))
                    {
                        binaryWriter.Write(sourceBytes);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        public static byte[] Decrypt(byte[] encryptedBytes, byte[] keyBytes)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.Padding = PaddingMode.PKCS7;

                // Extract the salt from the first block
                var salt = new byte[SaltSize];
                Buffer.BlockCopy(encryptedBytes, 0, salt, 0, SaltSize);

                using (var deriveBytes = new Rfc2898DeriveBytes(keyBytes, salt, 1000))
                {
                    aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
                    aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);
                }

                using (var decryptor = aes.CreateDecryptor())
                using (var inputStream = new MemoryStream(encryptedBytes, SaltSize, encryptedBytes.Length - SaltSize))
                using (var cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                using (var outputStream = new MemoryStream())
                {
                    cryptoStream.CopyTo(outputStream);
                    return outputStream.ToArray();
                }
            }
        }
    }
}