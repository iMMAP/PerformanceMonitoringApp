<%@ Page Title="3W Activities - Home" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<%--Custom GridView Class to include custom paging functionality.--%>
<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .ddlWidth
        {
            width: 300px;
        }
    </style>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            // Make XML DataFeed Link using hidden field
            var hf = $("#<%=hfReportLink.ClientID%>").val();
            var hl = $('#reportlink').attr("href");
            $('#reportlink').attr("href", hl + hf);

            // Add colgroup on top of the grid so kiketable_closizable jQuery script can work.
            // This script is to give user funcationality to increase/decrease width of grid column.
            $("#<%=gvReport.ClientID %>").prepend('<colgroup><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /><col /></colgroup>');
            $("#<%=gvReport.ClientID %>").kiketable_colsizable();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="text-align: center;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                    DynamicLayout="true">
                    <ProgressTemplate>
                        <img src="../images/ajaxlodr.gif">
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
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
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnExportToExcel" runat="server" CausesValidation="false" Text="Export To Excel"
                            CssClass="buttonA" OnClick="btnExportToExcel_Click" />
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
            <div id="dlg" class="dialog" style="width: 100%">
                <div class="gridheader" style="cursor: default">
                    <div class="outer">
                        <div class="inner">
                            <div class="content">
                                <h2>
                                </h2>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="overflow-x: auto; width: 100%">
                    <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="grid" OnRowDataBound="gvReport_RowDataBound"
                        OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true" EnableViewState="false"
                        AllowSorting="True" RowStyle-Height="30" AllowPaging="true" PageSize="10" ShowHeader="true"
                        OnPageIndexChanging="gvReport_PageIndexChanging">
                        <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                        <PagerSettings Mode="NumericFirstLast" />
                    </cc2:PagingGridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
