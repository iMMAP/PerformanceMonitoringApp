<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectsByCountry.aspx.cs" Inherits="SRFROWCA.Reports.ProjectsByCountry" %>
 <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Report</li>
            <li class="active">By Country</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
<asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="page-content">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 widget-container-span">
                        <div class="widget-box">
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
                                                    <img src="../assets/orsimages/ajaxlodr.gif" alt="Loading">
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="dlg" class="dialog" style="width: 100%">
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
                                                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/assets/orsimages/plus.png"
                                                                                    Style="margin-right: 5px;" runat="server" /><span class="gridheader">
                                                                                        <%#Eval("LocationName")%></span>
                                                                            </asp:Panel>
                                                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"
                                                                                    ShowHeader="true" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" />
                                                                                        <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" />
                                                                                        <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                                                                            SortExpression="ProjectTitle" />
                                                                                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" />
                                                                                        <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Wrap="true" ItemStyle-Width="150px"
                                                                                            SortExpression="Contact" />
                                                                                        <asp:TemplateField>
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lnkVieDetails" runat="server" Text="View" CommandName="ViewProject"
                                                                                                    CommandArgument='<%# Eval("ProjectId") %>' /></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </asp:Panel>
                                                                            <asp:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="pnlOrders"
                                                                                CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                                AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                                ExpandedImage="~/assets/orsimages/minus.png" CollapsedImage="~/assets/orsimages/plus.png"
                                                                                ExpandDirection="Vertical" />
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
