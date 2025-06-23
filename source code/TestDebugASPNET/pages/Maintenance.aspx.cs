using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class Maintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
            else
            {
                string action = Request["hiddenPostbackAction"] + "";
                if (action == "dropAllTables")
                {
                    DropTables();
                }
            }
        }

        protected void btDeleteTestDatabase_Click(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("The following databases were deleted...");
            sb.AppendLine();

            using (MySqlConnection conn = config.GetNewConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SHOW DATABASES LIKE 'test%';";
                    using (var reader = cmd.ExecuteReader())
                    {
                        List<string> databases = new List<string>();
                        while (reader.Read())
                        {
                            databases.Add(reader.GetString(0));
                        }

                        reader.Close();

                        foreach (var dbName in databases)
                        {
                            using (var dropCmd = conn.CreateCommand())
                            {
                                dropCmd.CommandText = $"DROP DATABASE `{dbName}`;";
                                dropCmd.ExecuteNonQuery();
                                sb.AppendLine(dbName);
                            }
                        }
                    }
                }
            }

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btDeleteTempDumpFiles_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("The following files were deleted:");
            sb.AppendLine();

            string folder = Server.MapPath("~/temp");
            System.IO.Directory.CreateDirectory(folder);

            foreach (var file in Directory.EnumerateFiles(folder))
            {
                try
                {
                    File.Delete(file);
                    sb.AppendLine(file);
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"Failed to delete: {file}");
                    sb.AppendLine(ex.Message);
                }
            }

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btDeleteTaskReport_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = "delete from progress_report;";
                    cmd.ExecuteNonQuery();
                }
            }

            ((masterPage1)this.Master).ShowMessage("Done", "All task reports deleted", true);
        }

        protected void DropTables()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("The following tables were dropped:");
                sb.AppendLine();

                using (MySqlConnection conn = config.GetNewConnection())
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();

                        DataTable dtTable = QueryExpress.GetTable(cmd, "show tables");

                        foreach (DataRow dr in dtTable.Rows)
                        {
                            try
                            {
                                string tableName = dr[0] + "";
                                cmd.CommandText = $"DROP TABLE IF EXISTS `{QueryExpress.EscapeIdentifier(tableName)}`";
                                cmd.ExecuteNonQuery();

                                sb.AppendLine(tableName);
                            }
                            catch { }
                        }
                    }
                }

                ph1.Controls.Add(new LiteralControl(sb.ToString()));
                ((masterPage1)this.Master).ShowMessage("Done", "All task reports deleted", true);
            }
            catch (Exception ex)
            {
                ph1.Controls.Add(new LiteralControl($@"Error:<br />{ex.Message}<br />{ex.StackTrace}"));
            }
        }
    }
}