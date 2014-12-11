﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListing.aspx.cs" Inherits="SRFROWCA.Ebola.IndicatorListing" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .radioButtonList{
    display:inline;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Indicators</li>
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

                                        <asp:Button ID="btnAddActivity" runat="server" Text="Add New Indicator" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddIndicator_Click" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%; margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="true" CssClass="width-80" AutoPostBack="true" OnSelectedIndexChanged="ddlObjective_SelectedIndexChanged">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Activity:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlActivity" runat="server" AppendDataBoundItems="true" CssClass="width-80" AutoPostBack="true" OnSelectedIndexChanged="ddlActivity_SelectedindexChanged">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>


                                                            <td class="width-20">
                                                                <label>
                                                                    Indicator Name:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="3" style="padding-top: 20px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" />

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
                <asp:GridView ID="gvIndicator" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" PagerSettings-Mode="NumericFirstLast"
                    OnRowCommand="gvIndicator_RowCommand" Width="100%" OnRowDataBound="gvIndicator_RowDataBound" PagerSettings-Position="Bottom"
                    CssClass=" table-striped table-bordered table-hover" OnSorting="gvIndicator_Sorting" OnPageIndexChanging="gvIndicator_PageIndexChanging" PageSize="30"
                    DataKeyNames="ReportFrequencyTypeId,ClusterId,ClusterObjectiveId,ObjectivePriorityId,PriorityActivityId,HumanitarianPriority,SiteLanguageId,DataName,IsPriorityIndicatory,IsSRPIndicator,UnitId,EmergencyId">

                    <Columns>
                        <asp:BoundField DataField="Objective" HeaderText="Objective" SortExpression="Objective" ItemStyle-Width="20%" />
                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" SortExpression="ActivityName" ItemStyle-Width="30%" />
                        <asp:BoundField DataField="DataName" HeaderText="Indicator" SortExpression="DataName" ItemStyle-Width="40%" />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                                    CommandName="EditIndicator" CommandArgument='<%# Eval("ActivityDataId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteIndicator" CommandArgument='<%# Eval("ActivityDataId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:ModalPopupExtender ID="mpeAddOrg" BehaviorID="mpeAddOrg" runat="server" TargetControlID="btntest"
                        PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" CancelControlID="btnClose">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pnlOrg" runat="server" Width="850px">
                        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="containerPopup">
                                    <div class="popupheading">
                                        Add/Edit Indicator
                                    </div>
                                    <div class="contentarea">
                                        <div class="formdiv">
                                            <table border="0" style="margin: 0 auto;">
                                                <tr>
                                                    <td>Objective:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList ID="ddlObjectiveNew" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="450px"
                                                            OnSelectedIndexChanged="ddlObjectiveNew_SelectedIndexChanged">
                                                            <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="ddlObjectiveNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Activity:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList ID="ddlActivityNew" runat="server" AppendDataBoundItems="true" Width="450px">
                                                            <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="ddlActivityNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Unit:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList ID="ddlUnit" runat="server" AppendDataBoundItems="true" Width="450px">
                                                            <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="ddlUnit" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>

                                                <tr runat="server" id="trEnglish">
                                                    <td>Indicator (EN):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtActivityEng" runat="server" Width="450px" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvEmgName" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="txtActivityEng" CssClass="error2"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trFrench">
                                                    <td>Indicator (FR):
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:TextBox ID="txtActivityFr" runat="server" Width="450px" MaxLength="1000" TextMode="MultiLine"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="rfvEmgNameFr" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="txtActivityFr" CssClass="error2"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Report Frequency:</td>
                                                    <td>
                                                        <asp:RadioButtonList runat="server" ID="rblFrequency" style="display:inline" >
                                                            <asp:ListItem Selected="True" Text="Daily" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Weekly" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Monthly" Value="1"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td align="left" class="frmControl">
                                                        <br />
                                                        <asp:HiddenField ID="hdnIndicatorId" runat="server" />
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add/Update" OnClick="btnAdd_Click" CssClass="btn btn-primary" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="btn btn-primary" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                                            ViewStateMode="Disabled"></asp:Label>
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
                                <asp:PostBackTrigger ControlID="btnAdd" />
                                <asp:PostBackTrigger ControlID="btnClose" />
                                <asp:AsyncPostBackTrigger ControlID="ddlObjectiveNew" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <div style="display: none">
            <asp:Button ID="btntest" runat="server" Width="1px" />
        </div>
    </div>
</asp:Content>