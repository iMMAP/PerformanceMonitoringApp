<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntryRpt.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntryRpt" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .altcolor {
            background-color: #fbfbfb;
        }

        .istrow {
            background: #ebf0f4;
        }


        .itemWidth {
            width: 100%;
            align-self: center;
        }

        .displaynone {
            display: none;
        }

        .tdTable {
            width: 5%;
        }

        .tdTop {
            width: 15%;
            text-align: center;
            background-color: gray;
        }

        .tdHeader {
            width: 5%;
            text-align: center;
            background-color: lightgray;
        }

        .graycolor {
            text-align: center;
            background-color: gray;
        }

        .lightgraycolor {
            text-align: center;
            background-color: lightgray;
        }

        .details1 {
            display: none;
        }

        .details0 {
            display: none;
        }

        #MainContent_cblLocations td {
            padding: 0 40px 0 0;
        }

        textarea, input[type="text"] {
            border: 1px solid #D5D5D5;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 11px;
            line-height: 1.2;
            padding: 2px 1px;
            transition-duration: 0.1s;
            text-align: right;
        }

        .commentstext {
            border: 1px solid #D5D5D5;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 12px;
            line-height: 1.2;
            padding: 0 0;
            transition-duration: 0.1s;
            text-align: left;
        }

        .langlinks {
            color: white;
        }
    </style>
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var needToConfirm = true;

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (needToConfirm) {
                var message = '';
                var e = e || window.event;
                // For IE and Firefox prior to version 4
                if (e) {
                    e.returnValue = message;
                }
                // For Safari
                return message;
            }
        }

        var launch = false;
        function launchModal() {
            launch = true;
        }

        var launchUserActivity = false;
        function launchUserActivityModal() {
            launchUserActivity = true;
        }

        function pageLoad() {
            if (launch) {
                $find("mpeAddActivity").show();
            }

            if (launchUserActivity) {
                $find("mpeUserActivity").show();
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function showHideObj() {
            $(document).on('click', '.checkObj', function () {
                var selectedObjs = [];
                $("[id*=cblObjectives] input:checked").each(function () {
                    selectedObjs.push($(this).val());
                });

                showAllObj();
                if (selectedObjs.length > 0) {
                    hideAllObj();
                    var i;
                    for (i = 0; i < selectedObjs.length; ++i) {
                        showObj(selectedObjs[i]);
                    }
                }
            });
        }

        function showAllObj() {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                $(this).parent().show();
            });
        }

        function hideAllObj() {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                $(this).parent().hide();
            });
        }

        function showObj(objId) {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                if ($(this).text() === objId || objId === '0') {
                    $(this).parent().show();
                }
            });
        }

        $(function () {
            //$('.showDetails1').click(function () {
            //    $(this).parent().parent().next('tr.details1').toggle();
            //    $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
            //                                                 '../assets/orsimages/minus.png' :
            //                                                  '../assets/orsimages/plus.png'))
            //});

            //$('.showDetails0').click(function () {
            //    //alert($(this).parent().parent().parent().parent().attr('class'));
            //    //alert($(this).parent().parent().parent().parent().find('tr.details0').attr('class'));

            //    $(this).parent().parent().parent().parent().find('tr.details0').toggle();
            //    $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
            //                                                 '../assets/orsimages/minus.png' :
            //                                                  '../assets/orsimages/plus.png'))
            //});

            showHideObj();

            allowOnlyNumeric();

            // Split location name from Target(T) and Achieved(A).
            splitLocationFromTA();

            $('.cbltest').on('click', ':checkbox', function () {
                if ($(this).is(':checked')) {
                    $(this).parent().addClass('highlight');
                }
                else {
                    $(this).parent().removeClass('highlight');
                }
            });

            $(".cbltest").find(":checkbox").each(function () {
                if ($(this).is(':checked')) {
                    $(this).parent().addClass('highlight');
                }
            });
        });

        function allowOnlyNumeric() {
            $(".numeric1").keydown(function (event) {
                // Allow: backspace, delete, tab, escape, enter
                if ($.inArray(event.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
                    // Allow: Ctrl+A
            (event.keyCode == 65 && event.ctrlKey === true) ||
                    // Allow: home, end, left, right
            (event.keyCode >= 35 && event.keyCode <= 39)) {
                    // let it happen, don't do anything
                    return;
                }
                else {
                    // Ensure that it is a number and stop the keypress
                    if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                        event.preventDefault();
                    }
                }
            });
        }

        // Split location namde and 'T' (means Target) and 'A' (means Achieved)
        function splitLocationFromTA() {
            //if (!(/chrom(e|ium)/.test(navigator.userAgent.toLowerCase())))
            {
                var list = '';
                var list2 = '';
                var j = 0;

                // Loop on all th in grid.
                $(".imagetable th").each(function () {
                    var value = ($(":first-child", this).is(":input"))
                        ? $(":first-child", this).val()
                        : ($(this).text() != "")
                            ? $(this).text()
                            : $(this).html();

                    // if contains '_' (which is passed from db in column name)
                    if (value.indexOf('_') >= 0) {
                        j++;
                        city1 = value.split('_');
                        $(this).text(city1[1]);

                        // Add city name after every two columns (first is T and second is A)
                        if (j % 1 === 0) {
                            list += '<th colspan="1" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                // Add header row in grid.
                //$(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th style="width: 70px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 70px;">&nbsp;</th><th style="width: 40px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" border="1">

    <div class="page-content">
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <asp:Localize ID="locClusterCaption" runat="server"
                            Text="Cluster:" meta:resourcekey="locClusterCaptionResource1"></asp:Localize>
                        <asp:Label ID="lblCluster" runat="server" meta:resourcekey="lblClusterResource1"></asp:Label>
                        <div class="pull-right">
                            <asp:LinkButton ID="lnkLanguageEnglish" Text="English" runat="server" OnClientClick="needToConfirm=false;" CssClass="langlinks"
                                CausesValidation="False" meta:resourcekey="lnkLanguageEnglishResource1"></asp:LinkButton>&nbsp;&nbsp;

                            <asp:LinkButton ID="lnkLanguageFrench" Text="Français" runat="server" OnClientClick="needToConfirm=false;" CssClass="langlinks"
                                CausesValidation="False" meta:resourcekey="lnkLanguageFrenchResource1"></asp:LinkButton>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Localize ID="locaNoTargetMessage" runat="server" Text="&lt;div style=&quot;color:Red;&quot;&gt;Please click on Locations button to add locations. You will then be able to add or edit your project activities.<br/>Please select the activties that your project is working on by ticking in the Select Column." meta:resourcekey="locaNoTargetMessageResource1"></asp:Localize>
                            </div>

                            <div class="spacer" style="clear: both;">
                            </div>
                            <br />
                            <div id="divMsg">
                            </div>
                            <div>
                                Filter Activities:<asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj" RepeatColumns="3"></asp:CheckBoxList>
                            </div>

                            <asp:Repeater ID="rptIndicators" runat="server" OnItemDataBound="rptIndicators_ItemDataBound">
                                <HeaderTemplate>
                                    <table style="margin: 0 auto; width: 100%;" border="1">
                                        <tr>
                                            <th style="width: 5%;" class="graycolor"></th>
                                            <th style="width: 15%;" class="graycolor"></th>
                                            <th style="width: 15%;" class="graycolor"></th>
                                            <th style="width: 5%;" class="graycolor"></th>
                                            <th style="width: 5%;" class="graycolor"></th>
                                            <th style="width: 5%;" class="graycolor"></th>
                                            <th class="tdTop" colspan="3">Cluster Target</th>
                                            <th class="tdTop" colspan="3">Project Target</th>
                                        </tr>
                                        <tr>
                                            <th style="width: 5%;" class="lightgraycolor">Obj</th>
                                            <th style="width: 15%;" class="lightgraycolor">Activity</th>
                                            <th style="width: 15%;" class="lightgraycolor">Indicator</th>
                                            <th style="width: 5%;" class="lightgraycolor">Unit</th>
                                            <th style="width: 5%;" class="lightgraycolor">Method</th>
                                            <th style="width: 5%;" class="lightgraycolor">Location</th>
                                            <th class="tdHeader">Total</th>
                                            <th class="tdHeader">Men</th>
                                            <th class="tdHeader">Women</th>
                                            <th class="tdHeader">Total</th>
                                            <th class="tdHeader">Men</th>
                                            <th class="tdHeader">Women</th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="margin: 0 auto; width: 100%;" border="1">
                                        <tr>
                                            <td style="width: 5%;">
                                                <asp:Label ID="lblKeyFigureIndicator" runat="server" Width="98%" Text='<%# Eval("Objective")%>'></asp:Label>
                                                <asp:HiddenField ID="hfKeyFigureIndicatorId" runat="server" Value='<%# Eval("ObjectiveId")%>' />
                                                <asp:HiddenField ID="hfKeyFigureReportId" runat="server" Value='<%# Eval("IndicatorId")%>' />
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lblGridActivity" runat="server" Text='<%# Eval("Activity") %>'
                                                    meta:resourcekey="lblGridActivityResource1"></asp:Label>
                                            </td>
                                            <td style="width: 15%;">
                                                <asp:Label ID="lblGridIndicator" runat="server" Text='<%# Eval("Indicator") %>' meta:resourcekey="lblGridIndicatorResource1"></asp:Label>
                                            </td>
                                            <td style="width: 5%;">
                                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                            </td>
                                            <td style="width: 5%;">
                                                <asp:Label ID="lblCalMethod" runat="server" Text='<%# Eval("CalculationType") %>'></asp:Label>
                                            </td>
                                            <td style="width: 5%; text-align: left;">
                                                <asp:Label ID="lblCountry" runat="server" Width="98%" Text=''></asp:Label>
                                                <asp:HiddenField ID="hfLocationId" runat="server" Value='<%# Eval("LocationId")%>' />
                                                <span class="showDetails1" style="font-size: smaller; color: blue; cursor: pointer;">Show Admin1</span>
                                                <%--<input type="button" id="Admin1" class="showDetails1" title="btn" value="Admin1" name="Admin1" runat="server"/>--%>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5"></td>
                                            <td colspan="7">
                                                <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="margin: 0 auto; width: 100%;" border="1">
                                                            <tr style="background-color: #EEEEEE" class="trAdmin1">
                                                                <td style="width: 200px;">
                                                                    <img src="../assets/orsimages/plus.png" class="showDetails1"
                                                                        title="Click to show/hide Admin2" />
                                                                    <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                    <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtTargetMale" runat="server" Text='<%#Eval("Admin1TargetMale") %>' ToolTip="Admin1 Male Total"
                                                                        CssClass="numeric1 trgtAdmin1GenderMale"
                                                                        Style="text-align: right;" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtTargetFeMale" runat="server" Text='<%#Eval("Admin1TargetFeMale") %>' ToolTip="Admin1 Female Total"
                                                                        CssClass="numeric1 trgtAdmin1GenderFemale"
                                                                        Style="text-align: right;" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtTarget" runat="server" Text='<%#Eval("Admin1Target") %>'
                                                                        ToolTip="Admin1 Total" CssClass="numeric1 trgtAdmin1GenderTotal"
                                                                        Style="text-align: right;" Width="50px" Enabled="false"></asp:TextBox>
                                                                </td>
                                                                <td><asp:TextBox ID="TextBox6" runat="server" Enabled="false"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="TextBox4" runat="server" Enabled="false"></asp:TextBox></td>
                                                                <td><asp:TextBox ID="TextBox5" runat="server" Enabled="false"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:Repeater ID="rptAdmin2" runat="server">
                                                                        <ItemTemplate>
                                                                            <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin2Gender">
                                                                                <tr>
                                                                                    <td style="width: 200px;">
                                                                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                        <asp:HiddenField ID="hfAdmin2Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtTargetMale" Style="text-align: right;" runat="server"
                                                                                            Text='<%#Eval("TargetMale") %>' Enabled="false"
                                                                                            CssClass="numeric1 trgtAdmin2GenderMale" Width="50px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtTargetFemale" Style="text-align: right;" runat="server"
                                                                                            Text='<%#Eval("TargetFemale") %>' Enabled="false"
                                                                                            CssClass="numeric1 trgtAdmin2GenderFemale" Width="50px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtTarget" Style="text-align: right;" runat="server"
                                                                                            Text='<%#Eval("ClusterTarget") %>' ToolTip="Admin2 Total"
                                                                                            CssClass="numeric1 trgtAdmin2GenderTotal" Width="50px"
                                                                                            Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                                                                                    <td><asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                                                                                    <td><asp:TextBox ID="TextBox3" runat="server"></asp:TextBox></td>
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
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
