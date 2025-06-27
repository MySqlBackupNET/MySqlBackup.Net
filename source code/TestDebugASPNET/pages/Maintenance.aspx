<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Maintenance.aspx.cs" Inherits="System.pages.Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input id="hiddenPostbackAction" type="hidden" name="hiddenPostbackAction" />

    <div class="div-center-framed-content">

        <h1>Maintenance</h1>

        <asp:Panel ID="panelButton" runat="server">
            <asp:Button ID="btDeleteTestDatabase" runat="server" Text="Delete All Test_xxx_... Databases in MySQL Server" OnClick="btDeleteTestDatabase_Click" OnClientClick="showBigLoading(0);" />
            <asp:Button ID="btDeleteTempDumpFiles" runat="server" Text="Delete All Dump Files in Temp Folder" OnClick="btDeleteTempDumpFiles_Click" OnClientClick="showBigLoading(0);" />
            <asp:Button ID="btDeleteTaskReport" runat="server" Text="Delete All Task Reports" OnClick="btDeleteTaskReport_Click" OnClientClick="showBigLoading(0);" />
            <button type="button" onclick="requestConfirmDropTables()">Drop All Tables</button>
        </asp:Panel>
        <asp:Panel ID="panelConfirmDelete" runat="server">
            Are you sure to delete the following databases?<br /><br />

            <asp:Button ID="btDeleteDatabaseYes" runat="server" Text="Yes, Confirm Delete" OnClick="btDeleteDatabaseYes_Click" />
            <asp:Button ID="btDeleteDatabaseNo" runat="server" Text="No, Cancel" OnClick="btDeleteDatabaseNo_Click" />
        </asp:Panel>

        <br />
        <br />
        <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>

    </div>

    <script>
        function requestConfirmDropTables() {

            spShowConfirmDialog(
                "Drop Tables",
                "Are you sure to drop all tables?",
                "",
                () => {
                    // Yes
                    document.querySelector("#hiddenPostbackAction").value = "dropAllTables";
                    document.forms[0].submit();
                },
                () => {
                    // No, do nothing
                }
            );
        }
    </script>
</asp:Content>
