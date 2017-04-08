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
Import/Restore
```C#
string constring = "server=localhost;user=root;pwd=qwerty;database=test;";

// Important Additional Connection Options
constring += "charset=utf8;convertzerodatetime=true;";

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

On the other hand, MySqlDump.exe cannot be used for Web applications. As most providers forbid that, MySqlBackup will be helpful in building a web-based (ASP.NET) backup tool. 

## Reminder for Using UTF8 and Unicode Characters

If your database involves any UTF8 or Unicode Characters. You must use a MySQL database with default character of UTF8. MySqlBackup.NET stands on top of MySql.Data.DLL. MySql.Data.DLL only works well with UTF8 database and tables while handling Unicode Characters, such as

* Western European specific languages, the character of 'À', 'ë', 'õ', 'Ñ'.
* Russian, Hebrew, India, Arabic, Chinese, Korean, Japanese characters, etc.

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
