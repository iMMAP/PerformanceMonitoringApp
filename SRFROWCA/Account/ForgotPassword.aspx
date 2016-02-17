<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs" Inherits="SRFROWCA.Account.ForgotPassword" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .login-container {
            margin: 0 auto;
            width: 450px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg"></div>
    <br />
    <div class="page-content">        
        <div class="login-layout">
            <div class="col-sm-10 col-sm-offset-1">
                <div class="login-container">
                    <div class="position-relative">
                        <div id="forgot-box" class="forgot-box visible widget-box no-border">
                            <div class="widget-body">
                                <div class="widget-main">
                                    <h4 class="header red lighter bigger">
                                        <i class="icon-key"></i>
                                        <asp:Localize ID="localRetrievePassword" runat="server" Text="Retrieve Password" meta:resourcekey="localRetrievePasswordResource1"></asp:Localize>
                                    </h4>
                                    <div class="space-6">
                                    </div>
                                    <p>
                                        <asp:Localize ID="localEnterEmailOrUserName" runat="server" Text="Enter your email OR User Name to receive instructions!" meta:resourcekey="localEnterEmailOrUserNameResource1"></asp:Localize>
                                    </p>
                                    <%--<label class="block clearfix">
                                                <span class="block input-icon input-icon-right">--%>
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="form-control" placeholder="Email" meta:resourcekey="txtEmailResource1" Visible="false"></asp:TextBox>
                                    <%--<i class="icon-envelope"></i></span>
                                            </label>
                                            OR--%>
                                    <label class="block clearfix">
                                        <span class="block input-icon input-icon-right">
                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="form-control" placeholder="Username" meta:resourcekey="txtUserNameResource1"></asp:TextBox>
                                            <i class="icon-user"></i></span>
                                    </label>
                                    <div class="clearfix">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Send Me!" CssClass="width-35 pull-right btn btn-sm btn-danger"
                                            OnClick="btnSubmit_Click" meta:resourcekey="btnSubmitResource1" />
                                    </div>
                                </div>
                                <!-- /widget-main -->
                                <div class="toolbar center">
                                    <a href="Login.aspx" class="back-to-login-link">
                                        <asp:Localize ID="localBakToLogin" runat="server" Text="Back to login" meta:resourcekey="localBakToLoginResource1"></asp:Localize>
                                        <i class="icon-arrow-right"></i></a>
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
