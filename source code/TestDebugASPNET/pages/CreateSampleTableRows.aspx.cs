using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace System.pages
{
    public partial class CreateSampleTableRows : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btGenerateData_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTotalRows.Text, out int totalRows))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic["operation"] = 3;
                dic["start_time"] = DateTime.Now;
                dic["end_time"] = DateTime.MinValue;
                dic["is_completed"] = false;
                dic["has_error"] = false;
                dic["is_cancelled"] = false;
                dic["filename"] = "";
                dic["total_tables"] = 1;
                dic["total_rows"] = totalRows;
                dic["total_rows_current_table"] = 0;
                dic["current_table"] = "sample1";
                dic["current_table_index"] = 1;
                dic["current_row"] = 0;
                dic["current_row_in_current_table"] = 0;
                dic["total_bytes"] = 0;
                dic["current_bytes"] = 0;
                dic["percent_complete"] = 0;
                dic["remarks"] = "";
                dic["dbfile_id"] = 0;
                dic["last_update_time"] = DateTime.Now;
                dic["client_request_cancel_task"] = false;
                dic["has_file"] = false;

                int taskid = 0;

                using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        SQLiteHelper h = new SQLiteHelper(cmd);
                        h.Insert("progress_report", dic);
                        taskid = (int)conn.LastInsertRowId;
                    }
                }

                // Fire and forget - start the background task
                _ = Task.Run(() => GenerateData(totalRows, taskid, cbDropRecreateTable.Checked));

                // Redirect to progress report page at the client side
                Header.Controls.Add(new LiteralControl("<script>window.location = '/ReportProgress';</script>"));
            }
            else
            {
                ((masterPage1)this.Master).WriteTopMessageBar("Invalid total rows", false);
                ((masterPage1)this.Master).ShowMessage("Error", "Invalid total rows", false);
            }
        }

        private void GenerateData(int totalRows, int taskid, bool dropRecreateTable)
        {
            try
            {
                var generator = new SampleTableRowsGenerator(config.ConnString, taskid, dropRecreateTable);
                generator.GenerateTableRows(totalRows);

                Dictionary<string, object> dic = new Dictionary<string, object>();

                //dic["operation"] = 3;
                //dic["start_time"] = DateTime.Now;
                dic["end_time"] = DateTime.Now;
                dic["is_completed"] = true;
                dic["has_error"] = false;
                dic["is_cancelled"] = false;
                dic["filename"] = "";
                dic["total_tables"] = 1;
                dic["total_rows"] = totalRows;
                dic["total_rows_current_table"] = totalRows;
                dic["current_table"] = "sample1";
                dic["current_table_index"] = 1;
                dic["current_row"] = totalRows;
                dic["current_row_in_current_table"] = totalRows;
                dic["total_bytes"] = 0;
                dic["current_bytes"] = 0;
                dic["percent_complete"] = 100;
                dic["remarks"] = "";
                dic["dbfile_id"] = 0;
                dic["last_update_time"] = DateTime.Now;
                dic["client_request_cancel_task"] = false;
                dic["has_file"] = false;

                using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        SQLiteHelper h = new SQLiteHelper(cmd);
                        h.Insert("progress_report", dic);
                        taskid = (int)conn.LastInsertRowId;
                    }
                }
            }
            catch (Exception ex)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                //dic["operation"] = 3;
                //dic["start_time"] = DateTime.Now;
                dic["end_time"] = DateTime.Now;
                dic["is_completed"] = true;
                dic["has_error"] = true;
                dic["is_cancelled"] = false;
                dic["filename"] = "";
                //dic["total_tables"] = 1;
                //dic["total_rows"] = totalRows;
                //dic["total_rows_current_table"] = totalRows;
                //dic["current_table"] = "sample1";
                //dic["current_table_index"] = 1;
                //dic["current_row"] = totalRows;
                //dic["current_row_in_current_table"] = totalRows;
                //dic["total_bytes"] = 0;
                //dic["current_bytes"] = 0;
                //dic["percent_complete"] = 100;
                dic["remarks"] = ex.Message;
                //dic["dbfile_id"] = 0;
                dic["last_update_time"] = DateTime.Now;
                dic["client_request_cancel_task"] = false;
                dic["has_file"] = false;

                using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        SQLiteHelper h = new SQLiteHelper(cmd);
                        h.Insert("progress_report", dic);
                        taskid = (int)conn.LastInsertRowId;
                    }
                }
            }
        }

        public class SampleTableRowsGenerator
        {
            int _taskid = 0;
            readonly string _connectionString;
            System.Timers.Timer _progressTimer;
            int _currentRows;
            int _totalRows;
            bool _stop_process = false;
            bool _dropRecreateTable = false;

            // Public properties
            public int IntervalForProgressReport { get; set; } = 500;

            public SampleTableRowsGenerator(string connectionString, int taskid, bool dropRecreateTable)
            {
                _taskid = taskid;
                _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
                _progressTimer = new System.Timers.Timer(IntervalForProgressReport);
                _progressTimer.Elapsed += OnProgressTimerElapsed;
                _progressTimer.AutoReset = true;
                _dropRecreateTable |= dropRecreateTable;
            }

            public void GenerateTableRows(int totalRows)
            {
                _totalRows = totalRows;
                _currentRows = 0;

                try
                {
                    _progressTimer.Interval = IntervalForProgressReport;
                    _progressTimer.Start();

                    using (MySqlConnection conn = new MySqlConnection(_connectionString))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            if (_dropRecreateTable)
                            {
                                cmd.CommandText = "DROP TABLE IF EXISTS sample1;";
                                cmd.ExecuteNonQuery();
                            }

                            // Create table
                            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS sample1 (
    id INT AUTO_INCREMENT PRIMARY KEY,
    col_tinyint TINYINT,
    col_smallint SMALLINT,
    col_mediumint MEDIUMINT,
    col_int INT,
    col_bigint BIGINT,
    col_decimal DECIMAL(10,2),
    col_numeric NUMERIC(8,3),
    col_float FLOAT,
    col_double DOUBLE,
    col_bit BIT(8),
    col_bool BOOLEAN,
    col_real REAL,
    col_double_precision DOUBLE PRECISION,
    col_integer INTEGER,
    col_dec DEC(10,2),
    col_fixed FIXED(8,3),
    col_serial BIGINT UNSIGNED NOT NULL,
    col_date DATE,
    col_time TIME,
    col_datetime DATETIME,
    col_timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    col_year YEAR,
    col_char CHAR(50),
    col_varchar VARCHAR(255),
    col_binary BINARY(16),
    col_varbinary VARBINARY(255),
    col_tinytext TINYTEXT,
    col_text TEXT,
    col_mediumtext MEDIUMTEXT,
    col_longtext LONGTEXT,
    col_tinyblob TINYBLOB,
    col_blob BLOB,
    col_mediumblob MEDIUMBLOB,
    col_longblob LONGBLOB,
    col_json JSON,
    col_geometry GEOMETRY,
    col_point POINT,
    col_linestring LINESTRING,
    col_polygon POLYGON,
    col_multipoint MULTIPOINT,
    col_multilinestring MULTILINESTRING,
    col_multipolygon MULTIPOLYGON,
    col_geometrycollection GEOMETRYCOLLECTION,
    col_enum ENUM('small', 'medium', 'large'),
    col_set SET('red', 'green', 'blue', 'yellow')
);";
                            cmd.ExecuteNonQuery();

                            // Bulk insert
                            var batchSize = 1000; // Increased for better performance
                            var values = new List<string>();
                            for (int i = 1; i <= totalRows; i++)
                            {
                                if (_stop_process)
                                {
                                    break;
                                }

                                _currentRows = i;
                                values.Add($@"
(
    {i % 127}, -- col_tinyint (0 to 255)
    {i % 32768}, -- col_smallint (-32768 to 32767)
    {i % 8388608}, -- col_mediumint (-8388608 to 8388607)
    {i % 2147483648}, -- col_int (-2147483648 to 2147483647)
    {i % 1000000}, -- col_bigint (use modulo to avoid overflow)
    {(i % 1000) + 0.01 * (i % 100)}, -- col_decimal (e.g., 0.00 to 999.99)
    {(i % 100) + 0.001 * (i % 1000)}, -- col_numeric (e.g., 0.000 to 99.999)
    {(float)(i % 1000) / 10}, -- col_float (e.g., 0.0 to 99.9)
    {(double)(i % 10000) / 100}, -- col_double (e.g., 0.00 to 99.99)
    b'{Convert.ToString(i % 256, 2).PadLeft(8, '0')}', -- col_bit (8 bits, 0 to 255)
    {i % 2}, -- col_bool (0 or 1)
    {(float)(i % 1000) / 10}, -- col_real (e.g., 0.0 to 99.9)
    {(double)(i % 10000) / 100}, -- col_double_precision (e.g., 0.00 to 99.99)
    {i % 2147483648}, -- col_integer (-2147483648 to 2147483647)
    {(i % 1000) + 0.01 * (i % 100)}, -- col_dec (e.g., 0.00 to 999.99)
    {(i % 100) + 0.001 * (i % 1000)}, -- col_fixed (e.g., 0.000 to 99.999)
    {i}, -- col_serial (auto-incrementing, safe as BIGINT UNSIGNED)
    '2024-01-01' + INTERVAL {(i % 365)} DAY, -- col_date (cycle through 1 year)
    TIME(CONCAT('{(i % 24):D2}:', '{(i % 60):D2}:', '{(i % 60):D2}')), -- col_time (cycle through hours/minutes/seconds)
    '2024-01-01 12:00:00' + INTERVAL {(i % 365)} DAY, -- col_datetime (cycle through 1 year)
    '2024-01-01 12:00:00' + INTERVAL {(i % 365)} DAY, -- col_timestamp (cycle through 1 year)
    {2000 + (i % 100)}, -- col_year (cycle through 2000 to 2099)
    'CHAR{i % 50}', -- col_char (cycle through small strings)
    'VARCHAR row {i % 255}', -- col_varchar
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_binary (16 bytes)
    UNHEX('{(i % 256):X2}{(i % 256):X2}'), -- col_varbinary
    'Tiny text {i % 100}', -- col_tinytext
    'Text content for row {i % 100}', -- col_text
    'Medium text for row {i % 100}', -- col_mediumtext
    'Long text content for row {i % 100}', -- col_longtext
    UNHEX('{(i % 256):X2}{(i % 256):X2}'), -- col_tinyblob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_blob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_mediumblob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_longblob
    JSON_OBJECT('id', {i % 1000}, 'name', CONCAT('Item ', {i % 1000}), 'active', {(i % 2 == 0 ? "true" : "false")}), -- col_json
    ST_GeomFromText('POINT({i % 100} {i % 100})'), -- col_geometry
    ST_GeomFromText('POINT({i % 100} {(i % 100) * 2})'), -- col_point
    ST_GeomFromText('LINESTRING(0 0, {i % 100} {i % 100})'), -- col_linestring
    ST_GeomFromText('POLYGON((0 0, {i % 100} 0, {i % 100} {i % 100}, 0 {i % 100}, 0 0))'), -- col_polygon
    ST_GeomFromText('MULTIPOINT({i % 100} {i % 100}, {(i % 100) * 2} {(i % 100) * 2})'), -- col_multipoint
    ST_GeomFromText('MULTILINESTRING((0 0, {i % 100} {i % 100}, {(i % 100) * 2} {(i % 100) * 2}))'), -- col_multilinestring
    ST_GeomFromText('MULTIPOLYGON(((0 0, {i % 100} 0, {i % 100} {i % 100}, 0 {i % 100}, 0 0)))'), -- col_multipolygon
    ST_GeomFromText('GEOMETRYCOLLECTION(POINT({i % 100} {i % 100}), LINESTRING(0 0, {i % 100} {i % 100}))'), -- col_geometrycollection
    '{(i % 3 == 0 ? "small" : i % 3 == 1 ? "medium" : "large")}', -- col_enum
    '{(i % 4 == 0 ? "red,blue" : i % 4 == 1 ? "green" : i % 4 == 2 ? "blue,yellow" : "red,green,blue")}' -- col_set
)");

                                // Execute batch when size is reached or at the end
                                if (values.Count >= batchSize || i == totalRows)
                                {
                                    if (values.Count > 0)
                                    {
                                        cmd.CommandText = $@"INSERT INTO sample1 (
                            col_tinyint, col_smallint, col_mediumint, col_int, col_bigint,
                            col_decimal, col_numeric, col_float, col_double, col_bit,
                            col_bool, col_real, col_double_precision, col_integer, col_dec,
                            col_fixed, col_serial, col_date, col_time, col_datetime,
                            col_timestamp, col_year, col_char, col_varchar, col_binary,
                            col_varbinary, col_tinytext, col_text, col_mediumtext, col_longtext,
                            col_tinyblob, col_blob, col_mediumblob, col_longblob, col_json,
                            col_geometry, col_point, col_linestring, col_polygon, col_multipoint,
                            col_multilinestring, col_multipolygon, col_geometrycollection, col_enum, col_set
                        ) VALUES {string.Join(",", values)};";

                                        cmd.ExecuteNonQuery();
                                        values.Clear();
                                    }
                                }
                            }
                        }
                    }

                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic["end_time"] = DateTime.Now;
                    dic["is_completed"] = true;
                    dic["has_error"] = false;
                    dic["dbfile_id"] = 0;
                    dic["last_update_time"] = DateTime.Now;
                    dic["has_file"] = false;

                    using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            conn.Open();
                            SQLiteHelper h = new SQLiteHelper(cmd);
                            h.Update("progress_report", dic, "id", _taskid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _progressTimer?.Stop();

                    Dictionary<string, object> dic = new Dictionary<string, object>();

                    dic["end_time"] = DateTime.Now;
                    dic["is_completed"] = true;
                    dic["has_error"] = true;
                    dic["remarks"] = ex.Message;
                    dic["last_update_time"] = DateTime.Now;
                    dic["has_file"] = false;

                    using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            conn.Open();
                            SQLiteHelper h = new SQLiteHelper(cmd);
                            h.Update("progress_report", dic, "id", _taskid);
                        }
                    }
                }
                finally
                {
                    OnProgressTimerElapsed(this, null);
                    Dispose();
                }
            }

            private void OnProgressTimerElapsed(object sender, ElapsedEventArgs e)
            {
                int percentage = _currentRows * 100 / _totalRows;

                if (percentage < 0)
                    percentage = 0;

                if (percentage > 100)
                    percentage = 100;

                Dictionary<string, object> dic = new Dictionary<string, object>();

                //dic["operation"] = 3;
                //dic["start_time"] = DateTime.Now;
                //dic["end_time"] = DateTime.MinValue;
                //dic["is_completed"] = false;
                //dic["has_error"] = false;
                //dic["is_cancelled"] = false;
                //dic["filename"] = "";
                //dic["total_tables"] = 1;
                //dic["total_rows"] = totalRows;
                //dic["total_rows_current_table"] = 0;
                //dic["current_table"] = "sample1";
                //dic["current_table_index"] = 1;
                dic["current_row"] = _currentRows;
                dic["current_row_in_current_table"] = _currentRows;
                //dic["total_bytes"] = 0;
                //dic["current_bytes"] = 0;
                dic["percent_complete"] = percentage;
                //dic["remarks"] = "";
                //dic["dbfile_id"] = 0;
                dic["last_update_time"] = DateTime.Now;
                //dic["client_request_cancel_task"] = false;
                //dic["has_file"] = false;

                int client_request_cancel_task = 0;

                using (var conn = new SQLiteConnection(BackupFilesManager.sqliteConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        SQLiteHelper h = new SQLiteHelper(cmd);
                        h.Update("progress_report", dic, "id", _taskid);

                        client_request_cancel_task = h.ExecuteScalar<int>($"select client_request_cancel_task from progress_report where id = {_taskid}");

                        if (client_request_cancel_task == 1)
                        {
                            _stop_process = true;

                            _progressTimer?.Stop();
                            _progressTimer = null;

                            dic.Clear();

                            //dic["operation"] = 3;
                            //dic["start_time"] = DateTime.Now;
                            dic["end_time"] = DateTime.Now;
                            dic["is_completed"] = true;
                            //dic["has_error"] = false;
                            dic["is_cancelled"] = true;
                            //dic["filename"] = "";
                            //dic["total_tables"] = 1;
                            //dic["total_rows"] = totalRows;
                            //dic["total_rows_current_table"] = 0;
                            //dic["current_table"] = "sample1";
                            //dic["current_table_index"] = 1;
                            //dic["current_row"] = _currentRows;
                            //dic["current_row_in_current_table"] = _currentRows;
                            //dic["total_bytes"] = 0;
                            //dic["current_bytes"] = 0;
                            //dic["percent_complete"] = percentage;
                            //dic["remarks"] = "";
                            //dic["dbfile_id"] = 0;
                            //dic["last_update_time"] = DateTime.Now;
                            //dic["client_request_cancel_task"] = false;
                            //dic["has_file"] = false;

                            h.Update("progress_report", dic, "id", _taskid);
                        }
                    }
                }
            }

            public void Dispose()
            {
                _progressTimer?.Stop();
                _progressTimer?.Dispose();
            }
        }
    }
}