<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterDataEntry.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry" %>

<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../assets/orsjs/jquery.wholenumber.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $(".numeric1").wholenumber();

                $.widget("ui.tooltip", $.ui.tooltip, {
                    options: {
                        content: function () {
                            return $(this).prop('title');
                        }
                    }
                });
                $('.tooltip2').tooltip();
            });
        });

    </script>
</asp:Content>

<asp:Content ID="cntMainClusterDataEntry" ContentPlaceHolderID="MainContent" runat="server">
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
                <table style="width: 100%">
                    <tr>
                        <td>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 ">
                                    <div class="widget-box">
                                        <div class="box widget-header widget-header-small header-color-blue2">
                                        </div>
                                        <div class="widget-body">
                                            <div class="widget-main">
                                                <table border="0" style="width: 98%; margin: 0px 10px 0px 20px">
                                                    <tr>
                                                        <td>
                                                           <label> <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label></label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <label><asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label></label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270">
                                                            </asp:DropDownList>

                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100px">
                                                            <label>
                                                                Month:</label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" CssClass="hidden">
                                                                <asp:ListItem Text="2015" Value="11" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        <td class="pull-right">
                                                            <asp:Button runat="server" ID="btnSaveAll" Text="Save" class="btn btn-primary btn-sm" OnClientClick="return validate();" OnClick="btnSaveAll_Click" />
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
                    <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="gvIndicators_RowDataBound"
                        CssClass="imagetable" Width="100%"
                        EmptyDataText="There are no output indicators available!">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
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

                            <asp:TemplateField ItemStyle-Width="46%" HeaderText="Indicator">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word;">
                                        <%# Eval("Indicator")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Unit" HeaderText="Unit" ItemStyle-Width="10%"></asp:BoundField>
                            <asp:TemplateField ItemStyle-Width="8%" HeaderText="Target" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("Target")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="8%"
                                ItemStyle-BackColor="LightYellow"
                                ItemStyle-HorizontalAlign="Right"
                                HeaderText="Monthly Achieved">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word;">
                                        <asp:TextBox runat="server" MaxLength="8" Width="100%" ID="txtAchieved"
                                            CssClass="numeric1" Style="text-align: right;" Text='<%# Eval("Achieved") %>'></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="8%" HeaderText="<span class='tooltip2' title='Caculated on the basis of Calculation Method of the Indicator.'>Running Value</span>" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>

                                    <asp:Label ID="lblSum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="8%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblCalcMethod" ToolTip="some text here" runat="server" Text=' <%# Eval("IndicatorCalculationType")%>'></asp:Label>
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
                            <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
                            <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="CreatedBy" HeaderText="Reported By" ItemStyle-Width="7%"></asp:BoundField>
                            <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="UpdatedBy" HeaderText="Updated By" ItemStyle-Width="7%"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <%--<hr />
            <div class="pull-right">
            <asp:Button ID="btnSave2" runat="server" Text="Save" class="btn btn-primary" OnClientClick="return validate();" OnClick="btnSaveAll_Click" />
                </div>--%>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlCountry" />
                <asp:AsyncPostBackTrigger ControlID="ddlCluster" />
                <asp:AsyncPostBackTrigger ControlID="ddlMonth" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
</asp:Content>
