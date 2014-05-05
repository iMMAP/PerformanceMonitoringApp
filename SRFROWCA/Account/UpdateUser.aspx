<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UpdateUser.aspx.cs" Inherits="SRFROWCA.Account.UpdateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbRegister" runat="server" Text="Add New User"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <label>
                                Role:</label>
                            <div>
                                <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-control width-70" AutoPostBack="true" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Select Your Role"
                                    CssClass="error2" InitialValue="0" Text="Select Your Role" ControlToValidate="ddlUserRole"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div>
                            <label>Full Name:</label>
                            <div>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="width-70"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Name Required"
                                    CssClass="error2" Text="Name Required" ControlToValidate="txtFullName"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div>
                            <label>
                                User Name:</label>
                            <div>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="width-70" Enabled="false"
                                    Style="background-color: ButtonFace;"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required"
                                    CssClass="error2" Text="Name Required" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="singalselect">
                            <label>
                                Email:
                            </label>
                            <div>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="width-70"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                    Text="Email Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="singalselect">
                            <label>
                                Phone:</label>
                            <div>
                                <asp:TextBox ID="txtPhone" MaxLength="50" CssClass="width-70" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ErrorMessage="Phone Required"
                                    CssClass="error2" Text="Phone Required" ControlToValidate="txtPhone"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="singalselect">
                            <label>
                                Organization:
                            </label>
                            <div>
                                <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="width-70">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div id="divCountry" runat="server" class="singalselect">
                            <label>
                                Country:</label>
                            <div>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-70">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Country"
                                    Text="*" InitialValue="0" ControlToValidate="ddlCountry" CssClass="error2"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div id="divCluster" runat="server">
                            <label>
                                Cluster:</label>
                            <div>
                                <asp:DropDownList ID="ddlClusters" runat="server" CssClass="form-control width-70">
                                </asp:DropDownList>
                                <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                    ErrorMessage="Select Your Cluster." meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>
                            </div>
                        </div>
                        <div class="space-10"></div>
                        <div>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                                CssClass="btn btn-primary" />
                            <asp:Button ID="btnBack" runat="server" Text="Back to Users List" OnClick="btnBack_Click"
                                CssClass="btn" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <label>Change Password:</label>
                            <div>
                                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Required" Text="Required" ForeColor="Red" ValidationGroup="Password"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                                    CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}" meta:resourcekey="valMinLengthResource1" ValidationGroup="Password"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="space-10"></div>
                        <div>
                            <asp:Button ID="btnPassword" runat="server" Text="Change Password" OnClick="btnChangePassword_Click"
                                CssClass="btn btn-primary" ValidationGroup="Password" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
