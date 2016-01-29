<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctlOutputIndicatorDataEntryCountry.ascx.cs" Inherits="SRFROWCA.ClustOutputIndicators.ctlOutputIndicatorDataEntryCountry" %>
<div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
    <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False"
        OnRowDataBound="gvIndicators_RowDataBound"
        CssClass="imagetable" Width="100%"
        EmptyDataText="There are no output indicators available!">
        <HeaderStyle BackColor="Control"></HeaderStyle>
        <RowStyle CssClass="istrow" />
        <AlternatingRowStyle CssClass="altcolor" />
        <Columns>
            <asp:TemplateField ItemStyle-Width="2%" HeaderText="#">
                <ItemTemplate>
                    <%# Container.DataItemIndex + 1 %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblClusterIndicatorID" runat="server" Text='<%# Eval("ClusterIndicatorID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="4%" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Image ID="imgRind" runat="server" />
                    <asp:Image ID="imgCind" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField ItemStyle-Width="46%" HeaderText="Indicator">
                <ItemTemplate>
                    <div style="word-wrap: break-word;">
                        <%# Eval("Indicator")%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Unit" HeaderText="Unit" ItemStyle-Width="10%"></asp:BoundField>
            <asp:TemplateField ItemStyle-Width="8%" HeaderText="Target" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblTarget" runat="server" Text=' <%# Eval("Target")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
          <asp:TemplateField ItemStyle-Width="8%"
                                ItemStyle-BackColor="LightYellow"
                                ItemStyle-HorizontalAlign="Right"
                                HeaderText="Monthly Achieved">
                                <ItemTemplate>
                                    <div style="word-wrap: break-word;">
                                        <asp:TextBox runat="server" MaxLength="8" Width="100%" ID="txtAchieved"
                                            CssClass="numeric1" Style="text-align: right;" Text='<%# Eval("Achieved") %>'></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="8%" HeaderText="<span class='tooltip2' title='Caculated on the basis of Calculation Method of the Indicator.'>Running Value</span>" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblSum" runat="server" Text=' <%# Eval("RunningValue")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="8%" HeaderText="<span class='tooltip2' title='Each Indicator has assigned a calcuation method type.</br>Sum: Sum of all monthly achieved.</br>Agerage: Average of all monthly achieved.</br>Max: Max data reported in any month.</br>Latest: Latest data reported.'>Calculation Method</span>" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblCalcMethod" ToolTip="some text here" runat="server" Text=' <%# Eval("IndicatorCalculationType")%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblCountryID" runat="server" Text='<%# Eval("CountryID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblClusterID" runat="server" Text='<%# Eval("ClusterID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="IsRegional" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
            <asp:BoundField DataField="IsSRP" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden"></asp:BoundField>
            <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="CreatedBy" HeaderText="Reported By" ItemStyle-Width="7%"></asp:BoundField>
            <asp:BoundField ItemStyle-Font-Size="Smaller" DataField="UpdatedBy" HeaderText="Updated By" ItemStyle-Width="7%"></asp:BoundField>
        </Columns>
    </asp:GridView>
    <%--<hr />
            <div class="pull-right">
            <asp:Button ID="btnSave2" runat="server" Text="Save" class="btn btn-primary" OnClientClick="return validate();" OnClick="btnSaveAll_Click" />
                </div>--%>
</div>
