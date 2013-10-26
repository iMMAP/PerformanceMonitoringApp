<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OfficeListing.aspx.cs" Inherits="SRFROWCA.Admin.Office.OfficeListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .ModalPopupBG1
        {
            background-color: #446633;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup1
        {
            display: block;
            top: 10px;
            left: 0;
            width: 600px;
            height: 300px;
            padding: 5px;
            margin: 10px;
            z-index: 10;
            font: 12px Verdana, sans-serif;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvOffice.ClientID %>');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) {
                        $(row).show('fast');
                    }
                    else {
                        $(row).hide('fast');
                    }
                }
            });
        }          
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
        <tr>
            <td class="signupheading2" colspan="3">
                <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                            ViewStateMode="Disabled"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table style="margin: auto; width: 100%;">
        <tr>
            <td>
                <b>Search:</b>
                <input type="text" id="txtSearch" class="searchFieldOrgs" />
            </td>
            <td align="right">
                <asp:Button ID="btnAddOffice" runat="server" Text="Add New Office" />
            </td>
        </tr>
    </table>
    <div style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvOffice" runat="server" AutoGenerateColumns="false" AllowSorting="True"
            OnRowCommand="gvOffice_RowCommand" Width="100%" OnRowDataBound="gvOffice_RowDataBound"
            OnSorting="gvOffice_Sorting">
            <Columns>
                <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="OfficeId" HeaderText="Office Id" HeaderStyle-Width="100px"
                    SortExpression="OfficeId" />
                <asp:BoundField DataField="OfficeName" HeaderText="Office Name" SortExpression="OfficeName" />
                <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" />
                <asp:BoundField DataField="OrganizationAcronym" HeaderText="Org Acronym" SortExpression="OrganizationAcronym" />
                <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="LocationName" />
                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                            CommandName="EditOffice" CommandArgument='<%# Eval("OfficeId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                            CommandName="DeleteOffice" CommandArgument='<%# Eval("OfficeId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationId" runat="server" Text='<%# Eval("LocationId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrganizaitonId" runat="server" Text='<%# Eval("OrganizationId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationParentId" runat="server" Text='<%# Eval("LocationParentId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="ButtonFace" />
        </asp:GridView>
    </div>
    <table>
        <tr>
            <td>
                <asp:ModalPopupExtender ID="mpeAddOrg" BehaviorID="mpeAddOrg" runat="server" TargetControlID="btnAddOffice"
                    PopupControlID="pnlOrg" BackgroundCssClass="ModalPopupBG1" CancelControlID="btnClose">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOrg" runat="server" Width="800px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="HellowWorldPopup1">
                                <table border="0" align="center" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="popupbordertd">
                                            <table border="0" style="margin: auto;">
                                                <tr>
                                                    <td>
                                                        Organization:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlOrganizations" runat="server" Width="300px" ValidationGroup="Office">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rgvOrg" runat="server" ErrorMessage="Required" InitialValue="0"
                                                            ValidationGroup="Office" Text="Required" ControlToValidate="ddlOrganizations"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Country:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                                                            CausesValidation="false" AutoPostBack="true" ValidationGroup="none" Width="300px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                            InitialValue="0" Text="Required" ControlToValidate="ddlLocations"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Location:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlLocations" runat="server" ValidationGroup="Office" Width="300px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ErrorMessage="Required"
                                                            ValidationGroup="Office" InitialValue="0" Text="Required" ControlToValidate="ddlLocations"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Office Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOfficeName" runat="server" Width="300px" MaxLength="100" ValidationGroup="Office"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvOfficeName" runat="server" ErrorMessage="Office Name"
                                                            ValidationGroup="Office" Text="Required" ControlToValidate="txtOfficeName"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="right">
                                                        <asp:HiddenField ID="hfOfficeId" runat="server" />
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add/Update Office" ValidationGroup="Office"
                                                            OnClick="btnAdd_Click" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close Window" CausesValidation="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                                            ViewStateMode="Disabled"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnAdd" />
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
