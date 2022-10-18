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
    public partial class FormTestCustomTablesExport : Form
    {
        public FormTestCustomTablesExport()
        {
            InitializeComponent();
            LoadData();
            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colnSelect.Index && e.RowIndex > -1)
            {
                dataGridView1.Rows[e.RowIndex].Cells[colnSelect.Index].Value = !Convert.ToBoolean(dataGridView1.Rows[e.RowIndex].Cells[colnSelect.Index].Value);
                dataGridView1.EndEdit();
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {

            DataTable dt = null;
            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    string db = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");

                    if (string.IsNullOrEmpty(db))
                    {
                        lbDb.Text = "Database: No database in use/selected.";
                        return;
                    }

                    lbDb.Text = "Database: " + db;

                    dt = QueryExpress.GetTable(cmd, "SHOW FULL TABLES WHERE `Table_type` = 'BASE TABLE';");

                    conn.Close();
                }
            }

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(dt.Rows.Count);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string table = dt.Rows[i][0]+"";
                dataGridView1.Rows[i].Cells[colnSelect.Index].Value = 1;
                dataGridView1.Rows[i].Cells[colnTable.Index].Value =table;

                string sql = string.Format("SELECT * FROM `{0}`", table);

                dataGridView1.Rows[i].Cells[colnSql.Index].Value = sql;
            }
        }

        private void btSelectNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Cells[0].Value = false;
            }
        }

        private void btSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.Cells[0].Value = true;
            }
        }

        private void btExportDic_Click(object sender, EventArgs e)
        {
            if (!Program.TargetDirectoryIsValid())
                return;

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(r.Cells[colnSelect.Index].Value))
                {
                    string tableName = r.Cells[colnTable.Index].Value + "";
                    string sql = r.Cells[colnSql.Index].Value + "";

                    dic[tableName] = sql;
                }
            }

            if (dic.Count == 0)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ExportInfo.TablesToBeExportedDic = dic;
                            mb.ExportToFile(Program.TargetFile);

                            conn.Close();
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

        private void btExportList_Click(object sender, EventArgs e)
        {
            if (!Program.TargetDirectoryIsValid())
                return;

            List<string> list = new List<string>();
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(r.Cells[colnSelect.Index].Value))
                {
                    string tableName = r.Cells[colnTable.Index].Value + "";
                    list.Add(tableName);
                }
            }

            if (list.Count == 0)
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            mb.ExportInfo.TablesToBeExportedList = list;
                            mb.ExportToFile(Program.TargetFile);

                            conn.Close();
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
    }
}
