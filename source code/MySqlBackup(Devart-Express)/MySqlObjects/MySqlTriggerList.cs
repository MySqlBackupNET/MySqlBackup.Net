using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Devart.Data.MySql
{
    public class MySqlTriggerList : IDisposable
    {
        List<MySqlTrigger> _lst = new List<MySqlTrigger>();
        string _sqlShowTriggers = "";

        bool _allowAccess = true;
        public bool AllowAccess { get { return _allowAccess; } }

        public string SqlShowTriggers { get { return _sqlShowTriggers; } }

        public MySqlTriggerList()
        { }

        public MySqlTriggerList(MySqlCommand cmd)
        {
            _sqlShowTriggers = "SHOW TRIGGERS;";
            try
            {
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowTriggers);

                foreach (DataRow dr in dt.Rows)
                {
                    _lst.Add(new MySqlTrigger(cmd, dr["Trigger"] + "", dr["Definer"] + ""));
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

        public MySqlTrigger this[int triggerIndex]
        {
            get
            {
                return _lst[triggerIndex];
            }
        }

        public MySqlTrigger this[string triggerName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == triggerName)
                    {
                        return _lst[i];
                    }
                }
                throw new Exception("Trigger \"" + triggerName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string triggerName)
        {
            if (this[triggerName] == null)
                return false;
            return true;
        }

        public IEnumerator<MySqlTrigger> GetEnumerator()
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
    }
}
