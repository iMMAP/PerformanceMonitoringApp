<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Ebola/Ebola.Master" CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Ebola.AllData" %>


<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
    <style>
        .ddlWidth {
            width: 100%;
        }
    </style>
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <link rel="stylesheet" type="text/css" href="../assets/css/tooltipster.css" />
    <!-- ace styles -->
    <%--<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>--%>
    <script type="text/javascript" src="../assets/js/jquery.tooltipster.min.js"></script>
    <script type="text/javascript">
        function pageLoad(sender, args)
        {
            $(function () {
                bindCalendars();
                $(".classsearchcriteriacustomreport").tooltip({
                    show: {
                        effect: "slideDown",
                        delay: 250
                    }
                });
            });
        }
        
        $(document).ready(function () {
            $('.tooltip').tooltipster({
                contentAsHTML: true
            });
        });

        function bindCalendars() {
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
            }
    </script>
</asp:Content>
<asp:Content ID="allDataContent" ContentPlaceHolderID="MainContent" runat="server">
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
    <asp:UpdatePanel ID="pnlAllData" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <div style="text-align: center;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAllData"
                        DynamicLayout="true">
                        <ProgressTemplate>
                            <img src="../assets/orsimages/ajaxlodr.gif" alt="Loading">
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <table style="width:100%">
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
                                                                                        <span>Pillars:</span>
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
                                                                                    <td>
                                                                                        <asp:Literal ID="ltrlValidated" runat="server" Text="Reported Data:"></asp:Literal>
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
                                                                                        <span>Organization:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="250%" DropDownBoxBoxHeight="300px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Organization" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:Label ID="lblOrganization" runat="server" Text="" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Interventions:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight="400%"></Style>
                                                                                            <Texts SelectBoxCaption="Select Interventions" />
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

                
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>Search Criteria
                            </h6>
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <small>
                                <div id="divSearchCriteria" runat="server"></div>
                            </small>
                        </div>
                    </div>
                </div>
                <%--</div>--%>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">

                        <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                            AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                            EnableViewState="false" AllowSorting="True" AllowPaging="true" PageSize="60"
                            ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging" EmptyDataText="Your filter criteria does not match any record in database!">
                            <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />

                            <Columns>
                                <%--<asp:BoundField DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" />--%>
                                <asp:BoundField DataField="Organization" HeaderText="Organization" SortExpression="Organization" />
                                <asp:BoundField DataField="Intervention" HeaderText="Intervention" SortExpression="Intervention" />

                                <%--<asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" />--%>
                                <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective" />
                                <%--<asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />--%>
                                <asp:BoundField DataField="Activity" HeaderText="Pillar" SortExpression="Activity" />
                                <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                                <asp:BoundField DataField="Accumulative" HeaderText="Accum" SortExpression="Accumulative"
                                    ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
                                <asp:BoundField DataField="Admin1" HeaderText="Admin1" SortExpression="Admin1" />
                                <asp:BoundField DataField="Admin2" HeaderText="Admin2" SortExpression="Admin2" />
                                
                                <asp:BoundField DataField="ReportFrequencyName" HeaderText="Reporting Frequency" SortExpression="ReportFrequencyName" />
                                <asp:BoundField DataField="ReportingPeriod" HeaderText="Reporting Period" SortExpression="ReportingPeriod" />
                                <asp:BoundField DataField="Variable" HeaderText="Variable" SortExpression="Variable"
                                    ItemStyle-HorizontalAlign="Right" />
                              <%--  <asp:BoundField DataField="Achieved" HeaderText="Monthly Achieved" SortExpression="Achieved"
                                    ItemStyle-HorizontalAlign="Right" />--%>
                                <asp:TemplateField HeaderText="Cmt">
                                   
                                    <ItemTemplate>
                                        <span class="tooltip" style="opacity: 100;" title="<%# Eval("Comments") %>">
                                            <img src="../assets/orsimages/edit-file-icon.png" />
                                        </span>
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="#4C8FBD" />
                                </asp:TemplateField>



                            </Columns>
                        </cc2:PagingGridView>

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
        <Triggers>
        <asp:PostBackTrigger ControlID="btnExportToExcel" />
    </Triggers>
    </asp:UpdatePanel>

  
</asp:Content>
