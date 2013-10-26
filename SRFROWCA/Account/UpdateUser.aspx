<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UpdateUser.aspx.cs" Inherits="SRFROWCA.Account.UpdateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-color: Silver;">
        Create User Account</div>
    <div style="width: 75%; float: left">
        <table>
            <tr>
                <td>
                    User Name:
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="150" Enabled="false" style="background-color:ButtonFace;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Email:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Phone:
                </td>
                <td>
                    <asp:TextBox ID="txtPhone" MaxLength="50" runat="server"></asp:TextBox>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Organization:
                </td>
                <td>
                    <asp:DropDownList ID="ddlOrganization" runat="server" Width="200px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div id="divCountry" runat="server">
            <table>
                <tr id="trCountry">
                    <td>
                        Country:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCountry" runat="server" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Country"
                            Text="*" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divLocation" runat="server">
            <table>
                <tr id="trLocations">
                    <td>
                        <asp:Literal ID="ltrlLocation" runat="server" Text="Country"></asp:Literal>
                    </td>
                    <td>
                        <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth">
                            <Texts SelectBoxCaption="Select Country" />
                        </cc:DropDownCheckBoxes>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        
        <div>
            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
            <table cellpadding="5" cellspacing="0" class="pstyle1">
                <tr>
                    <td class="signupheading2">
                        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnUpdate" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:ValidationSummary ID="valSumProjectGeneralInfo" HeaderText="Following fields are required:"
                            runat="server" ShowMessageBox="True" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
