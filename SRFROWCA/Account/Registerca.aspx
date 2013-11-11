<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Registerca.aspx.cs" Inherits="SRFROWCA.Account.Registerca" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updActivities" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMsg">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="text-align: center;">
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updActivities"
                    DynamicLayout="true">
                    <ProgressTemplate>
                        <img src="../images/ajaxlodr.gif" alt="Loading">
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="containerLogin">
                <div class="graybarLogin">
                    Register Yourself
                </div>
                <div class="contentarea">
                    <div class="formdiv">
                        <table border="0" width="100%">
                            <tr>
                                <td>
                                    <label>
                                        Name:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="ddlWidth"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Password Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Password:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"
                                        CssClass="ddlWidth"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                                        CssClass="error2" ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Confirm Password:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="128"
                                        CssClass="ddlWidth"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Confirm Password Required"
                                        CssClass="error2" Text="Required" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords don't match."
                                        CssClass="error2" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Email:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="ddlWidth"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                        Text="Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Phone:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="ddlWidth"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        <asp:Literal ID="ltrlLocation" runat="server" Text="Country:"></asp:Literal></label>
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
                        <div class="spacer" style="clear: both;">
                        </div>
                        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button_example"
                            OnClick="btnRegister_Click" />
                        <div class="spacerbig" style="clear: both;">
                        </div>
                        <div>
                            <asp:LinkButton ID="lbtnMissing" Style="color: Blue" runat="server" Text="Request Missing Organization Or Location"></asp:LinkButton>
                        </div>
                        <div class="spacer" style="clear: both;">
                        </div>
                    </div>
                </div>
                <div class="graybarcontainer">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
