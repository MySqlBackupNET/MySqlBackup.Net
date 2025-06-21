<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="QueryBrowser.aspx.cs" Inherits="System.pages.QueryBrowser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .div-table-result {
            max-width: calc(100vw - 50px);
            max-height: calc(100vh - 330px);
            overflow: auto;
            border: 2px solid #767676;
        }

            .div-table-result table {
                border-collapse: initial;
                border-spacing: 0;
            }

            .div-table-result th {
                position: sticky;
                top: 0;
                border-top: none;
                border-left: none;
                border-bottom: 1px solid black;
                border-right: 1px solid black;
            }

            .div-table-result td {
                border-top: none;
                border-left: none;
                border-bottom: 1px solid #878787;
                border-right: 1px solid #878787;
            }

            .div-table-result pre {
                width: 100%;
                max-width: 100%;
                box-sizing: border-box;
                overflow: auto;
            }

        table.table-settings {
            border-collapse: collapse;
            border-spacing: 0;
            border: 2px solid #515279;
            box-shadow: 5px 5px 5px #878787;
        }

            table.table-settings th {
                background: #515279;
                color: white;
                padding: 5px;
                border: 1px solid #adadad;
            }

            table.table-settings td {
                padding: 5px;
                border: 1px solid #adadad;
            }
    </style>
    <style id="style2" type="text/css"></style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">
        <b style="font-size: 15pt; color: #0347c2">MySQL</b>
        <asp:Button ID="btSelectShow" runat="server" Text="Select/Show (Enter)" OnClick="btSelectShow_Click" ClientIDMode="Static" />
        <asp:Button ID="btExecute" runat="server" Text="Execute (Ctrl+Enter)" OnClick="btExecute_Click" ClientIDMode="Static" />
        <asp:Button ID="btShowAllTables" runat="server" Text="Show All Tables" OnClick="btShowAllTables_Click" />
        Shift+Enter = New Line (Line Break) [<a href="#" onclick="toggleSettings(); return false;">Settings</a>]
        <br />
        <br />
        <div id="divSettings" style="display: none;">
            <table class="table-settings">
                <tr>
                    <th colspan="2">Settings</th>
                </tr>
                <tr>
                    <td>Take Effect After Postback</td>
                    <td>Max Rows:
                        <asp:TextBox ID="txtMaxRows" runat="server" TextMode="Number" Text="100" min="1" ClientIDMode="Static"></asp:TextBox>
                        Max Text Value Length:
                        <asp:TextBox ID="txtMaxTextValueLength" runat="server" TextMode="Number" Text="1000" min="50" ClientIDMode="Static"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Take Effect Real-time</td>
                    <td>Max Column Width (px):
                        <input type="number" id="txtMaxColumnWidth" value="300" min="100" />
                        Max Column Height (px):
                        <input type="number" id="txtMaxColumnHeight" value="200" min="40" />
                        Wrap Text:
                        <input type="checkbox" id="cbWrapText" />
                    </td>
                </tr>
            </table>
            <br />
        </div>

        <asp:TextBox ID="txtSql" runat="server" TextMode="MultiLine" Height="60px" Width="700px" ClientIDMode="Static" placeholder="SQL Statements (i.e. SELECT, INSERT, UPDATE, DELETE... SHOW TABLES;)"></asp:TextBox>

        <br />
        <br />

        <div class="table-grid">
            <asp:PlaceHolder ID="phResult" runat="server"></asp:PlaceHolder>
        </div>
    </div>

    <script>

        let style2 = document.getElementById("style2");

        let txtMaxRows = document.getElementById("txtMaxRows");
        let txtMaxTextValueLength = document.getElementById("txtMaxTextValueLength");
        let txtMaxColumnWidth = document.getElementById("txtMaxColumnWidth");
        let txtMaxColumnHeight = document.getElementById("txtMaxColumnHeight");
        let cbWrapText = document.getElementById("cbWrapText");

        let divSettings = document.getElementById('divSettings');
        let txtSql = document.getElementById('txtSql');
        let btExecute = document.getElementById('btExecute');
        let btSelectShow = document.getElementById('btSelectShow');

        function saveSettings() {
            let wrapTextChecked = cbWrapText.checked ? "1" : "0";
            let settingsString = `${txtMaxRows.value},${txtMaxTextValueLength.value},${txtMaxColumnWidth.value},${txtMaxColumnHeight.value},${wrapTextChecked}`;
            localStorage.setItem('queryBrowserSettings', settingsString);
        }

        function loadSettings() {

            let settingsLoaded = false;

            try {
                let settingsString = localStorage.getItem('queryBrowserSettings');
                if (settingsString) {
                    let settingsArray = settingsString.split(',');
                    if (settingsArray.length === 5) {
                        let maxRows = parseInt(settingsArray[0]);
                        let maxTextValueLength = parseInt(settingsArray[1]);
                        let maxColumnWidth = parseInt(settingsArray[2]);
                        let maxColumnHeight = parseInt(settingsArray[3]);
                        let wrapText = settingsArray[4] === '1';

                        txtMaxRows.value = maxRows >= 1 ? maxRows : 100;
                        txtMaxTextValueLength.value = maxTextValueLength >= 50 ? maxTextValueLength : 1000;
                        txtMaxColumnWidth.value = maxColumnWidth >= 100 ? maxColumnWidth : 300;
                        txtMaxColumnHeight.value = maxColumnHeight >= 40 ? maxColumnHeight : 200;
                        cbWrapText.checked = wrapText;

                        settingsLoaded = true;
                    }
                }
            }
            catch { }

            if (!settingsLoaded) {
                txtMaxRows.value = 100;
                txtMaxTextValueLength.value = 1000;
                txtMaxColumnWidth.value = 300;
                txtMaxColumnHeight.value = 200;
                cbWrapText.checked = false;
            }

            txtMaxRows.oninput = saveSettings;
            txtMaxTextValueLength.oninput = saveSettings;
            txtMaxColumnWidth.oninput = updateStyle2;
            txtMaxColumnHeight.oninput = updateStyle2;
            cbWrapText.oninput = updateStyle2;
        }

        function updateStyle2Only() {
            style2.innerHTML = `
.div-table-result td {
    max-width: ${txtMaxColumnWidth.value}px;
}

.div-table-result pre {
    max-height: ${txtMaxColumnHeight.value}px;
}
            `;

            if (cbWrapText.checked) {
                style2.innerHTML += `
.div-table-result pre {
    white-space: pre-wrap;
    word-wrap: break-word;
    box-sizing: border-box;
}
                `;
            }
        }

        function updateStyle2() {
            updateStyle2Only();
            saveSettings();
        }

        function toggleSettings() {
            if (divSettings.style.display == "none")
                divSettings.style.display = "block";
            else
                divSettings.style.display = "none";
        }

        document.addEventListener('DOMContentLoaded', function () {

            txtSql.addEventListener('keydown', function (e) {

                // Execute SQL (no return results)
                // Check for Enter (without Shift or Ctrl)
                if (e.key === 'Enter' && !e.shiftKey && !e.ctrlKey) {
                    // highlight the textarea
                    txtSql.style.backgroundColor = '#ffffcc';
                    setTimeout(function () {
                        txtSql.style.backgroundColor = '';
                    }, 200);
                    e.preventDefault(); // Prevent default behavior (new line)
                    btSelectShow.click(); // Trigger Select/Show button click
                    return false;
                }

                // Execute SELECT/SHOW (with return results)
                // Check for Ctrl+Enter
                if (e.ctrlKey && e.key === 'Enter') {
                    // highlight the textarea
                    txtSql.style.backgroundColor = '#ffffcc';
                    setTimeout(function () {
                        txtSql.style.backgroundColor = '';
                    }, 200);
                    e.preventDefault(); // Prevent default behavior (new line)
                    btExecute.click(); // Trigger Execute button click
                    return false;
                }

                // Default enter, no other actions
                // Check for Shift+Enter
                if (e.shiftKey && e.key === 'Enter') {
                    // Allow Shift+Enter to create a new line
                    // No preventDefault() here, so default behavior continues
                }
            });
        });

        txtSql.focus();
        txtSql.select();

        loadSettings();
        updateStyle2Only();

    </script>
</asp:Content>
