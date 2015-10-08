<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListing2016.aspx.cs" Inherits="SRFROWCA.ClusterLead.IndicatorListing2016" %>

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
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <asp:Button ID="btnBackToFramework" runat="server" Text="Back to Framework" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnBackToFramework_Click" Style="margin-right: 5px;" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%; margin: 0 auto;">
                                                        <tr>
                                                            <td align="right">
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-40">
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80" AutoPostBack="true" OnSelectedIndexChanged="ddl_IndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="right">
                                                                <label>Country:</label></td>
                                                            <td class="width-40">
                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" OnSelectedIndexChanged="ddl_IndexChanged"
                                                                     AutoPostBack="true" meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>
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
                    PagerSettings-Mode="NumericFirstLast" 
                    PagerSettings-Position="Bottom" 
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
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:TemplateField ItemStyle-Width="4%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>">
                            <ItemTemplate>
                                <asp:Label ID="lblCalcMethod" ToolTip="some text here" runat="server" Text=' <%# Eval("CalculationType")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
</asp:Content>
