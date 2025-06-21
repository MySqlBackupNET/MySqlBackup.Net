<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="CreateSampleTableRows.aspx.cs" Inherits="System.pages.CreateSampleTableRows" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .maintb {
            border-collapse: collapse;
        }

            .maintb th {
                padding: 6px;
                border: 1px solid #a3a3a3;
                background: #7b6f8a;
                color: white;
                font-weight: normal;
            }

            .maintb td {
                padding: 6px;
                border: 1px solid #a3a3a3;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
        <h1>Create Sample Table Rows</h1>

        <asp:Button ID="btGenerateData" runat="server" Text="Generate Sample Data" OnClick="btGenerateData_Click" OnClientClick="showBigLoading(0);" />

        Total Rows:
        <asp:TextBox ID="txtTotalRows" runat="server" TextMode="Number" Width="150px" Text="10000"></asp:TextBox>

        <br />
        <br />

        <table class="maintb">
            <tr>
                <th>Database Size</th>
                <th>Total Rows</th>
            </tr>
            <tr>
                <td>1MB</td>
                <td>1440</td>
            </tr>
            <tr>
                <td>5MB</td>
                <td>7201</td>
            </tr>
            <tr>
                <td>10MB</td>
                <td>14403</td>
            </tr>
            <tr>
                <td>100MB</td>
                <td>14403</td>
            </tr>
            <tr>
                <td>300MB</td>
                <td>432105</td>
            </tr>
            <tr>
                <td>500MB</td>
                <td>720175</td>
            </tr>
            <tr>
                <td>1GB</td>
                <td>1474920</td>
            </tr>
            <tr>
                <td>10GB</td>
                <td>14749200</td>
            </tr>
        </table>

        <br />

        <pre class="light-formatted">CREATE TABLE sample1 (
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
    col_real REAL,
    col_double_precision DOUBLE PRECISION,
    col_integer INTEGER,
    col_dec DEC(10,2),
    col_fixed FIXED(8,3),
    col_serial SERIAL,
    
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
);

for (int i = 1; i &lt;= totalRows; i++)
{
	_currentRows = i;
	cmd.CommandText = $@&quot;
INSERT INTO sample1 (
    col_tinyint, col_smallint, col_mediumint, col_int, col_bigint,
    col_decimal, col_numeric, col_float, col_double, col_bit, col_bool,
    col_real, col_double_precision, col_integer, col_dec, col_fixed,
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
    {i * 1.23}, {i * 12.3456}, {i * 10000}, {i * 123.45}, {i * 12.345},
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
);&quot;;
	cmd.ExecuteNonQuery();
}
        </pre>
    </div>

</asp:Content>
