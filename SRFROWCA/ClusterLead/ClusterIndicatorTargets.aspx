<%@ Page Title="Cluster Indicator Target" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClusterIndicatorTargets.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterIndicatorTargets" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

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

    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbClusterTarget" runat="server" Text="Cluster Targets" meta:resourcekey="localBreadCrumbClusterTargetResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>

        <div class="row">
            <div class="col-sm-3">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="localClusterTargetSO" runat="server" Text="Strategic Objectives" meta:resourcekey="localClusterTargetSOResource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj" meta:resourcekey="cblObjectivesResource1">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="localClusterTargetsHP" runat="server" Text="Humanitarian Priorities" meta:resourcekey="localClusterTargetsHPResource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr" meta:resourcekey="cblPrioritiesResource1">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="localClsuterTargetSearchAct" runat="server" Text="Search Activity/Indicator" meta:resourcekey="localClsuterTargetSearchActResource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <input id="txtActivity" type="text" class="width-100" placeholder="Search Activity" />
                                            <div class="space">
                                            </div>
                                            <input id="txtIndicator" type="text" class="width-100" placeholder="Search Indicator" />
                                            <div class="space">
                                            </div>
                                            <asp:DropDownList ID="ddlClusters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"></asp:DropDownList>
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
                                <i class="icon-download"></i>Excel
                            </button>
                        </div>
                        <div class="col-sm-6">
                            <h4>
                                <asp:Localize ID="localizeClusterName" runat="server" meta:resourcekey="localizeClusterNameResource1"></asp:Localize>
                            </h4>
                        </div>
                        <div class="col-sm-3">
                            <button id="btnSave1" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                                type="button" class="btn btn-sm btn-yellow pull-right">
                                <i class="icon-save"></i>
                                <asp:Localize ID="localClsuterTargetsSave" runat="server" Text="Save" meta:resourcekey="localClsuterTargetsSaveResource1"></asp:Localize>
                            </button>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <asp:GridView ID="gvClusterIndTargets" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId,ClusterIndicatorTargetId" CssClass="imagetable"
                                Width="100%" OnRowDataBound="gvClusterIndTargets_RowDataBound" EmptyDataText="Your Cluster Doesn Not Have Mastre List or SRP List Of Activities"
                                AllowSorting="True" OnSorting="gvClusterIndTargets_Sorting" meta:resourcekey="gvClusterIndTargetsResource1">
                                <HeaderStyle BackColor="Control"></HeaderStyle>
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                        ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource3">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Image ID="imgObjective" runat="server" AlternateText="O" meta:resourcekey="imgObjectiveResource1" />
                                            <asp:Image ID="imgPriority" runat="server" AlternateText="P" meta:resourcekey="imgPriorityResource1" />
                                            <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                            <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                                        </ItemTemplate>

                                        <HeaderStyle Width="100px"></HeaderStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                        SortExpression="ActivityName" meta:resourcekey="BoundFieldResource4">
                                        <ItemStyle CssClass="testact"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource5">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource6">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Indicator" HeaderText="Indicator" ItemStyle-CssClass="testind"
                                        SortExpression="Indicator" ItemStyle-Wrap="true" meta:resourcekey="BoundFieldResource7">
                                        <ItemStyle Wrap="True" CssClass="testind"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Mid Year Target" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMidYearTarget" runat="server" CssClass="numeric" Width="60px" Text='<%# Eval("TargetMidYear") %>' meta:resourcekey="txtMidYearTargetResource1"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Year Target" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFullYearTarget" runat="server" CssClass="numeric" Width="60px" Text='<%# Eval("TargetFullYear") %>' meta:resourcekey="txtFullYearTargetResource1"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div>
                        <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                            type="button" class="width-20 pull-right btn btn-sm btn-primary">
                            <i class="icon-save"></i>
                            <asp:Localize ID="localClusterTargetsSave2" runat="server" Text="Save" meta:resourcekey="localClusterTargetsSave2Resource1"></asp:Localize>
                        </button>
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
