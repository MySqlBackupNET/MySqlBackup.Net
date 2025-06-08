using System;
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
    }
}