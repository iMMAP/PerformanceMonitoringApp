<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectXMLFeeds.aspx.cs" Inherits="SRFROWCA.Admin.ProjectXMLFeeds" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a>
            </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbProjects" runat="server" Text="Project XML Feeds" meta:resourcekey="localBreadCrumbProjectsResource1"></asp:Localize></li>
      
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div style="margin-left:25px;padding-top:25px;">
        <asp:Button ID="btnSubmit" runat="server" Text="Load Projects Feed" CssClass="btn btn-primary" OnClick="btnSubmit_Click"  />
    
    </div>
    <br />
    <div style="margin-left:25px;padding-top:10px;">
    
        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
    </div>
   </asp:Content>
