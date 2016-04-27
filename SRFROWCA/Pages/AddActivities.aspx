<%@ Page Title="ORS - Data Entry" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../Controls/ReportedIndicatorComments.ascx" TagName="ReportedIndicatorComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
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
    </style>

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
                return "Leave this page If you don't have any unsaved changes OR Stay on the page and save your changes before leaving the page!";
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

        function alertComment() {
            var txtCmtArea = document.getElementById("<%=txtComments.ClientID %>");
            txtCmtArea.focus();
            txtCmtArea.value = '';

            var btnSaveCmt = document.getElementById("<%=btnSaveComments.ClientID %>");
            btnSaveCmt.value = "Save";

            var hiddenField = document.getElementById("MainContent_ucIndComments_hdnUpdate");
            hiddenField.value = "-1";
        }

        function clearComments() {
            var txtCmtArea = document.getElementById("<%=txtComments.ClientID %>");
            txtCmtArea.value = '';
        }


        function gridviewScroll(gridWidth, gridHeight) {
            $('#<%=gvActivities.ClientID%>').gridviewScroll({
                width: gridWidth,
                height: gridHeight,
                railcolor: "#F0F0F0",
                barcolor: "#CDCDCD",
                barhovercolor: "#606060",
                bgcolor: "#F0F0F0",
                freezesize: 1,
                arrowsize: 20,
                varrowtopimg: "../assets/orsimages/arrowvt.png",
                varrowbottomimg: "../assets/orsimages/arrowvb.png",
                harrowleftimg: "../assets/orsimages/arrowhl.png",
                harrowrightimg: "../assets/orsimages/arrowhr.png",
                headerrowcount: 2,
                railsize: 10,
                barsize: 10
            });
        }

        $(function () {
            $(".numeric1").wholenumber();

            var windowWidth = $(window).width();
            var windowHeight = $(window).height();
            var gridWidth = 1400;
            var gridHeight = 550;

            if (windowWidth <= 1100) {
                gridWidth = 780;
            }
            else if (windowWidth <= 1120) {
                gridWidth = 850;
            }
            else if (windowWidth <= 1200) {
                gridWidth = 870;
            }
            else if (windowWidth <= 1250) {
                gridWidth = 930;
            }
            else if (windowWidth <= 1370) {
                gridWidth = 950;
            }
            else if (windowWidth <= 1450) {
                gridWidth = 1050;
            }
            else if (windowWidth <= 1550) {
                gridWidth = 1100;
            }
            else if (windowWidth <= 1650) {
                gridWidth = 1150;
            }

            if (windowHeight <= 700) {
                gridHeight = 350;
            }
            else if (windowHeight <= 850) {
                gridHeight = 450;
            }

            gridviewScroll(gridWidth, gridHeight);
            $('.cbltest').on('click', ':checkbox', function () {
                if ($(this).is(':checked')) {
                    $(this).parent().addClass('highlight');
                }
                else {
                    $(this).parent().removeClass('highlight');
                }
            });
        });

        $(document).ready(function () {
            $(".cbltest").find(":checkbox").each(function () {
                if ($(this).is(':checked')) {
                    $(this).parent().addClass('highlight');
                }
            });
        });

    </script>
    <script type="text/javascript" src="../assets/js/gridviewScroll.min.js"></script>
    <link href="../assets/css/GridviewScroll.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div style="width: 100%;">
            <table border="0" style="margin: auto; width: 60%;">
                <tr>
                    <td>Month:<asp:DropDownList ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                        onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlMonthResource1">
                    </asp:DropDownList>
                        <%--<asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True">
                            <asp:ListItem Text="2016" Value="12" meta:resourcekey="ListItemResource3"></asp:ListItem>
                            <asp:ListItem Text="2015" Value="11" meta:resourcekey="ListItemResource4"></asp:ListItem>
                        </asp:DropDownList>--%>
                    </td>
                    <td>Projects:<asp:DropDownList ID="ddlProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged"
                        onchange="needToConfirm = false;" meta:resourcekey="rblProjectsResource1">
                    </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">

                    <div class="widget-body" style="padding-right: 20px; padding-left: 20px;">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Localize ID="lzeSelectLocaitonsText" runat="server"
                                    Text="Please click on 'Locations' button to select the locations you want to report on." meta:resourcekey="lzeSelectLocaitonsTextResource1"></asp:Localize>
                                <button id="btnOpenLocations" runat="server" onserverclick="btnLocation_Click" onclick="needToConfirm = false;"
                                    type="button" class="btn btn-sm btn-primary">
                                    <i class="icon-building-o"></i>
                                    <asp:Localize ID="localLocationButton" runat="server" Text="Locations" meta:resourcekey="localLocationButtonResource1"></asp:Localize>
                                </button>
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                                DataKeyNames="ActivityDataId,ProjectIndicatorId,ReportId,ActivityId"
                                Width="98%" OnRowDataBound="gvActivities_RowDataBound" OnRowCreated="gvActivities_RowCreated"
                                meta:resourcekey="gvActivitiesResource1" GridLines="None">
                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                <RowStyle CssClass="GridviewScrollItem" />
                                <PagerStyle CssClass="GridviewScrollPager" />
                                <Columns>
                                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId"
                                        meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                                    <asp:TemplateField ItemStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" meta:resourcekey="imgObjectiveResource1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="180px" HeaderStyle-Width="180px" HeaderText="Activity" meta:resourcekey="TemplateFieldResource3">
                                        <ItemTemplate>
                                            <div style="width: 99%; word-wrap: break-word;">
                                                <%# Eval("Activity")%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="180px" HeaderStyle-Width="180px" HeaderText="Output Indicator" meta:resourcekey="TemplateFieldResource4">
                                        <ItemTemplate>
                                            <div style="width: 99%; word-wrap: break-word;">
                                                <%# Eval("Indicator")%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="65px" HeaderText="Unit">
                                        <ItemTemplate>
                                            <div style="width: 65px; word-wrap: break-word;">
                                                <%# Eval("Unit")%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="20px" HeaderStyle-Width="20px" HeaderText="CMT" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnComments" runat="server" ImageUrl="~/assets/orsimages/edit-file-icon.png"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="AddComments" OnClick="btnImgClick"
                                                OnClientClick="needToConfirm = false;clearComments();" meta:resourcekey="imgbtnCommentsResource1" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="space">
                        </div>
                        <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                            type="button" class="pull-right btn btn-sm btn-primary">
                            <i class="icon-save"></i>
                            <asp:Localize ID="localSaveButton" runat="server" Text="Save" meta:resourcekey="localSaveButtonResource1"></asp:Localize>
                        </button>
                        <div class="space">
                        </div>
                        <div class="space">
                        </div>
                        <div class="space">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
    <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" BehaviorID="mpeAddActivity" TargetControlID="btnClientOpen"
        PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlLocations" runat="server" Width="800px" meta:resourcekey="pnlLocationsResource1">
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
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblLocAdmin1" runat="server" Text="Admin 1 Locations" meta:resourcekey="lblLocAdmin1Resource1"></asp:Label></legend>
                                                <asp:CheckBoxList ID="cblAdmin1" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"
                                                    CssClass="cbltest" meta:resourcekey="cblAdmin1Resource1">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblLocAdmin2" runat="server" Text="Admin 2 Locations" meta:resourcekey="lblLocAdmin2Resource1"></asp:Label></legend>--%>
                                    <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                        CssClass="cbltest" meta:resourcekey="cblLocationsResource1">
                                    </asp:CheckBoxList>
                                    <%--</fieldset>
                                        </td>
                                    </tr>--%>
                                </table>
                            </div>
                            <div class="modal-footer no-margin-top">
                                <asp:Button ID="btnClose" runat="server" Text="Close" Width="120px" CssClass="btn btn-primary"
                                    CausesValidation="False" OnClientClick="needToConfirm = false;" meta:resourcekey="btnCloseResource1" />
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
    <!-- Comments Box Start -->
    <asp:ModalPopupExtender ID="mpeComments" runat="server" TargetControlID="Button1"
        PopupControlID="Panel2" Drag="True" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Button runat="server" ID="Button1" Style="display: none" meta:resourcekey="Button1Resource1" />
    <asp:Panel ID="Panel2" Style="display: block; width: 800px;" runat="server" meta:resourcekey="Panel2Resource1">
        <div class="row">
            <div class="modal-dialog">

                <div class="modal-content">
                    <div class="modal-header" style="border-bottom-width: 0px;">
                        <button runat="server" id="btnCancelComments" onserverclick="btnCancelComments_Click"
                            class="close" data-dismiss="modal" onclick="needToConfirm = false;">
                            &times;
                       
                        </button>
                        <%--<h4 class="blue bigger">
                            <asp:Localize ID="localIndComments" runat="server" Text="Indicator Comments" meta:resourcekey="localIndCommentsResource1"></asp:Localize>
                        </h4>--%>
                    </div>
                    <span class="btn btn-sm btn-info no-radius" style="margin-top: 5px; margin-left: 8px; line-height: 8px;" onclick="javascript:alertComment();">New Comment</span>

                    <div class="modal-body overflow-visible" style="padding-top: 5px;">
                        <div class="row">

                            <uc1:ReportedIndicatorComments ID="ucIndComments" runat="server" />
                        </div>
                    </div>
                    <br />
                    <div class="form-actions" style="margin: 0 auto; width: 97%;">
                        <div class="input-group">
                            <input type="text" runat="server" id="txtComments" name="message" class="form-control" style="text-align: left;" placeholder="Type your comment here ...">
                            <span class="input-group-btn">
                                <%--<button type="button" class="btn btn-sm btn-info no-radius">
                                    <i class="icon-share-alt"></i>
                                    Save
                                </button>--%>
                                <asp:Button ID="btnSaveComments" runat="server" Text="Save" OnClick="btnSaveComments_Click"
                                    CssClass="btn btn-sm btn-info no-radius" OnClientClick="needToConfirm = false;" meta:resourcekey="btnSaveCommentsResource1" />
                            </span>
                        </div>
                        <asp:Button Visible="false" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancelComments_Click"
                            CssClass="btn btn-primary" OnClientClick="needToConfirm = false;" />
                    </div>
                    <br />


                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
