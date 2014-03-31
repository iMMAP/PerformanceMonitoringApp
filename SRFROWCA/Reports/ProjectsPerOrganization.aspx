<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsPerOrganization.aspx.cs" Inherits="SRFROWCA.Reports.ProjectsPerOrganization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Report</li>
            <li class="active">By Organizations</li>
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
                                        <%-- <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-primary"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>--%>
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="100px">
                                                                                Project Code:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlProjects" runat="server" CssClass="width-100" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Organization:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="width-100" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Funded
                                                                                <asp:CheckBoxList ID="cblFundingStatus" runat="server" RepeatColumns="2" AutoPostBack="true" OnSelectedIndexChanged="cblFundingStatus_SelectedIndexChanged">
                                                                                    <asp:ListItem Text="Yes" Value="1">
                                                                                    </asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0">
                                                                                    </asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                            <td>
                                                                                Reporting:
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
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table class="width-100">
                                                                        <tr>
                                                                            <td width="100px">
                                                                                Country:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-20" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Admin 1:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="width-20" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Admin 2:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlAdmin2" runat="server" CssClass="width-20" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged">
                                                                                </asp:DropDownList>
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
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="space">
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="grid"
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
    </div>
</asp:Content>
