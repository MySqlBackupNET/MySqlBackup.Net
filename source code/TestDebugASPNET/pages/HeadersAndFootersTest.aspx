<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="HeadersAndFootersTest.aspx.cs" Inherits="System.pages.HeadersAndFootersTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>SQL Dump Headers & Footers Tester</h1>

        <asp:Button ID="btRun" Runat="server" Text="Run Test" OnClick="btRun_Click" OnClientClick="showBigLoading(0);" />

        <hr />

        <pre class="light-formatted"><asp:Literal ID="ltResult" runat="server"></asp:Literal></pre>
    </div>
</asp:Content>
