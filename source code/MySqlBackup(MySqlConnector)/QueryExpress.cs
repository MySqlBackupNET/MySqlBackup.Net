﻿using System;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MySqlConnector
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

        public static string EscapeStringSequence(string data)
        {
            var builder = new StringBuilder();

            foreach (char c in data)
            {
                escape_string(builder, c);
            }

            return builder.ToString();
        }

        static void escape_string(StringBuilder sb, char c)
        {
            switch (c)
            {
                case '\\': // Backslash
                    sb.Append("\\\\");
                    break;
                case '\0': // Null
                    sb.Append("\\0");
                    break;
                case '\r': // Carriage return
                    sb.Append("\\r");
                    break;
                case '\n': // New Line
                    sb.Append("\\n");
                    break;
                case '\a': // Vertical tab
                    sb.Append("\\a");
                    break;
                case '\b': // Backspace
                    sb.Append("\\b");
                    break;
                case '\f': // Formfeed
                    sb.Append("\\f");
                    break;
                case '\t': // Horizontal tab
                    sb.Append("\\t");
                    break;
                case '\v': // Vertical tab
                    sb.Append("\\v");
                    break;
                case '\"': // Double quotation mark
                    sb.Append("\\\"");
                    break;
                case '\'': // Single quotation mark
                    sb.Append("''");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        public static string EraseDefiner(string input)
        {
            StringBuilder sb = new StringBuilder();
            string definer = " DEFINER=";
            int dIndex = input.IndexOf(definer);

            sb.AppendFormat(definer);

            bool pointAliasReached = false;
            bool point3rdQuoteReached = false;

            for (int i = dIndex + definer.Length; i < input.Length; i++)
            {
                if (!pointAliasReached)
                {
                    if (input[i] == '@')
                        pointAliasReached = true;

                    sb.Append(input[i]);
                    continue;
                }

                if (!point3rdQuoteReached)
                {
                    if (input[i] == '`')
                        point3rdQuoteReached = true;

                    sb.Append(input[i]);
                    continue;
                }

                if (input[i] != '`')
                {
                    sb.Append(input[i]);
                    continue;
                }
                else
                {
                    sb.Append(input[i]);
                    break;
                }
            }

            return input.Replace(sb.ToString(), string.Empty);
        }

        public static void ConvertToSqlValueFormat(StringBuilder sb, Object ob, bool escapeStringSequence, bool wrapStringWithSingleQuote)
        {
            ConvertToSqlValueFormat(sb, ob, null, escapeStringSequence, wrapStringWithSingleQuote);
        }

        public static void ConvertToSqlValueFormat(StringBuilder sb, Object ob, MySqlColumn col, bool escapeStringSequence, bool wrapStringWithSingleQuote)
        {
            if (ob == null || ob is System.DBNull)
            {
                sb.AppendFormat("NULL");
            }
            else if (ob is System.String)
            {
                string str = (string)ob;

                if (escapeStringSequence)
                    str = EscapeStringSequence(str);

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                sb.Append(str);

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

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

            }
            else if (ob is System.DateTime)
            {
                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                sb.AppendFormat(((DateTime)ob).ToString("yyyy-MM-dd HH:mm:ss", _dateFormatInfo));

                if (col.TimeFractionLength > 0)
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

    }
}