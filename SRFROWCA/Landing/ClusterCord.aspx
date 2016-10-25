<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClusterCord.aspx.cs" Inherits="SRFROWCA.Landing.ClusterCord" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>ORS - Cluster Coordinator Landing</title>
    <style>
        .tooltip {
            position:relative;
            font-size:13px;
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
                                    <asp:Localize ID="localQuickMenue" runat="server" Text="ORS - Sector Coordination" meta:resourcekey="localQuickMenueResource1"></asp:Localize>
                                </h4>
                            </div>
                            <h5>
                                <asp:Localize ID="localWelcomeMessage" runat="server"
                                    Text="Welcome back to ORS. You logged in as a Cluster/Sector coordinator."  meta:resourcekey="localWelcomeMessageResource1"></asp:Localize>
                            </h5>
                           
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
