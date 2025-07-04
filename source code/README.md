# About

A tool to backup and restore MySQL database in C#/VB.NET/ASP.NET.

Runs on MySql.Data.DLL, MySqlConnector.DLL and Devart.Express.MySql.DLL

More documentation is available at the [Project Github website](https://github.com/MySqlBackupNET/MySqlBackup.Net).

**For MySql.Data.DLL**
PM> Install-Package MySqlBackup.NET
[https://www.nuget.org/packages/MySqlBackup.NET/](https://www.nuget.org/packages/MySqlBackup.NET/)

**For dotConnector Devart.Express.MySql**
PM> Install-Package MySqlBackup.Net.DevartExpress
[https://www.nuget.org/packages/MySqlBackup.Net.DevartExpress/](https://www.nuget.org/packages/MySqlBackup.Net.DevartExpress/)

**For MySqlConnector (MIT)**
PM> Install-Package MySqlBackup.NET.MySqlConnector
[https://www.nuget.org/packages/MySqlBackup.NET.MysqlConnector/](https://www.nuget.org/packages/MySqlBackup.NET.MysqlConnector/)

## Backup/Export a MySQL Database
```C#
string constring = "server=localhost;user=root;pwd=qwerty;database=test;convertzerodatetime=true;";

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
string constring = "server=localhost;user=root;pwd=qwerty;database=test;convertzerodatetime=true;";

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

**Option 1: MySql.Data (Connector/NET)**
* [MySQL dot net Connector/Net (MySql.Data.DLL)](http://www.mysql.com/downloads/connector/net/)
* MySql.Data.DLL is developed by Oracle Corporation, licensed under GPL License (http://www.gnu.org/licenses/old-licenses/gpl-2.0.html)._

**Option 2: Devart Express (dotConnect)**
* [Devart dotConnect for MySQL Express](https://www.devart.com/dotconnect/mysql/)
* For license agreement, please read: [https://www.devart.com/dotconnect/mysql/licensing-faq.html](https://www.devart.com/dotconnect/mysql/licensing-faq.html)
* Devart.Data.DLL
* Devart.Data.MySql.DLL

**Option 3: MySqlConnector (MIT)**
* [MySqlConnector: High Performance MySQL Library for .NET](https://mysqlconnector.net/)
* Project URL: https://github.com/mysql-net/MySqlConnector
* Licensed under MIT
* MySqlConnector.DLL

## Reminder

### Reminder 1

* Always use the default character set of `utf8mb4`, or `utf8` in older MySQL versions that do not support `utf8mb4`.
* It is recommended to use `convertzerodatetime=true` in the connection string for compatibility when handling null datetime values..