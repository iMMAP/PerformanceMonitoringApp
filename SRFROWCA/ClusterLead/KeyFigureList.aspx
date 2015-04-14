<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="KeyFigureList.aspx.cs" Inherits="SRFROWCA.ClusterLead.KeyFigureList" %>
<asp:Content ID="cntHeadClusterDataEntry" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../assets/orsjs/jquery.wholenumber.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(".numeric1").wholenumber();
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
            <li class="active">Key Figures List</li>
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
                                      <div style="float:right;margin-right:10px;">
                                 <button runat="server" id="btnNew" style="height: 42px;width: 130px;" onserverclick="btnNew_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Add Key Figure">
                                     Add Key Figure

                                 </button>
                                     </div>
                                </div>
                             
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 95%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>
                                                   Category
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" ID="ddlCategory" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Category ---" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </td>
                                                <td>
                                                    Country
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:100px">
                                                   
                                                </td>
                                                <td>
                                                    
                                                </td>
                                                <td></td>
                                                <td>
                                                   

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
            <asp:GridView ID="gvKeyFigures" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvClusterIndicators_Sorting" HeaderStyle-BackColor="ButtonFace" OnRowDataBound="gvIndicators_RowDataBound"
                CssClass=" table-striped table-bordered table-hover" Width="100%" OnRowCommand="gvKeyFigure_RowCommand"
                EmptyDataText="There are no key figures available!">
                <HeaderStyle BackColor="Control"></HeaderStyle>

                <Columns>                   
                     <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName" ItemStyle-Width="15%"></asp:BoundField>
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" SortExpression="CategoryName" ItemStyle-Width="15%"></asp:BoundField>
                    <asp:BoundField DataField="KeyFigure" HeaderText="Key Figure" SortExpression="KeyFigure" ItemStyle-Width="20%"></asp:BoundField>
                     <asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitName" ItemStyle-Width="10%"></asp:BoundField>
                    <asp:BoundField DataField="PopulationInNeed" HeaderText="Population In Need" SortExpression="PopulationInNeed" ItemStyle-Width="12%"></asp:BoundField>
                    <asp:BoundField DataField="PopulationTargeted" HeaderText="Population Targeted" SortExpression="PopulationTargeted" ItemStyle-Width="12%"></asp:BoundField>
                    <asp:BoundField DataField="PercentageTargeted" HeaderText="%" SortExpression="PercentageTargeted" ItemStyle-Width="5%"></asp:BoundField>                 
                    <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" SortExpression="ModifiedBy" ItemStyle-Width="5%"></asp:BoundField>
                     <asp:TemplateField HeaderText="Last Modified" SortExpression="LastModified" ItemStyle-Width="25%">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("UpdatedDate")).ToString("MM-dd-yyyy") %>
                                                       
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="40px" CausesValidation="false" 
                                    CommandName="DeleteFigure" CommandArgument='<%# Eval("KeyFigureID") %>' >

                                </asp:LinkButton>                              
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" Width="40px" CausesValidation="false"
                                    CommandName="EditFigure" CommandArgument='<%# Eval("KeyFigureID") %>' >

                                </asp:LinkButton>                            
                            </ItemTemplate>
                        </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</asp:Content>
