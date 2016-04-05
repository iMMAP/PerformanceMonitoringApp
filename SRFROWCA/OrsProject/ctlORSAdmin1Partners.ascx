<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlORSAdmin1Partners.ascx.cs" Inherits="SRFROWCA.OrsProject.ctlORSAdmin1Partners" %>
<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
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

    .ddlWidth {
        width: 50%;
    }
</style>
<script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
<script type="text/javascript">
    //var needToConfirm = true;
    //window.onbeforeunload = confirmExit;
    //function confirmExit() {
    //    if (needToConfirm) {
    //        var message = '';
    //        var e = e || window.event;
    //        // For IE and Firefox prior to version 4
    //        if (e) {
    //            e.returnValue = message;
    //        }
    //        // For Safari
    //        return message;
    //    }
    //}


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


<div class="widget-header widget-header-small header-color-blue2">
   <%-- <asp:Localize ID="locClusterCaption" runat="server"
        Text="Sector:" meta:resourcekey="locClusterCaptionResource1"></asp:Localize>
    <asp:Label ID="lblCluster" runat="server" meta:resourcekey="lblClusterResource1"></asp:Label>--%>
    <div class="pull-right">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" 
            CausesValidation="False" Width="110px" CssClass="btn btn-sm btn-warning" meta:resourcekey="btnSaveResource1" />

        <asp:Localize ID="locbtnCloseWindow" runat="server"
            Text="&lt;input type=&quot;button&quot; class=&quot;btn btn-sm &quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"
            meta:resourcekey="locbtnCloseWindowResource1"></asp:Localize>
    </div>
</div>

<div class="widget-body">
    <div class="widget-main">
         <div class="pull-left">
            <asp:Label ID="lblPageNumber" runat="server" Text=""></asp:Label>
        </div>
        <div class="center">
            <asp:Button ID="btnPrevious" runat="server" OnClick="btnPrevious_Click" Text="<< Previous" 
                CausesValidation="False" Width="110px" CssClass="btn btn-sm btn-yellow"/>

            <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next >>" 
                CausesValidation="False" Width="110px" CssClass="btn btn-sm btn-yellow"/>
        </div>
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
                            <asp:Label ID="lblIndIdTemp" runat="server" Text='<%#Eval("IndicatorId")%>'></asp:Label>
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
                                Text="No Target Provided For This Inidcator, Consult Sector Coordinator"></asp:Label>
                            <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptCountry_ItemDataBound">
                                <ItemTemplate>
                                    <table style="width: 600px;" class="imagetable imagetable2 tblCountryGender">
                                        <tr>
                                            <th colspan="2" style="width: 100px;" class="lightgraycolor">Locaiton</th>
                                            <th class="tdHeader" style="width: 150px;">Organizations</th>
                                        </tr>
                                        <tr style="background-color: #C0C0C0">
                                            <td width="5px">
                                                <img src="../assets/orsimages/plus.png" class="showDetails0"
                                                    title="Click to show/hide Admin1" alt="Expand/Collapse Admin1" /></td>
                                            <td style="width: 120px;">
                                                <div style="float: left; width: 120px; padding: 2px; text-align: left;">
                                                    <%#Eval("LocationName")%>
                                                    <asp:HiddenField ID="hfCountryIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                            </td>
                                            <td class="tdTable">
                                                <%--<asp:DropDownList ID="ddlOrganizations" runat="server"></asp:DropDownList>--%>
                                            </td>
                                        </tr>
                                        <tr class="details0">
                                            <td style="width: 100%;" colspan="8">
                                                <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="margin: 0 auto; width: 600px;" border="0" class="imagetable imagetable2 tblAdmin1Gender">
                                                            <tr style="background-color: #D8D8D8" class="trAdmin1">
                                                                <td style="width: 142px;">
                                                                    <div style="float: left; width: 100%; padding: 2px; text-align: left;"><%#Eval("LocationName")%></div>
                                                                    <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                    <asp:HiddenField ID="hfAdmin1IndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                                                </td>
                                                                <td class="tdTable">
                                                                    <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server"
                                                                        AutoPostBack="true" AddJQueryReference="True"
                                                                        UseButtons="False" CssClass="ddlWidth" UseSelectAllNode="True">
                                                                        <Style SelectBoxWidth="" DropDownBoxBoxWidth="200%" DropDownBoxBoxHeight=""></Style>
                                                                        <Texts SelectBoxCaption="Select" />
                                                                    </cc:DropDownCheckBoxes>

                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
