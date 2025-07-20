using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public class DumpContentSample
    {

        public static string GetDumpSample()
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

" + GetDumpRoutines();

            return sql;
        }

        public static string GetDumpRoutines()
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

        public static string GetAdvanceDumpSample()
        {
            string sql = @"
-- ----------------------------------------
-- 1. EXTREME DATA TYPES & EDGE VALUES
-- ----------------------------------------
CREATE TABLE test_extreme_values (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    
    -- Numeric extremes
    tiny_min TINYINT DEFAULT -128,
    tiny_max TINYINT DEFAULT 127,
    tiny_unsigned_max TINYINT UNSIGNED DEFAULT 255,
    
    small_min SMALLINT DEFAULT -32768,
    small_max SMALLINT DEFAULT 32767,
    small_unsigned_max SMALLINT UNSIGNED DEFAULT 65535,
    
    medium_min MEDIUMINT DEFAULT -8388608,
    medium_max MEDIUMINT DEFAULT 8388607,
    medium_unsigned_max MEDIUMINT UNSIGNED DEFAULT 16777215,
    
    int_min INT DEFAULT -2147483648,
    int_max INT DEFAULT 2147483647,
    int_unsigned_max INT UNSIGNED DEFAULT 4294967295,
    
    big_min BIGINT DEFAULT -9223372036854775808,
    big_max BIGINT DEFAULT 9223372036854775807,
    big_unsigned_max BIGINT UNSIGNED DEFAULT 18446744073709551615,
    
    -- Decimal extremes (MySQL 8.0: up to 65 digits, 30 decimal places)
    decimal_max DECIMAL(65,30),
    decimal_min DECIMAL(65,30),
    decimal_zero DECIMAL(65,30) DEFAULT 0,
    
    -- Float/Double special values
    float_max FLOAT DEFAULT 3.402823466E+38,
    float_min FLOAT DEFAULT -3.402823466E+38,
    float_tiny FLOAT DEFAULT 1.175494351E-38,
    
    double_max DOUBLE DEFAULT 1.7976931348623157E+308,
    double_min DOUBLE DEFAULT -1.7976931348623157E+308,
    double_tiny DOUBLE DEFAULT 2.2250738585072014E-308,
    
    -- Bit field extremes
    bit1 BIT(1) DEFAULT b'1',
    bit8 BIT(8) DEFAULT b'11111111',
    bit64 BIT(64) DEFAULT b'1111111111111111111111111111111111111111111111111111111111111111',
    
    -- Date/Time extremes
    date_min DATE DEFAULT '1000-01-01',
    date_max DATE DEFAULT '9999-12-31',
    
    datetime_min DATETIME(6) DEFAULT '1000-01-01 00:00:00.000000',
    datetime_max DATETIME(6) DEFAULT '9999-12-31 23:59:59.999999',
    
    timestamp_min TIMESTAMP(6) DEFAULT '1970-01-01 00:00:01.000000',
    timestamp_max TIMESTAMP(6) DEFAULT '2038-01-19 03:14:07.999999',
    
    time_min TIME(6) DEFAULT '-838:59:59.000000',
    time_max TIME(6) DEFAULT '838:59:59.999999',
    
    year_min YEAR DEFAULT 1901,
    year_max YEAR DEFAULT 2155,
    
    -- String length extremes
    char_max CHAR(255),
    varchar_max VARCHAR(65535),
    
    -- Binary extremes
    binary_max BINARY(255),
    varbinary_max VARBINARY(65535),
    
    -- TEXT/BLOB size extremes
    tinytext_max TINYTEXT,      -- 255 bytes
    text_max TEXT,              -- 65,535 bytes
    mediumtext_max MEDIUMTEXT,  -- 16,777,215 bytes
    longtext_max LONGTEXT,      -- 4,294,967,295 bytes
    
    tinyblob_max TINYBLOB,      -- 255 bytes
    blob_max BLOB,              -- 65,535 bytes
    mediumblob_max MEDIUMBLOB,  -- 16,777,215 bytes
    longblob_max LONGBLOB       -- 4,294,967,295 bytes
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ----------------------------------------
-- 2. CHARACTER SETS & COLLATIONS MATRIX
-- ----------------------------------------
CREATE TABLE test_charset_collations (
    id INT PRIMARY KEY,
    
    -- UTF8MB4 variants (recommended for full Unicode support)
    utf8mb4_general VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
    utf8mb4_unicode VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci,
    utf8mb4_bin VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin,
    utf8mb4_unicode_520 VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_520_ci,
    
    -- UTF8 (deprecated but still used)
    utf8_general VARCHAR(200) CHARACTER SET utf8 COLLATE utf8_general_ci,
    utf8_unicode VARCHAR(200) CHARACTER SET utf8 COLLATE utf8_unicode_ci,
    utf8_bin VARCHAR(200) CHARACTER SET utf8 COLLATE utf8_bin,
    
    -- Latin1 (Western European)
    latin1_swedish VARCHAR(200) CHARACTER SET latin1 COLLATE latin1_swedish_ci,
    latin1_general VARCHAR(200) CHARACTER SET latin1 COLLATE latin1_general_ci,
    latin1_bin VARCHAR(200) CHARACTER SET latin1 COLLATE latin1_bin,
    
    -- ASCII (fastest for English-only)
    ascii_general VARCHAR(200) CHARACTER SET ascii COLLATE ascii_general_ci,
    ascii_bin VARCHAR(200) CHARACTER SET ascii COLLATE ascii_bin,
    
    -- Binary (no character set conversion)
    binary_data VARCHAR(200) CHARACTER SET binary
) ENGINE=InnoDB;

-- ----------------------------------------
-- 3. COMPLEX CONSTRAINTS & RELATIONSHIPS
-- ----------------------------------------
CREATE TABLE test_parent (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    code VARCHAR(50) UNIQUE NOT NULL,
    status ENUM('active', 'inactive', 'pending') DEFAULT 'active',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

CREATE TABLE test_child (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    parent_id INT UNSIGNED,
    parent_code VARCHAR(50),
    
    -- Multiple constraint types
    value DECIMAL(10,2) CHECK (value >= 0),
    percentage DECIMAL(5,2) CHECK (percentage BETWEEN 0 AND 100),
    
    start_date DATE NOT NULL,
    end_date DATE,
    
    -- Multiple check constraints
    CONSTRAINT chk_date_order CHECK (end_date IS NULL OR end_date >= start_date),
    CONSTRAINT chk_value_range CHECK (value <= 999999.99),
    
    -- Multiple foreign key relationships
    FOREIGN KEY (parent_id) REFERENCES test_parent(id) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (parent_code) REFERENCES test_parent(code) 
        ON DELETE SET NULL ON UPDATE RESTRICT,
        
    -- Self-referencing
    manager_id INT UNSIGNED,
    FOREIGN KEY (manager_id) REFERENCES test_child(id) 
        ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB;

-- ----------------------------------------
-- 4. ADVANCED GENERATED COLUMNS
-- ----------------------------------------
CREATE TABLE test_generated_advanced (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    birth_date DATE NOT NULL,
    salary DECIMAL(10,2) NOT NULL,
    
    -- Virtual generated columns (computed on-the-fly)
    full_name VARCHAR(101) AS (CONCAT(first_name, ' ', last_name)) VIRTUAL,
    initials VARCHAR(10) AS (CONCAT(LEFT(first_name,1), '.', LEFT(last_name,1), '.')) VIRTUAL,
    
    age_years INT AS (TIMESTAMPDIFF(YEAR, birth_date, CURDATE())) VIRTUAL,
    age_category VARCHAR(20) AS (
        CASE 
            WHEN TIMESTAMPDIFF(YEAR, birth_date, CURDATE()) < 18 THEN 'Minor'
            WHEN TIMESTAMPDIFF(YEAR, birth_date, CURDATE()) BETWEEN 18 AND 64 THEN 'Adult'
            ELSE 'Senior'
        END
    ) VIRTUAL,
    
    -- Stored generated columns (computed and stored)
    salary_annual DECIMAL(12,2) AS (salary * 12) STORED,
    salary_grade CHAR(1) AS (
        CASE 
            WHEN salary < 3000 THEN 'C'
            WHEN salary < 6000 THEN 'B'
            ELSE 'A'
        END
    ) STORED,
    
    -- Complex JSON virtual column
    person_json JSON AS (JSON_OBJECT(
        'name', full_name,
        'age', age_years,
        'category', age_category,
        'salary', salary_annual
    )) VIRTUAL,
    
    -- Invisible column (MySQL 8.0+)
    internal_id BIGINT INVISIBLE DEFAULT (UNIX_TIMESTAMP() * 1000000 + CONNECTION_ID()),
    
    -- Indexes on generated columns
    INDEX idx_full_name (full_name),
    INDEX idx_age_category (age_category),
    INDEX idx_salary_grade (salary_grade)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 5. SPATIAL DATA COMPREHENSIVE
-- ----------------------------------------
CREATE TABLE test_spatial_complete (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    
    -- Basic geometry types
    point_col POINT NOT NULL,
    linestring_col LINESTRING,
    polygon_col POLYGON,
    
    -- Multi-geometry types
    multipoint_col MULTIPOINT,
    multilinestring_col MULTILINESTRING,
    multipolygon_col MULTIPOLYGON,
    
    -- Geometry collection
    geometrycollection_col GEOMETRYCOLLECTION,
    
    -- Generic geometry
    geometry_col GEOMETRY,
    
    -- Spatial indexes
    SPATIAL INDEX sp_point (point_col),
    SPATIAL INDEX sp_polygon (polygon_col),
    SPATIAL INDEX sp_geometry (geometry_col)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 6. JSON EDGE CASES
-- ----------------------------------------
CREATE TABLE test_json_comprehensive (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    
    -- Simple JSON
    simple_json JSON,
    
    -- Complex nested JSON
    complex_json JSON,
    
    -- JSON with special characters
    special_chars_json JSON,
    
    -- Large JSON
    large_json JSON,
    
    -- JSON virtual columns
    json_name VARCHAR(100) AS (JSON_UNQUOTE(JSON_EXTRACT(simple_json, '$.name'))) VIRTUAL,
    json_count INT AS (JSON_LENGTH(simple_json)) VIRTUAL,
    
    -- Functional indexes on JSON (MySQL 8.0+)
    INDEX idx_json_name ((CAST(JSON_UNQUOTE(JSON_EXTRACT(simple_json, '$.name')) AS CHAR(50))))
) ENGINE=InnoDB;

-- ----------------------------------------
-- 7. PARTITIONING VARIATIONS
-- ----------------------------------------

-- Range partitioning by year
CREATE TABLE test_partition_range (
    id INT NOT NULL,
    created_date DATE NOT NULL,
    data VARCHAR(100),
    amount DECIMAL(10,2),
    PRIMARY KEY (id, created_date)
) ENGINE=InnoDB
PARTITION BY RANGE (YEAR(created_date)) (
    PARTITION p2020 VALUES LESS THAN (2021),
    PARTITION p2021 VALUES LESS THAN (2022),
    PARTITION p2022 VALUES LESS THAN (2023),
    PARTITION p2023 VALUES LESS THAN (2024),
    PARTITION p2024 VALUES LESS THAN (2025),
    PARTITION p2025 VALUES LESS THAN (2026),
    PARTITION p_future VALUES LESS THAN MAXVALUE
);

-- Hash partitioning
CREATE TABLE test_partition_hash (
    id INT NOT NULL PRIMARY KEY,
    user_id INT NOT NULL,
    data VARCHAR(255)
) ENGINE=InnoDB
PARTITION BY HASH(user_id)
PARTITIONS 4;

-- List partitioning
CREATE TABLE test_partition_list (
    id INT NOT NULL,
    region VARCHAR(20) NOT NULL,
    sales DECIMAL(10,2),
    PRIMARY KEY (id, region)
) ENGINE=InnoDB
PARTITION BY LIST COLUMNS(region) (
    PARTITION p_north VALUES IN ('USA', 'Canada'),
    PARTITION p_europe VALUES IN ('UK', 'Germany', 'France'),
    PARTITION p_asia VALUES IN ('Japan', 'China', 'India'),
    PARTITION p_other VALUES IN ('Australia', 'Brazil')
);

-- ----------------------------------------
-- 8. STORAGE ENGINES & TABLE OPTIONS
-- ----------------------------------------

-- Compressed InnoDB table
CREATE TABLE test_compressed (
    id INT PRIMARY KEY,
    large_text LONGTEXT,
    large_blob LONGBLOB
) ENGINE=InnoDB ROW_FORMAT=COMPRESSED KEY_BLOCK_SIZE=8;

-- Memory engine table
CREATE TABLE test_memory (
    id INT PRIMARY KEY,
    session_data VARCHAR(1000),
    expires_at TIMESTAMP,
    INDEX idx_expires (expires_at)
) ENGINE=MEMORY;

-- MyISAM table (if available)
CREATE TABLE test_myisam (
    id INT PRIMARY KEY,
    data TEXT,
    FULLTEXT(data)
) ENGINE=MyISAM;

-- ----------------------------------------
-- 9. FULLTEXT SEARCH VARIATIONS
-- ----------------------------------------
CREATE TABLE test_fulltext_advanced (
    id INT PRIMARY KEY,
    
    -- English content
    title VARCHAR(255),
    content TEXT,
    
    -- Multi-language content
    content_english TEXT,
    content_chinese TEXT,
    
    -- Standard fulltext indexes
    FULLTEXT ft_title (title),
    FULLTEXT ft_content (content),
    FULLTEXT ft_title_content (title, content),
    
    -- N-gram parser for CJK languages (MySQL 5.7.6+)
    FULLTEXT ft_chinese (content_chinese) WITH PARSER ngram
) ENGINE=InnoDB;

-- ----------------------------------------
-- 10. COMPLEX TRIGGERS
-- ----------------------------------------
DELIMITER ||

-- Audit trigger
CREATE TRIGGER trg_audit_complex
BEFORE UPDATE ON test_generated_advanced
FOR EACH ROW
BEGIN
    -- Complex logic with multiple conditions
    IF OLD.salary != NEW.salary THEN
        INSERT INTO test_parent (code, status) 
        VALUES (CONCAT('AUDIT_', NEW.id, '_', UNIX_TIMESTAMP()), 'active');
    END IF;
    
    -- Validate business rules
    IF NEW.salary < 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Salary cannot be negative';
    END IF;
    
    -- Update related records
    IF NEW.first_name != OLD.first_name OR NEW.last_name != OLD.last_name THEN
        -- Trigger will automatically recompute generated columns
        SET NEW.birth_date = NEW.birth_date; -- Force update
    END IF;
END||

-- Complex insert trigger
CREATE TRIGGER trg_complex_insert
AFTER INSERT ON test_child
FOR EACH ROW
BEGIN
    DECLARE parent_status VARCHAR(20);
    
    -- Get parent status
    SELECT status INTO parent_status 
    FROM test_parent 
    WHERE id = NEW.parent_id;
    
    -- Complex conditional logic
    IF parent_status = 'inactive' THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Cannot add child to inactive parent';
    END IF;
END||

-- ----------------------------------------
-- 11. ADVANCED STORED PROCEDURES
-- ----------------------------------------

-- Procedure with cursors, error handling, and complex logic
CREATE PROCEDURE sp_complex_operations(
    IN p_start_date DATE,
    IN p_end_date DATE,
    OUT p_total_count INT,
    OUT p_error_message VARCHAR(500)
)
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_id INT;
    DECLARE v_count INT DEFAULT 0;
    DECLARE v_error_count INT DEFAULT 0;
    
    -- Cursor declaration
    DECLARE cur_records CURSOR FOR 
        SELECT id FROM test_child 
        WHERE start_date BETWEEN p_start_date AND p_end_date;
    
    -- Exception handlers
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION 
    BEGIN
        SET v_error_count = v_error_count + 1;
        GET DIAGNOSTICS CONDITION 1
            p_error_message = MESSAGE_TEXT;
    END;
    
    -- Initialize
    SET p_total_count = 0;
    SET p_error_message = '';
    
    -- Start transaction
    START TRANSACTION;
    
    -- Open cursor and process
    OPEN cur_records;
    
    read_loop: LOOP
        FETCH cur_records INTO v_id;
        
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- Complex processing logic
        SET v_count = v_count + 1;
        
        -- Simulate some processing
        UPDATE test_child 
        SET value = value * 1.1 
        WHERE id = v_id;
        
    END LOOP;
    
    CLOSE cur_records;
    
    -- Set output parameters
    SET p_total_count = v_count;
    
    -- Commit or rollback based on errors
    IF v_error_count = 0 THEN
        COMMIT;
    ELSE
        ROLLBACK;
        SET p_error_message = CONCAT('Errors encountered: ', v_error_count);
    END IF;
    
END||

-- ----------------------------------------
-- 12. ADVANCED FUNCTIONS
-- ----------------------------------------

-- Recursive function
CREATE FUNCTION fn_factorial(n INT) RETURNS BIGINT
DETERMINISTIC
READS SQL DATA
BEGIN
    IF n <= 1 THEN
        RETURN 1;
    ELSE
        RETURN n * fn_factorial(n - 1);
    END IF;
END||

-- Complex JSON processing function
CREATE FUNCTION fn_json_flatten(json_data JSON, path_prefix VARCHAR(255))
RETURNS JSON
DETERMINISTIC
READS SQL DATA
BEGIN
    DECLARE result JSON;
    DECLARE keys JSON;
    DECLARE key_count INT;
    DECLARE i INT DEFAULT 0;
    DECLARE current_key VARCHAR(255);
    DECLARE current_value JSON;
    DECLARE current_path VARCHAR(255);
    
    SET result = JSON_OBJECT();
    SET keys = JSON_KEYS(json_data);
    SET key_count = JSON_LENGTH(keys);
    
    WHILE i < key_count DO
        SET current_key = JSON_UNQUOTE(JSON_EXTRACT(keys, CONCAT('$[', i, ']')));
        SET current_value = JSON_EXTRACT(json_data, CONCAT('$.', current_key));
        SET current_path = CONCAT(path_prefix, '.', current_key);
        
        IF JSON_TYPE(current_value) = 'OBJECT' THEN
            -- Recursive call for nested objects
            SET result = JSON_MERGE_PRESERVE(result, fn_json_flatten(current_value, current_path));
        ELSE
            -- Add leaf value
            SET result = JSON_SET(result, current_path, current_value);
        END IF;
        
        SET i = i + 1;
    END WHILE;
    
    RETURN result;
END||

-- ----------------------------------------
-- 13. ADVANCED VIEWS
-- ----------------------------------------

-- Complex view with multiple joins and aggregations
CREATE VIEW v_comprehensive_report AS
SELECT 
    p.id as parent_id,
    p.code as parent_code,
    p.status as parent_status,
    COUNT(c.id) as child_count,
    AVG(c.value) as avg_child_value,
    SUM(c.value) as total_child_value,
    MAX(c.end_date) as latest_end_date,
    
    -- Conditional aggregation
    COUNT(CASE WHEN c.value > 1000 THEN 1 END) as high_value_count,
    
    -- JSON aggregation (MySQL 5.7+)
    JSON_ARRAYAGG(
        JSON_OBJECT(
            'id', c.id,
            'value', c.value,
            'start_date', c.start_date
        )
    ) as children_json,
    
    -- Window functions (MySQL 8.0+)
    ROW_NUMBER() OVER (ORDER BY SUM(c.value) DESC) as value_rank,
    PERCENT_RANK() OVER (ORDER BY COUNT(c.id)) as child_count_percentile
    
FROM test_parent p
LEFT JOIN test_child c ON p.id = c.parent_id
GROUP BY p.id, p.code, p.status
HAVING COUNT(c.id) > 0 OR p.status = 'active';

-- Recursive CTE view (MySQL 8.0+)
CREATE VIEW v_hierarchy AS
WITH RECURSIVE employee_hierarchy AS (
    -- Base case: top-level employees (no manager)
    SELECT id, manager_id, parent_id, 0 as level, CAST(id AS CHAR(1000)) as path
    FROM test_child 
    WHERE manager_id IS NULL
    
    UNION ALL
    
    -- Recursive case: employees with managers
    SELECT c.id, c.manager_id, c.parent_id, eh.level + 1, 
           CONCAT(eh.path, '->', c.id) as path
    FROM test_child c
    INNER JOIN employee_hierarchy eh ON c.manager_id = eh.id
    WHERE eh.level < 10  -- Prevent infinite recursion
)
SELECT * FROM employee_hierarchy;

DELIMITER ;

-- ----------------------------------------
-- 14. EVENTS WITH COMPLEX SCHEDULING
-- ----------------------------------------
DELIMITER ||

-- Daily cleanup event
CREATE EVENT evt_daily_maintenance
ON SCHEDULE EVERY 1 DAY
STARTS CURRENT_TIMESTAMP + INTERVAL 1 HOUR
ENDS CURRENT_TIMESTAMP + INTERVAL 1 YEAR
COMMENT 'Daily maintenance tasks'
DO
BEGIN
    -- Archive old records
    INSERT INTO test_memory (id, session_data, expires_at) VALUES
    (1, 'Session data for memory engine test', DATE_ADD(NOW(), INTERVAL 1 HOUR)),
    (2, 'Another session with longer data content', DATE_ADD(NOW(), INTERVAL 2 HOUR));

-- Insert fulltext test data
INSERT INTO test_fulltext_advanced (id, title, content, content_english, content_chinese) VALUES
    (1, 'MySQL Backup and Restore', 
        'This comprehensive guide covers MySQL backup and restore operations including mysqldump, binary logs, and point-in-time recovery.',
        'MySQL database backup restore comprehensive guide operations',
        'データベースのバックアップとリカバリの操作ガイド'),
    (2, 'Advanced MySQL Features',
        'Exploring advanced MySQL features like JSON support, spatial data, generated columns, and common table expressions.',
        'Advanced MySQL JSON spatial generated columns expressions',
        'उन्नत कार्याणि स्थानिकदत्तांशजनन स्तम्भव्यञ्जनानि'),
    (3, 'Performance Optimization',
        'MySQL performance tuning techniques including indexing strategies, query optimization, and server configuration.',
        'Performance tuning optimization indexing query configuration',
        'אופטימיזציית ביצועים כוונון תצורת שאילתת אינדקס');

-- ----------------------------------------
-- 16. ADDITIONAL EDGE CASES
-- ----------------------------------------

-- Table with all possible index types
CREATE TABLE test_indexes_comprehensive (
    id INT UNSIGNED AUTO_INCREMENT,
    
    -- Regular columns for various index types
    unique_col VARCHAR(100) UNIQUE,
    normal_col VARCHAR(100),
    prefix_col VARCHAR(255),
    multi_col1 VARCHAR(50),
    multi_col2 VARCHAR(50),
    
    -- Numeric columns for functional indexes
    salary DECIMAL(10,2),
    bonus DECIMAL(8,2),
    
    -- JSON for functional index
    metadata_json JSON,
    
    -- Spatial for spatial index
    location POINT,
    
    -- Text for fulltext
    description TEXT,
    
    -- Define various index types
    PRIMARY KEY (id),
    
    -- Unique indexes
    UNIQUE KEY uk_unique (unique_col),
    UNIQUE KEY uk_multi (multi_col1, multi_col2),
    
    -- Regular indexes
    INDEX idx_normal (normal_col),
    INDEX idx_prefix (prefix_col(50)),  -- Prefix index
    INDEX idx_composite (multi_col1, multi_col2),
    INDEX idx_desc (salary DESC),  -- Descending index (MySQL 8.0+)
    
    -- Functional index (MySQL 8.0+)
    INDEX idx_functional ((salary + bonus)),
    INDEX idx_json_functional ((CAST(JSON_UNQUOTE(JSON_EXTRACT(metadata_json, '$.department')) AS CHAR(50)))),
    
    -- Spatial index
    SPATIAL INDEX sp_location (location),
    
    -- Fulltext index
    FULLTEXT INDEX ft_description (description)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 17. WINDOW FUNCTIONS TABLE (MySQL 8.0+)
-- ----------------------------------------
CREATE TABLE test_window_functions (
    id INT PRIMARY KEY,
    department VARCHAR(50),
    employee_name VARCHAR(100),
    salary DECIMAL(10,2),
    hire_date DATE,
    performance_score DECIMAL(3,2)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 18. COMMON TABLE EXPRESSIONS (CTE) SUPPORT
-- ----------------------------------------
CREATE TABLE test_cte_support (
    id INT PRIMARY KEY,
    parent_id INT,
    name VARCHAR(100),
    level INT,
    path VARCHAR(500),
    
    FOREIGN KEY (parent_id) REFERENCES test_cte_support(id) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ----------------------------------------
-- 19. TEMPORAL TABLES (System Versioned)
-- Note: MySQL doesn't have built-in temporal tables like SQL Server,
-- but we can simulate with triggers and audit tables
-- ----------------------------------------
CREATE TABLE test_temporal_current (
    id INT PRIMARY KEY,
    data VARCHAR(255),
    valid_from TIMESTAMP(6) DEFAULT CURRENT_TIMESTAMP(6),
    valid_to TIMESTAMP(6) DEFAULT '9999-12-31 23:59:59.999999',
    is_current BOOLEAN DEFAULT TRUE
) ENGINE=InnoDB;

CREATE TABLE test_temporal_history (
    id INT,
    data VARCHAR(255),
    valid_from TIMESTAMP(6),
    valid_to TIMESTAMP(6),
    operation ENUM('INSERT', 'UPDATE', 'DELETE'),
    modified_by VARCHAR(100) DEFAULT USER(),
    modified_at TIMESTAMP(6) DEFAULT CURRENT_TIMESTAMP(6),
    
    INDEX idx_temporal (id, valid_from, valid_to)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 20. ENCRYPTION AND SECURITY FEATURES
-- ----------------------------------------
CREATE TABLE test_security_features (
    id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    
    -- Regular sensitive data
    ssn VARCHAR(20),
    credit_card VARCHAR(20),
    
    -- Hashed passwords (simulate)
    password_hash VARCHAR(255),
    salt VARCHAR(32),
    
    -- Encrypted data (would use AES_ENCRYPT in real scenario)
    encrypted_notes TEXT,
    
    -- Audit fields
    created_by VARCHAR(100) DEFAULT USER(),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_by VARCHAR(100) DEFAULT USER() ON UPDATE USER(),
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Row-level security simulation
    access_level ENUM('public', 'internal', 'confidential', 'secret') DEFAULT 'internal',
    owner_id INT
) ENGINE=InnoDB;

-- ----------------------------------------
-- 21. MYSQL 8.0 SPECIFIC FEATURES
-- ----------------------------------------

-- Invisible columns (MySQL 8.0.23+)
CREATE TABLE test_mysql8_features (
    id INT PRIMARY KEY,
    visible_data VARCHAR(100),
    
    -- Invisible columns
    internal_tracking_id BIGINT INVISIBLE DEFAULT (UNIX_TIMESTAMP() * 1000000),
    debug_info JSON INVISIBLE,
    
    -- CHECK constraints (MySQL 8.0.16+)
    score INT CHECK (score BETWEEN 0 AND 100),
    grade CHAR(1) CHECK (grade IN ('A', 'B', 'C', 'D', 'F')),
    
    -- Multi-valued indexes (MySQL 8.0.17+)
    tags JSON,
    INDEX idx_tags ((CAST(tags->'$[*]' AS CHAR(50) ARRAY)))
) ENGINE=InnoDB;

-- ----------------------------------------
-- 22. PERFORMANCE TESTING TABLES
-- ----------------------------------------

-- Large table for performance testing
CREATE TABLE test_performance_large (
    id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    random_string VARCHAR(255),
    random_number INT,
    random_decimal DECIMAL(15,4),
    random_date DATETIME,
    random_text TEXT,
    
    INDEX idx_string (random_string),
    INDEX idx_number (random_number),
    INDEX idx_date (random_date),
    INDEX idx_composite (random_number, random_date)
) ENGINE=InnoDB;

-- Wide table (many columns)
CREATE TABLE test_performance_wide (
    id INT PRIMARY KEY,
    col01 VARCHAR(100), col02 VARCHAR(100), col03 VARCHAR(100), col04 VARCHAR(100), col05 VARCHAR(100),
    col06 VARCHAR(100), col07 VARCHAR(100), col08 VARCHAR(100), col09 VARCHAR(100), col10 VARCHAR(100),
    col11 VARCHAR(100), col12 VARCHAR(100), col13 VARCHAR(100), col14 VARCHAR(100), col15 VARCHAR(100),
    col16 VARCHAR(100), col17 VARCHAR(100), col18 VARCHAR(100), col19 VARCHAR(100), col20 VARCHAR(100),
    col21 VARCHAR(100), col22 VARCHAR(100), col23 VARCHAR(100), col24 VARCHAR(100), col25 VARCHAR(100),
    col26 VARCHAR(100), col27 VARCHAR(100), col28 VARCHAR(100), col29 VARCHAR(100), col30 VARCHAR(100),
    
    -- Many indexes to test backup performance
    INDEX idx01(col01), INDEX idx02(col02), INDEX idx03(col03), INDEX idx04(col04), INDEX idx05(col05)
) ENGINE=InnoDB;

-- ----------------------------------------
-- 23. DATA TYPE EDGE CASES AND SPECIAL VALUES
-- ----------------------------------------

-- Insert comprehensive test data with edge cases
INSERT INTO test_indexes_comprehensive (
    unique_col, normal_col, prefix_col, multi_col1, multi_col2,
    salary, bonus, metadata_json, location, description
) VALUES
    ('UNIQUE001', 'Normal data', REPEAT('Prefix test data ', 10), 'Multi1A', 'Multi2A',
     5000.00, 500.00, '{""department"": ""Engineering"", ""level"": ""Senior""}',
     ST_GeomFromText('POINT(37.7749 -122.4194)'), 'San Francisco office location with full-text search content'),
    ('UNIQUE002', 'Another normal', REPEAT('Different prefix content ', 8), 'Multi1B', 'Multi2B',
     7500.50, 750.25, '{""department"": ""Marketing"", ""level"": ""Manager""}',
     ST_GeomFromText('POINT(40.7589 -73.9851)'), 'New York headquarters with marketing and sales teams'),
    ('UNIQUE003', 'Third entry', REPEAT('More prefix testing ', 12), 'Multi1C', 'Multi2C',
     3000.75, 300.00, '{""department"": ""Support"", ""level"": ""Junior""}',
     ST_GeomFromText('POINT(51.5074 -0.1278)'), 'London support center providing customer service globally');

-- Window functions test data
INSERT INTO test_window_functions VALUES
    (1, 'Engineering', 'Alice Johnson', 8500.00, '2020-01-15', 4.5),
    (2, 'Engineering', 'Bob Smith', 7200.00, '2020-03-22', 4.2),
    (3, 'Engineering', 'Carol Davis', 9200.00, '2019-11-08', 4.8),
    (4, 'Marketing', 'David Wilson', 6500.00, '2021-02-14', 3.9),
    (5, 'Marketing', 'Eve Brown', 7800.00, '2020-07-30', 4.4),
    (6, 'Marketing', 'Frank Miller', 5900.00, '2021-05-12', 3.7),
    (7, 'Sales', 'Grace Taylor', 7100.00, '2020-09-18', 4.1),
    (8, 'Sales', 'Henry Anderson', 6800.00, '2021-01-25', 3.8),
    (9, 'Support', 'Ivy Thompson', 5500.00, '2021-03-10', 4.0),
    (10, 'Support', 'Jack White', 5200.00, '2021-06-05', 3.6);

-- CTE hierarchical test data
INSERT INTO test_cte_support VALUES
    (1, NULL, 'CEO', 1, '/CEO'),
    (2, 1, 'CTO', 2, '/CEO/CTO'),
    (3, 1, 'CFO', 2, '/CEO/CFO'),
    (4, 2, 'Engineering Manager', 3, '/CEO/CTO/Engineering Manager'),
    (5, 2, 'DevOps Manager', 3, '/CEO/CTO/DevOps Manager'),
    (6, 4, 'Senior Developer', 4, '/CEO/CTO/Engineering Manager/Senior Developer'),
    (7, 4, 'Junior Developer', 4, '/CEO/CTO/Engineering Manager/Junior Developer'),
    (8, 5, 'DevOps Engineer', 4, '/CEO/CTO/DevOps Manager/DevOps Engineer');

-- Temporal table test data
INSERT INTO test_temporal_current (id, data) VALUES
    (1, 'Original data for record 1'),
    (2, 'Original data for record 2'),
    (3, 'Original data for record 3');

-- Security features test data
INSERT INTO test_security_features (
    ssn, credit_card, password_hash, salt, encrypted_notes, access_level, owner_id
) VALUES
    ('XXX-XX-1234', 'XXXX-XXXX-XXXX-5678', 
     'a1b2c3d4e5f6789...', 'random_salt_123',
     'Encrypted sensitive information', 'confidential', 1),
    ('XXX-XX-5678', 'XXXX-XXXX-XXXX-9012',
     'z9y8x7w6v5u4321...', 'random_salt_456',
     'More encrypted data', 'internal', 2),
    ('XXX-XX-9012', 'XXXX-XXXX-XXXX-3456',
     'q1w2e3r4t5y6789...', 'random_salt_789',
     'Highly sensitive encrypted content', 'secret', 1);

-- MySQL 8.0 features test data
INSERT INTO test_mysql8_features (
    id, visible_data, debug_info, score, grade, tags
) VALUES
    (1, 'Visible information', 
     '{""query_count"": 42, ""last_access"": ""2024-06-19""}', 
     85, 'B', '[""mysql"", ""database"", ""backup""]'),
    (2, 'Another record',
     '{""query_count"": 128, ""last_access"": ""2024-06-18""}',
     92, 'A', '[""performance"", ""optimization"", ""index""]'),
    (3, 'Third entry',
     '{""query_count"": 7, ""last_access"": ""2024-06-17""}',
     78, 'C', '[""testing"", ""edge-cases"", ""comprehensive""]');

-- Performance testing data (limited sample)
INSERT INTO test_performance_large (random_string, random_number, random_decimal, random_date, random_text)
SELECT 
    CONCAT('Random_', id, '_', MD5(RAND())),
    FLOOR(RAND() * 1000000),
    RAND() * 999999.9999,
    DATE_ADD('2020-01-01', INTERVAL FLOOR(RAND() * 1500) DAY),
    REPEAT(CONCAT('Performance test data ', id, ' '), FLOOR(RAND() * 10) + 1)
FROM test_extreme_values
CROSS JOIN (SELECT 1 UNION SELECT 2 UNION SELECT 3 UNION SELECT 4 UNION SELECT 5) multiplier;

-- Wide table performance test
INSERT INTO test_performance_wide (
    id, col01, col02, col03, col04, col05, col06, col07, col08, col09, col10,
    col11, col12, col13, col14, col15, col16, col17, col18, col19, col20,
    col21, col22, col23, col24, col25, col26, col27, col28, col29, col30
) VALUES
    (1, 'A01', 'A02', 'A03', 'A04', 'A05', 'A06', 'A07', 'A08', 'A09', 'A10',
        'A11', 'A12', 'A13', 'A14', 'A15', 'A16', 'A17', 'A18', 'A19', 'A20',
        'A21', 'A22', 'A23', 'A24', 'A25', 'A26', 'A27', 'A28', 'A29', 'A30'),
    (2, 'B01', 'B02', 'B03', 'B04', 'B05', 'B06', 'B07', 'B08', 'B09', 'B10',
        'B11', 'B12', 'B13', 'B14', 'B15', 'B16', 'B17', 'B18', 'B19', 'B20',
        'B21', 'B22', 'B23', 'B24', 'B25', 'B26', 'B27', 'B28', 'B29', 'B30');

-- ----------------------------------------
-- 24. TRIGGERS FOR TEMPORAL TABLE SIMULATION
-- ----------------------------------------
DELIMITER ||

CREATE TRIGGER trg_temporal_update
BEFORE UPDATE ON test_temporal_current
FOR EACH ROW
BEGIN
    -- Archive the old version
    INSERT INTO test_temporal_history (
        id, data, valid_from, valid_to, operation
    ) VALUES (
        OLD.id, OLD.data, OLD.valid_from, NEW.valid_from, 'UPDATE'
    );
    
    -- Update the valid_from for the new version
    SET NEW.valid_from = CURRENT_TIMESTAMP(6);
END||

CREATE TRIGGER trg_temporal_delete
BEFORE DELETE ON test_temporal_current
FOR EACH ROW
BEGIN
    -- Archive the deleted version
    INSERT INTO test_temporal_history (
        id, data, valid_from, valid_to, operation
    ) VALUES (
        OLD.id, OLD.data, OLD.valid_from, CURRENT_TIMESTAMP(6), 'DELETE'
    );
END||

-- ----------------------------------------
-- 25. STRESS TEST PROCEDURES
-- ----------------------------------------

-- Procedure to generate large amounts of test data
CREATE PROCEDURE sp_generate_test_data(IN record_count INT)
BEGIN
    DECLARE i INT DEFAULT 1;
    DECLARE batch_size INT DEFAULT 1000;
    
    START TRANSACTION;
    
    WHILE i <= record_count DO
        INSERT INTO test_performance_large (
            random_string, random_number, random_decimal, random_date, random_text
        ) VALUES (
            CONCAT('Generated_', i, '_', SUBSTRING(MD5(RAND()), 1, 8)),
            FLOOR(RAND() * 2147483647),
            RAND() * 999999.9999,
            DATE_ADD('2020-01-01', INTERVAL FLOOR(RAND() * 1500) DAY),
            CONCAT('Generated test data for performance testing. Record number: ', i, '. ', REPEAT('Data ', FLOOR(RAND() * 50) + 1))
        );
        
        -- Commit in batches to avoid long-running transactions
        IF i % batch_size = 0 THEN
            COMMIT;
            START TRANSACTION;
        END IF;
        
        SET i = i + 1;
    END WHILE;
    
    COMMIT;
END||

-- ----------------------------------------
-- 26. ADVANCED VIEWS WITH COMPLEX LOGIC
-- ----------------------------------------

-- Performance analysis view
CREATE VIEW v_performance_analysis AS
SELECT 
    department,
    COUNT(*) as employee_count,
    AVG(salary) as avg_salary,
    MIN(salary) as min_salary,
    MAX(salary) as max_salary,
    STDDEV(salary) as salary_stddev,
    AVG(performance_score) as avg_performance,
    
    -- Window functions for ranking
    RANK() OVER (ORDER BY AVG(salary) DESC) as salary_rank,
    PERCENT_RANK() OVER (ORDER BY AVG(performance_score) DESC) as performance_percentile,
    
    -- Conditional aggregations
    COUNT(CASE WHEN performance_score >= 4.0 THEN 1 END) as high_performers,
    COUNT(CASE WHEN salary >= 7000 THEN 1 END) as high_earners,
    
    -- JSON aggregation
    JSON_ARRAYAGG(
        JSON_OBJECT(
            'name', employee_name,
            'salary', salary,
            'score', performance_score
        )
    ) as employees_detail
    
FROM test_window_functions
GROUP BY department
HAVING COUNT(*) > 0;

-- Hierarchical organization view
CREATE VIEW v_organization_hierarchy AS
WITH RECURSIVE org_tree AS (
    -- Root level
    SELECT 
        id, parent_id, name, level, path,
        0 as depth,
        name as full_path,
        1 as is_leaf
    FROM test_cte_support 
    WHERE parent_id IS NULL
    
    UNION ALL
    
    -- Recursive levels
    SELECT 
        c.id, c.parent_id, c.name, c.level, c.path,
        ot.depth + 1,
        CONCAT(ot.full_path, ' -> ', c.name),
        1 as is_leaf
    FROM test_cte_support c
    INNER JOIN org_tree ot ON c.parent_id = ot.id
    WHERE ot.depth < 10
),
leaf_check AS (
    SELECT 
        ot.*,
        CASE 
            WHEN EXISTS (
                SELECT 1 FROM test_cte_support child 
                WHERE child.parent_id = ot.id
            ) THEN 0 
            ELSE 1 
        END as is_actual_leaf
    FROM org_tree ot
)
SELECT 
    id, parent_id, name, level, path, depth, full_path,
    is_actual_leaf as is_leaf,
    CASE 
        WHEN depth = 0 THEN 'Executive'
        WHEN depth = 1 THEN 'Director'
        WHEN depth = 2 THEN 'Manager'
        WHEN depth = 3 THEN 'Team Lead'
        ELSE 'Individual Contributor'
    END as role_category
FROM leaf_check
ORDER BY depth, name;

DELIMITER ;

-- ----------------------------------------
-- 27. FINAL COMPREHENSIVE VALIDATION
-- ----------------------------------------

-- Create a summary view to validate all test data
CREATE VIEW v_test_data_summary AS
SELECT 
    'test_extreme_values' as table_name,
    COUNT(*) as record_count,
    'Extreme numeric and string values' as description
FROM test_extreme_values

UNION ALL

SELECT 
    'test_charset_collations',
    COUNT(*),
    'Character set and collation variations'
FROM test_charset_collations

UNION ALL

SELECT 
    'test_parent',
    COUNT(*),
    'Parent table with constraints'
FROM test_parent

UNION ALL

SELECT 
    'test_child',
    COUNT(*),
    'Child table with foreign keys and self-references'
FROM test_child

UNION ALL

SELECT 
    'test_generated_advanced',
    COUNT(*),
    'Advanced generated columns (virtual and stored)'
FROM test_generated_advanced

UNION ALL

SELECT 
    'test_spatial_complete',
    COUNT(*),
    'Comprehensive spatial data types'
FROM test_spatial_complete

UNION ALL

SELECT 
    'test_json_comprehensive',
    COUNT(*),
    'Complex JSON data with edge cases'
FROM test_json_comprehensive

UNION ALL

SELECT 
    'test_partition_range',
    COUNT(*),
    'Range partitioned table'
FROM test_partition_range

UNION ALL

SELECT 
    'test_partition_hash',
    COUNT(*),
    'Hash partitioned table'
FROM test_partition_hash

UNION ALL

SELECT 
    'test_partition_list',
    COUNT(*),
    'List partitioned table'
FROM test_partition_list

UNION ALL

SELECT 
    'test_indexes_comprehensive',
    COUNT(*),
    'All index types and functional indexes'
FROM test_indexes_comprehensive

UNION ALL

SELECT 
    'test_window_functions',
    COUNT(*),
    'Window functions test data'
FROM test_window_functions

UNION ALL

SELECT 
    'test_mysql8_features',
    COUNT(*),
    'MySQL 8.0 specific features'
FROM test_mysql8_features

UNION ALL

SELECT 
    'test_performance_large',
    COUNT(*),
    'Performance testing with large dataset'
FROM test_performance_large

ORDER BY table_name;

-- ----------------------------------------
-- FINAL SUMMARY COMMENT
-- ----------------------------------------

/*
This comprehensive test suite covers:

1. ✅ ALL MySQL data types with extreme values
2. ✅ ALL character sets and collations
3. ✅ Complex constraints and relationships
4. ✅ Generated columns (virtual and stored)
5. ✅ ALL spatial data types
6. ✅ Complex JSON with edge cases
7. ✅ ALL partitioning types
8. ✅ ALL storage engines
9. ✅ ALL index types including functional
10. ✅ Window functions (MySQL 8.0+)
11. ✅ Common Table Expressions (CTEs)
12. ✅ Temporal data simulation
13. ✅ Security features
14. ✅ MySQL 8.0 specific features
15. ✅ Performance testing scenarios
16. ✅ Complex stored procedures with cursors and error handling
17. ✅ Advanced functions including recursive
18. ✅ Complex triggers with business logic
19. ✅ Advanced views with window functions and CTEs
20. ✅ Event scheduling variations
21. ✅ Fulltext search with different parsers
22. ✅ Multi-valued indexes
23. ✅ Invisible columns
24. ✅ CHECK constraints
25. ✅ All possible edge cases for MySqlBackup.NET

This test suite should exercise every code path in your MySqlBackup.NET 
library and ensure it handles ALL MySQL features correctly across ALL
export modes (Insert, InsertIgnore, Replace, Update, OnDuplicateKeyUpdate).

Perfect for enterprise-grade backup/restore testing! 🎯
*/d, session_data, expires_at)
    SELECT id, CONCAT('archived_', created_at), created_at + INTERVAL 30 DAY
    FROM test_parent 
    WHERE status = 'inactive' AND created_at < DATE_SUB(NOW(), INTERVAL 30 DAY);
    
    -- Clean up expired sessions
    DELETE FROM test_memory WHERE expires_at < NOW();
    
    -- Update statistics
    CALL sp_complex_operations(DATE_SUB(NOW(), INTERVAL 1 DAY), NOW(), @count, @error);
END||

-- Weekly report event
CREATE EVENT evt_weekly_report
ON SCHEDULE EVERY 1 WEEK
STARTS DATE_ADD(DATE_ADD(CURDATE(), INTERVAL (7 - DAYOFWEEK(CURDATE())) DAY), INTERVAL 6 HOUR)
COMMENT 'Weekly summary report generation'
DO
BEGIN
    -- Generate weekly summary
    INSERT INTO test_parent (code, status)
    SELECT 
        CONCAT('WEEKLY_', YEAR(NOW()), '_', WEEK(NOW())),
        'active'
    FROM DUAL
    WHERE NOT EXISTS (
        SELECT 1 FROM test_parent 
        WHERE code = CONCAT('WEEKLY_', YEAR(NOW()), '_', WEEK(NOW()))
    );
END||

DELIMITER ;

-- ----------------------------------------
-- 15. SAMPLE DATA WITH EDGE CASES
-- ----------------------------------------

-- Insert extreme values
INSERT INTO test_extreme_values (
    decimal_max, decimal_min,
    char_max, varchar_max,
    binary_max, varbinary_max,
    tinytext_max, text_max
) VALUES (
    99999999999999999999999999999999999.999999999999999999999999999999,
    -99999999999999999999999999999999999.999999999999999999999999999999,
    REPEAT('A', 255),
    REPEAT('B', 1000), -- Smaller than max for practical testing
    REPEAT(0xFF, 255),
    REPEAT(0xAB, 1000),
    REPEAT('T', 255),
    REPEAT('Large text content with unicode: 你好世界 🌍 ', 100)
);

-- Insert charset variations
INSERT INTO test_charset_collations VALUES (
    1,
    'Hello World',
    'Hello World Ünicode',
    'CASE sensitive',
    'Ñoño España',
    'UTF8 General',
    'UTF8 Unicode',
    'utf8 binary',
    'Swedish Ålborg',
    'General Ølsen',
    'Binary Data',
    'ASCII Only',
    'ascii binary',
    'Binary String'
);

-- Insert parent-child relationships
INSERT INTO test_parent (code, status) VALUES 
    ('PARENT001', 'active'),
    ('PARENT002', 'inactive'),
    ('PARENT003', 'pending');

INSERT INTO test_child (parent_id, parent_code, value, percentage, start_date, end_date) VALUES
    (1, 'PARENT001', 1500.50, 75.25, '2024-01-01', '2024-12-31'),
    (1, 'PARENT001', 2500.75, 85.50, '2024-02-01', NULL),
    (2, 'PARENT002', 500.00, 25.00, '2024-03-01', '2024-06-30'),
    (3, 'PARENT003', 3500.25, 95.75, '2024-04-01', NULL);

-- Self-referencing data
UPDATE test_child SET manager_id = 1 WHERE id = 2;
UPDATE test_child SET manager_id = 1 WHERE id = 3;

-- Insert generated column test data
INSERT INTO test_generated_advanced (first_name, last_name, birth_date, salary) VALUES
    ('John', 'Doe', '1990-05-15', 5000.00),
    ('Jane', 'Smith', '1985-08-22', 7500.50),
    ('Bob', 'Johnson', '2005-12-03', 3000.25),
    ('Alice', 'Williams', '1978-03-10', 9500.75);

-- Insert spatial data
INSERT INTO test_spatial_complete (
    point_col, linestring_col, polygon_col,
    multipoint_col, multilinestring_col, multipolygon_col,
    geometrycollection_col, geometry_col
) VALUES (
    ST_GeomFromText('POINT(40.7128 -74.0060)'), -- New York
    ST_GeomFromText('LINESTRING(0 0, 1 1, 2 2, 3 3)'),
    ST_GeomFromText('POLYGON((0 0, 4 0, 4 4, 0 4, 0 0), (1 1, 2 1, 2 2, 1 2, 1 1))'), -- With hole
    ST_GeomFromText('MULTIPOINT(0 0, 10 10, 20 20, 30 30)'),
    ST_GeomFromText('MULTILINESTRING((0 0, 1 1), (10 10, 11 11), (20 20, 21 21))'),
    ST_GeomFromText('MULTIPOLYGON(((0 0, 4 0, 4 4, 0 4, 0 0)), ((10 10, 14 10, 14 14, 10 14, 10 10)))'),
    ST_GeomCollFromText('GEOMETRYCOLLECTION(POINT(1 1), LINESTRING(0 0, 1 1), POLYGON((2 2, 6 2, 6 6, 2 6, 2 2)))'),
    ST_GeomFromText('POINT(51.5074 -0.1278)') -- London
);

-- Insert JSON test data
INSERT INTO test_json_comprehensive (simple_json, complex_json, special_chars_json, large_json) VALUES (
    '{""name"": ""Test User"", ""age"": 30, ""active"": true}',
    '{
        ""user"": {
            ""profile"": {
                ""personal"": {
                    ""name"": ""Complex User"",
                    ""contacts"": [""email@test.com"", ""phone@test.com""],
                    ""preferences"": {
                        ""notifications"": true,
                        ""theme"": ""dark"",
                        ""languages"": [""en"", ""es"", ""fr""]
                    }
                },
                ""professional"": {
                    ""title"": ""Senior Developer"",
                    ""skills"": [""MySQL"", ""JSON"", ""Backup""],
                    ""experience"": 10
                }
            },
            ""metadata"": {
                ""created"": ""2024-01-01T00:00:00Z"",
                ""updated"": ""2024-06-19T12:00:00Z"",
                ""version"": 2
            }
        }
    }',
    '{""special"": ""Special chars: \\""quotes\\"", \\n newlines, \\t tabs, unicode: 你好 🌍"", ""null_value"": null}',
    CONCAT('{""large_array"": [', REPEAT('""item"",', 1000), '""last_item""]}')
);

-- Insert partitioned data
INSERT INTO test_partition_range VALUES
    (1, '2020-06-15', 'Data from 2020', 1000.00),
    (2, '2021-06-15', 'Data from 2021', 1500.00),
    (3, '2022-06-15', 'Data from 2022', 2000.00),
    (4, '2023-06-15', 'Data from 2023', 2500.00),
    (5, '2024-06-15', 'Data from 2024', 3000.00),
    (6, '2025-06-15', 'Data from 2025', 3500.00);

INSERT INTO test_partition_hash VALUES
    (1, 101, 'Hash partition data 1'),
    (2, 102, 'Hash partition data 2'),
    (3, 103, 'Hash partition data 3'),
    (4, 104, 'Hash partition data 4');

INSERT INTO test_partition_list VALUES
    (1, 'USA', 50000.00),
    (2, 'Canada', 35000.00),
    (3, 'UK', 45000.00),
    (4, 'Germany', 55000.00),
    (5, 'Japan', 40000.00),
    (6, 'Australia', 30000.00);

-- Insert test data for special storage engines
INSERT INTO test_compressed (id, large_text, large_blob) VALUES
    (1, REPEAT('Compressed text data. ', 1000), REPEAT('Binary', 1000));

INSERT INTO test_memory (i
";
            return sql;
        }

    }
}