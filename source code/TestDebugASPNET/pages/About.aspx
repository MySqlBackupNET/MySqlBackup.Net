<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="System.pages.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        .highlight {
            background-color: #f8f9fa;
            padding: 15px;
            border-left: 4px solid #3498db;
            margin: 20px 0;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>About</h1>

        <p><strong>MySqlBackup.NET</strong> is a comprehensive tool for backing up and restoring MySQL databases in C#, VB.NET, and other .NET languages.</p>

        <p>
            <strong>GitHub Repository:</strong><br>
            <a href="https://github.com/MySqlBackupNET/MySqlBackup.Net">https://github.com/MySqlBackupNET/MySqlBackup.Net</a>
        </p>

        <div class="highlight">
            <p>This application is a specialized development assistant tool designed specifically for MySqlBackup.NET development. It serves as a comprehensive testing, debugging, and demonstration platform that assists developers by detecting bugs and supporting the overall development process.</p>
        </div>

        <p>
            <strong>Documentation & Resources:</strong><br>
            For detailed documentation, guidelines, and wiki pages, please visit:<br>
            <a href="https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki">https://github.com/MySqlBackupNET/MySqlBackup.Net/wiki</a>
        </p>
        
        <p>Enjoy!</p>

    </div>

</asp:Content>
