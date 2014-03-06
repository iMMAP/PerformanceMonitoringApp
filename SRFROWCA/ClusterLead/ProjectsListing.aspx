<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectsListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .width
        {
            width: 130px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                <table>
                    <tr>
                        <td>
                            Proj Code:
                        </td>
                        <td>
                            <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width" AutoPostBack="true"
                                OnTextChanged="txtProjectCode_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Agency:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admin 1:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="width" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Admin 2:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAdmin2" runat="server" CssClass="width" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Proj Status:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProjStatus" runat="server" CssClass="width" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlProjStatus_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="On Going" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fund Status:
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblFundingStatus" runat="server" RepeatColumns="2">
                                <asp:ListItem Text="Yes" Value="0">
                                </asp:ListItem>
                                <asp:ListItem Text="No" Value="1">
                                </asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reporting:
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblReportingStatus" runat="server" RepeatColumns="2" AutoPostBack="true"
                                OnSelectedIndexChanged="cblReportingStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Yes" Value="1">
                                </asp:ListItem>
                                <asp:ListItem Text="No" Value="0">
                                </asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div class="tablegrid">
                <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                    AllowPaging="true" AllowSorting="true" PageSize="50" ShowHeaderWhenEmpty="true"
                    EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                    OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnPageIndexChanging="gvProjects_PageIndexChanging">
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" />
                        <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                            SortExpression="ProjectTitle" />
                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" />
                        <asp:BoundField DataField="OriginalRequest" HeaderText="Original Amount" SortExpression="OriginalRequest" />
                        <asp:BoundField DataField="CurrentRequest" HeaderText="Current Amount" SortExpression="CurrentRequest" />
                        <asp:BoundField DataField="LocationName" HeaderText="Locations" />
                        <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Wrap="true" ItemStyle-Width="150px"
                            SortExpression="Contact" />
                        <asp:BoundField DataField="Phone" HeaderText="Phone" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVieDetails" runat="server" Text="View" CommandName="ViewProject"
                                    CommandArgument='<%# Eval("ProjectId") %>' /></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
