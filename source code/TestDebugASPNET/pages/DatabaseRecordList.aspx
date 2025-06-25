<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="DatabaseRecordList.aspx.cs" Inherits="System.pages.DatabaseRecordList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .records-container {
            /*padding: 20px;*/
        }

        .summary-info {
            font-weight: bold;
            margin-bottom: 15px;
            color: #333;
        }

        .records-table {
            border-collapse: collapse;
            background-color: white;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
        }

            .records-table thead {
                background-color: #f8f9fa;
            }

            .records-table th {
                padding: 12px;
                text-align: left;
                font-weight: bold;
                border-bottom: 2px solid #d3d7dc;
                border-top: 2px solid #d3d7dc;
                color: #495057;
            }

            .records-table td {
                padding: 10px 12px;
                border-bottom: 1px solid #dee2e6;
                vertical-align: top;
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

        .label-filename {
            font-weight: bold;
        }

        .label-sha {
            font-size: 7pt;
            margin-right: 2px;
            color: #939393;
        }

        .value-sha {
            font-size: 8pt;
        }

        .action-buttons {
            margin-bottom: 20px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <input type="hidden" id="hiddenPostbackAction" name="hiddenPostbackAction" />

    <div class="main-content">

        <div class="action-buttons">
            <asp:Button ID="btnLoadFiles" runat="server" Text="Load Files" OnClick="btnLoadFiles_Click" />
            <button type="button" onclick="deleteSelected();">Delete Selected Files</button>
            <button type="button" onclick="newBackup();">Start a New Backup/Export</button>
            <a href="/ReportProgressRestoreFileUpload" class="buttonmain">Start a New Restore/Import From File</a>
            <iframe id="frame1" name="frame1" style="height: 0; width: 0; border: none; display: inline-block;"></iframe>
        </div>

        <asp:Literal ID="ltlRecords" runat="server"></asp:Literal>

    </div>

    <script type="text/javascript">
        function toggleSelectAll() {
            var selectAll = document.getElementById('chkSelectAll');
            var checkboxes = document.getElementsByClassName('record-checkbox');

            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].checked = selectAll.checked;
            }
        }

        function deleteSelected() {

            const checkboxes = document.querySelectorAll('.record-checkbox');

            if (checkboxes.length === 0 || !Array.from(checkboxes).some(checkbox => checkbox.checked)) {
                showErrorMessage("Action Aborted", "No files selected");
                return;
            }

            spShowConfirmDialog(
                "Delete",  // Dialog box title
                "Are you sure to delete selected files?",  // Message
                "",  // Custom Data, leave blank here, the data will be submitted through input[type=checkbox]
                (customData) => {
                    // Yes
                    document.getElementById("hiddenPostbackAction").value = "delete";
                    document.forms[0].submit();
                },
                (customData) => {
                    // No
                    // do nothing
                }
            );
        }

        function restoreFileId(fileid) {
            spShowConfirmDialog(
                "Restore",
                "Are you sure to restore selected files?",
                "",
                () => {
                    // Yes
                    fetch(`/apiProgressReport?action=restore&id=${fileid}`)
                        .then(response => response.text())
                        .then(result => {
                            if (result === "1") {
                                window.location = "/ReportProgress";
                            } else {
                                const errMsg = result.split("|")[1] || "Unknown error occurred";
                                spShowMessage("Error", errMsg, false);
                            }
                        })
                        .catch(error => {
                            spShowMessage("Error", "Failed to connect to the server", false);
                        });
                },
                () => {
                    // No, do nothing
                }
            );
        }

        function newBackup() {
            spShowConfirmDialog(
                "Backup",
                "Are you sure you want to backup MySQL database?",
                "",
                () => {
                    // Yes
                    fetch(`/apiProgressReport?action=backup`)
                        .then(response => response.text())
                        .then(result => {
                            if (result === "1") {
                                window.location = "/ReportProgress";
                            } else {
                                const errMsg = result.split("|")[1] || "Unknown error occurred";
                                spShowMessage("Error", errMsg, false);
                            }
                        })
                        .catch(error => {
                            spShowMessage("Error", "Failed to connect to the server", false);
                        });
                },
                () => {
                    // No, do nothing
                }
            );
        }
    </script>

</asp:Content>
