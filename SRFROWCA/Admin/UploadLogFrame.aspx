<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UploadLogFrame.aspx.cs" Inherits="SRFROWCA.Admin.UploadLogFrame" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Data Entry</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div class="col-sm-10">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main no-padding-top">
                            <div class="col-sm-14 widget-container-span">
                                <div class="widget-box">
                                    <div class="widget-header widget-header-small header-color-blue2">
                                        <h5>
                                            Bulk Upload Reports
                                        </h5>
                                        <span class="widget-toolbar"><a href="#" data-action="collapse"><i class="icon-chevron-up">
                                        </i></a></span>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                                                            ViewStateMode="Disabled"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                        <br />
                                                        <b>Please select excel file and click on import. It might take a while to import data.
                                                            Excel Sheet Name should be 'Sheet1'. First row should have column headers.<br/>
                                                            Column order and name should be (without hyphen) 
                                                            'Cluster' 'Organization' 'Year' 'Month' 'Project'  'Objective' 'Priority' 'Activity' 'Indicator'
                                                            'AnnualTarget' 'Accumulative' 'Achieved' </b>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <%--<asp:DropDownList ID="ddlEmergency" runat="server" Width="300px">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvEmergency" runat="server" ErrorMessage="Select Emergency"
                                                            InitialValue="0" Text="Required" ForeColor="Red" ControlToValidate="ddlEmergency"></asp:RequiredFieldValidator>--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:FileUpload ID="fuExcel" runat="server" />
                                                        <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btn btn-primary" OnClick="btnImport_Click" />
                                                    </td>
                                                </tr>
                                            </table>
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
</asp:Content>
