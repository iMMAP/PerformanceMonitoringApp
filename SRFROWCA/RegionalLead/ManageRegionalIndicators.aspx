<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageRegionalIndicators.aspx.cs" Inherits="SRFROWCA.RegionalLead.ManageRegionalIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
    <script type="text/javascript" src="../assets/orsjs/jq-highlight.js"></script>
    <script>
        $(function () {
            showHideObj();
            showHidePriority();

            $('#txtActivity').keyup(function () {
                $(".testact").unhighlight();
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".testact").highlight(terms);
                    }
                }

            });

            $('#txtIndicator').keyup(function () {
                $(".testind").unhighlight({ element: 'span', className: 'highlight2' });
                var searchTerm = $(this).val().toLowerCase();
                if (searchTerm) {
                    var terms = searchTerm.split(' ');
                    if (terms.length > 0) {
                        $(".testind").highlight(terms, { element: 'span', className: 'highlight2' });
                    }
                }
            });

            $('tr .testcb').click(function () {
                $(this).parent().parent().toggleClass('highlightRow2');
            });

            $("tr .testcb").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().addClass('highlightRow2');
                }
                else {
                    $(this).parent().removeClass('highlightRow2');
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="breadcrumbs" id="breadcrumbs">
            <script type="text/javascript">
                try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
            </script>
            <ul class="breadcrumb">
                <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
                <li class="active">Select Regional Indicators</li>
            </ul>
            <!-- .breadcrumb -->
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="widget-box no-border">
                                
                                <div class="widget-body">
                                    <div class="widget-main no-padding-top">
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>
                                                        Strategic Objectives</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj checkbox">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>
                                                        Humanitarian Priorities</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr checkbox">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>
                                                        Search (highlight) Activity</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <input id="txtActivity" type="text" class="ddlWidth" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>
                                                        Search (highlight) Indicator</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <input id="txtIndicator" type="text" class="ddlWidth" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-9 widget-container-span">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h4>
                                        <asp:Localize ID="localizeClusterName" runat="server" Text=""></asp:Localize>
                                    </h4>
                                  
                            <div class="btn-group pull-right">
                                <button runat="server" id="btnAddRegActivity" onserverclick="btnAddRegActivity_Click"
                                    class="width-10 btn btn-sm btn-yellow" title="Add Indicator" style="margin-right:5px;">
                                    <asp:Localize ID="localCountryIndicatorsAddAct" runat="server" Text="Add Activity & Indicator" meta:resourcekey="localCountryIndicatorsAddActResource1"></asp:Localize>
                                </button>
                                <button runat="server" id="btnAddIndicator" onserverclick="btnAddIndicator_Click"
                                    class="width-10 btn btn-sm btn-yellow" title="Add Indicator">
                                    <asp:Localize ID="localCountryIndicaotrsAddInd" runat="server" Text="Add Indicator" meta:resourcekey="localCountryIndicaotrsAddIndResource1"></asp:Localize>
                                </button>
                            </div>
                       <%--<a href="#" data-action="collapse"><i class="icon-chevron-up">
                                    </i></a>--%>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                            OnRowDataBound="gvIndicators_RowDataBound" Width="100%" OnRowCommand="gvIndicators_RowCommand">
                                            <HeaderStyle BackColor="Control"></HeaderStyle>
                                            <RowStyle CssClass="istrow" />
                                            <AlternatingRowStyle CssClass="altcolor" />
                                            <Columns>
                                                <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hidden"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                                    ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"
                                                    meta:resourcekey="BoundFieldResource2">
                                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hidden"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2">
                                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hidden"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                                        <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                                    SortExpression="ActivityName"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Regional Indicator" meta:resourcekey="TemplateFieldResource2"
                                                    SortExpression="IsRegional" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRegional" runat="server" AutoPostBack="true" OnCheckedChanged="chkRegional_CheckedChanged"
                                                            Checked='<%# Eval("IsRegional") %>' CssClass="testcb" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Indicator" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                                    SortExpression="Indicator" ItemStyle-Wrap="true"></asp:BoundField>
                                                <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkVieDetails" runat="server" Text="Edit" CommandName="EditIndicator"
                                                CommandArgument='<%# Eval("ActivityDataId") %>' meta:resourcekey="lnkVieDetailsResource1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        jQuery(function ($) {

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
</asp:Content>
