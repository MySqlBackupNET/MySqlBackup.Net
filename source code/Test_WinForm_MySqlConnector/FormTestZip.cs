using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;
using System.IO.Compression;
using System.IO;

namespace MySqlBackupTestApp
{
    public partial class FormTestZip : Form
    {
        public FormTestZip()
        {
            InitializeComponent();
        }

        private void btExportMemoryZip_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog f = new SaveFileDialog();
                f.Filter = "Zip|*.zip";
                f.FileName = "ZipTest " + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".zip";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string zipFilePath = f.FileName;
                string zipFileName = "SqlDump.sql";

                using (MemoryStream ms = new MemoryStream())
                {
                    using (TextWriter tw = new StreamWriter(ms, new UTF8Encoding(false)))
                    {
                        using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                using (MySqlBackup mb = new MySqlBackup(cmd))
                                {
                                    cmd.Connection = conn;
                                    conn.Open();

                                    mb.ExportToTextWriter(tw);
                                    conn.Close();

                                    using (ZipStorer zip = ZipStorer.Create(zipFilePath, "MySQL Dump"))
                                    {
                                        ms.Position = 0;
                                        zip.AddStream(ZipStorer.Compression.Deflate, zipFileName, ms, DateTime.Now, "MySQL Dump");
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btImportUnzipMemoryStream_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog f = new OpenFileDialog();
                f.Filter = "Zip|*.zip";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string file = f.FileName;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ZipStorer zip = ZipStorer.Open(file, FileAccess.Read))
                    {
                        List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
                        zip.ExtractFile(dir[0], ms);

                        ms.Position = 0;
                        using (TextReader tr = new StreamReader(ms))
                        {
                            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    using (MySqlBackup mb = new MySqlBackup(cmd))
                                    {
                                        cmd.Connection = conn;
                                        conn.Open();

                                        mb.ImportFromTextReader(tr);

                                        conn.Close();
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btImportUnzipFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                of.Filter = "Zip|*.zip";
                of.Title = "Select the Zip file";
                of.Multiselect = false;
                if (of.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string zipfile = of.FileName;

                FolderBrowserDialog f = new FolderBrowserDialog();
                f.Description = "Extract the dump file to which folder?";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string folder = f.SelectedPath;
                string dumpFile = "";

                using (ZipStorer zip = ZipStorer.Open(zipfile, FileAccess.Read))
                {
                    List<ZipStorer.ZipFileEntry> dir = zip.ReadCentralDir();
                    dumpFile = folder + "\\" + dir[0].FilenameInZip;
                    zip.ExtractFile(dir[0], dumpFile);
                }

                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ImportFromFile(dumpFile);

                            conn.Close();
                        }
                    }
                }

                MessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btExportFileZip_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog f = new FolderBrowserDialog();
                f.Description = "Select a folder to save the dump file and zip file.";
                if (f.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                string timenow = DateTime.Now.ToString("yyyyMMddHHmmss");
                string folder = f.SelectedPath;
                string filename = "dump" + timenow + ".sql";
                string fileDump = f.SelectedPath + "\\" + filename;
                string fileZip = f.SelectedPath + "\\dumpzip" + timenow + ".zip";

                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ExportToFile(fileDump);

                            conn.Close();
                        }
                    }
                }

                using (ZipStorer zip = ZipStorer.Create(fileZip, "MySQL Dump"))
                {
                    zip.AddFile(ZipStorer.Compression.Deflate, fileDump, filename, "MySQL Dump");
                }

                MessageBox.Show("Finished.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
