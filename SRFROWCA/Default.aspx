<%@ Page Title="ORS - Dashboard" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>ORS - Home</title>
    
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
                                    Welcome to ORS!
                                </h1>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <p>
                                        The ORS is a performance monitoring tool that allows UN agencies and NGOs participating in inter-agency planning 
                                        processes to directly report on the achievements based on the activities specified during the SRP/HRP.
                                        The database has been designed to facilitate information sharing and monitor performance of all humanitarian interventions.
                                        The tool hosts all project data as submitted by partners during the SRP/HRP process and is also linked to the FTS database
                                         and website that tracks funding requests and funding status of projects in inter-agency plans.
                                    </p>
                                </div>
                            </div>

                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right width-90">
                                                    <span class="label label-lg label-warning arrowed-in arrowed-in-right width-100">Access to ORS Data and Visualisations!
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li class="altcolor">
                                                        <a href="../Dashboard.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>ORS Dashboard
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Reports/CountryMaps.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Maps
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../KeyFigures/KeyFiguresListingPublic.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Key-Figures
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/ProjectsListingPublic.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Projects 2015 List
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <a href="../Anonymous/OutputIndicatorReport.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Cluster Output Indicators Reported
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/ActivitiesFrameworkPublic.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Cluster Framework 2015
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="../Anonymous/AllData.aspx">
                                                            <i class="ace-icon fa fa-caret-right blue"></i>Custom Report (Project Activities)
                                                        </a>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right width-70">
                                                    <span class="label-lg width-100" style="color:white">.
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <div class=" label label-lg label-info arrowed-in arrowed-right width-70 istrow">
                                                                <b>Login as Cluster Lead</b>
                                                            </div>
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <div class=" label label-lg label-info arrowed-in arrowed-right width-70">
                                                                <b>Login as OCHA Staff</b>
                                                            </div>
                                                        </a>
                                                    </li>

                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <div class=" label label-lg label-info arrowed-in arrowed-right  width-70">
                                                                <b>Login as Data Entry</b>
                                                            </div>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <a href="Account/Login.aspx">
                                                            <div class=" label label-lg label-info arrowed-in arrowed-right  width-70">
                                                                <b>Login as Regional Lead</b>
                                                            </div>
                                                        </a>
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
                                                <b>Do you need help?</b>
                                            </div>
                                            <ul class="list-unstyled spaced">
                                                <li>
                                                    <a href="../ContactUs/Contactus.aspx">
                                                        <i class="ace-icon fa fa-caret-right blue"></i>Send us an email
                                                    </a>
                                                </li>
                                                <li>Skype: Send us message on 'orshelpdesk'
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
