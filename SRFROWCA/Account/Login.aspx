<%@ Page Title="Log On" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="SRFROWCA.Account.Login" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style>
        .login-container {
            margin: 0 auto;
            width: 460px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <div class="page-content">
        <div class="login-layout">
            <div class="col-sm-10 col-sm-offset-1">
                <div class="login-container">
                    <div class="center">
                    </div>
                    <div class="position-relative">
                        <div id="login-box" class="login-box visible widget-box no-border">
                            <div class="widget-body">
                                <div class="widget-main">
                                    <h4 class="header blue lighter label-xlg">
                                        <i class="icon-coffee green"></i>
                                        <asp:Localize ID="localEnterYourInfo" runat="server" Text="Please Enter Your Information" meta:resourcekey="localEnterYourInfoResource1"></asp:Localize>
                                    </h4>
                                    <div class="space-6">
                                    </div>
                                    <asp:Login ID="LoginUser" runat="server" EnableViewState="False" RenderOuterTable="False"
                                        OnLoggedIn="LoginUser_LoggedIn" OnLoginError="LoginUser_LoginError" meta:resourcekey="LoginUserResource1">
                                        <LayoutTemplate>
                                            <label class="block clearfix">
                                                <span class="block">
                                                    <asp:Label ID="FailureText" runat="server" CssClass="error2" EnableViewState="False" meta:resourcekey="FailureTextResource1"></asp:Label>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="UserName" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="Username" meta:resourcekey="UserNameResource1"></asp:TextBox>
                                                    <i class="icon-user"></i>
                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                        CssClass="error2" ErrorMessage="User Name is required." ToolTip="User Name is required." meta:resourcekey="UserNameRequiredResource1" Text="User Name is required."></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password"
                                                        placeholder="Password" MaxLength="128" meta:resourcekey="PasswordResource1"></asp:TextBox>
                                                    <i class="icon-lock"></i>
                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                        CssClass="error2" ErrorMessage="Password is required." ToolTip="Password is required." meta:resourcekey="PasswordRequiredResource1" Text="Password is required."></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <div class="clearfix">
                                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In"
                                                    CssClass="width-35 pull-right btn btn-sm btn-primary" meta:resourcekey="LoginButtonResource1" />
                                            </div>
                                        </LayoutTemplate>
                                    </asp:Login>
                                </div>
                                <!-- /widget-main -->
                                <div class="toolbar clearfix">
                                    <div>
                                        <a href="ForgotPassword.aspx" class="forgot-password-link label-sm"><i class="icon-arrow-left"></i>
                                            <asp:Localize ID="localForgotPassword" runat="server" Text="I forgot my password" meta:resourcekey="localForgotPasswordResource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div>
                                        <a href="Register.aspx" class="user-signup-link label-lg">
                                            <asp:Localize ID="localToRegister" runat="server" Text="I want to register" meta:resourcekey="localToRegisterResource1"></asp:Localize>
                                            <i class="icon-arrow-right"></i></a>
                                    </div>
                                </div>
                            </div>
                            <!-- /widget-body -->
                        </div>
                        <!-- /login-box -->
                    </div>
                    <!-- /position-relative -->
                </div>
            </div>
            <!-- /.col -->
        </div>
        <!-- / .login-layout-->
    </div>
    <!-- /.page-content -->
    <script type="text/javascript">
        window.jQuery || document.write("<script src='assets/js/jquery-2.0.3.min.js'>" + "<" + "/script>");
    </script>
    <!-- <![endif]-->
    <!--[if IE]>
<script type="text/javascript">
 window.jQuery || document.write("<script src='assets/js/jquery-1.10.2.min.js'>"+"<"+"/script>");
</script>
<![endif]-->
    <script type="text/javascript">
        if ("ontouchend" in document) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>
</asp:Content>
