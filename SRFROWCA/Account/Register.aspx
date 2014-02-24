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
        .ddlWidthPx
        {
            width: 400px;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            $(function () {
                if ($('#<%=rbtnClusterLead.ClientID%>').is(':checked')) {
                    $('#trClusters').show();
                    $('#trOrganization').hide();
                }
                else {
                    $('#trClusters').hide();
                    $('#trOrganization').show();
                }

                $('#<%=rbtnUser.ClientID%>').change(function () {
                    if ($('#<%=rbtnUser.ClientID%>').is(':checked')) {
                        $('#trClusters').hide();
                        $('#trOrganization').show();
                    }
                });

                $('#<%=rbtnClusterLead.ClientID%>').change(function () {
                    if ($('#<%=rbtnClusterLead.ClientID%>').is(':checked')) {
                        $('#trClusters').show();
                        $('#trOrganization').hide();
                    }
                });

                $('#<%=rbtnCountryAdmin.ClientID%>').change(function () {
                    if ($('#<%=rbtnCountryAdmin.ClientID%>').is(':checked')) {
                        $('#trClusters').hide();
                        $('#trOrganization').hide();
                    }
                });
            });
        }

        function ValidateOrganization(source, args) {

            if (document.getElementById('<%=rbtnUser.ClientID%>').checked) {
                var ddl = document.getElementById('<%= ddlOrganization.ClientID %>');
                if (ddl.options[ddl.selectedIndex].value === '0') {

                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }

        function ValidateClustersList(source, args) {

            if (document.getElementById('<%=rbtnClusterLead.ClientID%>').checked) {
                var ddl = document.getElementById('<%= ddlClusters.ClientID %>');
                if (ddl.options[ddl.selectedIndex].value === '0') {

                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;
                }
            }
            else {
                args.IsValid = true;
            }
        }
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
                        <table style="margin: 0 auto;" border="0">
                            <tr>
                                <td>
                                    <label>
                                        Your Role:</label>
                                </td>
                                <td colspan="2">
                                    <div id="divUserRoles" runat="server">
                                        <asp:RadioButton ID="rbtnUser" runat="server" Text="User" GroupName="Roles" Checked="true" />
                                        <asp:RadioButton ID="rbtnClusterLead" runat="server" Text="Cluster Lead" GroupName="Roles" />
                                        <asp:RadioButton ID="rbtnCountryAdmin" runat="server" Text="Country Admin" GroupName="Roles" />
                                    </div>
                                    <div id="chkCountriesMessage">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label>
                                        Name:</label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="256" CssClass="ddlWidthPx"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="UserName Required"
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
                                        CssClass="ddlWidthPx"></asp:TextBox>
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
                                        CssClass="ddlWidthPx"></asp:TextBox>
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
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="256" CssClass="ddlWidthPx"></asp:TextBox>
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
                                    <asp:TextBox ID="txtPhone" MaxLength="50" runat="server" CssClass="ddlWidthPx"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trOrganization">
                                <td>
                                    <label>
                                        Organization:</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="ddlWidthPx">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator2" ClientValidationFunction="ValidateOrganization"
                                        ErrorMessage="Required."></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td width="109px">
                                    <label>
                                        Country:</label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="ddlWidthPx">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Location Required"
                                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trClusters">
                                <td>
                                    <label>
                                        Cluster:
                                    </label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlClusters" runat="server" CssClass="ddlWidthPx">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CustomValidator runat="server" ForeColor="Red" ID="CustomValidator1" ClientValidationFunction="ValidateClustersList"
                                        ErrorMessage="Required."></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="button_example"
                                        OnClick="btnRegister_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left">
                                    <asp:LinkButton ID="lbtnMissing" Style="color: Blue" runat="server" Text="Request Missing Organization Or Location"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <div class="spacer" style="clear: both;">
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
