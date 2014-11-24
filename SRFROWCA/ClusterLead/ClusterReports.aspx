<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="ClusterReports.aspx.cs" Inherits="SRFROWCA.ClusterLead.ClusterReports" %>

<asp:Content ID="cntClusterReports" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>

    <script type="text/javascript">

        function resetAll() {

            document.getElementById('<%=ddlCountry.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=ddlCluster.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=ddlMonth.ClientID%>').selectedIndex = 0;
            document.getElementById('<%=cbIncludeRegional.ClientID%>').checked = true; 

            return false;
        }

    </script>

</asp:Content>

<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Output Reports</li>
        </ul>
    </div>
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
                                          <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF"  class="btn btn-yellow" causesvalidation="false"
                                            title="PDF">
                                            <i class="icon-download"></i>PDF
                                       
                                        </button>
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
                                                <td>
                                                    <label>
                                                        Indicator:</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIndicatorName" runat="server" Width="270px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                </td>
                                                <td>
                                                    <cc:DropDownCheckBoxes UseButtons="False"   AddJQueryReference="True" AutoPostBack="true"  OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" >
                                                        <Style SelectBoxWidth="270" DropDownBoxBoxWidth="270" DropDownBoxBoxHeight=""></Style>
                                                        
                                                        <Texts SelectBoxCaption="Select Country" />
                                                        <%--<asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>--%>
                                                    </cc:DropDownCheckBoxes>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>Month:</td>
                                                <td>
                                                 <%--   <asp:DropDownList ID="ddlMonth" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                        <asp:ListItem Selected="True" Text="--- Select ---" Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>--%>

                                                     <cc:DropDownCheckBoxes UseButtons="False"   AddJQueryReference="True" AutoPostBack="true"  OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server" ID="ddlMonth" >
                                                        <Style SelectBoxWidth="170" DropDownBoxBoxWidth="270" DropDownBoxBoxHeight=""></Style>
                                                        
                                                        <Texts SelectBoxCaption="Select" />
                                                        <%--<asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>--%>
                                                    </cc:DropDownCheckBoxes></td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                </td>
                                                <td>
                                                   <%-- <asp:DropDownList AutoPostBack="True" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" runat="server" ID="ddlCluster" Width="270px" meta:resourcekey="ddlClusterResource1">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                    </asp:DropDownList>--%>

                                                     <cc:DropDownCheckBoxes UseButtons="False"   AddJQueryReference="True" AutoPostBack="true"  OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" runat="server" ID="ddlCluster" >
                                                        <Style SelectBoxWidth="170" DropDownBoxBoxWidth="270" DropDownBoxBoxHeight=""></Style>
                                                        <Texts SelectBoxCaption="Select Cluster" />
                                                    </cc:DropDownCheckBoxes></td>
                                                </td>
                                                <td style="text-align: right;">


                                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />

                                                    <button onclick="return resetAll();" class="btn btn-primary">Reset</button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td><asp:CheckBox ID="cbIncludeRegional" runat="server" Text="Show Regional Indicators" Checked="True" meta:resourcekey="cbIncludeRegionalResource1" /></td>
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
                <asp:GridView ID="gvClusterReports" Width="100%" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="SiteLanguageId"
                    ShowHeader="true" OnRowDataBound="gvClusterReports_RowDataBound" OnSorting="gvClusterReports_Sorting" CssClass=" table-striped table-bordered table-hover">
                    <EmptyDataTemplate>
                        Your filter criteria does not match any record in database!
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="imgRind" runat="server" />
                                <asp:Image ID="imgCind" runat="server" />
                            </ItemTemplate>


                        </asp:TemplateField>

                        <asp:BoundField ItemStyle-Width="16%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="20%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5"></asp:BoundField>
                        <asp:BoundField ItemStyle-Width="28%" DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6"></asp:BoundField>
                        <%--<asp:BoundField ItemStyle-Width="10%" DataField="Target" HeaderText="Target" SortExpression="Target" meta:resourcekey="BoundFieldResource7"></asp:BoundField>--%>

                        <asp:TemplateField ItemStyle-Width="10%" HeaderText="Target" ItemStyle-HorizontalAlign="Right" SortExpression="Target">
                            <ItemTemplate>
                                <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("Target")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataField="Achieved" HeaderText="Achieved" SortExpression="Achieved"></asp:BoundField>--%>

                         <asp:TemplateField ItemStyle-Width="10%" HeaderText="Achieved" ItemStyle-HorizontalAlign="Right" SortExpression="Achieved">
                            <ItemTemplate>
                                <asp:Label ID="lblAchieved" runat="server" Text=' <%# Eval("Achieved")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource8"></asp:BoundField>

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


    </div>
</asp:Content>
