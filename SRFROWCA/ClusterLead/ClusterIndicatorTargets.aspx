<%@ Page Title="Cluster Indicator Target" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClusterIndicatorTargets.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterIndicatorTargets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
    <script type="text/javascript" src="../assets/orsjs/jq-highlight.js"></script>
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
    <script>
        $(function () {
            showHideObj();
            showHidePriority();
            $(".numeric").numeric();

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

            $("tr .testrcb").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().addClass('highlightRow2');
                }
                else {
                    $(this).parent().parent().removeClass('highlightRow2');
                }
            });

            $("tr .testcb").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().removeClass('highlightRow2');
                    $(this).parent().parent().addClass('highlightRow');
                }
                else {
                    $(this).parent().parent().removeClass('highlightRow');
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
                <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
                <li class="active">Select Regional Indicators</li>
            </ul>
            <!-- .breadcrumb -->
        </div>
        <div id="divMsg">
        </div>
        <table>
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
                                                    <h5>Strategic Objectives</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>Humanitarian Priorities</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr">
                                                        </asp:CheckBoxList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-14 widget-container-span">
                                            <div class="widget-box">
                                                <div class="widget-header widget-header-small header-color-blue2">
                                                    <h5>Search Activity/Indicator</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
                                                </div>
                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <input id="txtActivity" type="text" class="width-100" placeholder="Search Activity" />
                                                        <div class="space">
                                                        </div>
                                                        <input id="txtIndicator" type="text" class="width-100" placeholder="Search Indicator" />
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
                                <div class="widget-header widget-header header-color-blue2">
                                    <div class="col-sm-3">
                                        <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="btn btn-sm btn-yellow"
                                            title="Excel">
                                            <i class="fa fa-download"></i>Excel
                                        </button>
                                    </div>
                                    <div class="col-sm-6">
                                        <h4>
                                            <asp:Localize ID="localizeClusterName" runat="server" Text=""></asp:Localize>
                                        </h4>
                                    </div>
                                    <div class="col-sm-3">
                                    <button id="Button1" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                                        type="button" class="btn btn-sm btn-yellow pull-right">
                                        <i class="fa fa-save"></i>Save
                                    </button>
                                </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <asp:GridView ID="gvClusterIndTargets" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId,ClusterIndicatorTargetId" CssClass="imagetable"
                                            Width="100%" OnRowDataBound="gvClusterIndTargets_RowDataBound" EmptyDataText="Your Cluster Doesn Not Have Mastre List or SRP List Of Activities"
                                            AllowSorting="true" OnSorting="gvClusterIndTargets_Sorting">
                                            <HeaderStyle BackColor="Control"></HeaderStyle>
                                            <RowStyle CssClass="istrow" />
                                            <AlternatingRowStyle CssClass="altcolor" />
                                            <Columns>
                                                <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                                <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                                    ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                                <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                                <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                                        <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                                        <asp:Image ID="imgRind" runat="server" />
                                                        <asp:Image ID="imgCind" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                                    SortExpression="ActivityName"></asp:BoundField>
                                                <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                <asp:BoundField DataField="Indicator" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                                    SortExpression="Indicator" ItemStyle-Wrap="true"></asp:BoundField>
                                                <asp:TemplateField HeaderText="Mid Year Target">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMidYearTarget" runat="server" CssClass="numeric" Width="60px" Text='<%# Eval("TargetMidYear") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Full Year Target">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFullYearTarget" runat="server" CssClass="numeric" Width="60px" Text='<%# Eval("TargetFullYear") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div>
                                    <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                                        type="button" class="width-20 pull-right btn btn-sm btn-primary">
                                        <i class="fa fa-save"></i>Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
