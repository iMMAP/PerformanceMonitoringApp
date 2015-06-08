<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="SRFROWCA.Account.Register" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

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

            if (roleVal === '4' || roleVal === '5' || roleVal === '6') {
                $('#divCluster').hide();
                $('#divOrganization').hide();
            }
        }

        $(function () {
            var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();

            showHideControlsOnUserRole(roleVal);

            $('#<%=ddlUserRole.ClientID%>').change(function () {
                var role = $('#<%=ddlUserRole.ClientID%> option:selected').val();
                showHideControlsOnUserRole(role);
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
    
    <div class="page-content">
        <div id="divMsg"></div>
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
                                                <i class="icon-group blue"></i>
                                                <asp:Localize ID="localRegisterNewUser" runat="server" Text="New User Registration" meta:resourcekey="localRegisterNewUserResource1"></asp:Localize>
                                            </h4>
                                            <label class="block clearfix">
                                                <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-control" meta:resourcekey="ddlUserRoleResource1">
                                                    <asp:ListItem Text="Select Your Role" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                    <asp:ListItem Text="Data Entry/Agency Officer" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                    <asp:ListItem Text="Country Cluster Lead" Value="2" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                    <asp:ListItem Text="Region Cluster Lead" Value="3" meta:resourcekey="ListItemResource4"></asp:ListItem>                                                    
                                                    <asp:ListItem Text="OCHA Country Admin" Value="5"></asp:ListItem>
                                                    <asp:ListItem Text="OCHA Country Staff" Value="6"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Select Your Role"
                                                    CssClass="error2" InitialValue="0" Text="Select Your Role" ControlToValidate="ddlUserRole" meta:resourcekey="rfvUserRoleResource1"></asp:RequiredFieldValidator>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtFullName" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="Full Name" meta:resourcekey="txtFullNameResource1"></asp:TextBox>
                                                    <i class="icon-user"></i>
                                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Name Required"
                                                        CssClass="error2" Text="Name Required" ControlToValidate="txtFullName" meta:resourcekey="rfvFullNameResource1"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="Username" meta:resourcekey="txtUserNameResource1"></asp:TextBox>
                                                    <i class="icon-user"></i>
                                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Username Required"
                                                        CssClass="error2" Text="Username Required" ControlToValidate="txtUserName" meta:resourcekey="rfvUserNameResource1"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"
                                                        CssClass="form-control" placeholder="Password" meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                                                    <i class="icon-lock"></i>
                                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password Required"
                                                        CssClass="error2" Text="Password Required" ControlToValidate="txtPassword" meta:resourcekey="rfvPasswordResource1"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                                                        CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}" meta:resourcekey="valMinLengthResource1"></asp:RegularExpressionValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="form-control"
                                                        placeholder="Email" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                                                    <i class="icon-envelope"></i>
                                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                                        Text="Email Required" CssClass="error2" ControlToValidate="txtEmail" meta:resourcekey="rfvEmailResource1"></asp:RequiredFieldValidator></span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="form-control"
                                                        placeholder="Phone" meta:resourcekey="txtPhoneResource1"></asp:TextBox>
                                                    <i class="icon-info-sign"></i></span>
                                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ErrorMessage="Phone Required"
                                                    CssClass="error2" Text="Phone Required" ControlToValidate="txtPhone" meta:resourcekey="rfvPhoneResource1"></asp:RequiredFieldValidator>
                                            </label>
                                            <div id="divOrganization">
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-control" meta:resourcekey="ddlOrganizationResource1">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator2" ClientValidationFunction="ValidateOrganization"
                                                            ErrorMessage="Select Your Organization." meta:resourcekey="CustomValidator2Resource1"></asp:CustomValidator>
                                                    <a href="../RequestOrganization.aspx" style="font-size:smaller;cursor:pointer;">Add organization if you dont see yours in the list.</a>
                                                    </span>
                                                </label>
                                            </div>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Select Your Country"
                                                        CssClass="error2" InitialValue="0" Text="Select Your Country" ControlToValidate="ddlCountry" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <div id="divCluster">
                                                <label class="block clearfix">
                                                    <span class="block input-icon input-icon-right">
                                                        <asp:DropDownList ID="ddlClusters" runat="server" CssClass="form-control" meta:resourcekey="ddlClustersResource1">
                                                        </asp:DropDownList>
                                                        <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                                            ErrorMessage="Select Your Cluster." meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>
                                                    </span>
                                                </label>
                                            </div>
                                            <div class="clearfix">
                                                

                                                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="width-65 pull-right btn btn-sm btn-success"
                                                    OnClick="btnRegister_Click" meta:resourcekey="btnRegisterResource1" />
                                            </div>
                                        </div>
                                        <div class="toolbar center">
                                            <a href="Login.aspx" class="back-to-login-link"><i class="icon-arrow-left"></i>
                                                <asp:Localize ID="localRegister" runat="server" Text="Back to login" meta:resourcekey="localRegisterResource1"></asp:Localize>
                                            </a>
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
