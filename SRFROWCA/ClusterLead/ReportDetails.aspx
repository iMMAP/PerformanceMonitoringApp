<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportDetails.aspx.cs" Inherits="SRFROWCA.ClusterLead.ReportDetails" MasterPageFile="~/Site.Master" %>

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
            <li><a href="ProjectsListing.aspx">Projects</a></li>
            <li><a href="ProjectDetails.aspx?pid=<%=ProjectID%>">Project Details</a></li>
            <li class="active">Report Details</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6>Report Details
                                    </h6>
                                    <div class="widget-toolbar">
                                        <a href="#" data-action="collapse"><i class="icon-chevron-down"></i></a>
                                    </div>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="form-group">
                                            <div class="row">
                                                <table>
                                                    <tr>
                                                        <td style="width: 30%; padding-left: 20px;">
                                                            <label class=" control-label no-padding-right" for="form-input-readonly">
                                                                Project:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblProjectTitle" runat="server" Text=""></asp:Label>
                                                            <%--<input readonly="" type="text" class="col-xs-10 col-sm-11" id="form-input-readonly"
                                                                value="This text field is readonly!" />--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; padding-left: 20px;">
                                                            <label class=" control-label no-padding-right" for="form-input-readonly">
                                                                Organization:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblOrganization" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; padding-left: 20px;">
                                                            <label class=" control-label no-padding-right" for="form-input-readonly">
                                                                Updated By:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUpdatedBy" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; padding-left: 20px;">
                                                            <label class="  control-label no-padding-right" for="form-input-readonly">
                                                                Updated On:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblUpdatedOn" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 30%; padding-left: 20px;">
                                                            <label class="  control-label no-padding-right" for="form-input-readonly">
                                                                Reporting Period:
                                                       
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblReportingPeriod" runat="server" Text=""></asp:Label>
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

                    <div class="col-xs-12 col-sm-12 ">
                        <div class="widget-box">

                           <%-- <div class="widget-body">
                              
                                <div class="widget-main">
                                    <div class="form-group">--%>
                                        <div class="row">
                                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                                    DataKeyNames="ReportId,ActivityDataId,ReportDetailId" CssClass="table table-striped table-bordered table-hover" Width="100%" OnRowDataBound="gvIndicators_RowDataBound">
                                                    <HeaderStyle BackColor="Control"></HeaderStyle>

                                                    <Columns>
                                                        <asp:BoundField DataField="ObjectiveId" HeaderText="Obj" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                        <asp:BoundField DataField="HumanitarianPriorityId" HeaderText="Pr" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                                        <asp:BoundField DataField="IsSRP" HeaderText="Country" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />

                                                        <asp:TemplateField ItemStyle-Wrap="false" meta:resourcekey="TemplateFieldResource2">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgObjective" runat="server" AlternateText="Obj" />
                                                                <asp:Image ID="imgPriority" runat="server" AlternateText="PR" />
                                                                <asp:Image ID="imgCind" runat="server" AlternateText="C" />
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="False"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Activity" HeaderText="Activity" />
                                                        <asp:BoundField DataField="Indicator" HeaderText="Indicator" />
                                                        <asp:BoundField DataField="Location" HeaderText="Location" />
                                                        <asp:BoundField DataField="AnnualTarget" HeaderText="Annual Target" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Monthly Achieved" />

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                  <%--   </div>
                               </div>
                            </div>--%>
                        </div>
                    </div>
    </div>



</asp:Content>
