<%@ Page Title="ORS - Dashboard" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .outdiv {
            height: 750px;
            overflow: hidden;
            position: relative;
            width: 49%;
            float: left;
        }

        .iniframe {
            height: 100%;
            left: 2px;
            position: absolute;
            top: 10px;
            width: 100%;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="col-sm-12 infobox-container">
            <div class="infobox infobox-green  ">
                <div class="infobox-icon">
                    <img src="assets/orsimages/Partners.png" />
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">103</span>
                    <div class="infobox-content">Organisations</div>
                </div>

            </div>

            <div class="infobox infobox-blue  ">
                <div class="infobox-icon">
                    <img src="assets/orsimages/Appeals.png" />
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">10</span>
                    <div class="infobox-content">Appeals</div>
                </div>


            </div>

            <div class="infobox infobox-pink  ">
                <div class="infobox-icon">
                    <img src="assets/orsimages/Projects.png" />
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">517</span>
                    <div class="infobox-content">Projects</div>
                </div>

            </div>

            <div class="infobox infobox-red">
                <div class="infobox-icon">
                    <img src="assets/orsimages/Requirements.png" />
                </div>

                <div class="infobox-data">
                    <span class="infobox-data-number">$1.95 Billion</span>
                    <div class="infobox-content">Total Requirement</div>
                </div>
            </div>

        </div>
   
     
    <div>
       
        <%--<div class="outdiv">
            <iframe src="SRP2015ORS.html" class="iniframe"></iframe>
        </div>

        <div class="outdiv">
            <iframe src="SRP2015ORSregion.html" class="iniframe"></iframe>
        </div>--%>
    </div>
   

</asp:Content>
