using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class MySqlDateTimeTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btTestMicroseconds_Click(object sender, EventArgs e)
        {
            TimePrecisionTester t = new TimePrecisionTester(config.ConnString);
            string result = t.RunTest();
            ltResult.Text = result;
        }

        protected void btTestFull_Click(object sender, EventArgs e)
        {
            MySqlDateTimeTestHelper t = new MySqlDateTimeTestHelper(config.ConnString);
            string output = t.RunTest();
            ltResult.Text = output;
        }
    }

    public class TimePrecisionTester
    {
        private StringBuilder output;
        private string connectionString;

        public TimePrecisionTester(string connStr = null)
        {
            connectionString = connStr;
            output = new StringBuilder();
        }

        public string RunTest(string connStr = null)
        {
            if (!string.IsNullOrEmpty(connStr))
                connectionString = connStr;

            if (string.IsNullOrEmpty(connectionString))
            {
                return "Error: Connection string not provided";
            }

            output.Clear();

            Random rd = new Random();
            string dbName = $"test_mysqldatetime_{rd.Next(1000,9999)}";

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"USE `{dbName}`";
                    cmd.ExecuteNonQuery();

                    AppendLine("=== SETTING TIMEZONE CONSISTENCY ===");
                    cmd.CommandText = "SET time_zone = '+00:00'";
                    cmd.ExecuteNonQuery();
                    AppendLine("Timezone set to UTC (+00:00)");
                    AppendLine();

                    // Step 1: Create specialized table for TIME precision testing
                    AppendLine("=== CREATING TEST TABLE ===");
                    cmd.CommandText = @"
                    DROP TABLE IF EXISTS time_precision_test;
                    CREATE TABLE time_precision_test (
                        id INT PRIMARY KEY,
                        time_col_0 TIME,
                        time_col_3 TIME(3),
                        time_col_6 TIME(6)
                    ) ENGINE=InnoDB;
                ";
                    cmd.ExecuteNonQuery();
                    AppendLine("Table created successfully");
                    AppendLine();

                    // Step 2: Insert test data with microsecond precision
                    AppendLine("=== INSERTING TEST DATA ===");
                    cmd.CommandText = @"
                    INSERT INTO time_precision_test VALUES 
                    (1, '14:30:45', '14:30:45.123', '14:30:45.123456'),
                    (2, '23:59:59', '23:59:59.999', '23:59:59.999999'),
                    (3, '00:00:00', '00:00:00.000', '00:00:00.000000');
                ";
                    cmd.ExecuteNonQuery();
                    AppendLine("Test data inserted successfully");
                    AppendLine();

                    // Step 3: Use MySqlDataReader to fetch and analyze the data types
                    cmd.CommandText = "SELECT * FROM time_precision_test ORDER BY id";

                    using (var reader = cmd.ExecuteReader())
                    {
                        // Get column schema information
                        var schemaTable = reader.GetSchemaTable();

                        AppendLine("=== COLUMN SCHEMA ANALYSIS ===");
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string columnName = row["ColumnName"].ToString();
                            string dataType = row["DataType"].ToString();
                            string providerType = row["ProviderType"].ToString();

                            AppendLine($"Column: {columnName}");
                            AppendLine($"  .NET Type: {dataType}");
                            AppendLine($"  Provider Type: {providerType}");
                            AppendLine();
                        }

                        AppendLine("=== DATA VALUES ANALYSIS ===");
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            AppendLine($"Row {id}:");

                            for (int i = 1; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                AppendLine($"  {columnName}:");
                                AppendLine($"    Value: {value}");
                                AppendLine($"    Type: {value?.GetType()?.FullName ?? "NULL"}");

                                // Special handling for different types
                                if (value is TimeSpan ts)
                                {
                                    AppendLine($"    TimeSpan Details:");
                                    AppendLine($"      TotalHours: {ts.TotalHours}");
                                    AppendLine($"      Hours: {ts.Hours}");
                                    AppendLine($"      Minutes: {ts.Minutes}");
                                    AppendLine($"      Seconds: {ts.Seconds}");
                                    AppendLine($"      Milliseconds: {ts.Milliseconds}");
                                    AppendLine($"      Ticks: {ts.Ticks}");
                                    AppendLine($"      Microseconds: {ts.Ticks / 10}");
                                    AppendLine($"      Microseconds (mod 1M): {(ts.Ticks / 10) % 1000000}");
                                }
                                else if (value is MySqlDateTime mdt)
                                {
                                    AppendLine($"    MySqlDateTime Details:");
                                    AppendLine($"      Hour: {mdt.Hour}");
                                    AppendLine($"      Minute: {mdt.Minute}");
                                    AppendLine($"      Second: {mdt.Second}");
                                    AppendLine($"      Microsecond: {mdt.Microsecond}");
                                    AppendLine($"      IsValidDateTime: {mdt.IsValidDateTime}");
                                }
                                else if (value is DateTime dt)
                                {
                                    AppendLine($"    DateTime Details:");
                                    AppendLine($"      TimeOfDay: {dt.TimeOfDay}");
                                    AppendLine($"      Ticks: {dt.Ticks}");
                                }

                                AppendLine();
                            }
                            AppendLine("------------------------");
                        }
                    }

                    // Step 4: Test MySqlBackup export behavior
                    AppendLine("=== TESTING MYSQLBACKUP EXPORT ===");

                    // Export the table
                    string exportedSql = mb.ExportToString();
                    AppendLine("Exported SQL:");
                    AppendLine(exportedSql);
                    AppendLine();

                    // Look for INSERT statements specifically
                    string[] lines = exportedSql.Split('\n');
                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("INSERT INTO") && line.Contains("time_precision_test"))
                        {
                            AppendLine("INSERT Statement Found:");
                            AppendLine(line);
                            AppendLine();
                            break;
                        }
                    }

                    // Step 5: Test individual column metadata
                    AppendLine("=== COLUMN METADATA ANALYSIS ===");
                    cmd.CommandText = @"
                    SELECT COLUMN_NAME, DATA_TYPE, DATETIME_PRECISION 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'time_precision_test' 
                    AND TABLE_SCHEMA = DATABASE()
                    ORDER BY ORDINAL_POSITION
                ";

                    using (var metaReader = cmd.ExecuteReader())
                    {
                        while (metaReader.Read())
                        {
                            AppendLine($"Column: {metaReader.GetString(0)}");
                            AppendLine($"  Data Type: {metaReader.GetString(1)}");
                            AppendLine($"  Precision: {(metaReader.IsDBNull(2) ? "NULL" : metaReader.GetValue(2).ToString())}");
                            AppendLine();
                        }
                    }

                    // Step 6: Test field type information
                    AppendLine("=== FIELD TYPE INFORMATION ===");
                    TestFieldTypes(cmd);

                    // Cleanup
                    AppendLine("=== CLEANUP ===");
                    cmd.CommandText = "DROP TABLE time_precision_test";
                    cmd.ExecuteNonQuery();
                    AppendLine("Test table dropped successfully");
                }
            }
            catch (Exception ex)
            {
                AppendLine($"ERROR: {ex.Message}");
                AppendLine($"Stack Trace: {ex.StackTrace}");
            }

            return output.ToString();
        }

        private void TestFieldTypes(MySqlCommand cmd)
        {
            cmd.CommandText = "SELECT time_col_6 FROM time_precision_test WHERE id = 1";

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var fieldType = reader.GetFieldType(0);
                    var dataTypeName = reader.GetDataTypeName(0);

                    AppendLine($"TIME(6) Column Analysis:");
                    AppendLine($"  Field Type: {fieldType}");
                    AppendLine($"  Data Type Name: {dataTypeName}");

                    // Get the actual value and analyze it
                    object value = reader.GetValue(0);
                    AppendLine($"  Actual Value: {value}");
                    AppendLine($"  Actual Type: {value?.GetType()?.FullName}");

                    if (value is TimeSpan ts)
                    {
                        AppendLine($"  TimeSpan Ticks: {ts.Ticks}");
                        AppendLine($"  TimeSpan Microseconds: {ts.Ticks / 10}");
                        AppendLine($"  TimeSpan String: {ts}");
                    }
                }
            }
        }

        private void AppendLine(string text = "")
        {
            output.AppendLine(text);
        }
    }

    public class MySqlDateTimeTestHelper
    {
        private StringBuilder output;
        private string connectionString;

        public MySqlDateTimeTestHelper(string connStr = null)
        {
            connectionString = connStr;
            output = new StringBuilder();
        }

        public string RunTest(string connStr = null)
        {
            if (!string.IsNullOrEmpty(connStr))
                connectionString = connStr;

            if (string.IsNullOrEmpty(connectionString))
            {
                return "Error: Connection string not provided";
            }

            output.Clear();

            Random rd = new Random();
            string dbName = $"test_mysqldatetime_{rd.Next(1000, 9999)}";

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                using (var cmd = conn.CreateCommand())
                using (var mb = new MySqlBackup(cmd))
                {
                    conn.Open();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{dbName}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"USE `{dbName}`";
                    cmd.ExecuteNonQuery();

                    // adjust the session time zone to UTC to export actual value without time zone offset
                    cmd.CommandText = "SET time_zone = '+00:00'";
                    cmd.ExecuteNonQuery();

                    // Step 1: Create comprehensive test table
                    AppendLine("=== CREATING COMPREHENSIVE TEST TABLE ===");
                    cmd.CommandText = @"
                    DROP TABLE IF EXISTS datetime_precision_test;
                    CREATE TABLE datetime_precision_test (
                        id INT PRIMARY KEY,
                        
                        -- DATE columns
                        date_col DATE,
                        date_null DATE,
                        
                        -- TIME columns with different precisions
                        time_col_0 TIME,
                        time_col_3 TIME(3),
                        time_col_6 TIME(6),
                        time_null_0 TIME,
                        time_null_3 TIME(3),
                        time_null_6 TIME(6),
                        
                        -- DATETIME columns with different precisions
                        datetime_col_0 DATETIME,
                        datetime_col_3 DATETIME(3),
                        datetime_col_6 DATETIME(6),
                        datetime_null_0 DATETIME,
                        datetime_null_3 DATETIME(3),
                        datetime_null_6 DATETIME(6),
                        
                        -- TIMESTAMP columns with different precisions
                        timestamp_col_0 TIMESTAMP,
                        timestamp_col_3 TIMESTAMP(3),
                        timestamp_col_6 TIMESTAMP(6),
                        timestamp_null_0 TIMESTAMP NULL,
                        timestamp_null_3 TIMESTAMP(3) NULL,
                        timestamp_null_6 TIMESTAMP(6) NULL
                    ) ENGINE=InnoDB;
                ";
                    cmd.ExecuteNonQuery();
                    AppendLine("Comprehensive test table created successfully");
                    AppendLine();

                    // Step 2: Insert comprehensive test data
                    AppendLine("=== INSERTING COMPREHENSIVE TEST DATA ===");
                    cmd.CommandText = @"
    INSERT INTO datetime_precision_test VALUES 
    (
        1,
        -- DATE columns
        '2024-01-15', NULL,
        
        -- TIME columns (non-null)
        '14:30:45', '14:30:45.123', '14:30:45.123456',
        NULL, NULL, NULL,
        
        -- DATETIME columns (non-null)
        '2024-01-15 14:30:45', '2024-01-15 14:30:45.123', '2024-01-15 14:30:45.123456',
        NULL, NULL, NULL,
        
        -- TIMESTAMP columns (non-null) ✅ SAFE VALUES
        '2024-01-15 14:30:45', '2024-01-15 14:30:45.123', '2024-01-15 14:30:45.123456',
        NULL, NULL, NULL
    ),
    (
        2,
        -- DATE columns
        '2023-12-31', NULL,
        
        -- TIME columns (edge case: 23:59:59)
        '23:59:59', '23:59:59.999', '23:59:59.999999',
        NULL, NULL, NULL,
        
        -- DATETIME columns (edge case: year-end)
        '2023-12-31 23:59:59', '2023-12-31 23:59:59.999', '2023-12-31 23:59:59.999999',
        NULL, NULL, NULL,
        
        -- TIMESTAMP columns (edge case: year-end) ✅ SAFE VALUES
        '2023-12-31 15:59:59', '2023-12-31 15:59:59.999', '2023-12-31 15:59:59.999999',
        NULL, NULL, NULL
    ),
    (
        3,
        -- DATE columns
        '1000-01-01', NULL,
        
        -- TIME columns (edge case: midnight)
        '00:00:00', '00:00:00.000', '00:00:00.000000',
        NULL, NULL, NULL,
        
        -- DATETIME columns (edge case: minimum date)
        '1000-01-01 00:00:00', '1000-01-01 00:00:00.000', '1000-01-01 00:00:00.000000',
        NULL, NULL, NULL,
        
        -- TIMESTAMP columns ✅ SAFE MINIMUM (accounting for +08:00 timezone)
        '1970-01-01 08:30:01', '1970-01-01 08:30:01.000', '1970-01-01 08:30:01.000000',
        NULL, NULL, NULL
    ),
    (
        4,
        -- DATE columns
        '9999-12-31', NULL,
        
        -- TIME columns (edge case: negative time)
        '-838:59:59', '-838:59:59.000', '-838:59:59.000000',
        NULL, NULL, NULL,
        
        -- DATETIME columns (edge case: maximum date)
        '9999-12-31 23:59:59', '9999-12-31 23:59:59.999', '9999-12-31 23:59:59.999999',
        NULL, NULL, NULL,
        
        -- TIMESTAMP columns ✅ SAFE MAXIMUM
        '2038-01-19 02:14:06', '2038-01-19 02:14:06.999', '2038-01-19 02:14:06.999999',
        NULL, NULL, NULL
    );
";
                    cmd.ExecuteNonQuery();
                    AppendLine("Comprehensive test data inserted successfully");
                    AppendLine();

                    // Step 3: Analyze column schema
                    cmd.CommandText = "SELECT * FROM datetime_precision_test ORDER BY id";

                    using (var reader = cmd.ExecuteReader())
                    {
                        var schemaTable = reader.GetSchemaTable();

                        AppendLine("=== COMPREHENSIVE COLUMN SCHEMA ANALYSIS ===");
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string columnName = row["ColumnName"].ToString();
                            string dataType = row["DataType"].ToString();
                            string providerType = row["ProviderType"].ToString();
                            bool allowDBNull = (bool)row["AllowDBNull"];

                            AppendLine($"Column: {columnName}");
                            AppendLine($"  .NET Type: {dataType}");
                            AppendLine($"  Provider Type: {providerType}");
                            AppendLine($"  Allow Null: {allowDBNull}");
                            AppendLine();
                        }

                        AppendLine("=== COMPREHENSIVE DATA VALUES ANALYSIS ===");
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            AppendLine($"=== ROW {id} ANALYSIS ===");

                            for (int i = 1; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);
                                bool isNull = reader.IsDBNull(i);

                                AppendLine($"Column: {columnName}");
                                AppendLine($"  Is Null: {isNull}");

                                if (!isNull)
                                {
                                    AppendLine($"  Value: '{value}'");
                                    AppendLine($"  Type: {value?.GetType()?.FullName}");

                                    // Detailed analysis based on type
                                    if (value is TimeSpan ts)
                                    {
                                        AppendLine($"  TimeSpan Analysis:");
                                        AppendLine($"    TotalHours: {ts.TotalHours}");
                                        AppendLine($"    Hours: {ts.Hours}");
                                        AppendLine($"    Minutes: {ts.Minutes}");
                                        AppendLine($"    Seconds: {ts.Seconds}");
                                        AppendLine($"    Milliseconds: {ts.Milliseconds}");
                                        AppendLine($"    Ticks: {ts.Ticks}");
                                        AppendLine($"    Microseconds: {ts.Ticks / 10}");
                                        AppendLine($"    Microseconds (fractional): {(ts.Ticks / 10) % 1000000}");
                                        AppendLine($"    String representation: {ts}");
                                    }
                                    else if (value is MySqlDateTime mdt)
                                    {
                                        AppendLine($"  MySqlDateTime Analysis:");
                                        AppendLine($"    Year: {mdt.Year}");
                                        AppendLine($"    Month: {mdt.Month}");
                                        AppendLine($"    Day: {mdt.Day}");
                                        AppendLine($"    Hour: {mdt.Hour}");
                                        AppendLine($"    Minute: {mdt.Minute}");
                                        AppendLine($"    Second: {mdt.Second}");
                                        AppendLine($"    Microsecond: {mdt.Microsecond}");
                                        AppendLine($"    IsValidDateTime: {mdt.IsValidDateTime}");
                                        AppendLine($"    String representation: {mdt}");

                                        if (mdt.IsValidDateTime)
                                        {
                                            try
                                            {
                                                DateTime dt = mdt.GetDateTime();
                                                AppendLine($"    As DateTime: {dt}");
                                                AppendLine($"    DateTime Ticks: {dt.Ticks}");
                                            }
                                            catch (Exception ex)
                                            {
                                                AppendLine($"    GetDateTime() Error: {ex.Message}");
                                            }
                                        }
                                    }
                                    else if (value is DateTime dt)
                                    {
                                        AppendLine($"  DateTime Analysis:");
                                        AppendLine($"    Date: {dt.Date}");
                                        AppendLine($"    TimeOfDay: {dt.TimeOfDay}");
                                        AppendLine($"    Ticks: {dt.Ticks}");
                                        AppendLine($"    Millisecond: {dt.Millisecond}");
                                        AppendLine($"    String representation: {dt}");
                                        AppendLine($"    ISO format: {dt:yyyy-MM-dd HH:mm:ss.ffffff}");
                                    }
                                    else
                                    {
                                        AppendLine($"  Generic Value Analysis:");
                                        AppendLine($"    ToString(): {value}");
                                    }
                                }
                                else
                                {
                                    AppendLine($"  Value: NULL");
                                }

                                AppendLine();
                            }
                            AppendLine("=" + new string('=', 50));
                        }
                    }

                    // Step 4: Test MySqlBackup export behavior
                    AppendLine("=== TESTING MYSQLBACKUP EXPORT BEHAVIOR ===");

                    string exportedSql = mb.ExportToString();

                    AppendLine("Looking for INSERT statements...");
                    string[] lines = exportedSql.Split('\n');
                    bool foundInsert = false;

                    foreach (string line in lines)
                    {
                        if (line.Trim().StartsWith("INSERT INTO") && line.Contains("datetime_precision_test"))
                        {
                            AppendLine("INSERT Statement Found:");
                            AppendLine(line);
                            AppendLine();
                            foundInsert = true;
                        }
                    }

                    if (!foundInsert)
                    {
                        AppendLine("No INSERT statements found. Full export:");
                        AppendLine(exportedSql);
                    }

                    // Step 5: Column metadata analysis
                    AppendLine("=== COLUMN METADATA FROM INFORMATION_SCHEMA ===");
                    cmd.CommandText = @"
                    SELECT 
                        COLUMN_NAME, 
                        DATA_TYPE, 
                        DATETIME_PRECISION,
                        IS_NULLABLE,
                        COLUMN_DEFAULT,
                        COLUMN_TYPE
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = 'datetime_precision_test' 
                    AND TABLE_SCHEMA = DATABASE()
                    ORDER BY ORDINAL_POSITION
                ";

                    using (var metaReader = cmd.ExecuteReader())
                    {
                        while (metaReader.Read())
                        {
                            AppendLine($"Column: {metaReader.GetString("COLUMN_NAME")}");
                            AppendLine($"  Data Type: {metaReader.GetString("DATA_TYPE")}");
                            AppendLine($"  Column Type: {metaReader.GetString("COLUMN_TYPE")}");

                            // ✅ FIX: Use column names directly with proper null checking
                            string precision = metaReader["DATETIME_PRECISION"] == DBNull.Value ? "NULL" : metaReader["DATETIME_PRECISION"].ToString();
                            string nullable = metaReader["IS_NULLABLE"] == DBNull.Value ? "NULL" : metaReader["IS_NULLABLE"].ToString();
                            string defaultVal = metaReader["COLUMN_DEFAULT"] == DBNull.Value ? "NULL" : metaReader["COLUMN_DEFAULT"].ToString();

                            AppendLine($"  Precision: {precision}");
                            AppendLine($"  Nullable: {nullable}");
                            AppendLine($"  Default: {defaultVal}");
                            AppendLine();
                        }
                    }

                    // Step 6: Test individual data type handling
                    AppendLine("=== INDIVIDUAL TYPE TESTING ===");
                    TestSpecificTypes(cmd);

                    // Step 7: Test edge cases
                    AppendLine("=== EDGE CASE TESTING ===");
                    TestEdgeCases(cmd);

                    // Cleanup
                    AppendLine("=== CLEANUP ===");
                    cmd.CommandText = "DROP TABLE datetime_precision_test";
                    cmd.ExecuteNonQuery();
                    AppendLine("Test table dropped successfully");
                }
            }
            catch (Exception ex)
            {
                AppendLine($"ERROR: {ex.Message}");
            }

            return output.ToString();
        }

        private void TestSpecificTypes(MySqlCommand cmd)
        {
            // Test TIME(6) specifically
            AppendLine("--- TIME(6) Specific Test ---");
            cmd.CommandText = "SELECT time_col_6 FROM datetime_precision_test WHERE id = 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var fieldType = reader.GetFieldType(0);
                    var dataTypeName = reader.GetDataTypeName(0);
                    object value = reader.GetValue(0);

                    AppendLine($"TIME(6) Field Analysis:");
                    AppendLine($"  Field Type: {fieldType}");
                    AppendLine($"  Data Type Name: {dataTypeName}");
                    AppendLine($"  Value: {value}");
                    AppendLine($"  .NET Type: {value?.GetType()?.FullName}");

                    if (value is TimeSpan ts)
                    {
                        AppendLine($"  TimeSpan Details:");
                        AppendLine($"    Original: {ts}");
                        AppendLine($"    Ticks: {ts.Ticks}");
                        AppendLine($"    Microseconds: {ts.Ticks / 10}");
                        AppendLine($"    Fractional Microseconds: {(ts.Ticks / 10) % 1000000}");
                    }
                }
            }
            AppendLine();

            // Test DATETIME(6) specifically
            AppendLine("--- DATETIME(6) Specific Test ---");
            cmd.CommandText = "SELECT datetime_col_6 FROM datetime_precision_test WHERE id = 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    var fieldType = reader.GetFieldType(0);
                    var dataTypeName = reader.GetDataTypeName(0);
                    object value = reader.GetValue(0);

                    AppendLine($"DATETIME(6) Field Analysis:");
                    AppendLine($"  Field Type: {fieldType}");
                    AppendLine($"  Data Type Name: {dataTypeName}");
                    AppendLine($"  Value: {value}");
                    AppendLine($"  .NET Type: {value?.GetType()?.FullName}");

                    if (value is DateTime dt)
                    {
                        AppendLine($"  DateTime Details:");
                        AppendLine($"    Original: {dt}");
                        AppendLine($"    Ticks: {dt.Ticks}");
                        AppendLine($"    Microseconds: {dt.Ticks / 10}");
                    }
                    else if (value is MySqlDateTime mdt)
                    {
                        AppendLine($"  MySqlDateTime Details:");
                        AppendLine($"    Microsecond: {mdt.Microsecond}");
                    }
                }
            }
            AppendLine();
        }

        private void TestEdgeCases(MySqlCommand cmd)
        {
            // Test NULL values
            AppendLine("--- NULL Values Test ---");
            cmd.CommandText = "SELECT time_null_6, datetime_null_6 FROM datetime_precision_test WHERE id = 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    AppendLine($"NULL TIME(6): IsDBNull = {reader.IsDBNull(0)}, Value = {(reader.IsDBNull(0) ? "NULL" : reader.GetValue(0))}");
                    AppendLine($"NULL DATETIME(6): IsDBNull = {reader.IsDBNull(1)}, Value = {(reader.IsDBNull(1) ? "NULL" : reader.GetValue(1))}");
                }
            }
            AppendLine();

            // Test negative TIME values
            AppendLine("--- Negative TIME Values Test ---");
            cmd.CommandText = "SELECT time_col_6 FROM datetime_precision_test WHERE id = 4";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    object value = reader.GetValue(0);
                    AppendLine($"Negative TIME Value: {value}");
                    AppendLine($"Type: {value?.GetType()?.FullName}");

                    if (value is TimeSpan ts)
                    {
                        AppendLine($"TimeSpan: {ts}");
                        AppendLine($"TotalHours: {ts.TotalHours}");
                        AppendLine($"IsNegative: {ts < TimeSpan.Zero}");
                    }
                }
            }
            AppendLine();

            // Test precision variations
            AppendLine("--- Precision Variations Test ---");
            cmd.CommandText = "SELECT time_col_0, time_col_3, time_col_6 FROM datetime_precision_test WHERE id = 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    for (int i = 0; i < 3; i++)
                    {
                        object value = reader.GetValue(i);
                        string colName = reader.GetName(i);
                        AppendLine($"{colName}: {value} (Type: {value?.GetType()?.Name})");

                        if (value is TimeSpan ts)
                        {
                            AppendLine($"  Microseconds: {(ts.Ticks / 10) % 1000000}");
                        }
                    }
                }
            }
        }

        private void AppendLine(string text = "")
        {
            output.AppendLine(text);
        }
    }
}