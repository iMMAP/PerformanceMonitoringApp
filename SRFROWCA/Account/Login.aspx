<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="SRFROWCA.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="containerLogin">
        <div class="graybarLogin">
            Please enter your username and password.
            <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Sign Up</asp:HyperLink>
            if you don't have an account.
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
                    OnLoginError="LoginUser_LoginError">
                    <LayoutTemplate>
                        <table style="margin:0 auto;">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="FailureText" runat="server" CssClass="error2" EnableViewState="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server" CssClass="textEntry" MaxLength="256" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                        CssClass="error2" ErrorMessage="User Name is required." ToolTip="User Name is required.">User Name is required.</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"
                                        MaxLength="128" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                        CssClass="error2" ErrorMessage="Password is required." ToolTip="Password is required.">Password is required.</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox ID="RememberMe" runat="server" />
                                    <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                                    <a href="ForgotPassword.aspx">Forgot Password?</a>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" CssClass="button_example" />
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:Login>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
</asp:Content>
