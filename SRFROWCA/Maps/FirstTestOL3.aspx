<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="FirstTestOL3.aspx.cs" Inherits="SRFROWCA.Maps.FirstTestOL3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="http://ol3js.org/en/master/css/ol.css" type="text/css">
    <style>
        .map
        {
            height: 400px;
            width: 100%;
        }
    </style>
    <script src="http://ol3js.org/en/master/build/ol.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="map" style="width: 100%, height: 400px"></div>
<script>
    new ol.Map({
        layers: [
      new ol.layer.Tile({ source: new ol.source.OSM() })
    ],
        view: new ol.View2D({
            center: [0, 0],
            zoom: 2
        }),
        target: 'map'
    });
</script>
</asp:Content>
