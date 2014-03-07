<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities" Culture="auto"
    UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_cblLocations td
        {
            padding: 0 40px 0 0;
        }
        
        .imgButtonImg 
        {
            margin:0; 
            padding:0;
            display:inline-block;            
            border: 0;
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
                                onchange="needToConfirm = false;" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                InitialValue="0" Text="*" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                <asp:Localize ID="locaYearMonth" runat="server" Text="
                                Year/Month:"></asp:Localize></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="60px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server" Width="90px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>Projects</legend>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                        onchange="needToConfirm = false;">
                    </asp:RadioButtonList>
                    <br />
                    <br />
                </fieldset>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>Strategic Objectives</legend>
                    <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj">
                    </asp:CheckBoxList>
                </fieldset>
                <fieldset>
                    <legend>Humanitarian Priorities</legend>
                    <asp:CheckBoxList ID="cblPriorities" runat="server" CssClass="checkPr">
                    </asp:CheckBoxList>
                </fieldset>
            </div>
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <b><a href="/Pages/CreateProject.aspx">Manage Projects</a></b>
                    <br />
                    <br />
                    <b><a href="/Pages/ManageActivities.aspx">Manage Activities</a></b>
                    <br />
                </fieldset>
            </div>
        </div>
        <div class="containerDataEntryGrid">
            <div class="buttonsdiv">
                <div class="savebutton2">
                    Please click on 'Locations' button to select the locations you want to report on.
                    <asp:Button ID="btnOpenLocations" runat="server" Text="Manage Locations" CausesValidation="False"
                        CssClass="button_example" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;" />
                </div>
                <div class="buttonright2">
                    Export To:
                    <asp:ImageButton ID="btnPDF" runat="server" ImageUrl="~/images/pdf.png" OnClick="btnPDF_Export"
                        OnClientClick="needToConfirm = false;" CssClass="imgButtonImg" />
                    <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/images/excel.png" OnClick="btnExcel_Export"
                        OnClientClick="needToConfirm = false;" CssClass="imgButtonImg" />
                </div>
                <div class="savebutton">
                </div>
                <div class="spacer" style="clear: both;">
                </div>
            </div>
            <div class="tablegrid">
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                    <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="False"
                        HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                        Width="100%" meta:resourcekey="gvActivitiesResource1" OnRowDataBound="gvActivities_RowDataBound">
                        <HeaderStyle BackColor="Control"></HeaderStyle>
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="ProjectId" HeaderText="pid" ItemStyle-Width="1px" ItemStyle-CssClass="hiddenelement"
                                HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="objAndPrAndPId" HeaderText="objprpid" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="objAndPId" HeaderText="objAndPId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="PrAndPId" HeaderText="PrAndPId" ItemStyle-Width="1px"
                                ItemStyle-CssClass="hiddenelement" HeaderStyle-CssClass="hiddenelement">
                                <HeaderStyle CssClass="hiddenelement"></HeaderStyle>
                                <ItemStyle Wrap="False" CssClass="hiddenelement"></ItemStyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Project Code" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblProjectcode" runat="server" Text='<%#Eval("ClusterName") %>' ToolTip='<%#Eval("ProjectTitle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Image ID="imgObjective" runat="server" ImageUrl="~/images/O.png" AlternateText="Obj" />
                                    <asp:Image ID="imgPriority" runat="server" ImageUrl="~/images/P.png" AlternateText="Obj" />
                                    <asp:Image ID="imgRind" runat="server" ImageUrl="" AlternateText="" />
                                    <asp:Image ID="imgCind" runat="server" ImageUrl="" AlternateText="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="260px" HeaderText="Activity">
                                <ItemTemplate>
                                    <div style="width: 260px; word-wrap: break-word;">
                                        <%# Eval("ActivityName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-Width="220px" HeaderText="Output Indicator">
                                <ItemTemplate>
                                    <div style="width: 220px; word-wrap: break-word;">
                                        <%# Eval("DataName")%>
                                    </div>
                                </ItemTemplate>
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
                        Width="100px" CssClass="button_example" />
                </div>
            </div>
        </div>
    </div>
    <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
    <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" BehaviorID="mpeAddActivity"
        TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlLocations" runat="server" Width="700px" meta:resourcekey="pnlLocationsResource1">
        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="containerPopup">
                    <div class="contentarea">
                        <div class="formdiv">
                            <table border="0" style="margin: 0 auto;">
                                <tr>
                                    <td>
                                        <div class="graybar">
                                            Admin 1
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblAdmin1" runat="server" RepeatColumns="6" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="graybar">
                                            Admin 2
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                            meta:resourcekey="cblLocationsResource1">
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
    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="HiddenTargetControlForModalPopup"
        PopupControlID="Panel1" Drag="true" BackgroundCssClass="modalpopupbackground">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="HiddenTargetControlForModalPopup" Style="display: none" />
    <asp:Panel ID="Panel1" Style="display: block; width: 700px;" runat="server">
        <div class="containerPopup">
            <div class="graybar">
                Select Projects You Want To Export
            </div>
            <div class="contentarea">
                <div class="formdiv">
                    <table border="0" style="margin: 0 auto;">
                        <tr>
                            <td>
                                <asp:Label ID="lblMonths" runat="server" Text="Select Months To Generate Document"></asp:Label>
                                <asp:CheckBoxList ID="cblMonths" runat="server" RepeatColumns="6">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBoxList ID="cblExportProjects" runat="server" RepeatColumns="4">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnOK" runat="server" Text="OK" OnClick="btnOK_Click" CssClass="button_example"
                                    OnClientClick="needToConfirm = false;" />
                                <asp:Button ID="btnExportToExcelClose" runat="server" Text="Close" CausesValidation="false"
                                    OnClick="btnExportToExcelClose_Click" CssClass="button_example" OnClientClick="needToConfirm = false;" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage2" runat="server" CssClass="error-message" Visible="false"
                                    ViewStateMode="Disabled"></asp:Label>
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
