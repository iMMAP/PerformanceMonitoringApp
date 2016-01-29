<%@ Page Title="ORS - Home" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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
                                    <asp:localize id="locDefaultWelcome" runat="server" text="Welcome to ORS!" meta:resourcekey="locDefaultWelcomeResource1"></asp:localize>
                                </h1>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <p>
                                        <asp:localize id="locDefaultIntro" runat="server" text="The Sahel Online Reporting System (ORS) is a performance monitoring tool that allows humanitarian partners 
                                        participating in inter-agency planning processes to directly report on the achievements based on the 
                                        activities specified during the SRP/HRP. The database has been designed to facilitate information sharing 
                                        and monitor response of humanitarian interventions."
                                            meta:resourcekey="locDefaultIntroResource1"></asp:localize>
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
                                                        <asp:localize id="localMenuORSLable" runat="server" text="Access to ORS Data and Visualisations!" meta:resourcekey="localMenuORSLableResource1"></asp:localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkDashboard" runat="server" Text="ORS Dashboard" PostBackUrl="../Dashboard.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuDashboardResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Dashboard.aspx">
                                                            <asp:localize id="localMenuDashboard" runat="server" text="ORS Dashboard" meta:resourcekey="localMenuDashboardResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkCountryMaps" runat="server" Text="Maps" PostBackUrl="../Reports/CountryMaps.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuMapsResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Reports/CountryMaps.aspx">
                                                            <asp:localize id="localMenuMaps" runat="server" text="Maps" meta:resourcekey="localMenuMapsResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkKeyFigures" runat="server" Text="Key-Figures" PostBackUrl="../KeyFigures/KeyFiguresListingPublic.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuKeyFiguresResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../KeyFigures/KeyFiguresListingPublic.aspx">
                                                            <asp:localize id="localMenuKeyFigures" runat="server" text="Key-Figures" meta:resourcekey="localMenuKeyFiguresResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProjects" runat="server" Text="Projects List" PostBackUrl="../Anonymous/ProjectsListingPublic.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuProjectsResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Anonymous/ProjectsListingPublic.aspx">
                                                            <asp:localize id="localMenuProjects" runat="server" text="Projects 2015 List" meta:resourcekey="localMenuProjectsResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkOutputIndicators" runat="server" Text="Cluter Output Indicators Reported" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                             PostBackUrl="../Anonymous/OutputIndicatorReport.aspx" meta:resourcekey="localMenuOutputIndResource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Anonymous/OutputIndicatorReport.aspx">
                                                            <asp:localize id="localMenuOutputInd" runat="server" text="Cluster Output Indicators Reported" meta:resourcekey="localMenuOutputIndResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkFramework" runat="server" Text="Cluster Framework" PostBackUrl="../Anonymous/ActivitiesFrameworkPublic.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuFramework2015Resource1"></asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Anonymous/ActivitiesFrameworkPublic.aspx">
                                                            <asp:localize id="localMenuFramework2015" runat="server" text="Cluster Framework" meta:resourcekey="localMenuFramework2015Resource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkAllData" runat="server" Text="Custom Report (Project Activities)" PostBackUrl="../Anonymous/AllData.aspx" CssClass="btn btn-xs btn-inverse width-70 tooltip elemopacity"
                                                            meta:resourcekey="localMenuCustomReportResource1">
                                                        </asp:LinkButton>
                                                    </li>
                                                    <%--<li>
                                                        <a href="../Anonymous/AllData.aspx">
                                                            <asp:localize id="localMenuCustomReport" runat="server" text="Custom Report (Project Activities)" meta:resourcekey="localMenuCustomReportResource1"></asp:localize>
                                                        </a>
                                                    </li>--%>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right width-100">
                                                    <span class="label label-lg label-warning arrowed-in arrowed-in-right width-80">
                                                        <asp:localize id="localMenuLogRegLable" runat="server" text="Login or Register!" meta:resourcekey="localMenuLogRegLableResource1"></asp:localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <asp:button id="btnLoginCC" runat="server" cssclass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                                postbackurl="~/Account/Login.aspx" text="Login as Cluster Coordinator" meta:resourcekey="btnLoginCCResource1" />
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <asp:button id="btnLoginOCHA" runat="server" cssclass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            postbackurl="~/Account/Login.aspx" text="Login as OCHA Staff" meta:resourcekey="btnLoginOCHAResource1" />

                                                    </li>

                                                    <li>
                                                        <asp:button id="btnLoginDE" runat="server" cssclass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            postbackurl="~/Account/Login.aspx" text="Login as Data Entry" meta:resourcekey="btnLoginDEResource1" />
                                                    </li>
                                                    <li>
                                                        <asp:button id="btnLoginRC" runat="server" cssclass="btn btn-xs btn-primary width-80 tooltip elemopacity"
                                                            postbackurl="~/Account/Login.aspx" text="Login as Regional Coordinator" meta:resourcekey="btnLoginRCResource1" />
                                                    </li>
                                                </ul>

                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <span class="center"></span>
                                                    </li>
                                                    <li>
                                                        <asp:localize id="localNoAccount" runat="server" text="Don't have ORS account?" meta:resourcekey="localDonthaveORSResource"></asp:localize>
                                                        <asp:button id="btnRegister" runat="server" text="Register"
                                                            postbackurl="~/Account/Register.aspx" cssclass="btn btn-sm btn-danger btn-sm" meta:resourcekey="btnRegisterResource1" />
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
                                                    <asp:localize id="locHelpLable" runat="server" text="Do you need help?" meta:resourcekey="locHelpLableResource1"></asp:localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="../ContactUs/Contactus.aspx">
                                                            <asp:localize id="locSendEmail" runat="server" text="Send us an email" meta:resourcekey="locSendEmailResource1"></asp:localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <asp:localize id="locSkype" runat="server" text="Skype: Send us message on 'orshelpdesk'" meta:resourcekey="locSkypeResource1"></asp:localize>
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
