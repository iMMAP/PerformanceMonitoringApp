<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.IndicatorListing" %>

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
            <li class="active">Indicators</li>
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
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>

                                        <asp:Button ID="btnAddActivity" runat="server" Text="Add New Indicator" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivity_Click" />
                                        <asp:Button ID="btnAddActivityAndIndicators" runat="server" Text="Add New Activity & Indicators" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivityAndIndicators_Click" style="margin-right:5px;" />
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
                                                                 <asp:DropDownList ID="ddlCluster" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Activity:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                 <asp:DropDownList ID="ddlActivity" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="true" CssClass="width-80" >
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                           <td class="width-20">
                                                                <label>
                                                                    Indicator Name:</label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                      
                                                        <tr>
                                                            <td colspan="3"></td>
                                                            <td colspan="2">
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top:20px;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" /></td>
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
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" PagerSettings-Mode="NumericFirstLast"
                    OnRowCommand="gvActivity_RowCommand" Width="100%" OnRowDataBound="gvActivity_RowDataBound" PagerSettings-Position="Bottom" DataKeyNames="IndicatorDetailId"
                     CssClass="table-striped table-bordered table-hover"  OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging" PageSize="30" OnRowDeleting="gvActivity_RowDeleting">
                    
                    <Columns>
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="150px" />                       
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                          <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                        <%--<asp:BoundField DataField="ActivityType" HeaderText="Activity Type" SortExpression="ActivityType"/>--%>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"   HeaderStyle-Width="80px">
                            <ItemTemplate >
                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false" 
                                    CommandName="EditActivity" CommandArgument='<%# Eval("IndicatorDetailId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false" 
                                    CommandName="Delete" CommandArgument='<%# Eval("IndicatorDetailId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
      
    </div>
</asp:Content>
