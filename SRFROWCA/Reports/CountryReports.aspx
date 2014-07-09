<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CountryReports.aspx.cs" Inherits="SRFROWCA.Reports.CountryReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(function () {
            $(".imagetable1 tr:even").css("background-color", "#F4F4F8");
            $(".imagetable1 tr:odd").css("background-color", "#EFF1F1");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="Div1">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">
                <asp:Localize ID="localBreadCrumbHome" runat="server" Text="Home"></asp:Localize></a> </li>
            <li class="active">
                <asp:Localize ID="localBreadCrumbCountryReports" runat="server" Text="Country Reports"></asp:Localize></li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <table style="margin: 0 auto; width: 70%">
                <tr>
                    <td>
                        <h2>
                            <asp:Label ID="lblCountryName" runat="server" Text=""></asp:Label></h2>
                        <div class="hr hr-18 dotted hr-double"></div>
                        <asp:Repeater ID="rptReportTypes" runat="server" OnItemDataBound="rptReportTypes_ItemDataBound">
                            <ItemTemplate>
                                <h3><%#Eval("ReportTypeTitle")%></h3>
                                <asp:HiddenField ID="hfReportTypeTitle" runat="server" Value='<%#Eval("ReportTypeId")%>' />

                                <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="false" GridLines="None"
                                    OnRowCommand="gvReports_RowCommand" 
                                    OnRowCreated="gvReports_RowCreated" Width="90%">
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex + 1%>.
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a href="<%#Eval("ReportURL")%>" target="_blank">
                                                    <%#Eval("ReportTitle")%>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="btnDelete" runat="server" CommandName="DeleteReport" CommandArgument='<%# Eval("CountryReportId") %>' Text="Remove" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlnkPDF" runat="server" NavigateUrl='<%#Eval("ReportURL") %>' Target="_blank"><img src="../assets/orsimages/pdf.png" /></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                                <%--<asp:Repeater ID="rptReports" runat="server" OnItemCommand="rptReports_ItemCommand">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td>
                                                    <%#Container.ItemIndex+1 %>:&nbsp;
                                                </td>
                                                <td>
                                                    
                                                    <a href="<%#Eval("ReportURL")%>" target="_blank">
                                                        <%#Eval("ReportTitle")%>
                                                    </a>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnDelete" runat="server" CommandName="DeleteReport" CommandArgument='<%# Eval("CountryReportId") %>' Text="Remove" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>--%>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
