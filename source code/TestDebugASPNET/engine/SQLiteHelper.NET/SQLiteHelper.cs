using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

// https://github.com/adriancs2/SQLiteHelper.net

namespace System.Data.SQLite
{
    public enum ColType
    {
        Text,
        DateTime,
        Integer,
        Decimal,
        BLOB
    }

    public class SQLiteHelper
    {
        SQLiteCommand cmd = null;

        public SQLiteHelper(SQLiteCommand command)
        {
            cmd = command;
        }

        #region DB Info

        public DataTable GetTableStatus()
        {
            return Select("SELECT * FROM sqlite_master;");
        }

        public DataTable GetTableList()
        {
            DataTable dt = GetTableStatus();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Tables");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t = dt.Rows[i]["name"] + "";
                if (t != "sqlite_sequence")
                    dt2.Rows.Add(t);
            }
            return dt2;
        }

        public DataTable GetColumnStatus(string tableName)
        {
            return Select(string.Format("PRAGMA table_info(`{0}`);", tableName));
        }

        public DataTable ShowDatabase()
        {
            return Select("PRAGMA database_list;");
        }

        #endregion

        #region Query

        public void BeginTransaction()
        {
            cmd.CommandText = "begin transaction;";
            cmd.ExecuteNonQuery();
        }

        public void Commit()
        {
            cmd.CommandText = "commit;";
            cmd.ExecuteNonQuery();
        }

        public void Rollback()
        {
            cmd.CommandText = "rollback";
            cmd.ExecuteNonQuery();
        }

        public DataTable Select(string sql)
        {
            return Select(sql, new List<SQLiteParameter>());
        }

        public DataTable Select(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = GetParametersList(dicParameters);
            return Select(sql, lst);
        }

        public DataTable Select(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public void Execute(string sql)
        {
            Execute(sql, new List<SQLiteParameter>());
        }

        public void Execute(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = GetParametersList(dicParameters);
            Execute(sql, lst);
        }

        public void Execute(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }
            cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql)
        {
            cmd.CommandText = sql;
            return cmd.ExecuteScalar();
        }

        public object ExecuteScalar(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = GetParametersList(dicParameters);
            return ExecuteScalar(sql, lst);
        }

        public object ExecuteScalar(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            return cmd.ExecuteScalar();
        }

        public dataType ExecuteScalar<dataType>(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = null;
            if (dicParameters != null)
            {
                lst = new List<SQLiteParameter>();
                foreach (KeyValuePair<string, object> kv in dicParameters)
                {
                    lst.Add(new SQLiteParameter(kv.Key, kv.Value));
                }
            }
            return ExecuteScalar<dataType>(sql, lst);
        }

        public dataType ExecuteScalar<dataType>(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
            return (dataType)Convert.ChangeType(cmd.ExecuteScalar(), typeof(dataType));
        }

        public dataType ExecuteScalar<dataType>(string sql)
        {
            cmd.CommandText = sql;
            return (dataType)Convert.ChangeType(cmd.ExecuteScalar(), typeof(dataType));
        }

        private List<SQLiteParameter> GetParametersList(Dictionary<string, object> dicParameters)
        {
            List<SQLiteParameter> lst = new List<SQLiteParameter>();
            if (dicParameters != null)
            {
                foreach (KeyValuePair<string, object> kv in dicParameters)
                {
                    lst.Add(new SQLiteParameter(kv.Key, kv.Value));
                }
            }
            return lst;
        }

        public string Escape(string data)
        {
            data = data.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            return data;
        }

        public void Insert(string tableName, Dictionary<string, object> dic)
        {
            StringBuilder sbCol = new System.Text.StringBuilder();
            StringBuilder sbVal = new System.Text.StringBuilder();

            foreach (KeyValuePair<string, object> kv in dic)
            {
                if (sbCol.Length == 0)
                {
                    sbCol.Append("insert into ");
                    sbCol.Append(tableName);
                    sbCol.Append("(");
                }
                else
                {
                    sbCol.Append(",");
                }

                sbCol.Append("`");
                sbCol.Append(kv.Key);
                sbCol.Append("`");

                if (sbVal.Length == 0)
                {
                    sbVal.Append(" values(");
                }
                else
                {
                    sbVal.Append(", ");
                }

                sbVal.Append("@v");
                sbVal.Append(kv.Key);
            }

            sbCol.Append(") ");
            sbVal.Append(");");

            cmd.CommandText = sbCol.ToString() + sbVal.ToString();

            foreach (KeyValuePair<string, object> kv in dic)
            {
                cmd.Parameters.AddWithValue("@v" + kv.Key, kv.Value);
            }

            cmd.ExecuteNonQuery();
        }

        public void Update(string tableName, Dictionary<string, object> dicData, string colCond, object varCond)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic[colCond] = varCond;
            Update(tableName, dicData, dic);
        }

        public void Update(string tableName, Dictionary<string, object> dicData, Dictionary<string, object> dicCond)
        {
            if (dicData.Count == 0)
                throw new Exception("dicData is empty.");

            StringBuilder sbData = new System.Text.StringBuilder();

            Dictionary<string, object> _dicTypeSource = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kv1 in dicData)
            {
                _dicTypeSource[kv1.Key] = null;
            }

            foreach (KeyValuePair<string, object> kv2 in dicCond)
            {
                if (!_dicTypeSource.ContainsKey(kv2.Key))
                    _dicTypeSource[kv2.Key] = null;
            }

            sbData.Append("update `");
            sbData.Append(tableName);
            sbData.Append("` set ");

            bool firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicData)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                    sbData.Append(",");

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@v");
                sbData.Append(kv.Key);
            }

            sbData.Append(" where ");

            firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicCond)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                {
                    sbData.Append(" and ");
                }

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@c");
                sbData.Append(kv.Key);
            }

            sbData.Append(";");

            cmd.CommandText = sbData.ToString();

            foreach (KeyValuePair<string, object> kv in dicData)
            {
                cmd.Parameters.AddWithValue("@v" + kv.Key, kv.Value);
            }

            foreach (KeyValuePair<string, object> kv in dicCond)
            {
                cmd.Parameters.AddWithValue("@c" + kv.Key, kv.Value);
            }

            cmd.ExecuteNonQuery();
        }

        public long LastInsertRowId()
        {
            return ExecuteScalar<long>("select last_insert_rowid();");
        }

        public T GetObject<T>(string sql)
        {
            DataTable dt = Select(sql);

            return Bind<T>(dt);
        }

        public T GetObject<T>(string sql, Dictionary<string, object> dicParameters)
        {
            DataTable dt = Select(sql, dicParameters);

            return Bind<T>(dt);
        }

        public List<T> GetObjectList<T>(string sql)
        {
            DataTable dt = Select(sql);
            return BindList<T>(dt);
        }

        public List<T> GetObjectList<T>(string sql, Dictionary<string, object> dicParameters)
        {
            DataTable dt = Select(sql, dicParameters);
            return BindList<T>(dt);
        }

        static T Bind<T>(DataTable dt)
        {
            List<T> lst = BindList<T>(dt);
            if (lst.Count == 0)
            {
                return Activator.CreateInstance<T>();
            }
            return lst[0];
        }

        static List<T> BindList<T>(DataTable dt)
        {
            var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (fieldInfo.Name == dc.ColumnName)
                        {
                            fieldInfo.SetValue(ob, GetValue(dr[dc.ColumnName], fieldInfo.FieldType));
                            break;
                        }
                    }
                }

                foreach (var propertyInfo in properties)
                {
                    if (!propertyInfo.CanWrite)
                        continue;

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (propertyInfo.Name == dc.ColumnName)
                        {
                            propertyInfo.SetValue(ob, GetValue(dr[dc.ColumnName], propertyInfo.PropertyType));
                            break;
                        }
                    }
                }

                lst.Add(ob);
            }

            return lst;
        }

        static object GetValue(object ob, Type t)
        {
            if (t == typeof(string))
            {
                return ob + "";
            }
            else if (t == typeof(bool))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return false;
                return Convert.ToBoolean(ob);
            }
            else if (t == typeof(byte))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToByte(ob);
            }
            else if (t == typeof(sbyte))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToSByte(ob);
            }
            else if (t == typeof(short))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToInt16(ob);
            }
            else if (t == typeof(ushort))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToUInt16(ob);
            }
            else if (t == typeof(int))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToInt32(ob);
            }
            else if (t == typeof(uint))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0;
                return Convert.ToUInt32(ob);
            }
            else if (t == typeof(long))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0L;
                return Convert.ToInt64(ob);
            }
            else if (t == typeof(ulong))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0L;
                return Convert.ToUInt64(ob);
            }
            else if (t == typeof(float))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0F;
                return Convert.ToSingle(ob);
            }
            else if (t == typeof(double))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0D;
                return Convert.ToDouble(ob, CultureInfo.InvariantCulture);
            }
            else if (t == typeof(decimal))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return 0m;
                return Convert.ToDecimal(ob, CultureInfo.InvariantCulture);
            }
            else if (t == typeof(char))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return Convert.ToChar("");
                return Convert.ToChar(ob);
            }
            else if (t == typeof(DateTime))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return DateTime.MinValue;
                return Convert.ToDateTime(ob, CultureInfo.InvariantCulture);
            }
            else if (t == typeof(byte[]))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return null;

                return (byte[])ob;
            }
            else if (t == typeof(Guid))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return null;

                return (Guid)ob;
            }
            else if (t == typeof(TimeSpan))
            {
                if (ob == null || ob.GetType() == typeof(DBNull))
                    return null;

                return (TimeSpan)ob;
            }

            return Convert.ChangeType(ob, t);
        }
        #endregion

        #region Utilities

        public void CreateTable(SQLiteTable table)
        {
            StringBuilder sb = new Text.StringBuilder();
            sb.Append("create table if not exists `");
            sb.Append(table.TableName);
            sb.AppendLine("`(");

            bool firstRecord = true;

            foreach (SQLiteColumn col in table.Columns)
            {
                if (col.ColumnName.Trim().Length == 0)
                {
                    throw new Exception("Column name cannot be blank.");
                }

                if (firstRecord)
                    firstRecord = false;
                else
                    sb.AppendLine(",");

                sb.Append(col.ColumnName);
                sb.Append(" ");

                if (col.AutoIncrement)
                {

                    sb.Append("integer primary key autoincrement");
                    continue;
                }

                switch (col.ColDataType)
                {
                    case ColType.Text:
                        sb.Append("text"); break;
                    case ColType.Integer:
                        sb.Append("integer"); break;
                    case ColType.Decimal:
                        sb.Append("decimal"); break;
                    case ColType.DateTime:
                        sb.Append("datetime"); break;
                    case ColType.BLOB:
                        sb.Append("blob"); break;
                }

                if (col.PrimaryKey)
                    sb.Append(" primary key");
                else if (col.NotNull)
                    sb.Append(" not null");
                else if (col.DefaultValue.Length > 0)
                {
                    sb.Append(" default ");

                    if (col.DefaultValue.Contains(" ") || col.ColDataType == ColType.Text || col.ColDataType == ColType.DateTime)
                    {
                        sb.Append("'");
                        sb.Append(col.DefaultValue);
                        sb.Append("'");
                    }
                    else
                    {
                        sb.Append(col.DefaultValue);
                    }
                }
            }

            sb.AppendLine(");");

            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();
        }

        public void RenameTable(string tableFrom, string tableTo)
        {
            cmd.CommandText = string.Format("alter table `{0}` rename to `{1}`;", tableFrom, tableTo);
            cmd.ExecuteNonQuery();
        }

        public void CopyAllData(string tableFrom, string tableTo)
        {
            DataTable dt1 = Select(string.Format("select * from `{0}` where 1 = 2;", tableFrom));
            DataTable dt2 = Select(string.Format("select * from `{0}` where 1 = 2;", tableTo));

            Dictionary<string, bool> dic = new Dictionary<string, bool>();

            foreach (DataColumn dc in dt1.Columns)
            {
                if (dt2.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            foreach (DataColumn dc in dt2.Columns)
            {
                if (dt1.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            StringBuilder sb = new Text.StringBuilder();

            foreach (KeyValuePair<string, bool> kv in dic)
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append("`");
                sb.Append(kv.Key);
                sb.Append("`");
            }

            StringBuilder sb2 = new Text.StringBuilder();
            sb2.Append("insert into `");
            sb2.Append(tableTo);
            sb2.Append("`(");
            sb2.Append(sb.ToString());
            sb2.Append(") select ");
            sb2.Append(sb.ToString());
            sb2.Append(" from `");
            sb2.Append(tableFrom);
            sb2.Append("`;");

            cmd.CommandText = sb2.ToString();
            cmd.ExecuteNonQuery();
        }

        public void DropTable(string table)
        {
            cmd.CommandText = string.Format("drop table if exists `{0}`", table);
            cmd.ExecuteNonQuery();
        }

        public void UpdateTableStructure(string targetTable, SQLiteTable newStructure)
        {
            newStructure.TableName = targetTable + "_temp";

            CreateTable(newStructure);

            CopyAllData(targetTable, newStructure.TableName);

            DropTable(targetTable);

            RenameTable(newStructure.TableName, targetTable);
        }

        public void AttachDatabase(string database, string alias)
        {
            Execute(string.Format("attach '{0}' as {1};", database, alias));
        }

        public void DetachDatabase(string alias)
        {
            Execute(string.Format("detach {0};", alias));
        }

        #endregion

    }
}