<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ValidateIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.ValidateIndicators" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/ReportedIndicatorComments.ascx" TagName="ReportedIndicatorComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            function alertComment() {
                var txtCmtArea = document.getElementById("<%=txtComments.ClientID %>");
                txtCmtArea.focus();
                txtCmtArea.value = '';

                var btnSaveCmt = document.getElementById("<%=btnSaveComments.ClientID %>");
                  btnSaveCmt.value = "Save";

                  var hiddenField = document.getElementById("MainContent_ucIndComments_hdnUpdate");
                  hiddenField.value = "-1";
              }
              try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li><a href="ValidateReportList.aspx">Validate</a></li>
            <li class="active">Validate Indicators</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>Report Details
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="col-sm-12">

                                                        <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly">
                                                            Project:
                                                       
                                                        </label>
                                                        <div class="col-sm-11">
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text=""></asp:Label>
                                                            <%--<input readonly="" type="text" class="col-xs-10 col-sm-11" id="form-input-readonly"
                                                                value="This text field is readonly!" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12">
                                                    <div class="col-sm-6">
                                                        <label class="col-sm-2 control-label no-padding-right" for="form-input-readonly">
                                                            Organization:
                                                       
                                                        </label>
                                                        <div class="col-sm-10">
                                                            <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12">
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Updated By:
                                                       
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Updated On:
                                                       
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblUpdatedOn" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Reporting Period:
                                                       
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblReportingPeriod" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>Validate Reported Indicators
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-toolbox">
                                        <div class="btn-toolbar">
                                            <div class="btn-group">
                                                <button runat="server" id="btnApprove" onserverclick="btnApprove_Click" class="btn btn-sm btn-success">
                                                    <i class="icon-ok bigger-110"></i>Approve
                                               
                                                </button>
                                                <%--<button runat="server" id="btnReject" onserverclick="btnReject_Click" class="btn btn-sm btn-danger">
                                                    <i class="icon-remove bigger-110"></i>Reject
                                                </button>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                                    <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                                        DataKeyNames="ReportId,ActivityDataId,ReportDetailId" CssClass="imagetable" Width="100%" OnRowCommand="gvIndicators_RowCommand" OnRowDataBound="gvIndicators_RowDataBound">
                                                        <HeaderStyle BackColor="Control"></HeaderStyle>
                                                        <RowStyle CssClass="istrow" />
                                                        <AlternatingRowStyle CssClass="altcolor" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ObjectiveId" HeaderText="Obj" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="Pr" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:BoundField DataField="IsSRP" HeaderText="Country" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApproved" runat="server" Checked='<%# Eval("IsApproved") %>'
                                                                        CssClass="testcb" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Cmt" meta:resourcekey="TemplateFieldResource5">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnComments" runat="server" ImageUrl="~/assets/orsimages/edit-file-icon.png"
                                                                        CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="AddComments" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30px"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" />
                                                                    <asp:Image ID="imgPriority" runat="server" AlternateText="PR" />
                                                                    <asp:Image ID="imgCind" runat="server" AlternateText="C" />
                                                                </ItemTemplate>
                                                                <ItemStyle Wrap="False"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                            <asp:BoundField DataField="Indicator" HeaderText="Indicator" />
                                                            <asp:BoundField DataField="Location" HeaderText="Location" />
                                                            <asp:BoundField DataField="AnnualTarget" HeaderText="Annual Target" />
                                                            <asp:BoundField DataField="Achieved" HeaderText="Monthly Achieved" />

                                                        </Columns>
                                                    </asp:GridView>
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
    <asp:ModalPopupExtender ID="mpeComments" runat="server" TargetControlID="Button1"
        PopupControlID="Panel2" Drag="True" BackgroundCssClass="modalpopupbackground"
        Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="Button1" Style="display: none" />
    <asp:Panel ID="Panel2" runat="server">
        <div class="row">
            <div style="width: 800px">

                <div class="modal-content">
                    <div class="modal-header">
                        <button runat="server" id="btnCancelComments" onserverclick="btnCancelComments_Click"
                            class="close" data-dismiss="modal">
                            &times;
                   
                        </button>
                        <h4 class="blue bigger">
                            <asp:Localize ID="localIndComments" runat="server" Text="Indicator Comments"></asp:Localize>
                        </h4>
                    </div>
                    <span class="btn btn-sm btn-info no-radius" style="margin-top: 5px; margin-left: 8px; line-height: 8px;" onclick="javascript:alertComment();">New Comment</span>

                    <div class="modal-body overflow-visible">
                        <div class="row">
                            <uc1:ReportedIndicatorComments ID="ucIndComments" runat="server" />
                        </div>
                    </div>
                    <%-- <div class="modal-footer">--%>
                    <div class="form-actions">
                        <div class="input-group">
                            <input type="text" runat="server" id="txtComments" name="message" class="form-control" style="text-align: left;" placeholder="Type your comment here ...">
                            <span class="input-group-btn">
                                <asp:Button ID="btnSaveComments" runat="server" Text="Save" OnClick="btnSaveComments_Click"
                                    CssClass="btn btn-primary" />
                            </span>
                        </div>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancelComments_Click"
                            CssClass="btn btn-primary" />
                    </div>
                    <%--</div>--%>
                </div>

            </div>
        </div>
    </asp:Panel>
</asp:Content>
