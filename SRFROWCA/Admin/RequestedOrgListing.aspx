<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RequestedOrgListing.aspx.cs" Inherits="SRFROWCA.Admin.RequestedOrgListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Requested Organizations</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
            <tr>
                <td class="signupheading2 error2" colspan="3">

                    <asp:Label ID="lblMessage" runat="server" CssClass="error2" Visible="false"
                        ViewStateMode="Disabled"></asp:Label>

                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <asp:GridView ID="gvOrganization" runat="server" AutoGenerateColumns="false" OnRowCommand="gvOrganization_RowCommand" OnRowDataBound="gvOrganization_RowDataBound"
                        CssClass=" table-striped table-bordered table-hover" Width="100%" DataKeyNames="OrganizationRequestID">

                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Eval("IsAdded") %>' OnCheckedChanged="cbActive_Changed" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization Name" SortExpression="OrganizationName" />
                            <asp:BoundField DataField="OrganizationAcronym" HeaderText="Acronym" SortExpression="OrganizationAcronym" />
                            <asp:BoundField DataField="OrganizationType" HeaderText="Type" SortExpression="OrganizationType" />
                            <asp:BoundField DataField="OrganizationPhone" HeaderText="Phone" SortExpression="OrganizationPhone" />
                             <asp:BoundField DataField="OrganizationContact" HeaderText="Contact" SortExpression="OrganizationContact" />
                            <asp:BoundField DataField="OrganizationEmail" HeaderText="Email" SortExpression="Email" />
                            <asp:BoundField DataField="DateCreated" HeaderText="Created Date" SortExpression="DateCreated"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200" />
                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                CommandName="DeleteReqOrg" CommandArgument='<%# Eval("OrganizationRequestID") %>' ToolTip="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                        </Columns>

                        <EmptyDataTemplate>
                            <div class="no-record">No record found!</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </td>
            </tr>
        </table>
</asp:Content>
