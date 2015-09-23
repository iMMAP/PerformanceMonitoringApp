<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectPartners.ascx.cs" Inherits="SRFROWCA.Pages.ProjectPartners" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>

<style>
    #MainContent_cblLocations td {
        padding: 0 40px 0 0;
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
    });

</script>


<div class="row">
    <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="hidden checkObj" meta:resourcekey="cblObjectivesResource1">
    </asp:CheckBoxList>
    <div class="col-sm-12 widget-container-span">
        <div class="widget-header widget-header-small header-color-blue2">
            <h4>
                <asp:Localize ID="local" runat="server" Text="Add/Remove Project Partner Organizations" meta:resourcekey="localResource1"></asp:Localize>
            </h4>
            <button id="Button1" runat="server" onserverclick="btnSave_Click" onclick="needToConfirm = false;"
                type="button" class="btn btn-sm btn-yellow pull-right">
                <i class="icon-save"></i>
                <asp:Localize ID="localClsuterTargetsSave" runat="server" Text="Save"></asp:Localize>
            </button>
        </div>
        <div class="widget-body" style="padding-right: 10px; padding-left: 10px;">
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
            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                    DataKeyNames="LocId,ActivityId" CssClass="imagetable tempclass"
                    Width="100%" OnRowDataBound="gvActivities_RowDataBound"
                    meta:resourcekey="gvActivitiesResource1">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                            ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1"></asp:BoundField>
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
                        <asp:BoundField DataField="Location" HeaderText="Location" ItemStyle-Width="10%"></asp:BoundField>
                        <asp:TemplateField HeaderText="Partners" ItemStyle-Width="30%">
                            <ItemTemplate>
                                <cc:DropDownCheckBoxes ID="ddlOrgs" Width="300px" runat="server" AddJQueryReference="True">
                                    <Style SelectBoxWidth="80%" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight=""></Style>
                                    <Texts SelectBoxCaption="Select Partner" />
                                </cc:DropDownCheckBoxes>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="height:100px">
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
