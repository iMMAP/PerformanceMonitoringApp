<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CountryIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.CountryIndicators" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>

<asp:Content ID="cntHeadCountryIndicators" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-content">
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2" style="padding-left: 0px;">
                                    <h6>
                                        <%--<button runat="server" id="btnExportPDF" onserverclick="ExportToPDF" class="btn btn-yellow" causesvalidation="false"
                                            title="PDF">
                                            <i class="icon-download"></i>PDF                                       
                                        </button>--%>

                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportToExcel_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                       
                                        </button>

                                        <asp:Button ID="btnAddIndicator" runat="server" OnClick="btnAddIndicator_Click" Text="Add Indicator" CausesValidation="False"
                                            CssClass="btn btn-yellow pull-right" meta:resourcekey="btnAddIndicatorResource1" />
                                    </h6>
                                </div>
                                <div id="divMsg">
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 95%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:" meta:resourcekey="lblCountryResource1"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270px" meta:resourcekey="ddlCountryResource1">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270px" meta:resourcekey="ddlClusterResource1">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>Year:
                                                    <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChnaged">
                                                        <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbIncludeRegional" runat="server" Text="Show Regional Indicators" Checked="True" AutoPostBack="true" OnCheckedChanged="cbIncudeRegional_CheckedChanged" meta:resourcekey="cbIncludeRegionalResource1" />
                                                </td>
                                                <td>
                                                    <label>
                                                        Indicator:</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIndicatorName" runat="server" Width="270px" meta:resourcekey="txtIndicatorNameResource1"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right;">

                                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                    <asp:Button ID="btnReset" runat="server" OnClick="btnReset_Click" Text="Reset" CssClass="btn btn-primary" CausesValidation="False" />
                                                </td>
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
                <cc2:PagingGridView ID="gvClusterIndicators" Width="100%" runat="server" ShowHeaderWhenEmpty="true"
                    AutoGenerateColumns="False" AllowSorting="True"
                    OnRowDataBound="gvClusterIndicators_RowDataBound" PageSize="50"
                    OnPageIndexChanging="gvClusterindicators_PageIndexChanging"
                    OnSorting="gvClusterIndicators_Sorting" ShowHeader="true"
                    OnRowCommand="gvClusterIndicators_RowCommand" CssClass="imagetable"
                    meta:resourcekey="gvClusterIndicatorsResource1">
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
                        <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1" />
                        <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2" />
                        <asp:BoundField Visible="false" DataField="ClusterIndicatorId" HeaderText="ID" SortExpression="ClusterIndicatorId" meta:resourcekey="BoundFieldResource3" />
                        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5" />
                        <asp:BoundField ItemStyle-Width="48%" DataField="Indicator" HeaderText="Indicator" HtmlEncode="false" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Target" HeaderText="Target" SortExpression="Target" meta:resourcekey="BoundFieldResource7" />
                        <asp:BoundField ItemStyle-Width="10%" DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource8" />
                        <asp:BoundField ItemStyle-Width="5%" DataField="CalculationMethod" HeaderText="Calculation" SortExpression="CalculationMethod" />

                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' ToolTip="Edit Indicator" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Del">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("EmergencyLocationId") %>' meta:resourcekey="lblCountryIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("EmergencyClusterId") %>' meta:resourcekey="lblClusterIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource7">
                            <ItemTemplate>
                                <asp:Label ID="lblIndAlternate" runat="server" HtmlEncode="false" Text='<%# Eval("IndicatorAlt") %>' meta:resourcekey="lblIndAlternateResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource8">
                            <ItemTemplate>
                                <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitID") %>' meta:resourcekey="lblUnitIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc2:PagingGridView>
            </div>
        </div>
    </div>
</asp:Content>
