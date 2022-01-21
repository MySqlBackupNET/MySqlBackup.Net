using System;
using System.Windows.Forms;

namespace MySqlBackupTestApp
{
    static class Program
    {
        public static string Version = "V2.3.6";
        public static string DateVersion = "October 17, 2021";

        private static string _connectionString = string.Empty;

        public static string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                    throw new Exception("Connection string is empty.");
                else
                    return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
        public static string DefaultFolder = string.Empty;
        public static string TargetFile = string.Empty;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        public static bool TargetDirectoryIsValid()
        {
            try
            {
                string dir = System.IO.Path.GetDirectoryName(Program.TargetFile);

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Specify path is not valid. Press [Export As] to specify a valid file path." + Environment.NewLine + Environment.NewLine + ex.Message, "Invalid Directory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public static bool SourceFileExists()
        {
            if (!System.IO.File.Exists(Program.TargetFile))
            {
                MessageBox.Show("File is not exists. Press [Select File] to choose a SQL Dump file." + Environment.NewLine + Environment.NewLine + Program.TargetFile, "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }
    }
}
