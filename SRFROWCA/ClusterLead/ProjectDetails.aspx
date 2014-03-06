<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectDetails.aspx.cs" Inherits="SRFROWCA.ClusterLead.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="containerLogin">
        <asp:FormView ID="fvProjects" runat="server">
            <ItemTemplate>                
                <h3>
                    <%# Eval("ProjectCode") %></h3>
                <table border="0">
                    <tr>
                        <td>
                            <label>Project Id:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectId") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Project Title:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectTitle")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Organization:</label>
                        </td>
                        <td>
                            <%# Eval("OrganizationName") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Country:</label>
                        </td>
                        <td>
                            <%# Eval("LocationName") %>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <label>Contact Name:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectContactName")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contact Email:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectContactEmail")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Contact Phone:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectContactPhone")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Start Date:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectStartDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>End Date:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectEndDate")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>Partners:</label>
                        </td>
                        <td>
                            <%# Eval("ProjectImplementingpartner")%>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:FormView>
        <%--<asp:DetailsView ID="dvProjects" runat="server" Height="600px" Width="100%" AutoGenerateRows="false"
            AllowPaging="true" OnPageIndexChanging="dvProjects_PageIndexChanging">
            <RowStyle CssClass="istrow" />
            <AlternatingRowStyle CssClass="altcolor" />
            <Fields>
                <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" />
                <asp:BoundField DataField="ProjectTitle" HeaderText="Project Title" />
            </Fields>
        </asp:DetailsView>--%>
    </div>
</asp:Content>
