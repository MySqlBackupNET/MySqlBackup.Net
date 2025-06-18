using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MySqlConnector;

namespace System
{
    public class Test1Basic
    {
        private readonly string _connectionString;
        private readonly string _sampleSqlDumpFile;
        private readonly string _dbFile1;
        private readonly string _dbFile2;
        private readonly string _dbFile3;
        private readonly string _dbName1;
        private readonly string _dbName2;
        private readonly string _dbName3;

        public Test1Basic(string connstr, string baseFolder)
        {
            _connectionString = connstr;
            string basePath = baseFolder;

            _dbFile1 = Path.Combine(basePath, "sqldump1.sql");
            _dbFile2 = Path.Combine(basePath, "sqldump2.sql");
            _dbFile3 = Path.Combine(basePath, "sqldump3.sql");

            // Generate random database names
            Random random = new Random();
            int rd = random.Next(1000, 9999); // random 4 digits
            string databaseBaseName = $"test{rd}";
            _dbName1 = $"{databaseBaseName}1";
            _dbName2 = $"{databaseBaseName}2";
            _dbName3 = $"{databaseBaseName}3";

            string[] files = { _dbFile1, _dbFile2, _dbFile3 };

            foreach (var f in files)
            {
                try
                {
                    if (File.Exists(f))
                    {
                        File.Delete(f);
                    }
                }
                catch { }
            }

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"drop database if exists {_dbName1};";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"drop database if exists {_dbName2};";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"drop database if exists {_dbName3};";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string RunTest()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Test1Basic: Starting basic functionality test...");
            sb.AppendLine("=========================================");
            sb.AppendLine($"Database 1: {_dbName1}");
            sb.AppendLine($"Database 2: {_dbName2}");
            sb.AppendLine($"Database 3: {_dbName3}");

            // Step 1: Drop and create test1 database
            sb.AppendLine();
            sb.AppendLine($"Step 1: Setting up {_dbName1} database");
            DropAndCreateDatabase(_dbName1);

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();

                    cmd.CommandText = $"use {_dbName1}";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromString(GetDumpSample());
                    }
                }
            }

            sb.AppendLine($"Imported sample SQL dump to {_dbName1}");

            // Step 2: Export test1 to file1
            sb.AppendLine();
            sb.AppendLine($"Step 2: Exporting {_dbName1} to file1");
            ExportToFile(_dbName1, _dbFile1);
            sb.AppendLine($"Exported to: {_dbFile1}");

            // Step 3: Drop and create test2, import from file1
            sb.AppendLine();
            sb.AppendLine($"Step 3: Setting up {_dbName2} database");
            DropAndCreateDatabase(_dbName2);
            ImportFromFile(_dbName2, _dbFile1);
            sb.AppendLine($"Imported file1 to {_dbName2}");

            // Step 4: Export test2 to file2
            sb.AppendLine();
            sb.AppendLine($"Step 4: Exporting {_dbName2} to file2");
            ExportToFile(_dbName2, _dbFile2);
            sb.AppendLine($"Exported to: {_dbFile2}");

            // Step 5: Drop and create test3, import from file2
            sb.AppendLine();
            sb.AppendLine($"Step 5: Setting up {_dbName3} database");
            DropAndCreateDatabase(_dbName3);
            ImportFromFile(_dbName3, _dbFile2);
            sb.AppendLine($"Imported file2 to {_dbName3}");

            // Step 6: Export test3 to file3
            sb.AppendLine();
            sb.AppendLine($"Step 6: Exporting {_dbName3} to file3");
            ExportToFile(_dbName3, _dbFile3);
            sb.AppendLine($"Exported to: {_dbFile3}");

            // Step 7: Calculate SHA256 checksums
            sb.AppendLine();
            sb.AppendLine("Step 7: Calculating SHA256 checksums");
            string sha1 = CalculateSHA256(_dbFile1);
            string sha2 = CalculateSHA256(_dbFile2);
            string sha3 = CalculateSHA256(_dbFile3);

            sb.AppendLine($"SHA256 of file1: {sha1}");
            sb.AppendLine($"SHA256 of file2: {sha2}");
            sb.AppendLine($"SHA256 of file3: {sha3}");

            // Step 8: Compare checksums
            sb.AppendLine();
            sb.AppendLine("Step 8: Comparing checksums");
            bool isMatch = sha2.Equals(sha3, StringComparison.OrdinalIgnoreCase);

            sb.AppendLine($"File2 SHA256: {sha2}");
            sb.AppendLine($"File3 SHA256: {sha3}");
            sb.AppendLine();
            sb.AppendLine($"Match result: {(isMatch ? "MATCH" : "NOT MATCH")}");

            // Final result
            sb.AppendLine("=========================================");
            sb.AppendLine();
            if (isMatch)
            {
                sb.AppendLine("SUCCESS: Backup and restore test passed!");
                sb.AppendLine("The tool maintains data integrity through multiple backup/restore cycles.");
            }
            else
            {
                sb.AppendLine("FAILURE: Backup and restore test failed!");
                sb.AppendLine("The SHA256 checksums do not match.");
            }

            // Cleanup databases
            sb.AppendLine();
            sb.AppendLine("Cleaning up test databases...");
            CleanupDatabases();
            sb.AppendLine("Cleanup completed");

            return sb.ToString();
        }

        private void DropAndCreateDatabase(string databaseName)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    // Drop if exists
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{databaseName}`";
                    cmd.ExecuteNonQuery();

                    // Create new database
                    cmd.CommandText = $"CREATE DATABASE `{databaseName}`";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void ImportFromFile(string databaseName, string filePath)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"USE `{databaseName}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ImportFromFile(filePath);
                    }
                }
            }
        }

        private void ExportToFile(string databaseName, string filePath)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"USE `{databaseName}`";
                    cmd.ExecuteNonQuery();

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        mb.ExportInfo.RecordDumpTime = false;
                        mb.ExportToFile(filePath);
                    }
                }
            }
        }

        private string CalculateSHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }

        private void CleanupDatabases()
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{_dbName1}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{_dbName2}`";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DROP DATABASE IF EXISTS `{_dbName3}`";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetDumpSample()
        {
            string sql = @"
-- Table 1: Numeric and String Types
CREATE TABLE test_table_1 (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY NOT NULL,
    tiny_int_col TINYINT,
    small_int_col SMALLINT,
    medium_int_col MEDIUMINT,
    int_col INT,
    big_int_col BIGINT,
    decimal_col DECIMAL(10,2),
    numeric_col NUMERIC(8,4),
    float_col FLOAT,
    double_col DOUBLE,
    bit_col BIT(8),
    char_col CHAR(10),
    varchar_col VARCHAR(255),
    binary_col BINARY(16),
    varbinary_col VARBINARY(255),
    tinytext_col TINYTEXT,
    text_col TEXT,
    mediumtext_col MEDIUMTEXT,
    longtext_col LONGTEXT,
    enum_col ENUM('small', 'medium', 'large'),
    set_col SET('read', 'write', 'execute'),
    INDEX idx_varchar (varchar_col),
    UNIQUE KEY uk_int (int_col),
    FULLTEXT KEY ft_text (text_col)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table 2: Date/Time and Binary Types
CREATE TABLE test_table_2 (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY NOT NULL,
    date_col DATE,
    time_col TIME(6),
    datetime_col DATETIME(6),
    timestamp_col TIMESTAMP(6) DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    year_col YEAR,
    tinyblob_col TINYBLOB,
    blob_col BLOB,
    mediumblob_col MEDIUMBLOB,
    longblob_col LONGBLOB,
    json_col JSON,
    point_col POINT NOT NULL,
    linestring_col LINESTRING,
    polygon_col POLYGON,
    geometry_col GEOMETRY,
    table1_id INT UNSIGNED,
    CONSTRAINT fk_table1 FOREIGN KEY (table1_id) REFERENCES test_table_1(id) ON DELETE CASCADE ON UPDATE CASCADE,
    SPATIAL KEY sp_point (point_col)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Table 3: Special types and features
CREATE TABLE test_table_3 (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY NOT NULL,
    boolean_col BOOLEAN,
    price DECIMAL(10,2),
    tax_rate DECIMAL(4,2),
    total_price DECIMAL(10,2) AS (price * (1 + tax_rate)) STORED,
    description VARCHAR(100),
    description_upper VARCHAR(100) AS (UPPER(description)) VIRTUAL,
    uuid_col CHAR(36),
    ip_address VARCHAR(45),
    age INT CHECK (age >= 0 AND age <= 150),
    internal_notes TEXT INVISIBLE,
    table2_id INT UNSIGNED,
    CONSTRAINT fk_table2 FOREIGN KEY (table2_id) REFERENCES test_table_2(id),
    INDEX idx_composite (price, tax_rate)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- CORRECTED: Insert sample data for Table 1 (removed extra values)
INSERT INTO test_table_1 (    id, tiny_int_col, small_int_col, medium_int_col, int_col, big_int_col,    decimal_col, numeric_col, float_col, double_col, bit_col,    char_col, varchar_col, binary_col, varbinary_col, tinytext_col,    text_col, mediumtext_col, longtext_col, enum_col, set_col) VALUES
(1, -128, -32768, -8388608, -2147483648, -9223372036854775808, 12345.67, 1234.5678, 3.14159, 2.718281828, b'10101010', 'Fixed', 'Variable length string', 0x48656C6C6F, 0x576F726C64, 'Tiny text', 'Regular text content', 'Medium text content here', 'Long text content can store up to 4GB', 'medium', 'read,write'),
(2, 0, 0, 0, 0, 0, 0.00, 0.0000, 0.0, 0.0, b'00000000', 'Zeros', 'All zeros test', 0x00000000, 0x00, 'Empty tiny', 'Empty regular', 'Empty medium', 'Empty long', 'small', 'execute'),
(3, 127, 32767, 8388607, 2147483647, 9223372036854775807, -99999.99, -9999.9999, -1.0, -999.999999, b'11111111', 'Max vals', 'Maximum values test', 0xFFFFFFFF, 0xFFFF, 'Max tiny text', 'Max regular text', 'Max medium text', 'Max long text', 'large', 'read,write,execute');

-- Insert sample data for Table 2 (this was correct)
INSERT INTO test_table_2 (    id, date_col, time_col, datetime_col, timestamp_col, year_col,    tinyblob_col, blob_col, mediumblob_col, longblob_col, json_col,    point_col, linestring_col, polygon_col, geometry_col, table1_id) VALUES
(1, '2024-01-15', '14:30:45.123456', '2024-01-15 14:30:45.123456', '2024-01-15 14:30:45.123456', 2024, 0x54696E79, 0x426C6F62, 0x4D656469756D426C6F62, 0x4C6F6E67426C6F62, '{""name"": ""Test"", ""value"": 123, ""nested"": {""key"": ""value""}}', ST_GeomFromText('POINT(1 1)'), ST_GeomFromText('LINESTRING(0 0, 1 1, 2 2)'),  ST_GeomFromText('POLYGON((0 0, 4 0, 4 4, 0 4, 0 0))'), ST_GeomFromText('POINT(5 5)'), 1),
(2, '2023-12-31', '23:59:59.999999', '2023-12-31 23:59:59.999999', '2023-12-31 23:59:59.999999', 2023, 0x41, 0x4242, 0x434343, 0x44444444, '{""array"": [1, 2, 3], ""boolean"": true, ""null"": null}', ST_GeomFromText('POINT(10 20)'), ST_GeomFromText('LINESTRING(0 0, 10 10)'),  ST_GeomFromText('POLYGON((0 0, 10 0, 10 10, 0 10, 0 0))'), ST_GeomFromText('POINT(15 15)'), 2),
(3, '2025-06-18', '00:00:00.000000', '2025-06-18 00:00:00.000000', CURRENT_TIMESTAMP, 2025, NULL, NULL, NULL, NULL, '{""empty"": {}, ""array"": [], ""unicode"": ""Hello 世界 🌍""}', ST_GeomFromText('POINT(-73.935242 40.730610)'), ST_GeomFromText('LINESTRING(-73 40, -74 41)'),  ST_GeomFromText('POLYGON((-73 40, -73 41, -74 41, -74 40, -73 40))'), ST_GeomFromText('POINT(0 0)'), 3);

-- Insert sample data for Table 3 (this was correct - uses explicit column list)
INSERT INTO test_table_3 (id, boolean_col, price, tax_rate, description, uuid_col, ip_address, age, internal_notes, table2_id) VALUES
(1, TRUE, 100.00, 0.10, 'Product A', '550e8400-e29b-41d4-a716-446655440000', '192.168.1.1', 25, 'Internal note 1', 1),
(2, FALSE, 250.50, 0.08, 'Product B', 'f47ac10b-58cc-4372-a567-0e02b2c3d479', '2001:0db8:85a3:0000:0000:8a2e:0370:7334', 30, 'Internal note 2', 2),
(3, NULL, 999.99, 0.15, 'Product C', UUID(), '10.0.0.1', 45, 'Internal note 3', 3);


-- Additional database objects for completeness

-- Character set and collation test
CREATE TABLE test_charset (
    id INT PRIMARY KEY,
    utf8_col VARCHAR(100) CHARACTER SET utf8 COLLATE utf8_general_ci,
    utf8mb4_col VARCHAR(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
    latin1_col VARCHAR(100) CHARACTER SET latin1 COLLATE latin1_swedish_ci
) ENGINE=InnoDB;

-- Partitioned table (MySQL 5.1+)
CREATE TABLE test_partitioned (
    id INT NOT NULL,
    created_date DATE NOT NULL,
    data VARCHAR(100),
    PRIMARY KEY (id, created_date)
) ENGINE=InnoDB
PARTITION BY RANGE (YEAR(created_date)) (
    PARTITION p2023 VALUES LESS THAN (2024),
    PARTITION p2024 VALUES LESS THAN (2025),
    PARTITION p2025 VALUES LESS THAN (2026),
    PARTITION pmax VALUES LESS THAN MAXVALUE
);

-- Insert some data into additional tables
INSERT INTO test_charset VALUES 
(1, 'UTF8 Text', 'UTF8MB4 Text with Emoji 😊', 'Latin1 Text');

INSERT INTO test_partitioned VALUES 
(1, '2023-06-15', 'Data from 2023'),
(2, '2024-06-15', 'Data from 2024'),
(3, '2025-06-15', 'Data from 2025');

" + GetDumpFunctionOnly();

            return sql;
        }

        public string GetDumpFunctionOnly()
        {
            string sql = @"
-- Stored Procedures
DELIMITER ||

CREATE PROCEDURE sp_get_table_stats()
BEGIN
    SELECT 'test_table_1' as table_name, COUNT(*) as row_count FROM test_table_1
    UNION ALL
    SELECT 'test_table_2', COUNT(*) FROM test_table_2
    UNION ALL
    SELECT 'test_table_3', COUNT(*) FROM test_table_3;
END||

CREATE PROCEDURE sp_clean_old_data(IN days_old INT)
BEGIN
    DELETE FROM test_table_2 
    WHERE 1=2 and 3=4 or 5=6 or 9=10;
    
    SELECT ROW_COUNT() as deleted_rows;
END||

-- Functions
CREATE FUNCTION fn_calculate_tax(price DECIMAL(10,2), tax_rate DECIMAL(4,2))
RETURNS DECIMAL(10,2)
DETERMINISTIC
BEGIN
    RETURN price * tax_rate;
END||

CREATE FUNCTION fn_format_json_name(json_data JSON)
RETURNS VARCHAR(255)
DETERMINISTIC
BEGIN
    DECLARE name_value VARCHAR(255);
    SET name_value = JSON_UNQUOTE(JSON_EXTRACT(json_data, '$.name'));
    RETURN COALESCE(name_value, 'Unknown');
END||

-- Triggers
CREATE TRIGGER trg_before_insert_table1
BEFORE INSERT ON test_table_1
FOR EACH ROW
BEGIN
    IF NEW.varchar_col IS NULL AND 1=2 THEN
        SET NEW.varchar_col = 'Default Value';
    END IF;
END||

CREATE TRIGGER trg_after_update_table3
AFTER UPDATE ON test_table_3
FOR EACH ROW
BEGIN
    INSERT INTO test_table_1 (tiny_int_col, varchar_col, enum_col, set_col)
    VALUES (1, CONCAT('Updated: ', NEW.description), 'small', 'write');
END||

DELIMITER ;

-- Views
CREATE VIEW v_numeric_summary AS
SELECT 
    id,
    tiny_int_col,
    small_int_col,
    medium_int_col,
    int_col,
    big_int_col,
    decimal_col,
    float_col,
    double_col
FROM test_table_1
WHERE decimal_col > 0;

CREATE VIEW v_recent_data AS
SELECT 
    t2.id,
    t2.datetime_col,
    t2.json_col,
    t3.description,
    t3.total_price
FROM test_table_2 t2
LEFT JOIN test_table_3 t3 ON t2.id = t3.table2_id
WHERE t2.datetime_col >= DATE_SUB(NOW(), INTERVAL 1 YEAR);

-- Events (requires event scheduler to be enabled)
DELIMITER ||

CREATE EVENT evt_daily_cleanup
ON SCHEDULE EVERY 1 DAY
STARTS CURRENT_TIMESTAMP + INTERVAL 1 DAY
COMMENT 'Clean up old data daily'
DO
BEGIN
    CALL sp_clean_old_data(365);
END||

CREATE EVENT evt_hourly_stats
ON SCHEDULE EVERY 1 HOUR
STARTS CURRENT_TIMESTAMP
ENDS CURRENT_TIMESTAMP + INTERVAL 1 YEAR
COMMENT 'Collect statistics every hour without modifying data'
DO
BEGIN
    SELECT HOUR(NOW()) AS current_hour, 'Hourly stat check' AS message;
END||

DELIMITER ;
";

            return sql;
        }
    }
}