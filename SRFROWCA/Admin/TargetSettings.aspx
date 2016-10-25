<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TargetSettings.aspx.cs" Inherits="SRFROWCA.Admin.TargetSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("#<%=txtDate.ClientID%>").datepicker({
                numberOfMonths: 1,
                dateFormat: "dd-mm-yy",
                onSelect: function (selected) {
                    $("#<%=txtDate.ClientID%>").datepicker("option", "minDate", selected)
                }
            });

            $(function () {
                $(".numeric1").wholenumber();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div class="tab-content no-border ">
                    <table>
                        <tr>
                            <td>Year:
                                                                
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFrameworkYear" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSelected">
                                    <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                    <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Country:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCountry" Width="270"
                                    OnSelectedIndexChanged="ddlSelected"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Cluster:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" AppendDataBoundItems="true" ID="ddlCluster" Width="270"
                                    OnSelectedIndexChanged="ddlSelected"
                                    AutoPostBack="true">
                                </asp:DropDownList>

                            </td>
                        </tr>                   
                        <tr>
                            <td>Deadline:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDate" Width="270"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" Text="Required" ControlToValidate="txtDate"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Number of Output Indicators Allowed:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtNoClusterIndicators"
                                    Width="270" CssClass="numeric1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClsInd" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" Text="Required" ControlToValidate="txtNoClusterIndicators"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td>Number of Activities Allowed:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtNoActivitiesFramework"
                                    Width="270" CssClass="numeric1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAct" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" Text="Required" ControlToValidate="txtNoActivitiesFramework"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td>Number of Activity Indicators Allowed:</td>
                            <td>
                                <asp:TextBox runat="server" ID="txtNoIndicatorsFramework"
                                    Width="270" CssClass="numeric1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvActInd" runat="server" ErrorMessage="Required" Display="Dynamic"
                                    CssClass="error2" Text="Required" ControlToValidate="txtNoIndicatorsFramework"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td>Provide Target:
                            </td>
                            <td>
                                <asp:RadioButton ID="rbTargetYes" runat="server" Text="Yes" Checked="true" GroupName="Target" />
                                <asp:RadioButton ID="rbTargetNo" runat="server" Text="No" GroupName="Target" />
                            </td>
                        </tr>
                        <tr>
                            <td>Admin Level:</td>
                            <td>
                                <asp:DropDownList ID="ddlLevel" runat="server" Width="200px">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Admin2" Value="Admin2"></asp:ListItem>
                                    <asp:ListItem Text="Admin1" Value="Admin1"></asp:ListItem>
                                    <asp:ListItem Text="Country" Value="Country"></asp:ListItem>
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
                                <asp:RadioButton ID="rbMandatoryYes" runat="server" Text="Yes" Checked="true" GroupName="Mandatory" />
                                <asp:RadioButton ID="rbMandatoryNo" runat="server" Text="No" GroupName="Mandatory" />
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnSave" runat="server"
                                    OnClick="btnSave_Click"
                                    Text="Save" CssClass="btn btn-sm btn-primary" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label runat="server" ID="lblFrameworkSettings" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="overflow-x: auto; width: 100%">
                <asp:GridView ID="gvSettings" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" Width="100%"
                    CssClass="imagetable">
                    <Columns>
                        <asp:TemplateField ItemStyle-Width="5%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="Year" HeaderText="Year" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="Country" HeaderText="Country" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="Cluster" HeaderText="Cluster" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="DateLimit" HeaderText="End Date" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="ClusterIndicatorMax" HeaderText="Output Indicator" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="ActivityMax" HeaderText="Activity" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="IndicatorMax" HeaderText="Indicator" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="AdminLevel" HeaderText="Admin Level" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="AdminType" HeaderText="Location Type" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="IsTargetNeeded" HeaderText="Target" />
                        <asp:BoundField HeaderStyle-HorizontalAlign="Center" DataField="IsTargetMandatory" HeaderText="Mandatory" />

                       <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" Width="80px" CausesValidation="false"
                                    CommandName="DeleteConfig" CommandArgument='<%# Eval("AdminTargetSettingKey") %>'>

                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
</asp:Content>
