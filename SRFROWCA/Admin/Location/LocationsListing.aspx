<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="LocationsListing.aspx.cs" Inherits="SRFROWCA.Admin.LocationsListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:Button ID="btnExcel" runat="server" Text="Export To Excel" OnClick="btnExcel_Click" />
                <asp:Button ID="btnWord" runat="server" Text="Export To Word" OnClick="btnWord_Click" />
                <asp:Button ID="btnCSV" runat="server" Text="Export To CSV" OnClick="btnCSV_Click" />
                <asp:Button ID="btnPDF" runat="server" Text="Export To PDF" OnClick="btnPDF_Click" />
                <asp:Button ID="btnAddLocation" runat="server" Text="Add Location" OnClick="btnAddLocation_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="radgridLocations">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="radgridLocations" LoadingPanelID="RadAjaxLoadingPanel1" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/ajaxloader.gif"></asp:Image>
                </telerik:RadAjaxLoadingPanel>
                <telerik:RadGrid ID="radgridLocations" runat="server" AllowPaging="False"
                    OnItemCommand="radgridLocations_ItemCommand" OnNeedDataSource="radgridLocations_NeedDataSource" GridLines="Both"
                    AutoGenerateColumns="False" CellSpacing="0" AllowSorting="True" AllowFilteringByColumn="True"
                    ShowGroupPanel="True" AllowMultiRowSelection="False">
                    <ClientSettings AllowDragToGroup="True" AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                        <Selecting AllowRowSelect="True" />
                        <Resizing AllowColumnResize="True" AllowRowResize="True" 
                            EnableRealTimeResize="True" ResizeGridOnColumnResize="True" />
                    </ClientSettings>
                    <MasterTableView>
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="LocationId" FilterControlAltText="Location Id"
                                HeaderText="Id" UniqueName="LocationId" ReadOnly="True">
                                <ItemStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="LocationName" FilterControlAltText="Location Name"
                                HeaderText="Location" UniqueName="LocationName" ReadOnly="True">
                                <ItemStyle Font-Bold="True" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="LocationTypeName" FilterControlAltText="LocationTypeName"
                                HeaderText="Location Type" UniqueName="LocationTypeName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="ProvinceName" FilterControlAltText="ProvinceName"
                                HeaderText="Region" UniqueName="ProvinceName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="DistrictName" FilterControlAltText="DistrictName"
                                HeaderText="Country" UniqueName="DistrictName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TehsilName" FilterControlAltText="TehsilName"
                                HeaderText="Governorate" UniqueName="TehsilName">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="UCName" FilterControlAltText="UCName" HeaderText="District"
                                UniqueName="UCName">
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                HeaderText="Edit" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Button ID="btnMapLocation" runat="server" Text="Edit" CommandName="MapLocation"
                                        CommandArgument='<%# Eval("LocationId") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                HeaderText="Delete" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Button ID="btnDeleteLocation" runat="server" Text="Delete" CommandName="DeleteLocation"
                                        ImageUrl="~/images/error.png" CommandArgument='<%# Eval("LocationId") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                Visible="false" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblFullLocationName" runat="server" Text='<%# Eval("LocationFullName") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                Visible="false" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationId" runat="server" Text='<%# Eval("LocationId") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                Visible="false" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblLat" runat="server" Text='<%# Eval("Latitude") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                Visible="false" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblLng" runat="server" Text='<%# Eval("Longitude") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="False" Groupable="False" ReadOnly="True"
                                Visible="false" ShowSortIcon="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblLocationType" runat="server" Text='<%# Eval("LocationTypeName") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                </telerik:RadGrid>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Button ID="btnAddNewSP" runat="server" Text="To Hide" />
                <asp:ModalPopupExtender ID="mpeAddActivity" BehaviorID="mpeAddAcitivty" runat="server"
                    TargetControlID="btnAddNewSP" PopupControlID="Panel1" BackgroundCssClass="ModalPopupBG1">
                </asp:ModalPopupExtender>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="HellowWorldPopup1">
                                <table width="50%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="8" height="48" valign="bottom">
                                            <img src="../images/pl.png" width="8" height="65" />
                                        </td>
                                        <td width="1268" valign="bottom" class="popupcent">
                                            <div class="poptcent">
                                                <img src="../images/addnewactivity.png" width="85" height="65" /></div>
                                            <div class="poph11">
                                                Update Location In Reports</div>
                                            <div class="tpclose1">
                                                <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/images/close_btn.png" /></div>
                                        </td>
                                        <td width="8" valign="bottom">
                                            <img src="../images/pr.png" width="8" height="48" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="63" colspan="3" bgcolor="#FFFFFF" style="border-left: #9db7df  4px solid;
                                            border-right: #9db7df  4px solid; border-bottom: #9db7df  4px solid">
                                            <table width="90%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <div style="text-align: center;">
                                                    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="uPanel1"
                                                        DynamicLayout="true">
                                                        <ProgressTemplate>
                                                            <img src="../images/ajaxlodr.gif">
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                                <tr>
                                                    <td height="45" colspan="2" valign="middle">
                                                        <div class="poph31">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            Type:</div>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlLocationType" runat="server" CssClass="input1" Width="200px"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlLocationType_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select Location Type" Value="0" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Province" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="District" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Tehsil" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="UC" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Village" Value="6"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            Province:</div>
                                                    </td>
                                                    <td width="77%" height="45">
                                                        <label for="textfield3">
                                                        </label>
                                                        <asp:DropDownList ID="ddlProvince" runat="server" Width="200px" CssClass="input"
                                                            OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            District:</div>
                                                    </td>
                                                    <td height="45">
                                                        <asp:DropDownList ID="ddlDistrict" runat="server" Width="200px" CssClass="input"
                                                            OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            Tehsil:</div>
                                                    </td>
                                                    <td height="45">
                                                        <asp:DropDownList ID="ddlTehsil" runat="server" Width="200px" CssClass="input" OnSelectedIndexChanged="ddlTehsil_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            UC:</div>
                                                    </td>
                                                    <td height="45">
                                                        <asp:DropDownList ID="ddlUC" runat="server" Width="200px" CssClass="input" OnSelectedIndexChanged="ddlUC_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="45">
                                                        <div class="poph41">
                                                            Village:</div>
                                                    </td>
                                                    <td height="45">
                                                        <asp:DropDownList ID="ddlVillage" runat="server" Width="200px" CssClass="input">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="20" colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td height="71">
                                                        <asp:Button ID="btnUpdateLocation" runat="server" OnClick="btnUpdateLocation_Click"
                                                            CssClass="button1" Text="Update" ValidationGroup="vgNewActivity" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOrgMessage" runat="server" ViewStateMode="Disabled"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
