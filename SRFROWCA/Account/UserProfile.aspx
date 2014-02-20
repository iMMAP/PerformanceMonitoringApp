<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="SRFROWCA.Account.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
    </div>
    <div class="containerLogin">
        <div class="graybarLogin">
            My Profile
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <div class="singalselect">
                    <label>
                        User Name:</label>
                    <div>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="ddlWidthAllData" Enabled="false"
                            Style="background-color: ButtonFace;"></asp:TextBox>
                    </div>
                </div>
                <div class="singalselect">
                    <label>
                        Email:
                    </label>
                    <div>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ddlWidthAllData"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtEmail"
                            Text="Required" ErrorMessage="Email" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="singalselect">
                    <label>
                        Phone:</label>
                    <div>
                        <asp:TextBox ID="txtPhone" MaxLength="50" CssClass="ddlWidthAllData" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div id="divCountry" runat="server" class="singalselect">
                    <label>
                        Country:</label>
                    <div>
                        <asp:DropDownList ID="ddlCountry" runat="server" Width="400px">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Country"
                            Text="*" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="spacer" style="clear: both;">
            </div>
        </div>
        <div>
            <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                CssClass="button_example" />
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
            </table>
        </div>
        <div class="spacer" style="clear: both;">
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
</asp:Content>
