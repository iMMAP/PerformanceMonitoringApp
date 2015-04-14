<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditKeyFigureIndicator.aspx.cs" Inherits="SRFROWCA.KeyFigures.EditKeyFigureIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Category:</td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server"  Width="300px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            </td>
            <td>Sub Category:</td>
            <td>
                <asp:DropDownList ID="ddlSubCategory"  Width="300px" runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                    CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlSubCategory"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>English:</td>
            <td>
                <asp:TextBox ID="txtKeyFigEng" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
            <td>French:</td>
            <td>
                <asp:TextBox ID="txtKeyFigFr" runat="server" Width="300px" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
            </td>
        </tr>
    </table>
</asp:Content>
