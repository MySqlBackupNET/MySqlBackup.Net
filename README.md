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
  https://www.nuget.org/packages/MySqlBackup.NET.MySqlConnector/

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

## Compatibility

We aim for MySqlBackup.NET to achieve 100% SQL compliance, ensuring seamless compatibility with `mysqldump` and `mysql.exe` for both backup and restore operations. Version 2.6 introduces key improvements, addressing some flaws in previous version and compatibility challenges with `mysqldump`-generated files. If you encounter any incompatibilities, we welcome feedback via [GitHub Issues](https://github.com/MySqlBackupNET/MySqlBackup.Net/issues) to help us refine and uphold this goal.

---

## Configuration Tips

### Unicode Support
* Always use the default character set of `utf8mb4`, or `utf8` in older MySQL versions that do not support `utf8mb4`.
* It is recommended to use `convertzerodatetime=true` in the connection string for compatibility when handling null datetime values.

---

## Performance Benchmark

For a 416 MB database (400,000 rows, 4 tables, InnoDB) on an Intel Core i7-4770S (3.10GHz, 16GB RAM, SSD Samsung 870 Evo 500GB):

![graph benchmark mysqlbackup.net 2025-07-04](https://raw.githubusercontent.com/MySqlBackupNET/MySqlBackup.Net/refs/heads/master/wiki/graph-benchmark-2025-07-04.png)

| Task         | Tool              | Avg. Time | File Size  |
|--------------|-------------------|-----------|------------|
| **Backup**   | MySqlBackup.NET (Parallel) | ~10.21s | 571.588 MB |
| **Backup**   | MySqlBackup.NET (Single)   | ~15.72s | 571.588 MB |
| **Backup**   | mysqldump.exe     | ~6.76s  | 566.976 MB |
| **Restore**  | MySqlBackup.NET   | ~35.87s | -          |
| **Restore**  | mysql.exe         | ~32.76s | -          |

MySqlBackup.NET v2.6 offers competitive performance, especially in parallel mode, with significant improvements over previous versions. Full details: [Performance Benchmark Wiki](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/Performance-Benchmark-(MySqlBackup.NET,-MySqlDump,-MySQL.EXE)).

---

## License

MySqlBackup.NET is released under [The Unlicense](https://github.com/MySqlBackupNET/MySqlBackup.Net/blob/master/LICENSE), making it free for any use.

---

## Conclusion

MySqlBackup.NET empowers developers and end-users alike with a robust, .NET-native solution for MySQL database management. Whether for desktop apps, web services, or automated backups, it’s a versatile addition to your toolkit, built by a global community for a global community.

Explore more on [GitHub](https://github.com/MySqlBackupNET/MySqlBackup.Net)!
