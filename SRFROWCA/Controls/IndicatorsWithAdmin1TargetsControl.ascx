<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicatorsWithAdmin1TargetsControl.ascx.cs"
    Inherits="SRFROWCA.Controls.IndicatorsWithAdmin1TargetsControl" %>
<style>
    .accordian
    {
        height:20px;width:20px;border:1px solid gray;font-size:24px;color:gray;line-height: 16px;
text-align: center;float:left;
    }

</style>
<div class="row">
    <h6 class="header blue bolder smaller">
        Indicator<asp:Label ID="lbl1stNumber" runat="server" Text=""></asp:Label></h6>
   <div class="col-xs-12 col-sm-12">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div>
                        <label>
                            Unit:</label>
                        <div>
        <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-40"></asp:DropDownList>
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
                            (English):</label>
                        <div>
                            <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-90"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required"
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required"
                                CssClass="error2" Text="Required" ControlToValidate="txtInd1Fr"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

     <div class="col-xs-12 col-sm-12">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div>
                        <div style="float:left;cursor:pointer;" onclick="$(this).parent().find('.content').toggle();$(this).find('.accordian').text() == '+' ? $(this).find('.accordian').text('-'): $(this).find('.accordian').text('+');" >
                        <div class="accordian">+</div>
                            <div style="margin-left:8px;margin-top:0px;float:left;">
                        
                            Admin1 targets
                                </div>
                            </div>
                        <div class="content" style="float:left;clear:both;margin-top:10px;margin-left:0px;">
       <asp:Repeater runat="server" ID="rptAdmin1">
           <ItemTemplate>
               <div style="float:left;width:120px;margin-bottom:20px;margin-right:20px;"><div style="float:left;width:80px;text-align:right;margin-top:5px;" ><%#Eval("LocationName")%>&nbsp;</div><asp:TextBox runat="server" ID="txtTarget" style="width:30px;"></asp:TextBox></div>
               <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%#Eval("LocationId")%>' />
           </ItemTemplate>
       </asp:Repeater>
    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<script>
    $(document).ready(function () {
        $(".content").hide();
    });

</script>
