<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UploadOPSProjectDetails.aspx.cs" Inherits="SRFROWCA.OPS.UploadOPSProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerLogin">
        <table border="0">            
            <tr>
                <td>
                    <br />
                    <br />
                    <ul>
                        <li>Please select excel file having OPS project details and click on 'import'. </li>
                        <li>Excel Sheet Name should be <strong>'Sheet1'</strong> </li>
                        <li>The first row must have <strong>column headers (column names)</strong> </li>
                        <li>It might take a while to import depending on the data in excel file.</li>
                    </ul>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fuOPSProjectDetails" runat="server" />
                    <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click"
                        CssClass="button_example" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                        ViewStateMode="Disabled"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
