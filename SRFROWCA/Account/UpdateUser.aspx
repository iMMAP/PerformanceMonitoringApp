<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UpdateUser.aspx.cs" Inherits="SRFROWCA.Account.UpdateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
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
                <asp:Localize ID="localBreadCrumbRegister" runat="server" Text="Add New User"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="row">
            <div>
                <label>Full Name</label>
                <div>
                    <asp:TextBox ID="txtFullName" runat="server"></asp:TextBox>
                </div>
            </div>
            <div>
                <label>
                    User Name:</label>
                <div>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="ddlWidthAllData" Enabled="false"
                        Style="background-color: ButtonFace;"></asp:TextBox>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Email:
                </label>
                <div>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="ddlWidthAllData"></asp:TextBox>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Phone:</label>
                <div>
                    <asp:TextBox ID="txtPhone" MaxLength="50" CssClass="ddlWidthAllData" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="singalselect">
                <label>
                    Organization:
                </label>
                <div>
                    <asp:DropDownList ID="ddlOrganization" runat="server" Width="400px">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="divCountry" runat="server" class="singalselect">
                <label>
                    Country:</label>
                <div>
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="400px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Country"
                        Text="*" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="space-10"></div>
            <div>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click"
                    CssClass="btn btn-blue2" />
            </div>
        </div>
    </div>
</asp:Content>
