<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountryIndicatorsAchievedReport.aspx.cs" Inherits="SRFROWCA.PivotTables.CountryIndicatorsAchievedReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active"><asp:Label ID="lblHeaderMessage" runat="server" Text=""></asp:Label></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12 col-sm-12 ">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>
                            <button runat="server" id="btnExport" onserverclick="btnExport_Click" class="btn btn-sm btn-yellow"
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
                                    <telerik:AjaxSetting AjaxControlID="rpgvCountryIndicatorsAchieved">
                                        <UpdatedControls>
                                            <telerik:AjaxUpdatedControl ControlID="rpgvCountryIndicatorsAchieved" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                        </UpdatedControls>
                                    </telerik:AjaxSetting>
                                </AjaxSettings>
                            </telerik:RadAjaxManager>
                            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
                            </telerik:RadAjaxLoadingPanel>
                            <telerik:RadPivotGrid ID="rpgvCountryIndicatorsAchieved" runat="server" OnNeedDataSource="rpgvCountryIndicatorsAchieved_NeedDataSource"
                                AllowFiltering="true" AllowPaging="true" PageSize="30" ShowFilterHeaderZone="false"
                                ShowDataHeaderZone="false" ShowRowHeaderZone="false" ShowColumnHeaderZone="false"
                                EnableConfigurationPanel="true" AllowSorting="true">
                                <Fields>
                                    <telerik:PivotGridRowField DataField="Indicator" Caption="Indicator" UniqueName="Indicator">
                                    </telerik:PivotGridRowField>
                                    <telerik:PivotGridAggregateField DataField="AnnualTarget" Aggregate="Sum">
                                    </telerik:PivotGridAggregateField>
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
