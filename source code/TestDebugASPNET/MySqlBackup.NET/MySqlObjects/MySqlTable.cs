using System;
using System.Text;

namespace MySqlConnector
{
    public class MySqlTable
    {
        string _name = string.Empty;
        MySqlColumnList _lst = null;
        long _totalRows = 0;
        string _createTableSql = string.Empty;
        string _createTableSqlWithoutAutoIncrement = string.Empty;
        string _insertStatementHeader = string.Empty;
        string _insertStatementHeaderWithoutColumns = string.Empty;

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
            string sql = string.Format("SHOW CREATE TABLE `{0}`;", QueryExpress.EscapeIdentifier(name));
            _createTableSql = QueryExpress.ExecuteScalarStr(cmd, sql, 1).Replace(Environment.NewLine, "^~~~~~~^").Replace("\r", "^~~~~~~^").Replace("\n", "^~~~~~~^").Replace("^~~~~~~^", Environment.NewLine).Replace("CREATE TABLE ", "CREATE TABLE IF NOT EXISTS ") + ";";
            _createTableSqlWithoutAutoIncrement = RemoveAutoIncrement(_createTableSql);
            _lst = new MySqlColumnList(cmd, name);
            GetInsertStatementHeaders();
        }

        void GetInsertStatementHeaders()
        {
            //_insertStatementHeaderWithoutColumns = string.Format("INSERT INTO `{0}` VALUES", _name);
            _insertStatementHeaderWithoutColumns = string.Format("INSERT INTO `{0}` VALUES", QueryExpress.EscapeIdentifier(_name));

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO `");
            
            //sb.Append(_name);
            sb.Append(QueryExpress.EscapeIdentifier(_name));

            sb.Append("` (");
            var i = 0;
            foreach (var column in _lst)
            {
                if (i > 0)
                    sb.Append(',');

                sb.Append('`');
                sb.Append(column.Name);
                sb.Append('`');
                i++;
            }
            sb.Append(") VALUES");

            _insertStatementHeader = sb.ToString();
        }

        public void GetTotalRowsByCounting(MySqlCommand cmd)
        {
            string sql = string.Format("SELECT COUNT(1) FROM `{0}`;", QueryExpress.EscapeIdentifier(_name));
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

                string d = string.Empty;

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
    }
}
