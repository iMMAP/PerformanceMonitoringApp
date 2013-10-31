<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="IndicatorActivities.aspx.cs" Inherits="SRFROWCA.Admin.Clusters.IndicatorActivities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <script type="text/javascript">
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvActivity.ClientID %>');
            table.find('tr').each(function (index, row) {
                var allCells = $(row).find('td');
                if (allCells.length > 0) {
                    var found = false;
                    allCells.each(function (index, td) {
                        var regExp = new RegExp(inputVal, 'i');
                        if (regExp.test($(td).text())) {
                            found = true;
                            return false;
                        }
                    });
                    if (found == true) {
                        $(row).show('fast');
                    }
                    else {
                        $(row).hide('fast');
                    }
                }
            });
        }          
    </script>
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
    <table border="0" style="margin: auto; background-color: ButtonFace">
        <tr>
            <td>
                Emergency:
            </td>
            <td>
                <asp:DropDownList ID="ddlLocEmergencies" runat="server" Width="300px" OnSelectedIndexChanged="ddlLocEmergencies_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rgvEmg" runat="server" ErrorMessage="Required" InitialValue="0"
                    Text="Required" ControlToValidate="ddlLocEmergencies"></asp:RequiredFieldValidator>
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
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required"
                    InitialValue="0" Text="Required" ControlToValidate="ddlObjectives"></asp:RequiredFieldValidator>
            </td>
            <td>
                Indicators:
            </td>
            <td>
                <asp:DropDownList ID="ddlIndicators" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvIndicators" runat="server" ErrorMessage="Required"
                    InitialValue="0" Text="Required" ControlToValidate="ddlIndicators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Activity Type:
            </td>
            <td>
                <asp:DropDownList ID="ddlActivityType" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required"
                    InitialValue="0" Text="Required" ControlToValidate="ddlActivityType"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <table>
                    <tr>
                        <td>
                            Activity:
                        </td>
                        <td>
                            <asp:TextBox ID="txtActivity" runat="server" Width="800px" TextMode="MultiLine" Height="80px"
                                MaxLength="500"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvActivity" runat="server" ErrorMessage="Required"
                                Text="Required" ControlToValidate="txtActivity"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <asp:HiddenField ID="hfPKId" runat="server" />
            <td colspan="6" align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add/Update Indicator" OnClick="btnAdd_Click" />
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
        <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="false" AllowSorting="True"
            AllowPaging="true" PageSize="100" OnRowCommand="gvActivity_RowCommand" Width="100%"
            OnRowDataBound="gvActivity_RowDataBound" OnSorting="gvActivity_Sorting" OnPageIndexChanging="gvActivity_PageIndexChanging">
            <Columns>
                <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="IndicatorActivityId" HeaderText="Id" HeaderStyle-Width="50px"
                    SortExpression="IndicatorActivityId" />
                <asp:BoundField DataField="EmergencyName" HeaderText="Emergency" SortExpression="EmergencyName"
                    ItemStyle-Width="150px" />
                <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName"
                    ItemStyle-Width="100px" />
                <asp:BoundField DataField="ObjectiveName" HeaderText="Objective" SortExpression="ObjectiveName"
                    ItemStyle-Width="250px" />
                <asp:BoundField DataField="IndicatorName" HeaderText="Indicator" SortExpression="IndicatorName"
                    ItemStyle-Width="250px" />
                <asp:TemplateField ItemStyle-Width="250px" HeaderText="Activity" SortExpression="ActivityName">
                    <ItemTemplate>
                        <asp:Label ID="lblActivityName" runat="server" Text='<%# Eval("ActivityName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ActivityType" HeaderText="Activity Type" SortExpression="ActivityName" ItemStyle-Width="80px" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Date" SortExpression="CreatedDate"
                    Visible="false" />
                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                            CommandName="EditActivity" CommandArgument='<%# Eval("IndicatorActivityId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                            CommandName="DeleteActivity" CommandArgument='<%# Eval("IndicatorActivityId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblObjectiveIndicatorId" runat="server" Text='<%# Eval("ObjectiveIndicatorId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblClusterObjectiveId" runat="server" Text='<%# Eval("ClusterObjectiveId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblEmergencyClusterId" runat="server" Text='<%# Eval("EmergencyClusterId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblLocationEmergencyId" runat="server" Text='<%# Eval("LocationEmergencyId") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="ButtonFace" />
        </asp:GridView>
    </div>
</asp:Content>
