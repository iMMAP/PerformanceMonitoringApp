<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="ProjectsListingPublic.aspx.cs" Inherits="SRFROWCA.Anonymous.ProjectsListingPublic" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/JavaScript">
        function pageLoad() {
            var manager = Sys.WebForms.PageRequestManager.getInstance();
            manager.add_beginRequest(OnBeginRequest);
        }

        function OnBeginRequest(sender, args) {
            $get('MainContent_UpdateProgress2').style.display = "block";
        }
    </script>
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <%--<asp:UpdatePanel ID="pnlOutputReportData" runat="server">
            <ContentTemplate>--%>
                <%--<div style="text-align: center;">
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlOutputReportData"
                        DynamicLayout="true">
                        <ProgressTemplate>
                            <img src="../assets/orsimages/ajaxlodr.gif" alt="Loading">
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>--%>

                <table class="width-100">
                    <tr>
                        <td>
                            <div class="widget-header widget-header-small header-color-blue2">
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
                                                                <asp:Label ID="lblCaptionCluster" runat="server"
                                                                    Text="Cluster:" meta:resourcekey="lblCaptionClusterResource1">
                                                                </asp:Label></label>
                                                        </td>
                                                        <td class="width-30">
                                                            <asp:DropDownList ID="ddlSecClusters" runat="server"
                                                                CssClass="width-80" meta:resourcekey="ddlSecClustersResource1" OnSelectedIndexChanged="SelectedIndexChanged"
                                                                 AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="width-20">
                                                            <label>
                                                                <asp:Label ID="lblCaptionSubSetCluster" runat="server"
                                                                    Text="Subset Of Cluster:" meta:resourcekey="lblCaptionSubSetClusterResource1">
                                                                </asp:Label>
                                                            </label>
                                                        </td>
                                                        <td class="width-30">
                                                            <asp:DropDownList ID="ddlClusters" runat="server" CssClass="width-80"
                                                                meta:resourcekey="ddlClustersResource1" AutoPostBack="true"
                                                                OnSelectedIndexChanged="SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <label>
                                                                <asp:Label ID="lblCaptionCountry" runat="server" Text="Country:"
                                                                    meta:resourcekey="lblCaptionCountryResource1">
                                                                </asp:Label>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList runat="server" ID="ddlCountry" CssClass="width-80"
                                                                meta:resourcekey="ddlCountryResource1" AutoPostBack="true"
                                                                OnSelectedIndexChanged="SelectedIndexChanged">
                                                            </asp:DropDownList></td>
                                                        <td>
                                                            <label>
                                                                <asp:Label ID="lblCaptionOrganization" runat="server" Text="Organization:"
                                                                    meta:resourcekey="lblCaptionOrganizationResource1">
                                                                </asp:Label>
                                                            </label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlOrg" runat="server" CssClass="width-80"
                                                                OnSelectedIndexChanged="SelectedIndexChanged" AutoPostBack="true"
                                                                meta:resourcekey="ddlOrgResource1">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    <asp:Label ID="lblCaptionProjCode" runat="server" Text="Project Code/Id:" 
                                                                        meta:resourcekey="lblCaptionProjCodeResource1">
                                                                    </asp:Label>
                                                                </label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtProjectCode" runat="server" CssClass="width-80"
                                                                    meta:resourcekey="txtProjectCodeResource1"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:CheckBox ID="cbFuned" runat="server" Text="Funded Projects" 
                                                                    Checked="false" OnCheckedChanged="SelectedIndexChanged" AutoPostBack="true" />
                                                                <asp:CheckBox ID="cbNotFunded" runat="server" Text="Not Funded Projects" 
                                                                    Checked="false" OnCheckedChanged="SelectedIndexChanged" AutoPostBack="true" />
                                                            </td>
                                                        </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td>
                                                            <asp:CheckBox ID="cbIsOPS" runat="server" Text="SRP Projects" 
                                                                Checked="false" OnCheckedChanged="SelectedIndexChanged" AutoPostBack="true" />
                                                            <asp:CheckBox ID="cbIsORS" runat="server" Text="ORS Projects" 
                                                                Checked="false" OnCheckedChanged="SelectedIndexChanged" AutoPostBack="true" />
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td colspan="4" style="padding-top: 10px;">
                                                            <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnSearchResource1" />--%>
                                                            <asp:Button ID="btnReset" runat="server" Text="Reset" Style="margin-left: 5px;" OnClick="btnReset_Click" CssClass="btn btn-primary" CausesValidation="False" meta:resourcekey="btnResetResource1" /></td>
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
                            <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
                                AllowPaging="True" AllowSorting="True" PageSize="50" ShowHeaderWhenEmpty="True"
                                EmptyDataText="Your filter criteria does not match any project!" Width="100%"
                                OnRowCommand="gvProjects_RowCommand" OnSorting="gvProjects_Sorting" OnRowDataBound="gvProjects_RowDataBound"
                                OnPageIndexChanging="gvProjects_PageIndexChanging"
                                DataKeyNames="ProjectId,ProjectOrganizationId,OrganizationId"
                                meta:resourcekey="gvProjectsResource1">
                                <RowStyle CssClass="istrow" />
                                <AlternatingRowStyle CssClass="altcolor" />
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>

                                        <ItemStyle Width="2%"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" SortExpression="ProjectCode"
                                        meta:resourcekey="BoundFieldResource1">
                                        <ItemStyle Width="10%"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Project Title" SortExpression="ProjectTitle" meta:resourcekey="BoundFieldResource2">
                                <ItemTemplate>
                                    <asp:Label ID="lblProjectTitle" runat="server"
                                        Text='<%# Eval("ProjectShortTitle") %>' ToolTip='<%# Eval("ProjectTitle") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                                    <asp:BoundField DataField="OrganizationName" HeaderText="Organization" SortExpression="OrganizationName"
                                        meta:resourcekey="BoundFieldResource3" ItemStyle-Width="15%"></asp:BoundField>
                                    <asp:BoundField DataField="ClusterName" HeaderText="Subset Cluster" SortExpression="ClusterName" meta:resourcekey="BoundFieldResource4" />
                                    <asp:BoundField DataField="SecCluster" HeaderText="Cluster" SortExpression="SecCluster" meta:resourcekey="BoundFieldResource5" />

                                    <asp:TemplateField HeaderText="Original Request" SortExpression="OriginalRequest" ItemStyle-HorizontalAlign="Right" meta:resourcekey="BoundFieldResource10">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOriginalRequest" runat="server" Text=' <%# Eval("OriginalRequest")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Funded Amount" SortExpression="FundedAmount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFundedAmount" runat="server" Text=' <%# Eval("FundedAmount")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="OPSProjectStatus" HeaderText="Status" SortExpression="OPSProjectStatus" meta:resourcekey="BoundFieldResource6" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PDF" meta:resourcekey="TemplateFieldResource2">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lnkPrint" runat="server" ImageUrl="../assets/orsimages/pdf.png" CommandName="PrintReport"
                                                CommandArgument='<%# Eval("ProjectId") %>' meta:resourcekey="lnkPrintResource1" />
                                        </ItemTemplate>

                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            <%--</ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlSecClusters" />
                <asp:AsyncPostBackTrigger ControlID="ddlClusters" />
                <asp:AsyncPostBackTrigger ControlID="ddlCountry" />
                <asp:AsyncPostBackTrigger ControlID="ddlOrg" />
                <asp:AsyncPostBackTrigger ControlID="cbFuned" />
                <asp:AsyncPostBackTrigger ControlID="cbNotFunded" />
                <asp:AsyncPostBackTrigger ControlID="cbIsOPS" />
                <asp:AsyncPostBackTrigger ControlID="cbIsORS" />                
            </Triggers>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
