<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities"
    Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

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
        }
        
        .commentstext
        {
            border: 1px solid #D5D5D5;
            border-radius: 0 !important;
            box-shadow: none !important;
            font-family: inherit;
            font-size: 12px;
            line-height: 1.2;
            padding: 0px 0px;
            transition-duration: 0.1s;
            text-align: left;
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
            $(".numeric1").numeric();
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

                $(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th style="width: 100px;">&nbsp;</th><th style="width: 50px;">&nbsp;</th><th style="width: 260px;">&nbsp;</th><th style="width: 220px;">&nbsp;</th><th style="width: 30px;">&nbsp;</th>' + list + '</tr></thead>');
            }

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Data Entry</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-3">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="Localize1" runat="server" Text="
                                Year/Month:" meta:resourcekey="lzeYearMonthResource1"></asp:Localize></label>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:DropDownList ID="ddlYear" runat="server" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlYearResource1">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlMonth" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlMonthResource1">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndProjects" runat="server" meta:resourcekey="lzeLgndProjectsResource1"
                                                Text="Projects"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                                                onchange="needToConfirm = false;" meta:resourcekey="rblProjectsResource1">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize>
                                                <asp:Localize ID="lzeLgndStrObjs" runat="server" meta:resourcekey="lzeLgndStrObjsResource1"
                                                    Text="Strategic Objectives"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
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
                                            <asp:Localize ID="lzeLgndHumPriorities" runat="server" meta:resourcekey="lzeLgndHumPrioritiesResource1"
                                                Text="Humanitarian Priorities"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
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
                        <h4>
                        </h4>
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="icon-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Localize ID="lzeSelectLocaitonsText" runat="server" meta:resourcekey="lzeSelectLocaitonsTextResource1"
                                    Text="Please click on 'Locations' button to select the locations you want to report on."></asp:Localize>
                                <button id="btnOpenLocations" runat="server" onserverclick="btnLocation_Click" onclick="needToConfirm = false;"
                                    type="button" class="btn btn-sm btn-primary">
                                    <i class="fa fa-building-o"></i>Locations
                                </button>
                            </div>
                            <div class="pull-right">
                                <asp:Localize ID="lzeExportToText" runat="server" meta:resourcekey="lzeExportToTextResource1"
                                    Text="Export To:"></asp:Localize>
                                <asp:ImageButton ID="btnPDF" runat="server" ImageUrl="~/assets/orsimages/pdf.png"
                                    OnClick="btnPDF_Export" OnClientClick="needToConfirm = false;" CssClass="imgButtonImg"
                                    meta:resourcekey="btnPDFResource1" />
                                <asp:ImageButton ID="btnExcel" runat="server" ImageUrl="~/assets/orsimages/excel.png"
                                    OnClick="btnExcel_Export" OnClientClick="needToConfirm = false;" CssClass="imgButtonImg"
                                    meta:resourcekey="btnExcelResource1" />
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                DataKeyNames="ActivityDataId,ProjectIndicatorId,ReportId" CssClass="imagetable"
                                Width="100%" meta:resourcekey="gvActivitiesResource1" OnRowDataBound="gvActivities_RowDataBound"
                                OnRowCommand="gvActivities_RowCommand">
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
                                            <asp:Label ID="lblProjectcode" runat="server" Text='<%# Eval("ProjectCode") %>' ToolTip='<%# Eval("ProjectTitle") %>'></asp:Label>
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
                                    <asp:BoundField DataField="RInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                    <asp:BoundField DataField="CInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="Cmt">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnComments" runat="server" ImageUrl="~/assets/orsimages/edit-file-icon.png"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="AddComments"
                                                OnClientClick="needToConfirm = false;" />
                                        </ItemTemplate>
                                        <ItemStyle Width="30px"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="space">
                        </div>
                        <button id="btnSave" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                            type="button" class="width-20 pull-right btn btn-sm btn-primary">
                            <i class="icon-save"></i>Save
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
    <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" TargetControlID="btnClientOpen"
        BehaviorID="mpeAddActivity" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
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
                                                    <asp:Label ID="lblLocAdmin1" runat="server" Text="Admin 1 Locations"></asp:Label></legend>
                                                <asp:CheckBoxList ID="cblAdmin1" runat="server" RepeatColumns="6" RepeatDirection="Horizontal"
                                                    meta:resourcekey="cblAdmin1Resource1" CssClass="cbltest">
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
                                                    meta:resourcekey="cblLocationsResource1" CssClass="cbltest">
                                                </asp:CheckBoxList>
                                            </fieldset>
                                        </td>
                                    </tr>
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
    <asp:Button runat="server" ID="Button1" Style="display: none" />
    <asp:Panel ID="Panel2" Style="display: block; width: 800px;" runat="server">
        <div class="row">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header no-padding">
                        <div class="table-header">
                            <button runat="server" id="btnCancelComments" onserverclick="btnCancelComments_Click"
                                class="close" data-dismiss="modal" aria-hidden="true" onclick="needToConfirm = false;">
                                <span class="white">&times;</span>
                            </button>
                            Indicator Comments
                        </div>
                    </div>
                    <div class="modal-body no-padding">
                        <table border="0" style="margin: 0 auto;">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtComments" runat="server" Width="400px" Height="300px" TextMode="MultiLine"
                                        CssClass="commentstext"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer no-margin-top">
                        <asp:Button ID="btnSaveComments" runat="server" Text="Save" OnClick="btnSaveComments_Click"
                            CssClass="btn btn-primary" OnClientClick="needToConfirm = false;" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
