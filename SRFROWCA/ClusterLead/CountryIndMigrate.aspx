<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountryIndMigrate.aspx.cs" Inherits="SRFROWCA.ClusterLead.CountryIndMigrate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2" style="padding-left: 0px;">
                                    <h6>
                                        <asp:Button ID="btnMigrate2016" runat="server" Text="Migrate Selected Indicators To 2016" CausesValidation="false"
                                            CssClass="btn btn-danger pull-right" OnClick="btnMigrate_Click"/>
                                    </h6>
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
                                                <td></td>
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
                <cc2:PagingGridView ID="gvClusterIndicators" Width="100%" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="SiteLanguageId"
                    OnRowDataBound="gvClusterIndicators_RowDataBound" PageSize="50" OnPageIndexChanging="gvClusterindicators_PageIndexChanging" OnSorting="gvClusterIndicators_Sorting" ShowHeader="true" OnRowCommand="gvClusterIndicators_RowCommand" CssClass=" table-striped table-bordered table-hover" meta:resourcekey="gvClusterIndicatorsResource1">
                    <EmptyDataTemplate>
                        Your filter criteria does not match any record in database!
                    </EmptyDataTemplate>
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
                        <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="48%" DataField="Indicator" HeaderText="Indicator" HtmlEncode="false" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6">
                            <ItemStyle Width="48%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center"
                              ItemStyle-BackColor="Red">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsSelected" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource8">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="False"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' Text="Edit" meta:resourcekey="btnEditResource1"></asp:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle Width="5%"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource4">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="False"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' meta:resourcekey="btnDeleteResource1"></asp:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle Width="5%"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
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
