<%@ Page Title="ORS Manage Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateProject.aspx.cs" Inherits="SRFROWCA.Pages.CreateProject" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="../Default.aspx">
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
                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="fa fa-chevron-up"></i></a></span>
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
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="fa fa-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <table>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectCode" runat="server" Text="Project Code:" meta:resourcekey="localProjectCodeResource1"></asp:Localize> </label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:Literal ID="ltrlProjectCode" runat="server" meta:resourcekey="ltrlProjectCodeResource1"></asp:Literal></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectTitle" runat="server" Text="Project Title:" meta:resourcekey="localProjectTitleResource1"></asp:Localize></label>
                                    </td>
                                    <td>
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
                                    <td>
                                        <asp:TextBox ID="txtProjectObjective" runat="server" Width="500px" Height="100px"
                                            TextMode="MultiLine" meta:resourcekey="txtProjectObjectiveResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCluster" runat="server" Text="Cluster:" meta:resourcekey="localClusterResource1"></asp:Localize></label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCluster" runat="server" Width="320px" meta:resourcekey="ddlClusterResource1">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCreateProjectStartDate" runat="server" Text="Start Date:" meta:resourcekey="localCreateProjectStartDateResource1"></asp:Localize></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" meta:resourcekey="txtFromDateResource1"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCreateProjectEndDate" runat="server" Text="End Date:" meta:resourcekey="localCreateProjectEndDateResource1"></asp:Localize></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" meta:resourcekey="txtToDateResource1"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSave" runat="server" Text="Save & Stay" CssClass="btn btn-primary"
                                            OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" />
                                        <asp:Button ID="btnSaveClose" runat="server" Text="Save & Close" CssClass="btn btn-primary"
                                            OnClick="btnSaveClose_Click" meta:resourcekey="btnSaveCloseResource1" />
                                        <asp:Button ID="btnManageActivities" runat="server" Text="Manage Activities" CssClass="btn btn-primary" Enabled="False"
                                            CausesValidation="False" OnClick="btnManageActivities_Click" meta:resourcekey="btnManageActivitiesResource1" />
                                    </td>
                                    <td>
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
</asp:Content>
