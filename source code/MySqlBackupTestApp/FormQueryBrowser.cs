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
    public partial class FormQueryBrowser : Form
    {
        DataTable dt = new DataTable();

        public FormQueryBrowser()
        {
            InitializeComponent();
            textBox1.Text = "SHOW TABLE STATUS;";
            ExecuteSQL(1);
        }

        private void btScript_Click(object sender, EventArgs e)
        {
            ExecuteSQL(2);
        }

        private void btSQL_Click(object sender, EventArgs e)
        {
            ExecuteSQL(1);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.A))
            {
                textBox1.SelectAll();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.Enter)
            {
                ExecuteSQL(1);
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                textBox1.Clear();
                e.SuppressKeyPress = true;
            }
        }

        void ExecuteSQL(int q)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    string sql = textBox1.Text.Trim();

                    if (q == 2)
                    {
                        MySqlScript script = new MySqlScript(conn);
                        script.Query = sql;
                        int i = script.Execute();
                        dt = new DataTable();
                        dt.Columns.Add("Result");
                        dt.Rows.Add(i + " statement(s) executed.");
                        BindData();
                    }
                    else
                    {
                        if (sql.StartsWith("select", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("show", StringComparison.OrdinalIgnoreCase))
                        {
                            if (sql.StartsWith("select", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!sql.ToLower().Contains(" limit "))
                                {
                                    if (sql.EndsWith(";"))
                                    {
                                        sql = sql.Remove(sql.Length - 1);

                                    }
                                    sql += " LIMIT 0,3000;";
                                    textBox1.Text = sql;
                                    textBox1.Refresh();
                                }
                            }

                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                dt = QueryExpress.GetTable(cmd, sql);
                                BindData();
                            }
                        }
                        else
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                cmd.CommandText = sql;
                                int i = cmd.ExecuteNonQuery();
                                dt = new DataTable();
                                dt.Columns.Add("Results");
                                dt.Rows.Add(i + " row(s) affected by the last command.");
                                BindData();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError(ex);
            }
        }

        void WriteError(Exception ex)
        {
            dt = new DataTable();
            dt.Columns.Add("Error");
            dt.Rows.Add(ex.Message);
            BindData();
        }

        void BindData()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<html><head><style>body { font-family: \"Segoe UI\", Arial; line-height: 150%; } table { border: 1px solid #5C5C5C; border-collapse: collapse; } td { font-size: 10pt; padding: 4px; border: 1px solid #5C5C5C; } </style></head>");
            sb.AppendLine("<body>");

            sb.AppendFormat(HtmlExpress.ConvertDataTableToHtmlTable(dt));

            sb.AppendLine("</body>");
            sb.AppendFormat("</html>");

            webBrowser1.DocumentText = sb.ToString();
        }

    }
}
