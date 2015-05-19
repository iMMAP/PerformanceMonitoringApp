<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KeyFiguresListing.aspx.cs" Inherits="SRFROWCA.KeyFigures.KeyFiguresListing" %>

<%@ Register Assembly="SRFROWCA" Namespace="SRFROWCA" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <script>
        $(function () {
            $(".imagetable").prepend('<thead><tr style="background-color:ButtonFace;"><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th><th colspan="3" style="text-align: center;">Total</th><th colspan="3" style="text-align: center;">In Need</th><th colspan="3" style="text-align: center;">Targeted</th> <th>&nbsp;</th> <th>&nbsp;</th><th>&nbsp;</th><th>&nbsp;</th></tr></thead>');
        });

        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker({
                dateFormat: "dd-mm-yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                    //LoadData();
                }
            });

            $("#<%=txtToDate.ClientID%>").datepicker({
                dateFormat: "dd-mm-yy",
                defaultDate: Date.now(),
                onSelect: function (selected) {
                    //LoadData();
                }
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                $("#<%=txtFromDate.ClientID%>").datepicker({
                    dateFormat: "dd-mm-yy",
                    defaultDate: Date.now(),
                    onSelect: function (selected) {
                        //LoadData();
                    }
                });

                $("#<%=txtToDate.ClientID%>").datepicker({
                    dateFormat: "dd-mm-yy",
                    defaultDate: Date.now(),
                    onSelect: function (selected) {
                        //LoadData();
                    }
                });
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a> </li>
            <li class="active">Key Figures List</li>
        </ul>

    </div>
    <div class="page-content">
        <table style="width: 100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportToExcel_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                       
                                        </button>

                                    </h6>
                                    <div style="float: right; margin-right: 10px;">
                                        <button runat="server" id="btnNew" style="height: 42px; width: 130px;" onserverclick="btnNew_ServerClick" class="btn btn-yellow" causesvalidation="false"
                                            title="Add Key Figure">
                                            Add Key Figure

                                        </button>
                                    </div>
                                </div>

                                <div class="widget-body">
                                    <div class="widget-main">
                                        <table border="0" style="width: 95%; margin: 0px 10px 0px 20px">
                                            <tr>
                                                <td>Country:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCountry" runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" Width="270" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="--- Select Country ---" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>From:</td>
                                                <td>
                                                    <asp:TextBox ID="txtFromDate" runat="server" Width="120px"></asp:TextBox>
                                                    To:
                                                    <asp:TextBox ID="txtToDate" runat="server" Width="120px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td>Category:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Width="270" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="--- Select Category ---" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </td>
                                                <td>Sub Category:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSubCategory" runat="server" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" Width="270" AutoPostBack="true">
                                                        <asp:ListItem Selected="True" Text="--- Select Category ---" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>

                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:CheckBox ID="cbShowAll" runat="server" Text="Show Only Latest Reported" OnCheckedChanged="cbShowAll_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td></td>
                                                <td align="right">
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click" />
                                                    <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-default" OnClick="btnReset_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>

        <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
            <asp:GridView ID="gvKeyFigures" runat="server" AutoGenerateColumns="False" OnRowCommand="gvKeyFigures_RowCommand"
                OnRowDataBound="gvKeyFigures_RowDataBound" AllowPaging="true" PageSize="50" OnPageIndexChanging="gvKeyFigures_PageIndexChanging"
                HeaderStyle-BackColor="ButtonFace" OnSorting="gvKeyFigures_Sorting" AllowSorting="true" AllowCustomPaging="true"
                CssClass="imagetable" Width="100%"
                DataKeyNames="AsOfDate,AsOfDate2,SubCategoryId,CategoryId,EmergencyLocationId,KeyFigureId"
                EmptyDataText="There are no key figures available!">
                <PagerSettings Mode="NumericFirstLast" />
                <HeaderStyle BackColor="Control"></HeaderStyle>
                <RowStyle CssClass="istrow" />
                <AlternatingRowStyle CssClass="altcolor" />
                <Columns>
                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>

                        <ItemStyle Width="2%"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" ItemStyle-Width="5%"></asp:BoundField>
                    <asp:BoundField DataField="SubCategory" HeaderText="Sub Category" SortExpression="SubCategory" ItemStyle-Width="7%"></asp:BoundField>
                    <asp:BoundField DataField="KeyFigure" HeaderText="Key Figure" SortExpression="KeyFigure" ItemStyle-Width="15%"></asp:BoundField>
                    <asp:BoundField DataField="AsOfDate" HeaderText="As Of Date" SortExpression="AsOfDate" ItemStyle-Width="8%"></asp:BoundField>
                    <asp:BoundField DataField="KeyFigureSource" HeaderText="Source" SortExpression="KeyFigureSource" HtmlEncode="false" ItemStyle-Width="15%"></asp:BoundField>
                    <asp:BoundField DataField="FromLocation" HeaderText="From Location" SortExpression="FromLocation" ItemStyle-Width="4%"></asp:BoundField>
                    <asp:BoundField DataField="ReportedLocation" HeaderText="Location" SortExpression="ReportedLocation" ItemStyle-Width="8%"></asp:BoundField>
                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalTotal" runat="server" Text=' <%# Eval("TotalTotal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Men" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalMen" runat="server" Text=' <%# Eval("TotalMen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Women" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalWomen" runat="server" Text=' <%# Eval("TotalWomen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblNeedTotal" runat="server" Text=' <%# Eval("NeedTotal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Men" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblNeedlMen" runat="server" Text=' <%# Eval("NeedMen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Women" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblNeedWomen" runat="server" Text=' <%# Eval("NeedWomen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTargetedTotal" runat="server" Text=' <%# Eval("TargetedTotal")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Men" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTargetedlMen" runat="server" Text=' <%# Eval("TargetedMen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Women" ItemStyle-Width="4%">
                        <ItemTemplate>
                            <asp:Label ID="lblTargetedWomen" runat="server" Text=' <%# Eval("TargetedWomen")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UpdatedBy" HeaderText="Reported By" />
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                CommandName="DeleteFigure" CommandArgument='<%# Eval("KeyFigureReportDetailId") %>' ToolTip="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                CommandName="EditFigure" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Edit Key Figure" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDuplicate" runat="server" ImageUrl="~/assets/orsimages/duplicate.png"
                                CommandName="DuplicateFigure" CommandArgument='<%# Container.DataItemIndex %>' ToolTip="Duplicate Key Figure" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
