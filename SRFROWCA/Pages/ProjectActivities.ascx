<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectActivities.ascx.cs" Inherits="SRFROWCA.Pages.ProjectActivities" %>

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
    });
    showHideObj();
</script>
<div class="row">
    <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="hidden checkObj" meta:resourcekey="cblObjectivesResource1" RepeatColumns="3">
    </asp:CheckBoxList>
    <div class="col-sm-12 widget-container-span">
        <div class="widget-header widget-header-small header-color-blue2">
            <h4>
                <asp:Localize ID="local" runat="server" Text="Add/Remove Indicators From Project" meta:resourcekey="localResource1"></asp:Localize>
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
                        HeaderStyle-BackColor="ButtonFace" DataKeyNames="IndicatorId" CssClass="imagetable"
                        OnRowDataBound="gvIndicators_RowDataBound" Width="100%" EmptyDataText="No Country Specific Indicators Available To Add In Project. Please contact with your country Cluster Lead." meta:resourcekey="gvIndicatorsResource1">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                            <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Objective" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:Image ID="imgObjective" runat="server" AlternateText="O" meta:resourcekey="imgObjectiveResource1" />
                                </ItemTemplate>
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Activity" HeaderText="Activity" ItemStyle-CssClass="testact"
                                SortExpression="Activity" meta:resourcekey="BoundFieldResource6">
                                <ItemStyle CssClass="testact"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Added In Project" HeaderStyle-Width="40px" SortExpression="IndicatorIsAdded" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbIsAdded" runat="server" Checked='<%# Eval("IndicatorIsAdded") %>'
                                        CssClass="testcb" meta:resourcekey="cbIsAddedResource1" />
                                </ItemTemplate>

                                <HeaderStyle Width="40px"></HeaderStyle>

                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Indicator" HeaderText="Activity Indicator" ItemStyle-CssClass="testind"
                                SortExpression="Indicator" ItemStyle-Wrap="true" meta:resourcekey="BoundFieldResource7">
                                <ItemStyle Wrap="True" CssClass="testind"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</div>
