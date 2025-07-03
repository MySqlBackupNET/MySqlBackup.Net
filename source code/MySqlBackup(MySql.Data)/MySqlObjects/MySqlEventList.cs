using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlEventList : IEnumerable<MySqlEvent>
    {
        string _sqlShowEvents = string.Empty;
        Dictionary<string, MySqlEvent> _lst = new Dictionary<string, MySqlEvent>();

        bool _allowAccess = true;
        public bool AllowAccess { get { return _allowAccess; } }

        public string SqlShowEvent { get { return _sqlShowEvents; } }

        public MySqlEventList()
        { }

        public MySqlEventList(MySqlCommand cmd)
        {
            try
            {
                string dbname = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
                _sqlShowEvents = string.Format("SHOW EVENTS WHERE UPPER(TRIM(Db))=UPPER(TRIM('{0}'));", QueryExpress.EscapeIdentifier(dbname));
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowEvents);

                foreach (DataRow dr in dt.Rows)
                {
                    var eventName = dr["Name"].ToString();
                    _lst.Add(eventName, new MySqlEvent(cmd, eventName, dr["Definer"].ToString()));
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

        public MySqlEvent this[string eventName]
        {
            get
            {
                if (_lst.ContainsKey(eventName))
                    return _lst[eventName];

                throw new Exception("Event \"" + eventName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string eventName)
        {
            return _lst.ContainsKey(eventName);
        }

        public IEnumerator<MySqlEvent> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}
