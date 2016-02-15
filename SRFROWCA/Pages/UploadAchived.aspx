﻿<%@ Page Title="ORS - Bulk Upload User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadAchived.aspx.cs" Inherits="SRFROWCA.Pages.UploadAchived" %>

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
                        <h4 class="smaller"><b>
                            <asp:Localize ID="localDownloadItems" runat="server" Text="In this template you will have:"></asp:Localize></b> </h4>

                        <ul class="list-unstyled spaced inline bigger-110 margin-15">
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <cc:DropDownCheckBoxes ID="ddlProjects" runat="server" CssClass="width-30"
                                    AddJQueryReference="True"
                                    UseButtons="False" UseSelectAllNode="True">
                                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="350%" DropDownBoxBoxHeight="300px"></Style>
                                    <Texts SelectBoxCaption="Select Projects" />
                                </cc:DropDownCheckBoxes>
                                <asp:Localize ID="Localize1" runat="server" Text="No Project means all"></asp:Localize>
                                <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="Localize2" runat="server" Text="Project Indicators"></asp:Localize>
                            </li>
                            <%--<li>
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

                            </li> --%>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="localDownloadClusterTargets" runat="server" Text="Cluster Targets of each indicator"></asp:Localize>
                            </li>
                            <li>
                                <i class="icon-hand-right blue"></i>
                                <asp:Localize ID="LocalDownloadLocations" runat="server" Text="All locations ('Admin1')"></asp:Localize>
                            </li>
                        </ul>
                        <div class="space"></div>
                        <div>
                            <h4 class="smaller"><b>Locations You Want To Report ON:</b></h4>
                            <asp:CheckBoxList ID="cblLocations" runat="server" RepeatColumns="4"></asp:CheckBoxList>
                        </div>
                        <div class="space"></div>
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
                                    <asp:Localize ID="localUploadItem1" runat="server" Text="Your Data Must be in first sheet"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    <asp:Localize ID="localUploadItem2" runat="server" Text="The First Row should have headers"></asp:Localize>
                                </li>

                                <li>
                                    <i class="icon-hand-right blue"></i>
                                    Month:
                                    <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </li>
                            </ul>
                        </div>
                        <div>Select The Month To Upload Data.</div>

                        <div class="center">
                            <asp:FileUpload ID="fuAchieved" runat="server" class="btn btn-grey" Enabled="false" />
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
