<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganizationList.aspx.cs" Inherits="SRFROWCA.Admin.Organization.OrganizationList" MasterPageFile="~/Site.Master" %>
<%@ MasterType virtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Organizations</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
     <div class="page-content">
          <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
        <tr>
            <td class="signupheading2 error2" colspan="3">
               
                        <asp:Label ID="lblMessage" runat="server" CssClass="error2" Visible="false"
                            ViewStateMode="Disabled"></asp:Label>
                  
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
                                       <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>
                                       <asp:Button ID="btnAddUser" runat="server" Text="Add New Organization" PostBackUrl="~/Admin/organization/AddEditOrganization.aspx"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Organization Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrganizationName" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Country:</label>
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="ddlCountry" runat="server" AppendDataBoundItems="true"  CssClass="width-90">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Status:</label>
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="ddlStatus" runat="server" CssClass="width-90">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="InActive" Value="0"></asp:ListItem>
                                                                   
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Organization Acronym:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAcronym" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Organization Type:
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="width-90" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                   
                                                                </asp:DropDownList>
                                                            </td>
                                                             <td>
                                                              <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                                                            </td>
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
        
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="gvOrganization" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                        CssClass="imagetable" PageSize="300" AllowSorting="true" Width="100%" OnPageIndexChanging="gvOrganization_PageIndexChanging"
                        OnSorting="gvOrganization_Sorting" OnRowCommand="gvOrganization_RowCommand" OnRowDeleting="gvOrganization_RowDeleting" OnRowDataBound="gvOrganization_RowDataBound">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization Name" SortExpression="OrganizationName" />
                            <asp:BoundField DataField="OrganizationAcronym" HeaderText="Organization Acronym" SortExpression="OrganizationAcronym" />
                             <asp:BoundField DataField="OrganizationType" HeaderText="Type" SortExpression="OrganizationType" />
                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                            <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" SortExpression="Phone" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate"
                                ItemStyle-HorizontalAlign="Center" />
                             <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("OrganizationID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>  
                             <asp:TemplateField HeaderText="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("OrganizationID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                         
                           
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="no-record">No record found!</div>                           
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
                 
                
    </div>
    </asp:Content>