using System;
using System.Collections.Generic;
using System.Text;

namespace Devart.Data.MySql
{
    public class MySqlFunction
    {
        string _name;
        string _createFunctionSQL = "";
        string _createFunctionSqlWithoutDefiner = "";

        public string Name { get { return _name; } }
        public string CreateFunctionSQL { get { return _createFunctionSQL; } }
        public string CreateFunctionSQLWithoutDefiner { get { return _createFunctionSqlWithoutDefiner; } }

        public MySqlFunction(MySqlCommand cmd, string functionName, string definer)
        {
            _name = functionName;

            string sql = string.Format("SHOW CREATE FUNCTION `{0}`;", functionName);

            _createFunctionSQL = QueryExpress.ExecuteScalarStr(cmd, sql, 2);

            _createFunctionSQL = _createFunctionSQL.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            _createFunctionSQL = _createFunctionSQL.Replace("\n", "^~~~~~~~~~~~~~~^");
            _createFunctionSQL = _createFunctionSQL.Replace("\r", "^~~~~~~~~~~~~~~^");
            _createFunctionSQL = _createFunctionSQL.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            string[] sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            _createFunctionSqlWithoutDefiner = _createFunctionSQL.Replace(definer, string.Empty);
        }
    }
}
