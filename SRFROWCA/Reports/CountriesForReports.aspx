<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountriesForReports.aspx.cs" Inherits="SRFROWCA.Reports.CountriesForReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .highlightColumn
        {
            background-color: lightgray;
            cursor: pointer;
        }
    </style>
    <script>
        $(function () {
            $(".imagetable td").hover(function () {
                $("td", $(this).closest("tr")).addClass("highlightColumn");
            }, function () {
                $("td", $(this).closest("tr")).removeClass("highlightColumn");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="Div1">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbCountryReports" runat="server" Text="Country Reports"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%; margin: 0 auto;">
                <asp:GridView ID="gvCountries" runat="server" AutoGenerateColumns="False" CssClass="imagetable" OnRowCommand="gvCountries_RowCommand" OnRowDataBound="gvCountries_RowDataBound"
                    ShowHeaderWhenEmpty="True"
                    Width="40%">
                    <RowStyle CssClass="istrow" Height="40px" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="LocationName" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
