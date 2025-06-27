<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ConditionalTablesExport.aspx.cs" Inherits="System.pages.ConditionalTablesExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        .maintb {
            margin-top: 20px;
        }

        .maintb table {
            border-collapse: collapse;
        }

        .maintb tr {
            height: 50px;
        }

        .maintb th {
            padding: 10px;
            background: #235a82;
            color: white;
            border: 1px solid #a3a3a3;
        }

        .maintb tr th:nth-child(3) {
            width: 620px;
        }

        .maintb td {
            padding: 0 10px;
            border: 1px solid #c8c8c8;
        }

        .maintb input[type=text] {
            width: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Prevent implicit submission of the form -->
    <button type="submit" disabled style="display: none" aria-hidden="true"></button>

    <div class="div-center-framed-content">

        <h1>Conditional Tables' Rows Export</h1>

        <asp:Button ID="btExport" runat="server" Text="Export" OnClick="btExport_Click" ClientIDMode="Static" OnClientClick="return validateExport();" />
        <asp:Button ID="btFetchTables" runat="server" Text="Fetch Tables" OnClick="btFetchTables_Click" />
        <asp:CheckBox ID="cbRunWithoutTryCatch" runat="server" /> Run Without Try Catch <br />
        *This will create a record at [<a href="/DatabaseRecordList">Database Record</a>]
        <div class="maintb" id="maintb">
            <asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
        </div>
    </div>

    <script>
        function hidePanels() {
            const btExport = document.getElementById("btExport");
            const maintb = document.getElementById("maintb");

            btExport.style.display = "none";
            maintb.style.display = "none";

            showBigLoading(0);
        }

        function validateExport() {
            const checkboxes = document.querySelectorAll('#maintb_body input[type="checkbox"]');
            const anyChecked = Array.from(checkboxes).some(checkbox => checkbox.checked);
            if (!anyChecked) {
                showMessage("Task Cancelled", "Please select at least one table.", false);
                return false;
            }
            return true;
        }

        function toggleSelectAll(cb) {
            const checkboxes = maintb.querySelectorAll('#maintb input[type="checkbox"]');
            const inputSqls = maintb.querySelectorAll("#maintb input[type='text']");
            checkboxes.forEach(checkbox => {
                checkbox.checked = cb.checked;
            });
            inputSqls.forEach(inputSql => {
                inputSql.style.display = cb.checked ? "block" : "none";
            });
        }

        function enableTable(cb, tableName) {
            const inputSql = document.getElementById(tableName);
            if (inputSql) {
                inputSql.style.display = cb.checked ? 'block' : 'none';
            }
        }
    </script>
</asp:Content>
