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

            <asp:CheckBox ID="cbRunStreamTest" runat="server" Checked="true" /> (For testing) Run Stream Test<br />
            <asp:CheckBox ID="cbRunMemoryStreamTest" runat="server" Checked="true" /> (For testing) Run Memory Test

            Restore File:
            <asp:FileUpload ID="fileUploadRestore" runat="server" />
        </div>

        <br />

        <asp:Button ID="btTest" runat="server" Text="Run Test" OnClick="btTest_Click" OnClientClick="showBigLoading(0);" />
        <asp:Button ID="btBackup" runat="server" Text="Backup" OnClick="btBackup_Click" OnClientClick="showBigLoading(3000);" />
        <asp:Button ID="btRestore" runat="server" Text="Restore" OnClick="btRestore_Click" OnClientClick="showBigLoading(0);" />

        <div style="height: 20px;"></div>

        <pre class="light-formatted"><asp:PlaceHolder ID="phResult" runat="server"></asp:PlaceHolder></pre>
    </div>

</asp:Content>
