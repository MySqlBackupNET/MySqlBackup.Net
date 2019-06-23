using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace System.Security.Cryptography
{
    public class CryptoExpress
    {
        public static string ConvertByteArrayToHexString(byte[] ba)
        {
            if (ba == null || ba.Length == 0)
                return "";
            // Method 1 (slower)
            //return "0x"+ BitConverter.ToString(bytes).Replace("-", string.Empty);

            // Method 2 (faster)
            char[] c = new char[ba.Length * 2 + 2];
            byte b;
            c[0] = '0'; c[1] = 'x';
            for (int y = 0, x = 2; y < ba.Length; ++y, ++x)
            {
                b = ((byte)(ba[y] >> 4));
                c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = ((byte)(ba[y] & 0xF));
                c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }

        public static string RandomString(int size)
        {
            byte[] randBuffer = new byte[size + (10)];
            RandomNumberGenerator.Create().GetBytes(randBuffer);
            return System.Convert.ToBase64String(randBuffer).Replace("/", string.Empty).Replace("+", string.Empty).Replace("=", string.Empty).Remove(size);
        }

        public static string Sha128Hash(string input)
        {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] ba = Encoding.UTF8.GetBytes(input);
            byte[] ba2 = sha.ComputeHash(ba);
            sha = null;
            return BitConverter.ToString(ba2).Replace("-", string.Empty).ToLower();
        }

        public static string Sha256Hash(string input)
        {
            byte[] ba = Encoding.UTF8.GetBytes(input);
            return Sha256Hash(ba);
        }   

        public static string Sha256Hash(byte[] ba)
        { 
            SHA256Managed sha2 = new SHA256Managed();
            byte[] ba2 = sha2.ComputeHash(ba);
            sha2 = null;
            return BitConverter.ToString(ba2).Replace("-", string.Empty).ToLower();
        }

        public static string Sha512Hash(string input)
        {
            byte[] ba = Encoding.UTF8.GetBytes(input);
            SHA512Managed sha5 = new SHA512Managed();
            byte[] ba2 = sha5.ComputeHash(ba);
            sha5 = null;
            return BitConverter.ToString(ba2).Replace("-", string.Empty).ToLower();
        }

        //public static string AES_Encrypt(string input, string password)
        //{
        //    byte[] clearBytes = System.Text.Encoding.UTF8.GetBytes(input);
        //    byte[] encryptedData = AES_Encrypt(clearBytes, password);
        //    return Convert.ToBase64String(encryptedData);
        //}

        //public static byte[] AES_Encrypt(byte[] input, string password)
        //{
        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //    return AES_Encrypt(input, pdb.GetBytes(32), pdb.GetBytes(16));
        //}

        //public static string AES_Decrypt(string input, string password)
        //{
        //    byte[] cipherBytes = Convert.FromBase64String(input);
        //    byte[] decryptedData = AES_Decrypt(cipherBytes, password);
        //    return System.Text.Encoding.UTF8.GetString(decryptedData);
        //}

        //public static byte[] AES_Decrypt(byte[] input, string password)
        //{
        //    PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //    return AES_Decrypt(input, pdb.GetBytes(32), pdb.GetBytes(16));
        //}

        //static byte[] AES_Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        //{
        //    byte[] encryptedData = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        using (Rijndael alg = Rijndael.Create())
        //        {
        //            alg.Key = Key;
        //            alg.IV = IV;
        //            using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearData, 0, clearData.Length);
        //                cs.Close();
        //            }
        //            encryptedData = ms.ToArray();
        //        }
        //    }
        //    return encryptedData;
        //}

        //static byte[] AES_Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        //{
        //    try
        //    {
        //        byte[] decryptedData = null;
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (Rijndael alg = Rijndael.Create())
        //            {
        //                alg.Key = Key;
        //                alg.IV = IV;
        //                using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(cipherData, 0, cipherData.Length);
        //                    cs.Close();
        //                }
        //                decryptedData = ms.ToArray();
        //            }
        //        }
        //        return decryptedData;
        //    }
        //    catch
        //    {
        //        throw new Exception("Incorrect password or corrupted context.");
        //    }
        //}
    }
}
