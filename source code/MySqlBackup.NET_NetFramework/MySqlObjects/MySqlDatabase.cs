using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlDatabase : IDisposable
    {
        string _name = "";
        string _createDatabaseSql = "";
        string _dropDatabaseSql = "";
        string _defaultCharSet = "";

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
                long t = 0;

                for (int i = 0; i < _listTable.Count; i++)
                {
                    t = t + _listTable[i].TotalRows;
                }

                return t;
            }
        }

        public MySqlDatabase()
        { }

        public void GetDatabaseInfo(MySqlCommand cmd, bool getTotalRowsForEachTable)
        {
            _name = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
            _defaultCharSet = QueryExpress.ExecuteScalarStr(cmd, "SHOW VARIABLES LIKE 'character_set_database';", 1);
            _createDatabaseSql = QueryExpress.ExecuteScalarStr(cmd, string.Format("SHOW CREATE DATABASE `{0}`;", _name), 1).Replace("CREATE DATABASE", "CREATE DATABASE IF NOT EXISTS") + ";";
            _dropDatabaseSql = string.Format("DROP DATABASE IF EXISTS `{0}`;", _name);

            _listTable = new MySqlTableList(cmd);
            _listProcedure = new MySqlProcedureList(cmd);
            _listFunction = new MySqlFunctionList(cmd);
            _listTrigger = new MySqlTriggerList(cmd);
            _listEvent = new MySqlEventList(cmd);
            _listView = new MySqlViewList(cmd);

            if (getTotalRowsForEachTable)
                GetTotalRows(cmd);
        }

        public void GetTotalRows(MySqlCommand cmd)
        {
            DataTable dtTotalRows = QueryExpress.GetTable(cmd, string.Format("SELECT TABLE_NAME, TABLE_ROWS FROM `information_schema`.`tables` WHERE `table_schema` = '{0}';", _name));

            int _tableCountTotalRow = 0;

            foreach(DataRow dr in dtTotalRows.Rows)
            {
                string _thisTableName = dr["TABLE_NAME"] + "";
                
                long _totalRowsThisTable = 0L;

                try
                {
                    long.TryParse(dr["TABLE_ROWS"] + "", out _totalRowsThisTable);
                }
                catch { }

                for (int i = 0; i < _listTable.Count; i++)
                {
                    if (_listTable[i].Name != _thisTableName)
                        continue;

                    _tableCountTotalRow = _tableCountTotalRow + 1;

                    _listTable[i].SetTotalRows(_totalRowsThisTable);

                    if (GetTotalRowsProgressChanged != null)
                    {
                        GetTotalRowsProgressChanged(this, new GetTotalRowsArgs(_listTable.Count, _tableCountTotalRow));
                    }

                    break;
                }
            }

            
            //for (int i = 0; i < _listTable.Count; i++)
            //{
            //    _listTable[i].GetTotalRows(cmd);

            //    if (GetTotalRowsProgressChanged != null)
            //    {
            //        GetTotalRowsProgressChanged(this, new GetTotalRowsArgs(_listTable.Count, i + 1));
            //    }
            //}
        }

        public void Dispose()
        {
            _listTable.Dispose();
            _listProcedure.Dispose();
            _listFunction.Dispose();
            _listEvent.Dispose();
            _listTrigger.Dispose();
            _listView.Dispose();
        }
    }
}
