using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MySql.Data.MySqlClient
{
    public class MySqlProcedureList : IEnumerable<MySqlProcedure>
    {
        string _sqlShowProcedures = string.Empty;
        Dictionary<string, MySqlProcedure> _lst = new Dictionary<string, MySqlProcedure>();

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
                _sqlShowProcedures = string.Format("SHOW PROCEDURE STATUS WHERE UPPER(TRIM(Db))= UPPER(TRIM('{0}'));", QueryExpress.EscapeIdentifier(dbname));
                DataTable dt = QueryExpress.GetTable(cmd, _sqlShowProcedures);

                foreach (DataRow dr in dt.Rows)
                {
                    var name = dr["Name"].ToString();
                    _lst.Add(name, new MySqlProcedure(cmd, name, dr["Definer"].ToString()));
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

        public MySqlProcedure this[string procedureName]
        {
            get
            {
                if (_lst.ContainsKey(procedureName))
                    return _lst[procedureName];

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
            return _lst.ContainsKey(procedureName);
        }

        public IEnumerator<MySqlProcedure> GetEnumerator() =>
            _lst.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
           _lst.Values.GetEnumerator();
    }
}