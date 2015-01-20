<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectsListing"
    Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" EnableEventValidation="false" %>

<asp:Content ID="headContent" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Projects" meta:resourcekey="localBreadCrumbProjectsResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table class="width-100">
            <tr>
                <td>
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h6>
                            <button runat="server" id="btnExportPDF" onserverclick="ExportToPDF" class="width-10 btn btn-sm btn-yellow"
                                title="PDF">
                                <i class="icon-download"></i>PDF
                                       
                            </button>
                            <button runat="server" id="btnExportToExcel" onserverclick="ExportToExcel" class="width-10 btn btn-sm btn-yellow"
                                title="Excel">
                                <i class="icon-download"></i>Excel
                                       
                            </button>
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
                                                    <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-80">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>Country:</label></td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80" meta:resourcekey="ddlCountryResource1">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>

                                                <td>Organization:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width-80"
                                                        meta:resourcekey="ddlOrgResource1">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <label>Project Code/Id:</label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width-80"
                                                        meta:resourcekey="txtProjectCodeResource1"></asp:TextBox>
                                                </td>

                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td colspan="4" style="padding-top: 10px;">
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" CausesValidation="false" />
                                                        <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" /></td>
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
        <div class="row">
            <div class="col-xs-12 col-sm-12 widget-container-span">
                <div class="widget-box">
                    <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass=" table-striped table-bordered table-hover"
                        AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                        EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                        OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnPageIndexChanging="gvProjects_PageIndexChanging"
                        meta:resourcekey="gvProjectsResource1">

                        <Columns>
                            <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                            <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode"
                                meta:resourcekey="BoundFieldResource1" />
                            <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true"
                                SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource2">
                                <ItemStyle Wrap="True"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                                meta:resourcekey="BoundFieldResource3" ItemStyle-Width="200px" />
                            <%--<asp:BoundField DataField="CurrentRequest" HeaderText="Current Request" SortExpression="CurrentRequest"
                                meta:resourcekey="BoundFieldResource5" />--%>
                            <asp:BoundField DataField="OriginalRequest" HeaderText="Original Request" SortExpression="OriginalRequest"
                                meta:resourcekey="BoundFieldResource10" />
                            <asp:BoundField DataField="OPSProjectStatus" HeaderText="Status" SortExpression="OPSProjectStatus"
                                 />
                            <asp:BoundField DataField="LocationName" Visible="false" HeaderText="Reported Locations" meta:resourcekey="BoundFieldResource7" />
                            <asp:BoundField DataField="Contact" HeaderText="Contact" ItemStyle-Wrap="true" ItemStyle-Width="150px"
                                SortExpression="Contact" meta:resourcekey="BoundFieldResource8">
                                <ItemStyle Wrap="True" Width="150px"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Phone" HeaderText="Phone" meta:resourcekey="BoundFieldResource9" />
                            <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="View" meta:resourcekey="TemplateFieldResource1">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkVieDetails" runat="server" ImageUrl="../assets/orsimages/view.png" CommandName="ViewProject"
                                        CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkVieDetailsResource1" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF">
                                <ItemTemplate>
                                    <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                        CommandArgument='<%# Eval("ProjectId") %>' />


                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
