<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmBox.ascx.cs" Inherits="SRFROWCA.Controls.ConfirmBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
 
<script runat="server">
    public string Title { set { this.lblTitle.Text = value; } }
    public string Message { set { this.lblMessage.Text = value; } }
    public string TargetControlId { set { this.popupConfirmBox.TargetControlID = this.btnConfirm.TargetControlID = value; } }
    public int Width { set { this.panelConfirmBox.Width = Unit.Pixel(value); } }
 
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnPopupClose.OnClientClick = "$find('" + popupConfirmBox.ClientID + "').hide();";
    }
</script>
   
<asp:Panel ID="panelConfirmBox" runat="server" CssClass="modalPopup" Width="400px" Style="display:none;">
    <h5><asp:Label ID="lblTitle" runat="server" Text="[Title]"></asp:Label></h5>
    <div class="closeButton">
        <asp:LinkButton ID="btnPopupClose" runat="server" >x</asp:LinkButton>
    </div>    
    <div class="clearBoth"></div>
     
    <br />  
    <div class="modalMessage">
        <asp:Label ID="lblMessage" runat="server" Text="[Message]"></asp:Label>
    </div>
    <br />
     
    <div class="modalButtons">
        <asp:Button ID="btnYes" runat="server" Text="Yes" /> &nbsp; <asp:Button ID="btnNo" runat="server" Text="No" />
    </div>
</asp:Panel>                    
<asp:ModalPopupExtender Id="popupConfirmBox" runat="server" PopupControlID="panelConfirmBox" BackgroundCssClass="modalBackground" CancelControlID="btnNo" OkControlId="btnYes" ></asp:ModalPopupExtender>
<asp:ConfirmButtonExtender ID="btnConfirm" runat="server" DisplayModalPopupID="popupConfirmBox"> </asp:ConfirmButtonExtender>

