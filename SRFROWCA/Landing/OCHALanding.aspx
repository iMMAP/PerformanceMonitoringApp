<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OCHALanding.aspx.cs" Inherits="SRFROWCA.Landing.OCHALanding" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>ORS - Cluster Coordinator Landing</title>
    <style>
        .tooltip {
            position:relative;
            font-size:13px;
        }
        .elemopacity {
            opacity: 100;
            width: 100%;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div class="space-3"></div>

                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="widget-box transparent">
                            <div class="widget-header widget-header-small">
                                <h4 class="widget-title smaller">
                                    <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                    <asp:Localize ID="localQuickMenue" runat="server" Text="ORS - Sector Coordination Quick Access Menu"></asp:Localize>
                                </h4>
                            </div>
                            <h5>
                                <asp:Localize ID="localWelcomeMessage" runat="server"
                                    Text="Welcome back to ORS. You logged in as a Cluster/Sector coordinator. Please tell us what you would like to do!"></asp:Localize>
                            </h5>
                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right">
                                                    <span class="label label-lg label-warning">
                                                        <i class="ace-icon fa fa-exclamation-triangle bigger-120"></i>
                                                        <asp:Localize ID="localToWork2016" runat="server"
                                                            Text="To Work On Your 2016 Sector Framework?"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="localCanDo2016" runat="server" Text="You Can Do ..."></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li> 
                                                        <asp:LinkButton ID="btnlnkValidateReport16" runat="server" Text="Validate 2016 Partners Report" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/ValidateReportList.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkClusterIndicators16" runat="server" Text="Go to my 2016 Output Indicators Page"
                                                             PostBackUrl="../ClusterLead/CountryIndicators.aspx?year=2016"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../ClusterLead/CountryIndicators.aspx">
                                                            <asp:Localize ID="localMenu2016OutputInd" runat="server" Text="Go to my 2016 Output Indicators Page" meta:resourcekey="localMenu2016OutputIndResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkSectorIndicators16" runat="server" Text="GO to my 2016 Sector Response Plan" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/IndicatorListing.aspx?year=2016"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>                                                  <a href="../ClusterLead/IndicatorListing.aspx">
                                                            <asp:Localize ID="localMenu2016SectorInd" runat="server" Text="Go to my 2016 Sector Response Plan" meta:resourcekey="localMenu2016SectorIndResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkOutputDataEntry16" runat="server" Text="Sector Outputput Indicactors 2016 Import Data" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/ClusterDataEntry16.aspx" meta:resourcekey="btnlnkOutputDataEntry16Resource"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProjects16" runat="server" Text="My Sector Projects 2016" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../OrsProject/ProjectsListing.aspx?year=2016" meta:resourcekey="btnlnkProjects16Resource"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnk3W16" runat="server" Text="3W of 2016 Activities" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/ProjecsList.aspx" meta:resourcekey="btnlnk3W16Resource"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProgress16" runat="server" Text="Progress Summary 2016" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../Reports/Summary/ProgressSummary.aspx" meta:resourcekey="btnlnkProgress2016Resource"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../ClusterLead/IndicatorListingMigrate.aspx">
                                                            <asp:Localize ID="localMenu2016Migrate" runat="server" Text="Migrate 2015 activites & indicators To 2016 Framework" meta:resourcekey="localMenu2016MigrateResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                     <li>
                                                        <asp:LinkButton ID="lbtnImportData" runat="server" Text="Import Data 2016" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/ImportData16.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkExportFramework" runat="server" Text="Export Framework 2016" CssClass="tooltip elemopacity"
                                                             PostBackUrl="Anonymous/ExpClusterFramework.aspx" meta:resourcekey="localMenu2016ExpFrameworkResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Anonymous/ExpClusterFramework.aspx">
                                                            <asp:Localize ID="localMenu2016ExpFramewrk" runat="server" Text="Export Framework 2016" meta:resourcekey="localMenu2016ExpFrameworkResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                    <li class="hidden">
                                                        <a href="../Reports/IndicatorTargetByProjectStatus.aspx">
                                                            <asp:Localize ID="localCCReport" runat="server" Text="Cluster FrameWork Report"></asp:Localize>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->

                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right">
                                                    <span class="label label-lg label-warning">
                                                        <i class="ace-icon fa fa-exclamation-triangle bigger-120"></i>
                                                        <asp:Localize ID="localToWork2015" runat="server"
                                                            Text="To Work On Your 2015 Sector Framework?" meta:resourcekey="localToWork2015Resource1"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>

                                            <div>
                                                <hr />
                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="localCanDo2015" runat="server" Text="You Can Do ..." meta:resourcekey="localCanDo2015Resource1"></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkValidateReport15" runat="server" Text="Validate 2015 Partners Report" CssClass=""
                                                             PostBackUrl="../ClusterLead/ValidateReportList.aspx" meta:resourcekey="localMenu2015ValidateResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../ClusterLead/ValidateReportList.aspx">
                                                            <asp:Localize ID="localMenu2015Validate" runat="server" Text="Validate 2015 Partners Report" meta:resourcekey="localMenu2015ValidateResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkClusterIndicators15" runat="server" Text="Go to my 2015 Output Indicators Page"
                                                             PostBackUrl="../ClusterLead/CountryIndicators.aspx?year=2015" meta:resourcekey="localMenu2015OutputIndResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../ClusterLead/CountryIndicators.aspx">
                                                            <asp:Localize ID="localMenu2016OutputInd" runat="server" Text="Go to my 2016 Output Indicators Page" meta:resourcekey="localMenu2016OutputIndResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkSectorIndicators15" runat="server" Text="GO to my 2015 Sector Response Plan" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/IndicatorListing.aspx?year=2015" meta:resourcekey="localMenu2015SectorIndResource1"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkOutputDataEntry15" runat="server" Text="Sector Outputput Indicactors 2015 Data Entry" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/ClusterDataEntry.aspx" meta:resourcekey="localMenu2015OutIndDataEntryResource1"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProjects15" runat="server" Text="My Sector Projects 2015" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../OrsProject/ProjectsListing.aspx?year=2015" meta:resourcekey="localMenu2015ProjectsResource1"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnk3W15" runat="server" Text="3W of 2015 Activities" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../OrsProject/ProjectsListing.aspx?year=2015" meta:resourcekey="localMenu20153WResource1"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProgress15" runat="server" Text="Progress Summary 2015" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../Reports/Summary/ProgressSummary.aspx" meta:resourcekey="btnlnkProgress2015Resource"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnImportData15" runat="server" Text="Import Data 2015" CssClass="tooltip elemopacity"
                                                             PostBackUrl="../ClusterLead/UploadAchieved.aspx"></asp:LinkButton>
                                                    </li>

                                                    <%--<li>
                                                        <a href="../ClusterLead/ClusterDataEntry.aspx">
                                                            <asp:Localize ID="localMenu2015OutIndDataEntry" runat="server" Text="Provide information for my 2015 sector output indicators" meta:resourcekey="localMenu2015OutIndDataEntryResource1"></asp:Localize>
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <a href="../ClusterLead/ProjectsListing.aspx">
                                                            <asp:Localize ID="localMenu2015Projects" runat="server" Text="See the list of 2015 project for my sector" meta:resourcekey="localMenu2015ProjectsResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Reports/ORS3W.aspx">
                                                            <asp:Localize ID="localMenu20153W" runat="server" Text="Access to my 3W data for 2015" meta:resourcekey="localMenu20153WResource1"></asp:Localize>
                                                        </a>
                                                    </li>--%>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->
                                    </div>
                                    <!-- /.row -->
                                </div>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div>

                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="locHelpLable" runat="server" Text="Do you need help?" meta:resourcekey="locHelpLableResource1"></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="../ContactUs/Contactus.aspx">
                                                            <asp:Localize ID="locSendEmail" runat="server" Text="Send us an email" meta:resourcekey="locSendEmailResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <asp:Localize ID="locSkype" runat="server" Text="Skype: Send us message on 'orshelpdesk'" meta:resourcekey="locSkypeResource1"></asp:Localize>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->

                                        <div class="col-sm-6">
                                            <div class="row">
                                            </div>
                                        </div>
                                        <!-- /.row -->
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>