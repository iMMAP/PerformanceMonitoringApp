<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData16.aspx.cs" Inherits="SRFROWCA.ClusterLead.ImportData16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <div class="vspace-10"></div>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <div class="row">
            <div class="col-xs-8">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <h2>Import Data</h2>
                            <p>
                                Year: (Select Year To Import Data)
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChnaged">
                                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="2017" Value="13"></asp:ListItem>
                                    <asp:ListItem Text="2016" Value="12"></asp:ListItem>
                                </asp:DropDownList>
                            </p>
                            <p>
                                <asp:Localize ID="localUploadBrowseText" runat="server" Text="Click 'Browse' To Select File."></asp:Localize>
                            </p>
                            <p>
                                <asp:FileUpload ID="fuAchieved" runat="server" class="btn btn-grey" Enabled="false" />
                            </p>
                            <p>
                                <asp:Button ID="btnImport" runat="server" Text="Import" Enabled="false" CssClass="btn btn-primary" OnClick="btnImport_Click" />
                            </p>
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
            <div class="col-xs-12">
                <p>
                    <span id="spnWrongLocMessage" runat="server" visible="false" class="btn btn-danger btn-sm popover-error" data-rel="popover" data-placement="top">Following Data is not imported becuase these project(s) are using invalid locations (Admin Level) to report! Please contact Country OCHA Admin OR ORS-help-desk for further clarifications.</span>
                </p>
                <div class="tablegrid">
                    <div style="overflow-x: auto; width: 100%">
                        <asp:GridView ID="gvActivity" runat="server" AutoGenerateColumns="False" Width="100%"
                            CssClass="imagetable table-hover"
                            ShowHeaderWhenEmpty="False">
                            <RowStyle CssClass="istrow" />
                            <AlternatingRowStyle CssClass="altcolor" />
                            <Columns>
                                <asp:BoundField DataField="Month" HeaderText="Month" />
                                <asp:BoundField DataField="ProjectId" HeaderText="Project Id" />
                                <asp:BoundField DataField="ProjectCode" HeaderText="Project Code" />
                                <asp:BoundField DataField="LocationId" HeaderText="Loc Id" />
                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                <asp:BoundField DataField="TransactionId" HeaderText="TID" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <!-- /.row -->
        </div>
</asp:Content>
