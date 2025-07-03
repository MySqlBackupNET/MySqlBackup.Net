using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Devart.Data.MySql
{
    public class MySqlColumnList : IEnumerable<MySqlColumn>
    {
        string _tableName;
        string _sqlShowFullColumns = string.Empty;
        Dictionary<string, MySqlColumn> _lst = new Dictionary<string, MySqlColumn>();

        public string SqlShowFullColumns { get { return _sqlShowFullColumns; } }

        public MySqlColumnList()
        { }

        public MySqlColumnList(MySqlCommand cmd, string tableName)
        {
            _tableName = tableName;
            DataTable dtDataType = QueryExpress.GetTable(cmd, string.Format("SELECT * FROM  `{0}` where 1 = 2;", QueryExpress.EscapeIdentifier(tableName)));
            
            _sqlShowFullColumns = string.Format("SHOW FULL COLUMNS FROM `{0}`;", QueryExpress.EscapeIdentifier(tableName));
            DataTable dtColInfo = QueryExpress.GetTable(cmd, _sqlShowFullColumns);

            for (int i = 0; i < dtDataType.Columns.Count; i++)
            {
                string isNullStr = (dtColInfo.Rows[i]["Null"] + "").ToLower();
                bool isNull = false;
                if (isNullStr == "yes")
                    isNull = true;

                var name = dtDataType.Columns[i].ColumnName;
                _lst.Add(
                    name, 
                    new MySqlColumn(
                        name, 
                        dtDataType.Columns[i].DataType,
                        dtColInfo.Rows[i]["Type"] + "", 
                        dtColInfo.Rows[i]["Collation"] + "",
                        isNull, 
                        dtColInfo.Rows[i]["Key"] + "",
                        dtColInfo.Rows[i]["Default"] + "", 
                        dtColInfo.Rows[i]["Extra"] + "",
                        dtColInfo.Rows[i]["Privileges"] + "", 
                        dtColInfo.Rows[i]["Comment"] + "")
                    );
            }
        }

        public MySqlColumn this[string columnName]
        {
            get
            {
                if (_lst.ContainsKey(columnName))
                    return _lst[columnName];

                throw new Exception("Column \"" + columnName + "\" is not existed in table \"" + _tableName + "\".");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string columnName)
        {
            return _lst.ContainsKey(columnName);
        }

        public IEnumerator<MySqlColumn> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}
