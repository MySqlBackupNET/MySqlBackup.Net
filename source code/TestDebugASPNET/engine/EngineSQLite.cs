using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace System
{
    public static class EngineSQLite
    {
        public static string folder
        {
            get
            {
                string f = HttpContext.Current.Server.MapPath("~/App_Data/backup");
                Directory.CreateDirectory(f);
                return f;
            }
        }

        public static string sqliteConnectionString
        {
            get
            {
                string f = Path.Combine(folder, "data.sqlite3");
                string c = $"Data Source={f};Version=3;";
                if (!File.Exists(f))
                {
                    using (var connection = new SQLiteConnection(c))
                    {
                        connection.Open();

                        string createTableSql = @"
                            CREATE TABLE IF NOT EXISTS DatabaseFile (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Operation TEXT,
                                Filename TEXT,
                                OriginalFilename TEXT,
                                Sha256 TEXT,
                                Filesize INTEGER,
                                DatabaseName TEXT,
                                DateCreated DATETIME,
                                Remarks TEXT
                            )";

                        using (var cmd = new SQLiteCommand(createTableSql, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                return c;
            }
        }

        public static void SaveRecord(DatabaseFileRecord dbFile)
        {
            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO DatabaseFile (Operation, Filename, OriginalFilename, Sha256, Filesize, DatabaseName, DateCreated, Remarks)
                    VALUES (@Operation, @Filename, @OriginalFilename, @Sha256, @Filesize, @DatabaseName, @DateCreated, @Remarks)";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Operation", dbFile.Operation ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Filename", dbFile.Filename ?? string.Empty);
                    cmd.Parameters.AddWithValue("@OriginalFilename", dbFile.OriginalFilename ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Sha256", dbFile.Sha256 ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Filesize", dbFile.Filesize);
                    cmd.Parameters.AddWithValue("@DatabaseName", dbFile.DatabaseName ?? string.Empty);
                    cmd.Parameters.AddWithValue("@DateCreated", dbFile.DateCreated);
                    cmd.Parameters.AddWithValue("@Remarks", dbFile.Remarks ?? string.Empty);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteRecord(int id)
        {
            string filename = "";
            int targetRecordId = 0;

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Filename FROM DatabaseFile WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            targetRecordId = reader.GetInt32(0);
                            filename = reader.GetString(1);
                        }
                    }
                }

                if (targetRecordId > 0)
                {
                    using (var cmd = new SQLiteCommand("DELETE FROM DatabaseFile WHERE Id = @Id", connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            if (targetRecordId > 0 && !string.IsNullOrEmpty(filename))
            {
                string filePath = Path.Combine(folder, filename);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public static string GetRecordFileContent(int id)
        {
            string filename = "";
            int targetRecordId = 0;

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                // Use parameterized query to prevent SQL injection
                string sql = "SELECT Id, Filename FROM DatabaseFile WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            targetRecordId = reader.GetInt32(0);
                            filename = reader.GetString(1);
                        }
                    }
                }
            }

            if (targetRecordId > 0 && !string.IsNullOrEmpty(filename))
            {
                string filePath = Path.Combine(folder, filename);
                if (File.Exists(filePath))
                {
                    return File.ReadAllText(filePath);
                }
            }

            return "";
        }

        public static List<DatabaseFileRecord> GetRecordList(string operation)
        {
            List<DatabaseFileRecord> lst = new List<DatabaseFileRecord>();

            using (var connection = new SQLiteConnection(sqliteConnectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM DatabaseFile WHERE Operation = @Operation ORDER BY DateCreated DESC";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@Operation", operation);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DatabaseFileRecord dbFile = new DatabaseFileRecord()
                            {
                                Id = (int)reader["Id"],
                                Operation = reader["Operation"] + "",
                                Filename = reader["Filename"] + "",
                                OriginalFilename = reader["OriginalFilename"] + "",
                                Sha256 = reader["Sha256"] + "",
                                Filesize = Convert.ToInt64(reader["Filesize"]),
                                DatabaseName = reader["DatabaseName"] + "",
                                DateCreated = (DateTime)reader["DateCreated"],
                                Remarks = reader["Remarks"] + ""
                            };

                            lst.Add(dbFile);
                        }
                    }
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

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var file in filesToDelete)
                        {
                            // Delete from database
                            using (var cmd = new SQLiteCommand("DELETE FROM DatabaseFile WHERE Id = @Id", connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Id", file.Id);
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
    }
}