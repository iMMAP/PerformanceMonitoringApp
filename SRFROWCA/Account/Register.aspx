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
                                <td>
                                    <label>
                                        Organization:</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="ddlWidth">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvOrganization" runat="server" ErrorMessage="Organization Required"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlOrganization"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <div id="divUserRoles" runat="server">
                                        <asp:RadioButton ID="rbtnUser" runat="server" Text="User" GroupName="Roles" Checked="true" />
                                        <asp:RadioButton ID="rbtnCountryAdmin" runat="server" Text="Country Admin" GroupName="Roles" />
                                    </div>
                                    <div id="chkCountriesMessage">
                                    </div>
                                </td>
                            </tr>
                            <tr id="trCountry">
                                <td width="109px">
                                    <label>
                                        Country:</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="ddlWidth">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Country Required"
                                        CssClass="error2" Text="Required" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trLocations">
                                <td width="109px">
                                    <label>
                                        <asp:Literal ID="ltrlLocation" runat="server" Text="Country:"></asp:Literal></label>
                                </td>
                                <td>
                                    <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth">
                                        <Texts SelectBoxCaption="Select Your Country" />
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
                            <asp:LinkButton ID="lbtnMissing" runat="server" Text="Request Missing Organization Or Location"></asp:LinkButton>
                        </div>
                        <div class="spacer" style="clear: both;">
                        </div>
                    </div>
                </div>
                <div class="graybarcontainer">
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
                                                <td height="63" colspan="3" bgcolor="#FFFFFF" style="border-left: #9db7df 4px solid;
                                                    border-top: #9db7df 4px solid; border-right: #9db7df 4px solid; border-bottom: #9db7df 4px solid">
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
