<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UnitsListing.aspx.cs" Inherits="SRFROWCA.Admin.UnitsListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg"></div>
        <table class="width-100">
            <tr>
                <td>
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>
                            <asp:Button ID="btnAddUnit" runat="server" OnClick="btnAddUnit_Click"
                                Text="Add Unit" CausesValidation="False"
                                CssClass="btn btn-yellow pull-right" />
                        </h6>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="row">
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td class="width-20">
                                                    <label>
                                                        Unit:
                                                    </label>
                                                    <asp:TextBox ID="txtUnits" runat="server"></asp:TextBox>
                                                </td>
                                                <td class="width-30">
                                                    <asp:CheckBox ID="cbIsGender" runat="server" Text="Is Gender" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                                                        OnClick="btnSearch_Click" CssClass="btn btn-primary"
                                                        CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
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
        <div class="row">
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvUnits" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        DataKeyNames="UnitId">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>

                                <ItemStyle Width="2%"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UnitId" HeaderText="Unit Id">
                                <ItemStyle Width="120px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitEn" HeaderText="Unit En" ItemStyle-Width="25%">
                            </asp:BoundField>
                            <asp:BoundField DataField="UnitFr" HeaderText="Unit Fr" ItemStyle-Width="25%"></asp:BoundField>
                            <asp:TemplateField ItemStyle-Width="25%" HeaderText="Is Gender">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlIsGender" runat="server" SelectedValue='<%# Bind ("IsGender") %>' Width="100px">
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
