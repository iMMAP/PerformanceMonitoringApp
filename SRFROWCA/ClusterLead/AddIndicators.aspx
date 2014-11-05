<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddIndicators.aspx.cs" Inherits="SRFROWCA.ClusterLead.AddIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add Indicators</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="divMessage" runat="server" class="error2">
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12 col-sm-12">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main">
                              <div>
                                <div class="row" id="dvCountry" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Country</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                             <div>
                                <div class="row" id="dvcluster" runat="server">
                                    <h6 class="header blue bolder smaller">
                                        Select Cluster</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCluster" runat="server" CssClass="width-90" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCluster"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                            <div>
                                <div class="row">
                                    <h6 class="header blue bolder smaller">
                                        Select Objective</h6>
                                    <div class="col-xs-6 col-sm-6">
                                        <div class="widget-box no-border">
                                            <div class="widget-body">
                                                <div class="widget-main no-padding-bottom no-padding-top">
                                                    <div>
                                                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-90" AutoPostBack="true" OnSelectedIndexChanged="ddlObjective_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                   
                                </div>
                            </div>
                            <div class="row">
                                <h6 class="header blue bolder smaller">
                                    Select Activity</h6>
                                <div class="col-xs-6 col-sm-6">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <label>
                                                    Activity:</label>
                                                <div>
                                                   <asp:DropDownList ID="ddlActivity" runat="server" CssClass="width-90">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlActivity"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                              
                            </div>
                            <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
                            </asp:Panel>
                        </div>
                        <div class="pull-right">
                            <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                                class="btn spinner-down btn-xs btn-danger" type="button" visible="false">
                                <i class="icon-minus smaller-75"></i>
                            </button>
                            <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click" causesvalidation="false"
                                class="btn spinner-up btn-xs btn-success" type="button">
                                <i class="icon-plus smaller-75"></i>
                            </button>
                        </div>
                        <%--<asp:Button ID="btnAddIndicatorControl" runat="server" Text="Add More Indicators"
                            OnClick="btnAddIndiatorControl_Click" CausesValidation="false" CssClass="width-10 btn btn-sm btn-danger pull-right" />--%>
                        <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
                            title="Save">
                            <i class="icon-ok bigger-110"></i>Save
                        </button>
                        <asp:Button ID="btnBackToSRPList" runat="server" Text="Back" OnClick="btnBackToSRPList_Click"
                            CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
