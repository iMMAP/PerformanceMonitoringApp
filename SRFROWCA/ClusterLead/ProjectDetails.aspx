<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectDetails.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
    <style>
        .tblMain td {
            width:10%;
            padding-bottom:5px;
        }
        .tblMain td label{
            font-weight:bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">

        <table class="width-100">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btExportPDF" class="width-10 btn btn-sm btn-yellow"
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
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <div >
                                                        <div class="widget-box no-border">
                                                            <div class="widget-body">
                                                                <div class="widget-main padding-6">
                                                                    
                                                                     <div class="containerLogin">
        <asp:FormView ID="fvProjects" runat="server" OnPageIndexChanging="fvProjects_PageIndexChanging">
            <ItemTemplate>                
                <h3>
                    <%# Eval("ProjectCode") %></h3>
                <table class="tblMain" border="0" style="width:100%">
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
                    <tr>
                        <td>
                           &nbsp;
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnViewReport" Text="View Reports" class="width-10 btn btn-sm" OnClick="btnViewReport_Click" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        
    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    
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

   
</div>

</asp:Content>
