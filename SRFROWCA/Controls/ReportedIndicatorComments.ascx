<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportedIndicatorComments.ascx.cs"
    Inherits="SRFROWCA.Controls.ReportedIndicatorComments" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<script type="text/javascript">
    $(document).ready(function () {
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
<div class="widget-box">
    <div class="widget-header widget-header-small header-color-blue2">
        Old Comments
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="slim-scroll" data-height="250">
                <asp:Repeater ID="rptIndComments" runat="server">
                    <ItemTemplate>
                        <div class="widget-box">
                            <div class="widget-header widget-header-small">
                                <span class="grey">
                                    <%#Eval("UserName")%></span> <span class="widget-toolbar no-border">
                                        <%#Eval("CreatedDate")%></span>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <span>
                                        <%#Eval("Comments")%></span>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</div>
<div>
    
    <FCKeditorV2:FCKeditor ID="fcComments" runat="server" ToolbarSet="Basic">
    </FCKeditorV2:FCKeditor>
</div>