<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditKeyFigureSubCategory.aspx.cs" Inherits="SRFROWCA.KeyFigures.EditKeyFigureSubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>Category</td>
            <td>
                <asp:DropDownList ID="ddlCategory" runat="server" Width="300px"></asp:DropDownList></td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
        </tr>
        <tr>
            <td>English:</td>
            <td>
                <asp:TextBox ID="txtKeyFigEng" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
            <td>French:</td>
            <td>
                <asp:TextBox ID="txtKeyFigFr" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
            <td>
                <asp:CheckBox ID="cbIsPopulation" runat="server" Text="Is Population" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
            </td>
        </tr>
    </table>
</asp:Content>
