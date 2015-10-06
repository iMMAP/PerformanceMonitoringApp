<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .details1 {
            display: none;
        }

        tdTable {
            width: 10%;
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

        function showHideObj() {
            $(document).on('click', '.checkObj', function () {
                var selectedObjs = [];
                $("[id*=cblObjectives] input:checked").each(function () {
                    selectedObjs.push($(this).val());
                });

                showAllObj();
                if (selectedObjs.length > 0) {
                    hideAllObj();
                    var i;
                    for (i = 0; i < selectedObjs.length; ++i) {
                        showObj(selectedObjs[i]);
                    }
                }
            });
        }

        function showAllObj() {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                $(this).parent().show();
            });
        }

        function hideAllObj() {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                $(this).parent().hide();
            });
        }

        function showObj(objId) {
            $('.istrow, .altcolor').find('td:nth-child(1)').each(function (i) {
                if ($(this).text() === objId || objId === '0') {
                    $(this).parent().show();
                }
            });
        }

        $(function () {
            //showHideObj();

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" border="1">

    <div class="page-content">
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <asp:Localize ID="locClusterCaption" runat="server"
                            Text="Cluster:" meta:resourcekey="locClusterCaptionResource1"></asp:Localize>
                        <asp:Label ID="lblCluster" runat="server" meta:resourcekey="lblClusterResource1"></asp:Label>
                        <div class="pull-right">
                            <asp:LinkButton ID="lnkLanguageEnglish" Text="English" runat="server" OnClientClick="needToConfirm=true;" CssClass="langlinks"
                                CausesValidation="False" OnClick="lnkLanguageEnglish_Click" meta:resourcekey="lnkLanguageEnglishResource1"></asp:LinkButton>&nbsp;&nbsp;

                            <asp:LinkButton ID="lnkLanguageFrench" Text="Français" runat="server" OnClientClick="needToConfirm=true;" CssClass="langlinks"
                                CausesValidation="False" OnClick="lnkLanguageFrench_Click" meta:resourcekey="lnkLanguageFrenchResource1"></asp:LinkButton>
                        </div>
                    </div>

                    <div class="widget-body">
                        <div class="widget-main">
                            <div class="pull-right">
                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                                    CausesValidation="False" Width="120px" CssClass="btn btn-primary" meta:resourcekey="btnSaveResource1" />
                                <asp:Localize ID="locbtnCloseWindow" runat="server" Text="&lt;input type=&quot;button&quot; class=&quot;btn btn-primary&quot; value=&quot;Close Window&quot; id=&quot;close&quot; onclick=&quot;window.close()&quot; /&gt;"
                                    meta:resourcekey="locbtnCloseWindowResource1"></asp:Localize>
                            </div>
                            <div class="spacer" style="clear: both;">
                            </div>
                            <br />
                            <div id="divMsg">
                            </div>
                            <div>
                                <asp:CheckBoxList ID="cblObjectives" runat="server" CssClass="checkObj hidden" RepeatColumns="3"></asp:CheckBoxList>
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
                                                <%--<asp:Label ID="lblObjective" runat="server" Text='<%# Eval("Objective") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource2">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblGridHeaderActivity" runat="server" Text="Activity" meta:resourcekey="lblGridHeaderActivityResource1"></asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblGridActivity" runat="server" Text='<%# Eval("Activity") %>'
                                                    meta:resourcekey="lblGridActivityResource1"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150" ItemStyle-Width="150" meta:resourcekey="TemplateFieldResource3">
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
                                        <asp:TemplateField HeaderStyle-Width="500" ItemStyle-Width="500px">
                                            <ItemTemplate>
                                                <asp:Repeater ID="rptCountryGender" runat="server" OnItemDataBound="rptCountryGender_ItemDataBound">
                                                    <ItemTemplate>
                                                        <table style="width: 100%;" class="imagetable tblCountryGender">
                                                            <tr>
                                                                <th colspan="2" style="width: 135px;" class="lightgraycolor"></th>
                                                                <th class="tdHeader" colspan="3">Cluster Targets</th>
                                                                <th class="tdHeader" colspan="3">Project Targets</th>
                                                            </tr>
                                                            <tr>
                                                                <th colspan="2" style="width: 135px;" class="lightgraycolor">Locaiton</th>
                                                                <th class="tdHeader">Male</th>
                                                                <th class="tdHeader">Female</th>
                                                                <th class="tdHeader">Total</th>
                                                                <th class="tdHeader">Male</th>
                                                                <th class="tdHeader">Female</th>
                                                                <th class="tdHeader">Total</th>
                                                            </tr>
                                                            <tr style="background-color: #C8C8C8">
                                                                <td width="5px">
                                                                    <img src="../assets/orsimages/plus.png" class="showDetails0"
                                                                        title="Click to show/hide Admin1" alt="Expand/Collapse Admin1" /></td>
                                                                <td style="width: 100px;">
                                                                    <div style="float: left; width: 100%; text-align: left;">
                                                                        <%#Eval("LocationName")%>
                                                                        <asp:HiddenField ID="hfCountryId" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                        <asp:HiddenField ID="hfCountryIndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                                                </td>
                                                                <td class="tdTable">

                                                                    <asp:TextBox ID="txtCountryTargetMaleCluster" runat="server"
                                                                        Text='<%#Eval("ClusterMale") %>' ToolTip="Cluster Country Target Total Male"
                                                                        CssClass="trgtCountryGenderMale" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtCountryTargetFemaleCluster" runat="server"
                                                                        Text='<%#Eval("ClusterFemale") %>' ToolTip="Cluster Country Target Total Female"
                                                                        CssClass="trgtCountryGenderMale txt" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtCountryTargetCluster" runat="server"
                                                                        Text='<%#Eval("ClusterTotal") %>' ToolTip="Cluster Country Target Total"
                                                                        CssClass="trgtCountryGenderMale txt" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtCountryTargetMaleProject" runat="server"
                                                                        Text='<%#Eval("ProjectMale") %>' ToolTip="Project Country Target Total Male"
                                                                        CssClass="trgtCountryGenderMale txt" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtCountryTargetFemaleProject" runat="server"
                                                                        Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Country Target Total Female"
                                                                        CssClass="trgtCountryGenderMale txt" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td class="tdTable">
                                                                    <asp:TextBox ID="txtCountryTargetProject" runat="server"
                                                                        Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Country Target Total"
                                                                        CssClass="trgtCountryGenderMale txt" Enabled="false" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr class="details0">
                                                                <td style="width: 100%;" colspan="8">
                                                                    <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1Gender_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin1Gender">
                                                                                <tr style="background-color: #EEEEEE" class="trAdmin1">
                                                                                    <td width="5px">
                                                                                        <img src="../assets/orsimages/plus.png" class="showDetails1"
                                                                                            title="Click to show/hide Admin2" /></td>
                                                                                    <td style="width: 150px;">

                                                                                        <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                        <asp:HiddenField ID="hfAdmin1Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                                        <asp:HiddenField ID="hfAdmin1IndicatorId" runat="server" Value='<%#Eval("IndicatorId")%>' />
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetMaleCluster" runat="server"
                                                                                            Text='<%#Eval("ClusterMale") %>' ToolTip="Cluster Admin1 Male Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetFemaleCluster" runat="server"
                                                                                            Text='<%#Eval("ClusterFemale") %>' ToolTip="Cluster Admin1 Female Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetCluster" runat="server"
                                                                                            Text='<%#Eval("ClusterTotal") %>' ToolTip="Cluster Admin1 Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetMaleProject" runat="server"
                                                                                            Text='<%#Eval("ProjectMale") %>' ToolTip="Project Admin1 Male Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetFemaleProject" runat="server"
                                                                                            Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Admin1 Female Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>
                                                                                    <td class="tdTable">
                                                                                        <asp:TextBox ID="txtAdmin1TargetProject" runat="server"
                                                                                            Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Admin1 Total"
                                                                                            CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                            Width="100%" Enabled="false"></asp:TextBox>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr class="details1">
                                                                                    <td></td>
                                                                                    <td style="width: 100%;" colspan="8">
                                                                                        <asp:Repeater ID="rptAdmin2" runat="server" OnItemDataBound="rptAdmin2_ItemDataBound">
                                                                                            <ItemTemplate>
                                                                                                <table style="margin: 0 auto; width: 100%;" border="0" class="imagetable tblAdmin2Gender">
                                                                                                    <tr>
                                                                                                        <td style="width: 150px">
                                                                                                            <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                                                                                            <asp:HiddenField ID="hfAdmin2Id" runat="server" Value='<%#Eval("LocationId")%>' />
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetMaleCluster" runat="server"
                                                                                                                Text='<%#Eval("ClusterMale") %>' ToolTip="Cluster Admin1 Male Total"
                                                                                                                CssClass="trgtAdmin2GenderMale" Style="text-align: right;"
                                                                                                                Width="100%" Enabled="false"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetFemaleCluster" runat="server"
                                                                                                                Text='<%#Eval("ClusterFemale") %>' ToolTip="Cluster Admin1 Female Total"
                                                                                                                CssClass="trgtAdmin2GenderMale" Style="text-align: right;"
                                                                                                                Width="100%" Enabled="false"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetCluster" runat="server"
                                                                                                                Text='<%#Eval("ClusterTotal") %>' ToolTip="Cluster Admin1 Total"
                                                                                                                CssClass="trgtAdmin2GenderMale" Style="text-align: right;"
                                                                                                                Width="100%" Enabled="false"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetMaleProject" runat="server"
                                                                                                                Text='<%#Eval("ProjectMale") %>' ToolTip="Project Admin1 Male Total"
                                                                                                                CssClass="trgtAdmin2GenderMale" Style="text-align: right;"
                                                                                                                Width="100%"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetFemaleProject" runat="server"
                                                                                                                Text='<%#Eval("ProjectFemale") %>' ToolTip="Project Admin1 Female Total"
                                                                                                                CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                                                Width="100%"></asp:TextBox>
                                                                                                        </td>
                                                                                                        <td class="tdTable">
                                                                                                            <asp:TextBox ID="txtAdmin2TargetProject" runat="server"
                                                                                                                Text='<%#Eval("ProjectTotal") %>' ToolTip="Project Admin1 Total"
                                                                                                                CssClass="trgtAdmin1GenderMale" Style="text-align: right;"
                                                                                                                Width="100%"></asp:TextBox>
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
