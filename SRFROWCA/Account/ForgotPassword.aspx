<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="SRFROWCA.Account.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="login-layout">
            <div class="main-container">
                <div class="main-content">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="position-relative">
                                <div id="forgot-box" class="forgot-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header red lighter bigger">
                                                <i class="icon-key"></i>Retrieve Password
                                            </h4>
                                            <div class="space-6">
                                            </div>
                                            <p>
                                                Enter your email OR User Name to receive instructions!
                                            </p>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" Width="300px" placeholder="Email"></asp:TextBox>
                                                    <i class="icon-envelope"></i></span>
                                            </label>
                                            OR
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" Width="300px" placeholder="Username"></asp:TextBox>
                                                    <i class="icon-user"></i></span>
                                            </label>
                                            <div class="clearfix">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Send Me!" CssClass="width-35 pull-right btn btn-sm btn-danger"
                                                    OnClick="btnSubmit_Click" />
                                            </div>
                                        </div>
                                        <!-- /widget-main -->
                                        <div class="toolbar center">
                                            <a href="Login.aspx" class="back-to-login-link">Back to login <i class="icon-arrow-right">
                                            </i></a>
                                        </div>
                                    </div>
                                    <!-- /widget-body -->
                                </div>
                                <!-- /forgot-box -->
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
