<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigSettings.aspx.cs" Inherits="SRFROWCA.Admin.ConfigSettings" MasterPageFile="~/Site.Master" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="cntMainConfigSettings" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-content">

        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="row">
                                    <table style="width: 85%; margin: 10px 10px 10px 20px" border="0">
                                        <tr>
                                            <td style="width: 30%">
                                                <label>
                                                    Send Email:</label>
                                            </td>
                                            <td style="width: 30%">
                                                <asp:RadioButtonList runat="server" ID="rbListEmailSetting" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                    <asp:ListItem Text="No" Selected="true" Value="false"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="width: 5%"></td>
                                            <td style="width: 20%"></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label>
                                                    Email Subject - <span style="font-size: 12px;">(Add in the beginning)</span>:</label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStagingSubject" runat="server" Width="270" MaxLength="50"></asp:TextBox>
                                            </td>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td>&nbsp;</td>
                                            <td style="padding-top: 20px;">
                                                <asp:Button ID="btnSaveEmailSettings" runat="server" CausesValidation="false" Text="Save Settings" CssClass="btn btn-primary" OnClick="btnSaveEmailSettings_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td style="padding-top: 20px;">
                                                <asp:Label runat="server" ID="lblEmailMessage" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
