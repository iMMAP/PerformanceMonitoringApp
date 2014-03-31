<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AllData.aspx.cs" Inherits="SRFROWCA.Reports.AllData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Widgets</li>
        </ul>
        <!-- .breadcrumb -->
        <div class="nav-search" id="nav-search">
            <form class="form-search">
            <span class="input-icon">
                <input type="text" placeholder="Search ..." class="nav-search-input" id="nav-search-input"
                    autocomplete="off" />
                <i class="icon-search nav-search-icon"></i></span>
            </form>
        </div>
        <!-- #nav-search -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div class="row">
                    <div class="col-xs-12 col-sm-3 widget-container-span">
                        <div class="widget-box">
                            <div class="widget-header">
                                <h5 class="smaller">
                                    With Label</h5>
                                <div class="widget-toolbar">
                                    <span class="label label-success">16% <i class="icon-arrow-up"></i></span>
                                </div>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main padding-6">
                                    <div class="alert alert-info">
                                        Hello World!
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-3 widget-container-span">
                        <div class="widget-box light-border">
                            <div class="widget-header header-color-dark">
                                <h5 class="smaller">
                                    With Badge</h5>
                                <div class="widget-toolbar">
                                    <span class="badge badge-danger">Alert</span>
                                </div>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main padding-6">
                                    <div class="alert alert-info">
                                        Hello World!
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 widget-container-span">
                        <div class="widget-box">
                            <div class="widget-header widget-header-small header-color-dark">
                                <h6 class="smaller">
                                    With Labels & Badges</h6>
                                <div class="widget-toolbar no-border">
                                    <label>
                                        <input type="checkbox" class="ace ace-switch ace-switch-3" />
                                        <span class="lbl"></span>
                                    </label>
                                </div>
                                <div class="widget-toolbar">
                                    <span class="label label-warning">1.2% <i class="icon-arrow-down"></i></span><span
                                        class="badge badge-info">info</span>
                                </div>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <div class="alert alert-info">
                                        Lorem ipsum dolor sit amet, consectetur adipiscing.
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
