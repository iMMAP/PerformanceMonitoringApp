<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ManageActivities.aspx.cs" Inherits="SRFROWCA.Pages.ManageActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                test
            </div>
        </div>
    </div>
    <asp:Wizard ID="Wizard1" runat="server" OnNextButtonClick="wzrdReport_NextButtonClick"
        OnPreviousButtonClick="wzrdReport_PreviousButtonClick" OnFinishButtonClick="wzrdReport_FinishButtonClick">
        <WizardSteps>
            <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1">
                <div class="containerDataEntryGrid">
                    <div class="tablegrid">
                        <table>
                            <tr>
                                <td>
                                    Projects:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProjects" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="cblLocation" runat="server" RepeatColumns="4">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:WizardStep>
            <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                    <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        HeaderStyle-BackColor="ButtonFace" DataKeyNames="PriorityActivityId" CssClass="imagetable"
                        Width="100%" meta:resourcekey="gvActivitiesResource1">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
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
                </div>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
