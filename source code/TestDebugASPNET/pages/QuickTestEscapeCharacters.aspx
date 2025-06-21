<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="QuickTestEscapeCharacters.aspx.cs" Inherits="System.pages.QuickTestEscapeCharacters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
        <h1>Quick Test - Backup/Restore Escape Characters</h1>
        *This test requires MySQL Root user or user with CREATE DATABASE privilege.<br />
        <br />

        <asp:Button ID="btRun" runat="server" ClientIDMode="Static" Text="Run Test" OnClick="btRun_Click" OnClientClick="showBigLoading(0); hideButton(this);" />
        <asp:CheckBox ID="cbNoTryCatch" runat="server" />
        Run Without Try Catch
        <asp:CheckBox ID="cbCleanDatabaseAfterUse" runat="server" Checked="true" /> Clean Up Database After Use 
        <asp:CheckBox ID="cbNoBackSlashEscape" runat="server" Checked="false" /> Run with [SQL_MODE=NO_BACKSLASH_ESCAPES] 
        <br />
        Rows Export Mode:
        <asp:CheckBox ID="cbInsert" runat="server" Checked="true" />
        Insert
        <asp:CheckBox ID="cbInsertIgnore" runat="server" Checked="false" />
        Insert Ignore
        <asp:CheckBox ID="cbReplace" runat="server" Checked="false" />
        Replace
        <asp:CheckBox ID="cbUpdate" runat="server" Checked="false" />
        Update
        <asp:CheckBox ID="cbInsertUpdate" runat="server" Checked="false" />
        Insert Update
        <br />
        <br />

        <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>
    </div>

    <script type="text/javascript">
        function hideButton(bt) {
            bt.style.display = "none";
        }
    </script>

</asp:Content>
