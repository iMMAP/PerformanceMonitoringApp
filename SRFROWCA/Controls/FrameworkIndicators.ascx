<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FrameworkIndicators.ascx.cs"
    Inherits="SRFROWCA.Controls.FrameworkIndicators" %>
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
    $(function () {
        $(".content").hide();
        $('.tooltip2').tooltip();
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

        $("#<%=pAdmin1Target.ClientID%>").click(function () {
            jQuery(this).next(".content").toggle();
            if ($(this).text() == "Click To Show Locations") {
                $(this).text("Click To Hide Locations");
            }
            else {
                $(this).text("Click To Show Locations");
            }
        });

        $("#<%=pAdmin1GenderTarget.ClientID%>").click(function () {
            jQuery(this).next(".content").toggle();
            if ($(this).text() == "Click To Show Locations") {
                $(this).text("Click To Hide Locations");
            }
            else {
                $(this).text("Click To Show Locations");
            }
        });

        $('#<%=ddlUnit.ClientID%>').change(function () {
            var selVal = $("#<%=ddlUnit.ClientID%>").val();
            if (selVal == 269 || selVal == 28 || selVal == 38
                || selVal == 193 || selVal == 219 || selVal == 198
                 || selVal == 311 || selVal == 287 || selVal == 67 || selVal == 132
                || selVal == 252 || selVal == 238) {
                $("#<%=divAdmin1Targets.ClientID%>").addClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").removeClass('hidden');
            }
            else {
                $("#<%=divAdmin1Targets.ClientID%>").removeClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").addClass('hidden');
            }
        });

        var selVal2 = $("#<%=ddlUnit.ClientID%>").val();
        if (selVal2 == 269 || selVal2 == 28 || selVal2 == 38
                || selVal2 == 193 || selVal2 == 219 || selVal2 == 198
                 || selVal2 == 311 || selVal2 == 287 || selVal2 == 67 || selVal2 == 132
                || selVal2 == 252 || selVal2 == 238) {
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
            <div style="float: left; width: 17%;">
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
        </div>

    </div>

<div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;">
    <div class="widget-box no-border">
        <div class="widget-body">
            <div id="divAdmin2Targets" runat="server">
                <div class="widget-main no-padding-bottom no-padding-top" id="divAdmin1Targets" runat="server">
                    <p style="color: red">
                        <asp:Localize ID="localProvideTargetsCap" runat="server" Text="Providing targets is mandatory." meta:resourcekey="localProvideTargetsCapResource1"></asp:Localize>
                    </p>
                    <a id="pAdmin1Target" runat="server" style="width: 20%">
                        <asp:Localize ID="localClickToShow1" runat="server" Text="Click To Show Locations" meta:resourcekey="localClickToShow1Resource1"></asp:Localize></a>
                    <div class="content">
                        <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptCountry_ItemDataBound">
                            <HeaderTemplate>
                                <table style="width: 500px;" class="imagetable tblCountry">
                                    <tr style="background-color: gray">
                                        <td style="width: 360px;">
                                            <asp:Label ID="lblRptLoc" runat="server" Text="Location" meta:resourcekey="lblRptLocResource1"></asp:Label></td>
                                        <td style="width: 100px;">
                                            <asp:Label runat="server" Text="Target" meta:resourcekey="LabelResource1"></asp:Label></td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #C8C8C8">
                                    <td style="width: 360px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                        <asp:HiddenField ID="hfCountryId" runat="server" Value='<%# Eval("LocationId") %>' />
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("CountryTarget") %>' ToolTip="Country Total"
                                            CssClass="numeric1 trgtCountry txt" Enabled="False" meta:resourcekey="txtTargetResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 500px;" colspan="2">
                                        <a class="showall">Expand All</a>
                                        <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1_ItemDataBound">
                                            <ItemTemplate>
                                                <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1">
                                                    <tr style="background-color: #EEEEEE">
                                                        <td>
                                                            <img src="../assets/orsimages/plus.png" class="showDetails1" title="Click to show/hide Admin2" /></td>
                                                        <td style="width: 355px;">

                                                            <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                            <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("Admin1Target") %>' ToolTip="Admin1 Total"
                                                                CssClass="numeric1 trgtAdmin1" Style="text-align: right;" Width="100px" Enabled="False" meta:resourcekey="txtTargetResource2"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr class="details1">
                                                        <td></td>
                                                        <td style="width: 500px;" colspan="2">
                                                            <asp:Repeater ID="rptAdmin2" runat="server">
                                                                <ItemTemplate>
                                                                    <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin2">
                                                                        <tr>
                                                                            <td style="width: 400px;">
                                                                                <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                <asp:HiddenField ID="hfAdmin2Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtTarget" Style="text-align: right;" runat="server"
                                                                                    Text='<%# Eval("ClusterTarget") %>' CssClass="numeric1 trgtAdmin2" Width="100px" meta:resourcekey="txtTargetResource3"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                </table>

                            <asp:HiddenField runat="server" ID="hfLocationId" Value='<%# Eval("LocationId") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="widget-main no-padding-bottom no-padding-top hidden" id="divAdmin1GenderTargets" runat="server">
                    <p style="color: red">Providing targets is mandatory.</p>
                    <a id="pAdmin1GenderTarget" runat="server" style="width: 20%">
                        <asp:Localize ID="localClickShow2" runat="server" Text="Click To Show Locations" meta:resourcekey="localClickShow2Resource1"></asp:Localize></a>
                    <div class="content">
                        <asp:Repeater ID="rptCountryGender" runat="server" OnItemDataBound="rptCountryGender_ItemDataBound">
                            <HeaderTemplate>
                                <table style="width: 600px;" class="imagetable tblCountryGender">
                                    <tr style="background-color: gray">
                                        <td style="width: 260px;">
                                            <asp:Label ID="lblRptGenLoc" runat="server" Text="Location" meta:resourcekey="lblRptGenLocResource1"></asp:Label></td>
                                        <td style="width: 100px;">
                                            <asp:Label ID="lblRptGenMale" runat="server" Text="Male" meta:resourcekey="lblRptGenMaleResource1"></asp:Label></td>
                                        <td style="width: 100px;">
                                            <asp:Label ID="lblRptGenFemale" runat="server" Text="Female" meta:resourcekey="lblRptGenFemaleResource1"></asp:Label></td>
                                        <td style="width: 100px;">
                                            <asp:Label ID="lblRptGenTotal" runat="server" Text="Total" meta:resourcekey="lblRptGenTotalResource1"></asp:Label></td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr style="background-color: #C8C8C8">
                                    <td style="width: 260px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                        <asp:HiddenField ID="hfCountryId" runat="server" Value='<%# Eval("LocationId") %>' />
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTargetMale" runat="server" Text='<%# Eval("CountryTargetMale") %>' ToolTip="Country Male Total"
                                            CssClass="numeric1 trgtCountryGenderMale txt" Enabled="False" meta:resourcekey="txtTargetMaleResource1"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTargetFemale" runat="server" Text='<%# Eval("CountryTargetFeMale") %>' ToolTip="Country Female Total"
                                            CssClass="numeric1 trgtCountryGenderFemale txt" Enabled="False" meta:resourcekey="txtTargetFemaleResource1"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("CountryTarget") %>'
                                            CssClass="numeric1 trgtCountryGenderTotal txt" ToolTip="Country Total"
                                            Enabled="False" meta:resourcekey="txtTargetResource4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 600px;" colspan="4">
                                        <a class="showallGen">Expand All</a>
                                        <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1Gender_ItemDataBound">
                                            <ItemTemplate>
                                                <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1Gender">
                                                    <tr style="background-color: #EEEEEE" class="trAdmin1">
                                                        <td>
                                                            <img src="../assets/orsimages/plus.png" class="showDetails1" title="Click to show/hide Admin2" /></td>
                                                        <td style="width: 240px;">

                                                            <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                            <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtTargetMale" runat="server" Text='<%# Eval("Admin1TargetMale") %>' ToolTip="Admin1 Male Total"
                                                                CssClass="numeric1 trgtAdmin1GenderMale" Style="text-align: right;" Width="100px" Enabled="False" meta:resourcekey="txtTargetMaleResource2"></asp:TextBox>
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtTargetFeMale" runat="server" Text='<%# Eval("Admin1TargetFeMale") %>' ToolTip="Admin1 Female Total"
                                                                CssClass="numeric1 trgtAdmin1GenderFemale" Style="text-align: right;" Width="100px" Enabled="False" meta:resourcekey="txtTargetFeMaleResource2"></asp:TextBox>
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtTarget" runat="server" Text='<%# Eval("Admin1Target") %>' ToolTip="Admin1 Total"
                                                                CssClass="numeric1 trgtAdmin1GenderTotal" Style="text-align: right;"
                                                                Width="100px" Enabled="False" meta:resourcekey="txtTargetResource5"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr class="details1">
                                                        <td></td>
                                                        <td style="width: 500px;" colspan="4">
                                                            <asp:Repeater ID="rptAdmin2" runat="server">
                                                                <ItemTemplate>
                                                                    <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin2Gender">
                                                                        <tr>
                                                                            <td style="width: 400px;">
                                                                                <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                <asp:HiddenField ID="hfAdmin2Id" runat="server" Value='<%# Eval("LocationId") %>' />
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtTargetMale" Style="text-align: right;" runat="server"
                                                                                    Text='<%# Eval("TargetMale") %>' CssClass="numeric1 trgtAdmin2GenderMale" Width="100px" meta:resourcekey="txtTargetMaleResource3"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtTargetFemale" Style="text-align: right;" runat="server"
                                                                                    Text='<%# Eval("TargetFemale") %>' CssClass="numeric1 trgtAdmin2GenderFemale" Width="100px" meta:resourcekey="txtTargetFemaleResource3"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtTarget" Style="text-align: right;" runat="server"
                                                                                    Text='<%# Eval("ClusterTarget") %>' ToolTip="Admin2 Total"
                                                                                    CssClass="numeric1 trgtAdmin2GenderTotal" Width="100px"
                                                                                    Enabled="False" meta:resourcekey="txtTargetResource6"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                                </table>

                            <asp:HiddenField runat="server" ID="hfLocationIdGender" Value='<%# Eval("LocationId") %>' />
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
