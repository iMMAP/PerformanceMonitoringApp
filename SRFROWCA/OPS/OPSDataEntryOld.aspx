<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntryOld.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntryOld" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_cblLocations td
        {
            padding: 0 40px 0 0;
        }

        textarea, input[type="text"]
        {
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

        .commentstext
        {
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

        .langlinks
        {
            color: white;
        }
    </style>
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
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
        $(function () {
            showHideObj();
            showHidePriority();

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
                $(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th style="width: 70px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 70px;">&nbsp;</th><th style="width: 40px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" border="1">

    <div class="page-content">
        <div class="row">
            <div class="col-sm-3">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">

                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndStrObjs" runat="server"
                                                Text="Strategic Objectives" meta:resourcekey="lzeLgndStrObjsResource1"></asp:Localize>
                                        </h5>

                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj" meta:resourcekey="cblObjectivesResource1">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndHumPriorities" runat="server"
                                                Text="Humanitarian Priorities" meta:resourcekey="lzeLgndHumPrioritiesResource1"></asp:Localize>
                                        </h5>

                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr" meta:resourcekey="cblPrioritiesResource1">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <asp:Localize ID="locClusterCaption" runat="server"
                            Text="Cluster:" meta:resourcekey="locClusterCaptionResource1"></asp:Localize>
                        <asp:Label ID="lblCluster" runat="server" meta:resourcekey="lblClusterResource1"></asp:Label>
                        <div class="pull-right">
                            <asp:LinkButton ID="lnkLanguageEnglish" Text="English" runat="server" OnClientClick="needToConfirm=false;" CssClass="langlinks"
                                CausesValidation="False" OnClick="lnkLanguageEnglish_Click" meta:resourcekey="lnkLanguageEnglishResource1"></asp:LinkButton>&nbsp;&nbsp;

                            <asp:LinkButton ID="lnkLanguageFrench" Text="Français" runat="server" OnClientClick="needToConfirm=false;" CssClass="langlinks"
                                CausesValidation="False" OnClick="lnkLanguageFrench_Click" meta:resourcekey="lnkLanguageFrenchResource1"></asp:LinkButton>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                                    CssClass="btn btn-primary" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                                    meta:resourcekey="btnOpenLocationsResource1" />
                                <asp:Localize ID="locaNoTargetMessage" runat="server" Text="&lt;div style=&quot;color:Red;&quot;&gt;Please click on Locations button to add locations. You will then be able to add or edit your project activities." meta:resourcekey="locaNoTargetMessageResource1"></asp:Localize>
                            </div>
                            <div class="pull-right">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                                    CausesValidation="False" Width="120px" CssClass="btn btn-primary" meta:resourcekey="btnSaveResource1" />
                                <asp:Localize ID="locbtnCloseWindow" runat="server" Text="&lt;input type=&quot;button&quot; class=&quot;btn btn-primary&quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"
                                    meta:resourcekey="locbtnCloseWindowResource1"></asp:Localize>
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                            <br />
                            <div id="divMsg">
                            </div>
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
                                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                    HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                                    Width="100%" OnRowDataBound="gvActivities_RowDataBound" meta:resourcekey="gvActivitiesResource1">
                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                                        <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                            ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"
                                            meta:resourcekey="BoundFieldResource2"></asp:BoundField>
                                        <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2"></asp:BoundField>                                        
                                        <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                            <ItemTemplate>
                                                <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" meta:resourcekey="imgObjectiveResource1" />
                                                <asp:Image ID="imgPriority" runat="server" AlternateText="Obj" meta:resourcekey="imgPriorityResource1" />
                                                <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                                <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden">
                                            <HeaderStyle CssClass="hidden"></HeaderStyle>

                                            <ItemStyle CssClass="hidden"></ItemStyle>
                                        </asp:BoundField>

                                        <asp:TemplateField HeaderStyle-Width="150" meta:resourcekey="TemplateFieldResource2">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblGridHeaderActivity" runat="server" Text="Activity" meta:resourcekey="lblGridHeaderActivityResource1"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridActivity" runat="server" Text='<%# Eval("ActivityName") %>'
                                                    meta:resourcekey="lblGridActivityResource1"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150" meta:resourcekey="TemplateFieldResource3">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblGridHeaderIndicator" runat="server" Text="Output Indicator" meta:resourcekey="lblGridHeaderIndicatorResource1"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridIndicator" runat="server" Text='<%# Eval("DataName") %>' meta:resourcekey="lblGridIndicatorResource1"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="150px"></HeaderStyle>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="50">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblUnitHeader" runat="server" Text="Unit"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" TargetControlID="btnClientOpen"
                    BehaviorID="mpeAddActivity" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
                    DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="200px" meta:resourcekey="pnlLocationsResource1">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class=" width-100 modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header no-padding">
                                            <div class="table-header">
                                                Select Locations
                                            </div>
                                        </div>
                                        <div class="modal-body no-padding">
                                            <table border="0" style="margin: 0 auto;">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBoxList ID="cbAdmin1Locaitons" runat="server" meta:resourcekey="cbAdmin1LocaitonsResource1" css="cbltest">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-primary"
                                                            Width="120px" CausesValidation="False" OnClientClick="needToConfirm = false;"
                                                            meta:resourcekey="btnCloseResource1" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="space"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>

</asp:Content>
