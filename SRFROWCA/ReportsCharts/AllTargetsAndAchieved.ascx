<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTargetsAndAchieved.ascx.cs"
    Inherits="SRFROWCA.Reports.AllTargetsAndAchieved" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<style type="text/css">
    .ddlWidth
    {
        width: 200px;
    }
</style>

<div>
    <table width="100%" class="label1" border='0'>
        <tr>
            <td class="formh01">
                Data:
            </td>
            <td colspan="5">
                <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="900px" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="formh01">
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="100px"
                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td class="formh01">
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
