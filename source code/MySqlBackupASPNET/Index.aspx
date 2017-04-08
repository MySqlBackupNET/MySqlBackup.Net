<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MySqlBackupASPNET.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>MySqlBackup.NET</title>
<meta name="keywords" content="MySqlBackup, MySQL, C#, VB.NET, Backup, Restore, Export, Import, ASP.NET" />
<meta name="description" content="A programming tool to backup and restore of MySQL database in C#/VB.NET/ASP.NET" />
<link href="templatemo_style.css" rel="stylesheet" type="text/css" />

<link rel="stylesheet" type="text/css" href="css/ddsmoothmenu.css" />

<script type="text/javascript" src="scripts/jquery.min.js"></script>
<script type="text/javascript" src="scripts/ddsmoothmenu.js">

/***********************************************
* Smooth Navigational Menu- (c) Dynamic Drive DHTML code library (www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/

</script>

<script type="text/javascript">

    ddsmoothmenu.init({
        mainmenuid: "templatemo_menu", //menu DIV id
        orientation: 'h', //Horizontal or vertical menu: Set to "h" or "v"
        classname: 'ddsmoothmenu', //class added to menu's outer DIV
        //customtheme: ["#1c5a80", "#18374a"],
        contentsource: "markup" //"markup" or ["container_id", "path_to_menu_file"]
    })

</script>

</head>
<body>

<div id="templatemo_wrapper">

	<div id="templatemo_header">
    
    	<div id="site_title"><h1><a href="http://mysqlbackup.somee.com/" target="_parent">MySqlBackup.NET</a></h1></div>
        
        <div id="templatemo_menu" class="ddsmoothmenu">
            <ul>
              	<li><a href="http://mysqlbackup.somee.com/" class="selected">Home</a></li>
          		<li><a href="http://mysqlbackupnet.codeplex.com/releases">Download Source Code</a></li>
                <li><a href="http://mysqlbackupnet.codeplex.com/documentation">Documentation</a></li>
                <li><a href="http://mysqlbackupnet.codeplex.com/discussions">Discussion</a></li>
            </ul>
            <br style="clear: left" />
        </div> <!-- end of templatemo_menu -->
        
    </div> <!-- end of header -->
    
    <div id="templatemo_main">
        <div class="col_w900 float_l">
        	
             <div>
                <p>This site serves as a simple demo of using MySqlBackup.NET in ASP.NET.</p>
                <p><b>MySqlBackup.NET</b> is a C# open source MySQL database backup and restore tool. 
                    It is an alternative to MySqlDump.
                    It can be used in VB.NET projects too. 
                    Official Project Site: <a href="http://mysqlbackupnet.codeplex.com">http://mysqlbackupnet.codeplex.com</a></p>
                 <p>
                    Below is a simple demo for backing up and restoring a MySQL database:
                </p>
                 <br />
                 <div id="contact_form" style="width: 100%;">
                    <form id="form1" runat="server">
                        <table style="width: 100%; border-collapse: collapse;">
                            <tr>
                                <td style="padding: 5px; border: 1px solid #5A6B7C; background-color: #465767;">
                                    <span style="font-weight:bold; color: #F7F7F7;">Simple Test (ASP.NET 4.5, MySqlBackup.NET 2.0.7)</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 5px; border: 1px solid #465767;">
                                    <b>MySQL Connection String</b>:<br />
                                    <asp:TextBox ID="txtConnString" runat="server" Font-Names="Courier New" Width="90%"></asp:TextBox><br />
                                    <i>Example: server=www.mywebsite.com;user=root;password=qwerty;database=test;allowzerodatetime=true;connectiontimeout=60;</i>
                                    <br />
                                    <br />
                                    <table>
                                        <tr>
                                            <td><asp:Button ID="btBackup" runat="server" CssClass="submit_btn" Text="Backup / Export Database" OnClick="btExport_Click" /><br /></td>
                                            <td><i>Note: Maximum length is limited to 100KB.</i></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Button ID="btRestore" runat="server" CssClass="submit_btn" Text="Restore / Import Database" OnClick="btImport" /></td>
                                            <td>Dump File: <asp:FileUpload ID="FileUpload1" onchange="return checkFileExtension(this);" runat="server" /><br /><i>(Support file type of SQL or ZIP)</i></td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lbError" runat="server" Visible="False" EnableViewState="False" ForeColor="Red" ViewStateMode="Disabled"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </form>
                </div>
                    <br /><br />
                    <strong>Notes</strong>:<br />
                    <ul class="tmo_list">
                        <li>All activities, inputs and outputs are not logged, saved and unable to be traced.</li>
                        <li>Data transmission of this website is not 
                            protected by secure connection (https). <br />
                            Data sent and received in human readable clear text form without encryption. <br />
                            You are advised not to use the service hosted here on your real and confidential website&#39;s database.
                        </li>
                        <li>Below are some of the online <strong>FREE MySQL Hosting</strong>, you may use it for testing here.
                            <ul class="tmo_list">
                                <li><a href="http://www.freesqldatabase.com/">FreeSQLDatabase (http://www.freesqldatabase.com/)</a></li>
                                <li><a href="http://www.db4free.net/">db4free (http://www.db4free.net/)</a></li>
                                <li><a href="http://www.freemysqlhosting.net/">Free MySQL Hosting (http://www.freemysqlhosting.net/)</a></li>
                                <li><a href="http://www.freemysql.net/">FreeMySQL.NET (http://www.freemysql.net/)</a></li>
                            </ul>
                        </li>
                        <li>The services hosted here are for testing purposes and for mini size of MySQL database only. <br />
                            The maximum length of the output dump content is limited to <b>100KB</b> in this website.</li>
                        <li>On using the services of this website, you are agreed to the user agreement.<br />
                            If you do not agree with the user agreement, do not use the services in this website.
                        </li>
                    </ul>
                 <br />
                    <table style="width: 100%; border-collapse: collapse;">
                        <tr>
                            <td style="padding: 5px; border: 1px solid #5A6B7C; background-color: #465767;">
                                <span style="font-weight:bold; color: #F7F7F7;">User Agreement</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 5px; border: 1px solid #465767;">
This is free and unencumbered software released into the public domain.<br />
                                <br />
                                Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

                                <br />
                                <br />
                                In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.<br />
                                <br />
                                THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

                                <br />
                                <br />
                                For more information, please refer to &lt;<a href="http://unlicense.org/">http://unlicense.org/</a>&gt;</td>
                        </tr>
                    </table>
            </div> 
        </div>

	    <div class="cleaner"></div>
    </div> <!-- end of main -->
</div>

<div id="templatemo_footer_wrapper">
    <div id="templatemo_footer">
        Powered by <a href="http://mysqlbackupnet.codeplex.com">mysqlbackupnet.codeplex.com</a> | 
        Website Designed by <a href="http://www.templatemo.com" target="_parent">Free CSS Templates</a>
        <div class="cleaner"></div>
    </div>
</div> 
  
</body>
</html>

<script type="text/javascript">
    function checkFileExtension(elem) {
        var filePath = elem.value;

        if (filePath.indexOf('.') == -1)
            return false;

        var validExtensions = new Array();
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

        validExtensions[0] = 'sql';
        validExtensions[1] = 'zip';

        for (var i = 0; i < validExtensions.length; i++) {
            if (ext == validExtensions[i])
                return true;
        }

        elem.value = null;
        alert('Only files with extension of ".sql" or ".zip" is allowed.');
        return false;
    }
</script>