<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UsersListing.aspx.cs" Inherits="SRFROWCA.Admin.Users.UsersListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:Button ID="btnExportExcel" runat="server" Text="Export To Excel" OnClick="btnExportExcel_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20"
                    AllowSorting="true" Width="100%" OnPageIndexChanging="gvUsers_PageIndexChanging" OnSorting="gvUsers_Sorting">
                    <HeaderStyle BackColor="ButtonFace" />
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                        <asp:BoundField DataField="password" HeaderText="Password" />
                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" />
                        <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName" />
                        <asp:TemplateField HeaderText="Approved" SortExpression="IsApproved">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIsApproved" AutoPostBack="true" runat="server" OnCheckedChanged="chkIsApproved_CheckedChanged"
                                    Checked='<%# Eval("IsApproved") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
