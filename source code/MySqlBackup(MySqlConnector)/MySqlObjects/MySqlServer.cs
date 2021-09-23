using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlConnector
{
    public class MySqlServer
    {
        string _versionNumber;
        string _edition;
        decimal _majorVersionNumber = 0;
        string _characterSetServer = "";
        string _characterSetSystem = "";
        string _characterSetConnection = "";
        string _characterSetDatabase = "";
        string _currentUser = "";
        string _currentUserClientHost = "";
        string _currentClientHost = "";

        public string Version { get { return string.Format("{0} {1}", _versionNumber, _edition); } }
        public string VersionNumber { get { return _versionNumber; } }
        public decimal MajorVersionNumber { get { return _majorVersionNumber; } }
        public string Edition { get { return _edition; } }
        public string CharacterSetServer { get { return _characterSetServer; } }
        public string CharacterSetSystem { get { return _characterSetSystem; } }
        public string CharacterSetConnection { get { return _characterSetConnection; } }
        public string CharacterSetDatabase { get { return _characterSetDatabase; } }
        public string CurrentUser { get { return _currentUser; } }
        public string CurrentUserClientHost { get { return _currentUserClientHost; } }
        public string CurrentClientHost { get { return _currentClientHost; } }

        public MySqlServer()
        { }

        public void GetServerInfo(MySqlCommand cmd)
        {
            _edition = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version_comment';", 1);
            _versionNumber = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'version';", 1);
            _characterSetServer = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_server';", 1);
            _characterSetSystem = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_system';", 1);
            _characterSetConnection = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_connection';", 1);
            _characterSetDatabase = QueryExpress.ExecuteScalarStr(cmd, "SHOW variables LIKE 'character_set_database';", 1);

            _currentUserClientHost = QueryExpress.ExecuteScalarStr(cmd, "SELECT current_user;");

            string[] ca = _currentUserClientHost.Split('@');

            _currentUser = ca[0];
            _currentClientHost = ca[1];

            GetMajorVersionNumber();
        }

        void GetMajorVersionNumber()
        {
            string[] vsa = _versionNumber.Split('.');
            string v = "";
            if (vsa.Length > 1)
                v = vsa[0] + "." + vsa[1];
            else
                v = vsa[0];
            decimal.TryParse(v, out _majorVersionNumber);
        }
    }
}

