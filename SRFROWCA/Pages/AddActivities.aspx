<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_cblLocations td
        {
            padding: 0 40px 0 0;
        }
    </style>
    <script type="text/javascript" src="../Scripts/ShowHideObJAndPr.js"></script>
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
                        city2 = city1[1].split('-');
                        $(this).text(city2[1]);
                        if (j % 3 === 0) {
                            list += '<th colspan="3" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                $(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th style="width: 100px;">&nbsp;</th><th style="width: 50px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 220px;">&nbsp;</th>' + list + '</tr></thead>');
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
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
                        <td>
                            <label>
                                <asp:Localize ID="lzeYearMonth" runat="server" Text="
                                Year/Month:" meta:resourcekey="lzeYearMonthResource1"></asp:Localize></label>
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
                </table>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>
                        <asp:Localize ID="lzeLgndProjects" runat="server" meta:resourcekey="lzeLgndProjectsResource1"
                            Text="Projects"></asp:Localize></legend>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                        onchange="needToConfirm = false;" meta:resourcekey="rblProjectsResource1">
                    </asp:RadioButtonList>
                    <br />
                    <br />
                </fieldset>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>
                        <asp:Localize ID="lzeLgndStrObjs" runat="server" meta:resourcekey="lzeLgndStrObjsResource1"
                            Text="Strategic Objectives"></asp:Localize></legend>
                    <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj" meta:resourcekey="cblObjectivesResource1">
                    </asp:CheckBoxList>
                </fieldset>
                <fieldset>
                    <legend>
                        <asp:Localize ID="lzeLgndHumPriorities" runat="server" meta:resourcekey="lzeLgndHumPrioritiesResource1"
                            Text="Humanitarian Priorities"></asp:Localize></legend>
                    <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr" meta:resourcekey="cblPrioritiesResource1">
                    </asp:CheckBoxList>
                </fieldset>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <b>
                        <asp:Localize ID="lzeLgndManageProjects" runat="server" meta:resourcekey="lzeLgndManageProjectsResource1"
                            Text="&lt;a href=&quot;/Pages/CreateProject.aspx&quot;&gt;Manage Projects&lt;/a&gt;"></asp:Localize></b>
                    <br />
                    <br />
                    <b>
                        <asp:Localize ID="lzeLgndManageActivities" runat="server" meta:resourcekey="lzeLgndManageActivitiesResource1"
                            Text="&lt;a href=&quot;/Pages/ManageActivities.aspx&quot;&gt;Manage Activities&lt;/a&gt;"></asp:Localize></b>
                    <br />
                </fieldset>
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div class="buttonsdiv">
                <div class="savebutton2">
                    <asp:Localize ID="lzeSelectLocaitonsText" runat="server" meta:resourcekey="lzeSelectLocaitonsTextResource1"
                        Text="
                    Please click on 'Locations' button to select the locations you want to report on."></asp:Localize>
                    <asp:Button ID="btnOpenLocations" runat="server" Text="Manage Locations" CausesValidation="False"
                        CssClass="button_example" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                        meta:resourcekey="btnOpenLocationsResource1" />
                </div>
                <div class="buttonright2">
                    <asp:Localize ID="lzeExportToText" runat="server" meta:resourcekey="lzeExportToTextResource1"
                        Text="Export To:"></asp:Localize>
                    <asp:ImageButton ID="btnPDF" runat="server" ImageUrl="~/images/pdf.png" OnClick="btnPDF_Export"
                        OnClientClick="needToConfirm = false;" CssClass="imgButtonImg" meta:resourcekey="btnPDFResource1" />
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/images/excel.png" OnClick="btnExcel_Export"
                        OnClientClick="needToConfirm = false;" CssClass="imgButtonImg" meta:resourcekey="btnExcelResource1" />
                </div>
                <div class="savebutton">
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                    <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                        DataKeyNames="ActivityDataId" CssClass="imagetable" Width="100%" meta:resourcekey="gvActivitiesResource1"
                        OnRowDataBound="gvActivities_RowDataBound">
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
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource3">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProjectId" HeaderText="pid" ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement"
                                HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource4">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="objAndPrAndPId" HeaderText="objprpid" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource5">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="objAndPId" HeaderText="objAndPId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource6">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PrAndPId" HeaderText="PrAndPId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement" meta:resourcekey="BoundFieldResource7">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Project Code" ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:Label ID="lblProjectcode" runat="server" Text='<%# Eval("ClusterName") %>' ToolTip='<%# Eval("ProjectTitle") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                <ItemTemplate>
                                    <asp:Image ID="imgObjective" runat="server" ImageUrl="~/images/O.png" AlternateText="Obj"
                                        meta:resourcekey="imgObjectiveResource1" />
                                    <asp:Image ID="imgPriority" runat="server" ImageUrl="~/images/P.png" AlternateText="Obj"
                                        meta:resourcekey="imgPriorityResource1" />
                                    <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1" />
                                    <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1" />
                                </ItemTemplate>
                                <ItemStyle Wrap="False"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="260px" HeaderText="Activity" meta:resourcekey="TemplateFieldResource3">
                                <ItemTemplate>
                                    <div style="width: 260px; word-wrap: break-word;">
                                        <%# Eval("ActivityName")%>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="260px"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="220px" HeaderText="Output Indicator" meta:resourcekey="TemplateFieldResource4">
                                <ItemTemplate>
                                    <div style="width: 220px; word-wrap: break-word;">
                                        <%# Eval("DataName")%>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="220px"></ItemStyle>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="spacer" style="clear: both;">
                    </div>
                    <div class="buttonright2">
                    </div>
                    <div class="spacer" style="clear: both;">
                    </div>
                    <div class="spacer" style="clear: both;">
                    </div>
                </div>
                <div class="buttonright2">
                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                        Width="100px" CssClass="button_example" meta:resourcekey="btnSaveResource1" />
                </div>
            </div>
        </div>
    </div>
    <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
    <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" TargetControlID="btnClientOpen"
        BehaviorID="mpeAddActivity" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlLocations" runat="server" Width="800px" meta:resourcekey="pnlLocationsResource1">
        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="containerPopup">
                    <div class="graybarcontainer">
                    </div>
                    <div class="contentarea">
                        <div class="formdiv">
                            <table border="0" style="margin: 0 auto;">
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblLocAdmin1" runat="server" Text="Admin 1 Locations"></asp:Label></legend>
                                            <asp:CheckBoxList ID="cblAdmin1" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"
                                                meta:resourcekey="cblAdmin1Resource1">
                                            </asp:CheckBoxList>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <fieldset>
                                            <legend>
                                                <asp:Label ID="lblLocAdmin2" runat="server" Text="Admin 2 Locations"></asp:Label></legend>
                                            <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                                meta:resourcekey="cblLocationsResource1">
                                            </asp:CheckBoxList>
                                        </fieldset>
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
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenTargetControlForModalPopup"
        PopupControlID="Panel1" Drag="True" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="HiddenTargetControlForModalPopup" Style="display: none"
        meta:resourcekey="HiddenTargetControlForModalPopupResource1" />
    <asp:Panel ID="Panel1" Style="display: block; width: 800px;" runat="server" meta:resourcekey="Panel1Resource1">
        <div class="containerPopup">
            <div class="graybar">
                <asp:Localize ID="lzeSelectEportProjects" runat="server" meta:resourcekey="lzeSelectEportProjectsResource1"
                    Text="Select Months & Projects You Want To Export"></asp:Localize>
            </div>
            <div class="contentarea">
                <div class="formdiv">
                    <table border="0" style="margin: 0 auto;">
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Select Months</legend>
                                    <asp:CheckBoxList ID="cblMonths" runat="server" RepeatColumns="6" meta:resourcekey="cblMonthsResource1">
                                    </asp:CheckBoxList>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <fieldset>
                                    <legend>Select Projects</legend>
                                    <asp:CheckBoxList ID="cblExportProjects" runat="server" RepeatColumns="5" meta:resourcekey="cblExportProjectsResource1">
                                    </asp:CheckBoxList>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="button_example"
                                    OnClientClick="needToConfirm = false;" meta:resourcekey="btnOKResource1" />
                                <asp:Button ID="btnExportToExcelClose" runat="server" Text="Close" CausesValidation="False"
                                    OnClick="btnExportToExcelClose_Click" CssClass="button_example" OnClientClick="needToConfirm = false;"
                                    meta:resourcekey="btnExportToExcelCloseResource1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
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
    </asp:Panel>
</asp:Content>
