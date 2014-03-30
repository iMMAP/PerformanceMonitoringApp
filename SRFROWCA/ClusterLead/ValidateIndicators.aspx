<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ValidateIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.ValidateIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li><a href="ValidateReportList.aspx">Validate</a></li>
            <li class="active">Validate Indicators</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        Report Details
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <div class="col-sm-12">
                                                        <label class="col-sm-1 control-label no-padding-right" for="form-input-readonly">
                                                            Project:
                                                        </label>
                                                        <div class="col-sm-11">
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text=""></asp:Label>
                                                            <%--<input readonly="" type="text" class="col-xs-10 col-sm-11" id="form-input-readonly"
                                                                value="This text field is readonly!" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12">
                                                    <div class="col-sm-6">
                                                        <label class="col-sm-2 control-label no-padding-right" for="form-input-readonly">
                                                            Organization:
                                                        </label>
                                                        <div class="col-sm-10">
                                                            <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12">
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Updated By:
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Updated On:
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblUpdatedOn" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <label class="col-sm-3 control-label no-padding-right" for="form-input-readonly">
                                                            Reporting Period:
                                                        </label>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lblReportingPeriod" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>
                                        Validate Reported Indicators
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-toolbox">
                                        <div class="btn-toolbar">
                                            <div class="btn-group">
                                                <button runat="server" id="btnApprove" onserverclick="btnApprove_Click" class="btn btn-sm btn-success">
                                                    <i class="icon-ok bigger-110"></i>Approve
                                                </button>
                                                <%--<button runat="server" id="btnReject" onserverclick="btnReject_Click" class="btn btn-sm btn-danger">
                                                    <i class="icon-remove bigger-110"></i>Reject
                                                </button>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                                    <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                                        DataKeyNames="ReportDetailId" CssClass="imagetable" Width="100%" OnRowCommand="gvIndicators_RowCommand">
                                                        <HeaderStyle BackColor="Control"></HeaderStyle>
                                                        <RowStyle CssClass="istrow" />
                                                        <AlternatingRowStyle CssClass="altcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Select" ItemStyle-Width="40px">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkApproved" runat="server" Checked='<%# Eval("IsApproved") %>'
                                                                        CssClass="testcb" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ActivityName" HeaderText="Activity" SortExpression="ActivityName" />
                                                            <asp:BoundField DataField="Indicator" HeaderText="Indicator" SortExpression="Indicator" />
                                                            <asp:BoundField DataField="LocationName" HeaderText="Location" SortExpression="Location" />
                                                            <asp:BoundField DataField="AnnualTarget" HeaderText="AnnualTarget" SortExpression="AnnualTarget" />
                                                            <asp:BoundField DataField="Achieved" HeaderText="Achieved" SortExpression="Achieved" />
                                                        </Columns>
                                                    </asp:GridView>
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
    </div>
</asp:Content>
