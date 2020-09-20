using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlEventList : IDisposable
    {
        List<MySqlEvent> _lst = new List<MySqlEvent>();
        string _sqlShowEvents = "";

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
                _sqlShowEvents = string.Format("SHOW EVENTS WHERE UPPER(TRIM(Db))=UPPER(TRIM('{0}'));", dbname);
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowEvents);

                foreach (DataRow dr in dt.Rows)
                {
                    _lst.Add(new MySqlEvent(cmd, dr["Name"] + "", dr["Definer"] + ""));
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

        public MySqlEvent this[int eventIndex]
        {
            get
            {
                return _lst[eventIndex];
            }
        }

        public MySqlEvent this[string eventName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == eventName)
                        return _lst[i];
                }
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
            if (this[eventName] == null)
                return false;
            return true;
        }

        public IEnumerator<MySqlEvent> GetEnumerator()
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
