using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace MySqlConnector
{
    public class MySqlTableList : IDisposable, IEnumerable<MySqlTable>
    {
        List<MySqlTable> _lst = new List<MySqlTable>();
        string _sqlShowFullTables = "";

        public string SqlShowFullTables { get { return _sqlShowFullTables; } }

        public MySqlTableList()
        { }

        public MySqlTableList(MySqlCommand cmd)
        {
            _sqlShowFullTables = "SHOW FULL TABLES WHERE Table_type = 'BASE TABLE';";
            DataTable dtTableList = QueryExpress.GetTable(cmd, _sqlShowFullTables);

            foreach (DataRow dr in dtTableList.Rows)
            {
                _lst.Add(new MySqlTable(cmd, dr[0] + ""));
            }
        }

        public MySqlTable this[int tableIndex]
        {
            get 
            {
                return _lst[tableIndex];
            }
        }

        public MySqlTable this[string tableName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == tableName)
                        return _lst[i];
                }
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

        public IEnumerator<MySqlTable> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        public void Dispose()
        {
            for (int i = 0; i < _lst.Count; i++)
            {
                _lst[i].Dispose();
                _lst[i] = null;
            }
            _lst = null;
        }

        public List<MySqlTable> GetList()
        {
            return _lst;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MySqlTable>)_lst).GetEnumerator();
        }

        public bool Contains(string name)
        {
            foreach(var t in _lst)
            {
                if (t.Name == name)
                    return true;
            }

            return false;
        }
    }
}
