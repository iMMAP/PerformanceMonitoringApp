<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportedIndicatorComments.ascx.cs"
    Inherits="SRFROWCA.Controls.ReportedIndicatorComments" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<script type="text/javascript">

    function check(chkVal, indicatorCmtID) {
        //uncheck all
        $("input:checkbox").prop('checked', false);

        //check selected
        chkVal.checked = true;

        var comments = document.getElementById('cmt-'+indicatorCmtID);
        
        var oEditor = FCKeditorAPI.GetInstance('<%=fcComments.ClientID %>');
        var newContent = comments.innerHTML; 

        oEditor.SetHTML(newContent);

        var btn = document.getElementById('MainContent_btnSaveComments');
        btn.value= 'Update';

        var hiddenField = document.getElementById("<%=hdnUpdate.ClientID %>");
        hiddenField.value = indicatorCmtID;

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
<div class="widget-box" style="height: 200px;">
    <div class="widget-header widget-header-small header-color-blue2">
        Old Comments
   
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="slim-scroll" data-height="180">
                <div class="table-responsive">
                    <table class="table table-striped table-bordered table-hover" id="sample-table-1">
                        <thead>
                            <tr>
                                <th style="padding: 2px;" class="center">View/Edit</th>
                                <th style="padding: 2px;">User</th>
                                <th style="padding: 2px;">Comment Date</th>
                                
                            </tr>
                        </thead>

                        <tbody>
                            <asp:Repeater ID="rptIndComments" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td class="center" style="padding: 2px;">
                                            <label>
                                                <input id="chkComment" type="checkbox" onclick="javascript: check(this,'<%#Eval("IndicatorCommentsDetailId")%>')" class="ace">
                                                <span class="lbl"></span>
                                       <div id="cmt-<%#Eval("IndicatorCommentsDetailId")%>" style="visibility:hidden;display:none;"><%#Eval("Comments")%></div>         
                                            </label>
                                        </td>

                                        <td style="padding: 2px;"><%#Eval("UserName")%>
                                        </td>
                                        <td style="padding: 2px;"><%#Eval("CreatedDate")%></td>
                                       
                                    </tr>

                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                     <asp:HiddenField ID="hdnUpdate" runat="server" Value="-1"  />
                </div>
            </div>
        </div>
    </div>
</div>
<br />
<div style="height: 200px;">

    <FCKeditorV2:FCKeditor ID="fcComments" runat="server" ToolbarSet="Basic">
    </FCKeditorV2:FCKeditor>

</div>
