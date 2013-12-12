<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SelectActivities.aspx.cs" Inherits="SRFROWCA.Pages.SelectActivities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="graybar">
            Select Your Emergency & Office.
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <div class="outer">
                    <div class="row ">
                        <label>
                            Emergency
                        </label>
                        <div>
                            <asp:DropDownList ID="ddlEmergency" runat="server" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                CssClass="ddlborder" InitialValue="0" Text="Emergency Required" ForeColor="Red"
                                ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <label>
                            Office
                        </label>
                        <div>
                            <asp:DropDownList ID="ddlOffice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvOffice" runat="server" ErrorMessage="Select Office"
                                InitialValue="0" Text="Office Required" ForeColor="Red" ControlToValidate="ddlOffice"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
        </div>
        <div class="graybarcontainer">
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
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updActivities"
                    DynamicLayout="true">
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
                        OnClick="btnAdd_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete Selected" CssClass="button_example"
                        OnClick="btnDelete_Click" CausesValidation="false" />
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <asp:Panel CssClass="grid" ID="pnlCust" runat="server">
                    <asp:UpdatePanel ID="pnlUpdate" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvClusters" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                                ShowHeaderWhenEmpty="true" CssClass="imagetable" OnRowDataBound="gvClusters_RowDataBound"
                                Width="100%">
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <EmptyDataTemplate>
                                    No Activitis? Please select emergency and office.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="Select Your Activities">
                                        <ItemTemplate>
                                            <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                    runat="server" /><span class="gridheader">
                                                        <%#Eval("ClusterName")%></span>
                                            </asp:Panel>
                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="false" CssClass="grid"
                                                    ShowHeader="true" OnRowCommand="gvActivities_RowCommand" Width="100%">
                                                    <RowStyle CssClass="altcolor" />
                                                    <AlternatingRowStyle CssClass="istrow" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Select" ItemStyle-Width="2%">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkActivitySelect" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Objective" HeaderText="Objective" ItemStyle-Width="38%" />
                                                        <asp:BoundField DataField="HumanitarianPriority" HeaderText="Priority" ItemStyle-Width="10%" />
                                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="40%" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="~/images/add_plus.png" CommandName="AddActivity"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblActivityId" runat="server" Text='<%# Eval("PriorityActivityId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="pnlOrders"
                                                CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
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