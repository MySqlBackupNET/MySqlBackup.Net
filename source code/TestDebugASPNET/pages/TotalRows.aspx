<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="TotalRows.aspx.cs" Inherits="System.pages.TotalRows" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Total Rows</h1>

        <asp:Button ID="btGetTotalRows" runat="server" Text="Get Total Rows and Database Size" OnClick="btGetTotalRows_Click" OnClientClick="showBigLoading(0);" />

        <asp:Literal ID="ltResult" runat="server"></asp:Literal>

    </div>

</asp:Content>
