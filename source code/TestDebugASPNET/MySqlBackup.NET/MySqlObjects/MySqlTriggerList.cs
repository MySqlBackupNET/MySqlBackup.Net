using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MySqlConnector
{
    public class MySqlTriggerList : IEnumerable<MySqlTrigger>
    {
        string _sqlShowTriggers = string.Empty;
        Dictionary<string, MySqlTrigger> _lst = new Dictionary<string, MySqlTrigger>();

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
                    var name = dr["Trigger"].ToString();
                    _lst.Add(name, new MySqlTrigger(cmd, name, dr["Definer"].ToString()));
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

        public MySqlTrigger this[string triggerName]
        {
            get
            {
                if (_lst.ContainsKey(triggerName))
                    return _lst[triggerName];

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
            return _lst.ContainsKey(triggerName);
        }

        public IEnumerator<MySqlTrigger> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}
