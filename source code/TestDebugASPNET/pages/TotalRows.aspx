<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="TotalRows.aspx.cs" Inherits="System.pages.TotalRows" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Total Rows</h1>

        <asp:Button ID="btGetTotalRows" runat="server" Text="Get Current Connected Database" OnClick="btGetTotalRows_Click" OnClientClick="showBigLoading(0);" />
        <asp:Button ID="btGetAllDatabases" runat="server" Text="Get All Databases" OnClick="btGetAllDatabases_Click" OnClientClick="showBigLoading(0);" />

        <hr />

        <asp:Literal ID="ltResult" runat="server"></asp:Literal>

    </div>

</asp:Content>
