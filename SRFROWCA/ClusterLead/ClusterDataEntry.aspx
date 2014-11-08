<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterDataEntry.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry" %>


<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>



<asp:Content ID="cntMainClusterDataEntry" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Cluster Data Entry</li>
        </ul>

    </div>
    <div class="page-content">

        <table style="width: 100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 40%; margin: 0px 10px 0px 20px">

                                            <tr>
                                                <td style="width: 40%;">
                                                    <label>
                                                        Reporting Year/Month:</label>
                                                </td>
                                                <td style="width: 10%;">
                                                    <asp:DropDownList ID="ddlYear" Enabled="false" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 20%;">
                                                    <asp:DropDownList ID="ddlMonth" runat="server">
                                                    </asp:DropDownList></td>
                                                <td style="width: 20%;"></td>

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
            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                CssClass=" table-striped table-bordered table-hover" Width="100%">
                <HeaderStyle BackColor="Control"></HeaderStyle>
               
                <Columns>
                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ClusterIndicatorID" HeaderText="ClusterIndicatorID" 
                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                        <ItemStyle CssClass="hidden" ></ItemStyle>
                    </asp:BoundField>
                     <asp:TemplateField ItemStyle-Width="45%" HeaderText="Indicator">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <%# Eval("Indicator")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                   <asp:TemplateField ItemStyle-Width="45%" HeaderText="Target">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <%# Eval("Target")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="Running Sum">
                        <ItemTemplate>
                            <div style=" word-wrap: break-word;">
                                <asp:TextBox runat="server" ID="txtRunningSum" ></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                      <asp:TemplateField ItemStyle-Width="5%" HeaderText="Achieved">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <asp:TextBox runat="server" ID="txtAchieved" ></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
