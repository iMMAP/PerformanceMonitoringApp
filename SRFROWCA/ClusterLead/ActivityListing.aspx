<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ActivityListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.ActivityListing" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

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
            <li class="active">Framework Activities</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="alert2 alert-block alert-info">
        <h6>
            <asp:Localize ID="localActivityInfo" runat="server" Text="Please provide the activities that cluster partners will undertake in 2015. You can specify up to 25 Activities that partners will be able to associate with their projects during project upload on OPS." meta:resourcekey="localActivityInfoResource1"></asp:Localize>
        </h6>
    </div>
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
                                          <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF"  class="btn btn-yellow" causesvalidation="false"
                                            title="PDF">
                                            <i class="icon-download"></i>PDF
                                       
                                        </button>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>

                                        <asp:Button ID="btnAddActivity" runat="server" Text="Add New Activity" CausesValidation="False"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivity_Click" meta:resourcekey="btnAddActivityResource1" />
                                        <asp:Button ID="btnAddActivityAndIndicators" runat="server" Text="Add New Activity & Indicators" CausesValidation="False"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivityAndIndicators_Click" style="margin-right:5px;" meta:resourcekey="btnAddActivityAndIndicatorsResource1" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                             <td class="width-20">
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlCluster" runat="server" AppendDataBoundItems="True" CssClass="width-80" meta:resourcekey="ddlClusterResource1">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True" meta:resourcekey="ListItemResource1"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                           <td class="width-20">
                                                                <label>
                                                                    Activity Name:</label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80" meta:resourcekey="txtActivityNameResource1"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="True" CssClass="width-80" meta:resourcekey="ddlObjectiveResource1" >
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True" meta:resourcekey="ListItemResource2"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                           <td></td>
                                                            <td></td>
                                                        </tr>
                                                      
                                                        <tr>
                                                            <td colspan="3"></td>
                                                            <td colspan="2">
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top:20px;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" /><asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left:5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False"  /></td>
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

        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" PagerSettings-Mode="NumericFirstLast" ShowHeaderWhenEmpty="True"
                    OnRowCommand="gvActivity_RowCommand" Width="100%" OnRowDataBound="gvActivity_RowDataBound" PagerSettings-Position="Bottom" DataKeyNames="ActivityDetailId"
                     CssClass="table-striped table-bordered table-hover"  OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging" PageSize="30" OnRowDeleting="gvActivity_RowDeleting"
                    EmptyDataText="Your filter criteria does not match any activity!">
                    
                    <Columns>
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource1" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource2" >                       
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" meta:resourcekey="BoundFieldResource3" />
                        <%--<asp:BoundField DataField="ActivityType" HeaderText="Activity Type" SortExpression="ActivityType"/>--%>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"   HeaderStyle-Width="80px" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate >
                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="False" 
                                    CommandName="EditActivity" CommandArgument='<%# Eval("ActivityDetailId") %>' meta:resourcekey="btnEditResource1" />
                            </ItemTemplate>

<HeaderStyle Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="80px" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="False" 
                                    CommandName="Delete" CommandArgument='<%# Eval("ActivityDetailId") %>' meta:resourcekey="btnDeleteResource1" />
                            </ItemTemplate>

<HeaderStyle Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />

                </asp:GridView>
            </div>
        </div>
      
    </div>
</asp:Content>
