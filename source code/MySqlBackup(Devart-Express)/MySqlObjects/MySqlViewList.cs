using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Devart.Data.MySql
{
    public class MySqlViewList : IEnumerable<MySqlView>
    {
        string _sqlShowViewList = string.Empty;
        Dictionary<string, MySqlView> _lst = new Dictionary<string, MySqlView>();

        bool _allowAccess = true;
        public bool AllowAccess { get { return _allowAccess; } }

        public string SqlShowViewList { get { return _sqlShowViewList; } }

        public MySqlViewList()
        { }

        public MySqlViewList(MySqlCommand cmd)
        {
            try
            {
                string dbname = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
                _sqlShowViewList = string.Format("SHOW FULL TABLES FROM `{0}` WHERE Table_type = 'VIEW';", QueryExpress.EscapeIdentifier(dbname));
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowViewList);

                foreach (DataRow dr in dt.Rows)
                {
                    var nome = dr[0].ToString();
                    _lst.Add(nome, new MySqlView(cmd, nome));
                }
            }
            catch (MySqlException myEx)
            {
                if (myEx.Message.ToLower().Contains("access denied"))
                    _allowAccess = false;
            }
            catch
            {
                throw;
            }
        }

        public MySqlView this[string viewName]
        {
            get
            {
                if (_lst.ContainsKey(viewName))
                    return _lst[viewName];

                throw new Exception("View \"" + viewName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string viewName)
        {
            return _lst.ContainsKey(viewName);
        }

        public IEnumerator<MySqlView> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}