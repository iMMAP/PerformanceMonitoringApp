<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectDetails.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .tblMain td {
            width: 350px;
            padding-bottom: 5px;
        }

            .tblMain td label {
                font-weight: bold;
            }
    </style>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active"><a href="ProjectsListing.aspx">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Projects" meta:resourcekey="localBreadCrumbProjectsResource1"></asp:Localize></a></li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjectDetails" runat="server" Text="Project Details"></asp:Localize></li>

        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table class="width-100">
            <tr>
                <td>
                    <div class="row">

                        <div class="widget-header widget-header-small header-color-blue2">
                            <h6>
                                <button runat="server" id="btExportPDF" onserverclick="btExportPDF_Click" class="width-10 btn btn-sm btn-yellow"
                                    title="PDF">
                                    <i class="icon-download"></i>PDF
                                       
                                </button>
                                <button runat="server" id="btnExportToExcel" class="width-10 btn btn-sm btn-yellow"
                                    title="Excel">
                                    <i class="icon-download"></i>Excel
                                       
                                </button>

                            </h6>
                            <div class="widget-toolbar">
                                <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                            </div>
                        </div>
                        <div class="widget-body">
                           
                            <div class="widget-main">

                                <div class="containerLogin">
                                    <asp:FormView ID="fvProjects" runat="server" OnPageIndexChanging="fvProjects_PageIndexChanging">
                                        <ItemTemplate>
                                            <h3>
                                                <%# Eval("ProjectCode") %></h3>
                                            <table class="tblMain">
                                                <tr>
                                                    <td>
                                                        <label>Project ID:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectId") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Project Title:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectTitle")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Organization:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("OrganizationName") %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Country:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("LocationName") %>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <label>Contact Name:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectContactName")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Contact Email:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectContactEmail")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Contact Phone:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectContactPhone")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Start Date:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectStartDate")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>End Date:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectEndDate")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Partners:</label>
                                                    </td>
                                                    <td>
                                                        <%# Eval("ProjectImplementingpartner")%>
                                                    </td>
                                                </tr>
                                                <%-- <tr>
                                                                                        <td>&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button Visible="false" runat="server" ID="btnViewReport" Text="View Reports" class="width-10 btn btn-sm" OnClick="btnViewReport_Click" />
                                                                                        </td>
                                                                                    </tr>--%>
                                            </table>
                                        </ItemTemplate>
                                    </asp:FormView>

                                </div>

                            </div>
                          
                        </div>

                        <%--<div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <table border="0" style="width:50%">
                                                <tr>
                                                    <td>
                                                        <label>
                                                            Date From:</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="width-80"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                     <td><label>
                                                            Date To:</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="width-80"></asp:TextBox>
                                                    </td>
                                                    <td colspan="2" style="padding-top:10px;text-align:right;">
                                                        <asp:Button runat="server" ID="btnSearch" Text="Search" class="width-10 btn btn-sm" OnClick="btnSearch_Click" />

                                                    </td>
                                                </tr>
                                               

                                            </table>
                                        </div>
                                    </div>
                                </div>--%>
                    </div>


                </td>
            </tr>
        </table>

        <div class="row">
        
                    <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" CssClass=" table-striped table-bordered table-hover"
                        AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any report!" Width="100%"
                        OnRowCommand="grdReports_RowCommand" OnSorting="grdReports_Sorting" OnPageIndexChanging="grdReports_PageIndexChanging">

                        <Columns>
                            <asp:BoundField DataField="ProjectCode" Visible="false" HeaderText="Project Code" SortExpression="ProjectCode" />
                            <%--<asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" >
                                <ItemStyle Wrap="True"></ItemStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="ReportID" HeaderText="Report ID" SortExpression="ReportID" />
                            <asp:BoundField DataField="ReportName" HeaderText="Report Name" SortExpression="ReportName" />
                            <asp:BoundField DataField="Country" HeaderText="Location" SortExpression="Country" />
                            <asp:BoundField DataField="IsApproved" HeaderText="Is Approved" SortExpression="IsApproved" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created On" SortExpression="CreatedDate"></asp:BoundField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View">

                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkViewDetails" runat="server" ImageUrl="../assets/orsimages/view.png" CommandName="ViewReport"
                                        CommandArgument='<%# Eval("ReportID") %>' />

                                    <%-- <asp:LinkButton ID="lnkVieDetails" runat="server" Text="View" CommandName="ViewProject"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkVieDetailsResource1" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                        CommandArgument='<%# Eval("ReportID") %>' />


                                </ItemTemplate>


                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
            
        </div>

    </div>
</asp:Content>
