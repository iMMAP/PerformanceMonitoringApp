<%@ Page Title="Log On" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="SRFROWCA.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="page-content">
        <div class="login-layout">
            <div class="main-container">
                <div class="main-content">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="center">
                            </div>
                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header blue lighter bigger">
                                                <i class="icon-coffee green"></i>Please Enter Your Information
                                            </h4>
                                            <div class="space-6">
                                            </div>
                                            <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false"
                                                OnLoggedIn="LoginUser_LoggedIn" OnLoginError="LoginUser_LoginError">
                                                <LayoutTemplate>
                                                    <label class="block clearfix">
                                                        <span class="block">
                                                            <asp:Label ID="FailureText" runat="server" CssClass="error2" EnableViewState="false"></asp:Label>
                                                        </span>
                                                    </label>
                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <asp:TextBox ID="UserName" runat="server" MaxLength="256" CssClass="form-control"
                                                                placeholder="Username"></asp:TextBox>
                                                            <i class="icon-user"></i>
                                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                                CssClass="error2" ErrorMessage="User Name is required." ToolTip="User Name is required.">User Name is required.</asp:RequiredFieldValidator>
                                                        </span>
                                                    </label>
                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password"
                                                                placeholder="Password" MaxLength="128" Width="300px"></asp:TextBox>
                                                            <i class="icon-lock"></i>
                                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                                CssClass="error2" ErrorMessage="Password is required." ToolTip="Password is required.">Password is required.</asp:RequiredFieldValidator>
                                                        </span>
                                                    </label>
                                                    <div class="clearfix">
                                                        <label class="inline">
                                                            <input id="RememberMe" runat="server" type="checkbox" class="ace" />
                                                            <span class="lbl">Remember Me</span>
                                                        </label>
                                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" CausesValidation="true"
                                                            CssClass="width-35 pull-right btn btn-sm btn-primary" />
                                                    </div>
                                                    <div class="space-4">
                                                    </div>
                                                </LayoutTemplate>
                                            </asp:Login>
                                        </div>
                                        <!-- /widget-main -->
                                        <div class="toolbar clearfix">
                                            <div>
                                                <a href="ForgotPassword.aspx" class="forgot-password-link"><i class="icon-arrow-left">
                                                </i>I forgot my password </a>
                                            </div>
                                            <div>
                                                <a href="Register.aspx" class="user-signup-link">I want to register <i class="icon-arrow-right">
                                                </i></a>
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
                <!-- /.main Content -->
            </div>
            <!-- / .main-container-->
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
