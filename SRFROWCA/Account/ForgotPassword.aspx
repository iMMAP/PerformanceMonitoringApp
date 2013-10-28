<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="SRFROWCA.Account.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblHeaderMessage" runat="server" CssClass="info-message" Visible="true"
                    ViewStateMode="Disabled" Text="Forgot your Password?"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Enter your username:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                OR
            </td>
        </tr>
        <tr>
            <td>
                Enter your email address:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="server" CssClass="info-message" Visible="false"
                    ViewStateMode="Disabled"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
