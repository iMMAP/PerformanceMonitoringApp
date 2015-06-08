<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Registerca.aspx.cs" Inherits="SRFROWCA.Account.Registerca" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">  

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

        function ValidateCountryList(source, args) {
            var roleVal = $('#<%=ddlUserRole.ClientID%> option:selected').val();
            if (roleVal !== '3') {
                var ddl = document.getElementById('<%= ddlLocations.ClientID %>');
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
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <table border="0">
                <tr>
                    <td>User Role:</td>
                    <td>
                        <asp:DropDownList ID="ddlUserRole" runat="server" Width="400px" AutoPostBack="true" OnSelectedIndexChanged="ddlUserRole_SelectedIndexChanged">
                            <asp:ListItem Text="Select Your Role" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Data Entry/Field Officer" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Country Cluster Lead" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Regional Cluster Lead" Value="3"></asp:ListItem>
                            <asp:ListItem Text="OCHA Staff" Value="4"></asp:ListItem>
                            <asp:ListItem Text="OCHA Country Admin" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="User Role Required" InitialValue="0" CssClass="error2"
                            Text="User Role Required" ControlToValidate="ddlUserRole"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Full Name:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFullName" runat="server" MaxLength="256" CssClass="width-100"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Full Name Required"
                            CssClass="error2" Text="Full Name Required" ControlToValidate="txtFullName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            User Name:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="width-100"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="User Name Required"
                            CssClass="error2" Text="User Name Required" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Password:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"
                            CssClass="width-100"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password Required"
                            CssClass="error2" Text="Password Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                            CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Email:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="width-100"></asp:TextBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                            Text="Email Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Phone:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="width-100"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <div id="divLocations" runat="server">
                    <td>
                       Country: 
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLocations" runat="server" CssClass="width-100"></asp:DropDownList>
                    </td>
                    <td>
                        <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator3" ClientValidationFunction="ValidateCountryList"
                                    ErrorMessage="Country Required" meta:resourcekey="CustomValidator2Resource1"></asp:CustomValidator>

                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Country Required" InitialValue="0" CssClass="error2"
                            Text="Country Required" ControlToValidate="ddlLocations"></asp:RequiredFieldValidator>--%>
                    </td>
                    </div>
                </tr>
                <tr>
                    <div id="divOrg" runat="server">
                        <td>Organization:</td>
                        <td>
                            <asp:DropDownList ID="ddlOrganization" runat="server" Width="400px" meta:resourcekey="ddlOrganizationResource1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator2" ClientValidationFunction="ValidateOrganization"
                                    ErrorMessage="Organization Required" meta:resourcekey="CustomValidator2Resource1"></asp:CustomValidator>
                        </td>
                    </div>
                </tr>
                <tr>
                    <div id="divCluster" runat="server">
                        <td>Cluster
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-100" meta:resourcekey="ddlClustersResource1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                    ErrorMessage="Cluster Required" meta:resourcekey="CustomValidator1Resource1"></asp:CustomValidator>
                        </td>
                    </div>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="width-30 btn btn-success"
                            OnClick="btnRegister_Click" />
                        <asp:Button ID="btnBack" runat="server" Text="Back to Users List" PostBackUrl="~/Admin/UsersListing.aspx" CausesValidation="false"
                                CssClass="btn" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
