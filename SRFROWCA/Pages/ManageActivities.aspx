<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Pages.ManageActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .highlight
        {
            background-color: yellow;
        }
    </style>
    <script type="text/javascript" src="../Scripts/ShowHideObJAndPr.js"></script>
    <script>
        $(function () {
            $('.srpind').parent('tr:contains("Yes")').addClass('highlight');
        });
        showHideObj();
        showHidePriority();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryMain">
            <div class="containerDataEntryProjects">
                <div class="containerDataEntryProjectsInner">
                    <fieldset>
                        <legend>Projects</legend>
                        <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </fieldset>
                    <br />
                </div>
                <div class="containerDataEntryProjectsInner">
                    <fieldset>
                        <legend>Strategic Objectives</legend>
                        <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj">
                        </asp:CheckBoxList>
                    </fieldset>
                    <fieldset>
                        <legend>Humanitarian Priorities</legend>
                        <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr">
                        </asp:CheckBoxList>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                    HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                    OnRowDataBound="gvIndicators_RowDataBound" Width="100%" EmptyDataText="No Country Specific Indicators Available To Add In Project. Please contact with your country Cluster Lead.">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                            ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"></asp:BoundField>
                        <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                            ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" />
                        <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                            ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"></asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Objective & Priority">
                            <ItemTemplate>
                                <asp:Image ID="imgObjective" runat="server" AlternateText="O" />
                                <asp:Image ID="imgPriority" runat="server" AlternateText="P" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                            SortExpression="ActivityName"></asp:BoundField>
                        <asp:TemplateField HeaderText="Country specific Indicator" SortExpression="IsSRP"
                            HeaderStyle-Width="40px" ItemStyle-CssClass="srpind">
                            <ItemTemplate>
                                <%# (Boolean.Parse(Eval("IsSRP").ToString())) ? "Yes" : "No"%></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Added In Project" HeaderStyle-Width="40px" SortExpression="IndicatorIsAdded">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsAdded" runat="server" Checked='<%# Eval("IndicatorIsAdded") %>'
                                    OnCheckedChanged="cbIsAdded_CheckedChanged" AutoPostBack="true" CssClass="testcb" />
                            </ItemTemplate>
                            <ItemStyle Width="2%" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                            SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
                        <asp:TemplateField HeaderText="" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsSRP" runat="server" Checked='<%# Eval("IsSRP") %>' CssClass="hiddenelement" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
