<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewIndicatorsControl.ascx.cs"
    Inherits="SRFROWCA.Controls.NewIndicatorsControl" %>
<div class="row">
    <h6 class="header blue bolder smaller">
        Indicator<asp:Label ID="lbl1stNumber" runat="server" Text=""></asp:Label></h6>
    <div class="col-xs-6 col-sm-6">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div>
                        <label>
                            (English):</label>
                        <div>
                            <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-90"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="UserName Required"
                                CssClass="error2" Text="Required" ControlToValidate="txtInd1Eng"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xs-6 col-sm-6">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div>
                        <label>
                            (French):</label>
                        <div>
                            <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-90"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="UserName Required"
                                CssClass="error2" Text="Required" ControlToValidate="txtInd1Fr"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
