<%@ Page Title="ORS - Home" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .elemopacity {
            opacity: 100;
            width: 100%;
        }
    </style>
    <script>
        $(function () {
            $.widget("ui.tooltip", $.ui.tooltip, {
                options: {
                    content: function () {
                        return $(this).prop('title');
                    }
                }
            });
            $('.tooltip').tooltip();
        });
    </script>


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
                                <h1 class="widget-title smaller">
                                    <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                    <asp:localize id="locDefaultWelcome" runat="server" text="Welcome to ORS!" meta:resourcekey="locDefaultWelcomeResource1"></asp:localize>
                                </h1>
                            </div>
                            <div class="widget-body">
                                <div class="widget-main">
                                    <p>
                                        <asp:localize id="locDefaultIntro" runat="server" text="The Sahel Online Reporting System (ORS) is a performance monitoring tool that allows humanitarian partners 
                                        participating in inter-agency planning processes to directly report on the achievements based on the 
                                        activities specified during the SRP/HRP. The database has been designed to facilitate information sharing 
                                        and monitor response of humanitarian interventions."
                                            meta:resourcekey="locDefaultIntroResource1"></asp:localize>
                                    </p>

                                    <iframe width="100%" height="600" src="https://app.powerbi.com/view?r=eyJrIjoiNGJlYjhlOTAtZTg0Yi00ZDg0LTgwNDUtMzhlZjAzMjA5NWY2IiwidCI6IjBmOWUzNWRiLTU0NGYtNGY2MC1iZGNjLTVlYTQxNmU2ZGM3MCIsImMiOjh9" frameborder="0" allowFullScreen="true"></iframe>
                                </div>
                            </div>

                            

                            <div class="widget-body transparent">
                                <div class="widget-main padding-24">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div>
                                                <div class=" label label-lg label-info arrowed-in arrowed-right">
                                                    <asp:localize id="locHelpLable" runat="server" text="Do you need help?" meta:resourcekey="locHelpLableResource1"></asp:localize>
                                                </div>
                                                <ul class="list-unstyled spaced">
                                                    <li>
                                                        <a href="../ContactUs/Contactus.aspx">
                                                            <asp:localize id="locSendEmail" runat="server" text="Send us an email" meta:resourcekey="locSendEmailResource1"></asp:localize>
                                                        </a>
                                                    </li>
                                                    <li>
                                                        <asp:localize id="locSkype" runat="server" text="Skype: Send us message on 'orshelpdesk'" meta:resourcekey="locSkypeResource1"></asp:localize>
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
