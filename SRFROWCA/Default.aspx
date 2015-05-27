﻿<%@ Page Title="ORS - Dashboard" Language="C#" MasterPageFile="Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta name="viewport" content="width=device-width">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
    <meta name="description" content="">
    <meta name="author" content="leon-dufour@un.org">

    <%--<link href="http://ors.ocharowca.info/favicon.ico" rel="icon" type="image/x-icon">--%>
    <link rel="stylesheet" href="Visualization/stylesheets/main.css">
    <script type='text/javascript' src='https://public.tableau.com/javascripts/api/viz_v1.js'></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="team">
        <%--<div class="viewport">
            <h1>
                <center>Online Reporting System (ORS)<center>
            </h1>
            <ul>
                <center>
                    <div class='tableauPlaceholder' style='width: 804px; height: 719px;'>
                        <noscript>
                            <a href='#'>
                                <img alt='Dashboard 1 ' src='https:&#47;&#47;public.tableau.com&#47;static&#47;images&#47;H4&#47;H4PTN7DHS&#47;1_rss.png' style='border: none' />
                            </a>
                        </noscript>
                            <object class='tableauViz' width='804' height='719' style='display:none;'>
                                <param name='host_url' value='https%3A%2F%2Fpublic.tableau.com%2F' /> 
                                <param name='path' value='shared&#47;H4PTN7DHS' /> 
                                <param name='toolbar' value='no' />
                                <param name='static_image' value='https:&#47;&#47;public.tableau.com&#47;static&#47;images&#47;H4&#47;H4PTN7DHS&#47;1.png' /> 
                                <param name='animate_transition' value='yes' />
                                <param name='display_static_image' value='yes' />
                                <param name='display_spinner' value='yes' />
                                <param name='display_overlay' value='yes' />
                                <param name='display_count' value='yes' />
                                <param name='showVizHome' value='no' />
                                <param name='showTabs' value='y' />
                                <param name='bootstrapWhenNotified' value='true' /></object>
                    </div>
                    <center>
<h2><center>ORS products<center></h2>
       
            <!-- ip tableau dashboard -->
                <li>
                    <a href="https://public.tableau.com/profile/ocha.rowca#!/vizhome/IPdashboardORS/IPDashboard" target="_blank">
                        <img src="visualization/images/ip.jpg" alt="Humanitarian Projects achievements">
                        <div>
                            <span class="name">Humanitarian Projects achievements</span>
                            <span class="position">Tableau Vizualitation</span>
                            <p>
                                Visualisation showing Humanitarian Projects achievements reported on ORS.
                            </p>
                        </div>
                    </a>
                </li>
                <!-- Output -->
                <li>
                    <a href="https://public.tableau.com/profile/ocha.rowca#!/vizhome/OutputIndicatorsORS/OutputIndicators" target="_blank">
                        <img src="visualization/images/out.jpg" alt="Activity Tracking">
                        <div>
                            <span class="name">Cluster Output indicator achievements</span>
                            <span class="position">Tableau Vizualitation</span>
                            <p>
                                Visualisation showing achievements for the regional output indicators
                            </p>
                        </div>
                    </a>
                </li>
                <!-- RRP6 Dataviz -->
                <li>
                    <a href="link to the report" target="_blank">
                        <img src="visualization/images/rpt.jpg" alt="rrp6">
                        <div>
                            <span class="name">Monthly reporting status</span>
                            <span class="position">Explore budget requirement</span>
                            <p>
                                Report on cluster achievements blablabla
                            </p>
                        </div>
                    </a>
                </li>
                <!-- IM Databox  -->
                <li>
                    <a href="https://blabla" target="_blank">
                        <img src="visualization/images/map.jpg" alt="Information management toolkit">
                        <div>
                            <span class="name">Reported activities per sector / cluster </span>
                            <span class="position">Map</span>
                            <p>
                                The map illustrates the number of reported ongoing activities from the 2015 Strategic Response Plans (SRP) of the 9 Sahel countries.The data was collected from partners who reported on their monthly project achievements via The Sahel Online Reporting System (ORS) 
                            </p>
                        </div>
                    </a>
                </li>
                <!-- IM Databox  -->
                <li>
                    <a href="https://blabla" target="_blank">
                        <img src="visualization/images/xcl.jpg" alt="Information management toolkit">
                        <div>
                            <span class="name">Excel report </span>
                            <span class="position">Excel</span>
                            <p>
                                All raw data blablabla
                            </p>
                        </div>
                    </a>
                </li>
            </ul>
            <div class="clearfix"></div>
        </div>--%>
    </div>
    <%--<script src="visualization/javascripts/main.js"></script>--%>
</asp:Content>
