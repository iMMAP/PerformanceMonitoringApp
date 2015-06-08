<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountryMaps.aspx.cs" Inherits="SRFROWCA.Reports.CountryMaps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {
            $(".imagetable1 tr:even").css("background-color", "#F4F4F8");
            $(".imagetable1 tr:odd").css("background-color", "#EFF1F1");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="page-content">
        <div class="row">
            <table style="margin: 0 auto; width: 70%">
                <tr>
                    <td>
                        <h2>
                            <asp:Label ID="lblMessage" runat="server" Text="Sahel - ORS Maps will be uploaded soon!"></asp:Label>
                            <asp:Label ID="lblCountryName" runat="server" Text=""></asp:Label></h2>
                        <asp:Repeater ID="rptCountry" runat="server" OnItemDataBound="rptrptCountry_ItemDataBound">
                            <ItemTemplate>
                                <h3><%#Eval("LocationName")%></h3>
                                <asp:HiddenField ID="hfLocationId" runat="server" Value='<%#Eval("EmergencyLocationId")%>' />

                                <asp:GridView ID="gvMaps" runat="server" AutoGenerateColumns="false" GridLines="None" Width="90%" ShowHeader="false">
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="MyLink" Target="_blank"
                                                    NavigateUrl='<%# Eval("MapURL", "~/orsmaps/{0}")  %>' runat="server" Text='<%#Eval("MapTitle")%>'></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
