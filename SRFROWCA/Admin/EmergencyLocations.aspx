<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmergencyLocations.aspx.cs" Inherits="SRFROWCA.Admin.EmergencyLocations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
    <div class="containerLogin">
        <div class="graybarLogin">
            Add/Remove Locations In Emergency
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="width: 50%; margin: 0 auto;">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlEmergencies" runat="server" OnSelectedIndexChanged="ddlEmergencies_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rgvEmg" runat="server" ErrorMessage="Required" InitialValue="0"
                                Text="Required" ControlToValidate="ddlEmergencies"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBoxList ID="cblLocations" CssClass="cb" runat="server" RepeatColumns="3">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="button_example" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="spacer" style="clear: both;">
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
</asp:Content>
