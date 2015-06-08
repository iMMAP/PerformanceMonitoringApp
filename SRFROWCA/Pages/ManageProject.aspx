<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageProject.aspx.cs" Inherits="SRFROWCA.Pages.ManageProject" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="CreateProject.ascx" TagName="CreateProject" TagPrefix="uc1" %>
<%@ Register Src="ProjectActivities.ascx" TagName="ProjectActivities" TagPrefix="uc2" %>
<%@ Register src="ProjectPartners.ascx" tagname="ProjectPartners" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style type="text/css">
        .MyTabStyle .ajax__tab_header
        {
            font-family: "Helvetica Neue" , Arial, Sans-Serif;
            font-size: 14px;
            font-weight:bold;
            display: block;
        }
        .MyTabStyle .ajax__tab_header .ajax__tab_outer
        {
            border-color: #222;
            color: #222;
            padding-left: 10px;
            margin-right: 3px;
            border:solid 1px #d7d7d7;
        }
        .MyTabStyle .ajax__tab_header .ajax__tab_inner
        {
            border-color: #666;
            color: #666;
            padding: 3px 10px 2px 0px;
        }
        .MyTabStyle .ajax__tab_hover .ajax__tab_outer
        {
            background-color:#DBDBDB;
        }
        .MyTabStyle .ajax__tab_hover .ajax__tab_inner
        {
            color: #fff;
        }
        .MyTabStyle .ajax__tab_active .ajax__tab_outer
        {
            border-bottom-color: #ffffff;
            background-color: #d7d7d7;
        }
        .MyTabStyle .ajax__tab_active .ajax__tab_inner
        {
            color: #000;
            border-color: #333;
        }
        .MyTabStyle .ajax__tab_body
        {
            font-family: verdana,tahoma,helvetica;
            font-size: 10pt;
            background-color: #fff;
            border-top-width: 0;
            border: solid 1px #d7d7d7;
            border-top-color: #ffffff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="page-content">
        <div id="divMsg">
        </div>
        
        <asp:TabContainer runat="server" ID="tabManageProject" ActiveTabIndex="0" CssClass="MyTabStyle">
            <asp:TabPanel runat="server" ID="tpnlCreateProject" HeaderText="Create Project">
                <ContentTemplate>
                    <uc1:CreateProject ID="ctlCreateProject" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpnlActivities" HeaderText="Project Activities">
                <ContentTemplate>
                    <uc2:ProjectActivities ID="ctlProjectActivities" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpnlPartners" HeaderText="Project Partners">
                <ContentTemplate>
                    <uc3:ProjectPartners ID="ctlProjectPartnes" runat="server" />
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
    </div>

</asp:Content>
