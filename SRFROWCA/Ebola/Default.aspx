<%@ Page Title="" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA.Ebola.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box">




                    <div class="widget-body">
                        <div class="widget-main">
                            <iframe src="https://data.hdx.rwlabs.org/ebola" height="768px" width="100%" frameborder="0"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </div>
</asp:Content>
