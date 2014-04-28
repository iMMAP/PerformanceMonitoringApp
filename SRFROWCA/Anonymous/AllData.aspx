<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Anonymous.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
    <style>
        .ddlWidth
        {
            width: 100%;
        }
    </style>
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#<%=txtFromDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected)
                }
            });
            $("#<%=txtToDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected)
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Custom Report</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <table width="100%">
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
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Cluster: </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" CssClass="ddlWidth" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Clusters" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:Label ID="lblCluster" runat="server" Text="" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Objectives</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlObjectives" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Objective" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Priority:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlPriority" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Priority" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Activities:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlActivities" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlActivities_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="300%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Activity" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Indicators:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlIndicators" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlIndicators_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="300%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Indicator" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbRegional" runat="server" Text="Regional" />
                                                                                            <asp:CheckBox ID="cbCountry" runat="server" Text="Country" /></span>

                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbOPSProjects" runat="server" Text="OPS Projects" />
                                                                                            <asp:CheckBox ID="cbORSProjects" runat="server" Text="ORS Projects" /></span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Organization:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="250%" DropDownBoxBoxHeight="300px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Organization" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:Label ID="lblOrganization" runat="server" Text="" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Projects:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight="400%"></Style>
                                                                                            <Texts SelectBoxCaption="Select Project" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Month:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="120px" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight="200px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Month" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>From:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox><span>(mm/dd/yyyy)</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>To:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox><span>(mm/dd/yyyy)</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                     Reported Data:
                                                                                    </td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbValidated" runat="server" Text="Validated" />
                                                                                            <asp:CheckBox ID="cbNotValidated" runat="server" Text="Not Validated" /></span>
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Country:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlCountry" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Country" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:Label ID="lblCountry" runat="server" Text="" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Admin1:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Admin1" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Admin2:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlAdmin2" runat="server" CssClass="ddlWidth" OnSelectedIndexChanged="ddlAdmin2_SelectedIndexChanged"
                                                                                            AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                                                                            UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Admin2" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                     Funding Status:
                                                                                    </td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbFunded" runat="server" Text="Funded" />
                                                                                            <asp:CheckBox ID="cbNotFunded" runat="server" Text="Not Funded" /></span>
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Funding Details:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlFundingStatus" runat="server" OnSelectedIndexChanged="ddlFundingStatus_SelectedIndexChanged">
                                                                                            <asp:ListItem Text="Select FTS Status" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Commitment" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="Paid contribution" Value="2"></asp:ListItem>
                                                                                            <asp:ListItem Text="Pledge" Value="3"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                                                            class="btn btn-primary" />
                                                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn" />
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
                            <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="imagetable"
                                AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                                EnableViewState="false" AllowSorting="True" AllowPaging="true" PageSize="60"
                                ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging" EmptyDataText="Your filter criteria does not match any record in database!">
                                <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Organization" HeaderText="Organization" SortExpression="Organization"
                                        HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="ProjectCode" HeaderText="Project" SortExpression="ProjectCode"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="FundingStatus" HeaderText="FTS" SortExpression="FundingStatus"
                                        HeaderStyle-Width="2%" />
                                    <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective"
                                        HeaderStyle-Width="4%" />
                                    <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority"
                                        HeaderStyle-Width="8%" />
                                    <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity"
                                        HeaderStyle-Width="20%" />
                                    <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator"
                                        HeaderStyle-Width="20%" />
                                    <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Admin1" HeaderText="Admin1" SortExpression="Admin1" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Admin2" HeaderText="Admin2" SortExpression="Admin2" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="AnnualTarget" HeaderText="Annual Target" SortExpression="AnnualTarget"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Accumulative" HeaderText="Accum" SortExpression="Accumulative"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Achieved" HeaderText="Monthly Achieved" SortExpression="Achieved"
                                        HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                                </Columns>
                            </cc2:PagingGridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="fullwidthdiv" style="clear: both;">
            </div>
            <script type="text/javascript">
                jQuery(function ($) {

                    $('#simple-colorpicker-1').ace_colorpicker({ pull_right: true }).on('change', function () {
                        var color_class = $(this).find('option:selected').data('class');
                        var new_class = 'widget-header';
                        if (color_class != 'default') new_class += ' header-color-' + color_class;
                        $(this).closest('.widget-header').attr('class', new_class);
                    });


                    // scrollables
                    $('.slim-scroll').each(function () {
                        var $this = $(this);
                        $this.slimScroll({
                            height: $this.data('height') || 100,
                            railVisible: true
                        });
                    });

                    /**$('.widget-box').on('ace.widget.settings' , function(e) {
                    e.preventDefault();
                    });*/



                    // Portlets (boxes)
                    $('.widget-container-span').sortable({
                        connectWith: '.widget-container-span',
                        items: '> .widget-box',
                        opacity: 0.8,
                        revert: true,
                        forceHelperSize: true,
                        placeholder: 'widget-placeholder',
                        forcePlaceholderSize: true,
                        tolerance: 'pointer'
                    });

                });
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
