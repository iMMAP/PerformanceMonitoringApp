<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditKeyFigureCategory.aspx.cs" Inherits="SRFROWCA.KeyFigures.EditKeyFigureCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
    <tr>
        <td>English:</td>
        <td><asp:TextBox ID="txtKeyFigEng" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
        <td>French:</td>
        <td><asp:TextBox ID="txtKeyFigFr" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
        
    </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
            </td>
        </tr>
</table>
</asp:Content>
