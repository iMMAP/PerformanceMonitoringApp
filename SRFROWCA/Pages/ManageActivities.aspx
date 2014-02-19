<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Pages.ManageActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>My Projects</legend>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                    </asp:RadioButtonList>
                    <br />
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
                        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="PriorityActivityId" CssClass="imagetable"
                            Width="100%" meta:resourcekey="gvActivitiesResource1">
                            <HeaderStyle BackColor="Control"></HeaderStyle>
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkActivitySelect" runat="server" meta:resourcekey="chkActivitySelectResource1" />
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PriorityActivityId" HeaderText="PriorityActivityId"></asp:BoundField>
                                <asp:BoundField DataField="ClusterName" HeaderText="Cluster"></asp:BoundField>
                                <asp:BoundField DataField="Objective" HeaderText="Objective"></asp:BoundField>
                                <asp:BoundField DataField="HumanitarianPriority" HeaderText="HumanitarianPriority" />
                                <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="450px">
                                    <ItemStyle Width="450px"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:WizardStep>
                <asp:WizardStep ID="WizardStep2" runat="server" Title="">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBoxList ID="cblLocation" runat="server" RepeatColumns="4">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </asp:WizardStep>
                <asp:WizardStep ID="wz3" runat="server">
                    <div id="Div1" style="overflow-x: auto; width: 100%;">
                        <asp:GridView ID="gvActLoc" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                            HeaderStyle-BackColor="ButtonFace" DataKeyNames="PriorityActivityId" CssClass="imagetable"
                            Width="100%" meta:resourcekey="gvActivitiesResource1">
                            <HeaderStyle BackColor="Control"></HeaderStyle>
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:BoundField DataField="ClusterName" HeaderText="Cluster"></asp:BoundField>
                                <asp:BoundField DataField="Objective" HeaderText="Objective"></asp:BoundField>
                                <asp:BoundField DataField="HumanitarianPriority" HeaderText="HumanitarianPriority" />
                                <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="450px">
                                    <ItemStyle Width="450px"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Bale Annual Target">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBale" runat="server" Text=""></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Banwa Annual Target">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtBanwa" runat="server" Text=""></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
    </div>
</asp:Content>
