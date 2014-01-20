<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmergencyListing.aspx.cs" Inherits="SRFROWCA.Admin.EmergencyListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">       
        
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvEmergency.ClientID %>');
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
                        <div id="divMsg">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnAddEmergency" runat="server" Text="Add New Emergency" CausesValidation="false"
                CssClass="button_example" OnClick="btnAddEmergency_Click" />
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
            <asp:GridView ID="gvEmergency" runat="server" AutoGenerateColumns="false" AllowSorting="True"
                OnRowCommand="gvEmergency_RowCommand" Width="100%" OnRowDataBound="gvEmergency_RowDataBound"
                CssClass="imagetable" OnSorting="gvEmergency_Sorting">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="EmergencyId" HeaderText="Emergency Id" SortExpression="EmergencyId" />
                    <asp:BoundField DataField="EmergencyName" HeaderText="Emergency Name" SortExpression="EmergencyName" />
                    <asp:BoundField DataField="EmergencyType" HeaderText="Disaster Type" SortExpression="EmergencyType" />                    
                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                                CommandName="EditEmergency" CommandArgument='<%# Eval("EmergencyId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                CommandName="DeleteEmergency" CommandArgument='<%# Eval("EmergencyId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>                    
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEmergencyTypeId" runat="server" Text='<%# Eval("EmergencyTypeId") %>'></asp:Label>
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
                <asp:ModalPopupExtender ID="mpeAddOrg" BehaviorID="mpeAddOrg" runat="server" TargetControlID="btntest"
                    PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOrg" runat="server" Width="800px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    Add/Edit Emergency
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    Emergency Name (English):
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmgNameEng" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgName" runat="server" ErrorMessage="Emg Name (Eng)"
                                                        Text="Required" ControlToValidate="txtEmgNameEng"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Emergency Name (French):
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmgNameFr" runat="server" Width="300px" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rfvEmgNameFr" runat="server" ErrorMessage="Emg Name (Fr)"
                                                        Text="Required" ControlToValidate="txtEmgNameFr"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Emergency Type:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlEmgType" runat="server" Width="300px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rgvEmgType" runat="server" ErrorMessage="Required"
                                                        InitialValue="0" Text="Required" ControlToValidate="ddlEmgType"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:HiddenField ID="hfLocEmgId" runat="server" />
                                                    <asp:Button ID="btnAdd" runat="server" Text="Add/Update" OnClick="btnAdd_Click" CssClass="button_example" />
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="button_example" />
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
    <div style="display: none">
        <asp:Button ID="btntest" runat="server" Width="1px" />
    </div>
</asp:Content>
