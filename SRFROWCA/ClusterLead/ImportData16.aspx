<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData16.aspx.cs" Inherits="SRFROWCA.ClusterLead.ImportData16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <asp:label id="lblMessage" runat="server" text=""></asp:label>
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <p>
                                Select Month You Are Reporting For:
                                    <asp:dropdownlist id="ddlMonth" runat="server">
                                    </asp:dropdownlist>
                            </p>

                            <p>
                                <asp:localize id="localUploadBrowseText" runat="server" text="Click 'Browse' To Select File."></asp:localize>
                            </p>
                            <p>
                                <asp:fileupload id="fuAchieved" runat="server" class="btn btn-grey" />
                            </p>
                            <p>
                                <asp:button id="btnImport" runat="server" text="Import" cssclass="btn btn-primary" onclick="btnImport_Click" />
                            </p>
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
            <!-- /.col -->
        </div>
        <!-- /.row -->
    </div>
</asp:Content>
