<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PerfMontPivot.aspx.cs" Inherits="SRFROWCA.PivotTables.PerfMontPivot" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Custom Report Pivot</li>
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
                                <i class="icon-download"></i>Excel
                            </button>
                        </h6>
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                                <AjaxSettings>
                                    <telerik:AjaxSetting AjaxControlID="rpgvCustomReport">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="rpgvCustomReport" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                            </telerik:RadAjaxLoadingPanel>
                            <telerik:RadPivotGrid ID="rpgvCustomReport" runat="server" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
                                AllowFiltering="true" AllowPaging="true" PageSize="30" ShowFilterHeaderZone="false"
                                ShowDataHeaderZone="false" ShowRowHeaderZone="false" ShowColumnHeaderZone="false"
                                EnableConfigurationPanel="true" AllowSorting="true" AggregatesLevel="0"
                                TotalsSettings-GrandTotalsVisibility="None"
                                TotalsSettings-RowsSubTotalsPosition="None">
                                <Fields>
                                    <telerik:PivotGridColumnField DataField="Month">
                                    </telerik:PivotGridColumnField>
                                    <telerik:PivotGridRowField DataField="Admin1" Caption="Location">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridRowField DataField="Indicator" Caption="Indicator" UniqueName="Indicator">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridAggregateField DataField="Achieved" Aggregate="Sum">
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
