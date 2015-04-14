<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddKeyFigureControl.ascx.cs" Inherits="SRFROWCA.KeyFigures.AddKeyFigureControl" %>


<asp:Repeater ID="rptKeyFigure" runat="server" >
    <HeaderTemplate>
        <table id="tblKeyFigure" style="margin: 0 auto; width: 100%;" border="1" runat="server">
            <tr>
                <th style="width: 35%;" class="graycolor"></th>
                <th style="width: 15%;" class="graycolor"></th>
                <th class="tdTop" colspan="3">
                    <asp:Literal ID="ltrlNeed" runat="server" Text="Need"></asp:Literal></th>
                <th class="tdTop" colspan="3">
                    <asp:Literal ID="ltrlTargated" runat="server" Text="Targated"></asp:Literal></th>
                <th class="tdTop" colspan="3">
                    <asp:Literal ID="ltrlReached" runat="server" Text="Reached"></asp:Literal></th>
            </tr>
            <tr>
                <th style="width: 35%;" class="lightgraycolor">
                    <asp:Literal ID="ltrlIndicator" runat="server" Text="Indicator"></asp:Literal></th>
                <th style="width: 15%;" class="lightgraycolor">
                    <asp:Literal ID="ltrlLocation" runat="server" Text="Location"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlNeedTotal" runat="server" Text="Total"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlNeedMen" runat="server" Text="Men"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlNeedWomen" runat="server" Text="Women"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlTargatedTotal" runat="server" Text="Total"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlTargatedMen" runat="server" Text="Men"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlTargatedWomen" runat="server" Text="Women"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlReachedTotal" runat="server" Text="Total"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlReachedMen" runat="server" Text="Men"></asp:Literal></th>
                <th class="tdHeader">
                    <asp:Literal ID="ltrlReachedWomen" runat="server" Text="Women"></asp:Literal></th>
            </tr>
        </table>
    </HeaderTemplate>
    <ItemTemplate>
        <table style="margin: 0 auto; width: 100%;" border="1" class="tblMain">
            <tr>
                <td style="width: 35%;">
                    <asp:Label ID="lblKeyFigureIndicator" runat="server" Width="98%" Text='<%# Eval("KeyFigureIndicator")%>'></asp:Label>
                    <asp:HiddenField ID="hfKeyFigureIndicatorId" runat="server" Value='<%# Eval("KeyFigureIndicatorId")%>' />
                    <asp:HiddenField ID="hfKeyFigureReportId" runat="server" Value='<%# Eval("KeyFigureReportId")%>' />
                </td>
                <td style="width: 15%;">
                    <asp:Label ID="lblCountry" runat="server" Width="98%" Text='<%# Eval("CountryName")%>'></asp:Label>
                    <asp:HiddenField ID="hfLocationId" runat="server" Value='<%# Eval("LocationId")%>' />
                    <span>Admin 1<img src="../assets/orsimages/plus.png" id="imgAdmin1" runat="server" class="showDetails1" /></span>
                    <%--<input type="button" id="Admin1" class="showDetails1" title="btn" value="Admin1" name="Admin1" runat="server"/>--%>
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
                <td class="tdTable">
                    <asp:TextBox ID="txtReachedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedTotal")%>'></asp:TextBox>
                </td>
                <td class="tdTable">
                    <asp:TextBox ID="txtReachedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedMen")%>'></asp:TextBox>
                </td>
                <td class="tdTable">
                    <asp:TextBox ID="txtReachedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedWomen")%>'></asp:TextBox>
                </td>
            </tr>
            <tr class="admin1">
                <td></td>
                <td colspan="10">
                    <asp:Repeater ID="rptAdmin1" runat="server">
                        <ItemTemplate>
                            <table style="margin: 0 auto; width: 100%;" border="1" class="sample">
                                <tr>
                                    <td style="width: 15%;">
                                        <div style="float: left; width: 80px; text-align: right; margin-top: 5px;"><%#Eval("LocationName")%>&nbsp;</div>
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
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtReachedTotal" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedTotal")%>'></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtReachedMen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedMen")%>'></asp:TextBox>
                                    </td>
                                    <td class="tdTable">
                                        <asp:TextBox ID="txtReachedWomen" runat="server" CssClass="itemWidth numeric1" MaxLength="9" Text=' <%# Eval("ReachedWomen")%>'></asp:TextBox>
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


