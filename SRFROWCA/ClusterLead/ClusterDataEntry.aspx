<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterDataEntry.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterDataEntry" %>


<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">

        function validate() {
            var txtObjList = document.getElementsByClassName('txtAchieved');
            for (var i = 0; i < txtObjList.length; i++) {

                if (txtObjList[i].value != '' && !isNumeric(txtObjList[i].value)) {
                    alert('Please enter valid values!');
                    return false;
                }
            }

            return true;
        }

        function isNumeric(myValue) {
            var numexp = /^[0-9]+$/;
            if (myValue.trim() != '') {
                if (myValue.match(numexp)) {
                    return true;
                } else {
                    return false;
                }
            }
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
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AppendDataBoundItems="true" ID="ddlCluster" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%;">
                                                    <label>
                                                        Reporting Year/Month:</label>
                                                </td>
                                                <td style="width: 1%;">
                                                    <asp:DropDownList ID="ddlYear" Enabled="false" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td style="width: 24%;">
                                                    <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                    </asp:DropDownList></td>
                                                <td style="width: 50%; text-align: right;">
                                                    <asp:Button runat="server" ID="btnSaveAll" Text="Save" class="width-10 btn btn-sm" OnClientClick="return validate();" OnClick="btnSaveAll_Click" />

                                                </td>



                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountryClusterTitle"></asp:Label>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label runat="server" ID="lblCountryCluster"></asp:Label>
                                                </td>
                                                <td></td>
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
            <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace" OnRowDataBound="gvIndicators_RowDataBound"
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
                    <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Image ID="imgRind" runat="server" />
                            <asp:Image ID="imgCind" runat="server" />
                        </ItemTemplate>

                        
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-Width="73%" HeaderText="Indicator">
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <%# Eval("Indicator")%>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Target" ItemStyle-HorizontalAlign="Right" HeaderText="Target" ItemStyle-Width="10%"></asp:BoundField>


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
                                <asp:TextBox runat="server" MaxLength="10" Width="100" ID="txtAchieved" CssClass="txtAchieved" Style="text-align: right;" Text='<%# Eval("Achieved") %>'></asp:TextBox>
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
