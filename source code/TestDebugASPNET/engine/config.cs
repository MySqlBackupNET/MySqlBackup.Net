using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using MySqlConnector;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Web.Services.Description;

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
                return _constr;
            }
            set
            {
                _constr = value;
            }
        }

        public static void InitializeVariables()
        {
            ReadConnStr();
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

        public static void GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path)
        {
            mysqld_path = "";
            mysqladmin_path = "";

            string fileMySqldExeConfig = HttpContext.Current.Server.MapPath("~/App_Data/mysqldpath.txt");
            if (File.Exists(fileMySqldExeConfig))
            {
                string a = File.ReadAllText(fileMySqldExeConfig);
                string[] pathArray = a.Split('|');
                if (pathArray.Length > 1)
                {
                    mysqld_path = pathArray[0];
                    mysqladmin_path = pathArray[1];
                }
            }
        }

        public static void SaveMysqlInstancePath(string mysqldExePath, string mysqladminpath)
        {
            string folder = HttpContext.Current.Server.MapPath("~/App_Data");
            Directory.CreateDirectory(folder);

            string fileMySqldExeConfig = HttpContext.Current.Server.MapPath("~/App_Data/mysqldpath.txt");

            File.WriteAllText(fileMySqldExeConfig, mysqldExePath + "|" + mysqladminpath);
        }

        public static bool CheckMySqlServerRunningState()
        {
            try
            {
                using (var conn = GetNewConnection())
                {
                    conn.Open();
                    return true;
                }

                //using (var tcpClient = new TcpClient())
                //{
                //    var result = tcpClient.BeginConnect(host, port, null, null);
                //    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3));

                //    if (success)
                //    {
                //        tcpClient.EndConnect(result);
                //        return true;
                //    }
                //    return false;
                //}
            }
            catch { }

            return false;
        }

        public static void StartMySqlServer()
        {
            // i.e. C:\mysql\v845\bin\mysqld.exe
            GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path);

            if (!File.Exists(mysqld_path))
            {
                throw new Exception("mysqld_path instance not found");
            }

            // Check the state first before running
            if (!CheckMySqlServerRunningState())
            {
                try
                {
                    MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder(ConnString);

                    // Create new process start info
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = mysqld_path,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    // Start the process
                    using (Process process = Process.Start(processInfo))
                    {
                        // Optionally, you can wait for the process to initialize
                        // process.WaitForExit(); // Uncomment if you need to wait for completion
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors (e.g., file not found, permission issues)
                    throw new Exception($"Failed to start MySQL server: {ex.Message}");
                }
            }
        }

        public static bool StopMySqlServer()
        {
            // i.e. C:\mysql\v845\bin\mysqld.exe
            GetMySqlInstancePath(out string mysqld_path, out string mysqladmin_path);

            if (!File.Exists(mysqladmin_path))
            {
                throw new Exception("mysqladmin_path instance not found");
            }

            // Check the state first before running
            if (CheckMySqlServerRunningState())
            {
                try
                {
                    string configFilePath = GenerateTempMySqlConfigFile();

                    // Create new process start info
                    ProcessStartInfo processInfo = new ProcessStartInfo
                    {
                        FileName = mysqladmin_path,
                        Arguments = $"\"--defaults-extra-file={configFilePath}\" shutdown",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    _ = Task.Run(() => { Threading.Thread.Sleep(1000); File.Delete(configFilePath); });

                    // Start the process
                    using (Process process = Process.Start(processInfo))
                    {
                        // Optionally, you can wait for the process to initialize
                        process.WaitForExit(); // Uncomment if you need to wait for completion
                    }

                    for (int checkMySqlStateAttemp = 0; checkMySqlStateAttemp < 3; checkMySqlStateAttemp++)
                    {
                        System.Threading.Thread.Sleep(500);

                        if (!CheckMySqlServerRunningState())
                        {
                            return true;
                        }
                    }

                }
                catch (Exception ex)
                {
                    // Handle any errors (e.g., file not found, permission issues)
                    throw new Exception($"Failed to start MySQL server: {ex.Message}");
                }
            }

            return false;
        }

        public static string GenerateTempMySqlConfigFile()
        {
            var builder = new MySqlConnectionStringBuilder(ConnString);

            string host = builder.Server ?? "localhost";
            string user = builder.UserID ?? "root";
            string password = builder.Password ?? "";
            uint port = builder.Port != 0 ? builder.Port : 3306;

            Random rd = new Random();

            string folder = HttpContext.Current.Server.MapPath("~/App_Data");
            Directory.CreateDirectory(folder);

            string filePathCnf = Path.Combine(folder, $"my_{rd.Next(100000, 999999)}.cnf");
            string cnfContent = $@"[client]
user={user}
password={password}
host={host}
port={port}";

            File.WriteAllText(filePathCnf, cnfContent);

            return filePathCnf;
        }
    }
}