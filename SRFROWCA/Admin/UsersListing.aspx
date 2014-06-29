<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="UsersListing.aspx.cs" Inherits="SRFROWCA.Admin.UsersListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker();
            $("#<%=txtToDate.ClientID%>").datepicker();
        });
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Users</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>
                                        <asp:Button ID="btnAddUser" runat="server" Text="Add New User" PostBackUrl="~/Account/Registerca.aspx"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    User Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>

                                                            <td>
                                                                <label>
                                                                    Organization:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrg" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>Registered From:</td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="width-40"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    User Email:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    User Type:
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRoles" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRoles_SelectedIndexChanged" CssClass="width-90">
                                                                    <asp:ListItem Text="All" Value="All" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="User" Value="User"></asp:ListItem>
                                                                    <asp:ListItem Text="Clsuter Lead" Value="ClusterLead"></asp:ListItem>
                                                                    <asp:ListItem Text="Regional Lead" Value="RegionalClusterLead"></asp:ListItem>
                                                                    <asp:ListItem Text="OCHA" Value="OCHA"></asp:ListItem>
                                                                    <asp:ListItem Text="OCHA Country Admin" Value="CountryAdmin"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>

                                                            <td>Registered To:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="width-40"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Country:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>Approved:
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rbIsApproved" runat="server" RepeatColumns="3">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Approved" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Not Approved" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td colspan="2">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>



                    </div>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                        CssClass="imagetable" PageSize="600" AllowSorting="true" Width="100%" OnPageIndexChanging="gvUsers_PageIndexChanging"
                        OnSorting="gvUsers_Sorting" OnRowCommand="gvUsers_RowCommand">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:TemplateField HeaderText="Approved" SortExpression="IsApproved" HeaderStyle-Width="50px"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsApproved" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsApproved_CheckedChanged"
                                        Checked='<%# Eval("IsApproved") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Locked" SortExpression="IsLockedOut" HeaderStyle-Width="50px"
                                ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsLocked" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsLocked_CheckedChanged"
                                        Checked='<%# Eval("IsLockedOut") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RoleName" HeaderText="Role" SortExpression="RoleName" />
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                                ItemStyle-Width="500px" />
                            <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="CreateDate" HeaderText="Create Date" SortExpression="DateCreated"
                                ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblUserId" runat="server" Visible="false" Text='<%#Eval("UserId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditUser" CommandArgument='<%# Eval("UserId") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
