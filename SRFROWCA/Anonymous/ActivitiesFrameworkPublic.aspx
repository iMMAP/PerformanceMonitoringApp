﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActivitiesFrameworkPublic.aspx.cs" Inherits="SRFROWCA.Anonymous.ActivitiesFrameworkPublic" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .modalDialog {
            width: 500px;
            position: relative;
            margin: 10% auto;
            padding: 5px 20px 13px 20px;
            border-radius: 2px;
            background: #ffffff;
        }

        .mycheckbox {
            margin-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click"
                                            class="btn btn-sm btn-yellow" causesvalidation="false" title="Excel">
                                            <i class="icon-download"></i>Excel
                                        </button>

                                        <button runat="server" id="btnExportToWord" onserverclick="btnExportWord_Click"
                                            class="btn btn-sm btn-yellow" causesvalidation="false" title="Word">
                                            <i class="icon-download"></i>Export To Word (2016)
                                        </button>
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%;">
                                                        <tr>
                                                            <td class="width-20">
                                                                <asp:Label runat="server" ID="lblCountry" Text="Country:" meta:resourcekey="lblCountryResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlSelectedIndexChnaged"
                                                                    meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lblCluster" Text="Cluster:" meta:resourcekey="lblClusterResource1"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlSelectedIndexChnaged" meta:resourcekey="ddlClusterResource1">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <asp:Label ID="lblObj" runat="server" Text="Objective:" meta:resourcekey="lblObjResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                                                    CssClass="width-80" OnSelectedIndexChanged="ddlSelectedIndexChnaged" meta:resourcekey="ddlObjectiveResource1">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <%--<td class="width-20">
                                                                <asp:Label ID="lblActivity" runat="server" Text="Activity:" meta:resourcekey="lblActivityResource1"></asp:Label>
                                                            </td>
                                                            <td class="width-30">

                                                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-80" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlActivitySelectedIndexChnaged" meta:resourcekey="ddlActivityResource1">
                                                                </asp:DropDownList>
                                                            </td>--%>
                                                             <td class="width-20">
                                                                <asp:Label ID="lblIndicator" runat="server" Text="Indicator" meta:resourcekey="lblIndicatorResource1"></asp:Label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80" meta:resourcekey="txtActivityNameResource1"></asp:TextBox>
                                                            </td>
                                                            </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <asp:Label ID="lblYear" runat="server" Text="Year:" meta:resourcekey="lblYearResource1"></asp:Label>
                                                            </td>
                                                        
                                                           
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChnaged" meta:resourcekey="ddlFrameworkYearResource1">
                                                                    <asp:ListItem Text="2017" Value="13"></asp:ListItem>
                                                                    <asp:ListItem Text="2016" Value="12" meta:resourcekey="ListItemResource2"></asp:ListItem>
                                                                    <asp:ListItem Text="2015" Value="11" meta:resourcekey="ListItemResource3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                               <label>CP/SGBV:</label>
                                                                <asp:DropDownList ID="ddlCP" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectedIndexChnaged">
                                                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Child Protection" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="SGBV" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top: 10px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click"
                                                                    CssClass="btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                                <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;"
                                                                    OnClick="btnReset_Click" CssClass="btn btn-sm btn-primary" CausesValidation="False" meta:resourcekey="btnResetResourceIndlst1" />
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
        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" AllowSorting="True" AllowPaging="True" Width="100%"
                    PagerSettings-Mode="NumericFirstLast" OnRowDataBound="gvActivity_RowDataBound" AllowCustomPaging="true"
                    PagerSettings-Position="Bottom" DataKeyNames="ActivityId,IndicatorDetailId,IndicatorId,IsMigrated"
                    CssClass="imagetable table-hover" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging"
                    PageSize="40" ShowHeaderWhenEmpty="True" EmptyDataText="Your filter criteria does not match any indicator!" meta:resourcekey="gvActivityResource1">

                    <PagerSettings Mode="NumericFirstLast"></PagerSettings>

                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ObjectiveId" HeaderText="" ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" ItemStyle-Width="80px" meta:resourcekey="BoundFieldResource1" />
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource2" />
                        <asp:TemplateField HeaderStyle-Width="60" ItemStyle-Width="60" >
                            <ItemTemplate>
                                <asp:Image ID="imgObjective" runat="server" />
                                <asp:Image ID="imgCP" ImageUrl="~/assets/orsimages/cp1.png" ToolTip="Child Protection Indicator" runat="server" Visible='<%# Eval("IsChildProtection") %>' />
                                <asp:Image ID="imgSGBV" ImageUrl="~/assets/orsimages/cp1.png" ToolTip="Sexual & Gender Based Voilence" runat="server" Visible='<%# Eval("IsSGBV") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" meta:resourcekey="BoundFieldResource4" />
                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" meta:resourcekey="BoundFieldResource5" />
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" meta:resourcekey="BoundFieldResource6" />
                        <asp:TemplateField ItemStyle-Width="4%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>" meta:resourcekey="TemplateFieldResource4">
                            <ItemTemplate>
                                <asp:Label ID="lblCalcMethod" ToolTip="" runat="server" Text='<%# Eval("CalculationType") %>' meta:resourcekey="lblCalcMethodResource1"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="4%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Country Target" SortExpression="IndicatorTarget" ItemStyle-HorizontalAlign="Right" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="lblIndTarget" runat="server" Text='<%# Eval("IndicatorTarget") %>' meta:resourcekey="lblIndTargetResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("EmergencyLocationId") %>' meta:resourcekey="lblCountryIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("EmergencyClusterId") %>' meta:resourcekey="lblClusterIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
    </div>
    <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
    <asp:Panel ID="Panel2" runat="server" CssClass="modalDialog" Style="display: none" Width="600px" meta:resourcekey="Panel1Resource1">
        <div>
            <br />
            Export With Admin2 (Targets):
                <asp:RadioButton ID="rbExlAdmin2Yes" runat="server" GroupName="ExcelAdmin2Target" Checked="false" Text="Yes" />
            <asp:RadioButton ID="rbExlAdmin2No" runat="server" GroupName="ExcelAdmin2Target" Checked="true" Text="NO" />
            <br />

            Include Identity (Ids) Columns:
                <asp:RadioButton ID="rbExlIdnYes" runat="server" GroupName="ExcelIdnTarget" Text="Yes" />

            <asp:RadioButton ID="rbExlIdnNO" runat="server" GroupName="ExcelIdnTarget" Checked="true" />
            <label>No</label>

            <div align="center">
                <asp:Button ID="btnExportExcelOK" runat="server" Text="OK" OnClick="btnExportExcelOK_Click" class="btn btn-sm btn-primary" meta:resourcekey="OkButtonResource1" />
                <button id="btnExportExcelCancel" class="btn btn-sm btn-primary">Close</button>

            </div>
        </div>
    </asp:Panel>

    <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server"
        TargetControlID="btnClientOpen" CancelControlID="btnExportExcelCancel"
        PopupControlID="Panel2"
        BackgroundCssClass="modalpopupbackground"
        DropShadow="True" DynamicServicePath="" Enabled="True" />

</asp:Content>
