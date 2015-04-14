<%@ Page Title="ORS Manage Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateProject.aspx.cs" Inherits="SRFROWCA.Pages.CreateProject" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function ShowModalPopup() {
            $find("mpeOrganizations").show();
            return false;
        }
        function HideModalPopup() {
            $find("mpeOrganizations").hide();
            return false;
        }
    </script>
    <script>
        $(function () {

            $("#<%=txtFromDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected);
                }
            });
            $("#<%=txtToDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected);
                }
            });
        });
    </script>
    <!-- ORS styles -->
    <link rel="stylesheet" href="../assets/css/ors.css" />
    <!-- ace styles -->

    <script>
        $(function () {
            $("tr .testcb").each(function () {

                var checkBox = $(this).find("input[type='checkbox']");
                if ($(checkBox).is(':checked')) {
                    $(this).parent().parent().removeClass('highlightRow2');
                    $(this).parent().parent().addClass('highlight');
                }
                else {
                    $(this).parent().parent().removeClass('highlight');
                }
            });

            // scrollables
            $('.slim-scroll').each(function () {
                var $this = $(this);
                $this.slimScroll({
                    height: $this.data('height') || 100,
                    railVisible: true
                });
            });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {

            $("#txtSearch").keyup(function (event) {
                var searchKey = $('#txtSearch').val().toLowerCase();
                $(".gvorgsclass tr td:nth-child(2)").each(function () {
                    var cellText = $(this).text().toLowerCase();
                    if (cellText.indexOf(searchKey) >= 0) {
                        $(this).parent().show();
                    }
                    else {
                        $(this).parent().hide();
                    }
                });
            });

            $('.gvorgsclass INPUT').click(function () {
                $(this).parent().parent().parent().toggleClass('highlight');
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home" meta:resourcekey="localBreadCrumbHomeResource1"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbManageProjects" runat="server" Text="Manage Projects" meta:resourcekey="localBreadCrumbDataEntryResource1"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-3 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>Projects
                        </h4>
                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged" meta:resourcekey="rblProjectsResource1">
                            </asp:RadioButtonList>
                            <div class="space"></div>
                            <asp:Button ID="btnCreateProject" runat="server" Text="Create New Project" CssClass="btn btn-primary"
                                CausesValidation="False" OnClick="btnCreateProject_Click" meta:resourcekey="btnCreateProjectResource1" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                            <asp:Localize ID="localMangeProjAddEditCaption" runat="server" Text="Add/Edit Project" meta:resourcekey="localMangeProjAddEditCaptionResource1"></asp:Localize>
                        </h4>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <table border="0">
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectCode" runat="server" Text="Project Code:" meta:resourcekey="localProjectCodeResource1"></asp:Localize>
                                        </label>
                                    </td>
                                    <td colspan="5">
                                        <label>
                                            <asp:Literal ID="ltrlProjectCode" runat="server" meta:resourcekey="ltrlProjectCodeResource1"></asp:Literal></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectTitle" runat="server" Text="Project Title:" meta:resourcekey="localProjectTitleResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtProjectTitle" runat="server" Width="500px" TextMode="MultiLine" meta:resourcekey="txtProjectTitleResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtProjectTitle"
                                            CssClass="error2" Text="Required." ErrorMessage="Required." ToolTip="Required." meta:resourcekey="rfvTitleResource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectObjective" runat="server" Text="Project Objective:" meta:resourcekey="localProjectObjectiveResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtProjectObjective" runat="server" Width="500px"
                                            TextMode="MultiLine" meta:resourcekey="txtProjectObjectiveResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="LocalizeImplementingPartners" runat="server" Text="Project Partners:"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <button id="btnOpenLocations" runat="server" causesvalidation="false"
                                            type="button" class="btn btn-sm btn-primary">
                                            <i class="icon-building-o"></i>
                                            <asp:Localize ID="localLocationButton" runat="server" Text="Add Project Partners" meta:resourcekey="localLocationButtonResource1"></asp:Localize>
                                        </button>
                                        <a href="../RequestOrganization.aspx">Request Missing Parner Organization</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCluster" runat="server" Text="Cluster:" meta:resourcekey="localClusterResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlCluster" runat="server" Width="150px" meta:resourcekey="ddlClusterResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="Localize1" runat="server" Text="Project Status:"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlProjectSatus" runat="server" Width="150px">
                                            <asp:ListItem Text="Select Project Status" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Ongoing" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Completed" Value="3"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCreateProjectStartDate" runat="server" Text="Start Date:" meta:resourcekey="localCreateProjectStartDateResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="150px" meta:resourcekey="txtFromDateResource1"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCreateProjectEndDate" runat="server" Text="End Date:" meta:resourcekey="localCreateProjectEndDateResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtToDate" runat="server" Width="150px" meta:resourcekey="txtToDateResource1"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Requested Amount:</td>
                                    <td>
                                        <asp:TextBox ID="txtRequestedAmount" runat="server" Width="150px"></asp:TextBox></td>
                                    <td>Currency:</td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlRequestedAmountCurrency" runat="server" Width="150px"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localFundingStatus" runat="server" Text="Funding Status:" meta:resourcekey="localFundingStatusResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlFundingStatus" runat="server" Width="150px">
                                            <asp:ListItem Text="Select Funding Status" Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Funded" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Partialy Funded" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Not Funded" Value="2"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localDonorName" runat="server" Text="Donor1:" meta:resourcekey="localDonorNameResource1"></asp:Localize></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDonorName" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>Contributed</td>
                                    <td>
                                        <asp:TextBox ID="txtDonor1Contributed" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>Currency:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlDonor1Currency" Width="150px" runat="server"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Donor2</td>
                                    <td>
                                        <asp:TextBox ID="txtDonor2Name" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>Contributed</td>
                                    <td>
                                        <asp:TextBox ID="txtDonor2Contributed" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                    </td>
                                    <td>Currency:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlDonor2Currency" Width="150px" runat="server"></asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>Contact Name:</td>
                                    <td>
                                        <asp:TextBox ID="txtContactName" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>Phone:</td>
                                    <td>
                                        <asp:TextBox ID="txtContactPhone" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td>Email:</td>
                                    <td>
                                        <asp:TextBox ID="txtContactEmail" runat="server" Width="150px" MaxLength="100"></asp:TextBox>
                                </tr>
                                <tr>
                                    <td colspan="3" style="padding-top: 20px;">
                                        <asp:Button ID="btnSave" runat="server" Text="Save & Stay" CssClass="btn btn-primary"
                                            OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
                                        <asp:Button ID="btnSaveClose" runat="server" Text="Save & Close" CssClass="btn btn-primary"
                                            OnClick="btnSaveClose_Click" meta:resourcekey="btnSaveCloseResource1" />
                                        <asp:Button ID="btnManageActivities" runat="server" Text="Manage Activities" CssClass="btn btn-primary" Enabled="False"
                                            CausesValidation="False" OnClick="btnManageActivities_Click" meta:resourcekey="btnManageActivitiesResource1" />
                                    </td>
                                    <td style="padding-top: 20px;">
                                        <asp:Button ID="btnDeleteProject" runat="server" Text="Delete Project" CssClass="btn btn-danger"
                                            CausesValidation="False" OnClick="btnDeleteProject_Click" OnClientClick="javascript:return confirm('Are you sure your want to delete this project?');" meta:resourcekey="btnDeleteProjectResource1" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:ModalPopupExtender ID="mpeOrganizations" runat="server" BehaviorID="mpeOrganizations" TargetControlID="btnOpenLocations"
        PopupControlID="pnlLocations" BackgroundCssClass="modalpopupbackground"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlLocations" runat="server" Width="800px" meta:resourcekey="pnlLocationsResource1">
        <asp:UpdatePanel ID="uPanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="row">
                    <div class=" width-100 modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header no-padding">
                                <div class="table-header">                                    
                               Search By Organization Name:  
                                            <input type="text" id="txtSearch" width="100px" />
                                </div>
                            </div>
                            <div class="modal-body no-padding">
                                <table border="0" style="margin: 0 auto;">
                                    <tr>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <fieldset>
                                                <div id="scrolledGridView" style="overflow-x: auto; width: 100%; height: 400px;">
                                                    <asp:GridView ID="gvOrgs" runat="server" AutoGenerateColumns="false" DataKeyNames="OrganizationId" CssClass="gvorgsclass">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbOrg" runat="server" Text="" CssClass="testcb" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization" HeaderStyle-Width="530px" />
                                                            <asp:BoundField DataField="OrganizationAcronym" HeaderText="Acronym" HeaderStyle-Width="220px" />
                                                        </Columns>
                                                        <HeaderStyle BackColor="ButtonFace" />
                                                    </asp:GridView>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="modal-footer no-margin-top">
                                <asp:Button ID="btnClose" runat="server" Text="Close" Width="120px" CssClass="btn btn-primary"
                                    CausesValidation="False" OnClientClick="return HideModalPopup()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnClose" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
