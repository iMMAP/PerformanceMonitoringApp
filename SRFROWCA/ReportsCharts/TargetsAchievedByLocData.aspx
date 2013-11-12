<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true"
    CodeBehind="TargetsAchievedByLocData.aspx.cs" Inherits="SRFROWCA.Reports.TargetsAchievedByLocData" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="http://code.highcharts.com/modules/exporting.js" type="text/javascript"></script>
    <script src="http://code.highcharts.com/highcharts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="divleftchartfilter">
        <table width="100%" class="label1" border='0'>
            <tr>
                <td>
                    Data:
                </td>
                <td colspan="5">
                    <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Country:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="200px"
                        OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Admin1:
                </td>
                <td>
                    <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" AddJQueryReference="True"
                        meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                        <Texts SelectBoxCaption="Select Location" />
                    </cc:DropDownCheckBoxes>
                </td>
            </tr>
            <td>
                Admin2:
            </td>
            <td>
                <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
            </tr>
        </table>
    </div>
    <div class="divrightchartfilter">
        <div style="width: 100%;">
            <div style="width: 54%; float: left;">
                <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
                <asp:HiddenField ID="hfChart" runat="server" ViewStateMode="Disabled" />
            </div>
            <div style="width: 44%; float: right;">
                <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
