using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MySqlBackupTestApp
{
    public partial class FormCompareFile : Form
    {
        bool file1Opened = false;
        bool file2Opened = false;
        string hash1 = "";
        string hash2 = "";
        string file1 = "";
        string file2 = "";

        public FormCompareFile()
        {
            InitializeComponent();
        }

        private void button_OpenFile1_Click(object sender, EventArgs e)
        {
            file1Opened = GetHash(ref file1, ref hash1);
            lbFilePath1.Text = "File: " + file1;
            lbSHA1.Text = "SHA256 Checksum: " + hash1;
            CompareFile();
        }

        private void button_OpenFile2_Click(object sender, EventArgs e)
        {
            file2Opened = GetHash(ref file2, ref hash2);
            lbFilePath2.Text = "File: " + file2;
            lbSHA2.Text = "SHA256 Checksum: " + hash2;
            CompareFile();
        }

        bool GetHash(ref string file, ref string hash)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                if (DialogResult.OK == f.ShowDialog())
                {
                    file = f.FileName;
                    byte[] ba = System.IO.File.ReadAllBytes(f.FileName);
                    hash = System.Security.Cryptography.CryptoExpress.Sha256Hash(ba);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("File not valid.\r\n\r\n" + ex.ToString());
                return false; 
            }
        }

        void CompareFile()
        {
            if (file1Opened && file2Opened)
            {
                if (hash1 == hash2)
                {
                    lbResult.Text = "Match. 100% same content.";
                    lbResult.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lbResult.Text = "Not match. Both files are not same.";
                    lbResult.ForeColor = Color.Red;
                }
            }
            else
            {
                lbResult.Text = "";
            }
        }

        private void btInfo_Click(object sender, EventArgs e)
        {
            string a =
@"This function can be used to find out both EXPORT and IMPORT are working as expected or not by comparing the results.

Instructions:

1. Build the database and fill some data.
2. Export into first dump file.
3. Drop the database.
4. Import from first dump file.
5. Export again into second dump file.
6. Compare the first and second dump by using this SHA256 checksum.
7. If both checksums are match, this will prove that both EXPORT and IMPORT are working good.

Remember to turn off ""Record Dump Time"", as this will create differences between the dump files";
            MessageBox.Show(a, "Info");
        }
    }
}
