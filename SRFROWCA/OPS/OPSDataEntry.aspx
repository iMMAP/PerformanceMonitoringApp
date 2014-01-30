<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery.numeric.min.js" type="text/javascript"></script>
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
            allowOnlyNumeric();

            // Split location name from Target(T) and Achieved(A).
            splitLocationFromTA();

            // Filter rows on strategic objects or specific objects.
            showHideRowsOnObjs();

            // Change coloumn size
            $("#<%=gvActivities.ClientID %>").kiketable_colsizable({ minWidth: 30 })

            $("#<%=ddlStrObjectives.ClientID %> > option").first().attr('selected', 'selected');
            $("#<%=ddlPriorities.ClientID %> > option").first().attr('selected', 'selected');
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

        // Filter rows on objects
        function showHideRowsOnObjs() {
            // Filter on strategic objectives
            $('#<%=ddlStrObjectives.ClientID %>').change(function () {
                var objId = $('#<%=ddlStrObjectives.ClientID %> :selected').val();

                var priorityId = $('#<%=ddlPriorities.ClientID %> :selected').val();

                var objPrId = '';
                if (objId !== '0' && priorityId !== '0') {
                    objPrId = objId + '-' + priorityId;
                    hideObjPriority(objPrId);
                }
                else if (objId === '0' && priorityId !== '0') {
                    hidePriority(priorityId);
                }
                else {
                    hideObj(objId);
                }

                $('#<%=gvActivities.ClientID %>').each(function () {
                    $('tr:odd', this).addClass('istrow').removeClass('altcolor');
                    $('tr:even', this).addClass('altcolor').removeClass('istrow');
                });

            });

            // Filter (hide) rows from spcobj grid
            $('#<%=ddlPriorities.ClientID %>').change(function () {
                //$("#<%=ddlStrObjectives.ClientID %> > option").first().attr('selected', 'selected');
                var objId = $('#<%=ddlStrObjectives.ClientID %> :selected').val();
                var priorityId = $('#<%=ddlPriorities.ClientID %> :selected').val();

                var objPrId = '';
                if (objId !== '0' && priorityId !== '0') {
                    objPrId = objId + '-' + priorityId;
                    hideObjPriority(objPrId);
                }
                else if (objId !== '0' && priorityId === '0') {
                    hideObj(objId);
                }
                else {
                    hidePriority(priorityId);
                }
            });

            $('#<%=gvActivities.ClientID %>').each(function () {
                $('tr:odd', this).addClass('istrow').removeClass('altcolor');
                $('tr:even', this).addClass('altcolor').removeClass('istrow');
            });
        }

        function hideObj(objId) {

            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                if ($(this).text() === objId || objId === '0') {
                    $(this).parent().show();
                }
                else {
                    $(this).parent().hide();
                }
            });
        }

        function hidePriority(priorityId) {
            $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
                if ($(this).text() === priorityId || priorityId === '0') {
                    $(this).parent().show();
                }
                else {
                    $(this).parent().hide();
                }
            });
        }

        function hideObjPriority(objPrId) {
            $('.istrow, .altcolor').find('td:nth-child(3)').each(function (i) {
                if ($(this).text() === objPrId || objPrId === '0') {
                    $(this).parent().show();
                }
                else {
                    $(this).parent().hide();
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
                        if (j % 2 === 0) {
                            list += '<th colspan="2" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                // Add header row in grid.
                $(".imagetable").prepend('<colgroup><col /><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th style="width: 40px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 120px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" border="1">
    <div id="divMsg">
    </div>
    <div class="containerOPS">
        <div class="graybar">
            <asp:Localize ID="locFilterContainerHeader" runat="server" Text=" Filter Activities"
                meta:resourcekey="locFilterContainerHeaderResource1"></asp:Localize>
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="margin: 0 auto; width: 100%">
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>
                            <asp:Localize ID="locClusterCaption" runat="server" 
                                 Text="Cluster:" meta:resourcekey="locClusterCaptionResource1" ></asp:Localize></b></clusterlabel>
                        </td>
                        <td>
                            <asp:Label ID="lblCluster" runat="server" CssClass="clusterLabel" meta:resourcekey="lblClusterResource1"></asp:Label>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkLanguageEnglish" Text="English" runat="server" OnClientClick="needToConfirm=false;"
                                CausesValidation="False" OnClick="lnkLanguageEnglish_Click" meta:resourcekey="lnkLanguageEnglishResource1"></asp:LinkButton>&nbsp;&nbsp;
                            <asp:LinkButton ID="lnkLanguageFrench" Text="Français" runat="server" OnClientClick="needToConfirm=false;"
                                CausesValidation="False" OnClick="lnkLanguageFrench_Click" meta:resourcekey="lnkLanguageFrenchResource1"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>
                            <asp:Localize ID="Localize2" runat="server"
                                 Text="Strategic Objectives:" meta:resourcekey="Localize2Resource1" ></asp:Localize></b></clusterlabel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStrObjectives" Width="800px" runat="server" meta:resourcekey="ddlStrObjectivesResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>                    
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>
                            <asp:Localize ID="Localize3" runat="server"
                                Text="Prioirties:" meta:resourcekey="Localize3Resource1" ></asp:Localize></b></clusterlabel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriorities" Width="800px" runat="server" meta:resourcekey="ddlPrioritiesResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer" style="clear: both;">
            </div>
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="False" Width="120px" CssClass="button_example" meta:resourcekey="btnSaveResource1" />
            <asp:Localize ID="locbtnCloseWindow" runat="server" Text="&lt;input type=&quot;button&quot; class=&quot;button_example&quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"
                meta:resourcekey="locbtnCloseWindowResource1"></asp:Localize>
        </div>
        <div>
            <asp:Localize ID="locaNoTargetMessage" runat="server" Text="
                &lt;div style=&quot;color:Red;&quot;&gt;To select an activity for  for which you do not know the target, please put a zero (0).&lt;/div&gt;
            " meta:resourcekey="locaNoTargetMessageResource1"></asp:Localize>
        </div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                meta:resourcekey="btnOpenLocationsResource1" />
        </div>
        <div class="savebutton">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div class="tablegrid">
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                Width="100%" OnRowDataBound="gvActivities_RowDataBound" meta:resourcekey="gvActivitiesResource1">
                <HeaderStyle BackColor="Control"></HeaderStyle>
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>                    
                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource1">
                        <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                        <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                        ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement"
                        meta:resourcekey="BoundFieldResource2">
                        <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                        <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource2">
                        <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                        <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                    </asp:BoundField>                   
                    <asp:BoundField DataField="SecondaryCluster" HeaderText="Cluster Partner" ItemStyle-Width="40px"
                        meta:resourcekey="BoundFieldResource3">
                        <ItemStyle Width="40px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Objective" HeaderText="Objective" ItemStyle-Width="150px"
                        meta:resourcekey="BoundFieldResource4">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="120" meta:resourcekey="TemplateFieldResource1">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderPriority" runat="server" Text="Priority" meta:resourcekey="lblGridHeaderPriorityResource1"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridPrioritiy" runat="server" Text='<%# Eval("HumanitarianPriority") %>'
                                meta:resourcekey="lblGridPrioritiyResource1"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="120px"></HeaderStyle>
                    </asp:TemplateField>
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
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" CausesValidation="False"
                OnClientClick="needToConfirm = false;" Width="120px" CssClass="button_example"
                meta:resourcekey="btnSave2Resource1" />
        </div>
        <div class="spacer" style="clear: both;">
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
                            <div class="containerPopup">
                                <div class="graybar">
                                    <asp:Localize ID="locLocaitonLevelCaption" runat="server" Text="Admin1 Locations"
                                        meta:resourcekey="locLocaitonLevelCaptionResource1"></asp:Localize>
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cbAdmin1Locaitons" runat="server" meta:resourcekey="cbAdmin1LocaitonsResource1">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button_location"
                                                        Width="120px" CausesValidation="False" OnClientClick="needToConfirm = false;"
                                                        meta:resourcekey="btnCloseResource1" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="spacer" style="clear: both;">
                                        </div>
                                    </div>
                                </div>
                                <div class="graybarcontainer">
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
