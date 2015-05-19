<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ORS3W.aspx.cs" Inherits="SRFROWCA.Reports.ORS3W" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">ORS3W</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
            <tr>
                <td class="signupheading2" colspan="3">
                    <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="divMsg">
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel                                       
                                        </button>
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main" style="margin-left: 20px;">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%;">


                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AutoPostBack="false">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <label>Country:</label></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlCountry" AutoPostBack="false" CssClass="width-80" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>
                                                        </tr>

                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Projects:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlProjects" runat="server" CssClass="width-80" AutoPostBack="false"
                                                                    OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Admin1:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlAdmin1" runat="server" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Organization:</label></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlOrganizations" runat="server" CssClass="width-80" AutoPostBack="false"
                                                                    OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged">
                                                                </asp:DropDownList></td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Status:</label>
                                                            </td>
                                                            <td class="width-30">

                                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="width-80">
                                                                    <asp:ListItem Text="--- Select Status ---" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Planned" Value="Planned"></asp:ListItem>
                                                                    <asp:ListItem Text="On Going" Value="On Going"></asp:ListItem>
                                                                    <asp:ListItem Text="Not Reported" Value="Not Reported"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Month:</label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlMonth" runat="server" Style="width: 59%;">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td> </td>
                                                            <td>
                                                               </td>

                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top: 10px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" /><asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" /></td>
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

        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" 
                    Width="100%" PagerSettings-Position="Bottom" AllowCustomPaging="true"
                    CssClass="imagetable" OnPageIndexChanging="gvActivity_PageIndexChanging"
                    PageSize="50" ShowHeaderWhenEmpty="true" EmptyDataText="Your filter criteria does not match any indicator!">
                    <PagerSettings Mode="NumericFirstLast" />
                    <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Organization" HeaderText="Organization" ItemStyle-Width="15%"></asp:BoundField>
                        <asp:BoundField DataField="PartnerOrganization" HeaderText="Partner" ItemStyle-Width="15%"></asp:BoundField>
                        <asp:BoundField DataField="Country" HeaderText="Country" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="PrimaryCluster" HeaderText="PrimaryCluster" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="SubCluster" HeaderText="Sub-Set Cluster" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="Admin1" HeaderText="Admin1" ItemStyle-Width="10%" />
                        <asp:BoundField DataField="Activity" HeaderText="Activity" ItemStyle-Width="55%" />
                        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="10%" />
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>
