<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ActivityData.aspx.cs" Inherits="SRFROWCA.Admin.Clusters.ActivityData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#txtSearch').on("keyup paste", function () {
                searchTable($(this).val());
            });
        });

        function searchTable(inputVal) {
            var table = $('#<%=gvData.ClientID %>');
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
                <asp:DropDownList ID="ddlIndicators" runat="server" Width="300px" OnSelectedIndexChanged="ddlIndicator_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvIndicators" runat="server" ErrorMessage="Required"
                    InitialValue="0" Text="Required" ControlToValidate="ddlIndicators"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Activity:
            </td>
            <td>
                <asp:DropDownList ID="ddlActivity" runat="server" Width="300px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfvIndActivity" runat="server" ErrorMessage="Required"
                    InitialValue="0" Text="Required" ControlToValidate="ddlActivity"></asp:RequiredFieldValidator>
            </td>
            <td>
                Unit:
            </td>
            <td>
                <asp:DropDownList ID="ddlUnit" runat="server" Width="300px">
                </asp:DropDownList>
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
                            <asp:TextBox ID="txtData" runat="server" Width="800px" TextMode="MultiLine" Height="80px"
                                MaxLength="500"></asp:TextBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvActivity" runat="server" ErrorMessage="Required"
                                Text="Required" ControlToValidate="txtData"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <asp:HiddenField ID="hfPKId" runat="server" />
            <td colspan="6" align="right">
                <asp:Button ID="btnAdd" runat="server" Text="Add/Update Data" OnClick="btnAdd_Click" />
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
        <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="false" AllowSorting="True" AllowPaging="true" PageSize="50"
            OnRowCommand="gvData_RowCommand" Width="100%" OnRowDataBound="gvData_RowDataBound"
            OnSorting="gvData_Sorting" OnPageIndexChanging="gvData_PageIndexChanging">
            <Columns>
                <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ActivityDataId" HeaderText="Id" HeaderStyle-Width="50px"
                    SortExpression="ActivityDataId" />
                <asp:BoundField DataField="EmergencyName" HeaderText="Emergency" SortExpression="EmergencyName"
                    ItemStyle-Width="150px" />
                <asp:BoundField DataField="ClusterName" HeaderText="Cluster" SortExpression="ClusterName"
                    ItemStyle-Width="100px" />
                <asp:BoundField DataField="ObjectiveName" HeaderText="Objective" SortExpression="ObjectiveName"
                    ItemStyle-Width="200px" />
                <asp:BoundField DataField="IndicatorName" HeaderText="Indicator" SortExpression="IndicatorName"
                    ItemStyle-Width="200px" />
                <asp:BoundField DataField="ActivityName" HeaderText="Activity" SortExpression="ActivityName"
                    ItemStyle-Width="200px" />
                <asp:TemplateField ItemStyle-Width="250px" HeaderText="Data" SortExpression="DataName">
                    <ItemTemplate>
                        <asp:Label ID="lblDataName" runat="server" Text='<%# Eval("DataName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit"  ItemStyle-Width="80px"/>
                <asp:BoundField DataField="CreatedDate" HeaderText="Date" SortExpression="CreatedDate" Visible="false" />
                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="80px" CausesValidation="false"
                            CommandName="EditData" CommandArgument='<%# Eval("ActivityDataId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                            CommandName="DeleteActivity" CommandArgument='<%# Eval("ActivityDataId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblIndicatorActivityId" runat="server" Text='<%# Eval("IndicatorActivityId") %>'></asp:Label>
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
