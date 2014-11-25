<%@ Page Title="" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true"
    CodeBehind="ActivityListing.aspx.cs" Inherits="SRFROWCA.Ebola.ActivityListing" %>

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
            <li class="active">Activities</li>
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

                                        <asp:Button ID="btnAddActivity" runat="server" Text="Add New Activity" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivity_Click" />
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
                                                                    Activity Name:</label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3"></td>
                                                            <td colspan="2">
                                                                
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top:20px;"><asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" /></td>
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
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" PagerSettings-Mode="NumericFirstLast"
                    OnRowCommand="gvActivity_RowCommand" Width="100%" OnRowDataBound="gvActivity_RowDataBound" PagerSettings-Position="Bottom" DataKeyNames="ClusterId,ClusterObjectiveId,ObjectivePriorityId,ActivityTypeId,HumanitarianPriority,SiteLanguageId,ActivityName,EmergencyId"
                     CssClass="table-striped table-bordered table-hover"  OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging" PageSize="30">
                    
                    <Columns>
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="30%" />
                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" SortExpression="ActivityName" ItemStyle-Width="50%"  />
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"   HeaderStyle-Width="80px">
                            <ItemTemplate >
                                <asp:LinkButton ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false" 
                                    CommandName="EditActivity" CommandArgument='<%# Eval("PriorityActivityId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false" 
                                    CommandName="DeleteActivity" CommandArgument='<%# Eval("PriorityActivityId") %>' />
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
                    <asp:Panel ID="pnlOrg" runat="server" Width="750px">
                        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="containerPopup">
                                    <div class="popupheading">
                                        Add/Edit Activity
                                    </div>
                                    <div class="contentarea">
                                        <div class="formdiv">
                                            <table border="0" style="margin: 0 auto;">
                                                <tr>
                                                    <td>Objective:
                                                    </td>
                                                    <td class="frmControl">
                                                        <asp:DropDownList ID="ddlObjectiveNew" runat="server" AppendDataBoundItems="true" Width="450px">
                                                            <asp:ListItem Text="Select" Value="-1" Selected="True"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                                            Text="Required" ControlToValidate="ddlObjectiveNew" CssClass="error2" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="trEnglish">
                                                    <td>Activity Name (English):
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
                                                    <td>Activity Name (French):
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
                                                    <td></td>
                                                    <td align="left" class="frmControl">
                                                        <br />
                                                        <asp:HiddenField ID="hdnPriorityActivityId" runat="server" />
                                                        <asp:Button ID="btnAdd" runat="server" Text="Add/Update" OnClick="btnAdd_Click" CssClass="btn-primary" />
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false" CssClass="btn-primary" />
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
