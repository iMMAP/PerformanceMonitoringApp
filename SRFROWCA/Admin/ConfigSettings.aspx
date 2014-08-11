<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigSettings.aspx.cs" Inherits="SRFROWCA.Admin.ConfigSettings" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbConfigSettings" runat="server" Text="Configuration Settings" meta:resourcekey="localBreadCrumbConfigSettingsResource1"></asp:Localize></li>

        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6></h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 50%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td style="width: 50%;">
                                                                <label>
                                                                    Send Email Settings:</label>
                                                            </td>
                                                            <td>
                                                                <asp:RadioButtonList runat="server" ID="rbListEmailSetting" RepeatDirection="Horizontal">
                                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Selected="true" Value="false"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Staging Email Subject Text:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtStagingSubject" runat="server" Width="300" MaxLength="50"></asp:TextBox>
                                                            </td>


                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Save Settings" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
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

                </td>
            </tr>
        </table>
    </div>



</asp:Content>
