<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadAchieved.aspx.cs" Inherits="SRFROWCA.ClusterLead.UploadAchieved" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
                            <h4 class="smaller"><asp:Localize ID="localDownloadItems" runat="server" Text="In this template you will have:"></asp:Localize></h4>

                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="localDownloadFirstItem" runat="server" Text="All projects of your cluster"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="Localize1" runat="server" Text="All Country Indicators"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="Localize2" runat="server" Text="All Regional Indicators"></asp:Localize>
                                </li>
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="Localize3" runat="server" Text="Project Indicators selected by user"></asp:Localize>
                                </li>
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="Localize5" runat="server" Text="Cluster Targets of each indicator"></asp:Localize>
                                </li>
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="Localize4" runat="server" Text="All locations ('Region')"></asp:Localize>
                                </li>
                            </ul>
                        <div class="space"></div>

                        <div class="center">
                            <asp:Button ID="btnDownloadTemplage" runat="server" Text="Download Template" CssClass="btn btn-primary" />
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
                            <span class="blue bigger-125">
                                <i class="icon-sitemap"></i>
                                404
                            </span>
                            Page Not Found
                        </h1>

                        <hr />
                        <h3 class="lighter smaller">We looked everywhere but we couldn't find it!</h3>

                        <div>
                            <form class="form-search">
                                <span class="input-icon align-middle">
                                    <i class="icon-search"></i>

                                    <input type="text" class="search-query" placeholder="Give it a search..." />
                                </span>
                                <button class="btn btn-sm" onclick="return false;">Go!</button>
                            </form>

                            <div class="space"></div>
                            <h4 class="smaller">Try one of the following:</h4>

                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    Re-check the url for typos
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    Read the faq
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    Tell us about it
                                </li>
                            </ul>
                        </div>

                        <hr />
                        <div class="space"></div>

                        <div class="center">
                            <a href="#" class="btn btn-grey">
                                <i class="icon-arrow-left"></i>
                                Go Back
                            </a>

                            <a href="#" class="btn btn-primary">
                                <i class="icon-dashboard"></i>
                                Dashboard
                            </a>
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
