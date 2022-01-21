# MySqlBackup.Net

A tool to backup and restore MySQL database in C#/VB.NET/ASP.NET.

Runs on MySql.Data.DLL, MySqlConnector.DLL and Devart.Express.MySql.DLL

## How to Add This Library into Your Project
Read [this wiki: How to Add This Library into Your Project](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/How-to-Add-This-Library-into-Your-Project)

## Backup ALL Databases in 1 Click
Export all the databases one by one to separate SQL dump files. This a sub-project expansion using MySqlBackup.NET. Read more: [MySqlBackup_All_DB](https://github.com/MySqlBackupNET/MySqlBackup_All_DB)

## Download
https://github.com/MySqlBackupNET/MySqlBackup.Net/releases

Install via NuGet:

**PM> Install-Package MySqlBackup.NET**  
[https://www.nuget.org/packages/MySqlBackup.NET/](https://www.nuget.org/packages/MySqlBackup.NET/)

**PM> Install-Package MySqlBackup.Net.DevartExpress**<br />
[https://www.nuget.org/packages/MySqlBackup.Net.DevartExpress/](https://www.nuget.org/packages/MySqlBackup.Net.DevartExpress/)

**PM> Install-Package MySqlBackup.NET.MySqlConnector**<br />
[https://www.nuget.org/packages/MySqlBackup.NET.MysqlConnector/](https://www.nuget.org/packages/MySqlBackup.NET.MysqlConnector/)

## Backup/Export a MySQL Database
```C#
string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
string file = "C:\\backup.sql";
using (MySqlConnection conn = new MySqlConnection(constring))
{
    using (MySqlCommand cmd = new MySqlCommand())
    {
        using (MySqlBackup mb = new MySqlBackup(cmd))
        {
            cmd.Connection = conn;
            conn.Open();
            mb.ExportToFile(file);
            conn.Close();
        }
    }
}
```

## Import/Restore a MySQL Database

```C#
string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
string file = "C:\\backup.sql";
using (MySqlConnection conn = new MySqlConnection(constring))
{
    using (MySqlCommand cmd = new MySqlCommand())
    {
        using (MySqlBackup mb = new MySqlBackup(cmd))
        {
            cmd.Connection = conn;
            conn.Open();
            mb.ImportFromFile(file);
            conn.Close();
        }
    }
}
```

## Introduction

MySqlBackup.NET is a tool (DLL) that can backup/restore MySQL database in .NET Programming Language. It is an alternative to MySqlDump.

This tool is developed in C# but able to be used in any .NET Language (i.e. VB.NET, F#, etc.).

Another benefit of making this tool is, we don't have to rely on two small programs - MySqlDump.exe and MySql.exe to perform the backup and restore task. We will have better control on the output result in .NET way.

The most common way to backup a MySQL Database is by using MySqlDump and MySQL Workbench.

MySQL Workbench is good for developers, but when comes to the client or end-user, the recommended way is to get every parameter preset and all they need to know is press the big button "Backup" and everything is done. Using MySQL Workbench as a backup tool is not a suitable solution for the client or end-user.

On the other hand, MySqlDump.exe cannot be executed directly from the Web Server. As some providers forbid that, MySqlBackup will be helpful in building a web-based (ASP.NET) backup tool.

## Features

* Backup and Restore of MySQL Database
* Can be used in any .NET Languages.
* Export/Import to/from MemoryStream
* Conditional Rows Export (Filter Tables or Rows)
* Progress Report is Available for Both Export and Import Task.
* Able to export rows into different modes. (Insert, Insert Ignore, Replace, On Duplicate Key Update, Update)
* Can be used directly in ASP.NET or web services.

## Prerequisite and Dependencies for Development, Compile and Production Usage

MySqlBackup.NET relies on the following component to work.

<b>Option 1: MySql.Data (Connector/NET)</b>
* [MySQL dot net Connector/Net (MySql.Data.DLL)](http://www.mysql.com/downloads/connector/net/)
* MySql.Data.DLL is developed by Oracle Corporation, licensed under GPL License (http://www.gnu.org/licenses/old-licenses/gpl-2.0.html).

<b>Option 2: Devart Express (dotConnect)</b>
* [Devart dotConnect for MySQL Express](https://www.devart.com/dotconnect/mysql/)
* For license agreement, please read: [https://www.devart.com/dotconnect/mysql/licensing-faq.html](https://www.devart.com/dotconnect/mysql/licensing-faq.html)
* Devart.Data.DLL
* Devart.Data.MySql.DLL

<b>Option 3: MySqlConnector (MIT)</b>
* [MySqlConnector: High Performance MySQL Library for .NET](https://mysqlconnector.net/)
* Project URL: https://github.com/mysql-net/MySqlConnector
* Licensed under MIT
* MySqlConnector.DLL

## Reminder

### Reminder 1

MySqlBackup.NET (or MySqlBackup.DLL) stands on top of MySql.Data.DLL which also stands on top of .NET Framework, which uses UTF8 encoding by default.
If your database involves any UTF8 or Unicode Characters. You must use a MySQL database with default character of **UTF8** while handling Unicode Characters, such as

* Western European specific languages, the character of 'À', 'ë', 'õ', 'Ñ'.
* Russian, Hebrew, India, Arabic, Chinese, Korean, Japanese characters, etc.

You are recommended to apply the connection string option of charset=utf8. Example:

```
server=localhost;user=root;pwd=mypwd;charset=utf8;
```
or
```
server=localhost;user=root;pwd=mypwd;charset=utf8mb4;
```
### Reminder 2

(For MySql.Data connector only)
DateTime conversion between MySQL and .NET Framework. In MySQL, there are various of DateTime format, such as null value or Date only data. But, in .NET Framework, there is no null value (or Date only) for DateTime. This error is not caused by MySqlBackup.DLL. MySql.Data.DLL (developed by Oracle) has decided to throw an exception of Data Conversion Error. Therefore, you are strongly recommended to apply the connection string option of **convertzerodatetime=true**. Example:

```
server=localhost;user=root;pwd=mypwd;charset=utf8;convertzerodatetime=true;
```

## Performance Benchmark (MySqlDump vs MySqlBackup.NET)
[Full Report - Wiki Page: Performance Benchmark (MySqlDump vs MySqlBackup.NET)](https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki/Performance-Benchmark-(MySqlDump-vs-MySqlBackup.NET)))

Summary Benchmark Report:
```
Database Size: 3.5GB (15 million rows)

Process     Tools              Time
-------     ---------          ------
Backup 1    MySqlDump         2m 36s
Backup 2    MySqlDump         2m 33s
Backup 3    MySqlDump         2m 35s

File size: 
4.66 GB (5,008,487,275 bytes)

Backup 4    MySqlBackup.NET   7m 48s
Backup 5    MySqlBackup.NET   7m 46s
Backup 6    MySqlBackup.NET   7m 50s

File size:
4.59 GB (4,931,743,894 bytes)

Restore 1   MySql.exe         8m 42s
Restore 2   MySql.exe         8m 23s
Restore 3   MySql.exe         8m 57s

Restore 4   MySqlBackup.NET   9m 44s
Restore 5   MySqlBackup.NET   9m 39s
Restore 6   MySqlBackup.NET   9m 39s
```
## License

MySqlBackup.Net is licensed under the [The Unlicense](https://github.com/MySqlBackupNET/MySqlBackup.Net/blob/master/LICENSE).
