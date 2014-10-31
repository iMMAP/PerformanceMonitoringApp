﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectsListing"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" EnableEventValidation="false" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Projects" meta:resourcekey="localBreadCrumbProjectsResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table class="width-100">
            <tr>
                <td>
                
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF" class="width-10 btn btn-sm btn-yellow"
                                            title="PDF">
                                            <i class="icon-download"></i>PDF
                                       
                                        </button>
                                        <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-yellow"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                       
                                        </button>
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                        
                                                    <div class="col-sm-3">
                                                      
                                                                    <table class="width-100">
                                                                        <tr>
                                                                            <td width="150px">
                                                                                <asp:Localize ID="localCountry" Visible="false" runat="server" Text="Country:" meta:resourcekey="localCountryResource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList Visible="false" ID="ddlCountry" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" meta:resourcekey="ddlCountryResource1">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="150px">
                                                                                <asp:Localize ID="localAdmin1" runat="server" Text="Region:" meta:resourcekey="localAdmin1Resource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" meta:resourcekey="ddlAdmin1Resource1">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <%-- <tr>
                                                                            <td>
                                                                                <asp:Localize ID="Localize1" runat="server" Text="Cercle:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlAdmin2" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" meta:resourcekey="ddlAdmin2Resource1">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>--%>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Localize ID="Localize2" runat="server" Text="Reporting Status:" meta:resourcekey="Localize2Resource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBoxList ID="cblReportingStatus" runat="server" RepeatColumns="2" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="cblReportingStatus_SelectedIndexChanged" meta:resourcekey="cblReportingStatusResource1">
                                                                                    <asp:ListItem Text="Yes" Value="1" meta:resourcekey="ListItemResource1">
                                                                                    </asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0" meta:resourcekey="ListItemResource2">
                                                                                    </asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Localize ID="Localize3" runat="server" Text="Funding Status:" meta:resourcekey="Localize3Resource1"></asp:Localize>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBoxList ID="cblFundingStatus" runat="server" RepeatColumns="2" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged" meta:resourcekey="cblFundingStatusResource1">
                                                                                    <asp:ListItem Text="Yes" Value="1" meta:resourcekey="ListItemResource3">
                                                                                    </asp:ListItem>
                                                                                    <asp:ListItem Text="No" Value="0" meta:resourcekey="ListItemResource4">
                                                                                    </asp:ListItem>
                                                                                </asp:CheckBoxList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                            
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table>
                                                                        <tr>
                                                                            <td>Project Code:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                    OnTextChanged="txtProjectCode_TextChanged" meta:resourcekey="txtProjectCodeResource1"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <div id="divOrganization" runat="server">
                                                                                <td>Organization:
                                                                            </td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                        OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged" meta:resourcekey="ddlOrgResource1">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </div>
                                                                        </tr>
                                                                        <%-- <tr>
                                                                            <td>Project Status:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlProjStatus" runat="server" CssClass="width-90" AutoPostBack="True"
                                                                                    OnSelectedIndexChanged="ddlProjStatus_SelectedIndexChanged" meta:resourcekey="ddlProjStatusResource1">
                                                                                    <asp:ListItem Text="All" Value="0" Selected="True" meta:resourcekey="ListItemResource5"></asp:ListItem>
                                                                                    <asp:ListItem Text="On Going" Value="1" meta:resourcekey="ListItemResource6"></asp:ListItem>
                                                                                    <asp:ListItem Text="Completed" Value="2" meta:resourcekey="ListItemResource7"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>--%>
                                                                        <tr>
                                                                            <div id="divClusters" runat="server">
                                                                                <td>Cluster:</td>
                                                                                <td>
                                                                                    <asp:DropDownList ID="ddlClusters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"></asp:DropDownList>
                                                                                </td>
                                                                            </div>
                                                                        </tr>
                                                                    </table>
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
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass=" table-striped table-bordered table-hover"
                        AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnPageIndexChanging="gvProjects_PageIndexChanging"
                        meta:resourcekey="gvProjectsResource1">

                        <Columns>
                            <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode"
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource2">
                                <ItemStyle Wrap="True"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                                meta:resourcekey="BoundFieldResource3" />
                            <asp:BoundField DataField="CurrentRequest" HeaderText="Current Request" SortExpression="CurrentRequest"
                                meta:resourcekey="BoundFieldResource5" />
                            <asp:BoundField DataField="OriginalRequest" HeaderText="Original Request" SortExpression="OriginalRequest"
                                meta:resourcekey="BoundFieldResource10" />
                            <asp:BoundField DataField="Funded" HeaderText="Funded" SortExpression="Funded"
                                meta:resourcekey="BoundFieldResource6" />
                            <asp:BoundField DataField="LocationName" Visible="false" HeaderText="Reported Locations" meta:resourcekey="BoundFieldResource7" />
                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Wrap="true" ItemStyle-Width="150px"
                                SortExpression="Contact" meta:resourcekey="BoundFieldResource8">
                                <ItemStyle Wrap="True" Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" meta:resourcekey="BoundFieldResource9" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkVieDetails" runat="server" ImageUrl="../assets/orsimages/view.png" CommandName="ViewProject"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkVieDetailsResource1" />

                                    <%-- <asp:LinkButton ID="lnkVieDetails" runat="server" Text="View" CommandName="ViewProject"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkVieDetailsResource1" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                        CommandArgument='<%# Eval("ProjectId") %>' />


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
