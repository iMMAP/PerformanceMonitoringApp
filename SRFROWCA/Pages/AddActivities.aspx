<%@ Page Title="ORS - Data Entry" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
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

            // scrollables
            $('.slim-scroll').each(function () {
                var $this = $(this);
                $this.slimScroll({
                    height: $this.data('height') || 100,
                    railVisible: true
                });
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
            <li><i class="fa fa-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbDataEntry" runat="server" Text="Data Entry" meta:resourcekey="localBreadCrumbDataEntryResource1"></asp:Localize></li>
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
                                            <asp:Localize ID="Localize1" runat="server" Text="Year/Month:" meta:resourcekey="Localize1Resource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
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
                                            <asp:Localize ID="lzeLgndProjects" runat="server"
                                                Text="Projects" meta:resourcekey="lzeLgndProjectsResource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="slim-scroll" data-height="200">
                                            <div class="widget-main">
                                                <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                                                    onchange="needToConfirm = false;" meta:resourcekey="rblProjectsResource1">
                                                </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            <asp:Localize ID="lzeLgndStrObjs" runat="server"
                                                Text="Strategic Objectives" meta:resourcekey="lzeLgndStrObjsResource1"></asp:Localize>
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
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
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
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
                            <button runat="server" id="btnGeneratePDF" onserverclick="btnPDF_Export" onclick="needToConfirm = false;"
                                class="width-10 btn btn-sm btn-yellow" title="PDF">
                                <i class="fa fa-download"></i>PDF
                            </button>
                            <button runat="server" id="btnExportToExcel" onserverclick="btnExcel_Export" onclick="needToConfirm = false;"
                                class="width-10 btn btn-sm btn-yellow" title="Excel">
                                <i class="fa fa-download"></i>Excel
                            </button>
                        </h4>
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="fa fa-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Localize ID="lzeSelectLocaitonsText" runat="server"
                                    Text="Please click on 'Locations' button to select the locations you want to report on." meta:resourcekey="lzeSelectLocaitonsTextResource1"></asp:Localize>
                                <button id="btnOpenLocations" runat="server" onserverclick="btnLocation_Click" onclick="needToConfirm = false;"
                                    type="button" class="btn btn-sm btn-primary">
                                    <i class="fa fa-building-o"></i>
                                    <asp:Localize ID="localLocationButton" runat="server" Text="Locations" meta:resourcekey="localLocationButtonResource1"></asp:Localize>
                                </button>
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>
                        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                DataKeyNames="ActivityDataId,ProjectIndicatorId,ReportId" CssClass="imagetable"
                                Width="100%" OnRowDataBound="gvActivities_RowDataBound"
                                OnRowCommand="gvActivities_RowCommand" meta:resourcekey="gvActivitiesResource1">
                                <HeaderStyle BackColor="Control"></HeaderStyle>
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="HumanitarianPriorityId"
                                        ItemStyle-Width="1px" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource2">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ObjAndPrId" HeaderText="objprid" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource3">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ProjectId" HeaderText="pid" ItemStyle-Width="1px" ItemStyle-CssClass="hidden"
                                        HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource4">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="objAndPrAndPId" HeaderText="objprpid" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource5">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="objAndPId" HeaderText="objAndPId" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource6">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PrAndPId" HeaderText="PrAndPId" ItemStyle-Width="1px"
                                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource7">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Project Code" ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProjectcode" runat="server" Text='<%# Eval("ProjectCode") %>' ToolTip='<%# Eval("ProjectTitle") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" meta:resourcekey="imgObjectiveResource1"
                                                />
                                            <asp:Image ID="imgPriority" runat="server" AlternateText="Obj" meta:resourcekey="imgPriorityResource1"
                                                />
                                            <asp:Image ID="imgRind" runat="server" meta:resourcekey="imgRindResource1"/>
                                            <asp:Image ID="imgCind" runat="server" meta:resourcekey="imgCindResource1"/>
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
                                    <asp:BoundField DataField="RInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource8">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CInd" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource9">
                                        <HeaderStyle CssClass="hidden"></HeaderStyle>

                                        <ItemStyle CssClass="hidden"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField ItemStyle-Width="30px" HeaderText="Cmt" meta:resourcekey="TemplateFieldResource5">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgbtnComments" runat="server" ImageUrl="~/assets/orsimages/edit-file-icon.png"
                                                CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="AddComments"
                                                OnClientClick="needToConfirm = false;" meta:resourcekey="imgbtnCommentsResource1" />
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
                            <i class="fa fa-save"></i>
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
    <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" BehaviorID="mpeAddActivity" TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
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
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <legend>
                                                    <asp:Label ID="lblLocAdmin2" runat="server" Text="Admin 2 Locations" meta:resourcekey="lblLocAdmin2Resource1"></asp:Label></legend>
                                                <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                                    CssClass="cbltest" meta:resourcekey="cblLocationsResource1">
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
    <asp:Button runat="server" ID="Button1" Style="display: none" meta:resourcekey="Button1Resource1" />
    <asp:Panel ID="Panel2" Style="display: block; width: 800px;" runat="server" meta:resourcekey="Panel2Resource1">
        <div class="row">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button runat="server" id="btnCancelComments" onserverclick="btnCancelComments_Click"
                            class="close" data-dismiss="modal" onclick="needToConfirm = false;">
                            &times;
                        </button>
                        <h4 class="blue bigger">
                            <asp:Localize ID="localIndComments" runat="server" Text="Indicator Comments" meta:resourcekey="localIndCommentsResource1"></asp:Localize>
                        </h4>
                    </div>
                    <div class="modal-body overflow-visible">
                        <div class="row">
                            <asp:TextBox ID="txtComments" runat="server" Width="500px" Height="200px" TextMode="MultiLine"
                                CssClass="commentstext" meta:resourcekey="txtCommentsResource1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveComments" runat="server" Text="Save" OnClick="btnSaveComments_Click"
                            CssClass="btn btn-primary" OnClientClick="needToConfirm = false;" meta:resourcekey="btnSaveCommentsResource1" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
