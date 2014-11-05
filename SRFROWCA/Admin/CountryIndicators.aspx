<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CountryIndicators.aspx.cs" Inherits="SRFROWCA.Admin.CountryIndicators" %>



<asp:Content ID="cntHeadCountryIndicators" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Country Indicators</li>
        </ul>

    </div>
    <div class="page-content">

        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>

                                        <asp:Button ID="btnAddIndicator" runat="server" OnClick="btnAddIndicator_Click" Text="Add Indicator" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">


                                        <div class="row">
                                            <table border="0" style="width: 80%; margin: 10px 10px 10px 20px">
                                                .
                                                         <tr>
                                                             <td>
                                                                 <label>
                                                                     Objective:</label>
                                                             </td>
                                                             <td>
                                                                 <asp:TextBox ID="txtObjectiveName" runat="server" Width="270"></asp:TextBox>
                                                             </td>
                                                             <td>
                                                                 <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                             </td>
                                                             <td>
                                                                 <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AppendDataBoundItems="true" ID="ddlCluster" Width="270">
                                                                     <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                                 </asp:DropDownList>
                                                             </td>
                                                         </tr>


                                                <tr>
                                                    <td>
                                                        <label>
                                                            Indicator:</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtIndicatorName" runat="server" Width="270"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270">
                                                            <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="padding-top: 20px;">
                                                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="false" />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td style="padding-top: 20px;">
                                                        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>

                                            </table>
                                        </div>


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
                <asp:GridView ID="gvClusterIndicators" Width="100%" runat="server" AutoGenerateColumns="false" AllowSorting="True" DataKeyNames="SiteLanguageId"
                    OnRowDataBound="gvClusterIndicators_RowDataBound" OnRowCommand="gvClusterIndicators_RowCommand" CssClass=" table-striped table-bordered table-hover">

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="false" DataField="ClusterIndicatorId" HeaderText="ID" SortExpression="ClusterIndicatorId" />

                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" />
                        <asp:BoundField ItemStyle-Width="15%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" />
                        
                        <asp:BoundField ItemStyle-Width="25%" Visible="false" DataField="Objective" HeaderText="Objective" SortExpression="Objective" />
                        <asp:BoundField ItemStyle-Width="23%" DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                        <asp:BoundField ItemStyle-Width="15%" DataField="Target" HeaderText="Target" SortExpression="Target" />

                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="false"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' Text="Edit">

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>'>

                                </asp:LinkButton>
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

    </div>


</asp:Content>
