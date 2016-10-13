<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestDataEntry.aspx.cs" Inherits="SRFROWCA.ClustOutputIndicators.TestDataEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">

        <div id="divMsg"></div>
        <table style="width: 100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="box widget-header widget-header-small header-color-blue2">
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 98%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" ID="ddlCountry" Width="270">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" ID="ddlCluster" Width="270">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    <label>
                                                        Month:</label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlMonth" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                        <asp:ListItem Text="2015" Value="11" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                                        <asp:ListItem Text="2017" Value="13"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td></td>
                                                <td class="pull-right">
                                                    <asp:Button runat="server" ID="btnSaveAll" Text="Save" class="btn btn-primary" 
                                                        OnClick="btnSaveAll_Click" OnClientClick="needToConfirm = false;" />
                                                </td>
                                            </tr>
                                        </table>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
        </asp:Panel>
    </div>
</asp:Content>
