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

        .bgcolorthispage {
            background-color: #2e6589;
        }
        .alertcustome {
            background-color: #458FBA;
            border-color: #bce8f1;
            color: #ffffff;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="col-sm-12 infobox-container">
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
       
        <div class="outdiv">
            <iframe src="SRP2015ORS.html" class="iniframe"></iframe>
        </div>

        <div class="outdiv">
            <iframe src="SRP2015ORSregion.html" class="iniframe"></iframe>
        </div>
    </div>--%>
    <div class="page-content">
        <div class="row">
            <div class="col-xs-12">
                <div style="height: 10px;" class="bgcolorthispage"></div>
                
                <div align="center" class="bgcolorthispage">
                    <div class="alert alertcustome" style="width:910px; font-size:11px; text-align:left">
                        <button type="button" class="close" data-dismiss="alert">
                            <i class="ace-icon fa fa-times"></i>
                        </button>

                        <i class="ace-icon fa fa-check green"></i>

                        During the 2015 Strategic Response Plan (SRP), The Sahel output indicators were defined in consultation with the regional 
                        sector focal points to provide a standard measure of performance for the Sahel. Country Cluster focal points provided their 
                        annual targets for each country and started to report on their cluster achievements for each of the output indicators on a 
                        monthly basis.<br />
                        The report below shows the performance of the Sahel Output Indicators on a quarterly basis for 2015. The monthly data is 
                        collected from the respective cluster focal points across the 9 Sahel countries (Burkina Faso, Cameroon, Chad, Gambia, Mali, 
                        Mauritania, Niger, Nigeria and Senegal) and validated with the regional sector focal points.<br />
                        The sectors represented at a regional level and included in the report include; Food Security, Nutrition, Health, 
                        Water Sanitation & Hygiene, Multi-sector for refugees, Education and protection.<br />
                        The country by country breakdown can also be viewed using the filters provided.

                    </div>
                    <iframe width="910" height="800" frameborder="0" scrolling="no" src="https://onedrive.live.com/embed?cid=880D08EC1E54FF60&resid=880d08ec1e54ff60%21527&authkey=ACcgJWLViOYr75g&em=2&AllowTyping=True&wdHideGridlines=True&wdHideHeaders=True&wdDownloadButton=True"></iframe>
                    <div style="position: relative; top: -55px; left: 0px; width: 910px; height: 60px; background: #2e6589;"></div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
