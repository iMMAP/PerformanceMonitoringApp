<%@ Page Title="ORS - FAQs/Help" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="faq.aspx.cs" Inherits="SRFROWCA.faq" Culture="auto" UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
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
                        <li class="active"><a data-toggle="tab" href="#faq-tab-1"><i class="blue icon-question-sign bigger-120"></i>
                            <asp:Localize ID="localTab1" runat="server" Text="FAQs"
                                meta:resourcekey="localTab1Resource1"></asp:Localize>
                        </a></li>
                        <li><a data-toggle="tab" href="#faq-tab-2"><i class="green icon-user bigger-120"></i>
                            <asp:Localize ID="localTab2" runat="server" Text="Help" meta:resourcekey="localTab2Resource1"></asp:Localize>
                        </a></li>
                        <li><a data-toggle="tab" href="#faq-tab-4"><i class="grey icon-facetime-video bigger-120"></i>
                            <asp:Localize ID="localTab3" runat="server" Text="Videos"
                                meta:resourcekey="localTab3Resource1"></asp:Localize></a></li>
                        <!-- /.dropdown -->
                        <div class="pull-right"><i class="green icon-envelope-alt icon-large"></i><b>ors@ocharowca.info</b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <i class="blue icon-skype icon-large"></i><b>orshelpdesk</b> </div>
                    </ul>

                    <div class="tab-content no-border padding-24">
                        <div id="faq-tab-1" class="tab-pane fade in active">
                            <h4 class="blue">
                                <i class="icon-ok bigger-110"></i>
                                <asp:Localize ID="localTab1Heading"
                                    runat="server" Text="FAQs" meta:resourcekey="localTab1HeadingResource1"></asp:Localize>
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-1" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-1" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ1" runat="server" Text="What is ORS?" meta:resourcekey="localGeneralQ1Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-1">
                                        <div class="panel-body">
                                            <asp:Localize ID="Localize2" runat="server"
                                                Text="<p>The Online Reporting System (ORS) is a real- time web-based performance monitoring tool that allows UN agencies and NGOs participating in inter-agency planning processes (Strategic Response Plans or Emergency Action Plans) to directly report on the achievements based on the activities they specified during the SRP. The database has been designed to facilitate information sharing and monitor performance of all humanitarian interventions.</p><p>ORS is being deployed across the Sahel region (9 countries) and is managed by the OCHA Regional Office for West and Central Africa (ROWCA) working closely with the country offices.</p><p>The tool hosts all project data as submitted by partners during the SRP process and is also linked to the Financial Tracking Service (FTS) database and website that tracks funding requests and funding status of projects in inter-agency plans.</p>" meta:resourcekey="Localize2Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-6" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ6" runat="server"
                                                Text="How can I get access to OPS?"
                                                meta:resourcekey="localGeneralQ6Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-6">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns6" runat="server"
                                                Text="You need to register online at http://ors.ocharowca.info/ Download the User Guide and follow the instructions." meta:resourcekey="localGeneralAns6Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-2" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-sort-by-attributes-alt"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ2" runat="server" Text="Who can access ORS?" meta:resourcekey="localGeneralQ2Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-2">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns2" runat="server"
                                                Text="<p>ORS is accessible to all humanitarian stakeholders including UN agencies and NGOs, donors , and national government agencies. Summarized Performance monitoring data is public and accessible to all , while detailed reports and other functionalities are accessible after registering to the database To access ORS go to: http://ors.ocharowca.info/ </p>" meta:resourcekey="localGeneralAns2Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-3" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-credit-card bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ3" runat="server"
                                                Text="What can you do in ORS?" meta:resourcekey="localGeneralQ3Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-3">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns3" runat="server"
                                                Text="<ul><li>ORS will provide real time activity based monitoring data (summarized/detailed) across the Sahel.</li><li>Organizations can report their periodic achievements at activity level and by location.</li><li>Cluster Coordinators can mark core country or regional indicators to monitor at country and regional level.</li><li>Cluster Coordinators can validate/approve reporting submitted by organization</li><li>Users can have access to dashboards , custom reports , charts , maps and pivot table in PDF/Excel formats.</li></ul>" meta:resourcekey="localGeneralAns3Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-4" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-sort-by-attributes-alt"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ4" runat="server"
                                                Text="How to select your role in ORS" meta:resourcekey="localGeneralQ4Resource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-4">
                                        <div class="panel-body">
                                            <div id="faq-list-nested-1" class="panel-group accordion-style1 accordion-style2">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-1" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            <asp:Localize ID="localGeneralQ1p1" runat="server"
                                                                Text="Data Entry/Field Officer"
                                                                meta:resourcekey="localGeneralQ1p1Resource1"></asp:Localize>
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-1">
                                                        <div class="panel-body">
                                                            <asp:Localize ID="localGeneraolAns1p1" runat="server"
                                                                Text="<p>You belong to an appealing organization (United Nations or NGO) based in the field and you want to report on ORS. You can view all projects entered by your organization and submit achievements on them.</p>" meta:resourcekey="localGeneraolAns1p1Resource1"></asp:Localize>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-2" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            <asp:Localize ID="localGeneralQ1p2" runat="server"
                                                                Text="Country Cluster Lead" meta:resourcekey="localGeneralQ1p2Resource1"></asp:Localize>
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-2">
                                                        <div class="panel-body">
                                                            <asp:Localize ID="localGeneraolAns1p2" runat="server"
                                                                Text="<p>You belong to an appealing organization (United Nations or NGO) based in the field, and you have the function of <b>Country Cluster Lead</b>.</p> <p>You have rights to submit reporting, select country indicators, validate achievements submitted by organizations, import data into the system and generate custom reports.</p>" meta:resourcekey="localGeneraolAns1p2Resource1"></asp:Localize>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-3" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            <asp:Localize ID="localGeneralQ1p3" runat="server"
                                                                Text="Regional Cluster Lead" meta:resourcekey="localGeneralQ1p3Resource1"></asp:Localize>
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-3">
                                                        <div class="panel-body">
                                                            <asp:Localize ID="localGeneraolAns1p3" runat="server"
                                                                Text="<p>You belong to an appealing organization (United Nations or NGO) based in the field, and you have the function of Regional Cluster Lead.</p> <p>You have rights to submit reporting, select regional indicators, and generate custom reports.</p>" meta:resourcekey="localGeneraolAns1p3Resource1"></asp:Localize>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-1-sub-4" data-parent="#faq-list-nested-1" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            <asp:Localize ID="localGeneralQ1p4" runat="server" Text="OCHA Field Staff" meta:resourcekey="localGeneralQ1p4Resource1"></asp:Localize>
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-1-sub-4">
                                                        <div class="panel-body">
                                                            <asp:Localize ID="localGeneraolAns1p4" runat="server"
                                                                Text="<p>You are the database administrator at field level. You can view and edit all data, upload achievements on behalf of other organizations and approve/reject/restore achievements on behalf of the Cluster Lead.</p>" meta:resourcekey="localGeneraolAns1p4Resource1"></asp:Localize>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-5" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-credit-card bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ5" runat="server"
                                                Text="Language Preferences" meta:resourcekey="localGeneralQ5Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-5">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns5" runat="server"
                                                Text="<p>ORS provides the user with an option to choose French or English language. To change the Language preference simply click on your choice on the top right part of the window.</p>" meta:resourcekey="localGeneralAns5Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-7" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ7" runat="server"
                                                Text="I forgot my password, do I need to-register again?" meta:resourcekey="localGeneralQ7Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-7">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns7" runat="server"
                                                Text="Go to ORS, click on <b>Log on</b> and select <b>“I forgot my password”</b> link next to the password box on the log in page." meta:resourcekey="localGeneralAns7Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-8" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ8" runat="server"
                                                Text="I cannot find my organization in the organizations list" meta:resourcekey="localGeneralQ8Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-8">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns8" runat="server"
                                                Text="If your organization name does not appear, click on the link “click here to inform us”. This will not complete the registration, but will enable our administrators to verify and add your organization to the search. Fill out the form and wait for an e-mail with further guidance." meta:resourcekey="localGeneralAns8Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-9" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ9" runat="server"
                                                Text="I filled the form providing information about my organization but cannot access the database." meta:resourcekey="localGeneralQ9Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-9">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns9" runat="server"
                                                Text="You have filled out the form, but that is not the same as completing the registration. You need to wait until you get an e-mail confirming that your organization has been uploaded in the organization list before proceeding with the on-line registration process." meta:resourcekey="localGeneralAns9Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-10" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ10" runat="server"
                                                Text="Who can register in ORS from my organization?" meta:resourcekey="localGeneralQ10Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-10">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns10" runat="server"
                                                Text="ORS is flexible so you can decide. You can have one registration for your organization with one password that you can share internally as appropriate, or you can have your colleagues registering with individual e-mails and passwords. Just be careful in selecting the correct roles in your profile. If you are based in the field it is UN/NGO field programme officer, and if you are based in the organization headquarters (which are outside the plan country), you should register as HQs agencies/NGOs." meta:resourcekey="localGeneralAns10Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-11" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ11" runat="server"
                                                Text="How many times can we report?" meta:resourcekey="localGeneralQ11Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-11">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns11" runat="server"
                                                Text="You can report as many times as possible per month." meta:resourcekey="localGeneralAns11Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-1-12" data-parent="#faq-list-1" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-left pull-right" data-icon-hide="icon-chevron-down" data-icon-show="icon-chevron-left"></i><i class="icon-user bigger-130"></i>&nbsp;
                                            <asp:Localize ID="localGeneralQ12" runat="server"
                                                Text="HHow do I know that my data has been approved?" meta:resourcekey="localGeneralQ12Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-1-12">
                                        <div class="panel-body">
                                            <asp:Localize ID="localGeneralAns12" runat="server"
                                                Text="You will be notified when you submit data and when the Cluster Coordinator approves the data." meta:resourcekey="localGeneralAns12Resource1"></asp:Localize>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="faq-tab-2" class="tab-pane fade">
                            <h4 class="blue">
                                <i class="green icon-user bigger-110"></i>
                                <asp:Localize ID="localTab2Heading"
                                    runat="server" Text="Help" meta:resourcekey="localTab2HeadingResource1"></asp:Localize>
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-2" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-1" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ1" runat="server" Text="Registration" meta:resourcekey="localHelpQ1Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-1">
                                        <div class="panel-body">
                                            <a id="linkAccountAns1" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/2_ORS_Resgistration_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-2" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ2" runat="server"
                                                Text="How to select language English/French" meta:resourcekey="localHelpQ2Resource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-2">
                                        <div class="panel-body">
                                            <a id="linkAccountAns2" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/6_ORS_Language_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-3" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right middle smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ3" runat="server"
                                                Text="What are the Roles In ORS" meta:resourcekey="localHelpQ3Resource1"></asp:Localize></a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-3">
                                        <div class="panel-body">
                                            <a id="linkAccountAns3" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/3_ORS_Role_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-4" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ4" runat="server" Text="How to Report On ORS" meta:resourcekey="localHelpQ4Resource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-4">
                                        <div class="panel-body">
                                            <a id="linkAccountAns4" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/4_ORS_Reporting_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-5" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ5" runat="server" Text="Reports IN ORS" meta:resourcekey="localHelpQ5Resource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-5">
                                        <div class="panel-body">
                                            <a id="linkAccountAns5" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/5_ORS_Reports_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-2-6" data-parent="#faq-list-2" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-chevron-right smaller-80" data-icon-hide="icon-chevron-down align-top"
                                                data-icon-show="icon-chevron-right"></i>&nbsp;
                                            <asp:Localize ID="localHelpQ6" runat="server" Text="Cluster Lead Role" meta:resourcekey="localHelpQ6Resource1"></asp:Localize>
                                        </a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-2-6">
                                        <div class="panel-body">
                                            <a id="linkAccountAns6" runat="server" href="~/HelpFiles/HelpDocs/HelpEng/7_ORS_ClusterLead_EN_v3.pdf"
                                                target="_blank">Click To View Document</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="faq-tab-4" class="tab-pane fade">
                            <h4 class="blue">
                                <i class="grey icon-facetime-video bigger-110"></i>
                                <asp:Localize ID="localTab3Heading" runat="server" Text="Videos"
                                    meta:resourcekey="localTab3HeadingResource1"></asp:Localize>
                            </h4>
                            <div class="space-8">
                            </div>
                            <div id="faq-list-4" class="panel-group accordion-style1 accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-4-1" data-parent="#faq-list-4" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-plus smaller-80" data-icon-hide="icon-minus" data-icon-show="icon-plus"></i>&nbsp;English</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-4-1">
                                        <div class="panel-body">
                                            <div id="faq-list-nested-4" class="panel-group accordion-style1 accordion-style2">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-4-sub-1" data-parent="#faq-list-nested-4" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;How to Log in 
                                                            
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-4-sub-1">
                                                        <div class="panel-body">
                                                            <div>
                                                                <a href="https://www.youtube.com/watch?v=ktlOY8qZKuc&list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    target="_blank">Watch on youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="640" height="360" src="//www.youtube.com/embed/ktlOY8qZKuc?list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    frameborder="0" allowfullscreen></iframe>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-4-sub-2" data-parent="#faq-list-nested-4" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            How to report on ORS Data Entry
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-4-sub-2">
                                                        <div class="panel-body">
                                                            <div>
                                                                <a href="https://www.youtube.com/watch?v=SKYB1TS9mII"
                                                                    target="_blank">Watch on youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="640" height="360" src="//www.youtube.com/embed/SKYB1TS9mII" frameborder="0" allowfullscreen></iframe>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-4-sub-3" data-parent="#faq-list-nested-4" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                             Using ORS as a Cluster Lead
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-4-sub-3">
                                                        <div class="panel-body">
                                                            <div>
                                                                <a href="http://www.youtube.com/watch?v=9lH4nw9jSKc&feature=youtu.be"
                                                                    target="_blank">Watch on youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="640" height="360" src="//www.youtube.com/embed/9lH4nw9jSKc" frameborder="0" allowfullscreen></iframe>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <a href="#faq-42" data-parent="#faq-list-42" data-toggle="collapse" class="accordion-toggle collapsed">
                                            <i class="icon-plus smaller-80" data-icon-hide="icon-minus" data-icon-show="icon-plus"></i>&nbsp;French</a>
                                    </div>
                                    <div class="panel-collapse collapse" id="faq-42">
                                        <div class="panel-body">
                                            <div id="faq-list-nested-42" class="panel-group accordion-style1 accordion-style2">
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-42-sub-1" data-parent="#faq-list-nested-42" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            Comment s'enregistrer sur ORS
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-42-sub-1">
                                                        <div class="panel-body">
                                                            <div>
                                                                <a href="https://www.youtube.com/watch?v=pks-71zvJHU&list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    target="_blank">Regarder sur youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="640" height="360" src="//www.youtube.com/embed/pks-71zvJHU?list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    frameborder="0" allowfullscreen></iframe>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-42-sub-2" data-parent="#faq-list-nested-42" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            Comment faire un reporting sur OR:
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-42-sub-2">
                                                        <div class="panel-body">

                                                            <div>
                                                                <a href="https://www.youtube.com/watch?v=BJh5ufW0ZqQ&list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    target="_blank">Regarder sur youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="640" height="360" src="//www.youtube.com/embed/BJh5ufW0ZqQ?list=PLlXl6B0m9IO1_-IZLhCD4cRr1Ma2H4DVX"
                                                                    frameborder="0" allowfullscreen></iframe>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="panel panel-default">
                                                    <div class="panel-heading">
                                                        <a href="#faq-list-42-sub-3" data-parent="#faq-list-nested-42" data-toggle="collapse"
                                                            class="accordion-toggle collapsed"><i class="icon-plus smaller-80 middle" data-icon-hide="icon-minus"
                                                                data-icon-show="icon-plus"></i>&nbsp;
                                                            Utilisation de ORS en tant que Coordonnateur de
                                                        </a>
                                                    </div>
                                                    <div class="panel-collapse collapse" id="faq-list-42-sub-3">
                                                        <div class="panel-body">
                                                            <div>
                                                                <a href="http://www.youtube.com/watch?v=QK3Molj9wtg&feature=youtu.be"
                                                                    target="_blank">Regarder sur youtube</a>
                                                            </div>
                                                            <div>
                                                                <iframe width="480" height="360" src="//www.youtube.com/embed/QK3Molj9wtg" frameborder="0" allowfullscreen></iframe>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
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
