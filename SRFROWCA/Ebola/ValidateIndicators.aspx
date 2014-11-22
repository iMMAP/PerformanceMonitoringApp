<%@ Page Title="" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true"
    CodeBehind="ValidateIndicators.aspx.cs" Inherits="SRFROWCA.Ebola.ValidateIndicators" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/ReportedIndicatorComments.ascx" TagName="ReportedIndicatorComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->

</asp:Content>
<asp:Content ID="vIndicatorContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">

            function CheckAll(Checkbox) {
                var GridVwHeaderChckbox = document.getElementById("<%=gvIndicators.ClientID %>");

                for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    GridVwHeaderChckbox.rows[i].cells[3].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                }
            }

            function clearComments() {
                var txtCmtArea = document.getElementById("<%=txtComments.ClientID %>");
                txtCmtArea.value = '';
            }

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

                                                <table class="width-90">
                                                    <tr>
                                                        <td>

                                                            <label class="control-label no-padding-right" style="padding-left:10px;" for="form-input-readonly">
                                                                Intervention
                                                       
                                                            </label>
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text=""></asp:Label>

                                                        </td>
                                                         <td>
                                                            <label class="control-label no-padding-right" for="form-input-readonly">
                                                                Updated By:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>


                                                            <label class="control-label padding-left no-padding-right" style="padding-left:10px;" for="form-input-readonly">
                                                                Organization:
                                                       
                                                            </label>
                                                        </td>

                                                        <td>
                                                            <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                                        </td>
                                                          <td>
                                                            <label class="control-label no-padding-right" for="form-input-readonly">
                                                                Updated On:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUpdatedOn" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    
                                                    
                                                    <tr>
                                                        <td>
                                                            <label class="control-label padding-left no-padding-right" style="padding-left:10px;" for="form-input-readonly">
                                                                Reporting Period:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblReportingPeriod" runat="server" Text=""></asp:Label>
                                                        </td>
                                                        <td>
                                                            <label class="control-label padding-left no-padding-right" style="padding-left:10px;" for="form-input-readonly">
                                                                Report Frequency:
                                                       
                                                            </label>
                                                        </td><td>
                                                            <asp:Label ID="lblReportFrequency" runat="server" Text=""></asp:Label>
                                                             </td>
                                                    </tr>
                                                </table>


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
                                                        DataKeyNames="ReportDataEntryId,ActivityDataId,ReportDataEntryDetailId" CssClass="imagetable" Width="100%" OnRowCommand="gvIndicators_RowCommand" OnRowDataBound="gvIndicators_RowDataBound">
                                                        <HeaderStyle BackColor="Control"></HeaderStyle>
                                                        <RowStyle CssClass="istrow" />
                                                        <AlternatingRowStyle CssClass="altcolor" />
                                                        <Columns>
                                                            <asp:BoundField DataField="ObjectiveId" HeaderText="Obj" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="Pr" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:BoundField DataField="IsSRP" HeaderText="Country" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="70px">
                                                                <HeaderTemplate>

                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApproved" runat="server" Checked='<%# Eval("IsApproved") %>'
                                                                        CssClass="testcb" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="30px" HeaderText="Cmt" meta:resourcekey="TemplateFieldResource5">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgbtnComments" runat="server" ImageUrl="~/assets/orsimages/edit-file-icon.png"
                                                                        CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="AddComments" OnClientClick="clearComments();" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30px"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                            <asp:BoundField DataField="Indicator" HeaderText="Indicator" />
                                                            <asp:BoundField DataField="Location" HeaderText="Location (Admin 2)" />
                                                            <asp:BoundField DataField="Achieved" HeaderText="Variable" />

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
            <div style="width: 600px">

                <div class="modal-content">
                    <div class="modal-header" style="border-bottom-width: 0px;">
                        <button runat="server" id="btnCancelComments" onserverclick="btnCancelComments_Click"
                            class="close" data-dismiss="modal">
                            &times;
                   
                        </button>
                        <%--<h4 class="blue bigger">
                            <asp:Localize ID="localIndComments" runat="server" Text="Indicator Comments"></asp:Localize>
                        </h4>--%>
                    </div>
                    <span class="btn btn-sm btn-info no-radius" style="margin-top: 5px; margin-left: 8px; line-height: 8px;" onclick="javascript:alertComment();">New Comment</span>

                    <div class="modal-body overflow-visible" style="padding-top: 5px;">
                        <div class="row">
                            <uc1:ReportedIndicatorComments ID="ucIndComments" runat="server" />
                        </div>
                    </div>
                    <br />
                    <div class="form-actions" style="margin: 0 auto; width: 97%;">
                        <div class="input-group">
                            <input type="text" runat="server" id="txtComments" name="message" class="form-control" style="text-align: left;" placeholder="Type your comment here ...">
                            <span class="input-group-btn">
                                <asp:Button ID="btnSaveComments" runat="server" Text="Save" OnClick="btnSaveComments_Click"
                                    CssClass="btn btn-sm btn-info no-radius" />
                            </span>
                        </div>
                        <asp:Button ID="btnCancel" Visible="false" runat="server" Text="Cancel" OnClick="btnCancelComments_Click"
                            CssClass="btn btn-primary" />
                    </div>
                    <br />
                </div>

            </div>
        </div>
    </asp:Panel>
</asp:Content>
