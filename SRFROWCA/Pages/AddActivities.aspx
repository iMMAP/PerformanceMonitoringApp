<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddActivities.aspx.cs" Inherits="SRFROWCA.Pages.AddActivities" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style>
       #MainContent_cblLocations td {
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

                $(".imagetable").prepend('<colgroup><col /><col /><col /><col /><col /></colgroup><thead><tr style="background-color:ButtonFace;"><th style="width: 30px;">&nbsp;</th><th style="width: 50px;">&nbsp;</th><th style="width: 250px;">&nbsp;</th><th style="width: 250px;">&nbsp;</th><th style="width: 250px;">&nbsp;</th>' + list + '</tr></thead>');
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
            Select Your Options To Report On
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table border="0" cellpadding="0" width="100%">
                    <tr>
                        <td>
                            <label>
                                Country:</label>
                        </td>
                        <td>
                            <asp:Label ID="lblCountry" runat="server" Text="" Width="100px"></asp:Label>
                        </td>
                        <td>
                            <label>
                                Organization:</label>
                        </td>
                        <td>
                            <asp:Label ID="lblOrganization" runat="server"></asp:Label>
                        </td>
                        <td>
                            <label>
                                Emergency:</label>
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlEmergency" runat="server" Width="250px" OnSelectedIndexChanged="ddlEmergency_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                InitialValue="0" Text="*" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Year:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYear" runat="server" Width="100px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                                <asp:ListItem Text="2010" Value="6"></asp:ListItem>
                                <asp:ListItem Text="2011" Value="7"></asp:ListItem>
                                <asp:ListItem Text="2012" Value="8"></asp:ListItem>
                                <asp:ListItem Text="2013" Value="9"></asp:ListItem>
                                <asp:ListItem Text="2014" Value="20"></asp:ListItem>
                                <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvYear" runat="server" ErrorMessage="Select Year"
                                InitialValue="0" Text="*" ControlToValidate="ddlYear"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                Month:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMonth" runat="server" Width="100px" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged"
                                onchange="needToConfirm = false;" AutoPostBack="true">
                                <asp:ListItem Text="Jan" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Feb" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Mar" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Apr" Value="4"></asp:ListItem>
                                <asp:ListItem Text="May" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Jun" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Jul" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Aug" Value="8"></asp:ListItem>
                                <asp:ListItem Text="Sep" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Oct" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Nov" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Dec" Value="12"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ErrorMessage="Select Month"
                                InitialValue="0" Text="*" ControlToValidate="ddlMonth"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <label>
                                Office:</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOffice" runat="server" Width="250px" AutoPostBack="true"
                                onchange="needToConfirm = false;" OnSelectedIndexChanged="ddlOffice_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvOffice" runat="server" ErrorMessage="Select Office"
                                InitialValue="0" Text="*" ControlToValidate="ddlOffice"></asp:RequiredFieldValidator>
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
                CausesValidation="true" Width="120" CssClass="button_example" /></div>
        <div class="buttonright">
            <asp:Button ID="btnOpenLocations" runat="server" Text="Locations" CausesValidation="false"
                CssClass="button_location" OnClick="btnLocation_Click" OnClientClick="needToConfirm = false;" />
        </div>
        <div class="spacer" style="clear: both;">
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
                    <asp:BoundField DataField="ClusterName" HeaderText="Cluster" ItemStyle-Width="50px"
                        ItemStyle-Wrap="false" />
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
                    TargetControlID="btnClientOpen" PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground">
                </asp:ModalPopupExtender>
                <asp:Panel ID="pnlLocations" runat="server" Width="600px">
                    <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="containerPopup">
                                <div class="graybar">
                                    Admin2 Locations
                                </div>
                                <div class="contentarea">
                                    <div class="formdiv">
                                        <table border="0" style="margin: 0 auto;">
                                            <tr>
                                                <td>
                                                    <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="columnGap">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnClose" runat="server" Text="Close" Width="120px" CssClass="button_location"
                                                        CausesValidation="false" OnClientClick="needToConfirm = false;" />
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
