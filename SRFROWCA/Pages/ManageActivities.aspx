<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Pages.ManageActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="../Scripts/ShowHideObJAndPr.js"></script>
    <script>
        showHideObj();
        showHidePriority();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryMain">
            <div class="containerDataEntryProjects">
                <div class="containerDataEntryProjectsInner">
                    <div class="graybar">
                        My Projects
                    </div>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                    </asp:RadioButtonList>
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
            <asp:Wizard ID="Wizard1" runat="server" OnNextButtonClick="wzrdReport_NextButtonClick"
                OnPreviousButtonClick="wzrdReport_PreviousButtonClick" OnFinishButtonClick="wzrdReport_FinishButtonClick"
                DisplaySideBar="false">
                <WizardSteps>
                    <asp:WizardStep ID="WizardStep1" runat="server" Title="">
                        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                OnRowDataBound="gvIndicators_RowDataBound" Width="100%">
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
                                    <asp:TemplateField HeaderText="SRP" SortExpression="IsSRP">
                                        <ItemTemplate>
                                            <%# (Boolean.Parse(Eval("IsSRP").ToString())) ? "Yes" : "No"%></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource2"
                                        SortExpression="IndicatorIsAdded">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsAdded" runat="server" Checked='<%# Eval("IndicatorIsAdded") %>'
                                                CssClass="testcb" />
                                        </ItemTemplate>
                                        <ItemStyle Width="2%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                        SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
                                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="hiddenelement">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbIsSRP" runat="server" Checked='<%# Eval("IsSRP") %>'
                                                CssClass="testcb" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" runat="server" Title="">
                        <div>
                            <fieldset>
                                <legend>Admin 1 Locaitons</legend>
                                <asp:CheckBoxList ID="cblAdmin1" runat="server" RepeatColumns="6">
                                </asp:CheckBoxList>
                            </fieldset>
                        </div>
                        <div>
                            <fieldset>
                                <legend>Admin 2 Locations</legend>
                                <asp:CheckBoxList ID="cblAdmin2" runat="server" RepeatColumns="6">
                                </asp:CheckBoxList>
                            </fieldset>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wz3" runat="server">
                        <div id="Div1" style="overflow-x: auto; width: 100%;">
                            <asp:GridView ID="gvTargts" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable2"
                                Width="100%" OnRowDataBound="gvTargts_RowDataBound">
                                <HeaderStyle BackColor="Control"></HeaderStyle>
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"></asp:BoundField>
                                    <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                        ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                    </asp:BoundField>
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
                                    <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-CssClass="testind"
                                        SortExpression="DataName" ItemStyle-Wrap="true"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
        </div>
    </div>
</asp:Content>
