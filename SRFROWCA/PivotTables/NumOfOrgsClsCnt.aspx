<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="NumOfOrgsClsCnt.aspx.cs" Inherits="SRFROWCA.PivotTables.NumOfOrgsClsCnt" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        div.qsf-right-content .qsf-col-wrap
        {
            position: static;
        }
        
        .RadForm.rfdFieldset fieldset.fieldset legend, .RadForm.rfdLabel .RadInput label.riLabel
        {
            font-size: 14px;
        }
        
        .fieldset .RadInput
        {
            margin-bottom: 5px;
        }
    </style>
   <%-- <script>
        $(function () {
            var obj = jQuery.parseJSON('[{"ProjectId":64233,"ProjectTitle":"Provision of humanitarian air services in the region (SO 200520)","OrganizationId":561,"OrganizationName":"World Food Programme"}, {"ProjectId":64235,"ProjectTitle":"Provision of humanitarian air services in the region (SO 200520)","OrganizationId":561,"OrganizationName":"World Food Programme"}]');
            for (var i = 0, len = obj.length; i < len; i++) {
                    alert(obj[i].ProjectId);
                }
            });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Organizations per Cluter & Country</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12 col-sm-12 ">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>
                            <button runat="server" id="btnExport" onserverclick="btnExport_Click" class="width-10 btn btn-sm btn-yellow"
                                title="Excel">
                                <i class="fa fa-download"></i>Excel
                            </button>
                        </h6>
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse"><i class="fa fa-chevron-down"></i></a>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">                          
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="rpgvOpePresence">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="rpgvOpePresence" LoadingPanelID="RadAjaxLoadingPanel1">
                                            </telerik:AjaxUpdatedControl>
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                            </telerik:RadAjaxLoadingPanel>
                            <telerik:RadPivotGrid ID="rpgvOpePresence" runat="server" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
                                AllowFiltering="true" AllowPaging="true" PageSize="30" ShowFilterHeaderZone="false"
                                ShowDataHeaderZone="false" ShowRowHeaderZone="false" ShowColumnHeaderZone="false"
                                 AllowSorting="true">
                                <Fields>
                                    <telerik:PivotGridRowField DataField="Cluster" Caption="Cluster"></telerik:PivotGridRowField>
                                    <telerik:PivotGridColumnField DataField="Country">
                                    </telerik:PivotGridColumnField>
                                    <telerik:PivotGridAggregateField DataField="NumberOfOrg" Aggregate="Sum">
                                    </telerik:PivotGridAggregateField>
                                </Fields>
                                <ConfigurationPanelSettings Position="Left" DefaultDeferedLayoutUpdate="true" />
                            </telerik:RadPivotGrid>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
