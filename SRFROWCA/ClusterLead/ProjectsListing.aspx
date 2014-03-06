<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectsListing.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectsListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <asp:GridView ID="gvProjects" runat="server" AutoGenerateColumns="False" CssClass="imagetable"
            Width="100%" OnRowCommand="gvProjects_RowCommand">
            <RowStyle CssClass="istrow" />
            <AlternatingRowStyle CssClass="altcolor" />
            <Columns>
                <asp:BoundField DataField="ProjectId" HeaderText="Id" />
                <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" />
                <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="OrganizationName" HeaderText="Organization" />
                <asp:BoundField DataField="ProjectContactName" HeaderText="Contact Name" />
                <asp:BoundField DataField="ProjectContactEmail" HeaderText="Contact Email" />
                <asp:BoundField DataField="ProjectContactPhone" HeaderText="Phone" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkVieDetails" runat="server" Text="View" CommandName="ViewProject"
                            CommandArgument='<%# Eval("ProjectId") %>' /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>

