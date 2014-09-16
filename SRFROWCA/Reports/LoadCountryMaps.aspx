<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LoadCountryMaps.aspx.cs" Inherits="SRFROWCA.Reports.LoadCountryMaps" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">

        .primaryBtn {
-moz-user-select: none;
-webkit-border-radius: 2px;
-moz-border-radius: 2px;
-ms-border-radius: 2px;
-o-border-radius: 2px;
border-radius: 2px;
cursor: pointer;
font-weight: normal;
font-size: 14px;
padding: 4px 20px;
text-align: center;
text-decoration: none;
background-color: #3191f2;
background-image: -webkit-gradient(linear,50% 0,50% 100%,color-stop(0%,#3191f2),color-stop(100%,#2c83da));
background-image: -webkit-linear-gradient(#3191f2,#2c83da);
background-image: -moz-linear-gradient(#3191f2,#2c83da);
background-image: -o-linear-gradient(#3191f2,#2c83da);
background-image: linear-gradient(#3191f2,#2c83da);
border: 1px solid #2B82D9;
color: #FFF;
width: auto;
display: inline-block;
}
    </style>
    <div style="width:100%;float:left;">
        
        
        <div style="width:90%;margin:0 auto;margin-top:30px;">
            <div style="float:left;"><h3><asp:Literal runat="server" ID="ltrlFileName"></asp:Literal></h3></div><div style="float:right;margin-top:10px;"><asp:Button runat="server" ID="btnDownload" CssClass="primaryBtn" Text="Download" OnClick="btnDownload_Click" /></div>
   <iframe src="<%=url%>" id="pdfViewer" height="600px" style="margin:0 auto;width:100%;"></iframe>
        </div>
        </div>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#pdfViewer").css("height", $(window).height() - 150);
        });

    </script>
</asp:Content>
