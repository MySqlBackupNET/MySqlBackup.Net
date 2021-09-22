using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySqlConnector;

namespace MySqlBackupTestApp
{
    public class HtmlExpress
    {
        public static string ConvertDataTableToHtmlTable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table>");

            sb.AppendLine("<tr>");
            sb.AppendFormat("\t");
            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<td>");
                sb.AppendFormat(EscapeForHtml(dc.ColumnName));
                sb.AppendFormat("</td>");
            }
            sb.AppendLine();
            sb.AppendLine("</tr>");

            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("<tr>");
                foreach (DataColumn dc in dt.Columns)
                {
                    sb.AppendFormat("<td>");

                    string dataStr = QueryExpress.ConvertToSqlFormat(dr[dc.ColumnName], false, false, null, BlobDataExportMode.HexString);

                    sb.AppendFormat(EscapeForHtml(dataStr));
                    sb.AppendFormat("</td>");
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        public static string EscapeForHtml(string input)
        {
            input = input.Replace("\r\n", "^||||^").Replace("\n", "^||||^").Replace("\r", "^||||^");
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            foreach (char c in input)
            {
                switch (c)
                {
                    case '&':
                        sb2.AppendFormat("&amp;");
                        break;
                    case '"':
                        sb2.AppendFormat("&quot;");
                        break;
                    case '\'':
                        sb2.AppendFormat("&#39;");
                        break;
                    case '<':
                        sb2.AppendFormat("&lt;");
                        break;
                    case ' ':
                        sb2.AppendFormat("&nbsp;");
                        break;
                    case '\t':
                        sb2.AppendFormat("&nbsp;&nbsp;&nbsp;&nbsp;");
                        break;
                    case '>':
                        sb2.AppendFormat("&gt;");
                        break;
                    default:
                        sb2.Append(c);
                        break;
                }
            }
            return sb2.ToString().Replace("^||||^", "<br />");
        }
    }
}
