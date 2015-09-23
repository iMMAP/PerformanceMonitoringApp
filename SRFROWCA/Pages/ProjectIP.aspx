<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectIP.aspx.cs" Inherits="SRFROWCA.Pages.ProjectIP" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
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

    
    <script type="text/javascript" src="../assets/orsjs/ShowHideObJAndPr.js"></script>
    
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

        $(function () {
            showHideObj();
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
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-3">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">
                            <%--  </div>--%>
                            <%-- <div class="col-sm-14 widget-container-span">--%>
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h5>
                                        <asp:Localize ID="lzeLgndProjects" runat="server"
                                            Text="Projects" meta:resourcekey="lzeLgndProjectsResource1"></asp:Localize>
                                    </h5>
                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="slim-scroll" data-height="200">
                                        <div class="widget-main">
                                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged"
                                                meta:resourcekey="rblProjectsResource1">
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%-- </div>--%>
                            <%-- <div class="col-sm-14 widget-container-span">--%>
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h5>
                                        <asp:Localize ID="lzeLgndStrObjs" runat="server"
                                            Text="Strategic Objectives" meta:resourcekey="lzeLgndStrObjsResource1"></asp:Localize>
                                    </h5>
                                    <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj" meta:resourcekey="cblObjectivesResource1">
                                        </asp:CheckBoxList>
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
                        <h4></h4>
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="icon-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body" style="padding-right: 20px; padding-left: 20px;">
                        <div class="widget-main">
                            <div class="pull-left">
                                <asp:Localize ID="lzeSelectLocaitonsText" runat="server"
                                    Text="Please click on 'Locations' button to select the locations to add partners." meta:resourcekey="lzeSelectLocaitonsTextResource1"></asp:Localize>
                                <button id="btnOpenLocations" runat="server" onserverclick="btnLocation_Click"
                                    type="button" class="btn btn-sm btn-primary">
                                    <i class="icon-building-o"></i>
                                    <asp:Localize ID="localLocationButton" runat="server" Text="Locations" meta:resourcekey="localLocationButtonResource1"></asp:Localize>
                                </button>
                            </div>
                            
                            <div class="spacer" style="clear: both;">
                            </div>
                        </div>

                        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                            DataKeyNames="LocId,ActivityId" CssClass="imagetable"
                            Width="100%" OnRowDataBound="gvActivities_RowDataBound"
                            meta:resourcekey="gvActivitiesResource1">
                            <HeaderStyle BackColor="Control"></HeaderStyle>
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                                    ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1">
                                    <HeaderStyle CssClass="hidden"></HeaderStyle>
                                    <ItemStyle CssClass="hidden" Width="1px"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="60%" HeaderText="Activity" meta:resourcekey="TemplateFieldResource3">
                                    <ItemTemplate>
                                        <div style="width: 90%; word-wrap: break-word;">
                                            <%# Eval("Activity")%>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="100px"></asp:BoundField>
                                <asp:TemplateField HeaderText="Partners">
                                    <ItemTemplate>
                                        <cc:DropDownCheckBoxes ID="ddlOrgs" Width="40%" runat="server" AddJQueryReference="True">
                                            <Style SelectBoxWidth="" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight=""></Style>
                                            <Texts SelectBoxCaption="Select Partner" />
                                        </cc:DropDownCheckBoxes>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="space">
                        </div>
                        <button id="btnSave" runat="server" onserverclick="btnSave_Click"
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
    <asp:ModalPopupExtender ID="mpeAddActivity1" runat="server" BehaviorID="mpeAddActivity" TargetControlID="btnClientOpen"
        PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
        Enabled="True">
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

                                </table>
                            </div>
                            <div class="modal-footer no-margin-top">
                                <asp:Button ID="btnClose" runat="server" Text="Close" Width="120px" CssClass="btn btn-primary" OnClick="btnClose_Click"
                                    CausesValidation="False" />
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

</asp:Content>
