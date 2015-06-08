<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgressSummary.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProgressSummary" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.min.js"></script>--%>
    <style>
        .reportTable {
            width: 100%;
        }

            .reportTable td {
                padding: 5px;
            }

            .reportTable tr {
                border-bottom: solid 1px #ccc;
            }

                .reportTable tr.heading {
                    border-bottom: solid 2px #ccc;
                    font-weight: bold;
                    font-size: 14px;
                }

                .reportTable tr.colHeading {
                    background-color: #ccc;
                    font-weight: bold;
                    font-size: 13px;
                }

        .countryHeading td {
            font-weight: bold;
            font-size: 13px;
            padding-top: 20px;
            /*border-bottom:solid 1px #ccc;*/
        }

        .countryData td {
            padding-left: 30px;
            /*border-bottom:solid 1px #ccc;*/
        }
    </style>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <asp:Label runat="server" ID="lblMessage"></asp:Label>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <asp:Button ID="btnPDFPrint" runat="server" Text="Export PDF" CssClass="btn btn-yellow" OnClick="btnPDFPrint_Click" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 50%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td>Country:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" CssClass="width-100" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>Cluster: 
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlClusters" runat="server" AutoPostBack="true" CssClass="width-80" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Month:</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlMonth" AutoPostBack="true" CssClass="width-100" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:RadioButtonList ID="rbIsOPSProject" runat="server" RepeatColumns="3" AutoPostBack="true" OnSelectedIndexChanged="rbIsOPSProject_SelectedIndexChanged">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="SRP" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="ORS" Value="0"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
                                                                <%--<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />--%>
                                                                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-primary" OnClick="btnReset_Click" />

                                                                
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
        <table class="reportTable">
            <tr class="heading">
                <td colspan="2">Summary:</td>
            </tr>
            <tr>
                <td style="width: 400px;">Total Projects:</td>
                <td>
                    <asp:Label ID="lblTotalProjects" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Total Organizations:</td>
                <td>
                    <asp:Label ID="lblTotalOrgs" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Reporting Projects:</td>
                <td>
                    <asp:Label ID="lblReportingProjects" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Reporting Organizations:</td>
                <td>
                    <asp:Label ID="lblReportingOrgs" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Reporting Countries:</td>
                <td>
                    <asp:Label ID="lblReportingCountries" runat="server" Text="---"></asp:Label></td>
            </tr>
            <tr>
                <td>Number Of Reports:</td>
                <td>
                    <asp:Label ID="lblReportsCount" runat="server" Text="---"></asp:Label></td>
            </tr>
        </table>
        <br />
        <br />
        <table id="tblCountry" runat="server" class="reportTable">
            <tr class="heading">
                <td colspan="5">Country Situation:</td>
            </tr>
            <tr class="colHeading">
                <td>#</td>
                <td>Country</td>
                <td>Projects Reporting</td>
                <td>Organizations Reporting</td>
                <td>Activities Reported</td>
                <td>Indicators Reported</td>
            </tr>
        </table>
        <br />
        <br />
        <table id="tblOrg" runat="server" class="reportTable">
            <tr class="heading">
                <td colspan="2">Organizations Reporting:</td>
            </tr>
        </table>
    </div>



</asp:Content>

