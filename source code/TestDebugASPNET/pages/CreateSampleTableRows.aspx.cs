using MySqlConnector;
using System;
using System.Collections.Generic;
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
                // Fire and forget - start the background task
                _ = Task.Run(() => GenerateData(totalRows));

                // Redirect to progress report page at the client side
                Header.Controls.Add(new LiteralControl("<script>window.location = '/ReportProgress';</script>"));
            }
            else
            {
                ((masterPage1)this.Master).WriteTopMessageBar("Invalid total rows", false);
                ((masterPage1)this.Master).ShowMessage("Error", "Invalid total rows", false);
            }
        }

        // Remove async since the actual work is synchronous
        private void GenerateData(int totalRows)
        {
            try
            {
                var generator = new SampleTableRowsGenerator(config.ConnString);
                generator.ProgressChanged += T_ProgressChanged;
                generator.ProgressCompleted += T_ProgressCompleted;

                // This is synchronous - no need for Task.Run here
                generator.GenerateTableRows(totalRows);
            }
            catch (Exception ex)
            {
                // Log the error since this is fire-and-forget
                // You might want to write to your logging system here
                System.Diagnostics.Debug.WriteLine($"Error in GenerateData: {ex.Message}");

                // Optionally notify through your progress tracking system
                T_ProgressCompleted(this, new ProgressCompletedEventArgs(true, ex));
            }
        }

        private void T_ProgressCompleted(object sender, ProgressCompletedEventArgs e)
        {
            try
            {
                // Write completion status to your tracking engine
                // This should be thread-safe since it's called from background thread

                if (e.HasError)
                {
                    // Write error status
                    WriteProgressToEngine($"ERROR: {e.LastError?.Message}", 0, 0, true);
                }
                else
                {
                    // Write completion status
                    WriteProgressToEngine("Completed successfully", 0, 0, false, true);
                }
            }
            catch (Exception ex)
            {
                // Log any errors in the progress reporting itself
                System.Diagnostics.Debug.WriteLine($"Error in T_ProgressCompleted: {ex.Message}");
            }
        }

        private void T_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                // Write progress data to your tracking engine
                // This should be thread-safe since it's called from background thread
                WriteProgressToEngine(
                    $"Processing: {e.CurrentRows}/{e.TotalRows}",
                    e.CurrentRows,
                    e.TotalRows,
                    false,
                    false,
                    e.PercentageComplete);
            }
            catch (Exception ex)
            {
                // Log any errors in the progress reporting itself
                System.Diagnostics.Debug.WriteLine($"Error in T_ProgressChanged: {ex.Message}");
            }
        }

        // Helper method to write progress data to your tracking engine
        private void WriteProgressToEngine(string message, int currentRows, int totalRows,
            bool hasError, bool isCompleted = false, double percentage = 0)
        {
            // This method should be thread-safe
            // Example implementation (adjust based on your tracking system):

            var progressData = new
            {
                Timestamp = DateTime.UtcNow,
                Message = message,
                CurrentRows = currentRows,
                TotalRows = totalRows,
                PercentageComplete = percentage,
                HasError = hasError,
                IsCompleted = isCompleted,
                SessionId = Session?.SessionID ?? "Unknown" // Be careful with Session in background threads
            };
        }

    public class SampleTableRowsGenerator
    {
        private readonly string _connectionString;
        private System.Timers.Timer _progressTimer;
        private int _currentRows;
        private int _totalRows;

        // Public properties
        public int IntervalForProgressReport { get; set; } = 500;

        // Events
        public event EventHandler<SampleTableRowsGenerator_ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<SampleTableRowsGenerator_ProgressCompletedEventArgs> ProgressCompleted;

        public SampleTableRowsGenerator(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _progressTimer = new System.Timers.Timer(IntervalForProgressReport);
            _progressTimer.Elapsed += OnProgressTimerElapsed;
            _progressTimer.AutoReset = true;
        }

        private void OnProgressTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (ProgressChanged != null && _totalRows > 0)
            {
                double percentage = (_currentRows / (double)_totalRows) * 100;
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(_currentRows, _totalRows, percentage));
            }
        }

        public async void GenerateTableRows(int totalRows)
        {
            _totalRows = totalRows;
            _currentRows = 0;

            try
            {
                _progressTimer.Interval = IntervalForProgressReport;
                _progressTimer.Start();

                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        // Drop table if exists
                        cmd.CommandText = "DROP TABLE IF EXISTS sample1;";
                        cmd.ExecuteNonQuery();

                        // Create table with all MySQL data types
                        cmd.CommandText = @"
CREATE TABLE sample1 (
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
    col_serial SERIAL,
    
    col_date DATE,
    col_time TIME,
    col_datetime DATETIME,
    col_timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    col_year YEAR,
    
    col_char CHAR(10),
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

                        // Insert sample rows
                        for (int i = 1; i <= totalRows; i++)
                        {
                            _currentRows = i;
                            cmd.CommandText = $@"
INSERT INTO sample1 (
    col_tinyint, col_smallint, col_mediumint, col_int, col_bigint,
    col_decimal, col_numeric, col_float, col_double, col_bit, col_bool,
    col_real, col_double_precision, col_integer, col_dec, col_fixed,
    col_date, col_time, col_datetime, col_year,
    col_char, col_varchar, col_binary, col_varbinary,
    col_tinytext, col_text, col_mediumtext, col_longtext,
    col_tinyblob, col_blob, col_mediumblob, col_longblob,
    col_json,
    col_geometry, col_point, col_linestring, col_polygon,
    col_multipoint, col_multilinestring, col_multipolygon, col_geometrycollection,
    col_enum, col_set
) VALUES (
    {i * 10}, {i * 100}, {i * 1000}, {i * 10000}, {i * 100000},
    {i * 123.45}, {i * 12.345}, {i * 1.23}, {i * 12.3456}, b'{Convert.ToString(i, 2).PadLeft(8, '0')}', {i % 2},
    {i * 1.23}, {i * 12.3456}, {i * 10000}, {i * 123.45}, {i * 12.345},
    DATE_ADD('2024-01-01', INTERVAL {i} DAY),
    TIME(CONCAT('{i % 24:D2}:', '{(i * 5) % 60:D2}:', '{(i * 3) % 60:D2}')),
    DATE_ADD('2024-01-01 12:00:00', INTERVAL {i} DAY),
    {2020 + i},
    'CHAR{i}', 'This is varchar row {i}',
    UNHEX('{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}'),
    UNHEX('{i:X2}{i:X2}{i:X2}{i:X2}'),
    'Tiny text {i}', 'This is text content for row {i}',
    'This is medium text content for row {i} with more data',
    'This is long text content for row {i} with even more data to test the long text column type',
    UNHEX('{i:X2}{i:X2}'), UNHEX('{i:X2}{i:X2}{i:X2}{i:X2}'),
    UNHEX('{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}'),
    UNHEX('{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}{i:X2}'),
    JSON_OBJECT('id', {i}, 'name', CONCAT('Item ', {i}), 'active', {(i % 2 == 0).ToString().ToLower()}),
    ST_GeomFromText('POINT({i} {i})'),
    ST_GeomFromText('POINT({i} {i * 2})'),
    ST_GeomFromText('LINESTRING(0 0, {i} {i})'),
    ST_GeomFromText('POLYGON((0 0, {i} 0, {i} {i}, 0 {i}, 0 0))'),
    ST_GeomFromText('MULTIPOINT({i} {i}, {i * 2} {i * 2})'),
    ST_GeomFromText('MULTILINESTRING((0 0, {i} {i}), ({i} {i}, {i * 2} {i * 2}))'),
    ST_GeomFromText('MULTIPOLYGON(((0 0, {i} 0, {i} {i}, 0 {i}, 0 0)))'),
    ST_GeomFromText('GEOMETRYCOLLECTION(POINT({i} {i}), LINESTRING(0 0, {i} {i}))'),
    CASE {i % 3} WHEN 0 THEN 'small' WHEN 1 THEN 'medium' ELSE 'large' END,
    CASE 
        WHEN {i % 4} = 0 THEN 'red,blue'
        WHEN {i % 4} = 1 THEN 'green'
        WHEN {i % 4} = 2 THEN 'blue,yellow'
        ELSE 'red,green,blue'
    END
);";
                            cmd.ExecuteNonQuery();
                        }

                        _progressTimer.Stop();

                        // Fire final progress event
                        if (ProgressChanged != null)
                        {
                            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(_currentRows, _totalRows, 100.0));
                        }

                        // Fire completion event
                        if (ProgressCompleted != null)
                        {
                            ProgressCompleted?.Invoke(this, new ProgressCompletedEventArgs(false, null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _progressTimer?.Stop();

                if (ProgressCompleted != null)
                {
                    ProgressCompleted?.Invoke(this, new ProgressCompletedEventArgs(true, ex));
                }
                else
                {
                    throw; // Re-throw if no subscribers to handle the error
                }
            }
        }

        public void Dispose()
        {
            _progressTimer?.Stop();
            _progressTimer?.Dispose();
        }
    }

    // Event argument classes
    public class SampleTableRowsGenerator_ProgressChangedEventArgs : EventArgs
    {
        public int CurrentRows { get; }
        public int TotalRows { get; }
        public double PercentageComplete { get; }

        public SampleTableRowsGenerator_ProgressChangedEventArgs(int currentRows, int totalRows, double percentageComplete)
        {
            CurrentRows = currentRows;
            TotalRows = totalRows;
            PercentageComplete = percentageComplete;
        }
    }

    public class SampleTableRowsGenerator_ProgressCompletedEventArgs : EventArgs
    {
        public bool HasError { get; }
        public Exception LastError { get; }

        public SampleTableRowsGenerator_ProgressCompletedEventArgs(bool hasError, Exception lastError)
        {
            HasError = hasError;
            LastError = lastError;
        }
    }
}