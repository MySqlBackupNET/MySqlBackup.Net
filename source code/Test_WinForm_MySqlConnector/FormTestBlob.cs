using MySqlConnector;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MySqlBackupTestApp
{
    public partial class FormTestBlob : Form
    {
        public FormTestBlob()
        {
            InitializeComponent();
            lbResult.Text = string.Empty;
        }

        private void btCreateTable_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        cmd.CommandText = "drop table if exists testblob";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = textBox1.Text;
                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }
                MessageBox.Show("done");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() != DialogResult.OK)
                {
                    lbResult.Text = string.Empty;
                    return;
                }

                // Step 1: Compute the hash value of the bytes of the file
                byte[] ba1 = File.ReadAllBytes(of.FileName);
                string hash1 = BitConverter.ToString(System.Security.Cryptography.SHA1Managed.Create().ComputeHash(ba1)).Replace("-", string.Empty).ToLower();

                lbHash1.Text = hash1;
                lbHash2.Text = string.Empty;
                lbResult.Text = string.Empty;
                lbDumpSize.Text = string.Empty;

                this.Refresh();

                // Step 2: Insert blob into database
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from testblob";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "insert into testblob(blobdata)values(@blobdata);";
                        cmd.Parameters.AddWithValue("@blobdata", ba1);
                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }

                if (MessageBox.Show("File inserted into database. Do you want to continue?", "Next", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    lbResult.Text = "Operation Cancelled";
                    return;
                }

                // Step 3: Export the database
                string sqlDump = string.Empty;
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ExportInfo.AddDropTable = true;
                            mb.ExportInfo.ExportTableStructure = true;
                            mb.ExportInfo.ExportRows = true;
                            
                            sqlDump = mb.ExportToString();

                            conn.Close();
                        }
                    }
                }
                lbDumpSize.Text = sqlDump.Length.ToString();
                this.Refresh();

                // Step 4: Reimport the database
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ImportFromString(sqlDump);

                            conn.Close();
                        }
                    }
                }

                // Step 5: Get the blob data from database
                byte[] ba2 = null;
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        cmd.CommandText = "select blobdata from testblob;";
                        ba2 = (byte[])cmd.ExecuteScalar();

                        conn.Close();
                    }
                }

                // Step 6: Compute the hash value of the bytes from database
                string hash2 = BitConverter.ToString(System.Security.Cryptography.SHA1Managed.Create().ComputeHash(ba2)).Replace("-", string.Empty).ToLower();

                lbHash2.Text = hash2;

                if (hash1 == hash2)
                {
                    lbResult.Text = "SUCCESS";
                    lbResult.ForeColor = Color.DarkGreen;
                }
                else
                {
                    lbResult.Text = "FAIL!";
                    lbResult.ForeColor = Color.Red;
                }

                this.Refresh();

                if (MessageBox.Show("test completed. Do you want to write the output bytes to file?", "completed", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveFileDialog sf = new SaveFileDialog();
                    sf.FileName = of.FileName;
                    if(sf.ShowDialog()== DialogResult.OK)
                    {
                        File.WriteAllBytes(sf.FileName, ba2);
                    }
                }
            }
            catch (Exception ex)
            {
                lbResult.Text = ex.Message;
                lbResult.ForeColor = Color.Red;

                MessageBox.Show("Error: " + ex.ToString());
            }
        }
    }
}
