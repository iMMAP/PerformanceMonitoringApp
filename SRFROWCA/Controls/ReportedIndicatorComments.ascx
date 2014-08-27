<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportedIndicatorComments.ascx.cs"
    Inherits="SRFROWCA.Controls.ReportedIndicatorComments" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<script type="text/javascript">

    function check(chkVal, indicatorCmtID) {
        //uncheck all
        $("input:checkbox").prop('checked', false);

        //check selected
        chkVal.checked = true;

        <%--var comments = document.getElementById('cmt-' + indicatorCmtID);

        var oEditor = FCKeditorAPI.GetInstance('<%=fcComments.ClientID %>');
        var newContent = comments.innerHTML;

        oEditor.SetHTML(newContent);--%>

        var btn = document.getElementById('MainContent_btnSaveComments');
        btn.value = 'Update';

        var hiddenField = document.getElementById("<%=hdnUpdate.ClientID %>");
        hiddenField.value = indicatorCmtID;

    }

    function edit(indicatorCmtID) {
        var comments = document.getElementById('cmt-' + indicatorCmtID);

        var txtInput = document.getElementById('MainContent_txtComments');
        txtInput.value = comments.innerHTML.trim();

        var btn = document.getElementById('MainContent_btnSaveComments');
        btn.value = 'Update';

        var hiddenField = document.getElementById("<%=hdnUpdate.ClientID %>");
        hiddenField.value = indicatorCmtID;

        <%--var hiddenField = document.getElementById("<%=hdnComments.ClientID %>");
        hiddenField.value = txtInput.value ;--%>
    }

    $(document).ready(

        function () {
            // scrollables
            $('.slim-scroll').each(function () {
                var $this = $(this);
                $this.slimScroll({
                    height: $this.data('height') || 100,
                    railVisible: true
                });
            });
        });
</script>
<div class="widget-box" style="height: 400px;">
    <div class="widget-header widget-header-small header-color-blue2">
        <%--Old Comments--%>
    </div>
      <div class="widget-body">
        <div class="widget-main">
            <div class="slim-scroll" data-height="380">
   <div class="tab-content padding-8 overflow-visible">
        <div class="tab-pane active" id="comment-tab">

           <%-- <div class="comments" style="overflow: hidden; width: auto;">--%>

                <asp:Repeater ID="rptIndComments" runat="server">
                    <ItemTemplate>

                        <div class="itemdiv commentdiv">
                            <div class="body">
                                <div class="name">
                                    <a href="#"><%#Eval("UserName")%></a>
                                </div>

                                <div class="time">
                                    <i class="icon-time"></i>
                                    <span class="green"><%#Eval("CreatedDate")%></span>
                                </div>

                                <div class="text">
                                    <i class="icon-quote-left"></i>
                                    <span id="cmt-<%#Eval("IndicatorCommentsDetailId")%>"><%#Eval("Comments")%></span>
                                </div>
                            </div>

                            <div class="tools">
                                <div class="inline position-relative">
                                    <button data-toggle="dropdown" class="btn btn-minier bigger btn-yellow dropdown-toggle">
                                        <i class="icon-angle-down icon-only bigger-120"></i>
                                    </button>

                                    <ul class="dropdown-menu dropdown-only-icon dropdown-yellow pull-right dropdown-caret dropdown-close">

                                        <li>
                                            <a title="" data-rel="tooltip" class="tooltip-warning" href="#" onclick="javascript: edit('<%#Eval("IndicatorCommentsDetailId")%>')" data-original-title="Reject">
                                                <span class="orange">
                                                    <i class="icon-edit bigger-110"></i>
                                                </span>
                                            </a>
                                        </li>

                                        <li>
                                            <a title="" data-rel="tooltip" class="tooltip-error" onclick="javascript:confirm('Are you sure you want to delete this comment?')" href="#" data-original-title="Delete">
                                                <span class="red">
                                                    <i class="icon-trash bigger-110"></i>
                                                </span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>

                    </ItemTemplate>
                </asp:Repeater>

            <%--</div>--%>
            <div class="slimScrollBar ui-draggable" style="background: none repeat scroll 0% 0% rgb(0, 0, 0); width: 7px; display:none; position: absolute; top: 0px; opacity: 0.4; border-radius: 7px; z-index: 99; right: 1px; height: 283.019px;"></div>
            <div class="slimScrollRail" style="width: 7px; height: 107%; display:none; position: absolute; top: 0px;  border-radius: 7px; background: none repeat scroll 0% 0% rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div>



            <asp:HiddenField ID="hdnUpdate" runat="server" Value="-1" />
        </div>
    </div>
        </div>

       </div>
    </div>
</div>
