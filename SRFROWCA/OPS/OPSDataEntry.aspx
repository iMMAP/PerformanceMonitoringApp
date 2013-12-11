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

        });

        // Filter rows on objects
        function showHideRowsOnObjs() {

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
                $(".imagetable").prepend('<colgroup><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th class="testhide" style="width: 40px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 60px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" border="1">
    <div id="divMsg">
    </div>
    <div class="buttonsdiv">
        <div class="buttonright">
            <asp:LinkButton ID="lnkLanguageEnglish" Text="English" runat="server" OnClientClick="needToConfirm=false;"
                OnClick="lnkLanguageEnglish_Click" CausesValidation="False" meta:resourcekey="lnkLanguageEnglishResource1"></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="lnkLanguageFrench" Text="Français" runat="server" OnClientClick="needToConfirm=false;"
                OnClick="lnkLanguageFrench_Click" CausesValidation="False" meta:resourcekey="lnkLanguageFrenchResource1"></asp:LinkButton>
        </div>
        <div class="spacerops" style="clear: both;">
        </div>
        <div class="savebutton">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="False" Width="120px" CssClass="button_example" meta:resourcekey="btnSaveResource1" />
            <asp:Localize ID="locbtnCloseWindow" runat="server" meta:resourcekey="locbtnCloseWindowResource1"
                Text="&lt;input type=&quot;button&quot; class=&quot;button_example&quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"></asp:Localize>
        </div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                meta:resourcekey="btnOpenLocationsResource1" />
        </div>
        <div class="spacerops" style="clear: both;">
        </div>
        <div class="savebutton">
            <clusterlabel>
                                <b><asp:Localize ID="locClusterCaption" runat="server" 
                                meta:resourcekey="locClusterCaptionResource1" Text="Cluster:"></asp:Localize></b></clusterlabel>
            <asp:Label ID="lblCluster" runat="server" CssClass="clusterLabel" meta:resourcekey="lblClusterResource1"></asp:Label>
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
                    <asp:BoundField DataField="SecondaryCluster" HeaderText="Secondary Cluster" ItemStyle-Width="40px">
                    </asp:BoundField>
                    <asp:BoundField DataField="Objective" HeaderText="Objective" ItemStyle-Width="150px">
                    </asp:BoundField>
                    <asp:BoundField DataField="HumanitarianPriority" HeaderText="Priority" ItemStyle-Width="60px"
                        ItemStyle-Wrap=" false"></asp:BoundField>
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="150px"
                        meta:resourcekey="BoundFieldResource4">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-Width="150px"
                        meta:resourcekey="BoundFieldResource5">
                        <ItemStyle Width="200px"></ItemStyle>
                    </asp:BoundField>
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
                <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" BehaviorID="mpeAddActivity"
                    TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
                    DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="200px" meta:resourcekey="pnlLocationsResource1">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    <asp:Localize ID="locLocaitonLevelCaption" runat="server" meta:resourcekey="locLocaitonLevelCaptionResource1"
                                        Text="Admin1 Locations"></asp:Localize>
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
    <table>
        <tr>
            <td>
                <input type="button" id="btnAddActivityOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddOrg" runat="server" TargetControlID="btnAddActivityOpen"
                    PopupControlID="pnlOrg" BackgroundCssClass="modalpopupbackground" DynamicServicePath=""
                    Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlOrg" runat="server" Width="900px" meta:resourcekey="pnlOrgResource1">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <clusterlabel>
                                <b><asp:Localize ID="locUserActStrObj" runat="server" 
                                meta:resourcekey="Localize1Resource1" Text="Strategic Objectives:"></asp:Localize></b></clusterlabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserStrObj" Width="408px" runat="server" meta:resourcekey="ddlStrObjectivesResource1">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlUserStrObj" runat="server" ErrorMessage="Required"
                                                        InitialValue="0" ForeColor="Red" Text="Required" ControlToValidate="ddlUserStrObj"
                                                        meta:resourcekey="rfvddlUserStrObjResource1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <clusterlabel>

                                <b><asp:Localize ID="locUserActPriority" runat="server" meta:resourcekey="Localize2Resource1" Text="Prioirties:"></asp:Localize></b></clusterlabel>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserPriority" Width="408px" runat="server" meta:resourcekey="ddlSpcObjectivesResource1">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvddlUserPriority" runat="server" ErrorMessage="Required"
                                                        InitialValue="0" ForeColor="Red" Text="Required" ControlToValidate="ddlUserPriority"
                                                        meta:resourcekey="rfvddlUserPriorityResource1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <asp:Localize ID="locUserActivityCaption" runat="server" meta:resourcekey="locUserActivityCaptionResource1"
                                                            Text="Activity"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserActivity" runat="server" Width="400px" meta:resourcekey="txtUserActivityResource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvUserActivity" runat="server" ErrorMessage="Required"
                                                        ForeColor="Red" Text="Required" ControlToValidate="txtUserActivity" meta:resourcekey="rfvUserActivityResource1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <asp:Localize ID="locUserOutputInd1Caption" runat="server" meta:resourcekey="locUserOutputInd1CaptionResource1"
                                                            Text="Output Indicator 1"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserOutputIndicator1" runat="server" Width="400px" meta:resourcekey="txtUserOutputIndicator1Resource1"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvUserOutputIndicator1" runat="server" ErrorMessage="Required"
                                                        ForeColor="Red" Text="Required" ControlToValidate="txtUserOutputIndicator1" meta:resourcekey="rfvUserOutputIndicator1Resource1"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <asp:Localize ID="Localize4locUserOutputInd2Caption" runat="server" meta:resourcekey="Localize4locUserOutputInd2CaptionResource1"
                                                            Text="Output Indicator 2"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserOutputIndicator2" runat="server" Width="400px" meta:resourcekey="txtUserOutputIndicator2Resource1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <asp:Localize ID="locUserOutputInd3Caption" runat="server" meta:resourcekey="locUserOutputInd3CaptionResource1"
                                                            Text="Output Indicator 3"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserOutputIndicator3" runat="server" Width="400px" meta:resourcekey="txtUserOutputIndicator3Resource1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <b>
                                                        <asp:Localize ID="locUserOutputInd4Caption" runat="server" meta:resourcekey="locUserOutputInd4CaptionResource1"
                                                            Text="Output Indicator 4"></asp:Localize>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserOutputIndicator4" runat="server" Width="400px" meta:resourcekey="txtUserOutputIndicator4Resource1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="right">
                                                    <asp:HiddenField ID="hfLocEmgId" runat="server" />
                                                    <asp:Button ID="btnSaveUserActivity" runat="server" Text="Save" OnClick="btnSaveUserActivity_Click"
                                                        CssClass="button_example" OnClientClick="needToConfirm = false;" meta:resourcekey="btnSaveUserActivityResource1" />
                                                    <asp:Button ID="btnCloseUserActivity" runat="server" Text="Close" CausesValidation="False"
                                                        OnClick="btnCloseUserActivity_Click" OnClientClick="needToConfirm = false;" CssClass="button_example"
                                                        meta:resourcekey="btnCloseUserActivityResource1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="False"
                                                        ViewStateMode="Disabled" meta:resourcekey="lblMessage2Resource1"></asp:Label>
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
                            <asp:PostBackTrigger ControlID="btnSaveUserActivity" />
                            <asp:PostBackTrigger ControlID="btnCloseUserActivity" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
