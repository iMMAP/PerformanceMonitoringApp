<%@ Page Title="" Language="C#" MasterPageFile="~/NestedMasterPage1.master" AutoEventWireup="true"
    CodeBehind="TargetsAchievedByLocDuration.aspx.cs" Inherits="SRFROWCA.Reports.TargetsAchievedByLocDuration" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" class="label1" border='1'>
        <tr>
            <td>
                Data:
            </td>
            <td colspan="7">
                <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="100px"
                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Admin1:
            </td>
            <td>
                <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Admin2:
            </td>
            <td>
                <asp:DropDownList ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Duration:
            </td>
            <td>
                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                    <asp:ListItem Text="2010" Value="6"></asp:ListItem>
                    <asp:ListItem Text="2011" Value="7"></asp:ListItem>
                    <asp:ListItem Text="2012" Value="8"></asp:ListItem>
                    <asp:ListItem Text="2013" Value="9" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="2014" Value="10"></asp:ListItem>
                    <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                    <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                    <asp:ListItem Text="2017" Value="13"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlDuration" runat="server" AutoPostBack="true" Width="100px"
                    OnSelectedIndexChanged="ddlDuration_SelectedIndexChanged">
                    <asp:ListItem Text="Monthly" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Quarterly" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Yearly" Value="3"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <div style="width: 60%; float: left;">
            <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
        </div>
        <div style="width: 40%; float: left;">
            <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
