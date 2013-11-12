<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="OrganizationListing.aspx.cs" Inherits="SRFROWCA.Admin.OrganizationListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvOrgs.ClientID %>');
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
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnExportToExcel" runat="server" CausesValidation="false" Text="Export To Excel"
                CssClass="button_example" OnClick="btnExportToExcel_Click" />
            <asp:Button ID="btnAddOrganization" runat="server" Text="Add New Organization" CssClass="button_example" />
        </div>
        <div class="quicksearch">
            Quick Search :
            <input type="text" id="txtSearch" class="searchFieldOrgs" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div class="tablegrid">
        <div style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvOrgs" runat="server" AutoGenerateColumns="false" AllowSorting="True"
                AllowPaging="true" PageSize="100" OnRowCommand="gvOrgs_RowCommand" Width="100%"
                OnRowDataBound="gvOrgs_RowDataBound" OnSorting="gvOrgs_Sorting" OnPageIndexChanging="gvOrgs_PageIndexChanging"
                CssClass="imagetable">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="25px" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrganizationId" HeaderText="Org Id" HeaderStyle-Width="100px"
                        SortExpression="OrganizationId" />
                    <asp:TemplateField HeaderText="Organization" SortExpression="OrganizationName">
                        <ItemTemplate>
                            <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrganizationName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acronym" SortExpression="OrganizationAcronym">
                        <ItemTemplate>
                            <asp:Label ID="lblOrgAcronym" runat="server" Text='<%# Eval("OrganizationAcronym") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="OrganizationType" HeaderText="Org Type" SortExpression="OrganizationType" />
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                                CommandName="EditOrg" CommandArgument='<%# Eval("OrganizationId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                CommandName="DeleteOrg" CommandArgument='<%# Eval("OrganizationId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblOrgTypeId" runat="server" Text='<%# Eval("OrganizationTypeId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle BackColor="ButtonFace" />
            </asp:GridView>
        </div>
    </div>
    <table>
        <tr>
            <td>
                <asp:ModalPopupExtender ID="mpeAddOrg" BehaviorID="mpeAddOrg" runat="server" TargetControlID="btnAddOrganization"
                    PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOrg" runat="server" Width="800px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    Add/Edit Organizations
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    Organzation Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrgTypes" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rgvOrgType" runat="server" ErrorMessage="Required"
                                                        InitialValue="0" Text="Required" ControlToValidate="ddlOrgTypes"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Organization Name:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrgName" runat="server" Width="300px" MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvOrgName" runat="server" ErrorMessage="Org Name"
                                                        Text="Required" ControlToValidate="txtOrgName"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Organization Acronym:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrgAcronym" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvOrgAcronym" runat="server" ErrorMessage="Org Acronym"
                                                        Text="Required" ControlToValidate="txtOrgAcronym"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:HiddenField ID="hfOrgId" runat="server" />
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add/Update Organization" OnClick="btnAdd_Click" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Close Window" CausesValidation="false" />
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
