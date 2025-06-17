using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using MySqlConnector;

namespace System
{
    public static class config
    {
        public static bool VariablesInitialized = false;

        static string _constr = null;

        public static string ConnString
        {
            get
            {
                if (_constr == null)
                {
                    ReadConnStr();
                }
                return _constr;
            }
            set
            {
                _constr = value;
            }
        }

        public static void InitializeVariables()
        {
            string e = ConnString;
            BackupFilesManager.InitializeVariables();
            VariablesInitialized = true;
        }

        public static MySqlConnection GetNewConnection()
        {
            return new MySqlConnection(ConnString);
        }

        public static bool TestConnectionOk()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_constr))
                    return false;

                using (MySqlConnection conn = GetNewConnection())
                {
                    conn.Open();
                }
                return true;
            }
            catch { }

            return false;
        }

        public static void SaveConnStr(string cstr)
        {
            string dir = HttpContext.Current.Server.MapPath("~/App_Data");
            Directory.CreateDirectory(dir);
            string file = HttpContext.Current.Server.MapPath("~/App_Data/constr.txt");
            File.WriteAllText(file, cstr);
            ReadConnStr();
        }

        public static void ReadConnStr()
        {
            string file = HttpContext.Current.Server.MapPath("~/App_Data/constr.txt");
            if (File.Exists(file))
            {
                _constr = File.ReadAllText(file);
            }
            else
            {
                _constr = null;
            }
        }

        public static string GetCurrentDatabaseName()
        {
            try
            {
                using (MySqlConnection conn = GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = "select database();";
                        return cmd.ExecuteScalar() + "";
                    }
                }
            }
            catch { }

            return "";
        }
    }
}