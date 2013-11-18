<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
    </div>
    <div class="container">
        <div class="graybar">
            Select Your Options To Report On
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                            <label>
                                Country:</label>
                        </td>
                        <td>
                            <asp:Label ID="lblCountry" runat="server" Text="" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <label>
                                Organization:</label>
                        </td>
                        <td>
                            <asp:Label ID="lblOrganization" runat="server"></asp:Label>
                        </td>
                        <td>
                            <label>
                                Emergency:</label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlEmergency" runat="server" Width="250px" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                InitialValue="0" Text="*" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Year:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="100px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                                <asp:ListItem Text="2010" Value="6"></asp:ListItem>
                                <asp:ListItem Text="2011" Value="7"></asp:ListItem>
                                <asp:ListItem Text="2012" Value="8"></asp:ListItem>
                                <asp:ListItem Text="2013" Value="9"></asp:ListItem>
                                <asp:ListItem Text="2014" Value="20"></asp:ListItem>
                                <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ErrorMessage="Select Year"
                                InitialValue="0" Text="*" ControlToValidate="ddlYear"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                Month:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" Width="100px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                                <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ErrorMessage="Select Month"
                                InitialValue="0" Text="*" ControlToValidate="ddlMonth"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                Office:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOffice" runat="server" Width="250px" AutoPostBack="true"
                                onchange="needToConfirm = false;" OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvOffice" runat="server" ErrorMessage="Select Office"
                                InitialValue="0" Text="*" ControlToValidate="ddlOffice"></asp:RequiredFieldValidator>
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
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="true" Width="120" CssClass="button_example" /></div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="false"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div class="tablegrid">
        <table border="0" cellpadding="2" cellspacing="0" class="quicksearch2">
            <tr>
                <td width="250px">
                    <input type="checkbox" id="chkShowHide" class="chkShowHide" />
                    <b>Show Only Checked:</b>
                </td>
                <td>
                    <b>Search:</b>
                    <input type="text" id="gridSearch" class="grdSearch" style="width: 400px;" />
                </td>
                <td align="right">
                </td>
                <td style="width: 40%;">
                </td>
            </tr>
        </table>
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:Panel CssClass="grid" ID="pnlCust" runat="server">
                <asp:UpdatePanel ID="pnlUpdate" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvClusters" runat="server" AutoGenerateColumns="false" ShowHeader="true"
                            ShowHeaderWhenEmpty="true" CssClass="imagetable" OnRowDataBound="gvClusters_RowDataBound"
                            Width="100%">
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <EmptyDataTemplate>
                                No Data
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Cluster">
                                    <ItemTemplate>
                                        <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                            <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                runat="server" /><span class="gridheader">
                                                    <%#Eval("ClusterName")%></span>
                                        </asp:Panel>
                                        <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                            <asp:GridView ID="gvStrObjectives" runat="server" AllowPaging="False" AllowSorting="False"
                                                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                CssClass="imagetable" Width="100%" OnRowDataBound="gvStrObjectives_RowDataBound">
                                                <RowStyle CssClass="istrow" />
                                                <AlternatingRowStyle CssClass="altcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="STR Objectives">
                                                        <ItemTemplate>
                                                            <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                    runat="server" /><span class="gridheader">
                                                                        <%#Eval("StrategicObjectiveName")%></span>
                                                            </asp:Panel>
                                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                <asp:GridView ID="gvSpcObjectives" runat="server" AllowPaging="False" AllowSorting="False"
                                                                    AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                                    CssClass="imagetable" Width="100%" OnRowDataBound="gvSpcObjectives_RowDataBound">
                                                                    <RowStyle CssClass="istrow" />
                                                                    <AlternatingRowStyle CssClass="altcolor" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="SPC Objectives">
                                                                            <ItemTemplate>
                                                                                <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                                    <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                                        runat="server" /><span class="gridheader">
                                                                                            <%#Eval("ObjectiveName")%></span>
                                                                                </asp:Panel>
                                                                                <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                    <asp:GridView ID="gvIndicators" runat="server" AllowPaging="False" AllowSorting="False"
                                                                                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                                                        CssClass="imagetable" Width="100%" OnRowDataBound="gvIndicators_RowDataBound">
                                                                                        <RowStyle CssClass="istrow" />
                                                                                        <AlternatingRowStyle CssClass="altcolor" />
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="Indicators">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                                                        <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                                                            runat="server" /><span class="gridheader">
                                                                                                                <%#Eval("IndicatorName")%></span>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                                        <asp:GridView ID="gvActivities" runat="server" AllowPaging="False" AllowSorting="False"
                                                                                                            AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                                                                            CssClass="imagetable" Width="100%" OnRowDataBound="gvActivities_RowDataBound">
                                                                                                            <RowStyle CssClass="istrow" />
                                                                                                            <AlternatingRowStyle CssClass="altcolor" />
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField HeaderText="Activities">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                                                                            <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                                                                                runat="server" /><span class="gridheader">
                                                                                                                                    <%#Eval("ActivityName")%></span>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                                                            <asp:GridView ID="gvData" runat="server" AllowPaging="False" AllowSorting="False"
                                                                                                                                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                                                                                                CssClass="imagetable" Width="100%" OnRowDataBound="gvData_RowDataBound">
                                                                                                                                <RowStyle CssClass="istrow" />
                                                                                                                                <AlternatingRowStyle CssClass="altcolor" />
                                                                                                                                <Columns>
                                                                                                                                    <asp:TemplateField HeaderText="Data">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Panel CssClass="group" ID="pnlCustomer" runat="server">
                                                                                                                                                <asp:Image ID="imgCollapsible" CssClass="first" ImageUrl="~/images/plus.png" Style="margin-right: 5px;"
                                                                                                                                                    runat="server" /><span class="gridheader">
                                                                                                                                                        <%#Eval("DataName")%></span>
                                                                                                                                            </asp:Panel>
                                                                                                                                            <asp:Panel Style="margin-left: 20px; margin-right: 20px" ID="pnlOrders" runat="server">
                                                                                                                                                <asp:GridView ID="gvLocations" runat="server" AllowPaging="False" AllowSorting="False"
                                                                                                                                                    AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                                                                                                                                                    CssClass="imagetable" Width="100%">
                                                                                                                                                    <RowStyle CssClass="istrow" />
                                                                                                                                                    <AlternatingRowStyle CssClass="altcolor" />
                                                                                                                                                    <Columns>
                                                                                                                                                        <asp:BoundField DataField="Location" HeaderText="Location" />
                                                                                                                                                        <asp:TemplateField HeaderText="Target 2014">
                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                <asp:TextBox ID="txtTarget" runat="server" Text='<%#Eval("Target")%>'></asp:TextBox>
                                                                                                                                                            </ItemTemplate>
                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                        <asp:TemplateField HeaderText="Target 2015">
                                                                                                                                                            <ItemTemplate>
                                                                                                                                                                <asp:TextBox ID="txtAchieved" runat="server" Text='<%#Eval("Achieved")%>'></asp:TextBox>
                                                                                                                                                            </ItemTemplate>
                                                                                                                                                        </asp:TemplateField>
                                                                                                                                                    </Columns>
                                                                                                                                                </asp:GridView>
                                                                                                                                            </asp:Panel>
                                                                                                                                            <asp:CollapsiblePanelExtender ID="cpeActivities" runat="Server" TargetControlID="pnlOrders"
                                                                                                                                                CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                                                                                                AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                                                                                                ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                </Columns>
                                                                                                                            </asp:GridView>
                                                                                                                        </asp:Panel>
                                                                                                                        <asp:CollapsiblePanelExtender ID="cpeActivities" runat="Server" TargetControlID="pnlOrders"
                                                                                                                            CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                                                                            AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                                                                            ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                    <asp:CollapsiblePanelExtender ID="cpeIndicator" runat="Server" TargetControlID="pnlOrders"
                                                                                                        CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                                                        AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                                                        ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                                                                                </ItemTemplate>
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </asp:Panel>
                                                                                <asp:CollapsiblePanelExtender ID="cpeSpcObj" runat="Server" TargetControlID="pnlOrders"
                                                                                    CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                                    AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                                    ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                            <asp:CollapsiblePanelExtender ID="cpeStrObj" runat="Server" TargetControlID="pnlOrders"
                                                                CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                                                AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                                                ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <asp:CollapsiblePanelExtender ID="cpeCluster" runat="Server" TargetControlID="pnlOrders"
                                            CollapsedSize="0" Collapsed="True" ExpandControlID="pnlCustomer" CollapseControlID="pnlCustomer"
                                            AutoCollapse="False" AutoExpand="False" ScrollContents="false" ImageControlID="imgCollapsible"
                                            ExpandedImage="~/images/minus.png" CollapsedImage="~/images/plus.png" ExpandDirection="Vertical" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="true" Width="120px" CssClass="button_example" /></div>
        <div class="buttonright">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" BehaviorID="mpeAddActivity" runat="server"
                    TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="ModalPopupBG1">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="700px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="HellowWorldPopup1">
                                <table width="50%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="63" colspan="3" bgcolor="#FFFFFF" style="border-left: #9db7df  4px solid;
                                            border-top: #9db7df  4px solid; border-right: #9db7df  4px solid; border-bottom: #9db7df  4px solid">
                                            <table border="0" style="margin: auto; background-color: Gray">
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Label ID="lblLocationLevelOfCountry" runat="server" Text="" BackColor="White"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: ButtonFace;">
                                                    <td>
                                                        Locations:
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td style="background-color: ButtonFace;">
                                                        Selected Locations:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 45%">
                                                        <asp:ListBox ID="lstLocations" runat="server" Height="180px" Width="315px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </td>
                                                    <td class="TitleCellBackgroud" align="center">
                                                        <%--<asp:Button ID="btnAddAll" runat="server" Text="&gt;&gt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnAddAll_Click" />--%>
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnAdd" runat="server" Text="&gt;" Height="30px" Width="50px" CausesValidation="false"
                                                            OnClick="btnAdd_Click" />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnRemove" runat="server" Text="&lt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnRemove_Click" />
                                                        <br />
                                                        <br />
                                                        <%--<asp:Button ID="btnRemoveAll" runat="server" Text="&lt;&lt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnRemoveAll_Click" />--%>
                                                    </td>
                                                    <td style="width: 45%">
                                                        <asp:ListBox ID="lstSelectedLocations" runat="server" Height="180px" Width="315px"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" Width="300px" Height="40px"
                                                            CausesValidation="false" OnClientClick="needToConfirm = false;" />
                                                        <%--<asp:Button ID="btnGetReports" runat="server" Text="Get Location Reports" OnClick="btnGetReports_Click"
                                                            Width="300px" Height="40px" CausesValidation="false" />--%>
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
