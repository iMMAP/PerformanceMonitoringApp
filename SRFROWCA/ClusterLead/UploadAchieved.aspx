<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadAchieved.aspx.cs" Inherits="SRFROWCA.ClusterLead.UploadAchieved" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
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
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbUploadAchieved" runat="server" Text="Bulk Upload"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <div class="row">
            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <h2 class="grey lighter smaller">
                            <span class="blue bigger-125">
                                <i class="icon-sitemap"></i>

                            </span>
                            <asp:Localize ID="localDownleadTemplate" runat="server" Text="Data Entry Template"></asp:Localize>
                        </h2>

                        <hr />
                        <h3 class="lighter smaller">
                            <asp:Localize ID="localTemplateInstructions" runat="server" Text="Please download the template to upload data!"></asp:Localize></h3>
                        <hr />
                        <div class="space"></div>
                        <h4 class="smaller">
                            <asp:Localize ID="localDownloadItems" runat="server" Text="In this template you will have:"></asp:Localize></h4>

                        <ul class="list-unstyled spaced inline bigger-110 margin-15">
                            <li id="liClusters" runat="server">
                                <i class="icon-hand-right blue"></i>

                                <label>
                                    Cluster:
                                    <asp:DropDownList ID="ddlClusters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"></asp:DropDownList>
                                </label>

                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="width-30"
                                    AddJQueryReference="True"
                                    UseButtons="False" UseSelectAllNode="True">
                                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="350%" DropDownBoxBoxHeight="300px"></Style>
                                    <Texts SelectBoxCaption="Select Organizations" />
                                </cc:DropDownCheckBoxes>
                                <asp:Localize ID="localDownloadFirstItem" runat="server" Text="No organization means all"></asp:Localize>
                                <asp:Label ID="lblOrganization" runat="server" Text="" Visible="false"></asp:Label>
                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>

                                <label>
                                    <input id="chkCountryIndicators" runat="server" name="form-field-checkbox" type="checkbox" class="ace" />
                                    <span class="lbl">Country Indicators</span>
                                </label>

                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>

                                <label>
                                    <input id="chkRegionalInidcators" runat="server" name="form-field-checkbox" type="checkbox" class="ace" />
                                    <span class="lbl">Regional Indicators</span>
                                </label>

                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>

                                <label>
                                    <input id="chkAllIndicators" runat="server" name="form-field-checkbox" type="checkbox" class="ace" />
                                    <span class="lbl">All Indicators (Master List)</span>
                                </label>

                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="localDownloadClusterTargets" runat="server" Text="Cluster Targets of each indicator"></asp:Localize>
                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="LocalDownloadLocations" runat="server" Text="All locations ('Region')"></asp:Localize>
                            </li>
                        </ul>
                        <div class="space"></div>
                        <div class="hidden">
                            <%--<asp:GridView ID="gvTemplate" runat="server" AutoGenerateColumns="true" OnRowDataBound="gvTemplate_RowDataBound"></asp:GridView>--%>
                        </div>
                        <div class="center">
                            <asp:Button ID="btnDownloadTemplage" runat="server" Text="Download Template" CssClass="btn btn-primary" OnClick="btnDownload_Click" />
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
            <!-- /.col -->

            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <h1 class="grey lighter smaller">
                            <asp:Localize ID="localUploadHeaderText" runat="server" Text="Upload Data"></asp:Localize>
                        </h1>

                        <hr />
                        <h3 class="lighter smaller">
                            <asp:Localize ID="localUploadBrowseText" runat="server" Text="Select excel file you want to upload"></asp:Localize>
                        </h3>
                        <hr />
                        <div>
                            <div class="space"></div>
                            <h4 class="smaller">
                                <asp:Localize ID="localUploadItemsMain" runat="server" Text="The Excel file must fullfill followin criteria:"></asp:Localize></h4>

                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="localUploadItem1" runat="server" Text="Your Data Must Be In First Sheet"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="localUploadItem2" runat="server" Text="The First Row should have headers"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:DropDownList ID="ddlMonth" runat="server">
                                    </asp:DropDownList>
                                </li>
                            </ul>
                        </div>

                        <hr />
                        <div class="space"></div>

                        <div class="center">
                            <asp:FileUpload ID="fuAchieved" runat="server" class="btn btn-grey" />
                        </div>
                        <div class="center">
                            <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btn btn-primary" OnClick="btnImport_Click" />
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </div>
</asp:Content>
