using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using MySqlConnector;
using System.IO;

namespace System.pages
{
    public partial class TotalRows : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        void LoadData()
        {
            string databaseName = "";
            string datadir = "";
            StringBuilder sb = new StringBuilder();
            DataTable dtTables = null;
            long totalRows = 0;
            long totalBytes = 0;

            using (var conn = config.GetNewConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();

                databaseName = QueryExpress.ExecuteScalarStr(cmd, "select database();");
                datadir = QueryExpress.ExecuteScalarStr(cmd, "SHOW GLOBAL VARIABLES LIKE 'datadir'", 1);
                dtTables = QueryExpress.GetTable(cmd, $"SHOW FULL TABLES WHERE Table_type = 'BASE TABLE';");
                dtTables.Columns.Add("total_rows", typeof(long));

                foreach (DataRow dr in dtTables.Rows)
                {
                    string tableName = dr[0] + "";
                    long totalRowsTable = QueryExpress.ExecuteScalarLong(cmd, $"select count(*) from `{QueryExpress.EscapeIdentifier(tableName)}`");
                    dr["total_rows"] = totalRowsTable;
                    totalRows += totalRowsTable;
                }
            }

            // Calculate database size by examining files in the database directory
            try
            {
                string databasePath = Path.Combine(datadir, databaseName);

                sb.AppendLine($"<p>Database Folder: {databasePath}</p>");

                if (Directory.Exists(databasePath))
                {
                    DirectoryInfo dbDir = new DirectoryInfo(databasePath);

                    foreach (var file in dbDir.EnumerateFiles())
                    {
                        totalBytes += file.Length;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle potential access issues
                sb.AppendLine($"<!-- Error calculating database size: {ex.Message} -->");
            }

            // Build the output
            sb.AppendLine($"<h3>Database Information</h3>");
            sb.AppendLine($"<p><strong>Database Name:</strong> {databaseName}</p>");
            sb.AppendLine($"<p><strong>Total Size:</strong> {FormatBytes(totalBytes)} ({totalBytes:N0} bytes)</p>");
            sb.AppendLine($"<p><strong>Total Rows:</strong> {totalRows:N0}</p>");

            // Generate HTML table with table information
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th>Table Name</th>");
            sb.AppendLine("<th>Total Rows</th>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            foreach (DataRow dr in dtTables.Rows)
            {
                string tableName = dr[0]?.ToString() ?? "";
                long tableRows = Convert.ToInt64(dr["total_rows"]);

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{System.Web.HttpUtility.HtmlEncode(tableName)}</td>");
                sb.AppendLine($"<td>{tableRows:N0}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            ltResult.Text = sb.ToString();
        }

        protected void btGetTotalRows_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        string FormatBytes(long bytes)
        {
            if (bytes >= 1024 * 1024 * 1024) // GB
            {
                return $"{(bytes / (1024.0 * 1024.0 * 1024.0)):0.###}GB";
            }
            else if (bytes >= 1024 * 1024) // MB
            {
                return $"{(bytes / (1024.0 * 1024.0)):0.###}MB";
            }
            else if (bytes >= 1024) // KB
            {
                return $"{(bytes / 1024.0):0.###}KB";
            }
            else
            {
                return $"{bytes} bytes";
            }
        }
    }
}