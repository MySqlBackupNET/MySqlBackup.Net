<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="System.pages.Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">

        <h1>Maintenance</h1>

        <asp:Button ID="btDeleteTestDatabase" runat="server" Text="Delete All Test_xxx_... Databases in MySQL Server" OnClick="btDeleteTestDatabase_Click" OnClientClick="showBigLoading(0);" />
        <asp:Button ID="btDeleteTempDumpFiles" runat="server" Text="Delete All Dump Files in Temp Folder" OnClick="btDeleteTempDumpFiles_Click" OnClientClick="showBigLoading(0);" />

        <br />
        <br />
        <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>

    </div>

</asp:Content>
