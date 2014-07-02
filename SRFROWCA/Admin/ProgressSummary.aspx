<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgressSummary.aspx.cs" Inherits="SRFROWCA.Admin.ProgressSummary" MasterPageFile="~/Site.Master" %>

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
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6></h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" width="50%">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Date From:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>

                                                           <%-- <td>Country:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" AppendDataBoundItems="true" runat="server" CssClass="width-100">
                                                                    <asp:ListItem Text="Select Country" Value="-1" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>--%>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Date To:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <%--<td>Cluster: 
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlClusters" AppendDataBoundItems="true" runat="server" CssClass="width-100">
                                                                    <asp:ListItem Text="Select Cluster" Value="-1" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>--%>

                                                            <td>&nbsp;
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rbIsOPSProject" runat="server" RepeatColumns="3">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="SRP" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="ORS" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td>&nbsp;
                                                            </td>

                                                            <td colspan="3" style="text-align: right;">
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
        <br />
        <table style="width: 25%;">
            <tr>
                <td>Number of Users:</td>
                <td>
                    <asp:Label ID="lblUsers" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Number of Orgs:</td>
                <td>
                    <asp:Label ID="lblOrganizations" runat="server" Text="---"></asp:Label></td>

            </tr>
            <tr>
                <td>Number of Orgs Reported:</td>
                <td>
                    <asp:Label ID="lblReportedOrgs" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Number of Countries Reported:</td>
                <td>
                    <asp:Label ID="lblReportedCountries" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Number of Reports:</td>
                <td>
                    <asp:Label ID="lblReports" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Number of Projects with Reports:</td>
                <td>
                    <asp:Label ID="lblReportedProjects" runat="server" Text="---"></asp:Label></td>
            </tr>
        </table>
    </div>



</asp:Content>

