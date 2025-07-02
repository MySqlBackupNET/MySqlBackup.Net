using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MySqlConnector
{
    public class MySqlFunctionList : IEnumerable<MySqlFunction>
    {
        string _sqlShowFunctions = string.Empty;
        Dictionary<string, MySqlFunction> _lst = new Dictionary<string, MySqlFunction>();

        bool _allowAccess = true;
        public bool AllowAccess { get { return _allowAccess; } }

        public string SqlShowFunctions { get { return _sqlShowFunctions; } }

        public MySqlFunctionList()
        { }

        public MySqlFunctionList(MySqlCommand cmd)
        {
            try
            {
                string dbname = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
                _sqlShowFunctions = string.Format("SHOW FUNCTION STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", QueryExpress.EscapeIdentifier(dbname));
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowFunctions);

                foreach (DataRow dr in dt.Rows)
                {
                    var name = dr["Name"].ToString();
                    _lst.Add(name, new MySqlFunction(cmd, name, dr["Definer"].ToString()));
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

        public MySqlFunction this[string functionName]
        {
            get
            {
                if (_lst.ContainsKey(functionName))
                    return _lst[functionName];

                throw new Exception("Function \"" + functionName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string functionName)
        {
            return _lst.ContainsKey(functionName);
        }

        public IEnumerator<MySqlFunction> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}
