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

    .details1 {
        display: none;
    }
</style>

<script type="text/javascript">

    $(document).ready(function () {


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
                    <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptCountry_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 500px;" class="imagetable tblCountry">
                                <tr style="background-color: gray">
                                    <td style="width: 360px;">Location</td>
                                    <td style="width: 100px;">Target</td>
                                </tr>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr style="background-color: #C8C8C8">
                                    <td style="width: 360px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                        <asp:HiddenField ID="hfCountryId" runat="server" Value='<%#Eval("LocationId")%>' />
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtCountryTarget" runat="server" Text='<%#Eval("CountryTarget") %>'
                                            CssClass="numeric1 trgtCountry" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 500px;" colspan="2">
                                        <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1_ItemDataBound">
                                            <ItemTemplate>
                                                <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1">
                                                    <tr style="background-color: #EEEEEE">
                                                        <td>
                                                            <img src="../assets/orsimages/plus.png" class="showDetails1" /></td>
                                                        <td style="width: 355px;">

                                                            <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                            <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtAdmin1Target" runat="server" Text='<%#Eval("Admin1Target") %>'
                                                                CssClass="numeric1 trgtAdmin1" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
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
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtAdmin2Target" Style="text-align: right;" runat="server"
                                                                                    Text='<%#Eval("ClusterTarget") %>' CssClass="numeric1 trgtAdmin2" Width="100px"></asp:TextBox>
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
                <p id="pAdmin1GenderTarget" runat="server" style="width: 20%">Click To Show Admin1</p>
                <div class="content">
                    <asp:Repeater ID="rptCountryGender" runat="server" OnItemDataBound="rptCountryGender_ItemDataBound">
                        <HeaderTemplate>
                            <table style="width: 600px;" class="imagetable tblCountryGender">
                                <tr style="background-color: gray">
                                    <td style="width: 260px;">Location</td>
                                    <td style="width: 100px;">Male</td>
                                    <td style="width: 100px;">Female</td>
                                    <td style="width: 100px;">Total</td>
                                </tr>
                            
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr style="background-color: #C8C8C8">
                                    <td style="width: 260px;">
                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                        <asp:HiddenField ID="hfCountryIdGender" runat="server" Value='<%#Eval("LocationId")%>' />
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtCountryTargetMale" runat="server" Text='<%#Eval("CountryTargetMale") %>'
                                            CssClass="numeric1 trgtCountryGenderMale" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtCountryTargetFemale" runat="server" Text='<%#Eval("CountryTargetFeMale") %>'
                                            CssClass="numeric1 trgtCountryGenderFemale" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtCountryTargetTotal" runat="server" Text='<%#Eval("CountryTarget") %>'
                                            CssClass="numeric1 trgtCountryGenderTotal" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 600px;" colspan="4">
                                        <asp:Repeater ID="rptAdmin1Gender" runat="server" OnItemDataBound="rptAdmin1Gender_ItemDataBound">
                                            <ItemTemplate>
                                                <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1Gender">
                                                    <tr style="background-color: #EEEEEE" class="trAdmin1">
                                                        <td>
                                                            <img src="../assets/orsimages/plus.png" class="showDetails1" /></td>
                                                        <td style="width: 240px;">

                                                            <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                            <asp:HiddenField ID="hfAdmin1IdGender" runat="server" Value='<%#Eval("LocationId")%>' />
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtAdmin1TargetMale" runat="server" Text='<%#Eval("Admin1TargetMale") %>'
                                                                CssClass="numeric1 trgtAdmin1GenderMale" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtAdmin1TargetFeMale" runat="server" Text='<%#Eval("Admin1TargetFeMale") %>'
                                                                CssClass="numeric1 trgtAdmin1GenderFemale" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                                        </td>
                                                        <td class="tdTable">
                                                            <asp:TextBox ID="txtAdmin1TargetTotal" runat="server" Text='<%#Eval("Admin1Target") %>'
                                                                CssClass="numeric1 trgtAdmin1GenderTotal" Style="text-align: right;" Width="100px" ReadOnly="true"></asp:TextBox>
                                                        </td>

                                                    </tr>
                                                    <tr class="details1">
                                                        <td></td>
                                                        <td style="width: 500px;" colspan="4">
                                                            <asp:Repeater ID="rptAdmin2Gender" runat="server">
                                                                <ItemTemplate>
                                                                    <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin2Gender">
                                                                        <tr>
                                                                            <td style="width: 400px;">
                                                                                <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtAdmin2TargetMale" Style="text-align: right;" runat="server"
                                                                                    Text='<%#Eval("TargetMale") %>' CssClass="numeric1 trgtAdmin2GenderMale" Width="100px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtAdmin2TargetFemale" Style="text-align: right;" runat="server"
                                                                                    Text='<%#Eval("TargetFemale") %>' CssClass="numeric1 trgtAdmin2GenderFemale" Width="100px"></asp:TextBox>
                                                                            </td>
                                                                            <td class="tdTable">
                                                                                <asp:TextBox ID="txtAdmin2GenderTotal" Style="text-align: right;" runat="server"
                                                                                    Text='<%#Eval("ClusterTarget") %>' ReadOnly="true"
                                                                                    CssClass="numeric1 trgtAdmin2GenderTotal" Width="100px"></asp:TextBox>
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
