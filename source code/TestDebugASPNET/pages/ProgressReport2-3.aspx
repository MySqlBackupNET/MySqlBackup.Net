<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ProgressReport2-3.aspx.cs" Inherits="System.pages.ProgressReport2_3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #divThemes {
            display: none;
            margin: 15px 0;
        }
    </style>

    <link id="linkThemeFile" href="/cssjs/MySqlBackup-Progress-Widget-Theme/steampunk.css" rel="stylesheet" />
    <script src="/cssjs/ProgressReportWidget2-3.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Progress Report 2-3</h1>
        <h2>Portable Javascript Object of MySqlBackupProgress Widget</h2>

        <button type="button" onclick="createTaskWidget();">Create Task Widget</button>
        <button type="button" onclick="changeTheme();">Change Theme</button>

        <div id="divThemes">
            <button type="button" onclick="changeTheme('light');">Light</button>
            <button type="button" onclick="changeTheme('dark');">Dark</button>
            <button type="button" onclick="changeTheme('cyberpunk');">Cyberpunk</button>
            <button type="button" onclick="changeTheme('retro');">
                Terminal<br />
                Alien 1986</button>
            <button type="button" onclick="changeTheme('steampunk');">
                Steampunk<br />
                Victorian</button>
            <button type="button" onclick="changeTheme('solarfire');">
                Solar<br />
                Fire</button>
            <button type="button" onclick="changeTheme('hud');">
                Futuristic<br />
                HUD</button>
        </div>

        <div id="div-backup-progress-widgets"></div>

    </div>

    <script>

        let newWidgetId = 0;
        let urlApiEndPoint = "/apiProgressReport2";

        function createTaskWidget() {
            let div = document.getElementById("div-backup-progress-widgets");
            let newWidget = document.createElement("div");
            newWidgetId++;
            newWidget.id = `div-backup-progress-widget-${newWidgetId}`;
            div.append(newWidget);

            // Build a MySqlBackupProgress widget object
            MySqlBackupProgress.init(newWidget.id, { apiUrl: urlApiEndPoint, widgetId: newWidgetId });
        }

        function changeTheme(themename) {

            if (themename)
                linkThemeFile.href = `/cssjs/MySqlBackup-Progress-Widget-Theme/${themename}.css`;

            divThemes.style.display = divThemes.style.display == "block" ? "none" : "block";
        }

    </script>
</asp:Content>