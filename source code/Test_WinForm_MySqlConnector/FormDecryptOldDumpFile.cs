using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace MySqlBackupTestApp
{
    public partial class FormDecryptOldDumpFile : Form
    {
        public FormDecryptOldDumpFile()
        {
            InitializeComponent();
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Multiselect = false;
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            txtSourceFile.Text = f.FileName;
        }

        private void btSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            txtOutputFile.Text = f.FileName;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            try
            {
                DecryptSqlDumpFile(txtSourceFile.Text, txtOutputFile.Text, txtPwd.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void DecryptSqlDumpFile(string originalFile, string newFile, string encryptionKey)
        {
            encryptionKey = Sha2Hash(encryptionKey);
            int saltSize = GetSaltSize(encryptionKey);

            if (!File.Exists(originalFile))
                throw new Exception("Original file is not exists.");

            UTF8Encoding utf8WithoutBOM = new UTF8Encoding(false);

            using (TextReader textReader = new StreamReader(originalFile, utf8WithoutBOM))
            {
                if (File.Exists(newFile))
                {
                    File.Delete(newFile);
                }

                string line = "";

                using (TextWriter textWriter = new StreamWriter(newFile, false, utf8WithoutBOM))
                {
                    while (line != null)
                    {
                        line = textReader.ReadLine();
                        if (line == null)
                            break;
                        line = DecryptWithSalt(line, encryptionKey, saltSize);
                        if (line.StartsWith("-- ||||"))
                            line = "";
                        
                        textWriter.WriteLine(line);
                    }
                }
            }
        }

        string Sha2Hash(string input)
        {
            byte[] ba = Encoding.UTF8.GetBytes(input);
            return Sha2Hash(ba);
        }

        string Sha2Hash(byte[] ba)
        {
            SHA256Managed sha2 = new SHA256Managed();
            byte[] ba2 = sha2.ComputeHash(ba);
            return BitConverter.ToString(ba2).Replace("-", string.Empty).ToLower();
        }

        int GetSaltSize(string key)
        {
            int a = key.GetHashCode();
            string b = Convert.ToString(a);
            char[] ca = b.ToCharArray();
            int c = 0;
            foreach (char cc in ca)
            {
                if (char.IsNumber(cc))
                    c += Convert.ToInt32(cc.ToString());
            }
            return c;
        }

        string DecryptWithSalt(string input, string key, int saltSize)
        {
            try
            {
                string salt = input.Substring(0, saltSize);
                string data = input.Substring(saltSize);
                return AES_Decrypt(data, key + salt);
            }
            catch
            {
                throw new Exception("Incorrect password or incomplete context.");
            }
        }

        string AES_Decrypt(string input, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(input);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
            byte[] decryptedData = AES_Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.UTF8.GetString(decryptedData);
        }

        byte[] AES_Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherData, 0, cipherData.Length);
                    cs.Close();
                    byte[] decryptedData = ms.ToArray();
                    return decryptedData;
                }
            }
        }
    }
}
