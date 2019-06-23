using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Devart.Data.MySql
{
    public class MySqlTable : IDisposable
    {
        string _name = "";
        MySqlColumnList _lst = null;
        long _totalRows = 0;
        string _createTableSql = "";
        string _createTableSqlWithoutAutoIncrement = "";
        string _insertStatementHeader = "";
        string _insertStatementHeaderWithoutColumns = "";

        public string Name { get { return _name; } }
        public long TotalRows { get { return _totalRows; } }
        public string CreateTableSql { get { return _createTableSql; } }
        public string CreateTableSqlWithoutAutoIncrement { get { return _createTableSqlWithoutAutoIncrement; } }
        public MySqlColumnList Columns { get { return _lst; } }
        public string InsertStatementHeaderWithoutColumns { get { return _insertStatementHeaderWithoutColumns; } }
        public string InsertStatementHeader { get { return _insertStatementHeader; } }

        public MySqlTable(MySqlCommand cmd, string name)
        {
            _name = name;
            string sql = string.Format("SHOW CREATE TABLE `{0}`;", name);
            _createTableSql = QueryExpress.ExecuteScalarStr(cmd, sql, 1).Replace(Environment.NewLine, "^~~~~~~^").Replace("\r", "^~~~~~~^").Replace("\n", "^~~~~~~^").Replace("^~~~~~~^", Environment.NewLine).Replace("CREATE TABLE ", "CREATE TABLE IF NOT EXISTS ") + ";";
            _createTableSqlWithoutAutoIncrement = RemoveAutoIncrement(_createTableSql);
            _lst = new MySqlColumnList(cmd, name);
            GetInsertStatementHeaders();
        }

        void GetInsertStatementHeaders()
        {
            _insertStatementHeaderWithoutColumns = string.Format("INSERT INTO `{0}` VALUES", _name);

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO `");
            sb.Append(_name);
            sb.Append("` (");
            for (int i = 0; i < _lst.Count; i++)
            {
                if (i > 0)
                    sb.Append(",");

                sb.Append("`");
                sb.Append(_lst[i].Name);
                sb.Append("`");
            }
            sb.Append(") VALUES");

            _insertStatementHeader = sb.ToString();
        }

        public void GetTotalRowsByCounting(MySqlCommand cmd)
        {
            string sql = string.Format("SELECT COUNT(*) FROM `{0}`;", _name);
            _totalRows = QueryExpress.ExecuteScalarLong(cmd, sql);
        }

        public void SetTotalRows(long _trows)
        {
            _totalRows = _trows;
        }

        string RemoveAutoIncrement(string sql)
        {
            string a = "AUTO_INCREMENT=";

            if (sql.Contains(a))
            {
                int i = sql.LastIndexOf(a);

                int b = i + a.Length;

                string d = "";

                int count = 0;

                while (char.IsDigit(sql[b + count]))
                {
                    char cc = sql[b + count];

                    d = d + cc;

                    count = count + 1;
                }

                sql = sql.Replace(a + d, string.Empty);
            }

            return sql;
        }

        public void Dispose()
        {
            _lst.Dispose();
            _lst = null;
        }
    }
}
