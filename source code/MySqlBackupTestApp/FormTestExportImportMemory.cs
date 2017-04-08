using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace MySqlBackupTestApp
{
    public partial class FormTestExportImportMemory : Form
    {
        byte[] _ba = null;

        public FormTestExportImportMemory()
        {
            InitializeComponent();
            ClearMemory();
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                MySqlConnection conn = new MySqlConnection(Program.ConnectionString);
                MySqlCommand cmd = new MySqlCommand();
                MySqlBackup mb = new MySqlBackup(cmd);
                cmd.Connection = conn;
                conn.Open();
                mb.ExportToMemoryStream(ms);
                conn.Close();
                LoadIntoMemory(ms.ToArray());
                MessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ba == null || _ba.Length == 0)
                {
                    MessageBox.Show("No content is loaded into memory, cannot perform Import/Restore task.");
                    return;
                }

                MemoryStream ms = new MemoryStream(_ba);
                MySqlConnection conn = new MySqlConnection(Program.ConnectionString);
                MySqlCommand cmd = new MySqlCommand();
                MySqlBackup mb = new MySqlBackup(cmd);
                cmd.Connection = conn;
                conn.Open();
                mb.ImportFromMemoryStream(ms);
                conn.Close();
                MessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void LoadIntoMemory(byte[] ba)
        {
            if (ba == null || ba.Length == 0)
            {
                ClearMemory();
            }
            else
            {
                _ba = ba;
                lbStatus.Text = "Loaded into memory.";
                lbStatus.ForeColor = Color.DarkGreen;
                btImport.Enabled = true;
            }
        }

        void ClearMemory()
        {
            _ba = null;
            lbStatus.Text = "No dump content is loaded in memory.";
            lbStatus.ForeColor = Color.Black;
            btImport.Enabled = false;
        }

        private void btLoadFile_Click(object sender, EventArgs e)
        {
            if (!Program.SourceFileExists())
                return;

            byte[] ba = File.ReadAllBytes(Program.TargetFile);

            LoadIntoMemory(ba);

            MessageBox.Show("Loaded into memory.");
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            ClearMemory();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ba == null || _ba.Length == 0)
                {
                    MessageBox.Show("No content is loaded into memory, nothing to save.");
                    return;
                }

                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "*.sql|*.sql|*.*|*.*";
                f.FileName = "MemoryDump.sql";
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    File.WriteAllBytes(f.FileName, _ba);
                }
                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}