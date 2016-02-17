<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Reset.aspx.cs" Inherits="SRFROWCA.Account.Reset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div>
                    <h1 class="grey lighter smaller">Choose your new password.</h1>
                    <hr />
                </div>
                <table>
                    <tr>
                        <td>New Password:</td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password" CssClass="error2"
                                Text="Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword" CssClass="error2"
                                ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Confirm Password:</td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="128"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Confirm Password" CssClass="error2"
                                Text="Required" ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords don't match." CssClass="error2"
                                ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-sm btn-primary" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" CssClass="info-message" Visible="false"
                                ViewStateMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
