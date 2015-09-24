<%@ Page Title="ORS - Home" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" culture="auto" meta:resourcekey="PageResource1" uiculture="auto"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .elemopacity {
            opacity: 100;
            width: 100%;
        }
    </style>
    <script>
        $(function () {
            $.widget("ui.tooltip", $.ui.tooltip, {
                options: {
                    content: function () {
                        return $(this).prop('title');
                    }
                }
            });
            $('.tooltip').tooltip();
        });
    </script>


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
                                <h1 class="widget-title smaller">
                                    <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                    <asp:Localize ID="locDefaultWelcome" runat="server" Text="Welcome to ORS!" meta:resourcekey="locDefaultWelcomeResource1"></asp:Localize>
                                </h1>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <p>
                                        <asp:Localize ID="locDefaultIntro" runat="server" Text="The Sahel Online Reporting System (ORS) is a performance monitoring tool that allows humanitarian partners 
                                        participating in inter-agency planning processes to directly report on the achievements based on the 
                                        activities specified during the SRP/HRP. The database has been designed to facilitate information sharing 
                                        and monitor response of humanitarian interventions." meta:resourcekey="locDefaultIntroResource1"></asp:Localize>
                                    </p>
                                </div>
                            </div>

                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right width-100">
                                                    <span class="label label-lg label-warning arrowed-in arrowed-in-right width-90">
                                                        <asp:Localize ID="localMenuORSLable" runat="server" Text="Access to ORS Data and Visualisations!" meta:resourcekey="localMenuORSLableResource1"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li class="altcolor">
                                                        <a href="../Dashboard.aspx">
                                                            <asp:Localize ID="localMenuDashboard" runat="server" Text="ORS Dashboard" meta:resourcekey="localMenuDashboardResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Reports/CountryMaps.aspx">
                                                            <asp:Localize ID="localMenuMaps" runat="server" Text="Maps" meta:resourcekey="localMenuMapsResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../KeyFigures/KeyFiguresListingPublic.aspx">
                                                            <asp:Localize ID="localMenuKeyFigures" runat="server" Text="Key-Figures" meta:resourcekey="localMenuKeyFiguresResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/ProjectsListingPublic.aspx">
                                                            <asp:Localize ID="localMenuProjects" runat="server" Text="Projects 2015 List" meta:resourcekey="localMenuProjectsResource1"></asp:Localize>
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <a href="../Anonymous/OutputIndicatorReport.aspx">
                                                            <asp:Localize ID="localMenuOutputInd" runat="server" Text="Cluster Output Indicators Reported" meta:resourcekey="localMenuOutputIndResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/ActivitiesFrameworkPublic.aspx">
                                                            <asp:Localize ID="localMenuFramework2015" runat="server" Text="Cluster Framework 2015" meta:resourcekey="localMenuFramework2015Resource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/AllData.aspx">
                                                            <asp:Localize ID="localMenuCustomReport" runat="server" Text="Custom Report (Project Activities)" meta:resourcekey="localMenuCustomReportResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right width-100">
                                                    <span class="label label-lg label-warning arrowed-in arrowed-in-right width-80">
                                                        <asp:Localize ID="localMenuLogRegLable" runat="server" Text="Login or Register!" meta:resourcekey="localMenuLogRegLableResource1"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <asp:Button ID="btnLoginCC" runat="server" CssClass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                                PostBackUrl="~/Account/Login.aspx" Text="Login as Cluster Coordinator" meta:resourcekey="btnLoginCCResource1"/>
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <asp:Button ID="btnLoginOCHA" runat="server" CssClass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            PostBackUrl="~/Account/Login.aspx" Text="Login as OCHA Staff" meta:resourcekey="btnLoginOCHAResource1" />

                                                    </li>

                                                    <li>
                                                        <asp:Button ID="btnLoginDE" runat="server" CssClass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            PostBackUrl="~/Account/Login.aspx" Text="Login as Data Entry" meta:resourcekey="btnLoginDEResource1" />
                                                    </li>
                                                    <li>
                                                        <asp:Button ID="btnLoginRC" runat="server" CssClass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            PostBackUrl="~/Account/Login.aspx" Text="Login as Regional Coordinator" meta:resourcekey="btnLoginRCResource1" />
                                                    </li>
                                                </ul>

                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <span class="center"></span>
                                                    </li>
                                                    <li>Don't have ORS account?
                                                            <asp:Button ID="btnRegister" runat="server" Text="Register"
                                                                PostBackUrl="~/Account/Register.aspx" CssClass="btn btn-danger btn-sm" meta:resourcekey="btnRegisterResource1" />
                                                    </li>
                                                </ul>
                                            </div>
                                    </div>
                                </div>
                                <!-- /.col -->
                            </div>
                            <!-- /.row -->
                        </div>

                        <div class="widget-body transparent">
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
