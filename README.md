# MySqlBackup.NET

*A versatile tool for backing up and restoring MySQL databases in C#, VB.NET, and ASP.NET.*

*Latest Release: v2.6 (July 04, 2025)*  
[Change Log](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/Change-Log)

---

## Overview

MySqlBackup.NET is a .NET library (DLL) designed to backup and restore MySQL databases. Compatible with multiple MySQL connectors—`MySql.Data.DLL`, `MySqlConnector.DLL`, and `Devart.Express.MySql.DLL`—it offers a programmatic alternative to tools like MySqlDump, providing greater control and flexibility in .NET environments.

Developed in C#, this library supports any .NET language (e.g., VB.NET, F#) and excels in scenarios where MySqlDump.exe or MySQL Workbench are impractical, such as web-based applications (ASP.NET) or end-user tools with simplified interfaces.

---

## Key Features

- Backup and restore MySQL databases programmatically.
- Supports all .NET languages.
- Export/import via files or `MemoryStream`.
- Conditional row exports (filter tables/rows).
- Progress reporting for export and import tasks.
- Flexible row export modes: `INSERT`, `INSERT IGNORE`, `REPLACE`, `ON DUPLICATE KEY UPDATE`, `UPDATE`.
- Ideal for ASP.NET and web service integration.

---

## Getting Started

### Installation

#### Download
Grab the latest release from: [GitHub Releases](https://github.com/MySqlBackupNET/MySqlBackup.Net/releases)

#### NuGet Packages
Install via NuGet Package Manager:

- **MySqlConnector**:  
  `PM> Install-Package MySqlBackup.NET.MySqlConnector`  
  https://www.nuget.org/packages/MySqlBackup.NET.MysqlConnector/
  
- **MySql.Data Connector**:  
  `PM> Install-Package MySqlBackup.NET`  
  https://www.nuget.org/packages/MySqlBackup.NET/

- **Devart Express Connector**:  
  `PM> Install-Package MySqlBackup.Net.DevartExpress`  
  https://www.nuget.org/packages/MySqlBackup.Net.DevartExpress/

#### Add to Your Project
See the detailed guide:  
[How to Add This Library into Your Project](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/How-to-Add-This-Library-into-Your-Project)

---

### Basic Usage

#### Backup a Database
```csharp
string constr = "server=localhost;user=root;pwd=1234;database=test1;convertzerodatetime=true;";
string filePath = @"C:\backup.sql";

using (var conn = new MySqlConnection(constr))
using (var cmd = conn.CreateCommand())
using (var mb = new MySqlBackup(cmd))
{
    conn.Open();
    mb.ExportToFile(filePath);
}
```

#### Restore a Database
```csharp
string constr = "server=localhost;user=root;pwd=1234;database=test1;convertzerodatetime=true;";
string filePath = @"C:\backup.sql";

using (var conn = new MySqlConnection(constr))
using (var cmd = conn.CreateCommand())
using (var mb = new MySqlBackup(cmd))
{
    conn.Open();
    mb.ImportFromFile(filePath);
}
```

---

## Why MySqlBackup.NET?

Unlike MySQL Workbench (developer-focused) or MySqlDump.exe (restricted in web environments), MySqlBackup.NET offers:

- **End-User Simplicity**: Preset parameters for a one-click backup experience.
- **Web Compatibility**: Runs seamlessly in ASP.NET, bypassing hosting restrictions on executables.
- **Programmatic Control**: Fine-tuned output handling within .NET.

---

## Dependencies

MySqlBackup.NET requires one of these MySQL connectors:

| Connector             | Source                                      | License       | DLLs                     |
|-----------------------|---------------------------------------------|---------------|--------------------------|
| **MySqlConnector**    | [MySqlConnector](https://mysqlconnector.net/) | MIT           | `MySqlConnector.dll`     |
| **MySql.Data**        | [MySQL Connector/Net](http://www.mysql.com/downloads/connector/net/) | GPL           | `MySql.Data.dll`         |
| **Devart Express**    | [dotConnect for MySQL](https://www.devart.com/dotconnect/mysql/)     | Custom ([FAQ](https://www.devart.com/dotconnect/mysql/licensing-faq.html)) | `Devart.Data.dll`, `Devart.Data.MySql.dll` |


---

## Configuration Tips

### Unicode Support
* Always use the default character set of `utf8mb4`, or `utf8` in older MySQL versions that do not support `utf8mb4`.
* It is recommended to use `convertzerodatetime=true` in the connection string for compatibility when handling null datetime values.

---

## Performance Benchmark

Compare MySqlBackup.NET to MySqlDump for a 3.5GB database (15M rows):

| Task         | Tool              | Time    | File Size      |
|--------------|-------------------|---------|----------------|
| **Backup**   | MySqlDump         | ~2m 35s | 4.66 GB        |
| **Backup**   | MySqlBackup.NET   | ~7m 48s | 4.59 GB        |
| **Restore**  | MySql.exe         | ~8m 41s | -              |
| **Restore**  | MySqlBackup.NET   | ~9m 41s | -              |

Full details: [Performance Benchmark Wiki](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/Performance-Benchmark-(MySqlDump-vs-MySqlBackup.NET))

---

## Additional Tools

**Backup All Databases**  
Export all databases to separate SQL files with this sub-project:  
[MySqlBackup_All_DB](https://github.com/MySqlBackupNET/MySqlBackup_All_DB)

---

## License

MySqlBackup.NET is released under [The Unlicense](https://github.com/MySqlBackupNET/MySqlBackup.Net/blob/master/LICENSE), making it free for any use.

---

## Conclusion

MySqlBackup.NET empowers developers and end-users alike with a robust, .NET-native solution for MySQL database management. Whether for desktop apps, web services, or automated backups, it’s a versatile addition to your toolkit.

Explore more on [GitHub](https://github.com/MySqlBackupNET/MySqlBackup.Net)!
