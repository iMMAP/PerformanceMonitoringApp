<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImplementingPartners.aspx.cs" Inherits="SRFROWCA.Landing.ImplementingPartners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>ORS - Cluster Coordinator Landing</title>
    <style>
        .tooltip {
            position: relative;
            font-size: 13px;
        }

        .elemopacity {
            opacity: 100;
            width: 100%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div class="space-3"></div>

                <div class="row">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="widget-box transparent">
                            <div class="widget-header widget-header-small">
                                <h4 class="widget-title smaller">
                                    <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                    <asp:Localize ID="localQuickMenue" runat="server" Text="ORS - Implementing Partners Quick Access Menu"></asp:Localize>
                                </h4>
                            </div>
                            <h5>
                                <asp:Localize ID="localWelcomeMessage" runat="server"
                                    Text="Welcome back to ORS. You logged in as an Implementing Partner. Please tell us what you would like to do!"></asp:Localize>
                            </h5>
                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right">
                                                    <span class="label label-lg label-warning">
                                                        <i class="ace-icon fa fa-exclamation-triangle bigger-120"></i>
                                                        <asp:Localize ID="localToWork2016" runat="server"
                                                            Text="To Work On Your 2016 Sector Framework?" meta:resourcekey="localToWork2016Resource1"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>
                                            <div>
                                                <hr />
                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="localCanDo2016" runat="server" Text="You Can Do ..." meta:resourcekey="localCanDo2016Resource1"></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkDataEntry" runat="server" Text="Data Entry 2016" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ValidateReportList.aspx"></asp:LinkButton>
                                                    </li>
                                                    
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkSectorIndicators16" runat="server" Text="GO to my 2016 Sector Response Plan" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/IndicatorListing.aspx"></asp:LinkButton>
                                                    </li>
                                                    
                                                    
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProjects16" runat="server" Text="My Sector Projects 2016" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProjecsList.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnk3W16" runat="server" Text="3W of 2016 Activities" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProjecsList.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProgress16" runat="server" Text="Progress Summary 2016" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProgressSummary.aspx"></asp:LinkButton>
                                                    </li>
                                                   
                                                   
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->

                                        <div class="col-sm-6">
                                            <div class="row">
                                                <div class="col-xs-11 arrowed arrowed-right">
                                                    <span class="label label-lg label-warning">
                                                        <i class="ace-icon fa fa-exclamation-triangle bigger-120"></i>
                                                        <asp:Localize ID="localToWork2015" runat="server"
                                                            Text="To Work On Your 2015 Sector Framework?" meta:resourcekey="localToWork2015Resource1"></asp:Localize>
                                                    </span>
                                                </div>
                                            </div>

                                            <div>
                                                <hr />
                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="localCanDo2015" runat="server" Text="You Can Do ..." meta:resourcekey="localCanDo2015Resource1"></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkDataEntry15" runat="server" Text="Data Entry 2015" CssClass=""
                                                            PostBackUrl="../ClusterLead/ValidateReportList.aspx"></asp:LinkButton>
                                                    </li>
                                                   
                                                 
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkSectorIndicators15" runat="server" Text="GO to my 2015 Sector Response Plan" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/IndicatorListing.aspx"></asp:LinkButton>
                                                    </li>
                                                    
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProjects15" runat="server" Text="My Sector Projects 2015" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProjecsList.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnk3W15" runat="server" Text="3W of 2015 Activities" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProjecsList.aspx"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnlnkProgress15" runat="server" Text="Progress Summary 2015" CssClass="tooltip elemopacity"
                                                            PostBackUrl="../ClusterLead/ProgressSummary.aspx"></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->
                                    </div>
                                    <!-- /.row -->
                                </div>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div>

                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:Localize ID="locHelpLable" runat="server" Text="Do you need help?" meta:resourcekey="locHelpLableResource1"></asp:Localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="../ContactUs/Contactus.aspx">
                                                            <asp:Localize ID="locSendEmail" runat="server" Text="Send us an email" meta:resourcekey="locSendEmailResource1"></asp:Localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <asp:Localize ID="locSkype" runat="server" Text="Skype: Send us message on 'orshelpdesk'" meta:resourcekey="locSkypeResource1"></asp:Localize>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <!-- /.col -->

                                        <div class="col-sm-6">
                                            <div class="row">
                                            </div>
                                        </div>
                                        <!-- /.row -->
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

