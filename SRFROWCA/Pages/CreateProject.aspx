<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateProject.aspx.cs" Inherits="SRFROWCA.Pages.CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {

            $("#<%=txtFromDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected)
                }
            });
            $("#<%=txtToDate.ClientID%>").datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected)
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
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Manage Projects</li>
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
                        <h4>
                        Projects List
                        </h4>
                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                        </i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                            </asp:RadioButtonList>
                            <div class="space"></div>
                            <asp:Button ID="btnCreateProject" runat="server" Text="Create New Project" CssClass="btn btn-primary"
                                CausesValidation="false" OnClick="btnCreateProject_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-9 widget-container-span">
                <div class="widget-box">
                    <div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                            Add/Edit Project
                        </h4>
                        <span class="widget-toolbar pull-right"><a href="#" data-action="collapse" class="pull-right">
                            <i class="icon-chevron-up pull-right"></i></a></span>
                    </div>
                    <div class="widget-body">
                        <div class="widget-main">
                            <table>
                                <tr>
                                    <td>
                                        <label>
                                            Project Code:</label>
                                    </td>
                                    <td>
                                        <label>
                                            <asp:Literal ID="ltrlProjectCode" runat="server" Text=""></asp:Literal></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Project Title:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProjectTitle" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtProjectTitle"
                                            CssClass="error2" Text="Required" ErrorMessage="Required." ToolTip="Required.">Required.</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Project Objective:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProjectObjective" runat="server" Width="500px" Height="100px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Cluster:</label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCluster" runat="server" Width="320px">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Start Date:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            End Date:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox><label>(mm/dd/yyyy)</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Button ID="btnSave" runat="server" Text="Save & Stay" CssClass="btn btn-primary"
                                            OnClick="btnSave_Click" />
                                        <asp:Button ID="btnSaveClose" runat="server" Text="Save & Close" CssClass="btn btn-primary"
                                            OnClick="btnSaveClose_Click" />
                                        <asp:Button ID="btnManageActivities" runat="server" Text="Manage Activities" CssClass="btn btn-primary"
                                            CausesValidation="false" OnClick="btnManageActivities_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeleteProject" runat="server" Text="Delete Project" CssClass="btn btn-danger"
                                            CausesValidation="false" OnClick="btnDeleteProject_Click" OnClientClick="javascript:return confirm('Are you sure your want to delete this project?');" />
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
