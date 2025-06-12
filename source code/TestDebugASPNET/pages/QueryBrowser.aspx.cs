using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySqlConnector;

namespace System.pages
{
    public partial class QueryBrowser : System.Web.UI.Page
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

                using (MySqlConnection conn = config.GetNewConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = txtSql.Text;

                        // Capture execution time
                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                        totalRowsAffected = cmd.ExecuteNonQuery();
                        stopwatch.Stop();

                        // Get warnings - MySqlConnector style
                        using (MySqlCommand warningCmd = new MySqlCommand("SHOW WARNINGS", conn))
                        using (MySqlDataReader warningReader = warningCmd.ExecuteReader())
                        {
                            int warningCount = 0;
                            List<string> warnings = new List<string>();

                            while (warningReader.Read())
                            {
                                string level = warningReader.GetString(0);    // Level
                                string code = warningReader.GetString(1);     // Code  
                                string message = warningReader.GetString(2);  // Message

                                string color = level == "Warning" ? "orange" : level == "Error" ? "red" : "blue";
                                warnings.Add($"<span style='color: {color}'>{level} ({code}): {message}</span>");
                                warningCount++;
                            }

                            if (warningCount > 0)
                            {
                                resultInfo.AppendLine($"<br/><strong>{warningCount} warning(s)/note(s) found:</strong>");
                                foreach (var warning in warnings)
                                {
                                    resultInfo.AppendLine($"<br/>{warning}");
                                }
                            }
                        }

                        // Get additional session info
                        Dictionary<string, string> sessionInfo = new Dictionary<string, string>();

                        // Get last insert ID if applicable
                        if (cmd.LastInsertedId > 0)
                        {
                            sessionInfo["Last Insert ID"] = cmd.LastInsertedId.ToString();
                        }

                        // Get current database
                        using (MySqlCommand dbCmd = new MySqlCommand("SELECT DATABASE()", conn))
                        {
                            var currentDb = dbCmd.ExecuteScalar();
                            if (currentDb != null && currentDb != DBNull.Value)
                            {
                                sessionInfo["Database"] = currentDb.ToString();
                            }
                        }

                        // Get connection ID
                        using (MySqlCommand connIdCmd = new MySqlCommand("SELECT CONNECTION_ID()", conn))
                        {
                            var connId = connIdCmd.ExecuteScalar();
                            if (connId != null)
                            {
                                sessionInfo["Connection ID"] = connId.ToString();
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
            catch (MySqlException mysqlEx)
            {
                // MySqlConnector specific error handling
                string errorHtml = $"<div style='color: red;'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - MySQL Error:<br/>" +
                                  $"• Error Code: {mysqlEx.ErrorCode}<br/>";

                // MySqlConnector uses ErrorCode instead of Number
                if (mysqlEx.SqlState != null)
                {
                    errorHtml += $"• SQL State: {mysqlEx.SqlState}<br/>";
                }

                errorHtml += $"• Message: {mysqlEx.Message}</div>";

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

                using (MySqlConnection conn = config.GetNewConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = txtSql.Text;

                        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
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
                                                    QueryExpress.ConvertByteArrayToHexString(sb, value);
                                                }
                                            }
                                            else if (value is string && ((string)value).Length > maxTextValueLength)
                                            {
                                                // Truncate very long strings
                                                string strVal = (string)value;
                                                sb.Append("<pre>");
                                                sb.Append(System.Web.HttpUtility.HtmlEncode(strVal.Substring(0, maxTextValueLength)));
                                                sb.Append("...<i>(truncated)</i>");
                                                sb.Append("</pre>");
                                            }
                                            else
                                            {
                                                // Use QueryExpress for normal formatting
                                                sb.Append("<pre>");
                                                QueryExpress.ConvertToSqlValueFormat(sb, value, false, false);
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
            catch (MySqlException mysqlEx)
            {
                string errorHtml = $"<div style='color: red;'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - MySQL Error:<br/>" +
                                  $"• Error Code: {mysqlEx.ErrorCode}<br/>";

                if (mysqlEx.SqlState != null)
                {
                    errorHtml += $"• SQL State: {mysqlEx.SqlState}<br/>";
                }

                errorHtml += $"• Message: {mysqlEx.Message}</div>";

                phResult.Controls.Add(new LiteralControl(errorHtml));
            }
            catch (Exception ex)
            {
                phResult.Controls.Add(new LiteralControl($"<div style='color: red;'><strong>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</strong> - General Error: {ex.Message}</div>"));
            }
        }
    }
}