<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="SRFROWCA.Account.Register" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../ContactUs/ContactUsControl.ascx" TagName="ContactUsControl"
    TagPrefix="uc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
            if ($('#<%=rbtnUser.ClientID%>').is(':checked')) {
                $('#chkCountriesMessage').text('Normal user, can only be registered in one country and can enter data.');
                $('#trLocations').hide();
                $('#trCountry').show();
            }

            if ($('#<%=rbtnCountryAdmin.ClientID%>').is(':checked')) {
                $('#chkCountriesMessage').text('Country Admin. Country admin can be in multiple countries. This user can administrate country level data.');
                $('#trLocations').show();
                $('#trCountry').hide();
                var validator = document.getElementById('<%=rfvCountry.ClientID%>');                
                ValidatorEnable(validator, false);
            }

            $('#<%=rbtnUser.ClientID%>').change(function () {
                if ($('#<%=rbtnUser.ClientID%>').is(':checked')) {
                    $('#chkCountriesMessage').text('Normal user, can only be registered in one country and can enter data.');
                    $('#trLocations').hide();
                    $('#trCountry').show();
                }
            });

            $('#<%=rbtnCountryAdmin.ClientID%>').change(function () {
                if ($('#<%=rbtnCountryAdmin.ClientID%>').is(':checked')) {
                    $('#chkCountriesMessage').text('Country Admin. Country admin can be in multiple countries. This user can administrate country level data.');
                    $('#trLocations').show();
                    $('#trCountry').hide();
                    var validator = document.getElementById('<%=rfvCountry.ClientID%>');                    
                    ValidatorEnable(validator, false);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="background-color: Silver;">
        Create User Account</div>
    <div style="width: 75%; float: left">
        <table>
            <tr>
                <td>
                    User Name:
                </td>
                <td>
                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="User Name"
                        Text="*" ControlToValidate="txtUserName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Password:
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="128"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password"
                        Text="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="valMinLength" runat="server" ControlToValidate="txtPassword"
                        ErrorMessage="At least 3 characters" ValidationExpression="[^\s]{3,}"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Confirm Password:
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="128"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Confirm Password"
                        Text="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="Passwords don't match."
                        ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Email:
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email" Text="*"
                        ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
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
                <td>
                    <asp:RequiredFieldValidator ID="rfvOrganization" runat="server" ErrorMessage="Organization"
                        InitialValue="0" Text="*" ControlToValidate="ddlOrganization"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <div id="divUserRoles" runat="server">
            <asp:RadioButton ID="rbtnUser" runat="server" Text="User" GroupName="Roles" Checked="true" />
            <asp:RadioButton ID="rbtnCountryAdmin" runat="server" Text="Country Admin" GroupName="Roles" />
        </div>
        <div id="chkCountriesMessage">
        </div>
        <table>
            <tr id="trCountry">
                <td width="109px">
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
            <tr id="trLocations">
                <td width="109px">
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
        <div>
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
            <asp:LinkButton ID="lbtnMissing" runat="server" Text="Request Missing Organization Or Location"></asp:LinkButton>
            <table cellpadding="5" cellspacing="0" class="pstyle1">
                <tr>
                    <td class="signupheading2">
                        <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnRegister" EventName="Click" />
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
    <table>
        <tr>
            <td>
                <asp:ModalPopupExtender ID="mpeRequest" BehaviorID="mpeRequest" runat="server" TargetControlID="lbtnMissing"
                    PopupControlID="pnlLocations" BackgroundCssClass="ModalPopupBG1">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="700px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="HellowWorldPopup1">
                                <table border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="63" colspan="3" bgcolor="#FFFFFF" style="border-left: #9db7df  4px solid;
                                            border-top: #9db7df  4px solid; border-right: #9db7df  4px solid; border-bottom: #9db7df  4px solid">
                                            <table border="0" style="margin: auto; background-color: Gray">
                                                <tr style="background-color: ButtonFace;">
                                                    <td>
                                                        <uc1:ContactUsControl ID="ContactUsControl1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close Window" Width="300px" Height="40px"
                                                            CausesValidation="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
