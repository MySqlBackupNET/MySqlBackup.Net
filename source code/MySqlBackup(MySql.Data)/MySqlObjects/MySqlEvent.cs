namespace MySql.Data.MySqlClient
{
    public class MySqlEvent
    {
        string _name;
        string _createEventSql = string.Empty;
        string _createEventSqlWithoutDefiner = string.Empty;

        public string Name { get { return _name; } }
        public string CreateEventSql { get { return _createEventSql; } }
        public string CreateEventSqlWithoutDefiner { get { return _createEventSqlWithoutDefiner; } }

        public MySqlEvent(MySqlCommand cmd, string eventName, string definer)
        {
            _name = eventName;

            _createEventSql = QueryExpress.ExecuteScalarStr(cmd, string.Format("SHOW CREATE EVENT `{0}`;", QueryExpress.EscapeIdentifier(_name)), "Create Event");

            _createEventSql = _createEventSql.Replace("\r\n", "^~~~~~~~~~~~~~~^");
            _createEventSql = _createEventSql.Replace("\n", "^~~~~~~~~~~~~~~^");
            _createEventSql = _createEventSql.Replace("\r", "^~~~~~~~~~~~~~~^");
            _createEventSql = _createEventSql.Replace("^~~~~~~~~~~~~~~^", "\r\n");

            string[] sa = definer.Split('@');
            definer = string.Format(" DEFINER=`{0}`@`{1}`", sa[0], sa[1]);

            _createEventSqlWithoutDefiner = _createEventSql.Replace(definer, string.Empty);
        }
    }
}
