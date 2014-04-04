<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OprOrgsClsCnt.aspx.cs" Inherits="SRFROWCA.PivotTables.OprOrgsClsCnt" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Operational Organizations</li>
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
                                EnableConfigurationPanel="true" AllowSorting="true" TotalsSettings-GrandTotalsVisibility="None"
                                TotalsSettings-ColumnGrandTotalsPosition="None" TotalsSettings-ColumnsSubTotalsPosition="None"
                                TotalsSettings-RowGrandTotalsPosition="None" TotalsSettings-RowsSubTotalsPosition="None">
                                <Fields>
                                    <telerik:PivotGridRowField DataField="Cluster" Caption="Cluster">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridRowField DataField="Country">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridRowField DataField="Organization">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridRowField DataField="Admin1" Caption="Admin1">
                                    </telerik:PivotGridRowField>
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
