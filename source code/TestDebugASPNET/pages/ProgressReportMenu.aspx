<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReportMenu.aspx.cs" Inherits="System.pages.ProgressReportMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">
        
        <h1>Progress Report Demo</h1>
        <br />
        <a href="/ProgressReportSimple">Progress Report - Simple API Demo</a> - Using <code>ConcurrentDictionary</code> as an intermediary to store progress information<br />
        <br />
        <a href="/ProgressReport1">Progress Report 1 - Demo Version 1</a> - Using SQLite as an intermediary to store progress information<br />
        <br />
        <a href="/ProgressReport2">Progress Report 2 - Demo Version 2</a> - Using <code>ConcurrentDictionary</code> as an intermediary to store progress information<br />

    </div>

</asp:Content>
