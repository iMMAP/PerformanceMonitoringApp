<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SRFROWCA.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div class="row">
            <div class="col-sm-9 widget-container-span">
                <div class="widget-box">
                    <div class="widget-body">
                        <div class="widget-main">
                            <div id="scrolledGridView" style="overflow-x: auto; width: 100%;">
                                <asp:GridView ID="gvIndicators" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                    CssClass="imagetable">
                                    <HeaderStyle BackColor="Control"></HeaderStyle>
                                    <RowStyle CssClass="istrow" />
                                    <AlternatingRowStyle CssClass="altcolor" />
                                    <Columns>
                                        <asp:BoundField DataField="ActivityName" HeaderText="Activity" ItemStyle-CssClass="testact"
                                            SortExpression="ActivityName">
                                            <ItemStyle CssClass="testact"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PriorityActivityId" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
