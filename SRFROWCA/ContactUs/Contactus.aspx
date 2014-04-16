<%@ Page Title="ORS - Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Contactus.aspx.cs" Inherits="SRFROWCA.ContactUs.Contactus" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Src="ContactUsControl.ascx" TagName="ContactUsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbContactUs" runat="server" Text="Contact Us" meta:resourcekey="localBreadCrumbContactUsResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <uc1:ContactUsControl ID="ContactUsControl1" runat="server" />
    </div>
</asp:Content>
