<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CountryMapsListing.aspx.cs" Inherits="SRFROWCA.Admin.CountryMapsListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Country Maps</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
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
                                                                    Map Month:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMapType" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        
                                                         <tr>
                                                            <td>
                                                                <label>
                                                                    Location:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                             <td></td>
                                                             <td></td>
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
                    CssClass="table table-striped table-bordered table-hover" OnSorting="gvEmergency_Sorting">

                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MapTitle" HeaderText="Map Title" SortExpression="MapTitle" />

                        <asp:BoundField DataField="MapTypeTitle" HeaderText="Map Month" SortExpression="MapTypeTitle" />
                        <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="LocationName" />
                        <asp:BoundField DataField="IsActive" HeaderText="Active" SortExpression="IsActive" />
                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="false"
                                    CommandName="EditMap" CommandArgument='<%# Eval("CountryMapId") %>' Text="Edit">

                                </asp:LinkButton>
                                <%--<asp:Button ID="btnEdit" runat="server" Width="80px" CausesValidation="false"
                                    CommandName="EditEmergency" CommandArgument='<%# Eval("EmergencyId") %>'></asp:Button>--%>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteMap" CommandArgument='<%# Eval("CountryMapIdentityId") %>' >

                                </asp:LinkButton>
                              <%--  <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteEmergency" CommandArgument='<%# Eval("EmergencyId") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmergencyTypeId" runat="server" Text='<%# Eval("CountryMapIdentityId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                   
                </asp:GridView>
            </div>
        </div>
    
    </div>
</asp:Content>
