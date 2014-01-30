<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SelectActivities.aspx.cs" Inherits="SRFROWCA.Pages.SelectActivities"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divEmergency" runat="server">
        <div class="contentarea">
            <label>
                <asp:Localize ID="locaEmgCaption" runat="server">Emergency</asp:Localize>
            </label>
            <div>
                <asp:DropDownList ID="ddlEmergency" runat="server" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                    AutoPostBack="True" meta:resourcekey="ddlEmergencyResource1">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                    CssClass="ddlborder" InitialValue="0" Text="Emergency Required" ForeColor="Red"
                    ControlToValidate="ddlEmergency" meta:resourcekey="rfvEmergencyResource1"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMsg">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="text-align: center;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updActivities">
                    <ProgressTemplate>
                        <img src="../images/ajaxlodr.gif" alt="Loading">
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="buttonsdiv">
                <div class="heading">
                </div>
                <div class="buttonright">
                    <asp:Button ID="btnAdd" runat="server" Text="Add Selected" CssClass="button_example"
                        OnClick="btnAdd_Click" meta:resourcekey="btnAddResource1" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete Selected" CssClass="button_example"
                        OnClick="btnDelete_Click" CausesValidation="False" meta:resourcekey="btnDeleteResource1" />
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <asp:Panel CssClass="grid" ID="pnlCust" runat="server" meta:resourcekey="pnlCustResource1">
                    <asp:UpdatePanel ID="pnlUpdate" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvClusters" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                CssClass="imagetable" OnRowDataBound="gvClusters_RowDataBound" OnRowCreated="gvClusters_RowCreated" Width="100%" meta:resourcekey="gvClustersResource1">
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <EmptyDataTemplate>
                                    No Activitis? Please select emergency and office.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Select Your Activities" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:Panel CssClass="group" ID="pnlCluster" runat="server" meta:resourcekey="pnlpnlClusterResource1">
                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                    runat="server" meta:resourcekey="imgCollapsibleResource1" /><span class="gridheader">
                                                        <%#Eval("ClusterName")%></span>
                                            </asp:Panel>
                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server"
                                                meta:resourcekey="pnlOrdersResource1">
                                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" CssClass="grid"
                                                    OnRowCommand="gvActivities_RowCommand" Width="100%" meta:resourcekey="gvActivitiesResource1">
                                                    <RowStyle CssClass="altcolor" />
                                                    <AlternatingRowStyle CssClass="istrow" />
                                                    <Columns>
                                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="rownum" Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" meta:resourcekey="TemplateFieldResource2">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkActivitySelect" runat="server" meta:resourcekey="chkActivitySelectResource1" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Objective" HeaderText="Objective" meta:resourcekey="BoundFieldResource1">
                                                            <ItemStyle Width="25%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="HumanitarianPriority" HeaderText="Priority" meta:resourcekey="BoundFieldResource2">
                                                            <ItemStyle Width="30%" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" meta:resourcekey="BoundFieldResource3">
                                                            <ItemStyle Width="40%" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField meta:resourcekey="TemplateFieldResource3">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="~/images/add_plus.png" CommandName="AddActivity"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" meta:resourcekey="ibtnAddResource1" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False" meta:resourcekey="TemplateFieldResource4">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActivityId" runat="server" Text='<%# Eval("PriorityActivityId") %>'
                                                                    meta:resourcekey="lblActivityIdResource1"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:CollapsiblePanelExtender ID="cpeExpandCollapseActivities" runat="server" TargetControlID="pnlOrders"
                                                CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCluster" CollapseControlID="pnlCluster"
                                                ImageControlID="imgCollapsible" ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png"
                                                Enabled="True" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
