<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="SRFROWCA.Account.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerLogin">
        <div class="graybarLogin">
            Forgot Your Password?
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="margin:0 auto;">
                    <tr>
                        <td>
                            <label>
                                Enter your username:</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>OR</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Enter your email address:</label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button_example"
                                OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" CssClass="erro2" Visible="false"
                                ViewStateMode="Disabled"></asp:Label>
                        </td>
                    </tr>
                </table>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
</asp:Content>
