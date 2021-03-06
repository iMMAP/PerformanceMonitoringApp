﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsListing.aspx.cs" Inherits="SRFROWCA.OrsProject.ProjectsListing"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" EnableEventValidation="false" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function popitup(url) {
            //newwindow = window.open(url, 'name', 'height=700,width=1000', scro);
            var width = 700;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height +
                ",status,resizable,left=" + left + ",top=" + top +
                "screenX=" + left + ",screenY=" + top + ",scrollbars=yes";

            newwindow = window.open(url, 'name', windowFeatures);
            if (window.focus) { newwindow.focus() }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg"></div>
        <table class="width-100">
            <tr>
                <td>    
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>
                            <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF" class="btn btn-yellow btn-sm"
                                title="Export All Projects (PDF)">
                                <i class="icon-download"></i>PDF
                                       
                            </button>
                            <asp:Button ID="btnCreateProject" runat="server" Visible="true"
                                Text="Create ORS Project" CausesValidation="False" PostBackUrl="~/OrsProject/CreateProject.aspx"
                                CssClass="btn btn-yellow pull-right btn-sm" />
                        </h6>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionCountry" runat="server" Text="Country:"
                                                            meta:resourcekey="lblCaptionCountryResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td class="width-20">
                                                    <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" AutoPostBack="true"
                                                        OnSelectedIndexChanged="SelectedIndexChanged" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionCluster" runat="server" Text="Cluster:"
                                                            meta:resourcekey="lblCaptionClusterResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td class="width-20">
                                                    <asp:DropDownList ID="ddlSecClusters" runat="server" CssClass="width-80" AutoPostBack="true"
                                                        OnSelectedIndexChanged="SelectedIndexChanged" meta:resourcekey="ddlSecClustersResource1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionSubSetCluster" runat="server"
                                                            Text="Subset Of Cluster:" OnSelectedIndexChanged="SelectedIndexChanged"
                                                            meta:resourcekey="lblCaptionSubSetClusterResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td class="width-20">
                                                    <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-80" AutoPostBack="true"
                                                        OnSelectedIndexChanged="SelectedIndexChanged" meta:resourcekey="ddlClustersResource1">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionOrganization" runat="server"
                                                            Text="Organization:" meta:resourcekey="lblCaptionOrganizationResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width-80" AutoPostBack="true"
                                                        OnSelectedIndexChanged="SelectedIndexChanged" meta:resourcekey="ddlOrgResource1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionProjCode" runat="server"
                                                            Text="Project Code/Id:"
                                                            meta:resourcekey="lblCaptionProjCodeResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width-80"
                                                        meta:resourcekey="txtProjectCodeResource1"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Label ID="lblCaptionStatus" runat="server"
                                                            Text="Status:" meta:resourcekey="lblCaptionStatusResource1">
                                                        </asp:Label>
                                                    </label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="width-80"
                                                        meta:resourcekey="ddlStatusResource1"
                                                        OnSelectedIndexChanged="SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="Approved by Cluster/Sector " Value="Approved by Cluster/Sector "></asp:ListItem>
                                                        <asp:ListItem Text="CAP Final Review Phase" Value="CAP Final Review Phase"></asp:ListItem>
                                                        <asp:ListItem Text="Draft" Value="Draft"></asp:ListItem>
                                                        <asp:ListItem Text="HQ Review Phase" Value="HQ Review Phase"></asp:ListItem>
                                                        <asp:ListItem Text="Published by CAP" Value="Published by CAP"></asp:ListItem>
                                                        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="cbIsOPS" runat="server"
                                                            Text="HRP Projects" Checked="true"
                                                            OnCheckedChanged="SelectedIndexChanged"
                                                            AutoPostBack="true" ToolTip="Oly Show HRP Projects" />
                                                        <asp:CheckBox ID="cbIsORS" runat="server"
                                                            Text="ORS Projects" Checked="false"
                                                            OnCheckedChanged="SelectedIndexChanged"
                                                            AutoPostBack="true" ToolTip="Only Show ORS Projects" />
                                                    </td>
                                                    <td></td>
                                                    <td>
                                                        <asp:CheckBox ID="cbFuned" runat="server"
                                                            Text="Funded" Checked="false"
                                                            OnCheckedChanged="SelectedIndexChanged"
                                                            AutoPostBack="true" ToolTip="Only Show Funded Projects" />
                                                        <asp:CheckBox ID="cbNotFunded" runat="server"
                                                            Text="Not Funded" Checked="false"
                                                            OnCheckedChanged="SelectedIndexChanged"
                                                            AutoPostBack="true" ToolTip="Only Show Not Funded Projects" />
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:Label ID="lblCaptionYear" runat="server"
                                                                Text="Year:">
                                                            </asp:Label>
                                                        </label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True"
                                                            OnSelectedIndexChanged="SelectedIndexChanged" meta:resourcekey="ddlFrameworkYearResource1" Width="70px">
                                                            <asp:ListItem Text="2017" Value="13"></asp:ListItem>
                                                            <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                                            <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:CheckBox ID="cbLCB" runat="server" style="margin-left:20px;"
                                                            Text="LCB" Checked="false"
                                                            OnCheckedChanged="SelectedIndexChanged"
                                                            AutoPostBack="true" ToolTip="Filter Lake Chad Basin Projects" />
                                                        <asp:Button ID="btnSearch" runat="server" class="hidden" Width="1px" OnClick="btnSearch_Click" />
                                                    </td>
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
        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-green">
                <div class="infobox-icon">
                    <i class="icon-file"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblProjects" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Projects</div>
                </div>
            </div>

            <div class="infobox infobox-blue">
                <div class="infobox-icon">
                    <i class="icon-building"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblOrgs" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Organizations</div>
                </div>
            </div>

            <div class="infobox infobox-pink">
                <div class="infobox-icon">
                    <i class="icon-map-marker"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblCountry" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Countries</div>
                </div>
            </div>

            <div class="infobox infobox-red">
                <div class="infobox-icon">
                    <i class="icon-list"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblClusters" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Clusters</div>
                </div>
            </div>

            <div class="infobox infobox-green">
                <div class="infobox-icon">
                    <i class="icon-usd"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblRequiremens" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Requested</div>
                </div>
            </div>

            <div class="infobox infobox-orange">
                <div class="infobox-icon">
                    <i class="icon-usd"></i>
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">
                        <asp:Label ID="lblFunded" runat="server" Text=""></asp:Label></span>
                    <div class="infobox-content">Funded</div>
                </div>

                <div class="badge badge-success">
                    <asp:Label ID="lblPercentFunded" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        AllowPaging="True" AllowSorting="True" PageSize="20" ShowHeaderWhenEmpty="True" AllowCustomPaging="true"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnRowDataBound="gvProjects_RowDataBound"
                        OnPageIndexChanging="gvProjects_PageIndexChanging"
                        DataKeyNames="ProjectId,ProjectOrganizationId,OrganizationId,IsOPS,EmergencyLocationId,EmergencyClusterId,UserOrgId,IsPartner,EmergencySecClusterId"
                        meta:resourcekey="gvProjectsResource1">
                        <PagerSettings Mode="NumericFirstLast" />
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
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
                            <asp:TemplateField HeaderText="Project Title" SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource2">
                                <ItemTemplate>
                                    <asp:Label ID="lblProjectTitle" runat="server"
                                        Text='<%# Eval("ProjectShortTitle") %>' ToolTip='<%# Eval("ProjectTitle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Organization" SortExpression="OrganizationAcronym" meta:resourcekey="BoundFieldResource3"
                                ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrganization" runat="server"
                                        Text='<%# Eval("OrganizationAcronym") %>' ToolTip='<%# Eval("OrganizationName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" meta:resourcekey="BoundFieldResource4" />
                            <asp:BoundField DataField="SecCluster" HeaderText="Sub-Set Cluster" SortExpression="SecCluster" meta:resourcekey="BoundFieldResource5" />

                            <asp:TemplateField HeaderText="Original Request" SortExpression="OriginalRequest" ItemStyle-HorizontalAlign="Right"
                                meta:resourcekey="BoundFieldResource10">
                                <ItemTemplate>
                                    <asp:Label ID="lblOriginalRequest" runat="server" Text=' <%# Eval("OriginalRequest")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Funded ($)" SortExpression="FundedAmount" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblFundedAmount" runat="server" Text=' <%# Eval("FundedAmount")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% Funded" ItemStyle-HorizontalAlign="Right" SortExpression="PercentageFunded">
                                <ItemTemplate>
                                    <asp:Label ID="lblPercentageFunded" runat="server" Text='<%# Eval("PercentageFunded")  + "%" %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="200px" HeaderText="Contact" SortExpression="ProjectContactName" meta:resourcekey="BoundFieldResource8">
                                <ItemTemplate>
                                    <asp:Label ID="lblContact" runat="server"
                                        Text='<%# Eval("Contact") %>' ToolTip='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="Phone" HeaderText="Phone" meta:resourcekey="BoundFieldResource9" />--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF" meta:resourcekey="TemplateFieldResource2">
                                <HeaderTemplate>
                                    <asp:Label ID="lblPDFExportHeader" runat="server" Text="PDF" ToolTip="Export Project (PDF)"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkPrintResource1" ToolTip="Export" />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="lblReportHeader" runat="server" Text="RPT" ToolTip="Project Data Entry"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnReport" runat="server" ImageUrl="../assets/orsimages/report1.png" ToolTip="Project Data Entry"
                                        OnClientClick=<%# string.Format("return popitup('ProjectDataEntry.aspx?pid={0}&orgid={1}')", Eval("ProjectId"), Eval("OrganizationId")) %> />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="lblTargetHeader" runat="server" Text="TGT" ToolTip="Project Targets"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnTargets" runat="server" ImageUrl="../assets/orsimages/target1.png" ToolTip="Project Targets"
                                        OnClientClick=<%# string.Format("return popitup('ProjectTargets.aspx?pid={0}')", Eval("ProjectId")) %> />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <asp:Label ID="lblPartnerHeader" runat="server" Text="PRT" ToolTip="Project Partners"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgbtnPartners" runat="server" ImageUrl="../assets/orsimages/partners2.png"
                                        ToolTip="Project Partners" PostBackUrl='<%# string.Format("ProjectPartners.aspx?pid={0}", Eval("ProjectId")) %>' />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                        CommandName="EditProject" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Edit Project Information" />
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                        CommandName="DeleteProject" CommandArgument='<%# Eval("ProjectId") %>' ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
