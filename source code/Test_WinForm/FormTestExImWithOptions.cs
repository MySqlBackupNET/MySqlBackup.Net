using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlBackupTestApp
{
    public partial class FormExImWithOptions : Form
    {
        public FormExImWithOptions()
        {
            InitializeComponent();

            comboBox_RowsExportMode.ValueMember = "id";
            comboBox_RowsExportMode.DisplayMember = "name";

            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(RowsDataExportMode));
            dt.Columns.Add("name");
            foreach (RowsDataExportMode mode in Enum.GetValues(typeof(RowsDataExportMode)))
            {
                dt.Rows.Add(mode, mode.ToString());
            }

            comboBox_RowsExportMode.DataSource = dt;
            comboBox_RowsExportMode.SelectedIndex = 0;

            dropTextEncoding.SelectedIndex = 0;
            dropBlobExportMode.SelectedIndex = 0;
        }

        private void button_Backup_Click(object sender, EventArgs e)
        {
            if (!Program.TargetDirectoryIsValid())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            mb.ExportInfo.AddDropDatabase = cbAddDropDatabase.Checked;
                            mb.ExportInfo.AddCreateDatabase = cbExAddCreateDatabase.Checked;
                            mb.ExportInfo.AddDropTable = cbAddDropTable.Checked;
                            mb.ExportInfo.ExportTableStructure = cbAddDropTable.Checked;
                            mb.ExportInfo.ExportRows = cbExExportRows.Checked;
                            mb.ExportInfo.RecordDumpTime = cbExRecordDumpTime.Checked;
                            mb.ExportInfo.ResetAutoIncrement = cbExResetAutoIncrement.Checked;
                            //mb.ExportInfo.EnableEncryption = cbExEnableEncryption.Checked;
                            //mb.ExportInfo.EncryptionPassword = txtExPassword.Text;
                            mb.ExportInfo.MaxSqlLength = (int)nmExMaxSqlLength.Value;
                            mb.ExportInfo.ExportFunctions = cbExExportRoutines.Checked;
                            mb.ExportInfo.ExportProcedures = cbExExportRoutines.Checked;
                            mb.ExportInfo.ExportTriggers = cbExExportRoutines.Checked;
                            mb.ExportInfo.ExportEvents = cbExExportRoutines.Checked;
                            mb.ExportInfo.ExportViews = cbExExportRoutines.Checked;
                            mb.ExportInfo.ExportRoutinesWithoutDefiner = cbExExportRoutinesWithoutDefiner.Checked;
                            mb.ExportInfo.RowsExportMode = (RowsDataExportMode)comboBox_RowsExportMode.SelectedValue;
                            mb.ExportInfo.WrapWithinTransaction = checkBox_WrapInTransaction.Checked;
                            if (dropTextEncoding.SelectedIndex < 1)
                                mb.ExportInfo.TextEncoding = new UTF8Encoding(false);
                            else if (dropTextEncoding.SelectedIndex == 1)
                                mb.ExportInfo.TextEncoding = System.Text.Encoding.UTF8;
                            else if (dropTextEncoding.SelectedIndex == 2)
                                mb.ExportInfo.TextEncoding = new ASCIIEncoding();

                            if (dropBlobExportMode.SelectedIndex < 1)
                                mb.ExportInfo.BlobExportMode = BlobDataExportMode.HexString;
                            else
                                mb.ExportInfo.BlobExportMode = BlobDataExportMode.BinaryChar;

                            mb.ExportInfo.BlobExportModeForBinaryStringAllow = cbAllowBinaryChar.Checked;

                            mb.ExportToFile(Program.TargetFile);
                        }
                        conn.Close();
                    }
                }

                MessageBox.Show("Finished. Dump saved at:" + Environment.NewLine + Environment.NewLine + Program.TargetFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Restore_Click(object sender, EventArgs e)
        {
            if (!Program.SourceFileExists())
                return;

            Exception error = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            //mb.ImportInfo.EnableEncryption = cbImEnableEncryption.Checked;
                            //mb.ImportInfo.EncryptionPassword = txtImPwd.Text;
                            mb.ImportInfo.IgnoreSqlError = cbImIgnoreSqlErrors.Checked;
                            //mb.ImportInfo.TargetDatabase = txtImTargetDatabase.Text;
                            //mb.ImportInfo.DatabaseDefaultCharSet = txtImDefaultCharSet.Text;
                            mb.ImportInfo.ErrorLogFile = txtImErrorLogFile.Text;
                            
                            mb.ImportFromFile(Program.TargetFile);

                            error = mb.LastError;
                        }

                        conn.Close();
                    }
                }

                if (error == null)
                    MessageBox.Show("Finished.");
                else
                    MessageBox.Show("Finished with errors." + Environment.NewLine + Environment.NewLine + error.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btImErrorFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "txt|*.txt|*.*|*.*";
            f.FileName = "ImportErrorLog.txt";
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtImErrorLogFile.Text = f.FileName;
            }
        }

    }
}
