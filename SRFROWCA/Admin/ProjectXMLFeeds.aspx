<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectXMLFeeds.aspx.cs" Inherits="SRFROWCA.Admin.ProjectXMLFeeds" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div style="margin-left:25px;padding-top:25px;">
        <asp:Button ID="btnSubmit" runat="server" Text="Load Projects Feed" OnClick="btnSubmit_Click" CssClass="button_example " />
    
    </div>
    <br />
    <div style="margin-left:25px;padding-top:10px;">
    
        <asp:Label runat="server" ID="lblMessage" Text=""></asp:Label>
    </div>
   </asp:Content>
