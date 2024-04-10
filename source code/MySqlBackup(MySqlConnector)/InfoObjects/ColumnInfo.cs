
namespace MySqlConnector.InfoObjects;
public class ColumnWithValue
{
    public string TableName { get; set; }
    public string ColumnName { get; set; }
    public string MySqlDataType { get; set; }
    public object Value { get; set; }
}
