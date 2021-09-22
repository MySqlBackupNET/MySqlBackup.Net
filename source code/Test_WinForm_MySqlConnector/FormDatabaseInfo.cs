using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySqlConnector;

namespace MySqlBackupTestApp
{
    public partial class FormDatabaseInfo : Form
    {
        StringBuilder sb;
        MySqlServer myServer;
        MySqlDatabase myDatabase;
        MySqlCommand cmd;
        Timer timer1;
        BackgroundWorker bw;

        public FormDatabaseInfo()
        {
            timer1 = new Timer();
            timer1.Interval = 100;
            timer1.Tick += timer1_Tick;
            bw = new BackgroundWorker();
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.DoWork += bw_DoWork;
            InitializeComponent();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            webBrowser1.DocumentText = sb.ToString();
        }

        void Start()
        {
            sb = new StringBuilder();
            sb.AppendLine("<html><head><style>h1 { line-height:160%; font-size: 20pt; } h2 { line-height:160%; font-size: 14pt; } body { font-family: \"Segoe UI\", Arial; line-height: 150%; } table { border: 1px solid #5C5C5C; border-collapse: collapse; } td { font-size: 10pt; padding: 4px; border: 1px solid #5C5C5C; } .code { font-family: \"Courier New\"; font-size: 10pt; line-height:110%; } </style></head>");
            sb.AppendLine("<body>");

            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                try
                {
                    conn.Open();


                    cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    myDatabase = new MySqlDatabase();
                    myDatabase.GetDatabaseInfo(cmd,  GetTotalRowsMethod.InformationSchema);
                    myServer = new MySqlServer();
                    myServer.GetServerInfo(cmd);

                    int stage = 1;

                    while (stage < 13)
                    {
                        try
                        {
                            switch (stage)
                            {
                                case 1: LoadDatabase(); break;
                                case 2: LoadUser(); break;
                                case 3: LoadGlobalPrivilege(); break;
                                case 4: LoadViewPrivilege(); break;
                                case 5: LoadFunctionPrivilege(); break;
                                case 6: LoadVariables(); break;
                                case 7: LoadTables(); break;
                                case 8: LoadFunctions(); break;
                                case 9: LoadProcedures(); break;
                                case 10: LoadTriggers(); break;
                                case 11: LoadViews(); break;
                                case 12: LoadEvents(); break;
                                default: break;
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteError(ex.Message);
                        }

                        stage += 1;
                    }

                    conn.Close();

                }
                catch (Exception exCon)
                {
                    WriteError(exCon.Message);
                }
            }

            sb.Append("</body>");
            sb.Append("</html>");
        }

        void LoadDatabase()
        {
            WriteHead1("Database");
            WriteCodeBlock(myDatabase.CreateDatabaseSQL);
        }

        void LoadUser()
        {
            WriteHead1("User");

            string sqlSelectCurrentUser = "SELECT current_user;";
            WriteCodeBlock(sqlSelectCurrentUser);
            WriteCodeBlock(myServer.CurrentUserClientHost);
        }

        void LoadGlobalPrivilege()
        {
            WriteHead2("Global Privileges");

            string curUser = "";
            if (myServer.CurrentUser != "root")
                curUser = myServer.CurrentUser;
            else
                WriteText("Current user is \"root\". All privileges are granted by default.");

            string sqlShowUserPrivilege = "SELECT * FROM mysql.db WHERE `user` = '" + curUser + "';";

            DataTable dt = QueryExpress.GetTable(cmd, sqlShowUserPrivilege);


            WriteCodeBlock(sqlShowUserPrivilege);
            WriteTable(dt);
        }

        void LoadViewPrivilege()
        {
            WriteHead2("Privileges of View");

            string sqlViewPrivilege =
@"SELECT  mv.host `Host`,  mv.user `User`,
CONCAT(mv.Db, '.', mv.Table_name) `Views`,
REPLACE(mv.Table_priv, ',', ', ') AS `Privileges`
FROM  mysql.tables_priv mv
WHERE mv.Db = '" + myDatabase.Name + @"' 
and mv.Table_name IN  
(SELECT  DISTINCT v.table_name `views` FROM information_schema.views AS v) 
ORDER BY  mv.Host,  mv.User,  mv.Db,  mv.Table_name;";

            DataTable dtViewPrivilege = QueryExpress.GetTable(cmd, sqlViewPrivilege);

            WriteCodeBlock(sqlViewPrivilege);
            WriteTable(dtViewPrivilege);
        }

        void LoadProcedurePrivilege()
        {
            WriteHead2("Privileges of Procedure");

            string sqlProcedurePrivilege =
@"SELECT  mp.host `Host`,  mp.user `User`,
CONCAT(mp.Db, '.', mp.Routine_name) `Procedures`,
REPLACE(mp.Proc_priv, ',', ', ') AS `Privileges`
FROM  mysql.procs_priv mp
WHERE mp.Db = '" + myDatabase.Name + @"' 
and mp.Routine_type = 'PROCEDURE' 
ORDER BY  mp.Host,  mp.User,  mp.Db,  mp.Routine_name;";

            DataTable dt = QueryExpress.GetTable(cmd, sqlProcedurePrivilege);

            WriteCodeBlock(sqlProcedurePrivilege);
            WriteTable(dt);
        }

        void LoadFunctionPrivilege()
        {
            WriteHead2("Privileges of Function");

            string sqlPrivilegeFunction =
@"SELECT  mf.host `Host`,  mf.user `User`,
CONCAT(mf.Db, '.', mf.Routine_name) `Procedures`,
REPLACE(mf.Proc_priv, ',', ', ') AS `Privileges`
FROM  mysql.procs_priv mf WHERE mf.Db = '" + myDatabase.Name + @"'
and mf.Routine_type = 'FUNCTION' 
ORDER BY  mf.Host,  mf.User,  mf.Db,  mf.Routine_name;";

            DataTable dtPrivilegeFunction = QueryExpress.GetTable(cmd, sqlPrivilegeFunction);

            WriteCodeBlock(sqlPrivilegeFunction);
            WriteTable(dtPrivilegeFunction);
        }

        void LoadVariables()
        {
            WriteHead1("System Variables");

            string sqlShowVariables = "SHOW variables;";

            DataTable dtVariables = QueryExpress.GetTable(cmd, sqlShowVariables);

            WriteCodeBlock(sqlShowVariables);
            WriteTable(dtVariables);
        }

        void LoadTables()
        {
            WriteHead1("Tables");

            WriteText("Note: Value of \"Rows\" shown below is not accurate. It is a cache value, it is not up to date. For accurate total rows count, please see the following next table.");

            string sqlShowTableStatus = "SHOW TABLE STATUS;";

            DataTable dtTableStatus = QueryExpress.GetTable(cmd, sqlShowTableStatus);

            WriteCodeBlock(sqlShowTableStatus);
            WriteTable(dtTableStatus);

            WriteHead2("Actual Total Rows For Each Table");

            DataTable dtTotalRows = new DataTable();
            dtTotalRows.Columns.Add("Table");
            dtTotalRows.Columns.Add("Total Rows");

            foreach (MySqlTable table in myDatabase.Tables)
            {
                dtTotalRows.Rows.Add(table.Name, table.TotalRows);
            }

            WriteTable(dtTotalRows);

            foreach (MySqlTable table in myDatabase.Tables)
            {
                WriteHead2(table.Name);
                WriteCodeBlock(table.Columns.SqlShowFullColumns);
                DataTable dtColumns = QueryExpress.GetTable(cmd, table.Columns.SqlShowFullColumns);
                WriteTable(dtColumns);

                WriteText("Data Type in .NET Framework");

                DataTable dtDataType = new DataTable();
                dtDataType.Columns.Add("Column Name");
                dtDataType.Columns.Add("MySQL Data Type");
                dtDataType.Columns.Add(".NET Data Type");

                foreach (MySqlColumn myCol in table.Columns)
                {
                    dtDataType.Rows.Add(myCol.Name, myCol.MySqlDataType, myCol.DataType.ToString());
                }

                WriteTable(dtDataType);

                WriteCodeBlock("SHOW CREATE TABLE `" + table.Name + "`;");
                WriteCodeBlock(table.CreateTableSqlWithoutAutoIncrement);
            }
        }

        void LoadFunctions()
        {
            WriteHead1("Functions");

            WriteCodeBlock(myDatabase.Functions.SqlShowFunctions);
            DataTable dtFunctionList = QueryExpress.GetTable(cmd, myDatabase.Functions.SqlShowFunctions);
            WriteTable(dtFunctionList);

            WriteCodeBlock("SHOW CREATE FUNCTION `<name>`;");

            if (!myDatabase.Functions.AllowAccess)
                WriteAccessDeniedErrMsg();

            foreach (MySqlFunction func in myDatabase.Functions)
            {
                WriteHead2(func.Name);
                WriteCodeBlock(func.CreateFunctionSQLWithoutDefiner);
            }
        }

        void LoadProcedures()
        {
            WriteHead1("Procedures");

            WriteCodeBlock(myDatabase.Procedures.SqlShowProcedures);
            DataTable dtProcedureList = QueryExpress.GetTable(cmd, myDatabase.Procedures.SqlShowProcedures);
            WriteTable(dtProcedureList);

            WriteCodeBlock("SHOW CREATE PROCEDURE `<name>`;");

            if (!myDatabase.Procedures.AllowAccess)
                WriteAccessDeniedErrMsg();

            foreach (MySqlProcedure proc in myDatabase.Procedures)
            {
                WriteHead2(proc.Name);
                WriteCodeBlock(proc.CreateProcedureSQLWithoutDefiner);
            }
        }

        void LoadTriggers()
        {
            WriteHead1("Triggers");

            WriteCodeBlock(myDatabase.Triggers.SqlShowTriggers);
            DataTable dtTriggerList = QueryExpress.GetTable(cmd, myDatabase.Triggers.SqlShowTriggers);
            WriteTable(dtTriggerList);

            WriteCodeBlock("SHOW CREATE TRIGGER `<name>`;");

            if (!myDatabase.Triggers.AllowAccess)
                WriteAccessDeniedErrMsg();

            foreach (MySqlTrigger trigger in myDatabase.Triggers)
            {
                WriteHead2(trigger.Name);
                WriteCodeBlock(trigger.CreateTriggerSQL);
            }
        }

        void LoadViews()
        {
            WriteHead1("Views");

            WriteCodeBlock(myDatabase.Views.SqlShowViewList);
            DataTable dtViewList = QueryExpress.GetTable(cmd, myDatabase.Views.SqlShowViewList);
            WriteTable(dtViewList);

            WriteCodeBlock("SHOW CREATE VIEW `<name>`;");

            if (!myDatabase.Views.AllowAccess)
                WriteAccessDeniedErrMsg();

            foreach (MySqlView myview in myDatabase.Views)
            {
                WriteHead2(myview.Name);
                WriteCodeBlock(myview.CreateViewSQL);
            }
        }

        void LoadEvents()
        {
            WriteHead1("Events");

            WriteCodeBlock(myDatabase.Events.SqlShowEvent);
            DataTable dtEventList = QueryExpress.GetTable(cmd, myDatabase.Events.SqlShowEvent);
            WriteTable(dtEventList);

            WriteCodeBlock("SHOW CREATE EVENT `<name>`;");

            if (!myDatabase.Events.AllowAccess)
                WriteAccessDeniedErrMsg();

            foreach (MySqlEvent myevent in myDatabase.Events)
            {
                WriteHead2(myevent.Name);
                WriteCodeBlock(myevent.CreateEventSql);
            }
        }

        void WriteHead1(string text)
        {
            sb.Append("<h1>");
            sb.Append(GetHtmlString(text.Trim()));
            sb.AppendLine("</h1>");
            sb.AppendLine("<hr />");
        }

        void WriteHead2(string text)
        {
            sb.Append("<h2>");
            sb.Append(GetHtmlString(text.Trim()));
            sb.AppendLine("</h2>");
        }

        void WriteText(string text)
        {
            sb.AppendLine("<p>");
            sb.AppendLine(GetHtmlString(text.Trim()));
            sb.AppendLine("</p>");
        }

        void WriteCodeBlock(string text)
        {
            sb.AppendLine("<span class=\"code\">");
            sb.AppendLine(GetHtmlString(text.Trim()));
            sb.AppendLine("</span>");
            sb.AppendLine("<br /><br />");
        }

        void WriteTable(DataTable dt)
        {
            sb.AppendFormat(HtmlExpress.ConvertDataTableToHtmlTable(dt));
            sb.AppendLine("<br />");
        }

        void WriteAccessDeniedErrMsg()
        {
            WriteError("Access denied for user " + myServer.CurrentUserClientHost);
        }

        void WriteError(string errMsg)
        {
            sb.AppendLine("<br />");
            sb.AppendLine("<div style=\"background-color: #FFE8E8; padding: 5px; border: 1px solid #FF0000;\">");
            sb.AppendLine("Error or Exception occured. Error message:<br />");
            sb.AppendLine(GetHtmlString(errMsg));
            sb.AppendLine("</div>");
            sb.AppendLine("<br />");
        }

        string GetHtmlString(string input)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            webBrowser1.DocumentText = "<h1>Database info is loading...<br />Please wait...</h1>";
            bw.RunWorkerAsync();
        }

        private void FormDatabaseInfo_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "HTML|*.html";
            sf.FileName = myDatabase.Name + ".html";
            if (DialogResult.OK == sf.ShowDialog())
            {
                System.IO.File.WriteAllText(sf.FileName, webBrowser1.DocumentText);
            }
        }

        private void btRefresh_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void btPrint_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
        }
    }
}