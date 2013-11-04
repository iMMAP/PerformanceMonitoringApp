<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="UsersListing.aspx.cs" Inherits="SRFROWCA.Admin.UsersListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:Button ID="btnExportExcel" runat="server" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </td>
            <td>
                <asp:Button ID="btnAddUser" runat="server" Text="Add New User" PostBackUrl="~/Account/Register.aspx" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                    PageSize="20" AllowSorting="true" Width="100%" OnPageIndexChanging="gvUsers_PageIndexChanging"
                    OnSorting="gvUsers_Sorting" OnRowCommand="gvUsers_RowCommand">
                    <HeaderStyle BackColor="ButtonFace" />
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
                        <asp:TemplateField HeaderText="Country Admin" SortExpression="IsCountryAdmin" HeaderStyle-Width="100px"
                            ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsCountryAdmin" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsCountryAdmin_CheckedChanged"
                                    Checked='<%# Eval("IsCountryAdmin") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                            ItemStyle-Width="500px" />
                        <asp:BoundField DataField="OrganizationAcronym" HeaderText="Acronym" SortExpression="OrganizationAcronym"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName"
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField>
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
