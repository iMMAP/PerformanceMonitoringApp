<%@ Page Title="ORS - Validate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ValidateReportList.aspx.cs" Inherits="SRFROWCA.ClusterLead.ValidateReportList" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
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
                <asp:Localize ID="localBreadCrumbValidateAchievements" runat="server" Text="Validate Achievements" meta:resourcekey="localBreadCrumbValidateAchievementsResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table class="width-100">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h5>
                                        <asp:Localize ID="localFilterReports" runat="server" Text="Filter Reports" meta:resourcekey="localFilterReportsResource1"></asp:Localize>
                                    </h5>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div class="col-sm-4">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table class="width-100">
                                                                        <tr>
                                                                            <td class="width-20">
                                                                                <asp:Localize ID="localProjectCode" runat="server" Text="Project Code:" meta:resourcekey="localProjectCodeResource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlProjects" runat="server" CssClass="width-100" 
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlProjectsResource1" >
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Localize ID="localMonths" runat="server" Text="Months:" meta:resourcekey="localMonthsResource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlMonths" runat="server" CssClass="width-100"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlMonthsResource1">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-8">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table class="width-100">
                                                                        <tr>
                                                                            <td class="width-20">
                                                                                <asp:Localize ID="localProjectTitle" runat="server" Text="Project Title:" meta:resourcekey="localProjectTitleResource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlProjectTitle" runat="server" CssClass="width-100"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlProjectTitleResource1">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Localize ID="localOrganization" runat="server" Text="Organization:" meta:resourcekey="localOrganizationResource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="width-100"
                                                                                AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlOrganizationsResource1">
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
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h5>
                            <asp:Localize ID="localReportsList" runat="server" Text="Reports List" meta:resourcekey="localReportsListResource1"></asp:Localize>
                        </h5>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                    DataKeyNames="ReportId" CssClass="imagetable" Width="100%" meta:resourcekey="gvActivitiesResource1"
                                    OnRowCommand="gvReports_RowCommand">
                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:BoundField DataField="ReportName" HeaderText="Report Name" SortExpression="ReportName" meta:resourcekey="BoundFieldResource1" />
                                        <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" meta:resourcekey="BoundFieldResource2" />
                                        <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource3" />
                                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" meta:resourcekey="BoundFieldResource4" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" meta:resourcekey="BoundFieldResource5" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Last Updated" SortExpression="CreatedDate" meta:resourcekey="BoundFieldResource6" />
                                        <asp:TemplateField HeaderText="View Details" meta:resourcekey="TemplateFieldResource1">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnView" runat="server" ImageUrl="~/assets/orsimages/view.png"
                                                    CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="ViewReport" meta:resourcekey="imgbtnViewResource1" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
