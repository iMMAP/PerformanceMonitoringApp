<%@ Page Title="ORS Manage Project Activities" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Ebola.ManageActivities" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
   
     <script>
        $(function () {
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

            // scrollables
            $('.slim-scroll').each(function () {
                var $this = $(this);
                $this.slimScroll({
                    height: $this.data('height') || 100,
                    railVisible: true
                });
            });
        });
        showHideObj();
        showHidePriority();
    </script>

</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="Div1">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbManageProjectActiviites" runat="server" Text="Manage Intervention Indicators" ></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-3">
               <%-- <div class="widget-box no-border">--%>
                    <div class="widget-body">
                    <%--    <div class="widget-main no-padding-top">--%>
                           <%-- <div class="col-sm-14 widget-container-span">--%>
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndProjects" runat="server"
                                                Text="Interventions"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="slim-scroll" data-height="200">
                                            <div class="widget-main">
                                                <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged" meta:resourcekey="rblProjectsResource1">
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            <%--</div>--%>
                           <%-- <div class="col-sm-14 widget-container-span">--%>
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndStrObjs" runat="server"
                                                Text="Strategic Objectives" meta:resourcekey="lzeLgndStrObjsResource1"></asp:Localize>
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
                           <%-- </div>
                            <div class="col-sm-14 widget-container-span">--%>
                                <div class="widget-box hidden">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndHumPriorities" runat="server"
                                                Text="Humanitarian Priorities" meta:resourcekey="lzeLgndHumPrioritiesResource1"></asp:Localize>
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
                          <%--  </div>--%>
                      <%--  </div>--%>
                    </div>
                <%--</div>--%>
            </div>
            <div class="col-sm-9 widget-container-span">
               <%-- <div class="widget-box">--%>
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                            <asp:Localize ID="local" runat="server" Text="Indicators"></asp:Localize>
                        </h4>
                        <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                            type="button" class="btn btn-sm btn-yellow pull-right">
                            <i class="icon-save"></i>
                            <asp:Localize ID="localClsuterTargetsSave" runat="server" Text="Save"></asp:Localize>
                        </button>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                    HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                    OnRowDataBound="gvIndicators_RowDataBound" Width="100%" EmptyDataText="No Country Specific Indicators Available To Add In Project. Please contact with your country Cluster Lead." meta:resourcekey="gvIndicatorsResource1">
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
                                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority" meta:resourcekey="TemplateFieldResource1" Visible="false">
                                            <ItemTemplate>
                                                <asp:Image ID="imgObjective" runat="server" AlternateText="O" meta:resourcekey="imgObjectiveResource1" />
                                                <asp:Image ID="imgPriority" runat="server" AlternateText="P" meta:resourcekey="imgPriorityResource1" />
                                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                                            </ItemTemplate>

                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource4">
                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource5">
                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ActivityName" HeaderText="Pillars" ItemStyle-CssClass="testact"
                                            SortExpression="ActivityName">
                                            <ItemStyle CssClass="testact"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Regional Indicator" SortExpression="IsRegional" HeaderStyle-CssClass="hidden"
                                            ItemStyle-CssClass="srpind hidden" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <%# (Boolean.Parse(Eval("IsRegional").ToString())) ? "Yes" : "No"%>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="srpind hidden"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Indicator" SortExpression="IsSRP" HeaderStyle-CssClass="hidden"
                                            ItemStyle-CssClass="srpind hidden" meta:resourcekey="TemplateFieldResource3">
                                            <ItemTemplate>
                                                <%# (Boolean.Parse(Eval("IsSRP").ToString())) ? "Yes" : "No"%>
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="srpind hidden"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Click To Add" HeaderStyle-Width="40px" SortExpression="IndicatorIsAdded">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsAdded" runat="server" Checked='<%# Eval("IndicatorIsAdded") %>'
                                                    CssClass="testcb" meta:resourcekey="cbIsAddedResource1" />
                                            </ItemTemplate>

                                            <HeaderStyle Width="40px"></HeaderStyle>

                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DataName" HeaderText="Indicators" ItemStyle-CssClass="testind"
                                            SortExpression="DataName" ItemStyle-Wrap="true">
                                            <ItemStyle Wrap="True" CssClass="testind"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="TemplateFieldResource5">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cbIsSRP" runat="server" Checked='<%# Eval("IsSRP") %>' CssClass="hidden" meta:resourcekey="cbIsSRPResource1" />
                                            </ItemTemplate>

                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
               <%-- </div>--%>
            </div>
        </div>
    </div>

</asp:Content>
