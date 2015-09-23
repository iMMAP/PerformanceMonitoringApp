<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.IndicatorListing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .modalDialog {
            width: 500px;
            position: relative;
            margin: 10% auto;
            padding: 5px 20px 13px 20px;
            border-radius: 2px;
            background: #ffffff;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-content">

        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        <button runat="server" id="btnExportToExcel" onserverclick="btnExportExcel_Click" class="btn btn-yellow" causesvalidation="false"
                                            title="Excel">
                                            <i class="icon-download"></i>Excel
                                       
                                        </button>

                                        <asp:Button ID="btnAddActivityAndIndicators" runat="server" Text="Add Activity & Indicators (Framework 2016)" CausesValidation="false"
                                            CssClass="btn btn-yellow pull-right" OnClick="btnAddActivityAndIndicators_Click" Style="margin-right: 5px;" />
                                        <asp:Button ID="btnMigrate2016" runat="server" Text="Migrate 2015 Framework To 2016" CausesValidation="false"
                                            CssClass="btn btn-danger pull-right" OnClick="btnMigrate2016_Click" />
                                    </h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" style="width: 100%;">
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Cluster:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <label>Country:</label></td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" meta:resourcekey="ddlCountryResource1">
                                                                </asp:DropDownList></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Objective:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlObjective" runat="server" AppendDataBoundItems="true" CssClass="width-80">
                                                                    <asp:ListItem Text="All" Value="-1" Selected="True"></asp:ListItem>

                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="width-20">
                                                                <label>
                                                                    Activity:</label>
                                                            </td>
                                                            <td class="width-30">

                                                                <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-80">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="width-20">
                                                                <label>
                                                                    Indicator Name:</label>
                                                            </td>

                                                            <td class="width-30">
                                                                <asp:TextBox ID="txtActivityName" runat="server" CssClass="width-80"></asp:TextBox>
                                                            </td>

                                                            <td class="width-20">
                                                                <label>
                                                                    Year:</label>
                                                            </td>
                                                            <td class="width-30">
                                                                <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChnaged">
                                                                    <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                                                    <asp:ListItem Text="2015" Value="11"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>

                                                        </tr>

                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="4" style="padding-top: 10px;">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch2_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                                <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <div id="divMsg"></div>
        <div id="divMissingTarget" runat="server" class="alert alert-block alert-danger" visible="false">
            <button type="button" class="close" data-dismiss="alert">
                <i class="ace-icon fa fa-times"></i>
            </button>

            <i class="ace-icon fa fa-check green"></i>
            Please note that the Inidcators higlighted red do not have targets specified.<br />
            You will not be able to publish this Framework until you provide targets for all the Indicators.
        </div>

        <div class="tablegrid">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" Width="100%"
                    PagerSettings-Mode="NumericFirstLast" OnRowCommand="gvActivity_RowCommand" OnRowDataBound="gvActivity_RowDataBound"
                    PagerSettings-Position="Bottom" DataKeyNames="ActivityId,IndicatorDetailId,IndicatorId,IsMigrated"
                    CssClass="imagetable" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging"
                    PageSize="70" ShowHeaderWhenEmpty="true" EmptyDataText="Your filter criteria does not match any indicator!">
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Country" HeaderText="Country" SortExpression="Country" ItemStyle-Width="80px"></asp:BoundField>
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ShortObjective" HeaderText="Objective" SortExpression="ShortObjective" ItemStyle-Width="90px" />
                        <asp:TemplateField HeaderText="Active Activity" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsActivityActive" runat="server" Checked='<%# Eval("IsActivityActive") %>' OnCheckedChanged="cbActivityActive_Changed" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity" />
                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                        <asp:TemplateField HeaderText="Active Indicator" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Eval("IsActive") %>' OnCheckedChanged="cbActive_Changed" AutoPostBack="true" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
                        <asp:TemplateField ItemStyle-Width="4%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>">
                            <ItemTemplate>
                                <asp:Label ID="lblCalcMethod" ToolTip="some text here" runat="server" Text=' <%# Eval("CalculationType")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Country Target" SortExpression="IndicatorTarget">
                            <ItemTemplate>
                                <asp:Label ID="lblIndTarget" runat="server" Text='<%# Eval("IndicatorTarget") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Edit">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/assets/orsimages/edit16.png"
                                    CommandName="EditActivity" CommandArgument='<%# Eval("ActivityId") %>' ToolTip="Edit Indicator" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" HeaderText="Del">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                    CommandName="DeleteInd" CommandArgument='<%# Eval("IndicatorDetailId") %>' ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource5">
                            <ItemTemplate>
                                <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("EmergencyLocationId") %>' meta:resourcekey="lblCountryIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" meta:resourcekey="TemplateFieldResource6">
                            <ItemTemplate>
                                <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("EmergencyClusterId") %>' meta:resourcekey="lblClusterIDResource1"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="ButtonFace" />
                </asp:GridView>
            </div>
        </div>
        <input type="button" id="btnClientOpen" runat="server" style="display: none;" />
        <asp:Panel ID="Panel1" runat="server" CssClass="modalDialog" Style="display: none" Width="600">
            <div>
                <div>
                    <h4>
                        <asp:Localize ID="localDisableConfirmBox" runat="server" Text="Are you sure you want to disable this indicator?"></asp:Localize>
                    </h4>
                </div>
                <br />
                <asp:Label ID="lblProjectsCaption" runat="server" Text="Following projects are using this indicator. Indicator will be removed from these projects:" Visible="false"></asp:Label>
                <br />
                <br />
                <b>
                    <asp:Label ID="lblProjectUsingIndicator" runat="server" Text=""></asp:Label></b>
                <br />
                <div align="center">
                    <asp:Button ID="OkButton" runat="server" Text="OK" OnClick="btnOK_Click" class="btn btn-primary" />
                    <asp:Button ID="CancelButton" runat="server" Text="Cancel" OnClick="btnCancel_Click" class="btn btn-default" />
                </div>
            </div>
        </asp:Panel>


        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
            TargetControlID="btnClientOpen"
            PopupControlID="Panel1"
            BackgroundCssClass="modalpopupbackground"
            DropShadow="true" />
    </div>
</asp:Content>
