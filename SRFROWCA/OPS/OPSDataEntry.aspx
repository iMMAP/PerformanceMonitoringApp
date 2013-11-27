<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" %>

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
            $(".numeric1").numeric();

            // Split location name from Target(T) and Achieved(A).
            splitLocationFromTA();

            // Filter rows on strategic objects or specific objects.
            showHideRowsOnObjs();

            // Change coloumn size
            $("#<%=gvActivities.ClientID %>").kiketable_colsizable({ minWidth: 30 })

            if ($('#<%=chkShowHideIndicator.ClientID%>').is(":checked")) {
                $('.testhide').show();
                $('#lblShowHideIndicator').text('Hide Indicator/Indicateur');
            }
            else {
                $('.testhide').hide();
                $('#lblShowHideIndicator').text('Show Indicator/Indicateur');
            }

            $('#<%=chkShowHideIndicator.ClientID%>').click(function () {
                if ($('#<%=chkShowHideIndicator.ClientID%>').is(":checked")) {
                    $('.testhide').show();
                    $('#lblShowHideIndicator').text('Hide Indicator/Indicateur');
                }
                else {
                    $('.testhide').hide();
                    $('#lblShowHideIndicator').text('Show Indicator/Indicateur');
                }
            });
        });

        // Filter rows on objects
        function showHideRowsOnObjs() {
            // Filter on strategic objectives
            $('#<%=ddlStrObjectives.ClientID %>').change(function () {
                var objId = $('#<%=ddlStrObjectives.ClientID %> :selected').val();

                //Get all items from spcobjectives dropdown
                var spcObjOptions = $("#<%=ddlSpcObjectives.ClientID %> > option");

                // remove hidden class from items to show all.
                showAllDropDownItems(spcObjOptions);

                // Hide matching items from spc dropdown.
                hideMatchingItemsInDropDown(spcObjOptions, objId)

                // filter (hide) rows from strobj grid.
                $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                    if ($(this).text() === objId || objId === '0') {
                        $(this).parent().show();
                    }
                    else {
                        $(this).parent().hide();
                    }
                });
            });

            // Filter (hide) rows from spcobj grid
            $('#<%=ddlSpcObjectives.ClientID %>').change(function () {
                var strSpcObjId = $('#<%=ddlSpcObjectives.ClientID %> :selected').val();
                var spcObjId = strSpcObjId.substring(strSpcObjId.indexOf('_') + 1, strSpcObjId.length);

                $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
                    if ($(this).text() === spcObjId || spcObjId === '0') {
                        $(this).parent().show();
                    }
                    else {
                        $(this).parent().hide();
                    }
                });
            });
        }

        // We alos need to filter specific objectives on selected str objective.
        // First remove 'hiddenelement' class which is to hide items in spc objective
        // dropdown i.e. display none                
        function showAllDropDownItems(controlItems) {
            $(controlItems).each(function () {
                $(this).removeClass('hiddenelement');
            });

            // Selecte first item in spc objectives.
            $(controlItems).first().attr('selected', 'selected');
        }

        function hideMatchingItemsInDropDown(controlItems, objId) {
            $(controlItems).each(function () {
                var strSpcObjId = $(this).val();
                if (strSpcObjId === "0" || objId === "0") {
                    $(this).removeClass('hiddenelement');
                }
                else {
                    var spcObjId = strSpcObjId.substring(strSpcObjId.indexOf('_') + 1, strSpcObjId.length);
                    var strObjId = strSpcObjId.substring(0, strSpcObjId.indexOf('_'));

                    if (strObjId !== objId) {
                        $(this).addClass('hiddenelement');
                    }
                    else {
                        $(this).removeClass('hiddenelement');
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
                        if (j % 2 === 0) {
                            list += '<th colspan="2" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                // Add header row in grid.
                $(".imagetable").prepend('<colgroup><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th class="testhide" style="width: 200px;">&nbsp;</th><th style="width: 200px;">&nbsp;</th><th style="width: 200px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
    </div>
    <div class="containerOPS">
        <div class="graybar">
            Filter Activities
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="margin: 0 auto; width: 100%">
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>Cluster:</b></clusterlabel>
                        </td>
                        <td>
                            <asp:Label ID="lblCluster" runat="server" CssClass="clusterLabel"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>Strategic Objectives/Objectif Strategique:</b></clusterlabel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlStrObjectives" Width="950px" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <clusterlabel>
                                <b>Cluster Objectives/Objectif:</b></clusterlabel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSpcObjectives" Width="950px" runat="server">
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
                CausesValidation="true" Width="120" CssClass="button_example" />
            <input type="button" class="button_example" value="Close Window" id="close" onclick="window.close()" />
        </div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="false"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
        <asp:CheckBox ID="chkShowHideIndicator" class="tempClassIndicator" runat="server"
            Text="" />
        <%--<input id="chkShowHideIndicator" type="checkbox" />--%>
        <label id="lblShowHideIndicator">
            Hide Indicator/Indicateur
        </label>
    </div>
    <div class="tablegrid">
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AllowPaging="False" AllowSorting="False"
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                DataKeyNames="ActivityDataId" CssClass="imagetable" Width="100%" OnRowDataBound="gvActivities_RowDataBound">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:BoundField DataField="StrObjName" HeaderText="strobj" ItemStyle-Wrap=" false"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" />
                    <asp:BoundField DataField="SpcObjName" HeaderText="spcobj" ItemStyle-Wrap=" false"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" />
                    <asp:BoundField DataField="IndicatorName" HeaderText="Indicator/Indicateur" ItemStyle-Width="200px"
                        ItemStyle-CssClass="testhide" HeaderStyle-CssClass="testhide" />
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity/Activité" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="DataName" HeaderText="Data/Donnée" ItemStyle-Width="200px" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="true" Width="120px" CssClass="button_example" />
            <asp:Button ID="btnUserActivity" runat="server" OnClick="btnUserActivity_Click" Text="Add User Activities"
                OnClientClick="needToConfirm = false;" CausesValidation="true" Width="150px"
                CssClass="button_location" /></div>
        <div class="buttonright">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" BehaviorID="mpeAddActivity" runat="server"
                    TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="200px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    Admin1 Locations
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cbAdmin1Locaitons" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button_location"
                                                        Width="120px" CausesValidation="false" OnClientClick="needToConfirm = false;" />
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
    <table>
        <tr>
            <td>
                <input type="button" id="btnUserActivities" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeUserActivity" BehaviorID="mpeUserActivity" runat="server"
                    TargetControlID="btnUserActivities" PopupControlID="pnlUserActivity" BackgroundCssClass="modalpopupbackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlUserActivity" runat="server" Width="70%">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    Admin1 Locations
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    Cluster:
                                                </td>
                                                <td>
                                                    Education
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Strategic Objective:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserStrObj" runat="server" Width="90%" OnSelectedIndexChanged="ddlUserStrObj_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Specific Objective:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserSpcObj" runat="server" Width="90%" OnSelectedIndexChanged="ddlUserSpcObj_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Indicator:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserIndicator" runat="server" Width="45%" OnSelectedIndexChanged="ddlUserIndicator_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtUserIndicator" runat="server" Width="45%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Activity:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserActivitiy" runat="server" Width="45%" OnSelectedIndexChanged="ddlUserActivity_SelectedIndexChanged"
                                                        AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtUserActivity" runat="server" Width="45%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Data:
                                                </td>
                                                <td>
                                                    <%--<asp:DropDownList ID="ddlUserData" runat="server" Width="35%">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtUserData" runat="server" Width="90%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnCloseUserActivities" runat="server" Text="Close" CssClass="button_location"
                                                        Width="120px" CausesValidation="false" OnClientClick="needToConfirm = false;" />
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
                            <asp:PostBackTrigger ControlID="btnCloseUserActivities" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
