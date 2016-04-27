<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ProjectPartners.aspx.cs" Inherits="SRFROWCA.OrsProject.ProjectPartners" Culture="auto"
    UICulture="auto" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function popitup() {
            //newwindow = window.open(url, 'name', 'height=700,width=1000', scro);
            var width = 700;
            var height = 600;
            var left = parseInt((screen.availWidth / 2) - (width / 2));
            var top = parseInt((screen.availHeight / 2) - (height / 2));
            var windowFeatures = "width=" + width + ",height=" + height +
                ",status,resizable,left=" + left + ",top=" + top +
                "screenX=" + left + ",screenY=" + top + ",scrollbars=yes";
            var projId = document.getElementById('<%= hfProjectId.ClientID %>').value;
            var url = "ProjectPartnersIndicators.aspx?pid=" + projId;
            newwindow = window.open(url, 'name', windowFeatures);
            if (window.focus) { newwindow.focus() }
            return false;
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        
        <div class="row">
            <div class="col-xs-12 col-sm-12 ">
                <div>
                    <asp:HiddenField ID="hfProjectId" runat="server" Value="" />
                    <div class="pull-left">
                        <h3>
                            <asp:Label ID="lblProjectCode" runat="server" Text=""></asp:Label></h3>
                    </div>
                    <div class="pull-right">
                        <a style="cursor: pointer;" class="pull-right btn btn-sm btn-warning" onclick="return popitup()">Assing Indicators to selected partners</a>
                    </div>
                </div>
                <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                    <h4>Project Partners</h4>
                    <asp:GridView ID="gvPartnerOrgs" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                        CssClass="imagetable" Width="100%"
                        OnRowCommand="gvPartnerOrgs_RowCommand" EmptyDataText="This project has no partners.">
                        <RowStyle CssClass="istrow" />
                        <AlternatingRowStyle CssClass="altcolor" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization Name" SortExpression="OrganizationName" />
                            <asp:BoundField DataField="OrganizationAcronym" HeaderText="Organization Acronym" SortExpression="OrganizationAcronym" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/assets/orsimages/delete16.png"
                                        CommandName="DeletePartner" CommandArgument='<%# Eval("OrganizationId") %>' ToolTip="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <EmptyDataTemplate>
                            <div class="no-record">No record found!</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>


                <div class="hr hr32 hr-dotted"></div>
                <h4>Add (More) Partners</h4>
                <div id="scrolledGridView1" style="overflow-x: auto; width: 100%; height: 250px;">
                    <asp:GridView ID="gvOrganization" runat="server" AutoGenerateColumns="false"
                        CssClass="imagetable" Width="100%"
                        OnRowCommand="gvOrganization_RowCommand">
                        <HeaderStyle CssClass="GridviewScrollHeader" />
                        <RowStyle CssClass="GridviewScrollItem" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <Columns>
                            <asp:TemplateField ItemStyle-CssClass="rownum" ItemStyle-Width="2%" HeaderText="#">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="OrganizationName" HeaderText="Organization Name" SortExpression="OrganizationName" />
                            <asp:BoundField DataField="OrganizationAcronym" HeaderText="Organization Acronym" SortExpression="OrganizationAcronym" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                HeaderStyle-Width="70">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/assets/orsimages/add.png"
                                        CommandName="AddPartner" CommandArgument='<%# Eval("OrganizationId") %>' ToolTip="Add" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>

                        <EmptyDataTemplate>
                            <div class="no-record">No record found!</div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
