﻿<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="ProjectsListingPublic.aspx.cs" Inherits="SRFROWCA.Anonymous.ProjectsListingPublic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHome"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Projects" meta:resourcekey="localBreadCrumbProjects"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg"></div>
        <table class="width-100">
            <tr>
                <td>
                    <div class="widget-header widget-header-small header-color-blue2">
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td class="width-20">
                                                    <label>
                                                        <asp:Label ID="lblCaptionCluster" runat="server" Text="Cluster:" meta:resourcekey="lblCaptionClusterResource1"></asp:Label></label>
                                                </td>
                                                <td class="width-30">
                                                    <asp:DropDownList ID="ddlSecClusters" runat="server" CssClass="width-80" meta:resourcekey="ddlSecClustersResource1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="width-20">
                                                    <label>
                                                        <asp:Label ID="lblCaptionSubSetCluster" runat="server" Text="Subset Of Cluster:" meta:resourcekey="lblCaptionSubSetClusterResource1"></asp:Label></label>
                                                </td>
                                                <td class="width-30">
                                                    <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-80" meta:resourcekey="ddlClustersResource1">
                                                    </asp:DropDownList>
                                                </td>



                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionCountry" runat="server" Text="Country:" meta:resourcekey="lblCaptionCountryResource1"></asp:Label></label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList></td>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionOrganization" runat="server" Text="Organization:" meta:resourcekey="lblCaptionOrganizationResource1"></asp:Label></label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width-80"
                                                        meta:resourcekey="ddlOrgResource1">
                                                    </asp:DropDownList>
                                                </td>


                                                <tr>
                                                    <td>
                                                        <label>
                                                            <asp:Label ID="lblCaptionProjCode" runat="server" Text="Project Code/Id:" meta:resourcekey="lblCaptionProjCodeResource1"></asp:Label></label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width-80"
                                                            meta:resourcekey="txtProjectCodeResource1"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="cbFuned" runat="server" Text="Funded Projects" Checked="false" />
                                                        <asp:CheckBox ID="cbNotFunded" runat="server" Text="Not Funded Projects" Checked="false" />
                                                    </td>
                                                </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="cbIsOPS" runat="server" Text="SRP Projects" Checked="false" />
                                                    <asp:CheckBox ID="cbIsORS" runat="server" Text="ORS Projects" Checked="false" />
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td colspan="4" style="padding-top: 10px;">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnResetResource1" /></td>
                                            </tr>
                                        </table>
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
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnRowDataBound="gvProjects_RowDataBound"
                        OnPageIndexChanging="gvProjects_PageIndexChanging"
                        DataKeyNames="ProjectId,ProjectOrganizationId,OrganizationId"
                        meta:resourcekey="gvProjectsResource1">

                        <Columns>
                            <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>

                                <ItemStyle Width="2%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode"
                                meta:resourcekey="BoundFieldResource1">
                                <ItemStyle Width="120px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource2">
                                <ItemStyle Wrap="True"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                                meta:resourcekey="BoundFieldResource3" ItemStyle-Width="120px"></asp:BoundField>
                            <asp:BoundField DataField="ClusterName" HeaderText="Subset Cluster" SortExpression="ClusterName" meta:resourcekey="BoundFieldResource4" />
                            <asp:BoundField DataField="SecCluster" HeaderText="Cluster" SortExpression="SecCluster" meta:resourcekey="BoundFieldResource5" />

                            <asp:TemplateField HeaderText="Original Request" SortExpression="OriginalRequest" meta:resourcekey="BoundFieldResource10">
                                <ItemTemplate>
                                    <asp:Label ID="lblOriginalRequest" runat="server" Text=' <%# Eval("OriginalRequest")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Funded Amount" SortExpression="FundedAmount">
                                <ItemTemplate>
                                    <asp:Label ID="lblFundedAmount" runat="server" Text=' <%# Eval("FundedAmount")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="OPSProjectStatus" HeaderText="Status" SortExpression="OPSProjectStatus" meta:resourcekey="BoundFieldResource6" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkPrintResource1" />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>