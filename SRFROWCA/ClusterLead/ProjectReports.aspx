<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectReports.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#<%=txtFromDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected)
                }
            });
            $("#<%=txtToDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected)
                }
            });

            $(".classsearchcriteriacustomreport").tooltip({
                show: {
                    effect: "slideDown",
                    delay: 250
                }
            });
        });
    </script>
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
                                       <button runat="server" id="btnExportPDF" onserverclick="btnExportPDF_Click" class="width-10 btn btn-sm btn-yellow"
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
                    <asp:GridView ID="grdReports" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                        AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any report!" Width="100%" 
                        OnRowCommand="grdReports_RowCommand" OnSorting="grdReports_Sorting" OnPageIndexChanging="grdReports_PageIndexChanging">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" />
                            <%--<asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" >
                                <ItemStyle Wrap="True"></ItemStyle>
                            </asp:BoundField>--%>
                            <asp:BoundField DataField="ReportID" HeaderText="Report ID" SortExpression="ReportID"/>
                            <asp:BoundField DataField="ReportName" HeaderText="Report Name" SortExpression="ReportName"/>
                            <asp:BoundField DataField="Country" HeaderText="Location" SortExpression="Country"/>
                            <asp:BoundField DataField="IsApproved" HeaderText="Is Approved" SortExpression="IsApproved" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created On" SortExpression="CreatedDate" ></asp:BoundField>
                            <asp:TemplateField >
                                <ItemTemplate>
                                     <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="~/assets/orsimages/pdf.png" CommandName="PrintReport"
                                                    CommandArgument='<%# Eval("ReportID") %>' />
                                            
                                 
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
