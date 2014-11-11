<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadSRP.aspx.cs" Inherits="SRFROWCA.ClusterLead.UploadSRP" %>

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
        <div id="divMsg">
        </div>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <div class="page-header">
            <h1>Form Elements
								<small>
                                    <i class="icon-double-angle-right"></i>
                                    Common form elements and layouts
                                </small>
            </h1>
        </div>
        <!-- /.page-header -->

        <div class="row">
            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->
                <div>
                    <label class="col-sm-3">Country: </label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-60"></asp:DropDownList>
                    </div>
                </div>
                <div class="space-20"></div>  
                <div>
                    <label class="col-sm-3">Cluster: </label>
                    <div class="col-sm-9">
                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-60"></asp:DropDownList>
                    </div>
                </div>
               <div class="space-20"></div>
                <div>
                    <label class="col-sm-3"></label>
                    <div class="col-sm-9" >
                        <asp:FileUpload ID="fuSRP" runat="server" />
                    </div>
                </div>
               
                <div>
                    <label class="col-sm-3"></label>

                    <div class="col-sm-9">
                        <asp:Button id="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    </div>
                </div>
                
            </div>
        </div>


    </div>

</asp:Content>
