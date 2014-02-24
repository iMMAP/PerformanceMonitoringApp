<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="UsersListing.aspx.cs" Inherits="SRFROWCA.Admin.UsersListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .ddlWidthpx
        {
            width: 250px;
        }
    </style>
    <script>
        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker();
            $("#<%=txtToDate.ClientID%>").datepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="graybar">
            Filter Users
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table width="100%">
                    <tr>
                        <td>
                            <label>
                                User Name:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="ddlWidthpx"></asp:TextBox>
                        </td>
                        <td>
                            <label>
                                User Email:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="ddlWidthpx"></asp:TextBox>
                        </td>
                        <td>
                            <label>
                                Organization:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrg" runat="server" Width="365px"></asp:TextBox>
                        </td>
                        <td>
                            <label>
                                Country:</label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="ddlWidthpx"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                User Type:
                            </label>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbUserTypes" runat="server" RepeatColumns="4">
                                <asp:ListItem Text="All" Value = "0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="User" Value = "1"></asp:ListItem>
                                <asp:ListItem Text="Clsuter Lead" Value = "2"></asp:ListItem>
                                <asp:ListItem Text="Country Admin" Value = "3"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            Approved:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbIsApproved" runat="server" RepeatColumns="3">
                                <asp:ListItem Text="All" Value = "-1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Approved" Value = "1"></asp:ListItem>
                                <asp:ListItem Text="Not Approved" Value = "0"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            Created
                        </td>
                        <td>
                            From:
                            <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                            TO:
                            <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnExportExcel" runat="server" Text="Export To Excel" OnClick="btnExportExcel_Click"
                CssClass="button_example" />
        </div>
        <div class="buttonright">
            <asp:Button ID="btnAddUser" runat="server" Text="Add New User" PostBackUrl="~/Account/Registerca.aspx"
                CssClass="button_example" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table width="100%">
        <tr>
            <td>
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                    CssClass="imagetable" PageSize="20" AllowSorting="true" Width="100%" OnPageIndexChanging="gvUsers_PageIndexChanging"
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
                        <%-- <asp:TemplateField HeaderText="Country Admin" SortExpression="IsCountryAdmin" HeaderStyle-Width="100px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsCountryAdmin" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsCountryAdmin_CheckedChanged"
                                    Checked='<%# Eval("IsCountryAdmin") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="UserRole" HeaderText="Role" SortExpression="UserRole" />
                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                            ItemStyle-Width="500px" />
                        <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName"
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
</asp:Content>
