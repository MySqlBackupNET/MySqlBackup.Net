using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlProcedureList : IDisposable
    {
        List<MySqlProcedure> _lst = new List<MySqlProcedure>();
        string _sqlShowProcedures = "";

        bool _allowAccess = true;
        public bool AllowAccess { get { return _allowAccess; } }

        public string SqlShowProcedures { get { return _sqlShowProcedures; } }

        public MySqlProcedureList()
        { }

        public MySqlProcedureList(MySqlCommand cmd)
        {
            try
            {
                string dbname = QueryExpress.ExecuteScalarStr(cmd, "SELECT DATABASE();");
                _sqlShowProcedures = string.Format("SHOW PROCEDURE STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", dbname);
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowProcedures);

                foreach (DataRow dr in dt.Rows)
                {
                    _lst.Add(new MySqlProcedure(cmd, dr["Name"] + "", dr["Definer"] + ""));
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

        public MySqlProcedure this[int indexProcedure]
        {
            get
            {
                return _lst[indexProcedure];
            }
        }

        public MySqlProcedure this[string procedureName]
        {
            get
            {
                for (int i = 0; i < _lst.Count; i++)
                {
                    if (_lst[i].Name == procedureName)
                    {
                        return _lst[i];
                    }
                }
                throw new Exception("Store procedure \"" + procedureName + "\" is not existed.");
            }
        }

        public int Count
        {
            get
            {
                return _lst.Count;
            }
        }

        public bool Contains(string procedureName)
        {
            if (this[procedureName] == null)
                return false;
            else
                return true;
        }

        public IEnumerator<MySqlProcedure> GetEnumerator()
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