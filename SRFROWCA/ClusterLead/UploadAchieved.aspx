﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadAchieved.aspx.cs" Inherits="SRFROWCA.ClusterLead.UploadAchieved" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
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
                            <asp:Localize ID="localTemplateInstructions" runat="server" Text="Please download the template to upload data!"></asp:Localize></h3>
                        <hr />                        
                        <h4 class="smaller">
                            <asp:Localize ID="localDownloadItems" runat="server" Text="In this template you will have:"></asp:Localize></h4>

                        <ul class="list-unstyled spaced bigger-110 margin-15">
                            <li id="liClusters" runat="server">
                                <i class="icon-hand-right blue"></i>

                                <label>
                                    Cluster:
                                    <asp:DropDownList ID="ddlClusters" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlClusters_SelectedIndexChanged"></asp:DropDownList>
                                </label>

                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <cc:DropDownCheckBoxes ID="ddlOrganizations" runat="server" CssClass="width-40"
                                    AddJQueryReference="True" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged"
                                    UseButtons="False" UseSelectAllNode="True">
                                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="300%" DropDownBoxBoxHeight="300px"></Style>
                                    <Texts SelectBoxCaption="Select Organizations" />
                                </cc:DropDownCheckBoxes>
                                <asp:Localize ID="localDownloadFirstItem" runat="server" Text="No organization means all"></asp:Localize>
                                <asp:Label ID="lblOrganization" runat="server" Text="" Visible="false"></asp:Label>
                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="width-40"
                                    AddJQueryReference="True"
                                    UseButtons="False" UseSelectAllNode="True">
                                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="100%" DropDownBoxBoxHeight="300px"></Style>
                                    <Texts SelectBoxCaption="Select Projects" />
                                </cc:DropDownCheckBoxes>
                                <asp:Localize ID="Localize1" runat="server" Text="No Project means all"></asp:Localize>
                                <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
                            </li>
                            
                           
                            <%--<li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="LocalDownloadLocations" runat="server" Text="All locations ('Region')"></asp:Localize>
                            </li>--%>
                        </ul>
                        <div class="space"></div>
                        <div>
                            <h4 class="smaller"><b>Locations You Want To Report ON:</b></h4>
                            <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="4"></asp:CheckBoxList>
                        </div>
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
