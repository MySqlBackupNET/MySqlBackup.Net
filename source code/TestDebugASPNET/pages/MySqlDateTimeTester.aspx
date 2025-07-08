<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="MySqlDateTimeTester.aspx.cs" Inherits="System.pages.MySqlDateTimeTester" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="div-center-framed-content">

        <h1>MySqlDateTime Tester</h1>

        <asp:Button ID="btTestMicroseconds" runat="server" Text="Test Microseconds" OnClick="btTestMicroseconds_Click" OnClientClick="showBigLoading(0);" />
        <asp:Button ID="btTestFull" runat="server" Text="Test All Date and Time" OnClick="btTestFull_Click" OnClientClick="showBigLoading(0);" />

        <hr />
        Test Result:<br />
        <pre class="light-formatted"><asp:Literal ID="ltResult" runat="server"></asp:Literal></pre>
    </div>

</asp:Content>
