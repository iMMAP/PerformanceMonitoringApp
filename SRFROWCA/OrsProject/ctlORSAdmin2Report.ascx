<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlORSAdmin2Report.ascx.cs" Inherits="SRFROWCA.OrsProject.ctlORSAdmin2Report" %>

<style>
    .page-content {
        margin: 0;
        padding: 8px 12px 24px;
    }

    .widget-main {
        padding: 2px;
    }

    table.imagetable2 td {
        padding: 2px 0px 2px 0px;
    }

    .padding1 {
        padding: 2px;
    }

    table.imagetable2 td {
        padding: 2px 0px 2px 0px;
    }

    .padding1 {
        padding: 2px;
    }

    .lblnotarget {
        color: red;
        font-weight: bold;
    }

    .details1 {
        display: none;
    }

    .txtalign {
        text-align: right;
    }

    .details0 {
        display: none;
    }

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
        width: 70px;
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

    .langlinks {
        color: white;
    }
</style>
<script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
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


    $(function () {

        $(".numeric1").wholenumber();
        $('.showDetails1').click(function () {
            $(this).parent().parent().next('tr.details1').toggle();
            $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                                                         '../assets/orsimages/minus.png' :
                                                          '../assets/orsimages/plus.png'))
        });

        $('.showDetails0').click(function () {
            $(this).parent().parent().parent().parent().find('tr.details0').toggle();
            $(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                                                         '../assets/orsimages/minus.png' :
                                                          '../assets/orsimages/plus.png'))
        });
    });
</script>
<div class="widget-header widget-header-small header-color-pink">
    <div>
        <label>
            <asp:Label ID="lblProjectCode" runat="server" Text=""></asp:Label>
        </label>
    </div>
    <div class="pull-right">
        <label>
            <asp:Label ID="lblReportingOrg" runat="server" Text=""></asp:Label></label>
    </div>
</div>

<div class="widget-header widget-header-small header-color-blue2">
    <div>
        Month:
        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChnaged">
        </asp:DropDownList>
    </div>
    <div class="pull-right">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
            CausesValidation="False" Width="110px" CssClass="btn btn-sm btn-warning" meta:resourcekey="btnSaveResource1" />

        <asp:Localize ID="locbtnCloseWindow" runat="server"
            Text="&lt;input type=&quot;button&quot; class=&quot;btn btn-sm &quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"
            meta:resourcekey="locbtnCloseWindowResource1"></asp:Localize>
    </div>
</div>

<div class="widget-body">
    <div class="widget-main">
        <div id="divMsg">
        </div>
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                HeaderStyle-BackColor="ButtonFace" DataKeyNames="IndicatorId,UnitId" CssClass="imagetable"
                Width="100%" OnRowDataBound="gvActivities_RowDataBound" meta:resourcekey="gvActivitiesResource1">
                <HeaderStyle BackColor="Control"></HeaderStyle>
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:BoundField DataField="ObjectiveId" HeaderText="ObjectiveId" ItemStyle-Width="1px"
                        ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" meta:resourcekey="BoundFieldResource1"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="30" ItemStyle-Width="30"
                        meta:resourcekey="TemplateFieldResource2">
                        <HeaderTemplate>
                            <asp:Label ID="lblObjectiveHeader" runat="server" Text=""></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HiddenField ID="hfIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                            <asp:Image ID="imgObjective" runat="server" meta:resourcekey="imgRindResource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="250" ItemStyle-Width="250" meta:resourcekey="TemplateFieldResource2">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderActivity" runat="server" Text="Activity" meta:resourcekey="lblGridHeaderActivityResource1"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridActivity" runat="server" Text='<%# Eval("Activity") %>'
                                meta:resourcekey="lblGridActivityResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="250" ItemStyle-Width="250" meta:resourcekey="TemplateFieldResource3">
                        <HeaderTemplate>
                            <asp:Label ID="lblGridHeaderIndicator" runat="server" Text="Indicator"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblGridIndicator" runat="server" Text='<%# Eval("Indicator") %>'
                                meta:resourcekey="lblGridIndicatorResource1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="50" ItemStyle-Width="50px">
                        <HeaderTemplate>
                            <asp:Label ID="lblUnitHeader" runat="server" Text="Unit"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="50" ItemStyle-Width="50px">
                        <HeaderTemplate>
                            <asp:Label ID="lblCalMethodHeader" runat="server" Text="Calc Method"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCalMethod" runat="server" Text='<%# Eval("CalculationType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="600" ItemStyle-Width="600">
                        <ItemTemplate>
                            <asp:Label ID="lblNoTarget" runat="server" CssClass="lblnotarget" Visible="false"
                                Text="No Target Provided For This Inidcator, Consult Secotr Coordinator"></asp:Label>
                            <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptCountry_ItemDataBound">
                                <ItemTemplate>
                                    <table style="width: 690px;" class="imagetable imagetable2 tblCountryGender">
                                        <tr>
                                            <th colspan="2" style="width: 125px;" class="lightgraycolor"></th>
                                            <th class="tdHeader" style="width: 210px;" colspan="3">Project Targets (Annual)</th>
                                            <th class="tdHeader" style="width: 210px" colspan="3">Reported Values (Monthly)</th>
                                            <th class="tdHeader" style="width: 90px" colspan="3">Running Value</th>
                                        </tr>
                                        <tr>
                                            <th colspan="2" style="width: 125px;" class="lightgraycolor">Locaiton</th>
                                            <th class="tdHeader" style="width: 70px;">Male</th>
                                            <th class="tdHeader" style="width: 70px;">Female</th>
                                            <th class="tdHeader" style="width: 70px;">Total</th>
                                            <th class="tdHeader" style="width: 70px;">Male</th>
                                            <th class="tdHeader" style="width: 70px;">Female</th>
                                            <th class="tdHeader" style="width: 70px;">Total</th>
                                            <th class="tdHeader" style="width: 90px;">Total</th>
                                        </tr>
                                        <tr style="background-color: #C0C0C0">
                                            <td width="5px">
                                                <img src="../assets/orsimages/plus.png" class="showDetails0"
                                                    title="Click to show/hide Admin1" alt="Expand/Collapse Admin1" /></td>
                                            <td style="width: 120px;">
                                                <div style="float: left; width: 120px; padding: 2px; text-align: left;">
                                                    <%#Eval("LocationName")%>
                                                    <asp:HiddenField ID="hfCountryId" runat="server" Value='<%#Eval("LocationId")%>' />
                                                    <asp:HiddenField ID="hfCountryIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetMaleCluster" runat="server"
                                                    Text='<%#Eval("ProjectMale") %>' ToolTip="Cluster Target: Project Total Male"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetFemaleCluster" runat="server"
                                                    Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Target: Country Total Female"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetCluster" runat="server"
                                                    Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Target: Country Total"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetMaleProject" runat="server"
                                                    Text='<%#Eval("ReportedMale") %>' ToolTip="Reported: Country Total Male"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetFemaleProject" runat="server"
                                                    Text='<%#Eval("ReportedFemale") %>' ToolTip="Reported: Country Total Female"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryTargetProject" runat="server"
                                                    Text='<%#Eval("ReportedTotal") %>' ToolTip="Reported: Country Total"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="70px"></asp:Label>
                                            </td>
                                            <td class="tdTable">
                                                <asp:Label ID="lblCountryRunningValue" runat="server"
                                                    Text='<%#Eval("RunningValue") %>' ToolTip="Running Value"
                                                    CssClass="trgtCountryGenderMale txtalign" Width="90px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="details0">
                                            <td style="width: 100%;" colspan="9">
                                                <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1Gender_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="margin: 0 auto; width: 690px;" border="0" class="imagetable imagetable2 tblAdmin1Gender">
                                                            <tr style="background-color: #D8D8D8" class="trAdmin1">
                                                                <td width="5px">
                                                                    <img src="../assets/orsimages/plus.png" class="showDetails1"
                                                                        title="Click to show/hide Admin2" /></td>
                                                                <td style="width: 130px;">
                                                                    <div style="float: left; width: 100%; padding: 2px; text-align: left;"><%#Eval("LocationName")%></div>
                                                                    <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                    <asp:HiddenField ID="hfAdmin1IndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetMaleCluster" runat="server"
                                                                        Text='<%#Eval("ProjectMale") %>' ToolTip="Project Tareget: Admin1 Total Male"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetFemaleCluster" runat="server"
                                                                        Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Target: Admin1 Total Female"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetCluster" runat="server"
                                                                        Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Target: Admin1 Total"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetMaleProject" runat="server"
                                                                        Text='<%#Eval("ReportedMale") %>' ToolTip="Reported: Admin1 Total Male"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetFemaleProject" runat="server"
                                                                        Text='<%#Eval("ReportedFemale") %>' ToolTip="Reported: Admin1 Total Female"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1TargetProject" runat="server"
                                                                        Text='<%#Eval("ReportedTotal") %>' ToolTip="Reported: Admin1 Total"
                                                                        CssClass="trgtAdmin1GenderMale txtalign"
                                                                        Width="70px"></asp:Label>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:Label ID="lblAdmin1RunningValue" runat="server"
                                                                        Text='<%#Eval("RunningValue") %>' ToolTip="Running Value"
                                                                        CssClass="trgtCountryGenderMale txtalign" Width="90px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr class="details1" style="background-color: #F0F0F0">
                                                                <td></td>
                                                                <td style="width: 100%;" colspan="9">
                                                                    <asp:Repeater ID="rptAdmin2" runat="server" OnItemDataBound="rptAdmin2_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <table style="margin: 0 auto; width: 685px;" border="0" class="imagetable tblAdmin2Gender">
                                                                                <tr>
                                                                                    <td style="width: 130px">
                                                                                        <div style="float: left; width: 100%; padding: 2px; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                        <asp:HiddenField ID="hfAdmin2Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:Label ID="lblAdmin2TargetMaleCluster" runat="server"
                                                                                            Text='<%#Eval("ProjectMale") %>' ToolTip="Project Target: Admin2 Total Male"
                                                                                            CssClass="trgtAdmin2GenderMale padding1 txtalign"
                                                                                            Width="70px"></asp:Label>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:Label ID="lblAdmin2TargetFemaleCluster" runat="server"
                                                                                            Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Target: Admin2 Total Female"
                                                                                            CssClass="trgtAdmin2GenderMale txtalign"
                                                                                            Width="70px"></asp:Label>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:Label ID="lblAdmin2TargetCluster" runat="server"
                                                                                            Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Target: Admin2 Total"
                                                                                            CssClass="trgtAdmin2GenderMale txtalign"
                                                                                            Width="70px"></asp:Label>
                                                                                    </td>
                                                                                    <td class="tdTable" style="padding-left: 2px;">
                                                                                        <asp:TextBox ID="txtAdmin2TargetMaleProject" runat="server"
                                                                                            Text='<%#Eval("ReportedMale") %>' ToolTip="Reported Admin2 Total Male"
                                                                                            CssClass="trgtAdmin2GenderMale numeric1 padding1 txtalign"
                                                                                            Width="70px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable" style="padding-left: 2px;">
                                                                                        <asp:TextBox ID="txtAdmin2TargetFemaleProject" runat="server"
                                                                                            Text='<%#Eval("ReportedFemale") %>' ToolTip="Reported Admin2 Total Female"
                                                                                            CssClass="trgtAdmin1GenderMale numeric1 padding1 txtalign"
                                                                                            Width="70px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable" style="padding-left: 2px;">
                                                                                        <asp:TextBox ID="txtAdmin2TargetProject" runat="server"
                                                                                            Text='<%#Eval("ReportedTotal") %>' ToolTip="Reported Admin2 Total"
                                                                                            CssClass="trgtAdmin1GenderMale numeric1 txtalign" Style="text-align: right;"
                                                                                            Width="70px"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:Label ID="lblAdmin2RunningValue" runat="server"
                                                                                            Text='<%#Eval("RunningValue") %>' ToolTip="Running Value"
                                                                                            CssClass="trgtCountryGenderMale txtalign" Width="90px"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField runat="server" ID="hfLocationIdGender" Value='<%# Eval("LocationId") %>' />
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>

