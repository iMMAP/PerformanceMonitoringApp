<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Pages.ManageActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
    <script>
        $(function () {
            $('.srpind').parent('tr:contains("Yes")').addClass('highlightRow2');

            $('tr .testcb').click(function () {
                $(this).parent().parent().toggleClass('highlightRow');
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
        showHideObj();
        showHidePriority();

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="Div1">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Manage Project Activities</li>
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
                                            <asp:Localize ID="lzeLgndProjects" runat="server" meta:resourcekey="lzeLgndProjectsResource1"
                                                Text="Projects"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize>
                                                <asp:Localize ID="lzeLgndStrObjs" runat="server" meta:resourcekey="lzeLgndStrObjsResource1"
                                                    Text="Strategic Objectives"></asp:Localize>
                                        </h5>
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
                                            <asp:Localize ID="lzeLgndHumPriorities" runat="server" meta:resourcekey="lzeLgndHumPrioritiesResource1"
                                                Text="Humanitarian Priorities"></asp:Localize>
                                        </h5>
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
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                            Master List Of Cluster Indicators
                        </h4>
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="icon-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                    HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                    OnRowDataBound="gvIndicators_RowDataBound" Width="100%" EmptyDataText="No Country Specific Indicators Available To Add In Project. Please contact with your country Cluster Lead.">
                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                        <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                            ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                                            <ItemTemplate>
                                                <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                                <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                            SortExpression="ActivityName"></asp:BoundField>
                                        <asp:TemplateField HeaderText="Regional Indicator" SortExpression="IsRegional" HeaderStyle-CssClass="hidden"
                                            ItemStyle-CssClass="srpind hidden">
                                            <ItemTemplate>
                                                <%# (Boolean.Parse(Eval("IsRegional").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Indicator" SortExpression="IsSRP" HeaderStyle-CssClass="hidden"
                                            ItemStyle-CssClass="srpind hidden">
                                            <ItemTemplate>
                                                <%# (Boolean.Parse(Eval("IsSRP").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Added In Project" HeaderStyle-Width="40px" SortExpression="IndicatorIsAdded">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsAdded" runat="server" Checked='<%# Eval("IndicatorIsAdded") %>'
                                                    OnCheckedChanged="cbIsAdded_CheckedChanged" AutoPostBack="true" CssClass="testcb" />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                            SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsSRP" runat="server" Checked='<%# Eval("IsSRP") %>' CssClass="hidden" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
