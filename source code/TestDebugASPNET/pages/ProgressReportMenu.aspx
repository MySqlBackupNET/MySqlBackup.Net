<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReportMenu.aspx.cs" Inherits="System.pages.ProgressReportMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Progress Report Demo</h1>
        <div style="line-height: 250%;">
            Recommended Basic Demo:<br />
            <a href="/ProgressReport2-2">Progress Report 2-2</a> - Using <code>ConcurrentDictionary</code> as an intermediary to store progress information<br />

            <a href="/ProgressReport2-3">Progress Report 2-3</a> - Using Portable Javascript Object to Build Progress Report Widget<br />

            <a href="/ProgressReport3">Progress Report 3</a> - Using Web Socket for Backend Server Pro-Actively Send Progress Status Data to FrontEnd

            <div style="border: 1px solid #c8e8ff; max-width: 900px; padding: 10px; background: #e4f4ff; margin-top: 15px;">
                Demo Theme: Light | Dark | Cyberpunk | Alien 1986 Terminal | Steampunk Victorian | Solar Fire | Futuristic HUD
            </div>



            <hr />
            Other Demo:<br />
            <a href="/ProgressReportSimple">Progress Report - Simple API Demo</a> - Using <code>ConcurrentDictionary</code> as an intermediary to store progress information<br />
            <a href="/ProgressReport1">Progress Report 1 - Demo Version 1</a> - Using SQLite as an intermediary to store progress information<br />
            <a href="/ProgressReport2">Progress Report 2-1 - Demo Version 2</a> - Using <code>ConcurrentDictionary</code> as an intermediary to store progress information<br />
        </div>

    </div>

</asp:Content>
