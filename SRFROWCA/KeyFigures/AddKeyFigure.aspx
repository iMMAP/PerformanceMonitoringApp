<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddKeyFigure.aspx.cs" Inherits="SRFROWCA.KeyFigures.AddKeyFigure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .altcolor {
            background-color: #fbfbfb;
        }

        .istrow {
            background: #ebf0f4;
        }
    </style>
    <style>
        .itemWidth {
            width: 100%;
            align-self: center;
        }

        .displaynone {
            display: none;
        }

        .tdTable {
            width: 5%;
        }

        .tdTop {
            width: 15%;
            text-align: center;
            background-color: gray;
        }

        .tdHeader {
            width: 5%;
            text-align: center;
            background-color: lightgray;
        }

        .graycolor {
            text-align: center;
            background-color: gray;
        }

        .lightgraycolor {
            text-align: center;
            background-color: lightgray;
        }
    </style>
    <script>

        var needToConfirm = true;

        window.onbeforeunload = confirmExit;
        function confirmExit() {
            var ctl = document.getElementById('__EVENTTARGET').value;

            if (ctl.indexOf("LanguageEnglish") != -1 || ctl.indexOf("LanguageFrench") != -1) {
                __EVENTTARGET.value = '';
                needToConfirm = false;
            }
            if (needToConfirm)
                return "Leave this page If you don't have any unsaved changes OR Stay on the page and save your changes before leaving the page!";
        }

        $(document).ready(function () {
            var openImgUrl = '~/assets/orsimages/edit16.png',
        closeImgUrl = 'http://www.70hundert.de/images/toggle-close.jpg';
            $(".admin1").hide();
            $(document).on("click", ".showDetails1", function () {
                $(this).closest('tr').next('tr').fadeToggle();
                $(this).text($(this).text() == 'Show Admin1' ? 'Hide Admin1' : 'Show Admin1');
            });
            $(".numeric1").numeric();
            $("table.tblMain tr:odd").addClass("altcolor");
            $("table.tblMain tr:odd").addClass("istrow");

            $("table.sample tr:even").addClass("altcolor");
            $("table.sample tr:odd").addClass("istrow");
        });
    </script>
    <script src="../assets/orsjs/jquery.numeric.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker({
                dateFormat: "mm/dd/yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                }
            });
        });

        function ShowOther(obj) {
            if ($(obj).val() == 'Other') {
                $("[id$=txtOther]").removeAttr("disabled");
            } else {
                $("[id$=txtOther]").attr("disabled", "disabled");
            }

        }
        function LoadData() {
            $("[id$=btnClick]").click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add/Edit Key Figure</li>
        </ul>
        <!-- .breadcrumb -->
    </div>

    <div class="page-content">
        <div id="divMsg"></div>
        <div class="alert2 alert-block alert-info">
            <h6>
                <asp:Localize ID="locMessageForUser" runat="server" Text="Select Country, Category & Sub-Category to report on Key Figure"></asp:Localize></h6>
        </div>
        <table border="0" style="margin: 0 auto; width: 100%">
            <tr>
                <td>As Of Date:*</td>
                <td colspan="5">
                    <asp:TextBox ID="txtFromDate" runat="server" Width="150px"></asp:TextBox>
                    <label>(MM/dd/yyyy)</label>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server"
                        ControlToValidate="txtFromDate"
                        ErrorMessage="Required."
                        ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Country:*</td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="150px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" AutoPostBack="true" onchange="needToConfirm = false;">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator></td>
                <td align="right">Category:*</td>
                <td>
                    <asp:DropDownList ID="ddlCategory" runat="server" Width="250px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" onchange="needToConfirm = false;">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                </td>
                <td align="right">Sub Category:*</td>
                <td>
                    <asp:DropDownList ID="ddlSubCategory" runat="server" Width="250px" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" AutoPostBack="true" onchange="needToConfirm = false;">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlSubCategory"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <hr />
        <table border="0" style="margin: 0 auto; width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="needToConfirm = false;"
                        OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
        <div>
        </div>
        <table>
            <tr>
                <th id="tst2" runat="server"></th>
                <td id="tst" runat="server"></td>
            </tr>
        </table>
        <asp:Repeater ID="rptKeyFigure" runat="server" OnItemDataBound="rptKeyFigure_ItemDataBound">
            <HeaderTemplate>
                <table id="tblKeyFigure" style="margin: 0 auto; width: 100%;" border="1" runat="server">
                    <tr>
                        <th style="width: 20%;" class="graycolor"></th>
                        <th style="width: 15%;" class="graycolor"></th>
                        <th class="graycolor" id="thFromLocTop" runat="server"></th>
                        <th style="width: 15%;" class="graycolor"></th>
                        <th class="tdTop" colspan="3">Total</th>
                        <th class="tdTop" colspan="3">In Need</th>
                        <th class="tdTop" colspan="3">Targated</th>
                    </tr>
                    <tr>
                        <th style="width: 20%;" class="lightgraycolor">Key Figure</th>
                        <th style="width: 15%;" class="lightgraycolor">Source</th>
                        <th class="lightgraycolor" id="thFromLoc" runat="server">From Location</th>
                        <th style="width: 15%;" class="lightgraycolor">Location</th>
                        <th class="tdHeader">Total</th>
                        <th class="tdHeader">Men</th>
                        <th class="tdHeader">Women</th>
                        <th class="tdHeader">Total</th>
                        <th class="tdHeader">Men</th>
                        <th class="tdHeader">Women</th>
                        <th class="tdHeader">Total</th>
                        <th class="tdHeader">Men</th>
                        <th class="tdHeader">Women</th>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table style="margin: 0 auto; width: 100%;" border="1" class="tblMain">
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label ID="lblKeyFigureIndicator" runat="server" Width="98%" Text='<%# Eval("KeyFigureIndicator")%>'></asp:Label>
                            <asp:HiddenField ID="hfKeyFigureIndicatorId" runat="server" Value='<%# Eval("KeyFigureIndicatorId")%>' />
                            <asp:HiddenField ID="hfKeyFigureReportId" runat="server" Value='<%# Eval("KeyFigureReportId")%>' />
                        </td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txtKFSouce" CssClass="itemWidth" runat="server" Text='<%# Eval("KeyFigureSource")%>'></asp:TextBox>
                        </td>
                        <td id="tdFromLocTop" runat="server">
                            <asp:TextBox ID="txtFromLocation" runat="server" CssClass="itemWidth" MaxLength="50" Text='<%# Eval("FromLocation")%>'></asp:TextBox>
                        </td>
                        <td style="width: 15%; text-align: left;">
                            <asp:Label ID="lblCountry" runat="server" Width="98%" Text='<%# Eval("CountryName")%>'></asp:Label>
                            <asp:HiddenField ID="hfLocationId" runat="server" Value='<%# Eval("LocationId")%>' />
                            <span class="showDetails1" style="font-size: smaller; color: blue; cursor: pointer;">Show Admin1</span>
                            <%--<input type="button" id="Admin1" class="showDetails1" title="btn" value="Admin1" name="Admin1" runat="server"/>--%>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTotalTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalTotal")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTotalMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalMen")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTotalWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalWomen")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtNeedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text='<%# Eval("NeedTotal")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtNeedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("NeedMen")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtNeedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("NeedWomen")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTargetedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedTotal")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTargetedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedMen")%>'></asp:TextBox>
                        </td>
                        <td class="tdTable">
                            <asp:TextBox ID="txtTargetedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedWomen")%>'></asp:TextBox>
                        </td>

                    </tr>
                    <tr class="admin1">
                        <td></td>
                        <td colspan="11">
                            <asp:Repeater ID="rptAdmin1" runat="server" OnItemDataBound="rptAdmin1_ItemDataBound">
                                <ItemTemplate>
                                    <table style="margin: 0 auto; width: 100%;" border="1" class="sample">
                                        <tr>
                                            <td style="width: 15%;">
                                                <asp:TextBox ID="txtKFSouce" CssClass="itemWidth" runat="server" Text='<%# Eval("KeyFigureSource")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable" id="tdFromLoc" runat="server">
                                                <asp:TextBox ID="txtFromLocation" runat="server" CssClass="itemWidth" MaxLength="50" Text='<%# Eval("FromLocation")%>'></asp:TextBox>
                                            </td>
                                            <td style="width: 15%; text-align: left;">
                                                <div style="float: left; width: 100%; text-align: left;"><%#Eval("LocationName")%></div>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalTotal")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalMen")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTotalWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TotalWomen")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text='<%# Eval("NeedTotal")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("NeedMen")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtNeedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("NeedWomen")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTargetedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedTotal")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTargetedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedMen")%>'></asp:TextBox>
                                            </td>
                                            <td class="tdTable">
                                                <asp:TextBox ID="txtTargetedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("TargetedWomen")%>'></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>

                                    <asp:HiddenField runat="server" ID="hfLocationId" Value='<%# Eval("LocationId") %>' />
                                </ItemTemplate>
                            </asp:Repeater>

                        </td>

                    </tr>
                </table>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
