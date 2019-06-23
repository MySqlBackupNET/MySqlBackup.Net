using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace MySql.Data.MySqlClient
{
    public class MySqlViewList : IDisposable, IEnumerable<MySqlView>
    {
        List<MySqlView> _lst = new List<MySqlView>();
        string _sqlShowViewList = "";

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
                _sqlShowViewList = string.Format("SHOW FULL TABLES FROM `{0}` WHERE Table_type = 'VIEW';", dbname);
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowViewList);

                foreach (DataRow dr in dt.Rows)
                {
                    _lst.Add(new MySqlView(cmd, dr[0] + ""));
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

        public MySqlView this[int viewIndex]
        {
            get
            {
                return _lst[viewIndex];
            }
        }

        public MySqlView this[string viewName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == viewName)
                        return _lst[i];
                }
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
            if (this[viewName] == null)
                return false;
            return true;
        }

        public IEnumerator<MySqlView> GetEnumerator()
        {
            return _lst.GetEnumerator();
        }

        public void Dispose()
        {
            for (int i = 0; i < _lst.Count; i++)
            {
                _lst[i] = null;
            }
            _lst = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MySqlView>)_lst).GetEnumerator();
        }
    }
}