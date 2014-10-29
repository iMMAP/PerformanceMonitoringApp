<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="LocationsList.aspx.cs" Inherits="SRFROWCA.Admin.Location.LocationsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    </asp:Content>
<%@ MasterType virtualPath="~/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Locations</li>
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
                                       <asp:Button ID="btnAddUser" runat="server" Text="Add New Location" PostBackUrl="~/Admin/Location/AddNewLocation.aspx"
                                            CssClass="btn btn-yellow pull-right" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Location Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLocationName" runat="server" CssClass="width-90"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>
                                                                    Location Type:</label>
                                                            </td>
                                                            <td>
                                                                 <asp:DropDownList ID="ddlType" runat="server" AppendDataBoundItems="true"  CssClass="width-90">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                           
                                                        </tr>                                                       
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td colspan="4" style="padding-top:20px">
                                                              <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />

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
                    <asp:GridView ID="gvLocation" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                        CssClass=" table-striped table-bordered table-hover" PageSize="30" AllowSorting="true" Width="100%" 
                        OnPageIndexChanging="gvLocation_PageIndexChanging" OnSorting="gvLocation_Sorting"
                        OnRowCommand="gvLocation_RowCommand">
                       
                        <Columns>                            
                            <asp:BoundField DataField="LocationName" HeaderText="Location Name" SortExpression="LocationName" />
                            <asp:BoundField DataField="LocationType" HeaderText="Location Type" SortExpression="LocationType" />
                             <asp:BoundField DataField="LocationPCode" HeaderText="PCode" SortExpression="LocationPCode" />
                            <asp:BoundField DataField="Latitude" HeaderText="Latitude" SortExpression="Latitude" />
                            <asp:BoundField DataField="Longitude" HeaderText="Longitude" SortExpression="Longitude" />
                            <asp:BoundField DataField="EstimatedPopulation" HeaderText="Population" SortExpression="EstimatedPopulation" />
                            <asp:BoundField DataField="IsAccurateLatLng" HeaderText="Is Accurate LatLong" SortExpression="IsAccurateLatLng" />
                             <asp:BoundField DataField="Region" HeaderText="Region" SortExpression="Region" />
                             <asp:BoundField DataField="National" HeaderText="National" SortExpression="National" />
                             <asp:BoundField DataField="Governorate" HeaderText="Governorate" SortExpression="Governorate" />
                             <asp:BoundField DataField="District" HeaderText="District" SortExpression="District" />
                            <asp:BoundField DataField="SubDistrict" HeaderText="Sub District" SortExpression="SubDistrict" />                                                                   
                           <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:LinkButton Text="Edit" ID="btnEdit" runat="server"  CommandName="Edit" CommandArgument='<%# Eval("LocationId") %>' />
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