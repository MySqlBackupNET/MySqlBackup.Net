using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MySqlConnector
{
    public class MySqlTableList : IEnumerable<MySqlTable>
    {
        string _sqlShowFullTables = string.Empty;
        Dictionary<string, MySqlTable> _lst = new Dictionary<string, MySqlTable>();

        public string SqlShowFullTables { get { return _sqlShowFullTables; } }

        public MySqlTableList()
        { }

        public MySqlTableList(MySqlCommand cmd)
        {
            _sqlShowFullTables = "SHOW FULL TABLES WHERE Table_type = 'BASE TABLE';";
            DataTable dtTableList = QueryExpress.GetTable(cmd, _sqlShowFullTables);

            foreach (DataRow dr in dtTableList.Rows)
            {
                var table = new MySqlTable(cmd, dr[0] + "");
                _lst.Add(table.Name, table);
            }
        }

        public MySqlTable this[string tableName]
        {
            get
            {
                if (_lst.ContainsKey(tableName))
                    return _lst[tableName];

                throw new Exception("Table \"" + tableName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public IEnumerator<MySqlTable> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            _lst.Values.GetEnumerator();

        public bool Contains(string name)
        {
            return _lst.ContainsKey(name);
        }
    }
}
