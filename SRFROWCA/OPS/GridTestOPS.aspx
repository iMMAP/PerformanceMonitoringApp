<%@ Page Title="" Language="C#" MasterPageFile="~/OPS.Master" AutoEventWireup="true" CodeBehind="GridTestOPS.aspx.cs" Inherits="SRFROWCA.OPS.GridTestOPS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .altcolor {
            background-color: #fbfbfb;
        }

        .istrow {
            background: #ebf0f4;
        }


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

        .details1 {
            display: none;
        }

        .details0 {
            display: none;
        }

    </style>
    <script>
        $(function () {
            $('.showDetails1').click(function () {
                
                //alert($(this).parent().parent().parent().parent().attr('class'));
                //alert($(this).parent().parent().parent().parent().find('tr.details0').attr('class'));

                $(this).closest('table').find('.details0').toggle();
                //$(this).attr('src', ($(this).attr('src') == '../assets/orsimages/plus.png' ?
                //                                             '../assets/orsimages/minus.png' :
                //                                              '../assets/orsimages/plus.png'))
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <asp:Repeater ID="rptIndicators" runat="server" OnItemDataBound="rptIndicators_ItemDataBound">
                <HeaderTemplate>
                    <table style="margin: 0 auto; width: 100%;" border="1">
                        <tr>
                            <th style="width: 5%;" class="graycolor"></th>
                            <th style="width: 15%;" class="graycolor"></th>
                            <th style="width: 15%;" class="graycolor"></th>
                            <th style="width: 5%;" class="graycolor"></th>
                            <th style="width: 5%;" class="graycolor"></th>
                            <th style="width: 5%;" class="graycolor"></th>
                            <th class="tdTop" colspan="3">Cluster Target</th>
                            <th class="tdTop" colspan="3">Project Target</th>
                        </tr>
                        <tr>
                            <th style="width: 5%;" class="lightgraycolor">Obj</th>
                            <th style="width: 15%;" class="lightgraycolor">Activity</th>
                            <th style="width: 15%;" class="lightgraycolor">Indicator</th>
                            <th style="width: 5%;" class="lightgraycolor">Unit</th>
                            <th style="width: 5%;" class="lightgraycolor">Method</th>
                            <th style="width: 5%;" class="lightgraycolor">Location</th>
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
                    <table style="margin: 0 auto; width: 100%;" border="1">
                        <tr id="trIndTarget" runat="server">
                            <td style="width: 5%;">
                                <asp:Label ID="lblObjective" runat="server" Width="98%" Text='<%# Eval("Objective")%>'></asp:Label>
                                <asp:HiddenField ID="hfObjectiveId" runat="server" Value='<%# Eval("ObjectiveId")%>' />
                            </td>
                            <td style="width: 15%;">
                                <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("Activity") %>'></asp:Label>
                            </td>
                            <td style="width: 15%;">
                                <asp:Label ID="lblIndicator" runat="server" Text='<%# Eval("Indicator") %>'></asp:Label>
                                <asp:HiddenField ID="hfIndicatorId" runat="server" Value='<%# Eval("IndicatorId")%>' />
                            </td>
                            <td style="width: 5%;">
                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                            </td>
                            <td style="width: 5%;">
                                <asp:Label ID="lblCalMethod" runat="server" Text='<%# Eval("CalculationType") %>'></asp:Label>
                            </td>
                            <td style="width: 5%; text-align: left;">
                                <asp:Label ID="lblLocationName" runat="server" Width="98%" Text='<%# Eval("LocationName") %>'></asp:Label>
                                <asp:HiddenField ID="hfLocationId" runat="server" Value='<%# Eval("LocationId")%>' />
                                <asp:HiddenField ID="hfLocParentId" runat="server" Value='<%# Eval("LocationParentId")%>' />
                                <asp:HiddenField ID="hfLocTypeId" runat="server" Value='<%# Eval("LocationTypeId")%>' />
                                <img src="../assets/orsimages/plus.png" class="showDetails1" title="Click to show/hide Admin2" alt="show" />
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtTotalTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtTotalMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtTotalWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtNeedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtNeedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                            <td class="tdTable">
                                <asp:TextBox ID="txtNeedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=''></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
