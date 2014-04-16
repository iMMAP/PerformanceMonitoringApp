<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TopIndicatorsGeneral.aspx.cs" Inherits="SRFROWCA.Reports.TopIndicatorsGeneral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Top 10 Indicators Regional</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <%-- <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-primary"
                                            title="Excel">
                                            <i class="fa fa-download"></i>Excel
                                        </button>--%>
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="fa fa-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td width="100px">
                                                                                Project Code:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-80" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="100px">
                                                                                Country:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-80" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-9">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table class="width-100">
                                                                        <tr>
                                                                           <td width="150px">
                                                                                Strategic Objectives:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlObjectives" runat="server" CssClass="width-30" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Humanitarian Priorities:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlPriorities" runat="server" CssClass="width-30" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div class="space">
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        AllowPaging="true" AllowSorting="true" PageSize="50" ShowHeaderWhenEmpty="true"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="Objective" HeaderText="Objective" />
                            <asp:BoundField DataField="Prioriry" HeaderText="Priority" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" />
                            <asp:BoundField DataField="Activity" HeaderText="Activity"  />
                            <asp:BoundField DataField="Indicator" HeaderText="Indicator" />
                            <asp:BoundField DataField="NumerOfTimes" HeaderText="Number Of Times Used"  />
                            
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
