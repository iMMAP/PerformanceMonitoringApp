<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="SRFROWCA.Account.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="fa fa-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">My Profile</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div id="user-profile-3" class="user-profile row">
        <div class="col-sm-offset-1 col-sm-10">
            <div class="space">
            </div>
            <div class="tab-content profile-edit-tab-content">
                <div id="edit-basic" class="tab-pane in active">
                    <h4 class="header blue bolder smaller">
                        General</h4>
                    <div class="row">
                        <div class="vspace-xs">
                        </div>
                        <div class="col-xs-12 col-sm-8">
                            <div class="form-group">
                                <label class="col-sm-4 control-label no-padding-right" for="form-field-username">
                                    Username</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="col-xs-12 col-sm-10" Enabled="false"
                                        Style="background-color: ButtonFace;"></asp:TextBox>
                                </div>
                            </div>
                            <div class="space">
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label no-padding-right" for="form-field-first">
                                    Name</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="col-xs-12 col-sm-10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                            Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                
                            </div>
                        </div>
                        <div class="space">
                        </div>
                    </div>
                    <div class="row">
                        <h4 class="header blue bolder smaller">
                            Contact</h4>
                        <div class="form-group">
                            <div class="col-xs-12 col-sm-8">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label no-padding-right" for="form-field-username">
                                        Email</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="col-xs-12 col-sm-10"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtEmail"
                                            Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="space-4">
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label no-padding-right" for="form-field-first">
                                        Phone</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtPhone" MaxLength="50" CssClass="col-xs-12 col-sm-6" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                            Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label no-padding-right" for="form-field-first">
                                        Country</label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="ddlCountry" runat="server" CssClass="col-xs-12 col-sm-6">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                            Text="Required" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <hr />
                <div>
                    <button runat="server" id="btnUpdate" onserverclick="btnUpdate_Click" class="width-10 btn btn-sm btn-primary"
                        title="Save">
                        <i class="fa fa-ok bigger-110"></i>Save
                    </button>
                </div>
            </div>
        </div>
    </div>
    <div id="divMsg">
    </div>
</asp:Content>
