<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllTargetsAndAchieved.ascx.cs"
    Inherits="SRFROWCA.Reports.AllTargetsAndAchieved" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<script src="../Scripts/hc/highcharts.js" type="text/javascript"></script>
<script type="text/javascript">
    
</script>
<div>
    <table width="100%" class="label1">
        <tr>
            <td class="formh01">
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Locations:
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
    <div>
        <asp:Literal ID="ltrChartLocation" runat="server"></asp:Literal>
    </div>
    <div>
        <asp:Literal ID="ltrChart" runat="server"></asp:Literal>
    </div>
</div>
