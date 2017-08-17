This project is moved from [https://mysqlbackupnet.codeplex.com/](https://mysqlbackupnet.codeplex.com/).

# MySqlBackupNet

A tool to backup and restore MySQL database in C#/VB.NET/ASP.NET.

Article at [CodeProject.com](http://www.codeproject.com/Articles/256466/MySqlBackup-NET)

Install via NuGet: **PM> Install-Package MySqlBackup.NET**  
[https://www.nuget.org/packages/MySqlBackup.NET/](https://www.nuget.org/packages/MySqlBackup.NET/)

## Backup/Export a MySQL Database
```C#
string constring = "server=localhost;user=root;pwd=qwerty;database=test;";

// Important Additional Connection Options
constring += "charset=utf8;convertzerodatetime=true;";

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

// Important Additional Connection Options
constring += "charset=utf8;convertzerodatetime=true;";

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

This tool is develop in C# but able to be used in any .NET Language (i.e. VB.NET, F#, etc).

Another benefits of making this tool is, we don't have to rely on two small programs - MySqlDump.exe and MySql.exe to perform the backup and restore task. We will have better control on the output result in .NET way.

The most common way to backup a MySQL Database is by using MySqlDump and MySQL Workbench.

MySQL Workbench is good for developers, but, when comes to client or end-user, the recommended way is to get every parameter preset and all they need to know is press the big button "Backup" and everything is done. Using MySQL Workbench as a backup tool is not a suitable solution for client or end-user.

On the other hand, MySqlDump.exe cannot be executed directly from the Web Server. As most providers forbid that, MySqlBackup will be helpful in building a web-based (ASP.NET) backup tool. 

## Features

* Backup and Restore of MySQL Database
* Can be used in any .NET Languages.
* Export/Import to/from MemoryStream
* Conditional Rows Export (Filter Tables or Rows)
* Build-In Internal Encryption Function
* Able Restore to New Non-Existed Database
* Progress Report is Available for Both Export and Import Task.
* Able to export rows into different mode. (Insert, Insert Ignore, Replace, On Duplicate Key Update, Update)
* Can be used directly in ASP.NET or web services.

## Prerequisite / Dependencies

MySqlBackup.NET relies on the following component to work.
* [MySQL dot net Connector/Net (MySql.Data.DLL)](http://www.mysql.com/downloads/connector/net/)<br />_A reference of this DLL must be added into your project in order for MySqlBackup.NET to work.<br />MySql.Data.DLL is developed by Oracle Corporation, licensed under GPL License (http://www.gnu.org/licenses/old-licenses/gpl-2.0.html)._

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

### Reminder 2

DateTime conversion between MySQL and .NET Framework. In MySQL, there are various of DateTime format, such as null value or Date only data. But, in .NET Framework, there is no null value (or Date only) for DateTime. This error is not caused by MySqlBackup.DLL. MySql.Data.DLL (developed by Oracle) has decided to throw an exception of Data Conversion Error. Therefore, you are strongly recommended to apply the connection string option of **convertzerodatetime=true**. Example:

```
server=localhost;user=root;pwd=mypwd;charset=utf8;convertzerodatetime=true;
```
