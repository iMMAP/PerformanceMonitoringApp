<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ActivitiesReport.aspx.cs" Inherits="SRFROWCA.Admin.Reports.ActivitiesReport" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">    
    <style type="text/css">
        .ddlWidth
        {
            width: 300px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var hf = $("#<%=hfReportLink.ClientID%>").val();
            var hl = $('#reportlink').attr("href");
            $('#reportlink').attr("href", hl + hf);

            $("#<%=gvReport.ClientID %>").prepend('<colgroup><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /></colgroup>');
            $("#<%=gvReport.ClientID %>").kiketable_colsizable();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblFilter" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td>
                Emergency:
            </td>
            <td>
                <asp:DropDownList ID="ddlEmergency" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Office:
            </td>
            <td>
                <asp:DropDownList ID="ddlOffice" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                User:
            </td>
            <td>
                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Year:
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Month:
            </td>
            <td>
                <asp:DropDownList ID="ddlMonth" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Location:
            </td>
            <td>
                <asp:DropDownList ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Clusters:
            </td>
            <td>
                <cc1:DropDownCheckBoxes ID="ddlClusters" runat="server">
                </cc1:DropDownCheckBoxes>
            </td>
            <td>
                Objectives:
            </td>
            <td>
                <asp:DropDownList ID="ddlObjective" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlObjective_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Indicators:
            </td>
            <td>
                <asp:DropDownList ID="ddlIndicators" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlIndicators_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Activity:
            </td>
            <td>
                <asp:DropDownList ID="ddlActivities" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlActivities_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Data:
            </td>
            <td>
                <asp:DropDownList ID="ddlData" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Organization:
            </td>
            <td>
                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Organization Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlOrgTypes" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOrgTypes_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="6">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnExportToExcel" runat="server" Text="Export To Excel" OnClick="btnExportToExcel_Click" />
            </td>
            <td>
                <a id="reportlink" href="../../DataFeed.ashx">
                    <img src="../../images/rssfeed.png" /></a>
            </td>
            <td colspan="4">
                <asp:HiddenField ID="hfReportLink" runat="server" Value="" />
            </td>
        </tr>
    </table>
    <div style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvReport" runat="server" Width="100%" CssClass="gvr" OnRowDataBound="gvReport_RowDataBound"
            ShowHeaderWhenEmpty="true">
            <HeaderStyle BackColor="ButtonFace" />
        </asp:GridView>
    </div>
</asp:Content>
