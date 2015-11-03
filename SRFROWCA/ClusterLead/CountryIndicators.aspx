<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CountryIndicators.aspx.cs"
    Inherits="SRFROWCA.ClusterLead.CountryIndicators" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

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

                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportToExcel_ServerClick" class="btn btn-sm btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                       
                                        </button>

                                        <asp:Button ID="btnAddIndicator" runat="server" OnClick="btnAddIndicator_Click" Text="Add Indicator" 
                                            CausesValidation="False" Enabled="false"
                                            CssClass="btn btn-sm btn-yellow pull-right" meta:resourcekey="btnAddIndicatorResource1" />
                                    </h6>
                                </div>
                                <div id="divMsg">
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 100%; margin: 0 auto;">
                                            <tr>
                                                <td style="width:60px">
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:" meta:resourcekey="lblCountryResource1"></asp:Label>
                                                </td>
                                                <td style="width:230px">
                                                    <asp:DropDownList ID="ddlCountry" runat="server" Width="200px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList>
                                                </td>

                                                <td style="width:60px">
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                </td>
                                                <td style="width:270px">
                                                    <asp:DropDownList ID="ddlCluster" runat="server" AutoPostBack="True" Width="260px"
                                                        OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlClusterResource1">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbIncludeRegional" runat="server" CssClass="ace ace-checkbox-2"
                                                        Checked="True" AutoPostBack="True" OnCheckedChanged="cbIncudeRegional_CheckedChanged" />
                                                    <asp:Localize ID="localCBIsRegCap" runat="server" Text="<span class='label-sm'>Show Regional Indicators</span>" meta:resourcekey="cbIncludeRegionalResource1"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblYear" runat="server" Text="Year:" meta:resourcekey="lblYearResource1"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_SelectedIndexChanged" meta:resourcekey="ddlFrameworkYearResource1">
                                                        <asp:ListItem Text="2016" Value="12" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                        <asp:ListItem Text="2015" Value="11" meta:resourcekey="ListItemResource4"></asp:ListItem>
                                                    </asp:DropDownList>
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
                <cc2:PagingGridView ID="gvClusterIndicators" Width="100%" runat="server" ShowHeaderWhenEmpty="True"
                    AutoGenerateColumns="False" AllowSorting="True"
                    OnRowDataBound="gvClusterIndicators_RowDataBound" PageSize="50"
                    OnPageIndexChanging="gvClusterindicators_PageIndexChanging"
                    OnSorting="gvClusterIndicators_Sorting"
                    OnRowCommand="gvClusterIndicators_RowCommand" CssClass="imagetable" AllowPaging="True" meta:resourcekey="gvClusterIndicatorsResource1">
                    <EmptyDataTemplate>
                        Your filter criteria does not match any record in database!
                    </EmptyDataTemplate>

                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>

                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2">
                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField Visible="false" DataField="ClusterIndicatorId" HeaderText="ID" SortExpression="ClusterIndicatorId" meta:resourcekey="BoundFieldResource3" />
                        <asp:TemplateField ItemStyle-Width="20px" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="20px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="15%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="40%" DataField="Indicator" HeaderText="Indicator" HtmlEncode="false" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource7"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="50px" DataField="CalculationMethod" HeaderText="Calculation" SortExpression="CalculationMethod" meta:resourcekey="BoundFieldResource8"></asp:BoundField>
                        <asp:TemplateField ItemStyle-Width="150px" HeaderStyle-Width="150px" HeaderText="Target" SortExpression="Target" ItemStyle-HorizontalAlign="Right" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:Label ID="lblIndTarget" runat="server" Text='<%# Eval("Target") %>' meta:resourcekey="lblIndTargetResource1"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Edit" meta:resourcekey="TemplateFieldResource4">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png" Visible="false"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' ToolTip="Edit Indicator" meta:resourcekey="btnEditResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Del" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png" Visible="false"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' ToolTip="Delete" meta:resourcekey="btnDeleteResource1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("EmergencyLocationId") %>' meta:resourcekey="lblCountryIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource7">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("EmergencyClusterId") %>' meta:resourcekey="lblClusterIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource8">
                            <ItemTemplate>
                                <asp:Label ID="lblIndAlternate" runat="server" HtmlEncode="false" Text='<%# Eval("IndicatorAlt") %>' meta:resourcekey="lblIndAlternateResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource9">
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
