<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicatorsWithAdmin1TargetsControl.ascx.cs"
    Inherits="SRFROWCA.Controls.IndicatorsWithAdmin1TargetsControl" %>
<style>
    .accordian
    {
        height: 20px;
        width: 20px;
        border: 1px solid gray;
        font-size: 24px;
        color: gray;
        line-height: 16px;
        text-align: center;
        float: left;
    }

    input[type="checkbox"]
    {
        margin-right: 10px;
        margin-top: 2px;
    }

        input[type="checkbox"] + label
        {
            margin-top: -3px;
        }
</style>
<script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>

<script type="text/javascript">

    $(function () {
        $(".numeric1").numeric();
    });
</script>
<div id="divMsg">
</div>
<div style="display: block; width: 100%; margin-bottom: 15px;">
    <div style="width: 80%; margin-top: 120px; margin-bottom: 0px; display: block;">
        <asp:Localize ID="localIndicatorInfo" runat="server" meta:resourcekey="localIndicatorInfoResource1"></asp:Localize>
    </div>
    <h6 class="header blue bolder smaller">Indicator<asp:Label ID="lbl1stNumber" runat="server" meta:resourcekey="lbl1stNumberResource1"></asp:Label></h6>

    <div class="col-xs-12 col-sm-12 dvIndicator" style="padding-left: 0px;">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <div style="float: left; width: 40%;">
                        <label>
                            <asp:HiddenField ID="hfIndicatorId" runat="server" />
                            (English):</label>
                        <div>
                            <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1EngResource1"></asp:TextBox>

                        </div>
                    </div>
                    <div style="float: left; width: 40%;">
                        <label>
                            (French):</label>
                        <div>
                            <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-95" TextMode="MultiLine" meta:resourcekey="txtInd1FrResource1"></asp:TextBox>

                        </div>
                    </div>
                    <div style="float: left; width: 15%;">
                        <label>
                            Unit:</label>
                        <div>
                            <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-95" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                CssClass="error2" Text="Required" ControlToValidate="ddlUnit" InitialValue="0" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
        <div class="widget-box no-border">
            <div class="widget-body">
                <div class="widget-main no-padding-bottom no-padding-top">
                    <asp:CheckBox runat="server" ID="chkGender" Text="Gender Disagregated" meta:resourcekey="chkGenderResource1" />
                </div>
            </div>
        </div>        
    </div>
</div>

<div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
    <div class="widget-box no-border">
        <div class="widget-body">
            <div class="widget-main no-padding-bottom no-padding-top">
                <div>
                    <div style="float: left; cursor: pointer;" onclick="$(this).parent().find('.content').toggle();$(this).find('.accordian').text() == '+' ? $(this).find('.accordian').text('-'): $(this).find('.accordian').text('+');">
                        <div class="accordian">+</div>
                        <div style="margin-left: 8px; margin-top: 0px; float: left;">
                            Admin1 targets
                               
                        </div>
                    </div>
                    <div class="content" style="float: left; clear: both; margin-top: 10px; margin-left: 0px;">
                        <asp:Repeater runat="server" ID="rptAdmin1">
                            <ItemTemplate>
                                <div style="float: left; width: 140px; margin-bottom: 20px; margin-right: 20px;">
                                    <div style="float: left; width: 80px; text-align: right; margin-top: 5px;"><%#Eval("LocationName")%>&nbsp;</div>
                                    <asp:TextBox ID="txtTarget" runat="server" MaxLength="8" Text='<%#Eval("ClusterTarget") %>' Style="width: 50px;" CssClass="numeric1" meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                </div>
                                <asp:HiddenField runat="server" ID="hdnLocationId" Value='<%# Eval("LocationId") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
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
    function isNumber(evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
</script>
