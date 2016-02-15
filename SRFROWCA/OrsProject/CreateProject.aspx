<%@ Page Title="ORS Manage Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateProject.aspx.cs" Inherits="SRFROWCA.OrsProject.CreateProject" Culture="auto" meta:resourcekey="PageResource1" UICulture="auto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {

            $("#<%=txtFromDate.ClientID%>").datepicker({
                dateFormat: "dd-mm-yy",
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtToDate.ClientID%>").datepicker("option", "minDate", selected);
                }
            });
            $("#<%=txtToDate.ClientID%>").datepicker({
                dateFormat: "dd-mm-yy",
                numberOfMonths: 2,
                onSelect: function (selected) {
                    $("#<%=txtFromDate.ClientID%>").datepicker("option", "maxDate", selected);
                }
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
            <div class="col-sm-12 widget-container-span">
                <div class="widget-box no-border">
                    <%--<div class="widget-header widget-header-small header-color-blue2">
                        <h4>
                            <asp:Localize ID="localMangeProjAddEditCaption" runat="server" Text="Add/Edit Project" meta:resourcekey="localMangeProjAddEditCaptionResource1"></asp:Localize>
                        </h4>
                    </div>--%>
                    <div class="widget-body">
                        <div class="widget-main">
                            <table border="0" style="margin: 0 auto">
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
                                            <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                                        </label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlCountry" runat="server" Width="400px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectTitle" runat="server" Text="Project Title:" meta:resourcekey="localProjectTitleResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtProjectTitle" runat="server" Width="400px" Height="90px" TextMode="MultiLine" meta:resourcekey="txtProjectTitleResource1"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtProjectTitle"
                                            CssClass="error2" Text="Required." ErrorMessage="Required." ToolTip="Required." meta:resourcekey="rfvTitleResource1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            Appealing Agency 1:
                                        </label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlOrgs" runat="server" Width="400px"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localProjectObjective" runat="server" Text="Project Objective:" meta:resourcekey="localProjectObjectiveResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtProjectObjective" runat="server" Width="400px" Height="90px"
                                            TextMode="MultiLine" meta:resourcekey="txtProjectObjectiveResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCluster" runat="server" Text="Cluster:" meta:resourcekey="localClusterResource1"></asp:Localize></label>
                                    </td>
                                    <td colspan="5">
                                        <asp:DropDownList ID="ddlCluster" runat="server" Width="400px" meta:resourcekey="ddlClusterResource1">
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
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="150px" meta:resourcekey="txtFromDateResource1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>
                                            <asp:Localize ID="localCreateProjectEndDate" runat="server" Text="End Date:" meta:resourcekey="localCreateProjectEndDateResource1"></asp:Localize></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" runat="server" Width="150px" meta:resourcekey="txtToDateResource1"></asp:TextBox>
                                    </td>
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
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
                                            OnClick="btnSave_Click" meta:resourcekey="btnSaveResource1" Enabled="true" />
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
