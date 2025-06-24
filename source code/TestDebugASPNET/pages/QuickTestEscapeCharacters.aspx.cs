using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class QuickTestEscapeCharacters : System.Web.UI.Page
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
                TestEscapeCharacters t = new TestEscapeCharacters(config.ConnString, folder, cbNoBackSlashEscape.Checked, cbCleanDatabaseAfterUse.Checked, cbInsert.Checked, cbInsertIgnore.Checked, cbReplace.Checked, cbUpdate.Checked, cbInsertUpdate.Checked);
                result = t.RunTest();
            }
            else
            {
                try
                {
                    TestEscapeCharacters t = new TestEscapeCharacters(config.ConnString, folder, cbNoBackSlashEscape.Checked, cbCleanDatabaseAfterUse.Checked, cbInsert.Checked, cbInsertIgnore.Checked, cbReplace.Checked, cbUpdate.Checked, cbInsertUpdate.Checked);
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

    public class TestEscapeCharacters
    {
        bool _requireCleanUp = false;
        string _connectionString;
        string basePath;
        StringBuilder sb = null;
        bool _no_back_slash_escape = false;

        List<MySqlConnector.RowsDataExportMode> lstRowsExportMode = new List<RowsDataExportMode>();

        public TestEscapeCharacters(string connstr, string baseFolder, bool no_back_slash_escape, bool requireCleanUp, bool rowInsert, bool rowInsertIgnore, bool rowReplaceInto, bool rowUpdate, bool rowInsertUpdate)
        {
            _connectionString = connstr;
            basePath = baseFolder;
            _requireCleanUp = requireCleanUp;
            _no_back_slash_escape = no_back_slash_escape;

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
                string f1 = Path.Combine(basePath, $"escape_test_dump_{mode}-1.sql");
                string f2 = Path.Combine(basePath, $"escape_test_dump_{mode}-2.sql");
                try
                {
                    if (File.Exists(f1))
                        File.Delete(f1);
                    if (File.Exists(f2))
                        File.Delete(f2);
                }
                catch { }
            }
        }

        public string RunTest()
        {
            sb = new StringBuilder();

            sb.AppendLine("=========================================");
            sb.AppendLine("Starting String Escape Characters Test...");
            sb.AppendLine("=========================================");

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    EnableNoBackSlashEscape(cmd, true);
                }
            }

            // Test each export mode
            foreach (RowsDataExportMode mode in lstRowsExportMode)
            {
                RunIndividualTest(mode);
            }

            return sb.ToString();
        }

        void EnableNoBackSlashEscape(MySqlCommand cmd, bool writeSqlStatementToOutput)
        {
            string sql = "";

            cmd.CommandText = "SELECT @@sql_mode;";
            string sql_mode = cmd.ExecuteScalar() + "";

            if (_no_back_slash_escape)
            {
                // Check if NO_BACKSLASH_ESCAPES is already set
                bool hasNoBackslashEscapes = sql_mode.Split(',')
                    .Any(mode => mode.Trim().Equals("NO_BACKSLASH_ESCAPES", StringComparison.OrdinalIgnoreCase));

                // If NO_BACKSLASH_ESCAPES is not set, add it
                if (!hasNoBackslashEscapes)
                {
                    // Add NO_BACKSLASH_ESCAPES to the existing modes
                    string newSqlMode = string.IsNullOrWhiteSpace(sql_mode)
                        ? "NO_BACKSLASH_ESCAPES"
                        : sql_mode + ",NO_BACKSLASH_ESCAPES";

                    sql = $"SET SESSION sql_mode = '{newSqlMode}';";

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sql = $"SET SESSION sql_mode = '{sql_mode}';";
                }
            }
            else
            {
                // Check if NO_BACKSLASH_ESCAPES is set
                bool hasNoBackslashEscapes = sql_mode.Split(',')
                    .Any(mode => mode.Trim().Equals("NO_BACKSLASH_ESCAPES", StringComparison.OrdinalIgnoreCase));

                // If NO_BACKSLASH_ESCAPES is set, remove it
                if (hasNoBackslashEscapes)
                {
                    // Remove NO_BACKSLASH_ESCAPES from the existing modes
                    var modes = sql_mode.Split(',')
                        .Select(m => m.Trim())
                        .Where(m => !m.Equals("NO_BACKSLASH_ESCAPES", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    string newSqlMode = string.Join(",", modes);

                    // If all modes were removed, set to empty string
                    sql = string.IsNullOrWhiteSpace(newSqlMode)
                        ? "SET SESSION sql_mode = '';"
                        : $"SET SESSION sql_mode = '{newSqlMode}';";

                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                    // Optionally, verify it was removed
                    cmd.CommandText = "SELECT @@sql_mode;";
                    sql_mode = cmd.ExecuteScalar() + "";
                }
                else
                {
                    sql = $"SET SESSION sql_mode = '{sql_mode}';";
                }
            }

            if (writeSqlStatementToOutput)
            {
                if (_no_back_slash_escape)
                {
                    sb.AppendLine("Enable [NO_BACKSLASH_ESCAPES]");
                }
                else
                {
                    sb.AppendLine("Disabling [NO_BACKSLASH_ESCAPES]");
                }
                sb.AppendLine(sql);
            }
        }

        private void RunIndividualTest(MySqlConnector.RowsDataExportMode mode)
        {
            // Generate random database names
            Random random = new Random();
            int rd = random.Next(1000, 9999);
            string dbName1 = $"test_escape_char_{rd}_1";
            string dbName2 = $"test_escape_char_{rd}_2";
            string dumpFile1 = Path.Combine(basePath, $"escape_test_dump_{mode}-1.sql");
            string dumpFile2 = Path.Combine(basePath, $"escape_test_dump_{mode}-2.sql");

            sb.AppendLine();
            sb.AppendLine($"--- Testing Export Mode: {mode} ---");
            sb.AppendLine($"Source Database: {dbName1}");
            sb.AppendLine($"Target Database: {dbName2}");
            sb.AppendLine($"Dump File 1: {dumpFile1}");
            sb.AppendLine($"Dump File 2: {dumpFile2}");

            // Create test string with ALL escape characters
            string originalValue = CreateTestStringWithAllEscapeCharacters();
            sb.AppendLine($"Original string length: {originalValue.Length} characters");
            sb.AppendLine($"Original string preview: {GetSafeStringPreview(originalValue)}");

            string originalSha256 = CalculateSHA256FromString(originalValue);

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    EnableNoBackSlashEscape(cmd, false);

                    // Step 1: Clean up and create first database
                    sb.AppendLine();
                    sb.AppendLine("Step 1: Setting up source database...");
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName1}`";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"CREATE DATABASE `{dbName1}`";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"USE `{dbName1}`";
                    cmd.ExecuteNonQuery();

                    // Step 2: Create test table
                    sb.AppendLine("Step 2: Creating test table...");
                    cmd.CommandText = @"
                        CREATE TABLE test_escape_table (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            test_string TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci
                        ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4";
                    cmd.ExecuteNonQuery();

                    // Step 3: Insert test data using parameterized query
                    sb.AppendLine("Step 3: Inserting test data with parameterized query...");
                    cmd.CommandText = "INSERT INTO test_escape_table (test_string) VALUES (@testString)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@testString", originalValue);
                    cmd.ExecuteNonQuery();

                    // Step 4: Export the first database
                    sb.AppendLine("Step 4: Exporting source database to dump file 1...");
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ExportInfo.RowsExportMode = mode;
                        mb.ExportInfo.RecordDumpTime = false;
                        mb.ExportInfo.EnableComment = true;
                        mb.ExportToFile(dumpFile1);
                    }

                    // Calculate SHA256 for first dump file
                    string dumpFile1Sha256 = CalculateSHA256FromFile(dumpFile1);

                    // Step 5: Create second database
                    sb.AppendLine("Step 5: Setting up target database...");
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName2}`";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"CREATE DATABASE `{dbName2}`";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"USE `{dbName2}`";
                    cmd.ExecuteNonQuery();

                    // Step 6: Import the dump file
                    sb.AppendLine("Step 6: Importing dump file to target database...");
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromFile(dumpFile1);
                    }

                    // Step 7: Export the second database
                    sb.AppendLine("Step 7: Exporting target database to dump file 2...");
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ExportInfo.RowsExportMode = mode;
                        mb.ExportInfo.RecordDumpTime = false;
                        mb.ExportInfo.EnableComment = true;
                        mb.ExportToFile(dumpFile2);
                    }

                    // Calculate SHA256 for second dump file
                    string dumpFile2Sha256 = CalculateSHA256FromFile(dumpFile2);

                    // Step 8: Select the data back
                    sb.AppendLine("Step 8: Retrieving data from target database...");
                    cmd.CommandText = "SELECT test_string FROM test_escape_table WHERE id = 1";
                    string retrievedValue = cmd.ExecuteScalar()?.ToString() ?? "";

                    // Step 9: Calculate checksum and compare
                    sb.AppendLine("Step 9: Comparing results...");
                    string retrievedSha256 = CalculateSHA256FromString(retrievedValue);

                    sb.AppendLine($"Retrieved string length: {retrievedValue.Length} characters");
                    sb.AppendLine($"Retrieved string preview: {GetSafeStringPreview(retrievedValue)}");

                    // Compare string checksums
                    bool stringIsMatch = originalSha256.Equals(retrievedSha256, StringComparison.OrdinalIgnoreCase);

                    // Compare dump file checksums
                    bool dumpFilesMatch = dumpFile1Sha256.Equals(dumpFile2Sha256, StringComparison.OrdinalIgnoreCase);

                    sb.AppendLine();
                    sb.AppendLine($"Original  String SHA256 : {originalSha256}");
                    sb.AppendLine($"Retrieved String SHA256 : {retrievedSha256}");
                    sb.AppendLine($"String Comparison       : {(stringIsMatch ? "MATCH ✓" : "NOT MATCH ✗")}");
                    sb.AppendLine();
                    sb.AppendLine($"Dump File 1 SHA256      : {dumpFile1Sha256}");
                    sb.AppendLine($"Dump File 2 SHA256      : {dumpFile2Sha256}");
                    sb.AppendLine($"Dump Files Comparison   : {(dumpFilesMatch ? "MATCH ✓" : "NOT MATCH ✗")}");
                    sb.AppendLine();

                    if (!stringIsMatch)
                    {
                        sb.AppendLine("ERROR: String escape/unescape test FAILED!");
                        sb.AppendLine("The original string and retrieved string do not match.");

                        // Detailed analysis of differences
                        AnalyzeDifferences(originalValue, retrievedValue);
                    }
                    else
                    {
                        sb.AppendLine("SUCCESS: String escape/unescape test PASSED!");
                        sb.AppendLine("Data integrity maintained through export/import cycle.");
                    }

                    // Cleanup
                    sb.AppendLine();
                    sb.AppendLine("Step 10: Cleaning up test databases...");
                    if (_requireCleanUp)
                    {
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName1}`";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName2}`";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        sb.AppendLine(">> Skipped, user requests not to clean up database.");
                    }
                }
            }

            sb.AppendLine($"--- End of {mode} Test ---");
            sb.AppendLine();
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

        private string CreateTestStringWithAllEscapeCharacters()
        {
            StringBuilder testString = new StringBuilder();

            // Header
            testString.AppendLine("=== MySQL Escape Characters Test String ===");

            // Single quote (most critical for SQL)
            testString.AppendLine("Single quotes: 'Hello' 'World' 'It''s a test'");

            // Double quotes
            testString.AppendLine("Double quotes: \"Hello\" \"World\" \"Test\"");

            // Backslash
            testString.AppendLine("Backslashes: \\ \\path\\to\\file \\\\ \\n \\t \\r");

            // Null character (if supported by string)
            testString.Append("Null character: ");
            testString.Append('\0');
            testString.AppendLine(" (between text)");

            // Carriage return and newline
            testString.Append("Carriage return: ");
            testString.Append('\r');
            testString.AppendLine(" (after CR)");

            testString.Append("Newline: ");
            testString.Append('\n');
            testString.AppendLine("(after LF)");

            // Tab character
            testString.Append("Tab characters: ");
            testString.Append('\t');
            testString.Append("between");
            testString.Append('\t');
            testString.AppendLine("words");

            // Backspace
            testString.Append("Backspace: ");
            testString.Append('\b');
            testString.AppendLine(" (after BS)");

            // Form feed
            testString.Append("Form feed: ");
            testString.Append('\f');
            testString.AppendLine(" (after FF)");

            // Vertical tab
            testString.Append("Vertical tab: ");
            testString.Append('\v');
            testString.AppendLine(" (after VT)");

            // Bell/Alert
            testString.Append("Bell character: ");
            testString.Append('\a');
            testString.AppendLine(" (after Bell)");

            // Unicode characters
            testString.AppendLine("Unicode: 你好世界 🌍 🚀 ñáéíóú àèìòù âêîôû");

            // Emoji and special Unicode
            testString.AppendLine("Emojis: 😀 😃 😄 😁 🎉 🎊 🔥 💯 ⭐ ❤️");

            // Mixed escape scenarios
            testString.AppendLine("Mixed: 'It\\'s a \"test\" with \\backslash and \ttab'");

            // SQL injection-like patterns (should be safely escaped)
            testString.AppendLine("SQL-like: '; DROP TABLE test; --");
            testString.AppendLine("More SQL: \" OR 1=1; /*");

            // Binary-like data
            testString.AppendLine("Binary-like: \x00\x01\x02\x1A\xFF");

            // End marker
            testString.AppendLine("=== End of Test String ===");

            return testString.ToString();
        }

        private string GetSafeStringPreview(string input, int maxLength = 100)
        {
            if (string.IsNullOrEmpty(input))
                return "(empty)";

            StringBuilder preview = new StringBuilder();
            int count = 0;

            foreach (char c in input)
            {
                if (count >= maxLength)
                {
                    preview.Append("...");
                    break;
                }

                // Replace non-printable characters with readable representations
                switch (c)
                {
                    case '\0': preview.Append("\\0"); break;
                    case '\r': preview.Append("\\r"); break;
                    case '\n': preview.Append("\\n"); break;
                    case '\t': preview.Append("\\t"); break;
                    case '\b': preview.Append("\\b"); break;
                    case '\f': preview.Append("\\f"); break;
                    case '\v': preview.Append("\\v"); break;
                    case '\a': preview.Append("\\a"); break;
                    case '\\': preview.Append("\\\\"); break;
                    case '\'': preview.Append("\\'"); break;
                    case '\"': preview.Append("\\\""); break;
                    default:
                        if (char.IsControl(c) && c > 31)
                            preview.Append($"\\x{(int)c:X2}");
                        else
                            preview.Append(c);
                        break;
                }
                count++;
            }

            return preview.ToString();
        }

        private string CalculateSHA256FromString(string input)
        {
            if (input == null) input = "";

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        private void AnalyzeDifferences(string original, string retrieved)
        {
            sb.AppendLine();
            sb.AppendLine("--- Detailed Difference Analysis ---");

            if (original.Length != retrieved.Length)
            {
                sb.AppendLine($"Length difference: Original={original.Length}, Retrieved={retrieved.Length}");
            }

            int minLength = Math.Min(original.Length, retrieved.Length);
            int differenceCount = 0;

            for (int i = 0; i < minLength && differenceCount < 10; i++)
            {
                if (original[i] != retrieved[i])
                {
                    differenceCount++;
                    sb.AppendLine($"Difference at position {i}: Original='{GetCharDescription(original[i])}' vs Retrieved='{GetCharDescription(retrieved[i])}'");
                }
            }

            if (original.Length != retrieved.Length)
            {
                if (original.Length > retrieved.Length)
                {
                    sb.AppendLine($"Original has extra characters from position {retrieved.Length}");
                }
                else
                {
                    sb.AppendLine($"Retrieved has extra characters from position {original.Length}");
                }
            }

            if (differenceCount == 0 && original.Length == retrieved.Length)
            {
                sb.AppendLine("No character differences found, but SHA256 differs - possible encoding issue?");
            }
        }

        private string GetCharDescription(char c)
        {
            switch (c)
            {
                case '\0':
                    return "NULL(\\0)";
                case '\r':
                    return "CR(\\r)";
                case '\n':
                    return "LF(\\n)";
                case '\t':
                    return "TAB(\\t)";
                case '\b':
                    return "BS(\\b)";
                case '\f':
                    return "FF(\\f)";
                case '\v':
                    return "VT(\\v)";
                case '\a':
                    return "BELL(\\a)";
                case '\\':
                    return "BACKSLASH(\\\\)";
                case '\'':
                    return "SINGLE_QUOTE(\\')";
                case '\"':
                    return "DOUBLE_QUOTE(\\\")";
                default:
                    if (char.IsControl(c))
                        return $"CTRL(\\x{(int)c:X2})";
                    else if (c > 127)
                        return $"UNICODE({c}:U+{(int)c:X4})";
                    else
                        return $"'{c}'";
            }
        }
    }
}