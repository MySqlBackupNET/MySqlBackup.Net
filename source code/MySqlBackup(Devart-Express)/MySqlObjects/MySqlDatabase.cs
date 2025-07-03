using System;
using System.Data;
using System.Linq;
using System.Timers;

namespace Devart.Data.MySql
{
    public class MySqlDatabase
    {
        string _name = string.Empty;
        string _createDatabaseSql = string.Empty;
        string _dropDatabaseSql = string.Empty;
        string _defaultCharSet = string.Empty;

        MySqlTableList _listTable = new MySqlTableList();
        MySqlProcedureList _listProcedure = new MySqlProcedureList();
        MySqlFunctionList _listFunction = new MySqlFunctionList();
        MySqlEventList _listEvent = new MySqlEventList();
        MySqlViewList _listView = new MySqlViewList();
        MySqlTriggerList _listTrigger = new MySqlTriggerList();

        public string Name { get { return _name; } }
        public string DefaultCharacterSet { get { return _defaultCharSet; } }
        public string CreateDatabaseSQL { get { return _createDatabaseSql; } }
        public string DropDatabaseSQL { get { return _dropDatabaseSql; } }

        public MySqlTableList Tables { get { return _listTable; } }
        public MySqlProcedureList Procedures { get { return _listProcedure; } }
        public MySqlEventList Events { get { return _listEvent; } }
        public MySqlViewList Views { get { return _listView; } }
        public MySqlFunctionList Functions { get { return _listFunction; } }
        public MySqlTriggerList Triggers { get { return _listTrigger; } }

        public delegate void getTotalRowsProgressChange(object sender, GetTotalRowsArgs e);
        public event getTotalRowsProgressChange GetTotalRowsProgressChanged;

        public long TotalRows
        {
            get
            {
                return _listTable.ToList().Sum(x => x.TotalRows);
            }
        }

        public MySqlDatabase()
        { }

        public void GetDatabaseInfo(MySqlCommand cmd, GetTotalRowsMethod enumGetTotalRowsMode)
        {
            _name = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
            _defaultCharSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'character_set_database';", 1);
            _createDatabaseSql = QueryExpress.ExecuteScalarStr(cmd, string.Format("SHOW CREATE DATABASE `{0}`;", QueryExpress.EscapeIdentifier(_name)), 1).Replace("CREATE DATABASE", "CREATE DATABASE IF NOT EXISTS") + ";";
            _dropDatabaseSql = string.Format("DROP DATABASE IF EXISTS `{0}`;", _name);

            _listTable = new MySqlTableList(cmd);
            _listProcedure = new MySqlProcedureList(cmd);
            _listFunction = new MySqlFunctionList(cmd);
            _listTrigger = new MySqlTriggerList(cmd);
            _listEvent = new MySqlEventList(cmd);
            _listView = new MySqlViewList(cmd);

            if (enumGetTotalRowsMode != GetTotalRowsMethod.Skip)
                GetTotalRows(cmd, enumGetTotalRowsMode);
        }

        public void GetTotalRows(MySqlCommand cmd, GetTotalRowsMethod enumGetTotalRowsMode)
        {
            int i = 0;
            var timer = new Timer
            {
                Interval = 10000
            };
            timer.Elapsed += (sender, e) =>
            {
                GetTotalRowsProgressChanged?.Invoke(this, new GetTotalRowsArgs(_listTable.Count, i));
            };

            if (enumGetTotalRowsMode == GetTotalRowsMethod.InformationSchema)
            {
                DataTable dtTotalRows = QueryExpress.GetTable(cmd, string.Format("SELECT TABLE_NAME, TABLE_ROWS FROM `information_schema`.`tables` WHERE `table_schema` = '{0}';", QueryExpress.EscapeIdentifier(_name)));
                timer.Start();
                foreach (DataRow dr in dtTotalRows.Rows)
                {
                    i++;
                    var _tbname = dr["TABLE_NAME"] + "";
                    long.TryParse(dr["TABLE_ROWS"] + "", out var _totalRowsThisTable);

                    if (_listTable.Contains(_tbname))
                        _listTable[_tbname].SetTotalRows((long)(_totalRowsThisTable * 1.1)); // Adiciona 10% de erro
                }
                timer.Stop();
                GetTotalRowsProgressChanged?.Invoke(this, new GetTotalRowsArgs(_listTable.Count, _listTable.Count));
            }
            else if (enumGetTotalRowsMode == GetTotalRowsMethod.SelectCount)
            {
                timer.Start();
                foreach (var table in _listTable)
                {
                    i++;
                    table.GetTotalRowsByCounting(cmd);
                }
                timer.Stop();
                GetTotalRowsProgressChanged?.Invoke(this, new GetTotalRowsArgs(_listTable.Count, _listTable.Count));
            }
        }
    }
}