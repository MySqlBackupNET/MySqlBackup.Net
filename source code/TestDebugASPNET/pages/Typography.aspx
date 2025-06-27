<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="Typography.aspx.cs" Inherits="System.pages.Typography" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="divGoodMsgBar">
        Ok. Some good message
    </div>

    <div class="divErrorMsgBar">
        Error: Some error message
    </div>

    <div class="div-center-framed-content">

        <h1>H1 Title</h1>

        <h2>H2 Title</h2>

        <h3>H3 Title</h3>

        <input type="submit" value="Input Submit" onclick="return false;" />
        <button type="button" value="Button" onclick="return false;">Button</button>
        <a href="#" class="buttonmain" onclick="return false;">A Link Button</a>

        <h2>Link</h2>

        This is a <a href="#" onclick="return false;">Link to another page</a>.

        <h2>Message Box</h2>

        <button type="button" onclick="showMessage('Ok', 'Good message', true);">
            Test Show Good Message
        </button>
        <button type="button" onclick="showMessage('Error', 'Bad Message', false);">
            Test Show Bad Message
        </button>

        <h2>Action Dialog Box</h2>

        <table>
            <tr>
                <td>Box Title:</td>
                <td><input type="text" id="confirmationBoxTitle" value="Action Title"></td>
            </tr>
            <tr>
                <td>Box Message:</td>
                <td><input type="text" id="confirmationBoxMessage" value="Some action message"></td>
            </tr>
            <tr>
                <td>Custom Data:</td>
                <td><input type="text" id="confirmationBoxCustomData" value="item:1"></td>
            </tr>
        </table>
        
        <br />
        <button type="button" onclick="showConfirm();">Show Confirm Message</button>

        <script>

            function showConfirm() {
                let title = document.getElementById("confirmationBoxTitle").value;
                let msg = document.getElementById("confirmationBoxMessage").value;
                let customdata = document.getElementById("confirmationBoxCustomData").value;

                spShowConfirmDialog(
                    title,
                    msg,
                    customdata,
                    (customData) => {
                        showGoodMessage("Yes", "You clicked [Yes] with data: " + customData);
                        // call some other function here
                        // save(); 
                    },
                    (customData) => {
                        showErrorMessage("No", "You clicked [No] with data:" + customData);
                        // call some other function here
                        // cancel(); 
                    }
                );
            }

        </script>

        <h2>Normal Text Lines</h2>

        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec hendrerit volutpat malesuada. Donec maximus<br />
        faucibus ipsum condimentum laoreet. Maecenas egestas ac enim eget mattis. Suspendisse vehicula, erat eget aliquet<br />
        bibendum, tortor diam condimentum elit, non feugiat neque eros id lacus. Proin interdum tortor pellentesque est<br />
        accumsan malesuada. Duis facilisis varius feugiat. Aenean fermentum scelerisque est, ut blandit neque blandit et.<br />
        Vestibulum varius odio vitae dolor sollicitudin, scelerisque luctus metus eleifend. Curabitur auctor tristique nibh,<br />
        quis dapibus justo dignissim et. Donec blandit nulla in odio ornare, at ullamcorper lectus viverra.<br />
        <br />

        <h2>PRE.light-formatted</h2>

        <pre class="light-formatted">INSERT INTO product_details (product_id, product_name, unit_price, items_in_stock) VALUES
(101, 'Mechanical Keyboard', 120.00, 75),
(102, 'Gaming Headset', 85.50, 120),
(103, 'External Hard Drive', 99.99, 90),
(104, 'Wireless Mouse', 30.00, 250),
(105, 'USB-C Hub', 45.75, 180);</pre>

        <table>
            <tr>
                <td>Text</td>
                <td>
                    <input type="text" />
                </td>
            </tr>
            <tr>
                <td>Number</td>
                <td>
                    <input type="number" />
                </td>
            </tr>
            <tr>
                <td>Date</td>
                <td>
                    <input type="date" />
                </td>
            </tr>
            <tr>
                <td>Textarea</td>
                <td>
                    <textarea></textarea>
                </td>
            </tr>
            <tr>
                <td>Select</td>
                <td>
                    <select>
                        <option>Option 1</option>
                        <option>Option 2</option>
                        <option>Option 3</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td>Checkbox</td>
                <td>
                    <input type="checkbox" />
                    Yes
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
