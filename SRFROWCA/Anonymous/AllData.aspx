<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Anonymous.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
    <%--DropDownCheckBoxes is custom dropdown with checkboxes to selectect multiple items.--%>
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
    <%--Custom GridView Class to include custom paging functionality.--%>
    <%@ register assembly="SRFROWCA" namespace="SRFROWCA" tagprefix="cc2" %>
    <style>
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            /* min-width: 200px;*/
            min-height: 150px;
            background: white;
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

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="graybar">
                    Select Your Report Options
                </div>
                <div class="contentarea">
                    <div class="formdiv">
                        <div class="divleft">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <label>
                                            Emergency:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlEmergency" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Emergency" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Cluster:
                                        </label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlClusters" runat="server" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"
                                            AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                            AutoPostBack="true" CssClass="ddlWidth" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Clusters" />
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
                                        <asp:DropDownList ID="ddlYear" runat="server" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                            <asp:ListItem Text="Select Year" Value="0" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="2010" Value="6"></asp:ListItem>
                                            <asp:ListItem Text="2011" Value="7"></asp:ListItem>
                                            <asp:ListItem Text="2012" Value="8"></asp:ListItem>
                                            <asp:ListItem Text="2013" Value="9"></asp:ListItem>
                                            <asp:ListItem Text="2014" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label>
                                            Month:</label>
                                        <cc:DropDownCheckBoxes ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                            AddJQueryReference="True" meta:resourcekey="checkBoxes2Resource1" UseButtons="False"
                                            UseSelectAllNode="True">
                                            <Style SelectBoxWidth="120px" DropDownBoxBoxWidth="200px" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Month" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="divcen">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <label>
                                            Country:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" CssClass="ddlWidth"
                                            OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Admin1:</label>
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
                                <tr>
                                    <td>
                                        <label>
                                            Admin2:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlAdmin2" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlAdmin2_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Location" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="divright">
                            <table width="100%">
                                <tr>
                                    <td>
                                        <label>
                                            Organization Type:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlOrgTypes" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOrgTypes_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Org Type" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Organization:</label>
                                    </td>
                                    <td>
                                        <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged" AddJQueryReference="True"
                                            meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Organization" />
                                        </cc:DropDownCheckBoxes>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Office:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlOffice" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
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
                        <input type="button" value="XML Of Data" class="button_example" /></a>
                    <asp:HiddenField ID="hfReportLink" runat="server" Value="" />
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <cc2:PagingGridView ID="gvReport" runat="server" Width="100%" CssClass="imagetable"
                    AutoGenerateColumns="false" OnSorting="gvReport_Sorting" ShowHeaderWhenEmpty="true"
                    EnableViewState="false" AllowSorting="True" RowStyle-Height="30" AllowPaging="true"
                    PageSize="100" ShowHeader="true" OnPageIndexChanging="gvReport_PageIndexChanging">
                    <PagerStyle BackColor="#efefef" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                    <PagerSettings Mode="NumericFirstLast" />
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="Emergency" HeaderText="Emergency" SortExpression="Emergency"
                            HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="OrganizationAcronym" HeaderText="Organization" SortExpression="OrganizationAcronym"
                            HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster"
                            HeaderStyle-Width="6%" />
                        <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective"
                            HeaderStyle-Width="10%" />
                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator"
                            HeaderStyle-Width="10%" />
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity"
                            HeaderStyle-Width="10%" />
                        <asp:BoundField DataField="Data" HeaderText="Data" SortExpression="Data" HeaderStyle-Width="10%" />
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country"
                            HeaderStyle-Width="5%" />
                        <asp:BoundField DataField="(AD1)Location" HeaderText="Admin1" SortExpression="(AD1)Location"
                            HeaderStyle-Width="6%" />
                        <asp:BoundField DataField="(Ad2)Location" HeaderText="Admin2" SortExpression="(AD2)Location"
                            HeaderStyle-Width="6%" />
                        <asp:BoundField DataField="Target" HeaderText="Target" SortExpression="Target" HeaderStyle-Width="3%"
                            ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" SortExpression="Achieved"
                            HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="WorkDone" HeaderText="%" SortExpression="WorkDone" HeaderStyle-Width="2%"
                            ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                </cc2:PagingGridView>
            </div>
            <div class="fullwidthdiv" style="clear: both;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenTargetControlForModalPopup"
        PopupControlID="Panel1" Drag="true" BackgroundCssClass="ModalPopupBG">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="HiddenTargetControlForModalPopup" Style="display: none" />
    <asp:Panel ID="Panel1" Style="display: block; width: 550px;" runat="server">
        <div class="containerPopup">
            <div class="graybar">
                Add/Remove Columns
            </div>
            <div class="contentarea">
                <div class="formdiv">
                    <table border="0" style="margin: 0 auto;">
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cbColumns" runat="server" RepeatColumns="4">
                                    <asp:ListItem Text="DataId" Value="DataId"></asp:ListItem>
                                    <asp:ListItem Text="Organization Full Name" Value="Organization"></asp:ListItem>
                                    <asp:ListItem Text="Office" Value="Office"></asp:ListItem>
                                    <asp:ListItem Text="(AD1)PCode" Value="(AD1)PCode"></asp:ListItem>
                                    <asp:ListItem Text="(Ad2)PCode" Value="(Ad2)PCode"></asp:ListItem>
                                    <asp:ListItem Text="UserName" Value="UserName"></asp:ListItem>
                                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                    <asp:ListItem Text="ReportDate" Value="ReportDate"></asp:ListItem>
                                    <asp:ListItem Text="Unit" Value="Unit"></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="button_example" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" OnClick="btnClose_Click"
                                    CssClass="button_example" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="spacer" style="clear: both;">
                    </div>
                </div>
            </div>
            <div class="graybarcontainer">
            </div>
        </div>
    </asp:Panel>
</asp:Content>
