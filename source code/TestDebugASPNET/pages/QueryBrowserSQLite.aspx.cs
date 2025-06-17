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

        protected void btExecute_Click(object sender, EventArgs e)
        {
            try
            {
                int totalRowsAffected = 0;
                StringBuilder resultInfo = new StringBuilder();

                using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = txtSql.Text;

                        // Capture execution time
                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                        totalRowsAffected = cmd.ExecuteNonQuery();
                        stopwatch.Stop();

                        // SQLite doesn't have SHOW WARNINGS like MySQL
                        // Instead, we can check for any pragma or other SQLite-specific info
                        // Most SQLite operations don't generate warnings in the same way MySQL does

                        // Get additional session info
                        Dictionary<string, string> sessionInfo = new Dictionary<string, string>();

                        // Get last insert rowid if applicable (SQLite equivalent of last insert ID)
                        long lastInsertRowId = conn.LastInsertRowId;
                        if (lastInsertRowId > 0)
                        {
                            sessionInfo["Last Insert RowID"] = lastInsertRowId.ToString();
                        }

                        // Get current database file path
                        using (SQLiteCommand dbCmd = new SQLiteCommand("PRAGMA database_list", conn))
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

                        // Get SQLite version
                        using (SQLiteCommand versionCmd = new SQLiteCommand("SELECT sqlite_version()", conn))
                        {
                            var version = versionCmd.ExecuteScalar();
                            if (version != null)
                            {
                                sessionInfo["SQLite Version"] = version.ToString();
                            }
                        }

                        // Build final result message
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

                    conn.Close();
                }
            }
            catch (SQLiteException sqliteEx)
            {
                // SQLite specific error handling
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

        protected void btSelectShow_Click(object sender, EventArgs e)
        {
            int.TryParse(txtMaxRows.Text, out int maxRowsToDisplay);

            if (maxRowsToDisplay == 0)
            {
                maxRowsToDisplay = 100;
                txtMaxRows.Text = "100";
            }

            int.TryParse(txtMaxTextValueLength.Text, out int maxTextValueLength);

            if (maxTextValueLength == 0)
            {
                maxTextValueLength = 500;
                txtMaxTextValueLength.Text = "500";
            }

            try
            {
                StringBuilder sb = new StringBuilder();
                int rowCount = 0;

                using (SQLiteConnection conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = txtSql.Text;

                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            sb.Append("<div class='div-table-result'><table>");

                            if (reader.HasRows)
                            {
                                // Build header with column metadata
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

                                sb.Append("</tr></thead>");
                                sb.Append("<tbody>");

                                // Read data rows with limit check
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

                                            // Special handling for different types
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
                                                    v = $"{(v.Substring(0, maxTextValueLength))}...<i>(truncated)</i>";
                                                }

                                                sb.Append("<pre>");
                                                sb.Append(System.Web.HttpUtility.HtmlEncode(v));
                                                sb.Append("</pre>");
                                            }
                                            else
                                            {
                                                // Use QueryExpress for normal formatting
                                                sb.Append("<pre>");
                                                MySqlConnector.QueryExpress.ConvertToSqlFormat(sb, value, false, false);
                                                sb.Append("</pre>");
                                            }
                                        }

                                        sb.Append("</td>");
                                    }

                                    sb.Append("</tr>");
                                }

                                // Check if we hit the limit
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

                        // Build summary
                        string summary = $@"<div style='color: green; margin-bottom: 10px;'>
<strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - Query executed successfully!<br/>
• {rowCount} row(s) displayed<br/>
• Execution time: {stopwatch.ElapsedMilliseconds}ms</div>";

                        phResult.Controls.Add(new LiteralControl(summary + sb.ToString()));
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

        protected void btShowAllTables_Click(object sender, EventArgs e)
        {
            txtSql.Text = "SELECT name FROM sqlite_master WHERE type='table';";
            btSelectShow_Click(null, null);
        }
    }
}