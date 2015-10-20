<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListingMigrate.aspx.cs" Inherits="SRFROWCA.ClusterLead.IndicatorListingMigrate" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .modalDialog {
            width: 500px;
            position: relative;
            margin: 10% auto;
            padding: 5px 20px 13px 20px;
            border-radius: 2px;
            background: #ffffff;
        }
    </style>
    <script>
        $(function () {
            $('.tooltip2').tooltip();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-content">
        <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
            <tr>
                <td class="signupheading2" colspan="3">
                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divMsg">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <asp:Button ID="btnMigrate" runat="server" Text="Migrate selected Indicators to 2016" CausesValidation="False"
                                            CssClass="btn btn-sm btn-danger pull-right" OnClick="btnMigrate_Click" meta:resourcekey="btnMigrateResource1" />
                                        <asp:Button ID="btnBackToFramework" runat="server" Text="Back to Framework" CausesValidation="False"
                                            CssClass="btn btn-sm btn-yellow pull-right" PostBackUrl="../ClusterLead/IndicatorListing.aspx" Style="margin-right: 5px;" meta:resourcekey="btnBackToFrameworkResource1" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%;">
                                                        <tr>
                                                            <td class="width-20">
                                                                <asp:Label ID="lblCountry" runat="server" Text="Country:" meta:resourcekey="lblCountryResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>

                                                            <td>
                                                                <asp:Label ID="lblCluster" runat="server" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80" meta:resourcekey="ddlClusterResource1">
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <asp:Label ID="lblObjective" runat="server" Text="Objective:" meta:resourcekey="lblObjectiveResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="True" CssClass="width-80" meta:resourcekey="ddlObjectiveResource1">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <asp:Label ID="lblActivity" runat="server" Text="Activity:" meta:resourcekey="lblActivityResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">

                                                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-80" meta:resourcekey="ddlActivityResource1">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top: 10px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                                <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnResetResource1" />
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
                </td>
            </tr>
        </table>
        <div id="divMigrateMessage" runat="server" class="alert alert-block alert-danger" visible="false">
            <asp:Label ID="lblMigrateMessage" runat="server" Text="test"></asp:Label>
        </div>
        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" Width="100%"
                    PagerSettings-Mode="NumericFirstLast" OnRowDataBound="gvActivity_RowDataBound"
                    PagerSettings-Position="Bottom" DataKeyNames="ActivityId,IndicatorId"
                    CssClass="imagetable" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging"
                    PageSize="70" ShowHeaderWhenEmpty="True" EmptyDataText="Your filter criteria does not match any indicator!" meta:resourcekey="gvActivityResource1">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" ItemStyle-Width="80px" meta:resourcekey="BoundFieldResource1">
                            <ItemStyle Width="80px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource2">
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="90px" meta:resourcekey="BoundFieldResource3">
                            <ItemStyle Width="90px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" meta:resourcekey="BoundFieldResource4" />
                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" meta:resourcekey="BoundFieldResource5" />
                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-BackColor="#ffb2b2" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsSelected" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>' meta:resourcekey="cbIsSelectedResource1" />
                            </ItemTemplate>

                            <ItemStyle HorizontalAlign="Center" BackColor="#FFB2B2"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource6" />
                        <asp:TemplateField ItemStyle-Width="4%" HeaderText="Calculation Method" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:Label ID="lblCalcMethod" runat="server" Text='<%# Eval("CalculationType") %>' meta:resourcekey="lblCalcMethodResource1"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="4%"></ItemStyle>
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
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />

                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
