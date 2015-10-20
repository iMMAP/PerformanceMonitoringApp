<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="SRFROWCA.Account.UserProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="col-sm-offset-1 col-sm-10">
            <div id="edit-basic" class="tab-pane in active">
                <div class="row">
                    <table width="100%">
                        <tr>
                            <td>User Name:</td>
                            <td>
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="col-xs-12 col-sm-10" Enabled="false"
                                    Style="background-color: ButtonFace;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Name:
                            </td>
                            <td>

                                <asp:TextBox ID="txtFullName" runat="server" CssClass="col-xs-12 col-sm-10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                    Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Email:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="col-xs-12 col-sm-10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txtEmail"
                                    Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Phone:</td>
                            <td>
                                <asp:TextBox ID="txtPhone" MaxLength="50" CssClass="col-xs-12 col-sm-6" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                    Text="Required" ErrorMessage="Required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Country:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="col-xs-12 col-sm-6" Enabled="false">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required"
                                    Text="Required" InitialValue="0" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                                <button runat="server" id="btnUpdate" onserverclick="btnUpdate_Click" class="width-10 btn btn-sm btn-primary"
                                    title="Save">
                                    <i class="icon-ok bigger-110"></i>Save
                                </button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="divMsg">
    </div>
</asp:Content>
