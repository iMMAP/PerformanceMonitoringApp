<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CreateProject.aspx.cs" Inherits="SRFROWCA.Pages.CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {
            $("#<%=txtFromDate.ClientID%>").datepicker();
            $("#<%=txtToDate.ClientID%>").datepicker();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="divMsg">
    </div>
    <div class="containerDataEntryMain">
        <div class="containerDataEntryProjects">
            <div class="containerDataEntryProjectsInner">
                <fieldset>
                    <legend>My Projects</legend>
                    <asp:RadioButtonList ID="rblProjects" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblProjects_SelectedIndexChanged">
                    </asp:RadioButtonList>
                    <br />
                    <br />
                    <asp:Button ID="btnCreateProject" runat="server" Text="Create New Project" CssClass="button_example"
                        OnClick="btnCreateProject_Click" />
                </fieldset>
            </div>
        </div>
    </div>
    <div class="containerLogin">
        <div class="tablegrid">
            <table>
                <tr>
                    <td>
                        <label>
                            Project Title:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectTitle" runat="server" Width="400px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtProjectTitle"
                                        CssClass="error2" Text="Required" ErrorMessage="Required." ToolTip="Required.">Required.</asp:RequiredFieldValidator>
                                
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Project Objective:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectObjective" runat="server" Width="400px" Height="100px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Cluster:</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCluster" runat="server" Width="310px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Start Date:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            End Date:</label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnSave" runat="server" Text="Save & Stay" CssClass="button_example"
                            OnClick="btnSave_Click" />
                        <asp:Button ID="btnSaveClose" runat="server" Text="Save & Close" CssClass="button_example"
                            OnClick="btnSaveClose_Click" />
                        <asp:Button ID="btnManageActivities" runat="server" Text="Manage Activities" CssClass="button_example" CausesValidation="false"
                            OnClick="btnManageActivities_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
