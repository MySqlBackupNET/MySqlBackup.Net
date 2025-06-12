<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="DatabaseRecordList.aspx.cs" Inherits="System.pages.DatabaseRecordList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .records-container {
            padding: 20px;
        }
        
        .summary-info {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 15px;
            color: #333;
        }
        
        .records-table {
            width: 100%;
            border-collapse: collapse;
            background-color: white;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        
        .records-table thead {
            background-color: #f8f9fa;
        }
        
        .records-table th {
            padding: 12px;
            text-align: left;
            font-weight: bold;
            border-bottom: 2px solid #dee2e6;
            color: #495057;
        }
        
        .records-table td {
            padding: 10px 12px;
            border-bottom: 1px solid #dee2e6;
        }
        
        .records-table tbody tr:hover {
            background-color: #f8f9fa;
        }
        
        .view-link {
            color: #007bff;
            text-decoration: none;
            font-weight: 500;
        }
        
        .view-link:hover {
            text-decoration: underline;
        }
        
        .action-buttons {
            margin-bottom: 20px;
        }
        
        .btn {
            padding: 10px 20px;
            margin-right: 10px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s;
        }
        
        .btn-primary {
            background-color: #007bff;
            color: white;
        }
        
        .btn-primary:hover {
            background-color: #0056b3;
        }
        
        .btn-danger {
            background-color: #dc3545;
            color: white;
        }
        
        .btn-danger:hover {
            background-color: #c82333;
        }
        
        .error-message {
            padding: 15px;
            margin: 20px 0;
            border: 1px solid #f5c6cb;
            background-color: #f8d7da;
            color: #721c24;
            border-radius: 4px;
        }
        
        .success-message {
            padding: 15px;
            margin: 20px 0;
            border: 1px solid #c3e6cb;
            background-color: #d4edda;
            color: #155724;
            border-radius: 4px;
        }
        
        input[type="checkbox"] {
            cursor: pointer;
            width: 18px;
            height: 18px;
        }
    </style>

    <script type="text/javascript">
        function toggleSelectAll() {
            var selectAll = document.getElementById('chkSelectAll');
            var checkboxes = document.getElementsByClassName('record-checkbox');
            
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = selectAll.checked;
            }
        }
        
        function deleteSelected() {
            var checkboxes = document.getElementsByClassName('record-checkbox');
            var hasSelection = false;
            
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    hasSelection = true;
                    break;
                }
            }
            
            if (!hasSelection) {
                alert('Please select at least one record to delete.');
                return false;
            }
            
            return confirm('Are you sure you want to delete the selected records? This action cannot be undone.');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="action-buttons">
        <asp:Button ID="btnLoadFiles" runat="server" Text="Load Files" CssClass="btn btn-primary" OnClick="btnLoadFiles_Click" />
        <asp:Button ID="btnDeleteSelected" runat="server" Text="Delete Selected Files" CssClass="btn btn-danger"
            OnClick="btnDeleteSelected_Click" OnClientClick="return deleteSelected();" />
    </div>

    <asp:Literal ID="ltlMessage" runat="server"></asp:Literal>

    <asp:Literal ID="ltlRecords" runat="server"></asp:Literal>
</asp:Content>
