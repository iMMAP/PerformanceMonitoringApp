<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterDataEntry.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry" %>


<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">

        function validate()
        {


            return true;
        }

    </script>

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
                                        <table border="0" style="width: 95%; margin: 0px 10px 0px 20px">

                                            <tr>
                                                <td style="width: 15%;">
                                                    <label>
                                                        Reporting Year/Month:</label>
                                                </td>
                                                <td style="width: 5%;">
                                                    <asp:DropDownList ID="ddlYear" Enabled="false" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 20%;">
                                                    <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                    </asp:DropDownList></td>
                                                <td style="width: 50%;text-align:right;">
                                                     <asp:Button runat="server" ID="btnSaveAll" Text="Save All" class="width-10 btn btn-sm" OnClientClick="return validate();" OnClick="btnSaveAll_Click"  />

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
            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                CssClass=" table-striped table-bordered table-hover" Width="100%">
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
                  
                     <asp:TemplateField ItemStyle-Width="65%" HeaderText="Indicator">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <%# Eval("Indicator")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Target" HeaderText="Target"  ItemStyle-Width="25%" ></asp:BoundField>
                  

                    <asp:TemplateField ItemStyle-Width="5%" HeaderText="Running Sum">
                        <ItemTemplate>
                            <div style=" word-wrap: break-word;">
                                <asp:TextBox runat="server" MaxLength="10" ID="txtRunningSum" Text='<%# Eval("RunningSum") %>' ></asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                      <asp:TemplateField ItemStyle-Width="5%" HeaderText="Achieved">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <asp:TextBox runat="server" MaxLength="10" ID="txtAchieved" Text='<%# Eval("Achieved") %>' ></asp:TextBox>
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

                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
