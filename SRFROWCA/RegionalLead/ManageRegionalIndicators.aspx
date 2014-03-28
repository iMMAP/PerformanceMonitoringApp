<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageRegionalIndicators.aspx.cs" Inherits="SRFROWCA.RegionalLead.ManageRegionalIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .ddlWidth
        {
            width: 100%;
        }
        .hiddenelement
        {
            display: none;
        }
        .highlight
        {
            background-color: yellow;
        }
        
        .highlight2
        {
            background-color: #ADFF2F;
        }
        
        .highlightRow
        {
            background-color: #DFF0D8;
        }
    </style>
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
                $(this).parent().parent().toggleClass('highlightRow');
            });

            $("tr .testcb").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().addClass('highlightRow');
                }
                else {
                    $(this).parent().removeClass('highlightRow');
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
                        <div class="col-sm-3 widget-container-span">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-orange">
                                    <h4>
                                        Filter Options</h4>
                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                    </i></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-pink">
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
                                                <div class="widget-header widget-header-small header-color-pink">
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
                                                <div class="widget-header widget-header-small header-color-pink">
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
                                                <div class="widget-header widget-header-small header-color-pink">
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
                                <div class="widget-header widget-header-small header-color-orange">
                                    <h4>
                                        Food Security Indicators List</h4>
                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                    </i></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                            OnRowDataBound="gvIndicators_RowDataBound" Width="100%">
                                            <HeaderStyle BackColor="Control"></HeaderStyle>
                                            <RowStyle CssClass="istrow" />
                                            <AlternatingRowStyle CssClass="altcolor" />
                                            <Columns>
                                                <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource1">
                                                    <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                                    ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"
                                                    meta:resourcekey="BoundFieldResource2">
                                                    <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource2">
                                                    <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                                    <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
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
                                                <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                                    SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
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
