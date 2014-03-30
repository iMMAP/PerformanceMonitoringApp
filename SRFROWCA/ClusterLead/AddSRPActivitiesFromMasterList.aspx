<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddSRPActivitiesFromMasterList.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddSRPActivitiesFromMasterList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Country Indicators </title>
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
                <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
                <li class="active">Select Regional Indicators</li>
            </ul>
            <!-- .breadcrumb -->
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
                                                    <h5>
                                                        Strategic Objectives</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
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
                                                    <h5>
                                                        Humanitarian Priorities</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
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
                                                    <h5>
                                                        Search Activity/Indicator</h5>
                                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                                    </i></a></span>
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
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h4>
                                        <asp:Localize ID="localizeClusterName" runat="server" Text=""></asp:Localize>
                                    </h4>
                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                    </i></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <asp:GridView ID="gvSRPIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                            Width="100%" OnRowDataBound="gvSRPIndicators_RowDataBound" EmptyDataText="Your Cluster Doesn Not Have Mastre List or SRP List Of Activities"
                                            AllowSorting="true" OnSorting="gvSRPIndicators_Sorting">
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
                                                        <asp:Image ID="imgRind" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                                    SortExpression="ActivityName"></asp:BoundField>
                                                <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="hidden" 
                                                    SortExpression="IsRegional" ItemStyle-CssClass="hidden" >
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRegional" runat="server"
                                                            Checked='<%# Eval("IsRegional") %>' CssClass="testrcb" Enabled="false" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Country Indicator" meta:resourcekey="TemplateFieldResource2"
                                                    SortExpression="IsSRP" ItemStyle-Width="40px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSRP" runat="server" AutoPostBack="true" OnCheckedChanged="chkSRP_CheckedChanged"
                                                            Checked='<%# Eval("IsSRP") %>' CssClass="testcb" />
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
</asp:Content>
