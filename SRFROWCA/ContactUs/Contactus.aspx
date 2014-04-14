<%@ Page Title="ORS - Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Contactus.aspx.cs" Inherits="SRFROWCA.ContactUs.Contactus" %>

<%@ Register Src="ContactUsControl.ascx" TagName="ContactUsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Contact Us</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <uc1:ContactUsControl ID="ContactUsControl1" runat="server" />
    </div>
</asp:Content>
