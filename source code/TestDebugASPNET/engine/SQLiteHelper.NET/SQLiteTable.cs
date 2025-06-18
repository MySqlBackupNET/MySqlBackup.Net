
// https://github.com/adriancs2/SQLiteHelper.net

namespace System.Data.SQLite
{
    public class SQLiteTable
    {
        public string TableName = "";
        public SQLiteColumnList Columns = new SQLiteColumnList();

        public SQLiteTable()
        { }

        public SQLiteTable(string name)
        {
            TableName = name;
        }
    }
}