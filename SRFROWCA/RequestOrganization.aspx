<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RequestOrganization.aspx.cs" Inherits="SRFROWCA.RequestOrganization" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <br />
    <br />
    <div class="page-content">
        <div id="divMsg"></div>
        <div class="login-layout">
            <div class="main-container">
                <div class="main-content">
                    <div class="col-sm-10 col-sm-offset-1">
                        <div class="login-container">
                            <div class="position-relative">
                                <asp:Label runat="server" ID="txtMessage" Text="" CssClass="error2"></asp:Label>
                                <div id="signup-box" class="signup-box visible widget-box no-border">
                                    <div class="widget-body">

                                        <div class="widget-main">
                                            <h4 class="header green lighter bigger">
                                                <i class="icon-group blue"></i>
                                                <asp:Label ID="lblHeader" runat="server" Text="Organization Register Request"></asp:Label>
                                            </h4>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtOrganizationName" runat="server" MaxLength="150" CssClass="form-control"
                                                        placeholder="Organization Name"></asp:TextBox>
                                                    <%-- <i class="icon-user"></i>--%>
                                                    <asp:RequiredFieldValidator ID="rfvOrgName" runat="server" ErrorMessage="Name Required"
                                                        CssClass="error2" Text="Name Required" ControlToValidate="txtOrganizationName"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtAcronym" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="Organization Acronym"></asp:TextBox>
                                                    <%--<i class="icon-user"></i>--%>
                                                    <asp:RequiredFieldValidator ID="rfvAcronym" runat="server" ErrorMessage="Acronym Required"
                                                        CssClass="error2" Text="Acronym Required" ControlToValidate="txtAcronym"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>

                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtType" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="Organization Type"></asp:TextBox>
                                                    <%--<i class="icon-user"></i>--%>
                                                    <asp:RequiredFieldValidator ID="rfvType" runat="server" ErrorMessage="Type Required"
                                                        CssClass="error2" Text="Type Required" ControlToValidate="txtType"></asp:RequiredFieldValidator>
                                                </span>
                                            </label>
                                            <%--<label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtCountry" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="Country"></asp:TextBox>
                                                    
                                                </span>
                                            </label>
                                            <br />--%>

                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="Contact Name"></asp:TextBox>
                                                    <i class="icon-user"></i>
                                                </span>
                                            </label>
                                            <br />

                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="50" CssClass="form-control"
                                                        placeholder="Phone"></asp:TextBox>
                                                    <i class="icon-info-sign"></i>
                                                </span>
                                            </label>
                                            <br />

                                            <label class="block clearfix">
                                                <span class="block input-icon input-icon-right">
                                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="150" CssClass="form-control"
                                                        placeholder="Email"></asp:TextBox>
                                                    <i class="icon-envelope"></i>
                                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email Required"
                                                        Text="Email Required" CssClass="error2" ControlToValidate="txtEmail"></asp:RequiredFieldValidator></span>
                                            </label>

                                            <div class="clearfix" style="float: right;">


                                                <asp:Button ID="btnRegister" runat="server" Text="Submit" CssClass="btn btn-sm btn-success" OnClick="btnRegister_Click" />
                                            </div>
                                        </div>

                                    </div>
                                    <!-- /widget-body -->
                                </div>
                                <!-- /signup-box -->
                            </div>
                            <!-- /position-relative -->
                        </div>
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.main Content -->
            </div>
            <!-- / .main-container-->
        </div>
        <!-- / .login-layout-->
    </div>

</asp:Content>
