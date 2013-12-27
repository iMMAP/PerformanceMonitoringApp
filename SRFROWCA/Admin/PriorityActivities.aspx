<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="PriorityActivities.aspx.cs" Inherits="SRFROWCA.Admin.PriorityActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
        <tr>
            <td class="signupheading2" colspan="3">
                <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblMessage" runat="server" CssClass="error-message" Visible="false"
                            ViewStateMode="Disabled"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <div class="containerLogin">
        <div class="graybarLogin">
            Add/Remove Clusters In Emergency
        </div>
        <div class="contentarea">
            <div class="formdiv">
                <table style="width: 50%; margin: 0 auto;">
                    <tr>
                        <td>
                            Emergency:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmergencies" runat="server" Width="300px" OnSelectedIndexChanged="ddlEmergencies_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rgvEmg" runat="server" ErrorMessage="Required" InitialValue="0"
                                Text="Required" ControlToValidate="ddlEmergencies"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Cluster:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEmgClusters" runat="server" Width="300px" OnSelectedIndexChanged="ddlEmgClusters_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvCluster" runat="server" ErrorMessage="Required"
                                InitialValue="0" Text="Required" ControlToValidate="ddlEmgClusters"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Objectives:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlObjectives" runat="server" Width="300px" OnSelectedIndexChanged="ddlObjectives_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvObjectives" runat="server" ErrorMessage="Required"
                                InitialValue="0" Text="Required" ControlToValidate="ddlObjectives"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            Priority:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPriority" runat="server" Width="300px" OnSelectedIndexChanged="ddlPriority_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ErrorMessage="Required"
                                InitialValue="0" Text="Required" ControlToValidate="ddlPriority"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="spacer" style="clear: both;">
        </div>
        <div class="graybarcontainer">
        </div>
    </div>
    <div style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="true" AllowSorting="True"
            AllowPaging="true" PageSize="100" Width="100%">
            <Columns>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
