<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TargetSettings.aspx.cs" Inherits="SRFROWCA.Admin.TargetSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div class="tab-content no-border ">
                    <table style="width: 100%;">
                        <tr>
                            <td width="200px">Country:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270" OnSelectedIndexChanged="ddlSelected"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Cluster:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCluster" Width="270" OnSelectedIndexChanged="ddlSelected"
                                    AutoPostBack="true">
                                </asp:DropDownList>

                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table style="width: 100%;">
                        <tr>
                            <td width="200px">Provide Target:
                            </td>
                            <td>
                                <asp:RadioButton ID="rbTargetYes" runat="server" Text="Yes" GroupName="Target" />
                                <asp:RadioButton ID="rbTargetNo" runat="server" Text="No" GroupName="Target" />
                            </td>
                        </tr>
                        <tr>
                            <td>Admin Level:</td>
                            <td>
                                <asp:DropDownList ID="ddlLevel" runat="server" Width="200px">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Admin2" Value="District"></asp:ListItem>
                                    <asp:ListItem Text="Admin1" Value="Governorate"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="National"></asp:ListItem>
                                </asp:DropDownList>
                        </tr>
                        <tr>
                            <td>Location Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlType" runat="server" Width="200px">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Government" Value="Government"></asp:ListItem>
                                    <asp:ListItem Text="Health" Value="Health"></asp:ListItem>
                                </asp:DropDownList>
                        </tr>
                        <tr>
                            <td>Target Mandatory:
                            </td>
                            <td>
                                <asp:RadioButton ID="rbMandatoryYes" runat="server" Text="Yes" GroupName="Mandatory" />
                                <asp:RadioButton ID="rbMandatoryNo" runat="server" Text="No" GroupName="Mandatory" />
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="padding-top: 20px;">
                                <asp:Button ID="btnSave" runat="server"
                                    OnClick="btnSave_Click"
                                    Text="Save" CssClass="btn btn-sm btn-primary" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="padding-top: 20px;">
                                <asp:Label runat="server" ID="lblFrameworkSettings" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvSettings" runat="server" AutoGenerateColumns="false"
                    CssClass="imagetable">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" DataField="Country" HeaderText="Country" />
                        <asp:BoundField ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" DataField="Cluster" HeaderText="Cluster" />
                        <asp:BoundField ItemStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" DataField="Target" HeaderText="Target" />
                        <asp:BoundField ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" DataField="Level" HeaderText="Admin Level" />
                        <asp:BoundField ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" DataField="Category" HeaderText="Location Type" />
                        <asp:BoundField ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" DataField="Mandatory" HeaderText="Mandatory" />
                        <asp:TemplateField HeaderStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteConfig" CommandArgument='<%# Eval("key") %>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
