<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsByCluster.aspx.cs" Inherits="SRFROWCA.Reports.ProjectsByCluster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Report</li>
            <li class="active">By Cluster</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="page-content">
                <table width="100%">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 ">
                                    <div class="widget-box">
                                        <div class="widget-header widget-header-small header-color-orange">
                                            <h6>
                                                <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-primary"
                                                    title="Excel">
                                                    <i class="icon-download"></i>Excel
                                                </button>
                                            </h6>
                                            <div class="widget-toolbar">
                                                <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                            </div>
                                        </div>
                                        <div class="widget-body">
                                            <div class="widget-main">
                                                <div class="row">
                                                    <div class="col-xs-12">
                                                        <div class="row">
                                                            <div class="col-sm-6">
                                                                <div class="widget-box">
                                                                    <div class="widget-header">
                                                                        <h5 class="smaller">
                                                                            General Inforamtion</h5>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Cluster: </span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" CssClass="ddlWidth" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Clusters" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Country:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlCountry" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Country" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Organization:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="250%" DropDownBoxBoxHeight="300px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Organization" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>From:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtFromDate" runat="server" Width="100px"></asp:TextBox><span>(mm/dd/yyyy)</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>To:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox><span>(mm/dd/yyyy)</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <%-- <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbRegional" runat="server" Text="Regional Ind" />
                                                                                            <asp:CheckBox ID="cbCountry" runat="server" Text="Country Ind" /></span>
                                                                                    </td>
                                                                                </tr>--%>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <div class="widget-box light-border">
                                                                    <div class="widget-header">
                                                                        <h5 class="smaller">
                                                                            Logframe Filters</h5>
                                                                    </div>
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Project Code:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProjectCode" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Project Title:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProjectTitle" runat="server"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <span>Funding Status:</span>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlFundingStatus" runat="server" OnSelectedIndexChanged="ddlFundingStatus_SelectedIndexChanged">
                                                                                            <asp:ListItem Text="Select FTS Status" Value="0"></asp:ListItem>
                                                                                            <asp:ListItem Text="Commitment" Value="1"></asp:ListItem>
                                                                                            <asp:ListItem Text="Paid contribution" Value="2"></asp:ListItem>
                                                                                            <asp:ListItem Text="Pledge" Value="3"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                                                            class="btn btn-primary" />
                                                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="space">
                </div>
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
                                                                                        <%#Eval("ClusterName")%></span>
                                                                            </asp:Panel>
                                                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"
                                                                                    ShowHeader="true" OnRowCommand="gvActivities_RowCommand" Width="100%">
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="LocationName" HeaderText="Country" />
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
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
