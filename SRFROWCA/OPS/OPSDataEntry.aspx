<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

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

    </script>
    <script type="text/javascript">
        var launch = false;
        function launchModal() {
            launch = true;
        }
        function pageLoad() {
            if (launch) {
                $find("mpeAddActivity").show();
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
            
        });

        // Filter rows on objects
        function showHideRowsOnObjs() {

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
            <asp:Localize ID="locFilterContainerHeader" runat="server" meta:resourcekey="locFilterContainerHeaderResource1"
                Text="Filter Activities "></asp:Localize>
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="margin: 0 auto; width: 100%">
                    <tr>
                        <td>
                            <clusterlabel>
                                <b><asp:Localize ID="locClusterCaption" runat="server" 
                                meta:resourcekey="locClusterCaptionResource1" Text=" Cluster:"></asp:Localize></b></clusterlabel>
                        </td>
                        <td>
                            <asp:Label ID="lblCluster" runat="server" CssClass="clusterLabel" meta:resourcekey="lblClusterResource1"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <clusterlabel>
                                <b><asp:Localize ID="locObjCaption" runat="server" 
                                meta:resourcekey="locObjCaptionResource1" Text=" Strategic Objectives:"></asp:Localize></b></clusterlabel>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSpcObjectives" Width="950px" runat="server" meta:resourcekey="ddlSpcObjectivesResource1">
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
                Width="120px" CssClass="button_example" meta:resourcekey="btnSaveResource1" />
            <asp:Localize ID="locbtnCloseWindow" runat="server" 
                meta:resourcekey="locbtnCloseWindowResource1" 
                Text="&lt;input type=&quot;button&quot; class=&quot;button_example&quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"></asp:Localize> 
        </div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                meta:resourcekey="btnOpenLocationsResource1" />
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
                    <asp:BoundField DataField="StrObjName" HeaderText="strobj" ItemStyle-Wrap=" false"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource1">
                        <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                        <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="SpcObjName" HeaderText="spcobj" ItemStyle-Wrap=" false"
                        ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource2">
                        <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                        <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="IndicatorName" HeaderText="Priority" ItemStyle-Width="200px"
                        ItemStyle-CssClass="testhide" HeaderStyle-CssClass="testhide" meta:resourcekey="BoundFieldResource3">
                        <HeaderStyle CssClass="testhide"></HeaderStyle>
                        <ItemStyle CssClass="testhide" Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="200px"
                        meta:resourcekey="BoundFieldResource4">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataName" HeaderText="Data" ItemStyle-Width="200px"
                        meta:resourcekey="BoundFieldResource5">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                Width="120px" CssClass="button_example" meta:resourcekey="btnSave2Resource1" />
        </div>
        <div class="buttonright">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" BehaviorID="mpeAddActivity" runat="server" TargetControlID="btnClientOpen"
                    PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground" DynamicServicePath=""
                    Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="200px" meta:resourcekey="pnlLocationsResource1">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    <asp:Localize ID="locLocaitonLevelCaption" runat="server" meta:resourcekey="locLocaitonLevelCaptionResource1"
                                        Text=" Admin1 Locations"></asp:Localize>
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cbAdmin1Locaitons" runat="server" meta:resourceKey="cbAdmin1LocaitonsResource1">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button_location"
                                                        Width="120px" CausesValidation="False" OnClientClick="needToConfirm = false;"
                                                        meta:resourceKey="btnCloseResource1" />
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
