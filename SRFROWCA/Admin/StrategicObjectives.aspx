<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="StrategicObjectives.aspx.cs" Inherits="SRFROWCA.Admin.StrategicObjectives" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table border="0" cellpadding="2" cellspacing="0" class="pstyle1" width="100%">
        <tr>
            <td class="signupheading2" colspan="3">
                <asp:UpdatePanel ID="updMessage" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                       <div id="divMsg">
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table border="0" style="margin: auto; background-color: ButtonFace">
        <tr>
            <td colspan="6">
                <table>
                    <tr>
                        <td>
                            Objective:
                        </td>
                        <td>
                            <asp:TextBox ID="txtObj" runat="server" Width="800px" TextMode="MultiLine" Height="80px"
                                MaxLength="500"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvObjective" runat="server" ErrorMessage="Required"
                                Text="Required" ControlToValidate="txtObj"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <asp:HiddenField ID="hfPKId" runat="server" />
            <td colspan="6" align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add/Update Objective" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <table style="margin: auto; width: 100%; background-color: Gray">
        <tr>
            <td>
                <b>Search:</b>
                <input type="text" id="txtSearch" class="searchFieldOrgs" />
            </td>
        </tr>
    </table>
    <div style="overflow-x: auto; width: 100%">
        <asp:GridView ID="gvStrategicObjective" runat="server" AutoGenerateColumns="false"
            OnRowCommand="gvStrategicObjective_RowCommand" Width="100%" OnRowDataBound="gvStrategicObjective_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ObjectiveId" HeaderText="Id" HeaderStyle-Width="40px"
                    SortExpression="ObjectiveId" />
                <asp:TemplateField HeaderText="Objective" SortExpression="Objective">
                    <ItemTemplate>
                        <asp:Label ID="lblObjective" runat="server" Text='<%# Eval("Objective") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                            CommandName="EditObjective" CommandArgument='<%# Eval("ObjectiveId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                            CommandName="DeleteOrg" CommandArgument='<%# Eval("ObjectiveId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>                
            </Columns>
            <HeaderStyle BackColor="ButtonFace" />
        </asp:GridView>
    </div>
</asp:Content>
