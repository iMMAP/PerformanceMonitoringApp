<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditOrganization.aspx.cs" Inherits="SRFROWCA.Admin.Organization.AddEditOrganization" %>

<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table width="100%">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <div class="row">
                                                    <table border="0" class="width-70" style="margin: 10px 10px 10px 20px">
                                                        <tr>
                                                            <td style="width: 200px">
                                                                <label>
                                                                    Organization Name:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrgName" runat="server" MaxLength="150" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Organization Name Required"
                                                                    CssClass="error2" Text="Required" ControlToValidate="txtOrgName"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>
                                                                    Organization Acronym:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtOrgAcronym" runat="server" MaxLength="150" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Organization Acronym Required"
                                                                    CssClass="error2" Text="Required" ControlToValidate="txtOrgAcronym"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Organization Type:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="width-100" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="Select Type" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvUserRole" runat="server" ErrorMessage="Organization Type Required" InitialValue="-1"
                                                                    CssClass="error2" Text="Required" ControlToValidate="ddlType"></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Country:</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="width-100" AppendDataBoundItems="true">
                                                                    <asp:ListItem Text="Select Country" Value="-1"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="200px">
                                                                <label>
                                                                    Phone:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="15" CssClass="width-100"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="200px">
                                                                <label>
                                                                    Address:</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtAddress" runat="server" MaxLength="250" CssClass="width-100" TextMode="MultiLine"></asp:TextBox>
                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td width="200px">
                                                                <label>
                                                                    Active:</label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkStatus" runat="server" />

                                                            </td>
                                                            <td></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="padding-top:20px;">
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="width-20 btn btn-sm btn-success" OnClick="btnSave_Click" />
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
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
