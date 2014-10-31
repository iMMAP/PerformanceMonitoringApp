<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfigSettings.aspx.cs" Inherits="SRFROWCA.Admin.ConfigSettings" MasterPageFile="~/Site.Master" %>

<asp:Content ID="cntHead" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {

            $("#<%=txtDate.ClientID%>").datepicker({
                numberOfMonths: 1,
                dateFormat: "dd-mm-yy",
                onSelect: function (selected) {
                    $("#<%=txtDate.ClientID%>").datepicker("option", "minDate", selected)
                }
            });
        });

            function isNumeric(myValue, elem){
                var numexp = /^[0-9]+$/;
                if (myValue.trim() != '') {
                    if (myValue.match(numexp)) {
                        return true;
                    } else {
                        alert("Entry must be a number");
                        elem.value = '';
                        elem.focus();
                        return false;
                    }
                }
            }
    </script>
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
                                                            <td rowspan="6">
                                                                <asp:ListBox runat="server" ID="lstConfigs" Style="height: 190px; width: 300px;"></asp:ListBox>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblCountry" runat="server" Text="Country:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270">
                                                                    <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>

                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCluster" Width="270">
                                                                    <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </td>
                                                            <td></td>

                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Change end date:

                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtDate" Width="270"></asp:TextBox>
                                                            </td>
                                                            <td style="padding-right: 15px;">
                                                                <asp:Button runat="server" ID="btnAdd" Text="Add >>" OnClick="btnAdd_Click" />
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    No. of Indicators Framework:

                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtNoIndicatorsFramework" Width="270" onblur="return isNumeric(this.value, this)"></asp:TextBox>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    No. of Cluster Indicators:

                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtNoClusterIndicators" Width="270" onblur="return isNumeric(this.value, this)"></asp:TextBox>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
                                                                <asp:Button ID="btnSubmit" runat="server" Text="Save Settings" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td style="padding-top: 20px;">
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
