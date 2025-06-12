using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_WinForm_MySqlConnector
{
    internal class QueryExpress
    {
        public static NumberFormatInfo _numberFormatInfo = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = string.Empty
        };

        public static DateTimeFormatInfo _dateFormatInfo = new DateTimeFormatInfo()
        {
            DateSeparator = "-",
            TimeSeparator = ":"
        };

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

        public static string EscapeStringSequence(string data)
        {
            var builder = new StringBuilder();

            foreach (char c in data)
            {
                escape_string(builder, c);
            }

            return builder.ToString();
        }

        public static void ConvertToSqlValueFormat(StringBuilder sb, Object ob, bool escapeStringSequence, bool wrapStringWithSingleQuote)
        {
            if (ob == null || ob is System.DBNull)
            {
                sb.AppendFormat("NULL");
            }
            else if (ob is System.String)
            {
                string str = (string)ob;

                if (escapeStringSequence)
                    str = QueryExpress.EscapeStringSequence(str);

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
                sb.AppendFormat(((short)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is int)
            {
                sb.AppendFormat(((int)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is long)
            {
                sb.AppendFormat(((long)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is ushort)
            {
                sb.AppendFormat(((ushort)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is uint)
            {
                sb.AppendFormat(((uint)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is ulong)
            {
                sb.AppendFormat(((ulong)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is double)
            {
                sb.AppendFormat(((double)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is decimal)
            {
                sb.AppendFormat(((decimal)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is float)
            {
                sb.AppendFormat(((float)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is byte)
            {
                sb.AppendFormat(((byte)ob).ToString(QueryExpress._numberFormatInfo));
            }
            else if (ob is sbyte)
            {
                sb.AppendFormat(((sbyte)ob).ToString(QueryExpress._numberFormatInfo));
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

                sb.AppendFormat(((DateTime)ob).ToString("yyyy-MM-dd HH:mm:ss", QueryExpress._dateFormatInfo));

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

                    sb.AppendFormat(dtime.ToString("yyyy-MM-dd HH:mm:ss", QueryExpress._dateFormatInfo));

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
                else
                {
                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");

                    sb.AppendFormat("0000-00-00 00:00:00");

                    if (wrapStringWithSingleQuote)
                        sb.AppendFormat("'");
                }
            }
            else if (ob is System.Guid)
            {
                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");

                sb.Append(ob);

                if (wrapStringWithSingleQuote)
                    sb.AppendFormat("'");
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
