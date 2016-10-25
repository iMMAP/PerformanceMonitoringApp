<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="docs.aspx.cs" Inherits="SRFROWCA.api.v2.docs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .method {
            border-bottom-color: #f0f0f0;
            border-bottom-style: solid;
            border-bottom-width: 1px;
            box-shadow: 0 1px 0 #fff;
            padding-bottom: 3px;
            padding-left: 3px;
            padding-right: 3px;
            padding-top: 3px;
        }

        .indented {
            margin-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div style="margin: 0 auto; width:80%;">
            <div class="page-header">
                <h1>ORS API</h1>
            </div>
            <p>
                This is the documentation for the v2 ORS API. If you have additional questions, or believe you have encountered a bug, don't hesitate to contact ORS help desk.
            </p>
            <h3>General</h3>
            <p>All API responses are XML or JSON. You can change the 'format' parameter to change the response.</p>
            <p>Users can trim API responses down to just the fields they are interested in using custom filters. Many fields are not normally returned (Ids for example) that can likewise be requested via a custom filter.</p>
            <hr />
            <h3 class="col-sm-12">Projects</h3>
            <div class="col-sm-8 indented">
                <div class="method">
                    <div class="col-sm-4"><a href="doc/ProjectsAPI.aspx">/Projects</a></div>
                    <div class="col-sm-8">Get all projects</div>
                    <br style="clear: both;">
                </div>
                <div class="method">
                    <div class="col-sm-4"><a href="doc/ProjectsAPI.aspx">/ProjectsTargets</a></div>
                    <div class="col-sm-8">Get projects Targets</div>
                    <br style="clear: both;">
                </div>
                <div class="method">
                    <div class="col-sm-4"><a href="doc/ProjectsAPI.aspx">/ProjectsReports</a></div>
                    <div class="col-sm-8">Get projects Reports</div>
                    <br style="clear: both;">
                </div>
            </div>
            <h3 class="col-sm-12">Key-Figures</h3>
            <div class="col-sm-8 indented">
                <div class="method">
                    <div class="col-sm-4"><a href="doc/KeyFiguresAPI.aspx">/Key-Figures</a></div>
                    <div class="col-sm-8">Get all Reported Key-Figures</div>
                    <br style="clear: both;">
                </div>
                <div class="method">
                    <div class="col-sm-4"><a href="doc/KeyFiguresLakeChadAPI.aspx">/Key-Figures Lake Chad</a></div>
                    <div class="col-sm-8">Get Lake Chad Key-Figures</div>
                    <br style="clear: both;">
                </div>
            </div>
            <h3 class="col-sm-12">Output Indicators</h3>
            <div class="col-sm-8 indented">
                <div class="method">
                    <div class="col-sm-4"><a href="doc/outputindicatorsapi.aspx">/OutputReports</a></div>
                    <div class="col-sm-8">Get Monthly Achived Reports Of Output Inidcators</div>
                    <br style="clear: both;">
                </div>
                <div class="method">
                    <div class="col-sm-4"><a href="doc/ProjectsAPI.aspx">/Output Indicators</a></div>
                    <div class="col-sm-8">Get list of output indicators</div>
                    <br style="clear: both;">
                </div>
            </div>
            <h3 class="col-sm-12">Cluster Framework</h3>
            <div class="col-sm-8 indented">
                <div class="method">
                    <div class="col-sm-4"><a href="doc/activitiesapi.aspx">/Activities</a></div>
                    <div class="col-sm-8">Get Cluster Framework 'Activities'</div>
                    <br style="clear: both;">
                </div>
                <div class="method">
                    <div class="col-sm-4"><a href="doc/Indicatorsapi.aspx">/Indicators</a></div>
                    <div class="col-sm-8">Get Cluster Framework 'Indicators'</div>
                    <br style="clear: both;">
                </div>
            </div>
            <h3 class="col-sm-12">ORS Misc.</h3>
            <div class="col-sm-8 indented">
                <div class="method">
                    <div class="col-sm-4"><a href="doc/orgsbycountry.aspx">/OrganizationByCountry</a></div>
                    <div class="col-sm-8">Get Organizations By Country (Depends on Projects)</div>
                    <br style="clear: both;">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
