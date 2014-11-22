<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="CountryIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.CountryIndicators" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>

<asp:Content ID="cntHeadCountryIndicators" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function validate() {
            var objEng = document.getElementById('<%=txtIndicatorEng.ClientID%>');
            var objFr = document.getElementById('<%=txtIndicatorFr.ClientID%>');
            if (objEng.value == '' && objFr.value == '') {
                alert("Please Enter Indicator!");
                return false;
            }
        }

        $(function () {
            $(".numeric1").numeric();
        });
    </script>
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
</asp:Content>


<asp:Content ID="cntMainContentCountryIndicators" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Output Indicators</li>
        </ul>
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="alert2 alert-block alert-info">
            <h6>
                <asp:Localize ID="locMessageForUser" runat="server" Text="This page allows the cluster coordinator to add output indicators up to a maximum of four (4). This is in addition to the pre-selected list of Sahel Indicators. You are expected to complete the Indicator in either language." meta:resourcekey="locMessageForUserResource1"></asp:Localize></h6>
        </div>

        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2" style="padding-left:0px;">
                                    <h6>
                                        <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF"  class="btn btn-yellow" causesvalidation="false"
                                            title="PDF">
                                            <i class="icon-download"></i>PDF
                                       
                                        </button>

                                           <button runat="server" id="btnExportToExcel" onserverclick="btnExportToExcel_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>

                                        <asp:Button ID="btnAddIndicator" runat="server" OnClick="btnAddIndicator_Click" Text="Add Indicator" CausesValidation="False"
                                            CssClass="btn btn-yellow pull-right" meta:resourcekey="btnAddIndicatorResource1" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 80%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>
                                                    <label>
                                                        Indicator:</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtIndicatorName" runat="server" Width="270px" meta:resourcekey="txtIndicatorNameResource1"></asp:TextBox>
                                                </td>

                                                <td>
                                                    <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged" AppendDataBoundItems="True" ID="ddlCluster" Width="270px" meta:resourcekey="ddlClusterResource1">
                                                        <asp:ListItem Selected="True" Text="--- Select Cluster ---" Value="-1" meta:resourcekey="ListItemResource1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbIncludeRegional" runat="server" Text="Show Regional Indicators" Checked="True" meta:resourcekey="cbIncludeRegionalResource1" />
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="lblCountry" Text="Country:" meta:resourcekey="lblCountryResource1"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" runat="server" AppendDataBoundItems="True" ID="ddlCountry" Width="270px" meta:resourcekey="ddlCountryResource1">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="-1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td>&nbsp;</td>
                                                <td style="padding-top: 10px;">
                                                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                </td>
                                                <td></td>
                                                <td></td>
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
        <div class="table-responsive">
            <div style="overflow-x: auto; width: 100%">
                <cc2:PagingGridView ID="gvClusterIndicators" Width="100%" runat="server" ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="SiteLanguageId"
                     OnRowDataBound="gvClusterIndicators_RowDataBound" OnSorting="gvClusterIndicators_Sorting" ShowHeader="true" OnRowCommand="gvClusterIndicators_RowCommand" CssClass=" table-striped table-bordered table-hover" meta:resourcekey="gvClusterIndicatorsResource1" >
                    <EmptyDataTemplate>
                        Your filter criteria does not match any record in database!
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2">
                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                            <ItemStyle CssClass="hidden"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField Visible="false" DataField="ClusterIndicatorId" HeaderText="ID" SortExpression="ClusterIndicatorId" meta:resourcekey="BoundFieldResource3" />
                        <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource2">
                            <ItemTemplate>
                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                            </ItemTemplate>


                            <ItemStyle HorizontalAlign="Center" Width="4%"></ItemStyle>


                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Country" HeaderText="Country" SortExpression="Country" meta:resourcekey="BoundFieldResource4">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Cluster" HeaderText="Cluster" SortExpression="Cluster" meta:resourcekey="BoundFieldResource5">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="48%" DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" meta:resourcekey="BoundFieldResource6">
                            <ItemStyle Width="48%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Target" HeaderText="Target" SortExpression="Target" meta:resourcekey="BoundFieldResource7">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField ItemStyle-Width="10%" DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource8">
                            <ItemStyle Width="10%"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource3">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnEdit" CausesValidation="False"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' Text="Edit" meta:resourcekey="btnEditResource1"></asp:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle Width="5%"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Center" meta:resourcekey="TemplateFieldResource4">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="False"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ClusterIndicatorId") %>' meta:resourcekey="btnDeleteResource1"></asp:LinkButton>
                            </ItemTemplate>

                            <HeaderStyle Width="5%"></HeaderStyle>

                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("CountryID") %>' meta:resourcekey="lblCountryIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("ClusterID") %>' meta:resourcekey="lblClusterIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource7">
                            <ItemTemplate>
                                <asp:Label ID="lblIndAlternate" runat="server" Text='<%# Eval("IndicatorAlt") %>' meta:resourcekey="lblIndAlternateResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource8">
                            <ItemTemplate>
                                <asp:Label ID="lblUnitID" runat="server" Text='<%# Eval("UnitID") %>' meta:resourcekey="lblUnitIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </cc2:PagingGridView>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:ModalPopupExtender ID="mpeEditIndicator" runat="server" TargetControlID="btnTarget"
                        PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose" DynamicServicePath="" Enabled="True">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlOrg" runat="server" Width="650px" meta:resourcekey="pnlOrgResource1">
                        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="containerPopup">
                                    <div class="popupheading">
                                        Edit Indicator
                                    </div>
                                    <div class="contentarea">
                                        <div class="formdiv">
                                            <table border="0" style="margin: 0 auto;">
                                                <tr>
                                                    <td>Indicator (English):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtIndicatorEng" runat="server" TextMode="MultiLine" Height="70px" Width="450px" MaxLength="4000" meta:resourcekey="txtIndicatorEngResource1"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Indicator (French):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtIndicatorFr" runat="server" TextMode="MultiLine" Height="70px" Width="450px" MaxLength="4000" meta:resourcekey="txtIndicatorFrResource1"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td>Target:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtTarget" CssClass="numeric1" runat="server" Width="450px" MaxLength="9" meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>Unit:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList runat="server" ID="ddlUnits" Width="450px" meta:resourcekey="ddlUnitsResource1"></asp:DropDownList>

                                                    </td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td></td>
                                                    <td align="left" class="frmControl">
                                                        <br />
                                                        <asp:HiddenField ID="hfClusterIndicatorID" runat="server" />
                                                        <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Update" OnClientClick="return validate();" CssClass="btn btn_primary" meta:resourcekey="btnEditResource2" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="False" CssClass="btn btn_primary" meta:resourcekey="btnCloseResource1" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="False"
                                                            ViewStateMode="Disabled" meta:resourcekey="lblMessage2Resource1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="spacer" style="clear: both;">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="graybarcontainer">
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnEdit" />
                                <asp:PostBackTrigger ControlID="btnClose" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div style="display: none">
            <asp:Button ID="btnTarget" runat="server" Width="1px" meta:resourcekey="btnTargetResource1" />
        </div>
    </div>
</asp:Content>
