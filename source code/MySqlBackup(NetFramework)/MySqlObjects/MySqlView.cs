using System;
using System.Collections.Generic;
using System.Text;

namespace MySql.Data.MySqlClient
{
    public class MySqlView
    {
        string _name = "";
        string _createViewSQL = "";
        string _createViewSQLWithoutDefiner = "";

        public string Name { get { return _name; } }
        public string CreateViewSQL { get { return _createViewSQL; } }
        public string CreateViewSQLWithoutDefiner { get { return _createViewSQLWithoutDefiner; } }

        public MySqlView(MySqlCommand cmd, string viewName)
        {
            _name = viewName;

            string sqlShowCreate = string.Format("SHOW CREATE VIEW `{0}`;", viewName);

            System.Data.DataTable dtView = QueryExpress.GetTable(cmd, sqlShowCreate);

            _createViewSQL = dtView.Rows[0]["Create View"] + ";";

            _createViewSQL = _createViewSQL.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            _createViewSQL = _createViewSQL.Replace("\n", "^~~~~~~~~~~~~~~^");
            _createViewSQL = _createViewSQL.Replace("\r", "^~~~~~~~~~~~~~~^");
            _createViewSQL = _createViewSQL.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            _createViewSQLWithoutDefiner = QueryExpress.EraseDefiner(_createViewSQL);
        }
    }
}
