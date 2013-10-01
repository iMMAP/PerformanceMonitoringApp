<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Contactus.aspx.cs" Inherits="SRFROWCA.ContactUs.Contactus" %>

<%@ Register Src="ContactUsControl.ascx" TagName="ContactUsControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:ContactUsControl ID="ContactUsControl1" runat="server" />
    &nbsp;
</asp:Content>
