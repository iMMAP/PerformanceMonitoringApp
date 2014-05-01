<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Registerca.aspx.cs" Inherits="SRFROWCA.Account.Registerca" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbRegister" runat="server" Text="Add New User"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <table border="0" class="width-100">
                <tr>
                    <td>User Role:</td>
                    <td>
                        <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="width-60" AutoPostBack="true" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                            <asp:ListItem Text="Select Your Role" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Data Entry/Field Officer" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Country Cluster Lead" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Regional Cluster Lead" Value="3"></asp:ListItem>
                            <asp:ListItem Text="OCHA Staff" Value="4"></asp:ListItem>
                            <asp:ListItem Text="OCHA Country Admin" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Required" InitialValue="0"
                            Text="Required" ControlToValidate="ddlUserRole"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="200px">
                        <label>
                            Full Name:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFullName" runat="server" MaxLength="256" CssClass="width-60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Full Name Required"
                            CssClass="error2" Text="Required" ControlToValidate="txtFullName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            User Name:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="width-60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="User Name Required"
                            CssClass="error2" Text="Required" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Password:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"
                            CssClass="width-60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password Required"
                            CssClass="error2" Text="Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                            CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Confirm Password:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="128"
                            CssClass="width-60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Confirm Password Required"
                            CssClass="error2" Text="Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords don't match."
                            CssClass="error2" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Email:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="width-60"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                            Text="Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Phone:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="width-60"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <label>
                            <asp:Literal ID="ltrlLocation" runat="server" Text="Country:"></asp:Literal></label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLocations" runat="server" CssClass="width-60"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required" InitialValue="0"
                            Text="Required" ControlToValidate="ddlLocations"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <div id="divOrg" runat="server">
                        <td>Organization:</td>
                        <td>
                            <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="width-60" meta:resourcekey="ddlOrganizationResource1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%--<asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator2" ClientValidationFunction="ValidateOrganization"
                                    ErrorMessage="Select Your Organization." meta:resourcekey="CustomValidator2Resource1"></asp:CustomValidator>--%>
                        </td>
                    </div>
                </tr>
                <tr>
                    <div id="divCluster" runat="server">
                        <td>Cluster
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-60" meta:resourcekey="ddlClustersResource1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <%--<asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                    ErrorMessage="Select Your Cluster." meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>--%>
                        </td>
                    </div>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="width-60 btn btn-sm btn-success"
                            OnClick="btnRegister_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
