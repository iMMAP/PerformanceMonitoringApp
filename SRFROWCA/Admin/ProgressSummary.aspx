﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgressSummary.aspx.cs" Inherits="SRFROWCA.Admin.ProgressSummary" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   <%-- <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>--%>
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
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Progress Summary" meta:resourcekey="localBreadCrumbProjectsResource1"></asp:Localize></li>

        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <asp:Label runat="server" ID="lblMessage"></asp:Label>
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
                                                    <table border="0" style="width: 50%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Date From:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>

                                                             <td>Country:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" AppendDataBoundItems="true" runat="server" AutoPostBack="true" CssClass="width-100" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select Country" Value="-1" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
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
                                                        </tr>
                                                        <tr >
                                                            <td >&nbsp;</td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />

                                                                <asp:Button ID="btnPDFPrint" runat="server" Text="Export PDF" CssClass="btn btn-primary" OnClick="btnPDFPrint_Click" />
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

