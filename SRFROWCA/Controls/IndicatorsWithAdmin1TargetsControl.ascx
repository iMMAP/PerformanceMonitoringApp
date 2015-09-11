<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IndicatorsWithAdmin1TargetsControl.ascx.cs"
    Inherits="SRFROWCA.Controls.IndicatorsWithAdmin1TargetsControl" %>
<style>
    .accordian {
        height: 20px;
        width: 20px;
        border: 1px solid gray;
        font-size: 24px;
        color: gray;
        line-height: 16px;
        text-align: center;
        float: left;
    }

    input[type="checkbox"] {
        margin-right: 10px;
        margin-top: 2px;
    }

        input[type="checkbox"] + label {
            margin-top: -3px;
        }
</style>

<script type="text/javascript">

    $(document).ready(function () {
        $('.tooltip2').tooltip();
        $(".tblGen").on('change', '.men', calTotal)
                      .on('change', '.women', calTotal);

        // find the value and calculate it

        function calTotal() {
            var $row = $(this).closest('tr'),
                price = parseInt($row.find('.men').val()),
                quantity = parseInt($row.find('.women').val());

            if (isNaN(price) || price == '')
                price = 0;

            if (isNaN(quantity) || quantity == '')
                quantity = 0;

            total = price + quantity;

            // change the value in total
            //$row.find('.total').val(total)
            $row.find('.total').val(total)
        }

        $(function () {
            $("#show-option").tooltip({
                show: {
                    effect: "slideDown",
                    delay: 250
                }
            });
            $("#hide-option").tooltip({
                hide: {
                    effect: "explode",
                    delay: 250
                }
            });
            $("#open-event").tooltip({
                show: null,
                position: {
                    my: "left top",
                    at: "left bottom"
                },
                open: function (event, ui) {
                    ui.tooltip.animate({ top: ui.tooltip.position().top + 10 }, "fast");
                }
            });
        });
    });


    jQuery(document).ready(function () {
        $("#<%=pAdmin1Target.ClientID%>").click(function () {
            jQuery(this).next(".content").slideToggle(500);
            if ($(this).text() == "Click To Show Admin1") {
                $(this).text("Click To Hide Admin1");
            }
            else {
                $(this).text("Click To Show Admin1");
            }
        });

        $("#<%=pAdmin1GenderTarget.ClientID%>").click(function () {
            jQuery(this).next(".content").slideToggle(500);
            if ($(this).text() == "Click To Show Admin1") {
                $(this).text("Click To Hide Admin1");
            }
            else {
                $(this).text("Click To Show Admin1");
            }
        });

        $('#<%=ddlUnit.ClientID%>').change(function () {
            var selVal = $("#<%=ddlUnit.ClientID%>").val();
            if (selVal == 269) {
                $("#<%=divAdmin1Targets.ClientID%>").addClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").removeClass('hidden');
            }
            else {
                $("#<%=divAdmin1Targets.ClientID%>").removeClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").addClass('hidden');
            }
        });

        var selVal2 = $("#<%=ddlUnit.ClientID%>").val();
        if (selVal2 == 269) {
            $("#<%=divAdmin1Targets.ClientID%>").addClass('hidden');
            $("#<%=divAdmin1GenderTargets.ClientID%>").removeClass('hidden');
        }
        else {
            $("#<%=divAdmin1Targets.ClientID%>").removeClass('hidden');
            $("#<%=divAdmin1GenderTargets.ClientID%>").addClass('hidden');
        }
    });
</script>
<script type="text/javascript">

    $(function () {
        $(".numeric1").wholenumber();
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

        <div class="widget-main no-padding-bottom no-padding-top">
            <div style="float: left; width: 32%;">
                <label>
                    <asp:HiddenField ID="hfIndicatorId" runat="server" />
                    (English):</label>
                <div>
                    <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine" Height="60px"
                        meta:resourcekey="txtInd1EngResource1"></asp:TextBox>

                </div>
            </div>
            <div style="float: left; width: 32%;">
                <label>
                    (French):</label>
                <div>
                    <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-95" TextMode="MultiLine" Height="60px"
                        meta:resourcekey="txtInd1FrResource1"></asp:TextBox>

                </div>
            </div>
            <div style="float: left; width: 19%;">
                <label>
                    Unit:</label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-90" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                </div>
            </div>
            <div style="float: left; width: 14%;">
                <label>
                    <span class='tooltip2' title='Each Indicator must have a calcualtion method. (1)Sum: Sum of all reported values.(2)Agerage: Average of all reported values.(3)Max: Max data reported in any month.</br>Latest: Latest reported data by month.'>Calculation: (?)</span>
                </label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlCalculationMethod" CssClass="width-100">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Sum" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Latest" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Max" Value="5"></asp:ListItem>
                    </asp:DropDownList>

                </div>

            </div>
        </div>
    </div>

</div>

<div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
    <div class="widget-box no-border">
        <div class="widget-body">
            <div class="widget-main no-padding-bottom no-padding-top" id="divAdmin1Targets" runat="server">
                <p id="pAdmin1Target" runat="server" style="width: 20%">Click To Show Admin1</p>
                <div class="content">
                    <asp:Repeater ID="rptAdmin1" runat="server">
                        <HeaderTemplate>
                            <table style="width: 400px;">
                                <tr style="background-color: lightgray">
                                    <td style="width: 300px;">Admin1</td>
                                    <td style="width: 100px;">Target</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 400px;">
                                <tr>
                                    <td style="width: 300px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%#Eval("ClusterTarget") %>' CssClass="numeric1" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField runat="server" ID="hfLocationId" Value='<%# Eval("LocationId") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <div class="widget-main no-padding-bottom no-padding-top hidden" id="divAdmin1GenderTargets" runat="server">
                <p id="pAdmin1GenderTarget" runat="server" style="width: 20%">Click To Show Admin1</p>
                <div class="content">
                    <asp:Repeater ID="rptAdmin1Gender" runat="server">
                        <HeaderTemplate>
                            <table style="width: 600px;">
                                <tr style="background-color: lightgray">
                                    <td style="width: 300px;">Admin1</td>
                                    <td style="width: 100px;">Men</td>
                                    <td style="width: 100px;">WoMen</td>
                                    <td style="width: 100px;">Total</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 600px;" class="tblGen">
                                <tr>
                                    <td style="width: 300px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTargetMen" runat="server" Text='<%#Eval("TargetMen") %>' CssClass="numeric1 men" MaxLength="9" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTargetWomen" runat="server" Text='<%#Eval("TargetWomen") %>' CssClass="numeric1 women" MaxLength="9" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTarget" Enabled="false" Text='<%#Eval("ClusterTarget") %>' runat="server" CssClass="numeric1 total" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField runat="server" ID="hfLocationId" Value='<%# Eval("LocationId") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
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
