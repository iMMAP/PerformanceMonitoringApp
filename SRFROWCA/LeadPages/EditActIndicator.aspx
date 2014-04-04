<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EditActIndicator.aspx.cs" Inherits="SRFROWCA.LeadPages.EditActIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <div class="widget-box no-border">
                    <div class="widget-body">
                        <div class="widget-main">
                            <div>
                                <label for="form-field-8">
                                    <div id="divObjPr" runat="server"></div>
                                    </label>
                                
                            </div>
                            <hr />
                            <div>
                                <label for="form-field-9">
                                    Activity</label>
                                    <asp:TextBox ID="txtActivity" runat="server" CssClass="width-80" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="Required"
                                                        CssClass="error2" Text="Required" ControlToValidate="txtActivity"></asp:RequiredFieldValidator>
                            </div>
                            <hr />
                            <div>
                                <label for="form-field-11">
                                    Indicator</label>
                                <asp:TextBox ID="txtIndicator" runat="server" CssClass="width-80" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                                                        CssClass="error2" Text="Required" ControlToValidate="txtIndicator"></asp:RequiredFieldValidator>
                            </div>
                            <hr />
                            <div class="btn-group pull-right">
                                        <button runat="server" id="btnSave" onserverclick="btnSave_Click"
                                            class="width-10 btn btn-sm btn-primary" title="Save">
                                            Save
                                        </button>
                                        <button runat="server" id="btnCancel" onserverclick="btnCancel_Click" causesvalidation="false"
                                            class="width-10 btn btn-sm btn-default" title="Cancel">
                                            Cancel
                                        </button>
                                    </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /span -->
        </div>
</asp:Content>
