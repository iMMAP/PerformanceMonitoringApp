<%@ Page Title="ORS - FAQs/Help" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="faq.aspx.cs" Inherits="SRFROWCA.faq" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">FAQs/Help</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->
                <div class="tabbable">
                    <ul class="nav nav-tabs padding-18 tab-size-bigger" id="myTab">
                        <li class="active"><a data-toggle="tab" href="#faq-tab-1"><i class="blue fa fa-question-sign bigger-120">
                        </i>General </a></li>
                        <li><a data-toggle="tab" href="#faq-tab-2"><i class="green fa fa-user bigger-120"></i>
                            Account </a></li>
                        <li><a data-toggle="tab" href="#faq-tab-3"><i class="orange fa fa-credit-card bigger-120">
                        </i>Help</a></li>
                        <!-- /.dropdown -->
                    </ul>
                    <div class="tab-content no-border padding-24">
                        <div id="faq-tab-1" class="tab-pane fade in active">
                            <h4 class="blue">
                                <i class="fa fa-ok bigger-110"></i>General Questions
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-1" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-1" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-left pull-right" data-fa fa-hide="fa fa-chevron-down" data-fa fa-show="fa fa-chevron-left">
                                            </i><i class="fa fa-user bigger-130"></i>&nbsp; What is ORS </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-1">
                                        <div class="panel-body">
                                            Online Reporting System is a web based performance monitoring tool.
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-2" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-left pull-right" data-fa fa-hide="fa fa-chevron-down" data-fa fa-show="fa fa-chevron-left">
                                            </i><i class="fa fa-sort-by-attributes-alt"></i>&nbsp; Who can use ORS
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-2">
                                        <div class="panel-body">
                                            <div id="faq-list-nested-1" class="panel-group accordion-style1 accordion-style2">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-1" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="fa fa-plus smaller-80 middle" data-fa fa-hide="fa fa-minus"
                                                                data-fa fa-show="fa fa-plus"></i>&nbsp; Field Officer
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-1">
                                                        <div class="panel-body">
                                                            Field Officer is the user who will report data in this system.
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-2" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="fa fa-plus smaller-80 middle" data-fa fa-hide="fa fa-minus"
                                                                data-fa fa-show="fa fa-plus"></i>&nbsp; Cluster Lead
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-2">
                                                        <div class="panel-body">
                                                            Cluster Leads are responsible to the over all work of their cluster in a country
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-3" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="fa fa-plus smaller-80 middle" data-fa fa-hide="fa fa-minus"
                                                                data-fa fa-show="fa fa-plus"></i>&nbsp; Regional Lead
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-3">
                                                        <div class="panel-body">
                                                            Regional Leads are responsible to the over all work of theri clsuter in the whole region.
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-3" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-left pull-right" data-fa fa-hide="fa fa-chevron-down" data-fa fa-show="fa fa-chevron-left">
                                            </i><i class="fa fa-credit-card bigger-130"></i>&nbsp; Features of ORS</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-3">
                                        <div class="panel-body">
                                            ORS has different features like Data-Entry, Reports, Funding Status from FTS etc.
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="faq-tab-2" class="tab-pane fade">
                            <h4 class="blue">
                                <i class="green fa fa-user bigger-110"></i>Account Questions
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-2" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-1" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-right smaller-80" data-fa fa-hide="fa fa-chevron-down align-top"
                                                data-fa fa-show="fa fa-chevron-right"></i>&nbsp; Who can create account on ORS</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-1">
                                        <div class="panel-body">
                                            Any one working in the humanitarian community to report the activities
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-2" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-right smaller-80" data-fa fa-hide="fa fa-chevron-down align-top"
                                                data-fa fa-show="fa fa-chevron-right"></i>&nbsp; What are the different User in ORS</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-2">
                                        <div class="panel-body">
                                            There are five types of users in ORS i.e. DataEntry, Cluster Lead, Regional Lead, OCHA and Donor
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-3" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-right middle smaller-80" data-fa fa-hide="fa fa-chevron-down align-top"
                                                data-fa fa-show="fa fa-chevron-right"></i>&nbsp; How to register with ORS</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-3">
                                        <div class="panel-body">
                                            You can register by clicking on Logon and then 'I want to register' link
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-4" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-chevron-right smaller-80" data-fa fa-hide="fa fa-chevron-down align-top"
                                                data-fa fa-show="fa fa-chevron-right"></i>&nbsp; How can I change my password </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-4">
                                        <div class="panel-body">
                                            Go to user settings and click on Change Password link
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="faq-tab-3" class="tab-pane fade">
                            <h4 class="blue">
                                <i class="orange fa fa-credit-card bigger-110"></i>Help
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-3" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-3-1" data-parent="#faq-list-3" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-plus smaller-80" data-fa fa-hide="fa fa-minus" data-fa fa-show="fa fa-plus">
                                            </i>&nbsp;Registration</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-3-1">
                                        <div class="panel-body">
                                            Will be updated soon!
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-3-2" data-parent="#faq-list-3" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="fa fa-plus smaller-80" data-fa fa-hide="fa fa-minus" data-fa fa-show="fa fa-plus">
                                            </i>&nbsp;Data Entry</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-3-2">
                                        <div class="panel-body">
                                            Will be updated soon
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </div>
    <!-- /.page-content -->
</asp:Content>
