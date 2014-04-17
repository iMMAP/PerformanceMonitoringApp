<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AddIndicatorOnActivity.aspx.cs" Inherits="SRFROWCA.LeadPages.AddIndicatorOnActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Add Activity & Indicator</li>
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
                            <div class="row">
                                <h6 class="header blue bolder smaller">
                                    Select Objective & Priority</h6>
                                <div class="col-xs-9 col-sm-9">
                                    <div class="widget-box no-border">
                                        <div class="widget-body">
                                            <div class="widget-main no-padding-bottom no-padding-top">
                                                <div>
                                                    <label>
                                                        Objetive
                                                    </label>
                                                    <div>
                                                        <asp:DropDownList ID="ddlObjective" runat="server" CssClass="width-90" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlObjective"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div>
                                                    <label>
                                                        Priority
                                                    </label>
                                                    <div>
                                                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="width-90" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlPriority"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div>
                                                    <label>
                                                        Activity
                                                    </label>
                                                    <div>
                                                        <asp:DropDownList ID="ddlActivities" runat="server" CssClass="width-90">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required"
                                                            CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlActivities"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel ID="pnlAdditionalIndicaotrs" runat="server">
                            </asp:Panel>
                            <div class="pull-right">
                                <button id="btnRemoveIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click"
                                    causesvalidation="false" class="btn spinner-down btn-xs btn-danger" type="button"
                                    visible="false">
                                    <i class="icon-minus smaller-75"></i>
                                </button>
                                <button id="btnAddIndicatorControl" runat="server" onserverclick="btnAddIndiatorControl_Click"
                                    causesvalidation="false" class="btn spinner-up btn-xs btn-success" type="button">
                                    <i class="icon-plus smaller-75"></i>
                                </button>
                            </div>
                            <button runat="server" id="btnSave" onserverclick="btnSave_Click" class="width-10 btn btn-sm btn-primary"
                                title="Save">
                                <i class="icon-ok bigger-110"></i>Save
                            </button>
                            <asp:Button ID="btnBackToSRPList" runat="server" Text="Back TO SRP List" OnClick="btnBackToSRPList_Click"
                                CssClass="width-10 btn btn-sm btn-primary" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
