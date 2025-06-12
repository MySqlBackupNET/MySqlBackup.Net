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

        public static MySqlConnection GetNewConnection()
        {
            return new MySqlConnection(_constr);
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
    }
}