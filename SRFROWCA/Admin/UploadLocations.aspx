<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadLocations.aspx.cs" Inherits="SRFROWCA.Admin.UploadLocations" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table>
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                    ViewStateMode="Disabled"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <br />
                <b>Please select excel file and click on import. It might take a while to complete.
                    Excel Sheet Name should be 'Sheet1' and Column order and name should be
                    (without hyphen) 'RowNumber' 'Cluster' 'sobjectives' 'objectives' 'Indicators' 'Activities'
                    'Data' 'Unit' </b>
                <br />
            </td>
        </tr>
        <tr>
            <td>
                Country Name:            
                <asp:TextBox ID="txtCountryName" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtCountryName" ErrorMessage="Required" Text="Required"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:FileUpload ID="fuExcel" runat="server" />
                <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
