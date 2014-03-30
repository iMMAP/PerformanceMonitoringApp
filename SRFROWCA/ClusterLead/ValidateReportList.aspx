<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ValidateReportList.aspx.cs" Inherits="SRFROWCA.ClusterLead.ValidateReportList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                        </h4>
                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                        </i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="False" HeaderStyle-BackColor="ButtonFace"
                                    DataKeyNames="ReportId" CssClass="imagetable" Width="100%" meta:resourcekey="gvActivitiesResource1"
                                    OnRowCommand="gvReports_RowCommand">
                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:BoundField DataField="ReportName" HeaderText="Report Name" SortExpression="ReportName" />
                                        <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode" />
                                        <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" SortExpression="ProjectTitle" />
                                        <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName" />
                                        <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Last Updated" SortExpression="CreatedDate" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgbtnView" runat="server" ImageUrl="~/assets/orsimages/view.png"
                                                    CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' CommandName="ViewReport" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
