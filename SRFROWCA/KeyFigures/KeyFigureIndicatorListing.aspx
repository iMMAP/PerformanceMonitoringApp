<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeyFigureIndicatorListing.aspx.cs" Inherits="SRFROWCA.KeyFigures.KeyFigureIndicatorListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Key Figure Framework</li>
        </ul>

    </div>
    <div class="page-content">
        <div class="page-content">

            <div id="divMsg"></div>
            <table style="width: 100%">
                <tr>
                    <td>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 ">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2" style="padding-left: 0px;">
                                        <h6>
                                            <button runat="server" id="btnExportToExce" onserverclick="btnExportToExcel_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel                                   
                                        </button>
                                            <asp:Button ID="btnAddKeyFigure" runat="server" OnClick="btnAddKeyFigure_Click" Text="Add Key figure" CausesValidation="False"
                                                CssClass="btn btn-purple pull-right" />
                                            <asp:Button ID="btnAddSubCategory" runat="server" OnClick="btnAddSubCategory_Click" Text="Add Sub Category" CausesValidation="False"
                                                CssClass="btn btn-default pull-right" />
                                            <asp:Button ID="btnAddCategory" runat="server" OnClick="btnAddCategory_Click" Text="Add Category" CausesValidation="False"
                                                CssClass="btn btn-yellow pull-right" />
                                        </h6>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                <asp:GridView ID="gvKFInd" runat="server" AutoGenerateColumns="False" OnRowCommand="gvKFInd_RowCommand" OnRowDataBound="gvKFInd_RowDataBound"
                    DataKeyNames="CategoryId,SubCategoryId,KeyFigureId"
                    HeaderStyle-BackColor="ButtonFace"
                    CssClass="imagetable" Width="100%"
                    EmptyDataText="There are no key figures available!">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Country"></asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditCategory" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                    CommandName="EditCategory" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Edit Category" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SubCategory" HeaderText="SubCategory" SortExpression="Category"></asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditSubCatgory" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                    CommandName="EditSubCategory" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Edit Sub-Category" />

                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="KeyFigure" HeaderText="Key Figure" SortExpression="KeyFigure"></asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditKFInd" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                    CommandName="EditKfInd" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Edit Key Figure" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                    CommandName="DeleteFigure" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
