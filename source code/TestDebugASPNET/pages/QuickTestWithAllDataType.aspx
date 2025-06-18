<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="QuickTestWithAllDataType.aspx.cs" Inherits="System.pages.QuickTestWithAllDataType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
        <h1>Quick Test With All MySQL Data Type</h1>
        *This test requires MySQL Root user or user with CREATE DATABASE privilege.<br />
        <br />

        <asp:Button ID="btRun" runat="server" Text="Run Test" OnClick="btRun_Click" OnClientClick="showBigLoading(); hideButton(this);" /><br />
        <br />

        <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>
    </div>

    <script type="text/javascript">
        function hideButton(bt) {
            bt.style.display = "none";
        }
    </script>

</asp:Content>
