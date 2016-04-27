<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Anonymous.AllData" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>

    <style>
        .mycheckbox {
            margin-left: 20px;
        }

        .ddlWidth {
            width: 100%;
        }
    </style>
    <script type="text/javascript">

        $(function () {
            $(".classsearchcriteriacustomreport").tooltip({
                show: {
                    effect: "slideDown",
                    delay: 250
                }
            });
        });
        $(document).ready(function () {
            $.widget("ui.tooltip", $.ui.tooltip, {
                options: {
                    content: function () {
                        return $(this).prop('title');
                    }
                }
            });

            $('.tooltip1').tooltip();
        });
    </script>
</asp:Content>
<asp:Content ID="allDataContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="pnlAllData" runat="server">
        <ContentTemplate>
            <div class="page-content">
                <div style="text-align: center;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAllData"
                        DynamicLayout="true">
                        <ProgressTemplate>
                            <img src="../assets/orsimages/ajaxlodr.gif" alt="Loading">
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 ">
                                    <div class="widget-box">
                                        <div class="widget-header widget-header-small header-color-blue2">
                                            <h6>
                                                <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-yellow"
                                                    title="Excel">
                                                    <i class="icon-download"></i>Excel
                                                </button>
                                                <button runat="server" id="btnExportToCSV" onserverclick="ExportToCSV" class="width-10 btn btn-sm btn-yellow"
                                                    title="CSV">
                                                    <i class="icon-download"></i>CSV
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
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>Cluster:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" CssClass="ddlWidth" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Clusters" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <label>
                                                                                            <asp:Label ID="lblCluster" runat="server" Text="" Visible="false"></asp:Label></label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>Month:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddl_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="120px" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight="200px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Month" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:DropDownList ID="ddlFrameworkYear" runat="server">
                                                                                            <asp:ListItem Text="2016" Value="2016" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                                                            <asp:ListItem Text="2015" Value="2015" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbOPSProjects" runat="server" Text="HRP Projects" />
                                                                                            <asp:CheckBox ID="cbORSProjects" runat="server" Text="Non HRP Projects" /></span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>
                                                                                            Organization:<label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="250%" DropDownBoxBoxHeight="300px"></Style>
                                                                                            <Texts SelectBoxCaption="Select Organization" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <label>
                                                                                            <asp:Label ID="lblOrganization" runat="server" Text="" Visible="false"></asp:Label></label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>Projects:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight="400%"></Style>
                                                                                            <Texts SelectBoxCaption="Select Project" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbValidated" runat="server" Text="Validated" />
                                                                                            <asp:CheckBox ID="cbNotValidated" runat="server" Text="Not Validated" /></span>


                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-sm-4">
                                                                <div class="widget-box no-border">
                                                                    <div class="widget-body">
                                                                        <div class="widget-main padding-6">
                                                                            <table class="width-100">
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>Country:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlCountry" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                                                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AddJQueryReference="True"
                                                                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Country" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                        <asp:Label ID="lblCountry" runat="server" Text="" Visible="false"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label>Admin1:</label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged"
                                                                                            AutoPostBack="true" AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1"
                                                                                            UseButtons="False" UseSelectAllNode="True">
                                                                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                                                                            <Texts SelectBoxCaption="Select Admin1" />
                                                                                        </cc:DropDownCheckBoxes>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <span>
                                                                                            <asp:CheckBox ID="cbFunded" runat="server" Text="Funded" />
                                                                                            <asp:CheckBox ID="cbNotFunded" runat="server" Text="Not Funded" /></span>
                                                                                        <asp:CheckBox ID="cbCPActivity" runat="server" Text="CP Activity:" TextAlign="Left" CssClass="mycheckbox"
                                                                                            OnCheckedChanged="ddl_SelectedIndexChanged" AutoPostBack="true" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td></td>
                                                                                    <td>
                                                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                                                            class="btn btn-primary btn-sm" />
                                                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-sm" />
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

                <%--<div class="col-xs-12 col-sm-12">--%>
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>Search Criteria
                        </h6>
                        <div class="widget-toolbar">
                            <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                        </div>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <small>
                                <div id="divSearchCriteria" runat="server"></div>
                            </small>
                        </div>
                    </div>
                </div>
                <%--</div>--%>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">

                        <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="imagetable"
                            AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                            EnableViewState="false" AllowSorting="True" AllowPaging="true" PageSize="40" AllowCustomPaging="true"
                            ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging" EmptyDataText="Your filter criteria does not match any record in database!">
                            <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:BoundField DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" />
                                <asp:BoundField DataField="Organization" HeaderText="Proj Owner" SortExpression="Organization" />
                                <asp:BoundField DataField="ReportingOrganization" HeaderText="Reporting Org" SortExpression="ReportingOrganization" />
                                <asp:BoundField DataField="ProjectCode" HeaderText="Project" SortExpression="ProjectCode" />
                                <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" />
                                <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective" />
                                <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                                <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                                <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
                                <asp:BoundField DataField="Admin1" HeaderText="Admin1" SortExpression="Admin1" />
                                <asp:BoundField DataField="Admin2" HeaderText="Admin2" SortExpression="Admin2" />
                                <asp:TemplateField HeaderText="Target Total" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("TargetTotal")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Achieved Total" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAchieved" runat="server" Text=' <%# Eval("AchievedTotal")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Running Value" ItemStyle-HorizontalAlign="Right" SortExpression="RunningValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountrySum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CalculationMethod" HeaderText="Calculation Method" SortExpression="CalculationMethod" />
                                <asp:TemplateField HeaderText="Appr" SortExpression="IsApproved">
                                    <ItemTemplate><%# (Boolean.Parse(Eval("IsApproved").ToString())) ? "Yes" : "No" %></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </cc2:PagingGridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
