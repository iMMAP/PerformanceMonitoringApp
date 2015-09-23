<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListingMigrate.aspx.cs" Inherits="SRFROWCA.ClusterLead.IndicatorListingMigrate" %>

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
                                        <asp:Button ID="btnMigrate" runat="server" Text="Migrate selected Indicators to 2016" CausesValidation="false"
                                            CssClass="btn btn-danger pull-right" OnClick="btnMigrate_Click" />
                                        <asp:Button ID="btnBackToFramework" runat="server" Text="Back to Framework" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" PostBackUrl="../ClusterLead/IndicatorListing.aspx" Style="margin-right: 5px;" />
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
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <label>Country:</label></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Activity:</label>
                                                            </td>
                                                            <td class="width-30">

                                                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top: 10px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                                <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
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

        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" Width="100%" 
                    PagerSettings-Mode="NumericFirstLast" OnRowDataBound="gvActivity_RowDataBound" 
                    PagerSettings-Position="Bottom" DataKeyNames="ActivityId,IndicatorId"
                    CssClass="imagetable" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging"
                    PageSize="70" ShowHeaderWhenEmpty="true" EmptyDataText="Your filter criteria does not match any indicator!">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" ItemStyle-Width="80px"></asp:BoundField>
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="90px" />
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center"
                              ItemStyle-BackColor="Red">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsSelected" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:TemplateField ItemStyle-Width="4%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>">
                            <ItemTemplate>
                                <asp:Label ID="lblCalcMethod" ToolTip="some text here" runat="server" Text=' <%# Eval("CalculationType")%>'></asp:Label>
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
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
