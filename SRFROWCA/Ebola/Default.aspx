<%@ Page Title="" Language="C#" MasterPageFile="~/Ebola/Ebola.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA.Ebola.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #outdiv
        {
            height: 800px;
            overflow: hidden;
            position: relative;
            width: 1100px;
        }

        #outdiv2
        {
            height: 800px;
            overflow: hidden;
            position: relative;
            width: 1100px;
        }

        #iniframe
        {
            height: 1200px;
            left: -2px;
            position: absolute;
            top: -170px;
            width: 1260px;
        }

        #iniframe2
        {
            height: 1200px;
            left: -40px;
            position: absolute;
            top: 10px;
            width: 1260px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div>
            <img src="http://sdr.ocharowca.info/logo-hdx.png" style="position: absolute; left: 1000px; top: 310px; z-index: 1000;" />
        </div>
        <div id="outdiv">
            <iframe src="https://data.hdx.rwlabs.org/ebola" scrolling="no" width="1000px" height="1200px" id="iniframe"></iframe>
        </div>
        <p></p>

        <div id="outdiv2">
            <iframe src="https://simonbjohnson.github.io/Ebola-3W-Dashboard/" scrolling="no" width="1000px" height="1200px" id="iniframe2"></iframe>
        </div>
    </div>
</asp:Content>
