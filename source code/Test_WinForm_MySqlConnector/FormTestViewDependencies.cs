using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using System.Security.Cryptography;

namespace MySqlBackupTestApp
{
    public partial class FormTestViewDependencies : Form
    {
        public FormTestViewDependencies()
        {
            InitializeComponent();
        }

        private void btRun_Click(object sender, EventArgs e)
        {
            try
            {
                Step1();
                Step2();
                Step3();
                Step4();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Step1()
        {
            MessageBox.Show("Running Step 1...");

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = string.Format("drop database if exists `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format("create database `{0}`;", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format("use `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();

                        mb.ImportFromString(txtStep1.Text);

                        conn.Close();
                    }
                }
            }
        }

        void Step2()
        {
            MessageBox.Show("Running Step 2...");

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = string.Format("use `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();

                        mb.ExportInfo.RecordDumpTime = false;
                        txtStep2.Text = mb.ExportToString();
                        conn.Close();
                    }
                }
            }

            byte[] ba = System.Text.Encoding.UTF8.GetBytes(txtStep2.Text);
            SHA1Managed sha = new SHA1Managed();
            ba = sha.ComputeHash(ba);
            string shastr = BitConverter.ToString(ba).Replace("-", string.Empty);
            txtSHA1.Text = shastr;
        }

        void Step3()
        {
            MessageBox.Show("Running Step 3...");

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = string.Format("drop database if exists `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format("create database `{0}`;", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = string.Format("use `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();

                        mb.ImportFromString(txtStep2.Text);

                        conn.Close();
                    }
                }
            }
        }

        void Step4()
        {
            MessageBox.Show("Running Step 4...");

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = string.Format("use `{0}`", txtDatabaseName.Text);
                        cmd.ExecuteNonQuery();

                        mb.ExportInfo.RecordDumpTime = false;
                        txtStep4.Text = mb.ExportToString();
                        conn.Close();
                    }
                }
            }

            byte[] ba = System.Text.Encoding.UTF8.GetBytes(txtStep4.Text);
            SHA1Managed sha = new SHA1Managed();
            ba = sha.ComputeHash(ba);
            string shastr = BitConverter.ToString(ba).Replace("-", string.Empty);
            txtSHA2.Text = shastr;
        }
    }
}
