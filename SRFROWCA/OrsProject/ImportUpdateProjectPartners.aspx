<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportUpdateProjectPartners.aspx.cs" Inherits="SRFROWCA.OrsProject.ImportUpdateProjectPartners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <div id="divMsg">
        </div>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <div class="row">
            <div class="col-xs-12">
                <!-- PAGE CONTENT BEGINS -->

                <div class="error-container">
                    <div class="well">
                        <div>
                            <p>
                                <asp:Localize ID="localUploadBrowseText" runat="server" Text="Click 'Browse' To Select File."></asp:Localize>
                            </p>
                            <p>
                                <asp:FileUpload ID="fuAchieved" runat="server" class="btn btn-grey" />
                            </p>
                            <p>
                                <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btn btn-primary" OnClick="btnImport_Click" />
                            </p>
                        </div>
                    </div>
                </div>
                <!-- PAGE CONTENT ENDS -->
            </div>
        </div>
    </div>
    
</asp:Content>
