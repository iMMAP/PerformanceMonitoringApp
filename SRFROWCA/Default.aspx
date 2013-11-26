<%@ Page Title="3W Activities - Home" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<%--Custom GridView Class to include custom paging functionality.--%>
<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="colcontainer">
        <script type='text/javascript' src='http://public.tableausoftware.com/javascripts/api/viz_v1.js'></script>
        <div class='tableauPlaceholder' style='width: 1204px; height: 969px;'>
            <noscript>
                <a href='#'>
                    <img alt='SUIVI DES ACTIVITES HUMANITAIRES ' src='http:&#47;&#47;public.tableausoftware.com&#47;static&#47;images&#47;3W&#47;3W_activity_11nov2013_FR&#47;Dashboard1&#47;1_rss.png'
                        style='border: none' /></a>
            </noscript>
            <object class='tableauViz' width='1204' height='969' style='display: none;'>
                <param name='host_url' value='http%3A%2F%2Fpublic.tableausoftware.com%2F' />
                <param name='site_root' value='' />
                <param name='name' value='3W_activity_11nov2013_FR&#47;Dashboard1' />
                <param name='tabs' value='no' />
                <param name='toolbar' value='yes' />
                <param name='static_image' value='http:&#47;&#47;public.tableausoftware.com&#47;static&#47;images&#47;3W&#47;3W_activity_11nov2013_FR&#47;Dashboard1&#47;1.png' />
                <param name='animate_transition' value='yes' />
                <param name='display_static_image' value='no' />
                <param name='display_spinner' value='yes' />
                <param name='display_overlay' value='yes' />
                <param name='display_count' value='no' />
            </object>
        </div>
        
    </div>
</asp:Content>
