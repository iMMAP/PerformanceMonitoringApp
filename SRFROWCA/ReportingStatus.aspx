<%@ Page Title="ORS - Reporting Status" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ReportingStatus.aspx.cs" Inherits="SRFROWCA.ReportingStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Reporting Status</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <script type='text/javascript' src='http://public.tableausoftware.com/javascripts/api/viz_v1.js'></script>
    <div class='tableauPlaceholder' style='width: 1004px; height: 869px;'>
        <noscript>
            <a href='#'>
                <img alt='Dashboard ' src='http:&#47;&#47;public.tableausoftware.com&#47;static&#47;images&#47;OR&#47;ORS_Reporting_25March&#47;Dashboard&#47;1_rss.png'
                    style='border: none' /></a></noscript><object class='tableauViz' width='1004' height='869'
                        style='display: none;'><param name='host_url' value='http%3A%2F%2Fpublic.tableausoftware.com%2F' />
                        <param name='site_root' value='' />
                        <param name='name' value='ORS_Reporting_25March&#47;Dashboard' />
                        <param name='tabs' value='no' />
                        <param name='toolbar' value='yes' />
                        <param name='static_image' value='http:&#47;&#47;public.tableausoftware.com&#47;static&#47;images&#47;OR&#47;ORS_Reporting_25March&#47;Dashboard&#47;1.png' />
                        <param name='animate_transition' value='yes' />
                        <param name='display_static_image' value='yes' />
                        <param name='display_spinner' value='yes' />
                        <param name='display_overlay' value='yes' />
                        <param name='display_count' value='yes' />
                        <param name='filter' value='amp;:showVizHome=no' />
                    </object>
    </div>
    <div style='width: 1004px; height: 22px; padding: 0px 10px 0px 0px; color: black;
        font: normal 8pt verdana,helvetica,arial,sans-serif;'>
        <div style='float: right; padding-right: 8px;'>
            <a href='http://www.tableausoftware.com/public/about-tableau-products?ref=http://public.tableausoftware.com/views/ORS_Reporting_25March/Dashboard'
                target='_blank'>En savoir plus sur Tableau</a></div>
    </div>
</asp:Content>
