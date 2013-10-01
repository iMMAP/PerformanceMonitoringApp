<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterCriteria.ascx.cs"
    Inherits="SRFROWCA.Controls.FilterCriteria" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<table width="100%" class="label1">
    <tr>
        <td class="formh01">
            Emergency:
        </td>
        <td>
            <cc:DropDownCheckBoxes ID="ddlEmergency" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged" AddJQueryReference="True"
                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                <Texts SelectBoxCaption="Select Emergency" />
            </cc:DropDownCheckBoxes>
        </td>
        <td>
            Cluster:
        </td>
        <td>
            <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"
                AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                AutoPostBack="true" CssClass="ddlWidth" UseSelectAllNode="True">
                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                <Texts SelectBoxCaption="Select Clusters" />
            </cc:DropDownCheckBoxes>
        </td>
        <td>
            Location:
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
    <tr>
        <td>
            Year:
        </td>
        <td>
            <asp:DropDownList ID="ddlYear" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                <asp:ListItem Text="Select Year" Value="0" Selected="True"></asp:ListItem>
                <asp:ListItem Text="2010" Value="6"></asp:ListItem>
                <asp:ListItem Text="2011" Value="7"></asp:ListItem>
                <asp:ListItem Text="2012" Value="8"></asp:ListItem>
                <asp:ListItem Text="2013" Value="9"></asp:ListItem>
                <asp:ListItem Text="2014" Value="20"></asp:ListItem>
                <asp:ListItem Text="2015" Value="11"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            Month:
        </td>
        <td>
            <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AddJQueryReference="True"
                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                <Texts SelectBoxCaption="Select Month" />
            </cc:DropDownCheckBoxes>
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
            Organization Type:
        </td>
        <td>
            <cc:DropDownCheckBoxes ID="ddlOrgTypes" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlOrgTypes_SelectedIndexChanged" AddJQueryReference="True"
                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                <Texts SelectBoxCaption="Select Org Type" />
            </cc:DropDownCheckBoxes>
        </td>
        <td>
            Organization:
        </td>
        <td>
            <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged" AddJQueryReference="True"
                meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                <Texts SelectBoxCaption="Select Organization" />
            </cc:DropDownCheckBoxes>
        </td>
        <td>
            Office:
        </td>
        <td>
            <asp:DropDownList ID="ddlOffice" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td colspan="6">
        </td>
    </tr>
</table>

