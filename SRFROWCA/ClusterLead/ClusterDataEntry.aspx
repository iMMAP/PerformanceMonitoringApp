<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterDataEntry.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry" %>


<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">

    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $(".numeric1").numeric();
        });

    </script>

</asp:Content>

<asp:Content ID="cntMainClusterDataEntry" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Output Indicator Data Entry</li>
        </ul>

    </div>
    <div class="page-content">
        <div id="divMsg"></div>
        <table style="width: 100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">

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
                                                <td style="width: 10%;">
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                                <td style="width: 5%"></td>
                                                <td style="width: 25%"></td>
                                            </tr>
                                            <tr>
                                                <td style="<%=ClusterDisplayNone%>">
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                </td>
                                                <td style="<%=ClusterDisplayNone%>">
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label>
                                                        Reporting Year/Month:</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlYear" Enabled="false" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                                <td style="text-align: right;">
                                                    <asp:Button runat="server" ID="btnSaveAll" Text="Save" class="width-10 btn btn-sm" OnClientClick="return validate();" OnClick="btnSaveAll_Click" />

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

        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvClusterIndicators_Sorting" HeaderStyle-BackColor="ButtonFace" OnRowDataBound="gvIndicators_RowDataBound"
                CssClass=" table-striped table-bordered table-hover" Width="100%" ShowFooter="true"
                EmptyDataText="There are no output indicators available!">
                <HeaderStyle BackColor="Control"></HeaderStyle>

                <Columns>
                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblClusterIndicatorID" runat="server" Text='<%# Eval("ClusterIndicatorID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Image ID="imgRind" runat="server" />
                            <asp:Image ID="imgCind" runat="server" />
                        </ItemTemplate>


                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="63%" HeaderText="Indicator" SortExpression="Indicator">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <%# Eval("Indicator")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" ItemStyle-Width="10%"></asp:BoundField>
                    <%--<asp:BoundField DataField="Target" ItemStyle-HorizontalAlign="Right" HeaderText="Target" SortExpression="Target" ItemStyle-Width="10%"></asp:BoundField>--%>
                    <asp:TemplateField ItemStyle-Width="10%" HeaderText="Target" ItemStyle-HorizontalAlign="Right" SortExpression="Target">
                        <ItemTemplate>
                            <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("Target")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <%--  <asp:TemplateField ItemStyle-Width="5%" HeaderText="Running Sum">
                        <ItemTemplate>
                            <div style=" word-wrap: break-word;">
                                <asp:TextBox runat="server" MaxLength="10" ID="txtRunningSum" Text='<%# Eval("RunningSum") %>' ></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>--%>

                    <asp:TemplateField ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Right" HeaderText="Achieved">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <asp:TextBox runat="server" MaxLength="8" Width="100" ID="txtAchieved" CssClass="numeric1" Style="text-align: right;" Text='<%# Eval("Achieved") %>'></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("CountryID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("ClusterID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                        <ItemStyle CssClass="hidden"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                        <ItemStyle CssClass="hidden"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
