using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySqlConnector;
using System.Data.SQLite;
using static System.Net.WebRequestMethods;

namespace System
{
    public partial class Default : System.Web.UI.Page
    {
        string folder
        {
            get
            {
                return EngineSQLite.folder;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadConstr();
            }
        }

        void LoadConstr()
        {
            txtConnStr.Text = config.ConnString;

            if (config.TestConnectionOk())
            {
                LoadDatabaseInfo();
            }
        }

        (bool, string) LoadDatabaseInfo()
        {
            DataTable dtTable = null;
            List<string> lstDocHeaders = null;
            List<string> lstDocFooters = null;

            using (MySqlConnection conn = config.GetNewConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    string dbname = QueryExpress.ExecuteScalarStr(cmd, "select database();");

                    if (dbname == null)
                    {
                        return (false, "No database is selected");
                    }

                    dtTable = QueryExpress.GetTable(cmd, "SHOW FULL TABLES WHERE Table_type = 'BASE TABLE';");

                    ExportInformations ef = new ExportInformations();
                    lstDocHeaders = ef.GetDocumentHeaders(cmd);
                    lstDocFooters = ef.GetDocumentFooters();
                }
            }

            txtScriptDelimiter.Text = "|";

            cbListIncludeTables.DataSource = dtTable;
            cbListIncludeTables.DataValueField = dtTable.Columns[0].ColumnName;
            cbListIncludeTables.DataTextField = dtTable.Columns[0].ColumnName;

            cbListExcludeTables.DataSource = dtTable;
            cbListExcludeTables.DataValueField = dtTable.Columns[0].ColumnName;
            cbListExcludeTables.DataTextField = dtTable.Columns[0].ColumnName;

            cbListExcludeRowsForTables.DataSource = dtTable;
            cbListExcludeRowsForTables.DataValueField = dtTable.Columns[0].ColumnName;
            cbListExcludeRowsForTables.DataTextField = dtTable.Columns[0].ColumnName;

            txtDocumentHeaders.Text = string.Join(Environment.NewLine, lstDocHeaders);
            txtDocumentFooters.Text = string.Join(Environment.NewLine, lstDocFooters);

            return (true, "");
        }

        void LoadDatabaseList()
        {

        }

        protected void btSaveConnStr_Click(object sender, EventArgs e)
        {
            config.SaveConnStr(txtConnStr.Text);

            try
            {
                string timenow = "";

                using (MySqlConnection conn = config.GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        cmd.CommandText = "select now();";
                        timenow = cmd.ExecuteScalar() + "";
                    }
                }

                ((masterPage1)this.Master).ShowGoodMessage("Connection Success");
                ((masterPage1)this.Master).WriteMessageBar($"Connection string saved and the connection test is success. {timenow}", true);
            }
            catch (Exception ex)
            {
                ((masterPage1)this.Master).ShowErrorMessage($"Connection Failed. Error: {ex.Message}");
            }
        }

        protected void btCreateSampleData_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = config.GetNewConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();
                        cmd.Connection = conn;

                        // Drop table if exists
                        cmd.CommandText = "DROP TABLE IF EXISTS table1;";
                        cmd.ExecuteNonQuery();

                        // Create table with all MySQL data types
                        cmd.CommandText = @"
                CREATE TABLE table1 (
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

                        // Insert 10 sample rows
                        for (int i = 1; i <= 10; i++)
                        {
                            cmd.CommandText = $@"
                    INSERT INTO table1 (
                        col_tinyint, col_smallint, col_mediumint, col_int, col_bigint,
                        col_decimal, col_numeric, col_float, col_double, col_bit, col_bool,
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

                        // Create additional tables to test relationships

                        cmd.CommandText = "DROP TABLE IF EXISTS test_child;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "DROP TABLE IF EXISTS test_parent;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS test_parent (
                    parent_id INT AUTO_INCREMENT PRIMARY KEY,
                    parent_name VARCHAR(100)
                );";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS test_child (
                    child_id INT AUTO_INCREMENT PRIMARY KEY,
                    parent_id INT,
                    child_name VARCHAR(100),
                    FOREIGN KEY (parent_id) REFERENCES test_parent(parent_id)
                );";
                        cmd.ExecuteNonQuery();

                        // Insert sample data for relationship testing
                        for (int i = 1; i <= 5; i++)
                        {
                            cmd.CommandText = $"INSERT INTO test_parent (parent_name) VALUES ('Parent {i}');";
                            cmd.ExecuteNonQuery();
                        }

                        for (int i = 1; i <= 10; i++)
                        {
                            cmd.CommandText = $"INSERT INTO test_child (parent_id, child_name) VALUES ({(i % 5) + 1}, 'Child {i}');";
                            cmd.ExecuteNonQuery();
                        }

                        // Create a view for testing
                        cmd.CommandText = @"
                CREATE OR REPLACE VIEW test_view AS
                SELECT 
                    id, col_int, col_varchar, col_datetime, col_json
                FROM table1
                WHERE col_int > 50000;";
                        cmd.ExecuteNonQuery();

                        // Create a stored procedure for testing
                        cmd.CommandText = @"
                DROP PROCEDURE IF EXISTS test_procedure;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE PROCEDURE test_procedure(IN input_id INT)
                BEGIN
                    SELECT * FROM table1 WHERE id = input_id;
                END;";
                        cmd.ExecuteNonQuery();

                        // Create a function for testing
                        cmd.CommandText = @"
                DROP FUNCTION IF EXISTS test_function;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE FUNCTION test_function(input_value INT) RETURNS INT
                DETERMINISTIC
                BEGIN
                    RETURN input_value * 2;
                END;";
                        cmd.ExecuteNonQuery();

                        // Create a trigger for testing
                        cmd.CommandText = @"
                DROP TRIGGER IF EXISTS test_trigger;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE TRIGGER test_trigger
                BEFORE INSERT ON test_parent
                FOR EACH ROW
                BEGIN
                    SET NEW.parent_name = CONCAT('AUTO_', NEW.parent_name);
                END;";
                        cmd.ExecuteNonQuery();

                        // Create an event for testing (if event scheduler is enabled)
                        cmd.CommandText = @"
                DROP EVENT IF EXISTS test_event;";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"
                CREATE EVENT IF NOT EXISTS test_event
                ON SCHEDULE EVERY 1 HOUR
                DO
                BEGIN
                    INSERT INTO test_parent (parent_name) VALUES (CONCAT('Event_', NOW()));
                END;";
                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }

                ((masterPage1)this.Master).ShowGoodMessage("Success! Sample table and data rows are created.");
            }
            catch (Exception ex)
            {
                ((masterPage1)this.Master).ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        protected void btGetDatabaseInfo_Click(object sender, EventArgs e)
        {
            try
            {
                var r = LoadDatabaseInfo();
                if (r.Item1)
                {
                    ((masterPage1)this.Master).ShowGoodMessage("Database info successfully obtained");
                }
                else
                {
                    ((masterPage1)this.Master).ShowErrorMessage(r.Item2);
                }
            }
            catch (Exception ex)
            {
                ((masterPage1)this.Master).ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        protected void btRunBackup_Click(object sender, EventArgs e)
        {
            // Variables to store backup metadata
            DatabaseFileRecord dbFile = new DatabaseFileRecord();
            dbFile.Operation = "Basic Demo";
            dbFile.Filename = $"BasicDemo-{DateTime.Now:yyyy-MM-dd_HHmmss}.sql";
            dbFile.OriginalFilename = dbFile.Filename;
            dbFile.DateCreated = DateTime.Now;
            dbFile.Remarks = "Backup";

            string dumpFile = Path.Combine(folder, dbFile.Filename);

            using (MySqlConnection conn = config.GetNewConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;

                    dbFile.DatabaseName = QueryExpress.ExecuteScalarStr(cmd, "select database();");

                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        #region Setup Export Info
                        mb.ExportInfo.AddDropDatabase = cbAddDropDatabase.Checked;
                        mb.ExportInfo.AddCreateDatabase = cbAddCreateDatabase.Checked;
                        mb.ExportInfo.AddDropTable = cbAddDropTable.Checked;
                        mb.ExportInfo.ExportTableStructure = cbExportTableStructure.Checked;
                        mb.ExportInfo.ExportRows = cbExportRows.Checked;
                        mb.ExportInfo.ExportProcedures = cbExportProcedures.Checked;
                        mb.ExportInfo.ExportFunctions = cbExportFunctions.Checked;
                        mb.ExportInfo.ExportTriggers = cbExportTriggers.Checked;
                        mb.ExportInfo.ExportViews = cbExportViews.Checked;
                        mb.ExportInfo.ExportRoutinesWithoutDefiner = cbExportRoutinesWithoutDefiner.Checked;
                        mb.ExportInfo.ResetAutoIncrement = cbResetAutoIncrement.Checked;
                        mb.ExportInfo.WrapWithinTransaction = cbWrapWithinTransaction.Checked;
                        mb.ExportInfo.EnableComment = cbEnableComments.Checked;
                        mb.ExportInfo.InsertLineBreakBetweenInserts = cbInsertLineBreakBetweenInserts.Checked;

                        if (!string.IsNullOrWhiteSpace(txtScriptDelimiter.Text))
                        {
                            mb.ExportInfo.ScriptsDelimiter = txtScriptDelimiter.Text;
                        }
                        else
                        {
                            mb.ExportInfo.ScriptsDelimiter = "|";
                        }

                        if (!string.IsNullOrWhiteSpace(txtMaxSqlLength.Text) && int.TryParse(txtMaxSqlLength.Text, out int maxLength))
                        {
                            mb.ExportInfo.MaxSqlLength = maxLength;
                        }
                        else
                        {
                            mb.ExportInfo.MaxSqlLength = 16 * 1024 * 1024;
                        }

                        mb.ExportInfo.RowsExportMode = (RowsDataExportMode)int.Parse(dropRowsExportMode.SelectedValue);
                        mb.ExportInfo.GetTotalRowsMode = (GetTotalRowsMethod)int.Parse(dropGetTotalRowsMode.SelectedValue);

                        if (!string.IsNullOrWhiteSpace(txtDocumentHeaders.Text))
                        {
                            var headers = txtDocumentHeaders.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            mb.ExportInfo.SetDocumentHeaders(headers.ToList());
                        }

                        if (!string.IsNullOrWhiteSpace(txtDocumentFooters.Text))
                        {
                            var footers = txtDocumentFooters.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                            mb.ExportInfo.SetDocumentFooters(footers.ToList());
                        }

                        List<string> includeTables = new List<string>();
                        foreach (ListItem item in cbListIncludeTables.Items)
                        {
                            if (item.Selected)
                            {
                                includeTables.Add(item.Value);
                            }
                        }
                        if (includeTables.Count > 0)
                        {
                            mb.ExportInfo.TablesToBeExportedList = includeTables;
                        }

                        foreach (ListItem item in cbListExcludeTables.Items)
                        {
                            if (item.Selected)
                            {
                                mb.ExportInfo.ExcludeTables.Add(item.Value);
                            }
                        }

                        foreach (ListItem item in cbListExcludeRowsForTables.Items)
                        {
                            if (item.Selected)
                            {
                                mb.ExportInfo.ExcludeRowsForTables.Add(item.Value);
                            }
                        }
                        #endregion

                        mb.ExportToFile(dumpFile);
                    }
                }
            }

            // get the file size
            dbFile.Filesize = new FileInfo(dumpFile).Length;

            // calculate the SHA256 hash
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            using (var stream = System.IO.File.OpenRead(dumpFile))
            {
                byte[] hash = sha256.ComputeHash(stream);
                dbFile.Sha256 = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            EngineSQLite.SaveRecord(dbFile);
        }

        protected void btRunRestore_Click(object sender, EventArgs e)
        {
            string dumpFile = Path.Combine(folder, $"{DateTime.Now}");
        }
    }
}