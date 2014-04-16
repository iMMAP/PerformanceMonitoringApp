<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="ChangePassword.aspx.cs" Inherits="SRFROWCA.Account.ChangePassword" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbChangePassword" runat="server" Text="Change Password" meta:resourcekey="localBreadCrumbChangePasswordResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="col-sm-offset-1 col-sm-10">
        <div class="space">
        </div>
        <div class="tab-content profile-edit-tab-content">
            <div id="edit-basic" class="tab-pane in active">
                <div class="vspace-xs">
                </div>
                <asp:ChangePassword ID="ChangeUserPassword" runat="server" CancelDestinationPageUrl="~/"
                    EnableViewState="False" RenderOuterTable="False" SuccessPageUrl="ChangePasswordSuccess.aspx" meta:resourcekey="ChangeUserPasswordResource1">
                    <ChangePasswordTemplate>
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="FailureText" runat="server" CssClass="error2" EnableViewState="False" meta:resourcekey="FailureTextResource1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword" meta:resourcekey="CurrentPasswordLabelResource1">Old Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"
                                        MaxLength="128" Width="300px" meta:resourcekey="CurrentPasswordResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                        CssClass="error2" ErrorMessage="Password is required." ToolTip="Old Password is required." meta:resourcekey="CurrentPasswordRequiredResource1">Required.</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword" meta:resourcekey="NewPasswordLabelResource1">New Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"
                                        MaxLength="128" Width="300px" meta:resourcekey="NewPasswordResource1"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                        CssClass="error2" ErrorMessage="New Password is required." ToolTip="New Password is required." meta:resourcekey="NewPasswordRequiredResource1">Required.</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword" meta:resourcekey="ConfirmNewPasswordLabelResource1">Confirm New Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password"
                                        MaxLength="128" Width="300px" meta:resourcekey="ConfirmNewPasswordResource1"></asp:TextBox>
                                </td>
                                <td style="width: 300px">
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                        CssClass="error2" Display="Dynamic" ErrorMessage="Confirm New Password is required."
                                        ToolTip="Confirm New Password is required." meta:resourcekey="ConfirmNewPasswordRequiredResource1">Required.</asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                        ControlToValidate="ConfirmNewPassword" CssClass="error2" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry." meta:resourcekey="NewPasswordCompareResource1">New Password Don't Match.</asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        Text="Cancel" CssClass="btn btn-default" meta:resourcekey="CancelPushButtonResource1" />
                                    <asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
                                        Text="Change Password" CssClass="btn btn-primary" meta:resourcekey="ChangePasswordPushButtonResource1" />
                                </td>
                            </tr>
                        </table>
                    </ChangePasswordTemplate>
                </asp:ChangePassword>
            </div>
        </div>
    </div>
</asp:Content>
