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

                                        <asp:Button ID="btnAddIndicator" runat="server" Text="Add Indicator" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                      
                                          
                                                <div class="row">
                                                    <table border="0" style="width: 50%; margin: 10px 10px 10px 20px">
                                                        .
                                                         <tr>
                                                             <td>
                                                                 <label>
                                                                     Country:</label>
                                                             </td>
                                                             <td>
                                                                 <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270">
                                                                     <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                                 </asp:DropDownList>
                                                             </td>
                                                         </tr>

                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCluster" Width="270">
                                                                    <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtObjectiveName"  runat="server" Width="270"></asp:TextBox>
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
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" CausesValidation="false" />
                                                            </td>
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
                    CssClass=" table-striped table-bordered table-hover">

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField ItemStyle-Width="5%" DataField="EmergencyObjectiveId" HeaderText="ID" SortExpression="EmergencyObjectiveId" />

                        <asp:BoundField ItemStyle-Width="25%" DataField="EmergencyName" HeaderText="Emergency Name" SortExpression="EmergencyName" />
                        <asp:BoundField ItemStyle-Width="45%" DataField="Objective" HeaderText="Objective Name" SortExpression="Objective" />
                        
                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="false"
                                    CommandName="EditObjective" CommandArgument='<%# Eval("EmergencyObjectiveId") %>' Text="Edit">

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteObjective" CommandArgument='<%# Eval("EmergencyObjectiveId") %>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                           <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmergencyId" runat="server" Text='<%# Eval("EmergencyId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblObjAlternate" runat="server" Text='<%# Eval("ObjectiveAlt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>

                </asp:GridView>
            </div>
        </div>

    </div>


</asp:Content>
