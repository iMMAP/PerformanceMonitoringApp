<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrameworkIndicators.ascx.cs"
    Inherits="SRFROWCA.Controls.FrameworkIndicators" %>

<style>
    .rightAlignCBThis {
          
    }
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

    .details1 {
        display: none;
    }

    .txt {
        text-align: right;
        width: 100px;
        font-weight: bold;
    }
</style>

<script type="text/javascript">

    function pageLoad() {
        //LoadTaretControlOnUnit();
    }
    

    $(function () {
        var isGender1 = $("#<%=hfAdmin2CtlIsGender.ClientID%>").val();
        if (isGender1 === 'True')
        {
            $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetCtl:first').addClass('hidden');
            $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetGenderCtl:first').removeClass('hidden');
        }
        else {
            $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetCtl:first').removeClass('hidden');
            $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetGenderCtl:first').addClass('hidden');
        }
        //$('.tooltip2').tooltip();
        $(".tblGen").on('change', '.men', calTotal)
                      .on('change', '.women', calTotal);
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

        //LoadTaretControlOnUnit();

        $('#<%=ddlUnit.ClientID%>').change(function () {
            $.ajax({
                url: "AddActivityAndIndicators.aspx/IsGenderDisaggregated",
                data: JSON.stringify({
                    "unitId": $("#<%=ddlUnit.ClientID%>").val()
                }),
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.d) {
                        $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetCtl:first').addClass('hidden');
                        $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetGenderCtl:first').removeClass('hidden');
                    }
                    else {
                        $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetCtl:first').removeClass('hidden');
                        $('#<%=ddlUnit.ClientID%>').closest('.dvIndicator').find('.pnlTargets:first').find('.targetGenderCtl:first').addClass('hidden');
                    }
                }
            });
        });
        
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
    <h6 class="header blue bolder smaller">
        <asp:Localize ID="locIndicatorCap" runat="server" Text="Indicator" meta:resourcekey="locIndicatorCapResource1"></asp:Localize><asp:Label ID="lbl1stNumber" runat="server" meta:resourcekey="lbl1stNumberResource1"></asp:Label></h6>

    <div class="col-xs-12 col-sm-12 dvIndicator" style="padding-left: 0px;">
        <div class="widget-main no-padding-bottom no-padding-top">
            <div style="float: left; width: 32%;">

                <asp:HiddenField ID="hfIndicatorId" runat="server" />
                <asp:Label ID="lblIndEngCap" runat="server" Text="(English):" meta:resourcekey="lblIndEngCapResource1"></asp:Label>
                <div>
                    <asp:TextBox ID="txtInd1Eng" runat="server" CssClass="width-95" TextMode="MultiLine" Height="60px" MaxLength="1000"
                        meta:resourcekey="txtInd1EngResource1"></asp:TextBox>
                    <asp:CustomValidator ID="cvIndicator" runat="server" ClientValidationFunction="validateIndicator" ValidateEmptyText="True"
                        CssClass="error2" meta:resourcekey="cvIndicatorResource1"></asp:CustomValidator>
                </div>
            </div>
            <div style="float: left; width: 32%;">
                <asp:Label ID="lblIndFrCap" runat="server" Text="(French):" meta:resourcekey="lblIndFrCapResource1"></asp:Label>
                <div>
                    <asp:TextBox ID="txtInd1Fr" runat="server" CssClass="width-95" TextMode="MultiLine" Height="60px" MaxLength="1000"
                        meta:resourcekey="txtInd1FrResource1"></asp:TextBox>

                </div>
            </div>
            <div style="float: left; width: 12%;">
                <asp:Label ID="lblUnitCap" runat="server" Text="Unit:*" meta:resourcekey="lblUnitCapResource1"></asp:Label>
                <div>
                    <asp:DropDownList runat="server" ID="ddlUnit" CssClass="width-90 pullUnits" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvUnit" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlUnit" meta:resourcekey="rfvUnitResource1"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div style="float: left; width: 12%;">
                <asp:Localize ID="localCalMethod" runat="server" Text="<span class='calctemp'>Calculation:*</span>" meta:resourcekey="localCalMethodResource1"></asp:Localize>
                <asp:Localize ID="localCalMethodHelp" runat="server" Text="<span class='orshelpicon' data-rel='popover' data-placement='top' data-original-title='<i class='icon-wrench  bigger-110 icon-only'></i>Calculation Method' data-content='<b>Sum:</b> Running Sum of monthly achievements.<br/> <b>Average:</b> Averate of monthly achievements.<br/><b>Latest:</b> Latest recorded achievement<br/><b>Max:</b> Maximum value of achievements.'>?</span>" meta:resourcekey="localCalMethodHelpResource1"></asp:Localize>

                <div>
                    <asp:DropDownList runat="server" ID="ddlCalculationMethod" CssClass="width-100 pullCalc" meta:resourcekey="ddlCalculationMethodResource1">
                        <asp:ListItem Text="Select" Value="0" meta:resourcekey="ListItemResource1"></asp:ListItem>
                        <asp:ListItem Text="Sum" Value="1" meta:resourcekey="ListItemResource2"></asp:ListItem>
                        <asp:ListItem Text="Average" Value="2" meta:resourcekey="ListItemResource3"></asp:ListItem>
                        <asp:ListItem Text="Latest" Value="3" meta:resourcekey="ListItemResource4"></asp:ListItem>
                        <asp:ListItem Text="Max" Value="5" meta:resourcekey="ListItemResource5"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:RequiredFieldValidator ID="frvCalcMethod" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCalculationMethod" meta:resourcekey="frvCalcMethodResource1"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div style="float: left; width: 6%; margin-left:15px" class="cbCP">
                <asp:Label ID="lblCP" runat="server" Text="CP" ToolTip="Child Protection Indicator" CssClass="marig-10"></asp:Label>
                <div>
                    <asp:CheckBox ID="cbCP" runat="server" Text="Yes" TextAlign="Right" ToolTip="Child Protection Indicator"/>
                </div>
            </div>
        </div>        
        <asp:Panel ID="pnlTargets" runat="server" CssClass="pnlTargets"></asp:Panel>
        <asp:HiddenField runat="server" ID="hfAdmin2CtlIsGender" Value=""/>
    </div>
</div>
