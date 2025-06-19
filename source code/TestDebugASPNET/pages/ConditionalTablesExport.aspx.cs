using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class ConditionalTablesExport : System.Web.UI.Page
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
            MySqlDatabase d = new MySqlDatabase();

            using (MySqlConnection conn = config.GetNewConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    d.GetDatabaseInfo(cmd, GetTotalRowsMethod.Skip);
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($@"
<table>
<thead>
<tr>
<th style='text-align: left;'>Table Name</th>
<th>Export Rows<br />
<input type='checkbox' onchange='toggleSelectAll(this);' checked />
</th>
<th>Condition Select Statement</th>
</tr>
</thead>
<tbody id='maintb_body'>
");

            foreach (var table in d.Tables)
            {
                string encodedTableName = Server.HtmlEncode(table.Name);
                string txtSqlId = $"txtSql_{encodedTableName}";

                sb.Append($@"
<tr>
<td>{table.Name}</td>
<td style='text-align: center;'><input type='checkbox' name='cbExportTable_{encodedTableName}' onchange=""enableTable(this, '{txtSqlId}')"" checked /></td>
<td><input id='{txtSqlId}' type='text' name='{txtSqlId}' value='select * from `{Server.HtmlEncode(QueryExpress.EscapeIdentifier(table.Name))}`'></td>
</tr>
");
            }

            sb.Append("</tbody></table>");

            ph1.Controls.Add(new LiteralControl(sb.ToString()));
        }

        protected void btFetchTables_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected async void btExport_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dicTableSelctSql = new Dictionary<string, string>();

            foreach (var key in Request.Form.AllKeys)
            {
                if (key.StartsWith("cbExportTable_"))
                {
                    string tableName = key.Substring(14);

                    string selectSql = Request.Form[$"txtSql_{tableName}"];

                    if (!string.IsNullOrEmpty(selectSql))
                    {
                        dicTableSelctSql[tableName] = selectSql;
                    }
                    else
                    {
                        dicTableSelctSql[tableName] = $"select * from `{QueryExpress.EscapeIdentifier(tableName)}`";
                    }
                }
            }

            MySqlConnector.ExportInformations exportInfo = new ExportInformations();
            exportInfo.TablesToBeExportedDic = dicTableSelctSql;

            // this task async
            ServiceBackup backup = new ServiceBackup();
            await backup.StartAsync(exportInfo);

            // redirect to this page to see the progress report, while waiting for the task to finish
            Header.Controls.Add(new LiteralControl("<script>window.location = '/ReportProgress';</script>"));
        }
    }
}