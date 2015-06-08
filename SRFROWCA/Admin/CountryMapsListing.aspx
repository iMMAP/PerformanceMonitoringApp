<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CountryMapsListing.aspx.cs" Inherits="SRFROWCA.Admin.CountryMapsListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
            <tr>
                <td class="signupheading2" colspan="3">
                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divMsg">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>

                                        <asp:Button ID="btnAddEmergency" runat="server" Text="Add New Map" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddEmergency_Click" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 90%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Map Title:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMapTitle" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Country:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="cbIsPublic" runat="server" Text="Only Published" />
                                                            </td>
                                                            <%--<td>
                                                                <label>
                                                                    Month:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMonth" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>--%>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
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
                <asp:GridView ID="gvEmergency" runat="server" AutoGenerateColumns="false" AllowSorting="True"
                    OnRowCommand="gvEmergency_RowCommand" Width="100%" OnRowDataBound="gvEmergency_RowDataBound"
                    CssClass="table-striped table-bordered table-hover" OnSorting="gvEmergency_Sorting">

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MapTitle" HeaderText="Map Title" SortExpression="MapTitle" />
                        <asp:BoundField DataField="LocationName" HeaderText="Country" SortExpression="LocationName" />
                        <asp:BoundField DataField="IsPublic2" HeaderText="Published" SortExpression="IsPublic2" />
                        <asp:TemplateField HeaderText="View Map">
                            <ItemTemplate>
                                <asp:HyperLink ID="MyLink" ImageUrl="~/assets/orsimages/map24.png" Target="_blank" 
                                    NavigateUrl='<%# Eval("MapURL", "~/orsmaps/{0}")  %>' runat="server"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                CommandName="DeleteMap" CommandArgument='<%# Eval("CountryMapId") %>' ToolTip="Delete Map" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                CommandName="EditMap" CommandArgument='<%# Eval("CountryMapId") %>' ToolTip="Edit Map Entry" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmergencyTypeId" runat="server" Text='<%# Eval("CountryMapId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>
