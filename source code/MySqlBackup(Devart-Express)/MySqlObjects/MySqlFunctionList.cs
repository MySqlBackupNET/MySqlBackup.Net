using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Devart.Data.MySql
{
    public class MySqlFunctionList : IDisposable
    {
        List<MySqlFunction> _lst = new List<MySqlFunction>();
        string _sqlShowFunctions = "";

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
                _sqlShowFunctions = string.Format("SHOW FUNCTION STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", dbname);
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowFunctions);

                foreach (DataRow dr in dt.Rows)
                {
                    _lst.Add(new MySqlFunction(cmd, dr["Name"] + "", dr["Definer"] + ""));
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

        public MySqlFunction this[int functionIndex]
        {
            get
            {
                return _lst[functionIndex];
            }
        }

        public MySqlFunction this[string functionName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == functionName)
                    {
                        return _lst[i];
                    }
                }
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
            if (this[functionName] == null)
                return false;
            return true;
        }

        public IEnumerator<MySqlFunction> GetEnumerator()
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
