<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SahelIndicatorTargets.aspx.cs" Inherits="SRFROWCA.ClusterLead.SahelIndicatorTargets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $(function () {
            $(".numeric1").wholenumber();
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <table border="0" style="margin: auto; width: 100%">
            <tr>
                <td>
                    <label>
                        Clsuter:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCluster" runat="server" Width="300px" AutoPostBack="true" OnSelectedIndexChanged="ddlCluster_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>

                <td>
                    <label>
                        Country:</label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" Width="300px">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ErrorMessage="Required" Display="Dynamic"
                        CssClass="error2" InitialValue="0" Text="Required" ControlToValidate="ddlCountry"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
                <td style="text-align: right;">
                    <asp:Button runat="server" ID="btnSave" Text="Save" class="width-20 btn btn-sm" OnClick="btnSave_Click" />
                </td>
            </tr>
        </table>
        <div class="table-responsive">
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvSahelIndTargets" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                    CssClass=" table-striped table-bordered table-hover" Width="100%"
                    EmptyDataText="There are no output indicators available!" DataKeyNames="ClusterIndicatorId">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="2%" HeaderText="#" meta:resourcekey="TemplateFieldResource1">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>

                            <ItemStyle Width="2%"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ClusterIndicator" HeaderText="Sahel (Regional) Indicator" SortExpression="ClusterIndicator" ItemStyle-Width="70%" />
                        <asp:BoundField DataField="RunningSum" HeaderText="Running Sum" SortExpression="RunningSum" />
                        <asp:TemplateField HeaderText="Target" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTarget" runat="server" CssClass="numeric1" Text='<%# Eval("CountryClusterTarget") %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
