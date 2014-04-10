<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SumOfCountryIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.SumOfCountryIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">
                <asp:Label ID="lblHeaderMessage" runat="server" Text=""></asp:Label></li>
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
                                <i class="icon-download"></i>Excel
                            </button>
                        </h6>
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <asp:GridView ID="gvReport" runat="server" Width="100%" CssClass="imagetable"
                                AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                                EnableViewState="false" AllowSorting="True" AllowPaging="true" PageSize="60"
                                ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging" EmptyDataText="Your filter criteria does not match any record in database!">
                                <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator"
                                        HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="ClusterMidYearTarget" HeaderText="Cluster Mid Year Target" SortExpression="ClusterMidYearTarget"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="ClusterFullYearTarget" HeaderText="Cluster Full Year Target" SortExpression="ClusterFullYearTarget"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Achieved" HeaderText="Total Achieved" SortExpression="Achieved"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
