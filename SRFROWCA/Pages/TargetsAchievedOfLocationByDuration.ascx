<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TargetsAchievedOfLocationByDuration.ascx.cs"
    Inherits="SRFROWCA.Pages.TargetsAchievedOfLocationByDuration" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>

<div>
    <table width="100%" class="label1" border='2'>
        <tr>
            <td>
                Data:
            </td>
            <td colspan="5">
                <asp:DropDownList ID="ddlDuration" runat="server" AutoPostBack="true" Width="100px"
                    OnSelectedIndexChanged="ddlDuration_SelectedIndexChanged">
                    <asp:ListItem Text="Monthly" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Quarterly" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Biannually" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Yearly" Value="4"></asp:ListItem>
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
                <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
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
    <div style="width: 100%;">
        <div style="width: 60%; float: left;">
            <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
        </div>
        <div style="width: 40%; float: left;">
            <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
        </div>
    </div>
</div>
