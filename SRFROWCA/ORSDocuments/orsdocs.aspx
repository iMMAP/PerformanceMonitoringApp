<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="orsdocs.aspx.cs" Inherits="SRFROWCA.ORSDocuments.orsdocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="../Default.aspx">Home</a></li>
            <li class="active">Documents</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="false"
                    CssClass="imagetable" Width="100%">
                    <HeaderStyle BackColor="Control"></HeaderStyle>
                    <RowStyle CssClass="istrow" />
                    <AlternatingRowStyle CssClass="altcolor" />
                    <Columns>
                        <asp:BoundField DataField="CountryName" HeaderText="Country" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="ClusterName" HeaderText="Cluster" ItemStyle-Width="15%" />
                        <asp:BoundField DataField="DocumentNamePublic" HeaderText="Document Name" ItemStyle-Width="60%" />
                        <asp:TemplateField HeaderText="Download">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnDocument" runat="server" Text="Download" OnClick="DownloadFile" CommandArgument='<%# Eval("DocumentURL") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
