<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadSRP.aspx.cs" Inherits="SRFROWCA.ClusterLead.UploadSRP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .word-icon
        {
            background-image:url(../assets/orsimages/word.png);
            width:16px;
            height:16px;
        }
         .pdf-icon
        {
            background-image:url(../assets/orsimages/pdf.png);
            width:16px;
            height:16px;
        }
           .excel-icon
        {
            background-image:url(../assets/orsimages/excel.png);
            width:16px;
            height:16px;
        }

    </style>
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbUploadAchieved" runat="server" Text="Import Framework"></asp:Localize></li>
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
                        <h3 class="lighter smaller">
                            <asp:Localize ID="Localize3" runat="server" Text="Import Framework"></asp:Localize>                            
                        </h3>

                        <asp:HyperLink ID="hlTemplate" runat="server" Text="Download Framework Template" NavigateUrl="../Test/Template.xlsx"></asp:HyperLink>
                        <hr />
                        <h4 class="smaller">
                            Import Framework To ORS</h4>

                        <div>
                            <label class="col-sm-3">Country: </label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-60" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="space-20"></div>
                        <div>
                            <label class="col-sm-3">Cluster: </label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-60" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                            <div class="col-sm-9">
                                <asp:FileUpload ID="fuSRP" runat="server" />
                            </div>
                        <div>
                            <div>
                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                            </div>
                        </div>
                       <hr />
                    </div>
                </div>
            </div>
            <div class="col-xs-6">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <h3 class="grey lighter smaller">
                            <asp:Localize ID="localUploadHeaderText" runat="server" Text="Export Framework"></asp:Localize>
                        </h3>

                        <div>
                            <label class="col-sm-3">Country: </label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlCountryExport" runat="server" CssClass="width-60" AutoPostBack="true" OnSelectedIndexChanged="ddlCountryExport_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="space-20"></div>
                        <div>
                            <label class="col-sm-3">Cluster: </label>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlClusterExport" runat="server" CssClass="width-60" AutoPostBack="true" OnSelectedIndexChanged="ddlClusterExport_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlClusterExport"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div>
                            
                            <ul class="list-unstyled spaced inline bigger-110 margin-15">
                                <li id="liReport" runat="server">
                                    <div class="word-icon" style="float:left;margin-right:5px;margin-top:2px;"></div>
                                    <a href="../Reports/DownloadReport.aspx?type=10" runat="server" id="menueExportWord"><span class="menu-text"><asp:Localize ID="Localize12" runat="server" Text="Export In Word" meta:resourcekey="localReportResource1"></asp:Localize>
                                </span></a>
                                </li>
                                 <li id="li1" runat="server">
                                    <div class="excel-icon" style="float:left;margin-right:5px;margin-top:2px;"></div>
                                    <a href="../Reports/DownloadReport.aspx?type=11" runat="server" id="menueExportExcel"><span class="menu-text"><asp:Localize ID="Localize1" runat="server" Text="Export to Excel" meta:resourcekey="localReportResource1"></asp:Localize>
                                </span></a>
                                </li>
                                 <li id="li2" runat="server"  class="hidden">
                                    <div class="pdf-icon" style="float:left;margin-right:5px;margin-top:2px;"></div>
                                    <a href="../Reports/DownloadReport.aspx?type=12" runat="server" id="menueExportPDF"><span class="menu-text"><asp:Localize ID="Localize2" runat="server" Text="Export to PDF" meta:resourcekey="localReportResource1"></asp:Localize>
                                </span></a>
                                </li>
                            </ul>
                        </div>
                         <hr />
                        <div class="space-10"></div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
        </div>
    </div>
</asp:Content>
