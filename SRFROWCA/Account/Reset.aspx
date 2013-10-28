<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Reset.aspx.cs" Inherits="SRFROWCA.Account.Reset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                Choose your new password.
            </td>
        </tr>
        <tr>
            <td>
                New Password:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password"
                    Text="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                Confirm Password:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="50"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Confirm Password"
                    Text="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords don't match."
                    ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
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
