<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="QuickTestWithAllDataType.aspx.cs" Inherits="System.pages.QuickTestWithAllDataType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="div-center-framed-content">

        <h1>Quick Test With All MySQL Data Types</h1>
        *This is one of the most important tests of all. This test requires MySQL Root user or user with CREATE DATABASE privilege.<br />
        <br />

        <asp:Button ID="btRun" runat="server" ClientIDMode="Static" Text="Run Test" OnClick="btRun_Click" OnClientClick="showBigLoading(0); hideButton(this);" />
        <asp:CheckBox ID="cbNoTryCatch" runat="server" />
        Run Without Try Catch
        <asp:CheckBox ID="cbPreserveDb1Db3" runat="server" Checked="true" />
        Preserve DB1 and DB3 (Used for Data Integrity Test)
        <hr />
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
