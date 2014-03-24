<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="SRFROWCA.Account.Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function showHideControlsOnUserRole(roleVal) {
            if (roleVal === '1') {
                $('#divCluster').show();
                $('#divOrganization').show();
            }

            if (roleVal === '1') {
                $('#divCluster').hide();
                $('#divOrganization').show();
            }

            if (roleVal === '2' || roleVal === '3') {
                $('#divCluster').show();
                $('#divOrganization').hide();
            }

            if (roleVal === '4') {
                $('#divCluster').hide();
                $('#divOrganization').hide();
            }
        }

        $(function () {
            var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();

            showHideControlsOnUserRole(roleVal);

            $('#<%=ddlUserRole.ClientID%>').change(function () {
                var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();
                showHideControlsOnUserRole(roleVal);
            });
        });

        function ValidateOrganization(source, args) {
            var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();
            if (roleVal === '1') {
                var ddl = document.getElementById('<%= ddlOrganization.ClientID %>');
                if (ddl.options[ddl.selectedIndex].value === '0') {

                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }

        function ValidateClustersList(source, args) {
            var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();
            if (roleVal === '2' || roleVal === '3') {
                var ddl = document.getElementById('<%= ddlClusters.ClientID %>');
                if (ddl.options[ddl.selectedIndex].value === '0') {

                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <%--<asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMsg">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="text-align: center;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updActivities"
                    DynamicLayout="true">
                    <ProgressTemplate>
                        <img src="../assets/images/ajaxlodr.gif" alt="Loading">
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>--%>
    <div class="page-content">
        <div class="login-layout">
            <div class="main-container">
                <div class="main-content">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="position-relative">
                                <div id="signup-box" class="signup-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header green lighter bigger">
                                                <i class="icon-group blue"></i>New User Registration
                                            </h4>
                                            <label class="block clearfix">
                                                <asp:DropDownList ID="ddlUserRole" runat="server" Width="290px">
                                                    <asp:ListItem Text="Select Your Role" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Data Entry/Field Officer" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Country Cluster Lead" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Region Cluster Lead" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="OCHA Staff" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Select Your Role"
                                                    CssClass="error2" InitialValue="0" Text="Select Your Role" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="User Name"></asp:TextBox>
                                                    <i class="icon-user"></i>
                                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="UserName Required"
                                                        CssClass="error2" Text="User Name Required" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"
                                                        CssClass="form-control" placeholder="Password"></asp:TextBox>
                                                    <i class="icon-lock"></i>
                                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password Required"
                                                        CssClass="error2" Text="Password Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                                                        CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="Email"></asp:TextBox>
                                                    <i class="icon-envelope"></i>
                                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                                        Text="Email Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator></span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="form-control"
                                                        placeholder="Phone"></asp:TextBox>
                                                    <i class="icon-info-sign"></i></span>
                                            </label>
                                            <div id="divOrganization">
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:DropDownList ID="ddlOrganization" runat="server" Width="290px">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator2" ClientValidationFunction="ValidateOrganization"
                                                            ErrorMessage="Select Your Organization."></asp:CustomValidator>
                                                    </span>
                                                </label>
                                            </div>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" Width="290px">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Your Country"
                                                        CssClass="error2" InitialValue="0" Text="Select Your Country" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <div id="divCluster">
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:DropDownList ID="ddlClusters" runat="server" Width="290px">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                                            ErrorMessage="Select Your Cluster."></asp:CustomValidator>
                                                    </span>
                                                </label>
                                            </div>
                                            <div class="clearfix">
                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="width-30 pull-left btn btn-sm" />
                                                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="width-65 pull-right btn btn-sm btn-success"
                                                    OnClick="btnRegister_Click" />
                                            </div>
                                        </div>
                                        <div class="toolbar center">
                                            <a href="Login.aspx" class="back-to-login-link"><i class="icon-arrow-left"></i>Back
                                                to login </a>
                                        </div>
                                    </div>
                                    <!-- /widget-body -->
                                </div>
                                <!-- /signup-box -->
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
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
