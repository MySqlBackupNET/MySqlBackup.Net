<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="MemoryStreamDemo.aspx.cs" Inherits="System.pages.MemoryStreamDemo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Memory Stream / Compression / Encryption</h1>

        <hr />

        *This page will export and import in memory stream without saving it as physical file.<br />
        <br />

        <div style="line-height: 400%;">
            <asp:CheckBox ID="cbCompress" runat="server" />
            Compress the output (using GZip)<br />
            <asp:CheckBox ID="cbEncrypt" runat="server" />
            Encrypt the output (Using AES 256 bit)<br />
            <asp:CheckBox ID="cbExportRows" runat="server" />
            Export Rows<br />

            Encryption Password:
            <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
            <br />

            Restore File:
            <asp:FileUpload ID="fileUploadRestore" runat="server" />
        </div>

        <br />

        <asp:Button ID="btBackup" runat="server" Text="Backup" OnClick="btBackup_Click" OnClientClick="showBigLoading(3000);" />
        <asp:Button ID="btRestore" runat="server" Text="Restore" OnClick="btRestore_Click" OnClientClick="showBigLoading(0);" />

    </div>

</asp:Content>
