<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OutputIndicatorReport.aspx.cs" Inherits="SRFROWCA.Anonymous.OutputIndicatorReport" %>

<asp:Content ID="cntClusterReports" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
    <style>
        .ddlWidth {
            width: 270px;
        }
    </style>

    <script type="text/javascript">

        function resetAll() {

            document.getElementById('<%=ddlCountry.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=ddlCluster.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=ddlMonth.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=cbIncludeRegional.ClientID%>').checked = true;

            return false;
        }

    </script>

</asp:Content>

<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <asp:UpdatePanel ID="pnlOutputReportData" runat="server">
            <ContentTemplate>
                <div style="text-align: center;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlOutputReportData"
                        DynamicLayout="true">
                        <ProgressTemplate>
                            <img src="../assets/orsimages/ajaxlodr.gif" alt="Loading">
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div id="divMsg"></div>

                <table width="100%">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 ">
                                    <div class="widget-box">
                                        <div class="widget-header widget-header-small header-color-blue2" style="padding-left: 0px;">
                                            <h6>
                                                <button runat="server" id="btnExportToExcel" onserverclick="btnExportToExcel_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                                    title="Excel">
                                                    <i class="icon-download"></i>Excel
                                                </button>

                                            </h6>
                                        </div>
                                        <div class="widget-body">
                                            <div class="widget-main">
                                                <table border="0" style="width: 95%; margin: 0px 10px 0px 20px">
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270">
                                                            </asp:DropDownList>
                                                        </td>

                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>Month:</td>
                                                        <td>
                                                            <cc:DropDownCheckBoxes UseButtons="False" AddJQueryReference="True" CssClass="ddlWidth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server" ID="ddlMonth">
                                                                <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight=""></Style>
                                                                <Texts SelectBoxCaption="Select" />
                                                            </cc:DropDownCheckBoxes></td>

                                                        <td>
                                                            <label>
                                                                Indicator:</label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtIndicatorName" runat="server" Width="270px"></asp:TextBox>
                                                        </td>

                                                        <td style="text-align: right;">
                                                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                            <%--<asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" CssClass="btn btn-primary" CausesValidation="False"/>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td colspan="4">
                                                            <asp:CheckBox ID="cbIncludeRegional" runat="server" Text="Show Regional Indicators" Checked="True" meta:resourcekey="cbIncludeRegionalResource1" /></td>
                                                    </tr>


                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="table-responsive">
                    <div style="overflow-x: auto; width: 100%">
                        <asp:GridView ID="gvClusterReports" Width="100%" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False"
                            AllowSorting="True" DataKeyNames="SiteLanguageId" ShowHeader="true" OnRowDataBound="gvClusterReports_RowDataBound"
                            OnSorting="gvClusterReports_Sorting" CssClass="imagetable" AllowPaging="true" AllowCustomPaging="true" PageSize="50"
                            OnPageIndexChanging="gvClusterReports_PageIndexChanging">
                            <PagerSettings Mode="NumericFirstLast" />
                            <EmptyDataTemplate>
                                Your filter criteria does not match any record in database!
                            </EmptyDataTemplate>
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="4px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Image ID="imgRind" runat="server" />
                                        <asp:Image ID="imgCind" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4"></asp:BoundField>
                                <asp:BoundField ItemStyle-Width="10%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5"></asp:BoundField>
                                <asp:BoundField ItemStyle-Width="30%" DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6"></asp:BoundField>
                                <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource8"></asp:BoundField>
                                <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month"></asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="7%" HeaderText="Target" ItemStyle-HorizontalAlign="Right" SortExpression="CurrentTarget">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("Target")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="7%" HeaderText="Monthly Achieved" ItemStyle-HorizontalAlign="Right" SortExpression="MonthlyAchieved">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountryAchieved" runat="server" Text=' <%# Eval("MonthlyAchieved")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="7%" HeaderText="Running Value" ItemStyle-HorizontalAlign="Right" SortExpression="RunningValue">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountrySum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CalculationMethod" HeaderText="Calculation Method" SortExpression="CalculationMethod" />
                                <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                    <HeaderStyle CssClass="hidden"></HeaderStyle>

                                    <ItemStyle CssClass="hidden"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                    <ItemStyle CssClass="hidden"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="ReportedBy" HeaderText="Reported By" SortExpression="ReportedBy" ItemStyle-Width="7%"></asp:BoundField>
                                <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" ItemStyle-Width="7%"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCountry" />
                <asp:AsyncPostBackTrigger ControlID="ddlCluster" />
                <asp:AsyncPostBackTrigger ControlID="ddlMonth" />
                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            </Triggers>
        </asp:UpdatePanel>


    </div>
</asp:Content>
