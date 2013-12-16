<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities" Culture="auto"
    meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        #MainContent_cblLocations td
        {
            padding: 0 40px 0 0;
        }
    </style>
    <script type="text/javascript" src="../Scripts/jquery.kiketable.colsizable-1.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery.event.drag-1.4.min.js"></script>
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
            $(".numeric1").numeric();
            $('#gridSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });

            $(".chkShowHide").click(function () {
                if ($(this).is(':checked')) {
                    $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                        if ($(this).is(':checked')) {
                            $(this).parent().parent().show();
                        }
                        else {
                            $(this).parent().parent().hide();
                        }
                    })
                } else {
                    $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                        if ($(this).is(':checked')) {
                            $(this).parent().parent().show();
                        }
                        else {
                            $(this).parent().parent().show();
                        }

                    })
                }
            })

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
                        $(this).text(city1[1]);

                        if (j % 2 === 0) {
                            list += '<th colspan="2" style="width:100px; text-align:center;">' + city1[0] + '</th>';
                        }
                    }
                });

                $(".imagetable").prepend('<colgroup><col /><col /><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th style="width: 7px;">&nbsp;</th><th style="width: 45px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 60px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th><th style="width: 150px;">&nbsp;</th>' + list + '</tr></thead>');
            }

            $("#<%=gvActivities.ClientID %>").kiketable_colsizable({ minWidth: 50 })
        });


        function searchTable(inputVal) {
            var table = $('#<%=gvActivities.ClientID %>');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) {
                        $(row).show('fast', function () {

                            if ($(".chkShowHide").is(":Checked")) {
                                $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                                    if ($(this).is(':checked')) {
                                        $(this).parent().parent().show();
                                    }
                                    else {
                                        $(this).parent().parent().hide();
                                    }
                                })
                            }
                            else {
                                $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                                    if ($(this).is(':checked')) {
                                        $(this).parent().parent().show();
                                    }
                                })
                            }
                        });
                    }
                    else {
                        $(row).hide('fast', function () {

                            if ($(".chkShowHide").is(":Checked")) {
                                $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                                    if ($(this).is(':checked')) {
                                        $(this).parent().parent().hide();
                                    }
                                })
                            }
                            else {
                                $('#<%=gvActivities.ClientID %>').find("input[type='checkbox']").each(function () {
                                    if ($(this).is(':checked')) {
                                        $(this).parent().parent().show();
                                    }
                                })
                            }
                        });
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divMsg">
    </div>
    <div class="container">
        <div class="graybar">
            
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <td align="right">
                            <label>
                                <asp:Localize ID="locaEmergencyCaption" runat="server" meta:resourcekey="locaEmergencyCaptionResource1"
                                    Text="
                                Emergency:"></asp:Localize>
                                (<asp:Label ID="lblCountry" runat="server" meta:resourcekey="lblCountryResource1"></asp:Label>)
                            </label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlEmergency" runat="server" Width="350px" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlEmergencyResource1">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                InitialValue="0" Text="*" ControlToValidate="ddlEmergency" meta:resourcekey="rfvEmergencyResource1"></asp:RequiredFieldValidator>
                        </td>
                        <td align="right">
                            <label>
                                <asp:Localize ID="locaYearMonth" runat="server" meta:resourcekey="locaYearMonthResource1"
                                    Text="
                                Year/Month:"></asp:Localize></label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="100px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" meta:resourcekey="ddlYearResource1">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server" Width="100px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="True" 
                                meta:resourcekey="ddlMonthResource1">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer" style="clear: both;">
            </div>
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                Width="120px" CssClass="button_example" meta:resourcekey="btnSaveResource1" /></div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="False"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;"
                meta:resourcekey="btnOpenLocationsResource1" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div class="tablegrid">
        <table border="0" cellpadding="2" cellspacing="0" class="quicksearch2">
            <tr>
                <td width="250px">
                    <input type="checkbox" id="chkShowHide" class="chkShowHide" />
                    <asp:Localize ID="locaOnlyCheckCaption" runat="server" 
                        meta:resourcekey="locaOnlyCheckCaptionResource1" 
                        Text=" &lt;b&gt;Show Only Checked:&lt;/b&gt;"></asp:Localize>
                </td>
                <td>
                    <asp:Localize ID="locaSearchCaption" runat="server" 
                        meta:resourcekey="locaSearchCaptionResource1" 
                        Text=" &lt;b&gt;Search:&lt;/b&gt;"></asp:Localize>
                    <input type="text" id="gridSearch" class="grdSearch" style="width: 400px;" />
                </td>
                <td align="right">
                </td>
                <td style="width: 40%;">
                </td>
            </tr>
        </table>
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                HeaderStyle-BackColor="ButtonFace" DataKeyNames="ActivityDataId" CssClass="imagetable"
                Width="100%" OnRowDataBound="gvActivities_RowDataBound" meta:resourcekey="gvActivitiesResource1">
                <HeaderStyle BackColor="Control"></HeaderStyle>
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelected" runat="server" Checked='<%# bool.Parse(Eval("IsActive").ToString()) %>'
                                meta:resourcekey="chkSelectedResource1" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ClusterName" HeaderText="Cluster" ItemStyle-Width="50px"
                        meta:resourcekey="BoundFieldResource1">
                        <ItemStyle Width="50px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Objective" HeaderText="Objective" ItemStyle-Width="150px"
                        meta:resourcekey="BoundFieldResource2">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="HumanitarianPriority" HeaderText="Priority" ItemStyle-Wrap=" false"
                        ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource3">
                        <ItemStyle Wrap="False" Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Width="150px"
                        meta:resourcekey="BoundFieldResource4">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="DataName" HeaderText="Output Indicator" ItemStyle-Width="150px" meta:resourcekey="BoundFieldResource5">
                        <ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                Width="120px" CssClass="button_example" meta:resourcekey="btnSave2Resource1" /></div>
        <div class="buttonright">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" runat="server" BehaviorID="mpeAddActivity" TargetControlID="btnClientOpen"
                    PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
                    DynamicServicePath="" Enabled="True">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="600px" meta:resourcekey="pnlLocationsResource1">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    <asp:Localize ID="locaAdmin2LocationsCaption" runat="server" meta:resourcekey="locaAdmin2LocationsCaptionResource1"
                                        Text="
                                    Admin2 Locations"></asp:Localize>
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="4" RepeatDirection="Horizontal"
                                                        CssClass="columnGap" meta:resourcekey="cblLocationsResource1">
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
            </td>
        </tr>
    </table>
</asp:Content>
