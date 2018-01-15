MySql.Data v6.9.10
Source Code: https://dev.mysql.com/downloads/connector/net/6.9.html

MySql.Data v6.10.5
Source Code: https://dev.mysql.com/downloads/connector/net/6.10.html

1. The source code is originally obtained from www.mysql.com (oracle)
2. The binaries of MySql.Data.DLL in this repository are manually recompiled by me from the source code.
3. The reason of manually recompiling MySql.Data.DLL
- Reason 1:  mysql.com no more release official binary for older framework such as .NET Framework 2.0 and 4.0
- Reason 2: there are too many versions of MySql.Data.DLL among developers. During compilation of MySqlBackup.NET, it only recognize the version of MySql.Data.DLL when it is compiled. Therefore, I statically compile MySqlBackup.NET with specific version of MySql.Data.DLL.
4. If you wish to use other version of MySql.Data.DLL (such as the latest version), you can manually recompile MySqlBackup.NET which referencing the version of MySql.Data.DLL that you prefer.