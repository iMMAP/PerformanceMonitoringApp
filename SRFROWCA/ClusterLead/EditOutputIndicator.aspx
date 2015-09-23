<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditOutputIndicator.aspx.cs" Inherits="SRFROWCA.ClusterLead.EditOutputIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField runat="server" ID="hdnIsRegional" Value="0" />
    <div class="page-content">


        <table border="0" style="margin: 0 auto; width: 80%">
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="Button1" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
                    <asp:Button ID="Button2" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                </td>
            </tr>
            <tr>
                <td width="100px">Country:*
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" Width="250px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Cluster:*
                </td>
                <td>
                    <asp:DropDownList ID="ddlCluster" runat="server" Width="250px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Indicator (Eng):
                                        <asp:HiddenField ID="hfIndicatorId" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtInd1Eng" runat="server" Width="500px" TextMode="MultiLine"
                        meta:resourcekey="txtInd1EngResource1" Style="height: 80px;"></asp:TextBox>
                    <asp:CustomValidator ID="cvActivityEng" runat="server" ClientValidationFunction="validateIndicators"
                        CssClass="error2"></asp:CustomValidator></td>
            </tr>
            <tr>
                <td>Indicator (Fr):</td>
                <td>
                    <asp:TextBox ID="txtInd1Fr" runat="server" Width="500px"
                        TextMode="MultiLine" meta:resourcekey="txtInd1FrResource1"
                        Style="height: 80px;"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Units:*</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlUnit" Width="250px" meta:resourcekey="ddlUnitResource1"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvUnits" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlUnit"></asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td><span class='tooltip2' title='Each Indicator must have a calcualtion method. (1)Sum: Sum of all reported values.(2)Agerage: Average of all reported values.(3)Max: Max data reported in any month.</br>Latest: Latest reported data by month.'>Calculation:* (?)</span></td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlCalculationMethod" Width="250px" CssClass="pullCalc">
                        <asp:ListItem Text="Select Calculation" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Sum" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Latest" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Max" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="frvCalcMethod" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCalculationMethod"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblIndTargetCaption" runat="server" Text="Indicator Target"></asp:Label></td>
                <td>
                    <div class="col-xs-12 col-sm-12" style="float: left; margin-bottom: 10px; padding-left: 0px;" id="divTargets" runat="server">
                        <div class="widget-box no-border">
                            <div class="widget-body">
                                <div>
                                    <div class="widget-main no-padding-bottom no-padding-top" id="divAdmin1Targets" runat="server">
                                        <a id="pAdmin1Target" runat="server" style="width: 20%">Click To Show Locations</a>
                                        <div class="content">
                                            <asp:Repeater ID="rptAdmin" runat="server">
                                                <HeaderTemplate>
                                                    <table style="width: 300px;">
                                                        <tr style="background-color: gray">
                                                            <td style="width: 200px;">Location</td>
                                                            <td style="width: 100px;">Target</td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table style="width: 300px;">
                                                        <tr style="background-color: #C8C8C8">
                                                            <td style="width: 200px;">
                                                                <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%#Eval("LocationId")%>' />
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <asp:TextBox ID="txtTarget" Width="100px" runat="server" 
                                                                    Text='<%#Eval("CountryTarget") %>' ToolTip="Country Total"
                                                                    CssClass="numeric1 trgtCountry txt"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="widget-main no-padding-bottom no-padding-top hidden" id="divAdmin1GenderTargets" runat="server">
                            <a id="pAdmin1GenderTarget" runat="server" style="width: 20%">Click To Show Locations</a>
                            <div class="content">
                                <asp:Repeater ID="rptAdmin1Gender" runat="server">
                                    <HeaderTemplate>
                                        <table style="width: 500px;">
                                            <tr style="background-color: gray">
                                                <td style="width: 200px;">Location</td>
                                                <td style="width: 100px;">Male</td>
                                                <td style="width: 100px;">Female</td>
                                                <td style="width: 100px;">Total</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 500px;">
                                            <tr style="background-color: #C8C8C8">
                                                <td style="width: 200px;">
                                                    <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                    <asp:HiddenField ID="hdnLocationId" runat="server" Value='<%#Eval("LocationId")%>' />
                                                </td>
                                                <td style="width: 100px;">
                                                    <asp:TextBox ID="txtTargetMale" Width="100px" runat="server" Text='<%#Eval("TargetMale") %>' ToolTip="Country Male Total"
                                                        CssClass="numeric1 trgtCountryGenderMale txt"></asp:TextBox>
                                                </td>
                                                <td style="width: 100px;">
                                                    <asp:TextBox ID="txtTargetFemale" Width="100px" runat="server" Text='<%#Eval("TargetFeMale") %>' ToolTip="Country Female Total"
                                                        CssClass="numeric1 trgtCountryGenderFemale txt"></asp:TextBox>
                                                </td>
                                                <td style="width: 100px;">
                                                    <asp:TextBox ID="txtTarget" Width="100px" runat="server" Text='<%#Eval("CountryTarget") %>'
                                                        CssClass="numeric1 trgtCountryGenderTotal txt" ToolTip="Country Total"
                                                        Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="width-10 btn btn-sm btn-primary" />
                    <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                        CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                </td>
            </tr>
        </table>

    </div>
    <div id="divMsg">
    </div>
    <script>
        function validateIndicators(sender, args) {
            var txtEng = $("[id$=txtInd1Eng]").val();
            var txtFr = $("[id$=txtInd1Fr]").val();

            var countryId = $("#<%=ddlCountry.ClientID%>").val();
            var clusterId = $("#<%=ddlCluster.ClientID%>").val();
            var unitId = $("#<%=ddlUnit.ClientID%>").val();
            var calcId = $("#<%=ddlCalculationMethod.ClientID%>").val();

            if (countryId == "0" || clusterId == "0" || unitId == "0" || calcId == "0") {
                args.IsValid = false;
            }
            else if (txtEng.trim() == '' && txtFr.trim() == '') {
                alert("Please add Indicator atleast in one Language!")
                args.IsValid = false;
            }
            else
                arg.IsValid = true;
        }

        $(document).ready(function () {
            $(".content").hide();
            $(".numeric1").wholenumber();

            $('.tooltip2').tooltip();
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
                    || selVal == 252) {
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
                    || selVal2 == 252) {
                $("#<%=divAdmin1Targets.ClientID%>").addClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").removeClass('hidden');
            }
            else {
                $("#<%=divAdmin1Targets.ClientID%>").removeClass('hidden');
                $("#<%=divAdmin1GenderTargets.ClientID%>").addClass('hidden');
            }

        });

    </script>
</asp:Content>
