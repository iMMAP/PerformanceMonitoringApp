<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportData16.aspx.cs" Inherits="SRFROWCA.ClusterLead.ImportData16" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <asp:label id="lblMessage" runat="server" text=""></asp:label>
        <div class="row">
            <div class="col-xs-8">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <h2>Import Data</h2>
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
            <div class="col-xs-4">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <p>This is only for testing purpose!</p>
                            <p> 
                                Export My Data.
                            </p>
                            <p>
                                <asp:button id="btnExport" runat="server" text="Export To Excel" cssclass="btn btn-primary" onclick="btnExport_Click" />
                            </p>
                            
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
             <div class="col-xs-8">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <h3><a href="../OrsProject/ImportUpdateProjectPartners.aspx">Import Partners</a></h3>
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
        </div>
        <!-- /.row -->
    </div>
</asp:Content>
