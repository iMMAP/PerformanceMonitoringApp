<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Anonymous.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ register assembly="SRFROWCA" namespace="SRFROWCA" tagprefix="cc2" %>
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
                        <img src="../images/ajaxlodr.gif" alt="loading...">
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="container">
                <div class="graybar">
                    Select your Activities
                </div>
                <div class="contentarea">
                    <div class="formdiv">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <label>
                                        Emergency:</label>
                                </td>
                                <td>
                                    <label>
                                        Cluster:
                                    </label>
                                </td>
                                <td>
                                    <label>
                                        Location:
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc:DropDownCheckBoxes ID="ddlEmergency" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged" AddJQueryReference="True"
                                        meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                        <Texts SelectBoxCaption="Select Emergency" />
                                    </cc:DropDownCheckBoxes>
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
                                    <label>
                                        Year:
                                    </label>
                                </td>
                                <td>
                                    <label>
                                        Month</label>
                                </td>
                                <td>
                                    <label>
                                        User:</label>
                                </td>
                            </tr>
                            <tr>
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
                                    <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AddJQueryReference="True"
                                        meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                        <Texts SelectBoxCaption="Select Month" />
                                    </cc:DropDownCheckBoxes>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Organization Type:</label>
                                </td>
                                <td>
                                    <label>
                                        Organization:</label>
                                </td>
                                <td>
                                    <label>
                                        Office:</label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <cc:DropDownCheckBoxes ID="ddlOrgTypes" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlOrgTypes_SelectedIndexChanged" AddJQueryReference="True"
                                        meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                        <Texts SelectBoxCaption="Select Org Type" />
                                    </cc:DropDownCheckBoxes>
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
                                    <asp:DropDownList ID="ddlOffice" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="spacer" style="clear: both;">
                    </div>
                </div>
                <div class="graybarcontainer">
                </div>
            </div>
            <div class="buttonsdiv">
                <div class="savebutton">
                    <asp:Button ID="btnExportToExcel" runat="server" CausesValidation="false" Text="Export To Excel"
                        CssClass="button_example" OnClick="btnExportToExcel_Click" />                        
                        <a id="reportlink" href="../DataFeed.ashx">
                            <input type="button" value="XML Of Data"  class="button_example"/></a>
                    <asp:HiddenField ID="hfReportLink" runat="server" Value="" />
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="imagetable" OnRowDataBound="gvReport_RowDataBound"
                    OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true" EnableViewState="false"
                    AllowSorting="True" RowStyle-Height="30" AllowPaging="true" PageSize="100" ShowHeader="true"
                    OnPageIndexChanging="gvReport_PageIndexChanging">
                    <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                    <PagerSettings Mode="NumericFirstLast" />
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                </cc2:PagingGridView>
            </div>
            <div class="fullwidthdiv" style="clear: both;"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
