using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class MySqlProcedure
    {
        string _name;
        string _createProcedureSQL;
        string _createProcedureSQLWithoutDefiner;

        public string Name { get { return _name; } }
        public string CreateProcedureSQL { get { return _createProcedureSQL; } }
        public string CreateProcedureSQLWithoutDefiner { get { return _createProcedureSQLWithoutDefiner; } }

        public MySqlProcedure(MySqlCommand cmd, string procedureName, string definer)
        {
            _name = procedureName;

            string sql = string.Format("SHOW CREATE PROCEDURE `{0}`;", procedureName);

            _createProcedureSQL = QueryExpress.ExecuteScalarStr(cmd, sql, 2);

            _createProcedureSQL = _createProcedureSQL.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            _createProcedureSQL = _createProcedureSQL.Replace("\n", "^~~~~~~~~~~~~~~^");
            _createProcedureSQL = _createProcedureSQL.Replace("\r", "^~~~~~~~~~~~~~~^");
            _createProcedureSQL = _createProcedureSQL.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            string[] sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            _createProcedureSQLWithoutDefiner = _createProcedureSQL.Replace(definer, string.Empty);
        }
    }
}
