<%@ Page Title="" Language="C#" MasterPageFile="~/masterPage1.Master" AutoEventWireup="true" CodeBehind="ShaFilesCompute.aspx.cs" Inherits="System.pages.ShaFilesCompute" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="main-content">

        <h1>SHA256 Files Checksum Compute</h1>

        <asp:Button ID="btCompute" runat="server" Text="Compute SHA256 Checksums" OnClick="btCompute_Click" />

        <br />
        <br />
        File 1: 
    <asp:FileUpload ID="fileUpload1" runat="server" AllowMultiple="false" />
        File 2:
    <asp:FileUpload ID="fileUpload2" runat="server" AllowMultiple="false" />
        File 3:
    <asp:FileUpload ID="fileUpload3" runat="server" AllowMultiple="false" />
        <br />
        <br />
        <pre class="light-formatted"><asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder></pre>

    </div>

</asp:Content>
