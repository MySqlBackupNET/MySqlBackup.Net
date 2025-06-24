using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace System.pages
{
    public partial class QuickTestAdjustColumnValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btRun_Click(object sender, EventArgs e)
        {
            ph1.Controls.Add(new LiteralControl("Results:" + Environment.NewLine + Environment.NewLine));

            string result = "";
            string folder = Server.MapPath("~/App_Data/temp");

            Directory.CreateDirectory(folder);

            if (cbNoTryCatch.Checked)
            {
                Test3AdjustColumnValue t = new Test3AdjustColumnValue(config.ConnString, folder, cbCleanDatabaseAfterUse.Checked, cbInsert.Checked, cbInsertIgnore.Checked, cbReplace.Checked, cbUpdate.Checked, cbInsertUpdate.Checked);
                result = t.RunTest();
            }
            else
            {
                try
                {
                    Test3AdjustColumnValue t = new Test3AdjustColumnValue(config.ConnString, folder, cbCleanDatabaseAfterUse.Checked, cbInsert.Checked, cbInsertIgnore.Checked, cbReplace.Checked, cbUpdate.Checked, cbInsertUpdate.Checked);
                    result = t.RunTest();
                }
                catch (Exception ex)
                {
                    result = $@"
{ex.Message}
{ex.StackTrace}
";
                }
            }

            ph1.Controls.Add(new LiteralControl(result));
        }
    }

    class Test3AdjustColumnValue
    {
        bool _requireCleanUp = false;
        string _connectionString;
        string basePath;
        StringBuilder sb = null;

        List<MySqlConnector.RowsDataExportMode> lstRowsExportMode = new List<RowsDataExportMode>();

        public Test3AdjustColumnValue(string connstr, string baseFolder, bool requireCleanUp, bool rowInsert, bool rowInsertIgnore, bool rowReplaceInto, bool rowUpdate, bool rowInsertUpdate)
        {
            _connectionString = connstr;
            basePath = baseFolder;
            _requireCleanUp = requireCleanUp;

            if (rowInsert)
                lstRowsExportMode.Add(RowsDataExportMode.Insert);

            if (rowInsertIgnore)
                lstRowsExportMode.Add(RowsDataExportMode.InsertIgnore);

            if (rowReplaceInto)
                lstRowsExportMode.Add(RowsDataExportMode.Replace);

            if (rowUpdate)
                lstRowsExportMode.Add(RowsDataExportMode.Update);

            if (rowInsertUpdate)
                lstRowsExportMode.Add(RowsDataExportMode.OnDuplicateKeyUpdate);

            // Clean up
            foreach (RowsDataExportMode mode in Enum.GetValues(typeof(RowsDataExportMode)))
            {
                string f1 = Path.Combine(basePath, $"adjust_value_dump_{mode}-1.sql");
                string f2 = Path.Combine(basePath, $"adjust_value_dump_{mode}-2.sql");
                string f3 = Path.Combine(basePath, $"adjust_value_dump_{mode}-3.sql");
                try
                {
                    if (File.Exists(f1))
                        File.Delete(f1);
                    if (File.Exists(f2))
                        File.Delete(f2);
                    if (File.Exists(f3))
                        File.Delete(f3);
                }
                catch { }
            }
        }

        public string RunTest()
        {
            sb = new StringBuilder();

            sb.AppendLine("=========================================");
            sb.AppendLine("Starting Adjust Value Test...");
            sb.AppendLine("=========================================");

            sb.AppendLine("Initial Sample SQL Dump Content:");
            sb.AppendLine();
            sb.AppendLine(GetSampleDumpContent());
            sb.AppendLine();

            // Test each export mode
            foreach (RowsDataExportMode mode in lstRowsExportMode)
            {
                RunIndividualTest(mode);
            }

            return sb.ToString();
        }

        private void RunIndividualTest(MySqlConnector.RowsDataExportMode mode)
        {
            string dbName1 = $"test_adjust_value_{mode}_1";
            string dbName2 = $"test_adjust_value_{mode}_2";
            string dbName3 = $"test_adjust_value_{mode}_3";

            string dumpFile1 = Path.Combine(basePath, $"adjust_value_dump_{mode}-1.sql");
            string dumpFile2 = Path.Combine(basePath, $"adjust_value_dump_{mode}-2.sql");
            string dumpFile3 = Path.Combine(basePath, $"adjust_value_dump_{mode}-3.sql");

            sb.AppendLine();
            sb.AppendLine($"--- Testing Export Mode: {mode} ---");
            sb.AppendLine($"Database 1: {dbName1}");
            sb.AppendLine($"Database 2: {dbName2}");
            sb.AppendLine($"Database 3: {dbName3}");
            sb.AppendLine($"Dump File 1: {dumpFile1}");
            sb.AppendLine($"Dump File 2: {dumpFile2}");
            sb.AppendLine($"Dump File 3: {dumpFile3}");

            sb.AppendLine();

            sb.AppendLine($"Step 1: Create Database 1");

            ImportFromString(dbName1, GetSampleDumpContent());

            sb.AppendLine($"Step 2: Export Database 1 to Dump File 1");

            ExportToFile(dbName1, dumpFile1, mode);

            sb.AppendLine($"Step 3: Import Dump File 1 to Database 2");

            ImportFromFile(dbName2, dumpFile1);

            sb.AppendLine($"Step 4: Export Database 2 to Dump File 2");

            ExportToFile(dbName2, dumpFile2, mode);

            sb.AppendLine($"Step 5: Import Dump File 2 to Database 3");

            ImportFromFile(dbName3, dumpFile2);

            sb.AppendLine($"Step 5: Export Database 3 to Dump File 3");

            ExportToFile(dbName3, dumpFile3, mode);

            sb.AppendLine($"Step 6: Compute SHA256 checksums for Dump File 1, 2, 3");

            string sha1 = CalculateSHA256FromFile(dumpFile1);
            string sha2 = CalculateSHA256FromFile(dumpFile2);
            string sha3 = CalculateSHA256FromFile(dumpFile3);

            // Compare dump file checksums
            bool dumpFilesMatch = sha2.Equals(sha3, StringComparison.OrdinalIgnoreCase);

            sb.AppendLine();
            sb.AppendLine($"Dump File 1 SHA256   : {sha1}");
            sb.AppendLine($"Dump File 2 SHA256   : {sha2}");
            sb.AppendLine($"Dump File 3 SHA256   : {sha3}");
            sb.AppendLine($"Compare File 2 and 3 : {(dumpFilesMatch ? "MATCH ✓" : "NOT MATCH ✗")}");
            sb.AppendLine();

            if (dumpFilesMatch)
            {
                sb.AppendLine("SUCCESS: Backup and restore test passed!");
                sb.AppendLine("The tool maintains data integrity through multiple backup/restore cycles.");
            }
            else
            {
                sb.AppendLine("FAILURE: Backup and restore test failed!");
                sb.AppendLine("The SHA256 checksums do not match.");
            }

            // Cleanup
            sb.AppendLine();
            sb.AppendLine("Step 7: Cleaning up test databases...");
            if (_requireCleanUp)
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName1}`;";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName2}`;";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName3}`;";
                        cmd.ExecuteNonQuery();
                    }
                }
                sb.AppendLine($"DROP DATABASE IF EXISTS `{dbName1}`;");
                sb.AppendLine($"DROP DATABASE IF EXISTS `{dbName2}`;");
                sb.AppendLine($"DROP DATABASE IF EXISTS `{dbName3}`;");
            }
            else
            {
                sb.AppendLine(">> Skipped, user requests not to clean up database.");
            }

            sb.AppendLine();
            sb.AppendLine($"--- End of {mode} Test ---");
        }

        void ImportFromString(string dbName, string sqlDump)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName}`;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"USE `{dbName}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromString(sqlDump);
                    }
                }
            }
        }

        void ImportFromFile(string dbName, string dumpFile)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName}`;";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"USE `{dbName}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromFile(dumpFile);
                    }
                }
            }
        }

        void ExportToFile(string dbName, string dumpFile, RowsDataExportMode mode)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"USE `{dbName}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ExportInfo.RowsExportMode = mode;
                        mb.ExportInfo.RecordDumpTime = false;
                        mb.ExportInfo.InsertLineBreakBetweenInserts = true;

                        mb.ExportInfo.AddTableColumnValueAdjustment("member2", "name", MaskName);
                        mb.ExportInfo.AddTableColumnValueAdjustment("member2", "email", MaskEmail);
                        mb.ExportInfo.AddTableColumnValueAdjustment("member2", "username", MaskUsername);

                        mb.ExportToFile(dumpFile);
                    }
                }
            }
        }

        object MaskName(object obInput)
        {
            if (obInput == null) return null;

            string input = obInput.ToString();
            if (string.IsNullOrEmpty(input)) return "";

            // If input is less than 6 characters, mask everything with asterisks
            if (input.Length < 6)
            {
                return new string('*', input.Length);
            }

            // Return first 3 characters + 12 asterisks + last 3 characters
            string first3 = input.Substring(0, 3);
            string last3 = input.Substring(input.Length - 3);

            return $"{first3}************{last3}";
        }

        object MaskEmail(object obInput)
        {
            if (obInput == null) return null;

            string email = obInput.ToString();
            if (string.IsNullOrEmpty(email)) return "";

            int atIndex = email.IndexOf('@');
            if (atIndex == -1) return "***@***.***"; // Invalid email format

            string localPart = email.Substring(0, atIndex);
            string domain = email.Substring(atIndex + 1);

            // Mask local part: show first 3 chars (or less) + asterisks
            string maskedLocal = localPart.Length >= 3
                ? localPart.Substring(0, 3) + "*****"
                : new string('*', Math.Max(localPart.Length, 3)) + "*****";

            // Mask domain: first char + asterisks + extension
            int lastDotIndex = domain.LastIndexOf('.');
            if (lastDotIndex == -1)
            {
                return $"{maskedLocal}@*****.***";
            }

            string extension = domain.Substring(lastDotIndex + 1);
            string maskedDomain = domain[0] + "****." + extension;

            return $"{maskedLocal}@{maskedDomain}";
        }

        static object MaskUsername(object obInput)
        {
            if (obInput == null) return null;

            string username = obInput.ToString();
            if (string.IsNullOrEmpty(username)) return "";

            // Show first 2 characters + 12 asterisks
            if (username.Length < 2)
            {
                return new string('*', 14);
            }

            string first2 = username.Substring(0, 2);
            return $"{first2}************";
        }

        private string CalculateSHA256FromFile(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(fileStream);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        string GetSampleDumpContent()
        {
            string sql = @"CREATE TABLE IF NOT EXISTS `member1` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  `username` VARCHAR(100) NOT NULL,
  `position` VARCHAR(100) NOT NULL,
  `group_id` INT NOT NULL,
  PRIMARY KEY (`id`)
);

INSERT INTO `member1` (`id`, `name`, `email`, `username`, `position`, `group_id`) VALUES
(1, 'Jennifer Wilson', 'jennifer.wilson@innovatetech.com', 'jwilson', 'Frontend Developer', 1),
(2, 'Alex Johnson', 'alex.johnson@creativestudio.net', 'ajohnson', 'Content Strategist', 2),
(3, 'Maria Garcia', 'maria.garcia@datasolutions.org', 'mgarcia', 'Business Analyst', 3);


CREATE TABLE IF NOT EXISTS `member2` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  `username` VARCHAR(100) NOT NULL,
  `position` VARCHAR(100) NOT NULL,
  `group_id` INT NOT NULL,
  PRIMARY KEY (`id`)
);

INSERT INTO `member2` (`id`, `name`, `email`, `username`, `position`, `group_id`) VALUES
(1, 'John Anderson', 'john.anderson@lightmoon.com', 'janderson', 'Senior Software Engineer', 1),
(2, 'Sarah Mitchell', 'sarah.mitchell@marssen.com', 'smitchell', 'Marketing Manager', 2),
(3, 'Michael Chen', 'michael.chen@wendysam.com', 'mchen', 'Product Manager', 3);

CREATE TABLE IF NOT EXISTS `staff` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(255) NOT NULL,
  `email` VARCHAR(255) NOT NULL,
  `username` VARCHAR(100) NOT NULL,
  `position` VARCHAR(100) NOT NULL,
  `group_id` INT NOT NULL,
  PRIMARY KEY (`id`)
);

INSERT INTO `staff` (`id`, `name`, `email`, `username`, `position`, `group_id`) VALUES
(1, 'David Thompson', 'david.thompson@techcorp.net', 'dthompson', 'DevOps Engineer', 1),
(2, 'Lisa Rodriguez', 'lisa.rodriguez@innovate.io', 'lrodriguez', 'UX Designer', 2),
(3, 'Robert Kim', 'robert.kim@dataflow.biz', 'rkim', 'Data Analyst', 3);";

            return sql;
        }
    }
}