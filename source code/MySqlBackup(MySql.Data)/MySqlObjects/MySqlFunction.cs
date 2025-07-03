namespace MySql.Data.MySqlClient
{
    public class MySqlFunction
    {
        string _name;
        string _createFunctionSQL = string.Empty;
        string _createFunctionSqlWithoutDefiner = string.Empty;

        public string Name { get { return _name; } }
        public string CreateFunctionSQL { get { return _createFunctionSQL; } }
        public string CreateFunctionSQLWithoutDefiner { get { return _createFunctionSqlWithoutDefiner; } }

        public MySqlFunction(MySqlCommand cmd, string functionName, string definer)
        {
            _name = functionName;

            string sql = string.Format("SHOW CREATE FUNCTION `{0}`;", QueryExpress.EscapeIdentifier(functionName));

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
