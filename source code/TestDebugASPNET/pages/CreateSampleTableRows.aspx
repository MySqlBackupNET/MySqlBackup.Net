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

    <div class="div-center-framed-content">

        <h1>Create Sample Table Rows</h1>

        <asp:Button ID="btGenerateData" runat="server" Text="Generate Sample Data" OnClick="btGenerateData_Click" OnClientClick="showBigLoading(0);" />

        Total Tables:
        <asp:TextBox ID="txtTotalTables" runat="server" TextMode="Number" Width="60px" Text="1"></asp:TextBox> 
        Total Rows:
        <asp:TextBox ID="txtTotalRows" runat="server" TextMode="Number" Width="100px" Text="10000"></asp:TextBox>

        <asp:CheckBox ID="cbDropRecreateTable" runat="server" Checked="true" />
        Drop and Re-Create the sample table:
        

        <br />
        <br />

        <table class="maintb">
            <tr>
                <th>Total Rows</th>
                <th>Size</th>
                <th>Size (Bytes)</th>
            </tr>
            <tr>
                <td>10000</td>
                <td>17MB</td>
                <td>17825792</td>
            </tr>
            <tr>
                <td>50000</td>
                <td>56MB</td>
                <td>58720256</td>
            </tr>
            <tr>
                <td>70000</td>
                <td>72MB</td>
                <td>75497472</td>
            </tr>
            <tr>
                <td>100000</td>
                <td>104MB</td>
                <td>109051904</td>
            </tr>
            <tr>
                <td>500000</td>
                <td>476MB</td>
                <td>499122176</td>
            </tr>
            <tr>
                <td>1000000</td>
                <td>940MB</td>
                <td>985661440</td>
            </tr>
            <tr>
                <td>5000000</td>
                <td>4.55GB</td>
                <td>4886364160</td>
            </tr>
        </table>

        <br />

        <pre class="light-formatted">CREATE TABLE IF NOT EXISTS sample1 (
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
    col_serial BIGINT UNSIGNED NOT NULL,
    col_date DATE,
    col_time TIME,
    col_datetime DATETIME,
    col_timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    col_year YEAR,
    col_char CHAR(50),
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

INSERT INTO sample1 (
    col_tinyint, col_smallint, col_mediumint, col_int, col_bigint,
    col_decimal, col_numeric, col_float, col_double, col_bit,
    col_bool, col_real, col_double_precision, col_integer, col_dec,
    col_fixed, col_serial, col_date, col_time, col_datetime,
    col_timestamp, col_year, col_char, col_varchar, col_binary,
    col_varbinary, col_tinytext, col_text, col_mediumtext, col_longtext,
    col_tinyblob, col_blob, col_mediumblob, col_longblob, col_json,
    col_geometry, col_point, col_linestring, col_polygon, col_multipoint,
    col_multilinestring, col_multipolygon, col_geometrycollection, col_enum, col_set
) VALUES

for (int i = 1; i <= totalRows; i++)
{
   
    _currentRows = i;
    values.Add($@"
    (
    {i % 127}, -- col_tinyint (0 to 255)
    {i % 32768}, -- col_smallint (-32768 to 32767)
    {i % 8388608}, -- col_mediumint (-8388608 to 8388607)
    {i % 2147483648}, -- col_int (-2147483648 to 2147483647)
    {i % 1000000}, -- col_bigint (use modulo to avoid overflow)
    {(i % 1000) + 0.01 * (i % 100)}, -- col_decimal (e.g., 0.00 to 999.99)
    {(i % 100) + 0.001 * (i % 1000)}, -- col_numeric (e.g., 0.000 to 99.999)
    {(float)(i % 1000) / 10}, -- col_float (e.g., 0.0 to 99.9)
    {(double)(i % 10000) / 100}, -- col_double (e.g., 0.00 to 99.99)
    b'{Convert.ToString(i % 256, 2).PadLeft(8, '0')}', -- col_bit (8 bits, 0 to 255)
    {i % 2}, -- col_bool (0 or 1)
    {(float)(i % 1000) / 10}, -- col_real (e.g., 0.0 to 99.9)
    {(double)(i % 10000) / 100}, -- col_double_precision (e.g., 0.00 to 99.99)
    {i % 2147483648}, -- col_integer (-2147483648 to 2147483647)
    {(i % 1000) + 0.01 * (i % 100)}, -- col_dec (e.g., 0.00 to 999.99)
    {(i % 100) + 0.001 * (i % 1000)}, -- col_fixed (e.g., 0.000 to 99.999)
    {i}, -- col_serial (auto-incrementing, safe as BIGINT UNSIGNED)
    '2024-01-01' + INTERVAL {(i % 365)} DAY, -- col_date (cycle through 1 year)
    TIME(CONCAT('{(i % 24):D2}:', '{(i % 60):D2}:', '{(i % 60):D2}')), -- col_time (cycle through hours/minutes/seconds)
    '2024-01-01 12:00:00' + INTERVAL {(i % 365)} DAY, -- col_datetime (cycle through 1 year)
    '2024-01-01 12:00:00' + INTERVAL {(i % 365)} DAY, -- col_timestamp (cycle through 1 year)
    {2000 + (i % 100)}, -- col_year (cycle through 2000 to 2099)
    'CHAR{i % 50}', -- col_char (cycle through small strings)
    'VARCHAR row {i % 255}', -- col_varchar
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_binary (16 bytes)
    UNHEX('{(i % 256):X2}{(i % 256):X2}'), -- col_varbinary
    'Tiny text {i % 100}', -- col_tinytext
    'Text content for row {i % 100}', -- col_text
    'Medium text for row {i % 100}', -- col_mediumtext
    'Long text content for row {i % 100}', -- col_longtext
    UNHEX('{(i % 256):X2}{(i % 256):X2}'), -- col_tinyblob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_blob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_mediumblob
    UNHEX('{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}{(i % 256):X2}'), -- col_longblob
    JSON_OBJECT('id', {i % 1000}, 'name', CONCAT('Item ', {i % 1000}), 'active', {(i % 2 == 0 ? "true" : "false")}), -- col_json
    ST_GeomFromText('POINT({i % 100} {i % 100})'), -- col_geometry
    ST_GeomFromText('POINT({i % 100} {(i % 100) * 2})'), -- col_point
    ST_GeomFromText('LINESTRING(0 0, {i % 100} {i % 100})'), -- col_linestring
    ST_GeomFromText('POLYGON((0 0, {i % 100} 0, {i % 100} {i % 100}, 0 {i % 100}, 0 0))'), -- col_polygon
    ST_GeomFromText('MULTIPOINT({i % 100} {i % 100}, {(i % 100) * 2} {(i % 100) * 2})'), -- col_multipoint
    ST_GeomFromText('MULTILINESTRING((0 0, {i % 100} {i % 100}, {(i % 100) * 2} {(i % 100) * 2}))'), -- col_multilinestring
    ST_GeomFromText('MULTIPOLYGON(((0 0, {i % 100} 0, {i % 100} {i % 100}, 0 {i % 100}, 0 0)))'), -- col_multipolygon
    ST_GeomFromText('GEOMETRYCOLLECTION(POINT({i % 100} {i % 100}), LINESTRING(0 0, {i % 100} {i % 100}))'), -- col_geometrycollection
    '{(i % 3 == 0 ? "small" : i % 3 == 1 ? "medium" : "large")}', -- col_enum
    '{(i % 4 == 0 ? "red,blue" : i % 4 == 1 ? "green" : i % 4 == 2 ? "blue,yellow" : "red,green,blue")}' -- col_set
    )");
        </pre>
    </div>

</asp:Content>
