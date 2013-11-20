<%@ Page Title="" Language="C#" MasterPageFile="~/ops.Master" AutoEventWireup="true"
    CodeBehind="OPSDataEntry.aspx.cs" Inherits="SRFROWCA.OPS.OPSDataEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery.numeric.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        var needToConfirm = true;

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            if (needToConfirm)
                return "";
        }
    </script>
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
    </script>
    <style type="text/css">
        .ModalPopupBG1
        {
            background-color: #446633;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup1
        {
            display: block;
            top: 10px;
            left: 0;
            width: 600px;
            height: 300px;
            padding: 5px;
            margin: 10px;
            z-index: 10;
            font: 12px Verdana, sans-serif;
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
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
                $(".imagetable").prepend('<colgroup><col /><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th style="width: 5px;">&nbsp;</th><th style="width: 200px;"><%:OPSClusterName %></th><th style="width: 200px;">&nbsp;</th><th style="width: 200px;">&nbsp;</th>' + list + '</tr></thead>');

            }

            $("#<%=gvActivities.ClientID %>").kiketable_colsizable({ minWidth: 30 })
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
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="true" Width="120" CssClass="button_example" /></div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="false"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;" />
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <div class="containerOPS">
        <div class="graybar">
            Select Your Options To Report On
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="margin: 0 auto; width: 100%">
                    <tr>
                        <td>
                            <label>
                                Strategic Objectives:</label>
                        </td>
                        <td>
                            <a href="../webform5.aspx">Back</a>
                            <asp:DropDownList ID="ddlStrObjectives" Width="950px" runat="server" OnSelectedIndexChanged="ddlStrObjectives_SelectedIndexChanged"
                                AutoPostBack="true" onchange="needToConfirm = false;">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Specific Objectives:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSpcObjectives" Width="950px" runat="server" OnSelectedIndexChanged="ddlSpcObjectives_SelectedIndexChanged"
                                AutoPostBack="true" onchange="needToConfirm = false;">
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
    <div class="tablegrid">
        <table border="0" cellpadding="2" cellspacing="0" class="quicksearch2">
            <tr>
                <td width="250px">
                    <input type="checkbox" id="chkShowHide" class="chkShowHide" />
                    <b>Show Only Checked:</b>
                </td>
                <td>
                    <b>Search:</b>
                    <input type="text" id="gridSearch" class="grdSearch" style="width: 400px;" />
                </td>
                <td align="right">
                </td>
                <td style="width: 40%;">
                </td>
            </tr>
        </table>
        <div id="scrolledGridView" style="overflow-x: auto; width: 100%">
            <asp:GridView ID="gvActivities" runat="server" AllowPaging="False" AllowSorting="False"
                AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" HeaderStyle-BackColor="ButtonFace"
                DataKeyNames="ActivityDataId" CssClass="imagetable" Width="100%" OnRowDataBound="gvActivities_RowDataBound">
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSelected" runat="server" Checked='<%#bool.Parse(Eval("IsActive").ToString())%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IndicatorName" HeaderText="Indicator" ItemStyle-Wrap=" false"
                        ItemStyle-Width="350px" />
                    <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-Wrap="false"
                        ItemStyle-Width="350px" />
                    <asp:BoundField DataField="DataName" HeaderText="Data" ItemStyle-Wrap="false" ItemStyle-Width="350px" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="buttonsdiv">
        <div class="savebutton">
            <asp:Button ID="btnSave2" runat="server" OnClick="btnSave_Click" Text="Save" OnClientClick="needToConfirm = false;"
                CausesValidation="true" Width="120px" CssClass="button_example" /></div>
        <div class="buttonright">
        </div>
        <div class="spacer" style="clear: both;">
        </div>
    </div>
    <table>
        <tr>
            <td>
                <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
                <asp:ModalPopupExtender ID="mpeAddActivity" BehaviorID="mpeAddActivity" runat="server"
                    TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="ModalPopupBG1">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="700px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="HellowWorldPopup1">
                                <table width="50%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="63" colspan="3" bgcolor="#FFFFFF" style="border-left: #9db7df  4px solid;
                                            border-top: #9db7df  4px solid; border-right: #9db7df  4px solid; border-bottom: #9db7df  4px solid">
                                            <table border="0" style="margin: auto; background-color: Gray">
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Label ID="lblLocationLevelOfCountry" runat="server" Text="" BackColor="White"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: ButtonFace;">
                                                    <td>
                                                        Locations:
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td style="background-color: ButtonFace;">
                                                        Selected Locations:
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 45%">
                                                        <asp:ListBox ID="lstLocations" runat="server" Height="180px" Width="315px" SelectionMode="Multiple">
                                                        </asp:ListBox>
                                                    </td>
                                                    <td class="TitleCellBackgroud" align="center">
                                                        <%--<asp:Button ID="btnAddAll" runat="server" Text="&gt;&gt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnAddAll_Click" />--%>
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnAdd" runat="server" Text="&gt;" Height="30px" Width="50px" CausesValidation="false"
                                                            OnClick="btnAdd_Click" />
                                                        <br />
                                                        <br />
                                                        <asp:Button ID="btnRemove" runat="server" Text="&lt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnRemove_Click" />
                                                        <br />
                                                        <br />
                                                        <%--<asp:Button ID="btnRemoveAll" runat="server" Text="&lt;&lt;" Height="30px" Width="50px"
                                                            CausesValidation="false" OnClick="btnRemoveAll_Click" />--%>
                                                    </td>
                                                    <td style="width: 45%">
                                                        <asp:ListBox ID="lstSelectedLocations" runat="server" Height="180px" Width="315px"
                                                            SelectionMode="Multiple"></asp:ListBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" align="center">
                                                        <asp:Button ID="btnClose" runat="server" Text="Close" Width="300px" Height="40px"
                                                            CausesValidation="false" OnClientClick="needToConfirm = false;" />
                                                        <%--<asp:Button ID="btnGetReports" runat="server" Text="Get Location Reports" OnClick="btnGetReports_Click"
                                                            Width="300px" Height="40px" CausesValidation="false" />--%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOrgMessage" runat="server" ViewStateMode="Disabled"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
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
