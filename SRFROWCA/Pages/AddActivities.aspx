<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_cblLocations td
        {
            padding: 0 40px 0 0;
        }
    </style>
    <script src="../Scripts/jquery.numeric.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var needToConfirm = true;

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            var ctl = document.getElementById('__EVENTTARGET').value;

            if (ctl.indexOf("LanguageEnglish") != -1 || ctl.indexOf("LanguageFrench") != -1) {
                __EVENTTARGET.value = '';
                needToConfirm = false;
            }
            if (needToConfirm)
                return "";
        }

        var launch = false;
        function launchModal() {
            launch = true;
        }
        function pageLoad() {
            if (launch) {
                $find("mpeAddActivity").show();
            }
        }

        $(function () {
            $("#format").buttonset();
            $(".numeric1").numeric();
            $('#gridSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });

            showHideObj();
            showHidePriority();

            if (!(/chrom(e|ium)/.test(navigator.userAgent.toLowerCase()))) {
                var list = '';
                var list2 = '';
                var j = 0;
                $(".imagetable th").each(function () {
                    var value = ($(":first-child", this).is(":input"))
                ? $(":first-child", this).val()
                : ($(this).text() != "")
                  ? $(this).text()
                  : $(this).html();
                    if (value.indexOf('_') >= 0) {
                        j++;
                        city1 = value.split('_');
                        $(this).text(city1[1]);

                        if (j % 3 === 0) {
                            list += '<th colspan="3" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                $(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th style="width: 100px;">&nbsp;</th><th style="width: 30px;">&nbsp;</th><th style="width: 450px;">&nbsp;</th><th style="width: 350px;">&nbsp;</th>' + list + '</tr></thead>');
            }

            $("#<%=gvActivities.ClientID %>").kiketable_colsizable({ minWidth: 50 })
        });

        $(".checkbox").change(function () {

        });

        function showHideObj() {
            $(".checkObj").live("click", function () {
                var selectedObjs = [];
                $("[id*=cblObjectives] input:checked").each(function () {
                    selectedObjs.push($(this).val());
                });

                var selectedPr = [];
                $("[id*=cblPriorities] input:checked").each(function () {
                    selectedPr.push($(this).val());
                });

                showObj();
                if (selectedObjs.length > 0) {
                    var i;
                    for (i = 0; i < selectedObjs.length; ++i) {
                        hideObj(selectedObjs[i]);
                    }
                }

                if (selectedPr.length > 0) {
                    var i;
                    for (i = 0; i < selectedPr.length; ++i) {
                        hidePriority(selectedPr[i]);
                    }
                }
            });
        }

        function showHidePriority() {

            $(".checkPr").live("click", function () {
                var selectedPr = [];
                $("[id*=cblPriorities] input:checked").each(function () {
                    selectedPr.push($(this).val());
                });

                var selectedObjs = [];
                $("[id*=cblObjectives] input:checked").each(function () {
                    selectedObjs.push($(this).val());
                });

                showPriority();
                if (selectedPr.length > 0) {
                    var i;
                    for (i = 0; i < selectedPr.length; ++i) {
                        hidePriority(selectedPr[i]);
                    }
                }

                if (selectedObjs.length > 0) {
                    var i;
                    for (i = 0; i < selectedObjs.length; ++i) {
                        hideObj(selectedObjs[i]);
                    }
                }
            });
        }

        function showObj() {

            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                $(this).parent().show();
            });
        }

        function hideObj(objId) {

            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                if ($(this).text() === objId || objId === '0') {
                    $(this).parent().hide();
                }
            });
        }

        function showPriority() {
            $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
                $(this).parent().show();
            });
        }

        function hidePriority(priorityId) {
            $('.istrow, .altcolor').find('td:nth-child(2)').each(function (i) {
                if ($(this).text() === priorityId || priorityId === '0') {
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

        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
    </div>
    <div class="containerDataEntryMain">
        <table border="0" cellpadding="0" width="100%">
            <tr>
                <td align="right" style="display: none">
                    <label>
                        <asp:Localize ID="locaEmergencyCaption" runat="server" meta:resourcekey="locaEmergencyCaptionResource1"
                            Text="
                                Emergency:"></asp:Localize>
                        (<asp:Label ID="lblCountry" runat="server" meta:resourcekey="lblCountryResource1"></asp:Label>)
                    </label>
                </td>
            </tr>
        </table>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div style="display: none">
        <div class="buttonsdiv">
            <div class="savebutton">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                    Width="120px" CssClass="button_example" meta:resourcekey="btnSaveResource1" /></div>
            <div class="buttonright">
            </div>
            <div class="spacer" style="clear: both;">
            </div>
        </div>
    </div>
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                <table>
                    <tr>
                        <td colspan="2" style="display: none">
                            <asp:DropDownList ID="ddlEmergency" runat="server" Width="350px" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlEmergencyResource1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                InitialValue="0" Text="*" ControlToValidate="ddlEmergency" meta:resourcekey="rfvEmergencyResource1"></asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <label>
                                <asp:Localize ID="locaYearMonth" runat="server" meta:resourcekey="locaYearMonthResource1"
                                    Text="
                                Year/Month:"></asp:Localize></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlYearResource1">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server" Width="90px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlMonthResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                                meta:resourcekey="btnOpenLocationsResource1" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="containerDataEntryProjectsInner">
                <asp:CheckBoxList ID="cblProjects" runat="server">
                </asp:CheckBoxList>
                <br />
                <br />
            </div>
            <div class="containerDataEntryProjectsInner">
                <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj">
                </asp:CheckBoxList>
                <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr">
                </asp:CheckBoxList>
                <asp:Button ID="Button1" runat="server" Text="Manage Projects" CssClass="button_example" />
                <br />
                <br />
                <br />
                <asp:Button ID="btnTest2" runat="server" Text="Manage Activities" CssClass="button_example" />
                <br />
                <br />
                <br />
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div class="tablegrid">
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%; height: 530px;">
                    <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                        Width="100%" Height="530px" meta:resourcekey="gvActivitiesResource1" OnRowDataBound="gvActivities_RowDataBound">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px" />
                            <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                ItemStyle-Width="1px" HeaderStyle-CssClass="hiddenelement" />
                            <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px" />
                            <asp:TemplateField HeaderText="Project Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblProjectcode" runat="server" Text='<%#Eval("ClusterName") %>' ToolTip='<%#Eval("ProjectTitle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Image ID="imgObjective" runat="server" ImageUrl="~/images/O.png" AlternateText="Obj" />
                                    <asp:Image ID="imgPriority" runat="server" ImageUrl="~/images/P.png" AlternateText="Obj" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="450px"
                                meta:resourcekey="BoundFieldResource4">
                                <ItemStyle Width="450px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-Width="450px"
                                meta:resourcekey="BoundFieldResource5">
                                <ItemStyle Width="450px"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
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
                <asp:Panel ID="pnlLocations" runat="server" Width="700px" meta:resourcekey="pnlLocationsResource1">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    <asp:Localize ID="locaAdmin2LocationsCaption" runat="server" meta:resourcekey="locaAdmin2LocationsCaptionResource1"
                                        Text="
                                    Admin2 Locations"></asp:Localize>
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                                        CssClass="columnGap" meta:resourcekey="cblLocationsResource1">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="120px" CssClass="button_location"
                                                        CausesValidation="False" OnClientClick="needToConfirm = false;" meta:resourcekey="btnCloseResource1" />
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
