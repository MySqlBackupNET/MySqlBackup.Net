<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Result.aspx.cs" Inherits="MySqlBackupASPNET.Result" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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
                <p><b>Exported Dump Content:</b></p>
                 <div id="contact_form" style="width: 100%;">
                    <form id="form1" runat="server">
                        <table>
                            <tr>
                                <td><asp:Button ID="btDownload" runat="server" Text="Download" CssClass="submit_btn" OnClick="btDownload_Click" /></td>
                                <td> Dump content is stored for 5 minutes in this session.</td>
                            </tr>
                        </table>
                        
                        <pre style="font-family: 'Courier New', Courier, 'Nimbus Mono L', monospace; font-size: 9pt; text-align: left; line-height: 12px;"><asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder></pre>
                    </form>
                </div>
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