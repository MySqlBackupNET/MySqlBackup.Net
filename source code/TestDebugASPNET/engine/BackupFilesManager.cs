using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Description;

namespace System
{
    public static class BackupFilesManager
    {
        public static bool VariableInitialized = false;

        static string _folder = null;
        static string _tempFolder = null;
        static string _tempZipFolder = null;
        static string _sqlitecstr = null;
        static string _sqliteFilePath = null;

        public static string folder
        {
            get
            {
                return _folder;
            }
        }

        public static string tempFolder
        {
            get
            {
                return _tempFolder;
            }
        }

        public static string tempZipFolder
        {
            get
            {
                return _tempZipFolder;
            }
        }

        public static string sqliteConnectionString
        {
            get
            {
                return _sqlitecstr;
            }
        }

        public static void InitializeVariables()
        {
            _folder = HttpContext.Current.Server.MapPath("~/App_Data/backup");
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            _tempFolder = Path.Combine(folder, "temp");
            if (!Directory.Exists(_tempFolder))
                Directory.CreateDirectory(_tempFolder);

            _tempZipFolder = Path.Combine(folder, "temp-zip");
            if (!Directory.Exists(_tempZipFolder))
                Directory.CreateDirectory(_tempZipFolder);

            _sqliteFilePath = Path.Combine(_folder, "data.sqlite3");

            _sqlitecstr = $"Data Source={_sqliteFilePath};Version=3;";

            CreateSQLiteDatabaseFile();

            VariableInitialized = true;
        }

        static void CreateSQLiteDatabaseFile()
        {
            using (var connection = new SQLiteConnection(_sqlitecstr))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    string tableConfig = h.ExecuteScalar<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='Config';");

                    if (string.IsNullOrEmpty(tableConfig))
                    {
                        h.Execute(@"
                CREATE TABLE IF NOT EXISTS Config (
                    `Key` TEXT PRIMARY KEY,
                    `Value` TEXT
                )");
                        h.Execute("INSERT INTO Config (`Key`, `Value`) VALUES ('DataVersion','3');");
                        h.Execute("INSERT INTO Config (`Key`, `Value`) VALUES ('vDatabaseFile','3');");
                        h.Execute("INSERT INTO Config (`Key`, `Value`) VALUES ('vLog','3');");
                        h.Execute("INSERT INTO Config (`Key`, `Value`) VALUES ('vprogress_report','3');");
                    }

                    // Handle DatabaseFile table
                    HandleTableWithVersioning(h, "DatabaseFile", "vDatabaseFile", 3, @"
            CREATE TABLE DatabaseFile (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Operation TEXT,
                Filename TEXT,
                LogFilename TEXT,
                OriginalFilename TEXT,
                Sha256 TEXT,
                Filesize INTEGER,
                DatabaseName TEXT,
                DateCreated DATETIME,
                TaskId INTEGER,
                Remarks TEXT
            )");

                    // Handle Log table
                    HandleTableWithVersioning(h, "Log", "vLog", 3, @"
            CREATE TABLE Log (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DatabaseFileId INTEGER,
                Content TEXT
            )");

                    // Handle progress_report table
                    HandleTableWithVersioning(h, "progress_report", "vprogress_report", 3, @"
            CREATE TABLE progress_report (
                id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                operation int,
                start_time DATETIME,
                end_time DATETIME,
                is_completed INTEGER,
                has_error INTEGER,
                is_cancelled INTEGER,
                filename TEXT,
                total_tables INTEGER,
                total_rows INTEGER,
                total_rows_current_table INTEGER,
                current_table TEXT,
                current_table_index INTEGER,
                current_row INTEGER,
                current_row_in_current_table INTEGER,
                total_bytes INTEGER,
                current_bytes INTEGER,
                percent_complete INTEGER,
                remarks TEXT,
                dbfile_id INTEGER,
                last_update_time DATETIME,
                client_request_cancel_task INTEGER,
                has_file INTEGER
            )");
                }
            }
        }

        static void HandleTableWithVersioning(SQLiteHelper h, string tableName, string versionKey, int currentVersion, string createTableSql)
        {
            // Check if table exists
            string tableExists = h.ExecuteScalar<string>($"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';");

            if (string.IsNullOrEmpty(tableExists))
            {
                // Table doesn't exist, create it
                h.Execute(createTableSql);

                // Set version in config
                h.Execute($"INSERT OR REPLACE INTO Config (`Key`, `Value`) VALUES ('{versionKey}','{currentVersion}');");
            }
            else
            {
                // Table exists, check version
                string versionStr = h.ExecuteScalar<string>($"SELECT Value FROM Config WHERE Key = '{versionKey}';");

                if (int.TryParse(versionStr, out int existingVersion))
                {
                    if (existingVersion < currentVersion)
                    {
                        // Perform upgrade
                        UpgradeTable(h, tableName, versionKey, currentVersion, createTableSql);
                    }
                    // If existingVersion == currentVersion, do nothing
                }
                else
                {
                    // Version not found or invalid, treat as upgrade needed
                    UpgradeTable(h, tableName, versionKey, currentVersion, createTableSql);
                }
            }
        }

        static void UpgradeTable(SQLiteHelper h, string tableName, string versionKey, int newVersion, string createTableSql)
        {
            string oldTable = tableName;
            string tempTable = tableName + "_temp";

            try
            {
                // Create new table with temp name
                string tempCreateSql = createTableSql.Replace($"CREATE TABLE {tableName}", $"CREATE TABLE {tempTable}");
                h.Execute(tempCreateSql);

                // Copy data from old table to temp table
                h.CopyAllData(oldTable, tempTable);

                // Drop old table
                h.DropTable(oldTable);

                // Rename temp table to original name
                h.RenameTable(tempTable, oldTable);

                // Update version in config
                h.Execute($"INSERT OR REPLACE INTO Config (`Key`, `Value`) VALUES ('{versionKey}','{newVersion}');");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upgrade table {tableName}: {ex.Message}", ex);
            }
        }

        public static int SaveRecord(DatabaseFileRecord dbFile)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["Operation"] = dbFile.Operation;
            dic["Filename"] = dbFile.Filename;
            dic["OriginalFilename"] = dbFile.OriginalFilename;
            dic["LogFilename"] = dbFile.LogFilename;
            dic["Sha256"] = dbFile.Sha256;
            dic["Filesize"] = dbFile.Filesize;
            dic["DatabaseName"] = dbFile.DatabaseName;
            dic["DateCreated"] = dbFile.DateCreated;
            dic["Remarks"] = dbFile.Remarks;
            dic["TaskId"] = dbFile.TaskId;

            int newid = 0;

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    h.Insert("DatabaseFile", dic);

                    newid = (int)connection.LastInsertRowId;
                }
            }

            return newid;
        }

        public static void DeleteRecord(int id)
        {
            List<int> lst = new List<int>();
            lst.Add(id);
            DeleteRecords(lst);
        }

        public static void DeleteRecords(List<int> lstId)
        {
            List<int> lstTargetId = new List<int>();
            List<string> lstFilename = new List<string>();

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                if (lstId.Count > 0)
                {
                    string placeholders = string.Join(",", lstId.Select((_, i) => $"@id{i}"));
                    string sql = $"SELECT Id, Filename FROM DatabaseFile WHERE Id IN ({placeholders})";

                    using (var cmd = new SQLiteCommand(sql, connection))
                    {
                        for (int i = 0; i < lstId.Count; i++)
                        {
                            cmd.Parameters.AddWithValue($"@id{i}", lstId[i]);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int targetRecordId = reader.GetInt32(0);
                                string filename = reader.GetString(1);
                                if (targetRecordId > 0)
                                {
                                    lstTargetId.Add(targetRecordId);
                                    lstFilename.Add(filename);
                                }
                            }
                        }
                    }
                }

                if (lstTargetId.Count > 0)
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = new SQLiteCommand("DELETE FROM DatabaseFile WHERE Id = @Id", connection, transaction))
                            {
                                foreach (var id in lstTargetId)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@Id", id);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }

            if (lstFilename.Count > 0)
            {
                foreach (var filename in lstFilename)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(filename))
                        {
                            string filePath = Path.Combine(folder, filename);
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                    }
                    catch { }
                }
            }
        }

        public static DatabaseFileRecord GetRecord(int id)
        {
            var lst = GetRecordList(id, null, null);
            if (lst.Count > 0)
            {
                return lst[0];
            }
            return null;
        }

        public static DatabaseFileRecord GetRecord(string filename)
        {
            var lst = GetRecordList(0, null, filename);
            if (lst.Count > 0)
            {
                return lst[0];
            }
            return null;
        }

        public static List<DatabaseFileRecord> GetRecordList(int id, string operation, string filename)
        {
            Dictionary<string, object> dicParam = new Dictionary<string, object>();

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT * FROM DatabaseFile WHERE 1=1");

            if (id != 0)
            {
                sb.Append($" AND Id = @Id");
                dicParam["@Id"] = id;
            }

            if (!string.IsNullOrEmpty(operation))
            {
                sb.Append($" AND Operation = @Operation");
                dicParam["@Operation"] = operation;
            }

            if (!string.IsNullOrEmpty(filename))
            {
                sb.Append($" AND (Filename = @Filename or LogFilename = @Filename)");
                dicParam["@Filename"] = filename;
            }

            sb.Append(" ORDER BY DateCreated DESC;");

            List<DatabaseFileRecord> lst = new List<DatabaseFileRecord>();

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    lst = h.GetObjectList<DatabaseFileRecord>(sb.ToString(), dicParam);
                }
            }

            return lst;
        }

        public static void CleanUpOldFiles(int keepMostRecent, string operation)
        {
            if (keepMostRecent < 0)
                return;

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                string sql = "";

                if (string.IsNullOrEmpty(operation))
                {
                    sql = @"
                SELECT Id, Filename 
                FROM DatabaseFile 
                ORDER BY DateCreated DESC, Id DESC
                LIMIT -1 OFFSET @offset";
                }
                else
                {
                    sql = @"
                SELECT Id, Filename 
                FROM DatabaseFile 
                WHERE Operation = @Operation
                ORDER BY DateCreated DESC, Id DESC
                LIMIT -1 OFFSET @offset";
                }

                var filesToDelete = new List<(int Id, string Filename)>();

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Operation", operation ?? "");
                    cmd.Parameters.AddWithValue("@offset", keepMostRecent);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            filesToDelete.Add((
                                reader.GetInt32(0),
                                reader.IsDBNull(1) ? "" : reader.GetString(1)
                            ));
                        }
                    }
                }

                using (var cmd = connection.CreateCommand())
                {
                    SQLiteHelper h = new SQLiteHelper(cmd);

                    h.BeginTransaction();

                    Dictionary<string, object> dicParam = new Dictionary<string, object>();

                    foreach (var file in filesToDelete)
                    {
                        dicParam["@Id"] = file.Id;
                        h.Execute("DELETE FROM DatabaseFile WHERE Id = @Id", dicParam);
                    }

                    h.Commit();
                }

                // Delete physical files after database commit
                foreach (var file in filesToDelete)
                {
                    if (!string.IsNullOrEmpty(file.Filename))
                    {
                        string filePath = Path.Combine(folder, file.Filename);
                        try
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        public static string ComputeSha256File(string filePath)
        {
            using (var sha256 = SHA256.Create())
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = sha256.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string GetFileContent(int id)
        {
            try
            {
                var dbFile = GetRecord(id);

                if (dbFile != null && dbFile.Id > 0 && !string.IsNullOrEmpty(dbFile.Filename))
                {
                    string filePath = Path.Combine(folder, dbFile.Filename);
                    if (File.Exists(filePath))
                    {
                        return File.ReadAllText(filePath);
                    }
                    else
                    {
                        return "File not exists";
                    }
                }
                else
                {
                    return "File not exists";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string GetFileContent(string filename)
        {
            try
            {
                var dbFile = GetRecord(filename);

                if (dbFile != null && dbFile.Id > 0)
                {
                    string filePath = "";

                    if (dbFile.Filename == filename)
                    {
                        filePath = Path.Combine(folder, dbFile.Filename);
                    }
                    else if (dbFile.LogFilename == filename)
                    {
                        filePath = Path.Combine(folder, dbFile.LogFilename);
                    }

                    if (File.Exists(filePath))
                    {
                        return File.ReadAllText(filePath);
                    }
                    else
                    {
                        return "File not exists";
                    }
                }
                else
                {
                    return "File not exists";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string GetFilePath(int id)
        {
            try
            {
                var dbFile = GetRecord(id);

                if (dbFile != null && dbFile.Id > 0 && !string.IsNullOrEmpty(dbFile.Filename))
                {
                    string filePath = Path.Combine(folder, dbFile.Filename);
                    if (File.Exists(filePath))
                    {
                        return filePath;
                    }
                }
            }
            catch { }

            return null;
        }

        public static bool FileExists(int id)
        {
            var filePath = GetFilePath(id);
            return filePath != null;
        }

        #region Zip File Management

        /// <summary>
        /// Gets the cache ID for a file based on its properties
        /// </summary>
        public static string GetFileCacheId(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var fileInfo = new FileInfo(filePath);
            string orifilename = Path.GetFileNameWithoutExtension(filePath);
            string cacheKey = $"{orifilename}_{fileInfo.Length}_{fileInfo.LastWriteTimeUtc.Ticks}";
            return GetMD5Hash(cacheKey);
        }

        /// <summary>
        /// Gets the zip file path for a given SQL file. Creates the zip if it doesn't exist.
        /// </summary>
        public static string GetOrCreateZipFile(string sqlFilePath)
        {
            if (!File.Exists(sqlFilePath))
                throw new FileNotFoundException($"SQL file not found: {sqlFilePath}");

            string cacheId = GetFileCacheId(sqlFilePath);
            string zipFilename = $"{cacheId}.zip";
            string zipFilePath = Path.Combine(tempZipFolder, zipFilename);

            // If zip already exists, update access time and return
            if (File.Exists(zipFilePath))
            {
                File.SetLastAccessTime(zipFilePath, DateTime.Now);
                return zipFilePath;
            }

            // Create the zip file
            CreateZipFile(sqlFilePath, cacheId);
            return zipFilePath;
        }

        /// <summary>
        /// Creates a zip file for the given SQL file
        /// </summary>
        private static void CreateZipFile(string sqlFilePath, string cacheId)
        {
            string zipFilePath = Path.Combine(tempZipFolder, $"{cacheId}.zip");

            using (var zip = ZipStorer.Create(zipFilePath, ""))
            {
                string entryName = Path.GetFileName(sqlFilePath);
                zip.AddFile(ZipStorer.Compression.Deflate, sqlFilePath, entryName, "");
            }
        }

        /// <summary>
        /// Pre-generates a zip file for a SQL backup file (useful after backup completion)
        /// </summary>
        public static void PreGenerateZipFile(string sqlFilePath)
        {
            try
            {
                GetOrCreateZipFile(sqlFilePath);
            }
            catch (Exception ex)
            {
                // Log error but don't throw - pre-generation is optional
                // You might want to log this to your logging system
            }
        }

        /// <summary>
        /// Cleans up old temporary files
        /// </summary>
        public static void CleanupOldTempFiles(int hoursToKeep = 3)
        {
            try
            {
                var cutoffTime = DateTime.Now.AddHours(-hoursToKeep);

                // Clean old GUID folders in temp folder
                if (Directory.Exists(tempFolder))
                {
                    foreach (var dir in Directory.GetDirectories(tempFolder))
                    {
                        try
                        {
                            if (Directory.GetCreationTime(dir) < cutoffTime)
                            {
                                Directory.Delete(dir, true);
                            }
                        }
                        catch { }
                    }
                }

                // Clean old zip files in temp-zip folder (use LastAccessTime)
                if (Directory.Exists(tempZipFolder))
                {
                    foreach (var file in Directory.GetFiles(tempZipFolder, "*.zip"))
                    {
                        try
                        {
                            if (File.GetLastAccessTime(file) < cutoffTime)
                            {
                                File.Delete(file);
                            }
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        private static string GetMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        #endregion

    }
}