<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ReportsDefault.aspx.cs" Inherits="SRFROWCA.Reports.ReportsDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script src="/Scripts/hc/highcharts.js" type="text/javascript"></script>
<script src="/Scripts/hc/modules/exporting.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div style="width: 8%; float: left;">
            <asp:Menu ID="ReportMenu" runat="server" CssClass="menu" EnableViewState="false" OnMenuItemClick="ReportMenue_OnClick"
                    IncludeStyleBlock="false" Orientation="Vertical">
                    <Items>
                        <asp:MenuItem NavigateUrl="~/ReportsCharts/ReportsDefault.aspx" Text="T&A By Locations" />
                        <asp:MenuItem NavigateUrl="~/ReportsCharts/ReportsDefault.aspx" Text="T&A By Duration" />
                    </Items>
                </asp:Menu>
        </div>
        <div style="width: 92%; float: left;">
            <asp:Panel ID="pnlReport" runat="server">
            </asp:Panel>
        </div>
    </div>
</asp:Content>
