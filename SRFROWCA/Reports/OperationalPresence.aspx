<%@ Page Title="ORS - Contact List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OperationalPresence.aspx.cs" Inherits="SRFROWCA.Reports.OperationalPresence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Contact List</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table class="width-100">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-yellow"
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
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div class="col-sm-3">
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                Organization:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="width-100" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
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
                                                                            <td width="100px">
                                                                                Country:
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-20" AutoPostBack="true"
                                                                                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
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
                    <asp:GridView ID="gvPresence" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        AllowPaging="true" AllowSorting="true" PageSize="50" ShowHeaderWhenEmpty="true"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        OnSorting="gvPresence_Sorting" OnPageIndexChanging="gvPresence_PageIndexChanging">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                            <asp:BoundField DataField="Contact" HeaderText="Contact" SortExpression="Contact" />
                           
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
