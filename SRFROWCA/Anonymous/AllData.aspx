<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Anonymous.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ register assembly="SRFROWCA" namespace="SRFROWCA" tagprefix="cc2" %>
    <style>
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            /* min-width: 200px;*/
            min-height: 150px;
            background: white;
        }
        .ddlWidth
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            // Make XML DataFeed Link using hidden field
            var hf = $("#<%=hfReportLink.ClientID%>").val();
            var hl = $('#reportlink').attr("href");
            $('#reportlink').attr("href", hl + hf);

            // Add colgroup on top of the grid so kiketable_closizable jQuery script can work.
            // This script is to give user funcationality to increase/decrease width of grid column.

        }
    </script>
    
    <script>
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
            <li class="active">Data Entry</li>
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
                                        <div class="widget-header widget-header-small header-color-orange">
                                            <h6>
                                                <i class="icon-sort"></i>Small Header & Collapsed
                                            </h6>
                                            <div class="widget-toolbar">
                                                <a href="#" data-action="settings"><i class="icon-cog"></i></a><a href="#" data-action="reload">
                                                    <i class="icon-refresh"></i></a><a href="#" data-action="collapse"><i class="icon-chevron-down">
                                                    </i></a><a href="#" data-action="close"><i class="icon-remove"></i></a>
                                            </div>
                                        </div>
                                        <div class="widget-body">
                                            <div class="widget-main">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="row">
                                                            <div class="col-sm-4">
                                                                <div class="widget-box">
                                                                    <div class="widget-header">
                                                                        <h5 class="smaller">
                                                                            General Inforamtion</h5>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Projects:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="300px" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Project" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Organization:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="600px" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Organization" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Month:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                                                                            AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                                                                            UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="120px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Month" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            From:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            To:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box light-border">
                                                                    <div class="widget-header">
                                                                        <h5 class="smaller">
                                                                            Logframe Filters</h5>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Cluster:
                                                                                        </label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                                                                            AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                                                                            CssClass="ddlWidth" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Clusters" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Objectives</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlObjectives" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Objective" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Priority:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlPriority" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Priority" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Activities:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlActivities" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Activity" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Indicators:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlIndicators" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Indicator" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:CheckBox ID="cbRegional" runat="server" Text="Regional Ind" />
                                                                                        <asp:CheckBox ID="cbCountry" runat="server" Text="Country Ind" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box light-border">
                                                                    <div class="widget-header">
                                                                        <h5 class="smaller">
                                                                            Location Filters</h5>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Country:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlCountry" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Country" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Admin1:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Admin1" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Admin2:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlAdmin2" runat="server" CssClass="ddlWidth"
                                                                                            OnSelectedIndexChanged="ddlAdmin2_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Admin2" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Funding Status:</label>
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
                                                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="btn btn-primary" />
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
                            <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="table table-striped table-bordered table-hover"
                                AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                                EnableViewState="false" AllowSorting="True" AllowPaging="true" PageSize="60"
                                ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging"
                                 EmptyDataText="Your filter criteria does not match any record in database!" >
                                <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                                <PagerSettings Mode="NumericFirstLast" />
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster"
                                        HeaderStyle-Width="6%" />
                                    <asp:BoundField DataField="Organization" HeaderText="Organization" SortExpression="Organization"
                                        HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="ProjectCode" HeaderText="Project" SortExpression="ProjectCode"
                                        HeaderStyle-Width="8%" />
                                        <asp:BoundField DataField="FundingStatus" HeaderText="FTS" SortExpression="FundingStatus"
                                        HeaderStyle-Width="5%" />                                        
                                    <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority"
                                        HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity"
                                        HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator"
                                        HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Admin1" HeaderText="Admin1" SortExpression="Admin1"
                                        HeaderStyle-Width="5%" />
                                    <asp:BoundField DataField="Admin2" HeaderText="Admin2" SortExpression="Admin2"
                                        HeaderStyle-Width="5%" />
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
            <div class="buttonsdiv">
                <div class="savebutton">
                    <asp:Button ID="btnExportToExcel" runat="server" CausesValidation="false" Text="Export To Excel"
                        CssClass="button_example" OnClick="btnExportToExcel_Click" />
                    <a id="reportlink" href="../DataFeed.ashx">
                        <input type="button" value="XML Of Data" class="button_example" /></a>
                    <asp:HiddenField ID="hfReportLink" runat="server" Value="" />
                </div>
                <div class="spacer" style="clear: both;">
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
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenTargetControlForModalPopup"
        PopupControlID="Panel1" Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="HiddenTargetControlForModalPopup" Style="display: none" />
    <asp:Panel ID="Panel1" Style="display: block; width: 550px;" runat="server">
        <div class="containerPopup">
            <div class="graybar">
                Add/Remove Columns
            </div>
            <div class="contentarea">
                <div class="formdiv">
                    <table border="0" style="margin: 0 auto;">
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cbColumns" runat="server" RepeatColumns="4">
                                    <asp:ListItem Text="DataId" Value="DataId"></asp:ListItem>
                                    <asp:ListItem Text="Organization Full Name" Value="Organization"></asp:ListItem>
                                    <asp:ListItem Text="Office" Value="Office"></asp:ListItem>
                                    <asp:ListItem Text="(AD1)PCode" Value="(AD1)PCode"></asp:ListItem>
                                    <asp:ListItem Text="(Ad2)PCode" Value="(Ad2)PCode"></asp:ListItem>
                                    <asp:ListItem Text="UserName" Value="UserName"></asp:ListItem>
                                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                    <asp:ListItem Text="ReportDate" Value="ReportDate"></asp:ListItem>
                                    <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="button_example" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" OnClick="btnClose_Click"
                                    CssClass="button_example" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer" style="clear: both;">
                    </div>
                </div>
            </div>
            <div class="graybarcontainer">
            </div>
        </div>
    </asp:Panel>
</asp:Content>
