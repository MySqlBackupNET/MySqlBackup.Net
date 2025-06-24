using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;

namespace System.pages
{
    public partial class QueryBrowserSQLite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private static readonly HashSet<string> ShowKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "SELECT", "PRAGMA", "EXPLAIN", "WITH"
        };

        private void ProcessQuery(bool overrideDetection, bool isShow)
        {
            try
            {
                StringBuilder resultInfo = new StringBuilder();
                string query = txtSql.Text.Trim();
                bool isShowQuery = overrideDetection ? isShow : DetermineQueryType(query);

                using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    conn.Open();
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        if (isShowQuery)
                        {
                            // Handle "show" queries (SELECT, PRAGMA, etc.)
                            ProcessShowQuery(cmd, resultInfo, stopwatch);
                        }
                        else
                        {
                            // Handle "execute" queries (INSERT, UPDATE, etc.)
                            ProcessExecuteQuery(cmd, resultInfo, stopwatch);
                        }
                    }

                    conn.Close();
                }
            }
            catch (SQLiteException sqliteEx)
            {
                string errorHtml = $"<div style='color: red;'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - SQLite Error:<br/>" +
                                   $"• Error Code: {sqliteEx.ErrorCode}<br/>" +
                                   $"• Result Code: {sqliteEx.ResultCode}<br/>" +
                                   $"• Message: {sqliteEx.Message}</div>";
                phResult.Controls.Add(new LiteralControl(errorHtml));
            }
            catch (Exception ex)
            {
                phResult.Controls.Add(new LiteralControl($"<div style='color: red;'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - General Error: {ex.Message}</div>"));
            }
        }

        private bool DetermineQueryType(string query)
        {
            // Extract the first word of the query (case-insensitive)
            string firstWord = query.Split(new[] { ' ', '\n', '\t', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                   .FirstOrDefault()?.ToUpperInvariant() ?? "";
            return ShowKeywords.Contains(firstWord);
        }

        private void ProcessShowQuery(SQLiteCommand cmd, StringBuilder resultInfo, System.Diagnostics.Stopwatch stopwatch)
        {
            int maxRowsToDisplay = int.TryParse(txtMaxRows.Text, out int maxRows) ? maxRows : 100;
            int maxTextValueLength = int.TryParse(txtMaxTextValueLength.Text, out int maxText) ? maxText : 500;
            txtMaxRows.Text = maxRowsToDisplay.ToString();
            txtMaxTextValueLength.Text = maxTextValueLength.ToString();

            StringBuilder sb = new StringBuilder();
            int rowCount = 0;

            using (SQLiteDataReader reader = cmd.ExecuteReader())
            {
                sb.Append("<div class='div-table-result'><table>");

                if (reader.HasRows)
                {
                    // Build header
                    sb.Append("<thead><tr>");
                    List<Type> columnTypes = new List<Type>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Type colType = reader.GetFieldType(i);
                        columnTypes.Add(colType);
                        sb.Append("<th title='");
                        sb.Append(System.Web.HttpUtility.HtmlEncode($"Type: {colType.Name}"));
                        sb.Append("'>");
                        sb.Append(System.Web.HttpUtility.HtmlEncode(reader.GetName(i)));
                        sb.Append("</th>");
                    }
                    sb.Append("</tr></thead><tbody>");

                    // Read data rows
                    while (reader.Read() && rowCount < maxRowsToDisplay)
                    {
                        rowCount++;
                        sb.Append("<tr>");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            sb.Append("<td>");
                            if (reader.IsDBNull(i))
                            {
                                sb.Append("<i>NULL</i>");
                            }
                            else
                            {
                                object value = reader.GetValue(i);
                                if (value is byte[])
                                {
                                    byte[] bytes = (byte[])value;
                                    if (bytes.Length > 30)
                                    {
                                        sb.Append($"<i>BLOB ({bytes.Length} bytes)</i>");
                                    }
                                    else
                                    {
                                        MySqlConnector.QueryExpress.ConvertByteArrayToHexString(sb, value);
                                    }
                                }
                                else if (value is string)
                                {
                                    string v = (string)value;
                                    if (v.Length > maxTextValueLength)
                                    {
                                        v = $"{v.Substring(0, maxTextValueLength)}...<i>(truncated)</i>";
                                    }
                                    sb.Append("<pre>");
                                    sb.Append(System.Web.HttpUtility.HtmlEncode(v));
                                    sb.Append("</pre>");
                                }
                                else
                                {
                                    sb.Append("<pre>");
                                    MySqlConnector.QueryExpress.ConvertToSqlFormat(sb, value, false, false);
                                    sb.Append("</pre>");
                                }
                            }
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }

                    if (reader.Read())
                    {
                        sb.Append("<tr><td colspan='");
                        sb.Append(reader.FieldCount);
                        sb.Append("' style='font-style: italic; background: silver;'>");
                        sb.Append($"Results limited to {maxRowsToDisplay} rows. More rows available...");
                        sb.Append("</td></tr>");
                    }
                    sb.Append("</tbody>");
                }
                else
                {
                    sb.Append("<tbody><tr><td>No rows returned.</td></tr></tbody>");
                }
                sb.Append("</table></div>");
            }

            stopwatch.Stop();
            string summary = $@"<div style='color: green; margin-bottom: 10px;'>
<strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - Query executed successfully!<br/>
• {rowCount} row(s) displayed<br/>
• Execution time: {stopwatch.ElapsedMilliseconds}ms</div>";

            phResult.Controls.Add(new LiteralControl(summary + sb.ToString() + resultInfo.ToString()));
        }

        private void ProcessExecuteQuery(SQLiteCommand cmd, StringBuilder resultInfo, System.Diagnostics.Stopwatch stopwatch)
        {
            int totalRowsAffected = cmd.ExecuteNonQuery();
            stopwatch.Stop();

            // Get session info
            Dictionary<string, string> sessionInfo = new Dictionary<string, string>();
            long lastInsertRowId = cmd.Connection.LastInsertRowId;
            if (lastInsertRowId > 0)
            {
                sessionInfo["Last Insert RowID"] = lastInsertRowId.ToString();
            }
            using (SQLiteCommand dbCmd = new SQLiteCommand("PRAGMA database_list", cmd.Connection))
            using (SQLiteDataReader dbReader = dbCmd.ExecuteReader())
            {
                if (dbReader.Read())
                {
                    string dbFile = dbReader["file"].ToString();
                    if (!string.IsNullOrEmpty(dbFile))
                    {
                        sessionInfo["Database File"] = System.IO.Path.GetFileName(dbFile);
                    }
                }
            }
            using (SQLiteCommand versionCmd = new SQLiteCommand("SELECT sqlite_version()", cmd.Connection))
            {
                var version = versionCmd.ExecuteScalar();
                if (version != null)
                {
                    sessionInfo["SQLite Version"] = version.ToString();
                }
            }

            string successStyle = resultInfo.Length > 0 ? "color: orange;" : "color: green;";
            string mainMessage = $"<div style='{successStyle}'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - Query executed successfully!<br/>" +
                                $"• {totalRowsAffected} row(s) affected<br/>" +
                                $"• Execution time: {stopwatch.ElapsedMilliseconds}ms";
            if (sessionInfo.Count > 0)
            {
                mainMessage += "<br/>• Session info: ";
                foreach (var info in sessionInfo)
                {
                    mainMessage += $"{info.Key}={info.Value} ";
                }
            }
            mainMessage += "</div>";

            phResult.Controls.Add(new LiteralControl(mainMessage + resultInfo.ToString()));
        }

        protected void btSelectExecute_Click(object sender, EventArgs e)
        {
            ProcessQuery(false, false);
        }

        protected void btExecute_Click(object sender, EventArgs e)
        {
            ProcessQuery(true, false); // Override detection, treat as execute
        }

        protected void btSelectShow_Click(object sender, EventArgs e)
        {
            ProcessQuery(true, true); // Override detection, treat as show
        }

        protected void btShowAllTables_Click(object sender, EventArgs e)
        {
            txtSql.Text = "SELECT name FROM sqlite_master WHERE type='table';";
            ProcessQuery(true, true);
        }

    }
}