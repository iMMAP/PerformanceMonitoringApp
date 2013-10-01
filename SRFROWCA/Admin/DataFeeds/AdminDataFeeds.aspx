<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="AdminDataFeeds.aspx.cs" Inherits="SRFROWCA.Admin.DataFeeds.AdminDataFeeds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
        .row {background-color:ButtonFace;}
        .altrow {background-color:#D0D0D0;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div style="overflow-x: auto; margin: 0 auto; width: 50%">
        <table width="100%" border="0" style="font-size:larger; margin: 0 auto;">
            <tr class="row">
                <td>
                    <a href="OrganizationsFeed.ashx">Orgaizations</a>
                </td>
                <td>
                    <a href="OfficesFeed.ashx">Offices</a>
                </td>
            </tr>
            <tr class="altrow">
                <td>
                    <a href="ClustersFeed.ashx">Clusters</a>
                </td>
                <td>
                    <a href="EmergencyFeed.ashx">Emergency</a>
                </td>
            </tr>
            <tr class="row">
                <td>
                    <a href="Admin1Feed.ashx">Admin1 Locations</a>
                </td>
                <td>
                    <a href="Admin2Feed.ashx">Admin2 Locations</a>
                </td>
            </tr>            
            <tr class="row">
                <td colspan='2'>
                    <a href="LogFrameDataFeed.ashx">Log Frame Data</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
