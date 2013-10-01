<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SelectActivities.aspx.cs" Inherits="SRFROWCA.Pages.SelectActivities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" class="label1">
        <tr>
            <td>
                <span class="lablel1">Emergency:</span>
                <asp:DropDownList ID="ddlEmergency" runat="server" Width="200px" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                    CssClass="ddlborder" InitialValue="0" Text="*" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
            </td>
            <td>
                <span class="lablel1">Office:</span>
                <asp:DropDownList ID="ddlOffice" runat="server" Width="200px" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvOffice" runat="server" ErrorMessage="Select Office"
                    InitialValue="0" Text="*" ControlToValidate="ddlOffice"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="signupheading2">
                        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <div style="text-align: center;">
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updActivities"
                                DynamicLayout="true">
                                <ProgressTemplate>
                                    <img src="../images/ajaxlodr.gif" alt="Loading">
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </td>
                    <td align="right">
                        <asp:Button ID="btnAdd" runat="server" Text="Add Selected" CssClass="buttonA" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete Selected" CssClass="buttonA"
                            OnClick="btnDelete_Click" CausesValidation="false" />
                    </td>
                </tr>
            </table>
            <div id="dlg" class="dialog" style="width: 100%">
                <div class="gridheader" style="cursor: default">
                    <div class="outer">
                        <div class="inner">
                            <div class="content">
                                <h2>
                                    Select Your Activities</h2>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="gridbody">
                    <div class="outer">
                        <div class="inner">
                            <div class="content">
                                <asp:Panel CssClass="grid" ID="pnlCust" runat="server">
                                    <asp:UpdatePanel ID="pnlUpdate" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvClusters" runat="server" AutoGenerateColumns="false" ShowHeader="False"
                                                OnRowDataBound="gvClusters_RowDataBound" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                    runat="server" /><span class="gridheader">
                                                                        <%#Eval("ClusterName")%></span>
                                                            </asp:Panel>
                                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="false" CssClass="grid"
                                                                    ShowHeader="true" OnRowCommand="gvActivities_RowCommand" Width="100%">
                                                                    <RowStyle CssClass="row" />
                                                                    <AlternatingRowStyle CssClass="altrow" />
                                                                    <Columns>
                                                                        <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%">
                                                                            <ItemTemplate>
                                                                                <%# Container.DataItemIndex + 1 %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Select"  ItemStyle-Width="4%">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkActivitySelect" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="IndicatorName" HeaderText="Indicator" ItemStyle-Width="40%" />
                                                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="50%" />                                                                        
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ibtnAdd" runat="server" ImageUrl="~/images/add_plus.png" CommandName="AddActivity"
                                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField Visible="false">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblActivityId" runat="server" Text='<%# Eval("IndicatorActivityId") %>'></asp:Label>
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
                        </div>
                    </div>
                </div>
                <div class="gridfooter">
                    <div class="outer">
                        <div class="inner">
                            <div class="content">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
