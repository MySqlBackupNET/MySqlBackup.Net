using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.Types;

namespace MySql.Data.MySqlClient
{
    public class QueryExpress
    {
        internal static NumberFormatInfo _numberFormatInfo = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = string.Empty
        };

        internal static DateTimeFormatInfo _dateFormatInfo = new DateTimeFormatInfo()
        {
            DateSeparator = "-",
            TimeSeparator = ":"
        };

        public static NumberFormatInfo MySqlNumberFormat { get { return _numberFormatInfo; } }

        public static DateTimeFormatInfo MySqlDateTimeFormat { get { return _dateFormatInfo; } }

        public static DataTable GetTable(MySqlCommand cmd, string sql)
        {
            DataTable dt = new DataTable();
            cmd.CommandText = sql;
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public static string ExecuteScalarStr(MySqlCommand cmd, string sql)
        {
            cmd.CommandText = sql;
            object ob = cmd.ExecuteScalar();
            if (ob is byte[])
                return Encoding.UTF8.GetString((byte[])ob);
            else
                return ob + "";
        }

        public static string ExecuteScalarStr(MySqlCommand cmd, string sql, int columnIndex)
        {
            DataTable dt = GetTable(cmd, sql);

            if (dt.Rows[0][columnIndex] is byte[])
                return Encoding.UTF8.GetString((byte[])dt.Rows[0][columnIndex]);
            else
                return dt.Rows[0][columnIndex] + "";
        }

        public static string ExecuteScalarStr(MySqlCommand cmd, string sql, string columnName)
        {
            DataTable dt = GetTable(cmd, sql);

            if (dt.Rows[0][columnName] is byte[])
                return Encoding.UTF8.GetString((byte[])dt.Rows[0][columnName]);
            else
                return dt.Rows[0][columnName] + "";
        }

        public static long ExecuteScalarLong(MySqlCommand cmd, string sql)
        {
            long l = 0;
            cmd.CommandText = sql;
            long.TryParse(cmd.ExecuteScalar() + "", out l);
            return l;
        }

        public static void EscapeStringSequence(StringBuilder builder, string data)
        {
            foreach (char c in data)
            {
                switch (c)
                {
                    case '\\': // Backslash character
                        builder.Append("\\\\");
                        break;
                    case '\0': // Null character (ASCII 0)
                        builder.Append("\\0");
                        break;
                    case '\r': // Carriage return character
                        builder.Append("\\r");
                        break;
                    case '\n': // Newline (line feed) character
                        builder.Append("\\n");
                        break;
                    case '\b': // Backspace character
                        builder.Append("\\b");
                        break;
                    case '\t': // Tab character
                        builder.Append("\\t");
                        break;
                    case '\x1A': // ASCII 26 (Control+Z) - END-OF-FILE on Windows
                        builder.Append("\\Z");
                        break;
                    case '\"': // Double quote character
                        builder.Append("\\\"");
                        break;
                    case '\'': // Single quote character
                               // mysqldump uses \' instead of ''
                        builder.Append("\\'");
                        break;

                    // These characters are not defined in MySQL
                    // MySQL doesn't know the meaning of these characters
                    // '\a' will become 'a'
                    //case '\a': // Alert (bell) character
                    //case '\f': // Form feed character
                    //case '\v': // Vertical tab character
                    //    builder.Append(c); // Pass through as-is
                    //    break;

                    default: // Any other character
                        builder.Append(c);
                        break;
                }
            }
        }

        public static string EraseDefiner(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Pattern explanation:
            // \s*DEFINER\s*=\s*   - "DEFINER=" with optional whitespace
            // `[^`]+`             - backtick-quoted username (any chars except backtick)
            // @                   - literal @ symbol  
            // `[^`]+`             - backtick-quoted hostname (any chars except backtick)
            // \s*                 - optional trailing whitespace

            string pattern = @"\s*DEFINER\s*=\s*`[^`]+`@`[^`]+`\s*";

            return System.Text.RegularExpressions.Regex.Replace(input, pattern, " ",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        //public static string EraseDefiner(string input)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string definer = " DEFINER=";
        //    int dIndex = input.IndexOf(definer);

        //    sb.AppendFormat(definer);

        //    bool pointAliasReached = false;
        //    bool point3rdQuoteReached = false;

        //    for (int i = dIndex + definer.Length; i < input.Length; i++)
        //    {
        //        if (!pointAliasReached)
        //        {
        //            if (input[i] == '@')
        //                pointAliasReached = true;

        //            sb.Append(input[i]);
        //            continue;
        //        }

        //        if (!point3rdQuoteReached)
        //        {
        //            if (input[i] == '`')
        //                point3rdQuoteReached = true;

        //            sb.Append(input[i]);
        //            continue;
        //        }

        //        if (input[i] != '`')
        //        {
        //            sb.Append(input[i]);
        //            continue;
        //        }
        //        else
        //        {
        //            sb.Append(input[i]);
        //            break;
        //        }
        //    }

        //    return input.Replace(sb.ToString(), string.Empty);
        //}

        /// <summary>
        /// This method is left here for legacy purpose, just not to break any old project that depends on this method
        /// </summary>
        /// <param name="ob"></param>
        /// <param name="escapeStringSequence"></param>
        /// <param name="wrapStringWithSingleQuote"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ConvertToSqlFormat(object ob, bool escapeStringSequence, bool wrapStringWithSingleQuote, MySqlColumn col)
        {
            StringBuilder sb = new StringBuilder();

            if (ob == null || ob is DBNull)
            {
                return "NULL";
            }
            else if (ob is string)
            {
                string str = (string)ob;

                if (escapeStringSequence)
                    EscapeStringSequence(sb, str);

                return wrapStringWithSingleQuote ? $"'{str}'" : str;
            }
            else if (ob is bool)
            {
                return Convert.ToInt32(ob).ToString();
            }
            else if (ob is byte[])
            {
                return ConvertByteArrayToHexString((byte[])ob);
            }
            else if (ob is short)
            {
                return ((short)ob).ToString(_numberFormatInfo);
            }
            else if (ob is int)
            {
                return ((int)ob).ToString(_numberFormatInfo);
            }
            else if (ob is long)
            {
                return ((long)ob).ToString(_numberFormatInfo);
            }
            else if (ob is ushort)
            {
                return ((ushort)ob).ToString(_numberFormatInfo);
            }
            else if (ob is uint)
            {
                return ((uint)ob).ToString(_numberFormatInfo);
            }
            else if (ob is ulong)
            {
                return ((ulong)ob).ToString(_numberFormatInfo);
            }
            else if (ob is double)
            {
                return ((double)ob).ToString(_numberFormatInfo);
            }
            else if (ob is decimal)
            {
                return ((decimal)ob).ToString(_numberFormatInfo);
            }
            else if (ob is float)
            {
                return ((float)ob).ToString(_numberFormatInfo);
            }
            else if (ob is byte)
            {
                return ((byte)ob).ToString(_numberFormatInfo);
            }
            else if (ob is sbyte)
            {
                return ((sbyte)ob).ToString(_numberFormatInfo);
            }
            else if (ob is TimeSpan ts)
            {
                string time = $"{((int)ts.TotalHours):D2}:{ts.Duration().Minutes:D2}:{ts.Duration().Seconds:D2}";
                return wrapStringWithSingleQuote ? $"'{time}'" : time;
            }
            else if (ob is DateTime dt)
            {
                string dateTime = dt.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo);
                if (col?.TimeFractionLength > 0)
                {
                    dateTime += "." + dt.ToString("".PadLeft(col.TimeFractionLength, 'f'));
                }
                return wrapStringWithSingleQuote ? $"'{dateTime}'" : dateTime;
            }
            else if (ob is MySqlDateTime mdt)
            {
                if (mdt.IsValidDateTime)
                {
                    DateTime dtime = mdt.GetDateTime();
                    string dateTime;

                    if (col != null)
                    {
                        if (col.MySqlDataType == "datetime")
                            dateTime = dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo);
                        else if (col.MySqlDataType == "date")
                            dateTime = dtime.ToString("yyyy-MM-dd", _dateFormatInfo);
                        else if (col.MySqlDataType == "time")
                            dateTime = $"{mdt.Hour}:{mdt.Minute}:{mdt.Second}";
                        else
                            dateTime = dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo);

                        if (col.TimeFractionLength > 0)
                        {
                            dateTime += "." + mdt.Microsecond.ToString().PadLeft(col.TimeFractionLength, '0');
                        }
                    }
                    else
                    {
                        dateTime = dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo);
                    }

                    return wrapStringWithSingleQuote ? $"'{dateTime}'" : dateTime;
                }
                else
                {
                    string dateTime;

                    if (col != null)
                    {
                        if (col.MySqlDataType == "datetime")
                            dateTime = "0000-00-00 00:00:00";
                        else if (col.MySqlDataType == "date")
                            dateTime = "0000-00-00";
                        else if (col.MySqlDataType == "time")
                            dateTime = "00:00:00";
                        else
                            dateTime = "0000-00-00 00:00:00";

                        if (col.TimeFractionLength > 0)
                        {
                            dateTime += "." + "".PadRight(col.TimeFractionLength, '0');
                        }
                    }
                    else
                    {
                        dateTime = "0000-00-00 00:00:00";
                    }

                    return wrapStringWithSingleQuote ? $"'{dateTime}'" : dateTime;
                }
            }
            else if (ob is Guid guid)
            {
                if (col != null && col.MySqlDataType == "binary(16)")
                {
                    return ConvertByteArrayToHexString(guid.ToByteArray());
                }
                else
                {
                    string guidStr = guid.ToString();
                    return wrapStringWithSingleQuote ? $"'{guidStr}'" : guidStr;
                }
            }
            else
            {
                throw new Exception($"Unhandled data type. Current processing data type: {ob.GetType()}. Please report this bug with this message to the development team.");
            }
        }

        // Helper method to convert byte array to hex string
        private static string ConvertByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            hex.Append("0x");
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public static void ConvertToSqlFormat(StringBuilder sb, Object ob, bool escapeStringSequence, bool wrapStringWithSingleQuote)
        {
            ConvertToSqlFormat(sb, ob, null, escapeStringSequence, wrapStringWithSingleQuote);
        }

        public static void ConvertToSqlFormat(StringBuilder sb, Object ob, MySqlColumn col, bool escapeStringSequence, bool wrapStringWithSingleQuote)
        {
            if (ob == null || ob is System.DBNull)
            {
                sb.AppendFormat("NULL");
            }
            else if (ob is System.String)
            {
                string str = (string)ob;

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                //sb.Append(str);
                if (escapeStringSequence)
                    EscapeStringSequence(sb, str);

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");
            }
            else if (ob is System.Boolean)
            {
                sb.AppendFormat(Convert.ToInt32(ob).ToString());
            }
            else if (ob is System.Byte[])
            {
                ConvertByteArrayToHexString(sb, ob);
            }
            else if (ob is short)
            {
                sb.AppendFormat(((short)ob).ToString(_numberFormatInfo));
            }
            else if (ob is int)
            {
                sb.AppendFormat(((int)ob).ToString(_numberFormatInfo));
            }
            else if (ob is long)
            {
                sb.AppendFormat(((long)ob).ToString(_numberFormatInfo));
            }
            else if (ob is ushort)
            {
                sb.AppendFormat(((ushort)ob).ToString(_numberFormatInfo));
            }
            else if (ob is uint)
            {
                sb.AppendFormat(((uint)ob).ToString(_numberFormatInfo));
            }
            else if (ob is ulong)
            {
                sb.AppendFormat(((ulong)ob).ToString(_numberFormatInfo));
            }
            else if (ob is double)
            {
                sb.AppendFormat(((double)ob).ToString(_numberFormatInfo));
            }
            else if (ob is decimal)
            {
                sb.AppendFormat(((decimal)ob).ToString(_numberFormatInfo));
            }
            else if (ob is float)
            {
                sb.AppendFormat(((float)ob).ToString(_numberFormatInfo));
            }
            else if (ob is byte)
            {
                sb.AppendFormat(((byte)ob).ToString(_numberFormatInfo));
            }
            else if (ob is sbyte)
            {
                sb.AppendFormat(((sbyte)ob).ToString(_numberFormatInfo));
            }
            else if (ob is TimeSpan)
            {
                TimeSpan ts = (TimeSpan)ob;

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                sb.AppendFormat(((int)ts.TotalHours).ToString().PadLeft(2, '0'));
                sb.AppendFormat(":");
                sb.AppendFormat(ts.Duration().Minutes.ToString().PadLeft(2, '0'));
                sb.AppendFormat(":");
                sb.AppendFormat(ts.Duration().Seconds.ToString().PadLeft(2, '0'));

                if (col != null && col.TimeFractionLength > 0)
                {
                    sb.Append(".");
                    long totalMicroseconds = ts.Ticks / 10;
                    long microsecondPart = totalMicroseconds % 1000000;
                    sb.Append(microsecondPart.ToString().PadLeft(col.TimeFractionLength, '0'));
                }

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

            }
            else if (ob is System.DateTime)
            {
                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                sb.AppendFormat(((DateTime)ob).ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo));

                if (col != null && col.TimeFractionLength > 0)
                {
                    sb.Append(".");
                    string _microsecond = ((DateTime)ob).ToString("".PadLeft(col.TimeFractionLength, 'f'));
                    sb.Append(_microsecond);
                }

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");
            }
            else if (ob is MySqlDateTime mdt)
            {
                if (mdt.IsValidDateTime)
                {
                    DateTime dtime = mdt.GetDateTime();

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");

                    if (col != null)
                    {
                        if (col.MySqlDataType == "datetime")
                            sb.AppendFormat(dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo));
                        else if (col.MySqlDataType == "date")
                            sb.AppendFormat(dtime.ToString("yyyy-MM-dd", _dateFormatInfo));
                        else if (col.MySqlDataType == "time")
                            sb.AppendFormat("{0}:{1}:{2}", mdt.Hour, mdt.Minute, mdt.Second);
                        else
                            sb.AppendFormat(dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo));

                        if (col.TimeFractionLength > 0)
                        {
                            sb.Append(".");
                            sb.Append(((MySqlDateTime)ob).Microsecond.ToString().PadLeft(col.TimeFractionLength, '0'));
                        }
                    }
                    else
                    {
                        sb.AppendFormat(dtime.ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo));
                    }

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
                else
                {
                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");

                    if (col != null)
                    {
                        if (col.MySqlDataType == "datetime")
                            sb.AppendFormat("0000-00-00 00:00:00");
                        else if (col.MySqlDataType == "date")
                            sb.AppendFormat("0000-00-00");
                        else if (col.MySqlDataType == "time")
                            sb.AppendFormat("00:00:00");
                        else
                            sb.AppendFormat("0000-00-00 00:00:00");

                        if (col.TimeFractionLength > 0)
                        {
                            sb.Append(".".PadRight(col.TimeFractionLength, '0'));
                        }
                    }
                    else
                    {
                        sb.AppendFormat("0000-00-00 00:00:00");
                    }

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
            }
            else if (ob is System.Guid)
            {
                if (col != null && col.MySqlDataType == "binary(16)")
                {
                    ConvertByteArrayToHexString(sb, ob);
                }
                else if (col != null && col.MySqlDataType == "char(36)")
                {
                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");

                    sb.Append(ob);

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
                else
                {
                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");

                    sb.Append(ob);

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
            }
            else
            {
                throw new Exception("Unhandled data type. Current processing data type: " + ob.GetType().ToString() + ". Please report this bug with this message to the development team.");
            }
        }

        public static void ConvertByteArrayToHexString(StringBuilder sb, object ob)
        {
            if (ob == null || ob == DBNull.Value)
            {
                sb.Append("NULL");
            }
            else
            {
                byte[] ba = (byte[])ob;
                if (ba.Length == 0)
                {
                    sb.Append("''");
                }
                else
                {
                    char[] c = new char[ba.Length * 2 + 2];
                    byte b;
                    c[0] = '0';
                    c[1] = 'x';
                    for (int y = 0, x = 2; y < ba.Length; ++y, ++x)
                    {
                        b = ((byte)(ba[y] >> 4));
                        c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                        b = ((byte)(ba[y] & 0xF));
                        c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                    }
                    sb.Append(new string(c));
                }
            }
        }

        public static string EscapeIdentifier(string identifierName)
        {
            return identifierName.Replace("`", "``");
        }

        public static long EstimateByteCount(string str, System.Text.Encoding textEncoding)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            // Quick check - if all ASCII, return length
            bool isAscii = true;
            foreach (char c in str)
            {
                if (c > 127)
                {
                    isAscii = false;
                    break;
                }
            }

            if (isAscii)
                return str.Length;

            // For non-ASCII, use actual calculation
            return textEncoding.GetByteCount(str);
        }
    }
}