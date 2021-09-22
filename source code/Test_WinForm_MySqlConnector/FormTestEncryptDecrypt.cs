using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace MySqlBackupTestApp
{
    public partial class FormTestEncryptDecrypt : Form
    {
        public FormTestEncryptDecrypt()
        {
            InitializeComponent();
        }

        private void btSourceFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                txtSource.Text = f.FileName;
            }
        }

        private void btOutputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            if (DialogResult.OK == f.ShowDialog())
            {
                txtOutput.Text = f.FileName;
            }
        }

        private void btDecrypt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("this function is dropped in V2.3");
            //try
            //{
            //    using (MySqlBackup mb = new MySqlBackup())
            //    {
            //        mb.DecryptDumpFile(txtSource.Text, txtOutput.Text, txtPwd.Text);
            //    }
            //    MessageBox.Show("Done");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btEncrypt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("this function is dropped in V2.3");
            //try
            //{
            //    using (MySqlBackup mb = new MySqlBackup())
            //    {
            //        mb.EncryptDumpFile(txtSource.Text, txtOutput.Text, txtPwd.Text);
            //    }
            //    MessageBox.Show("Done");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btSwitch_Click(object sender, EventArgs e)
        {
            string f1 = txtSource.Text;
            string f2 = txtOutput.Text;
            txtSource.Text = f2;
            txtOutput.Text = f1;
        }
    }
}
