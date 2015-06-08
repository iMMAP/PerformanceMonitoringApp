<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectXMLFeeds.aspx.cs" Inherits="SRFROWCA.Admin.ProjectXMLFeeds" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-content">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 ">
                            <div class="widget-box">
                                <div class="widget-header widget-header-small header-color-blue2">
                                    <h6></h6>
                                </div>
                                <div class="widget-body">
                                    <div class="widget-main">
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <asp:Button ID="btnSubmit" runat="server" Text="Load Projects Feed" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div style="margin-left: 25px; padding-top: 10px;">

                                                <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>

        </table>
    </div>
</asp:Content>
