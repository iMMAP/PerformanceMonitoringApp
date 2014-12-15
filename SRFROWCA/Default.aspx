<%@ Page Title="ORS - Dashboard" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SRFROWCA._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Welcome page - ORS</title>
    <style type="text/css">
       

        .textwelcome
        {
            font-size: 20px;
            color: #438eb9;
            padding: 10px;
        }

        .textpublic
        {
            font-size: 14px;
            color: #438eb9;
        }

        .blocparteners
        {
            float: left;
            width: 349px;
            height: 120px;
            padding: 10px;
            margin-right: 30px;
            color: #FFF;
            font-size: 16px;
            background: #ff7b02; /* Old browsers */
            background: -moz-linear-gradient(45deg, #ff7b02 2%, #ff8f02 99%); /* FF3.6+ */
            background: -webkit-gradient(linear, left bottom, right top, color-stop(2%,#ff7b02), color-stop(99%,#ff8f02)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(45deg, #ff7b02 2%,#ff8f02 99%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(45deg, #ff7b02 2%,#ff8f02 99%); /* Opera 11.10+ */
            background: -ms-linear-gradient(45deg, #ff7b02 2%,#ff8f02 99%); /* IE10+ */
            background: linear-gradient(45deg, #ff7b02 2%,#ff8f02 99%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ff7b02', endColorstr='#ff8f02',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
        }

            .blocparteners:hover
            {
                float: left;
                width: 349px;
                height: 120px;
                padding: 10px;
                margin-right: 30px;
                color: #FFF;
                font-size: 16px;
                background: #da6c06; /* Old browsers */
                background: -moz-linear-gradient(45deg, #da6c06 2%, #ff8f02 99%); /* FF3.6+ */
                background: -webkit-gradient(linear, left bottom, right top, color-stop(2%,#da6c06), color-stop(99%,#ff8f02)); /* Chrome,Safari4+ */
                background: -webkit-linear-gradient(45deg, #da6c06 2%,#ff8f02 99%); /* Chrome10+,Safari5.1+ */
                background: -o-linear-gradient(45deg, #da6c06 2%,#ff8f02 99%); /* Opera 11.10+ */
                background: -ms-linear-gradient(45deg, #da6c06 2%,#ff8f02 99%); /* IE10+ */
                background: linear-gradient(45deg, #da6c06 2%,#ff8f02 99%); /* W3C */
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ff7b02', endColorstr='#ff8f02',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            }

        .bloccluster
        {
            float: left;
            width: 349px;
            height: 120px;
            padding: 10px;
            color: #FFF;
            font-size: 16px;
            background: #528d8d; /* 00cbcb Old browsers */
            background: -moz-linear-gradient(45deg, #528d8d 2%, #68b7b7 99%); /* FF3.6+ */
            background: -webkit-gradient(linear, left bottom, right top, color-stop(2%,#528d8d), color-stop(99%,#68b7b7)); /* Chrome,Safari4+ */
            background: -webkit-linear-gradient(45deg, #528d8d 2%,#68b7b7 99%); /* Chrome10+,Safari5.1+ */
            background: -o-linear-gradient(45deg, #528d8d 2%,#68b7b7 99%); /* Opera 11.10+ */
            background: -ms-linear-gradient(45deg, #528d8d 2%,#68b7b7 99%); /* IE10+ */
            background: linear-gradient(45deg, #528d8d 2%,#68b7b7 99%); /* W3C */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ff7b02', endColorstr='#ff8f02',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
        }

            .bloccluster:hover
            {
                float: left;
                width: 349px;
                height: 120px;
                padding: 10px;
                color: #FFF;
                font-size: 16px;
                background: #099595; /* Old browsers */
                background: -moz-linear-gradient(45deg, #099595 2%, #01c1c1 99%); /* FF3.6+ */
                background: -webkit-gradient(linear, left bottom, right top, color-stop(2%,#099595), color-stop(99%,#01c1c1)); /* Chrome,Safari4+ */
                background: -webkit-linear-gradient(45deg, #099595 2%,#01c1c1 99%); /* Chrome10+,Safari5.1+ */
                background: -o-linear-gradient(45deg, #099595 2%,#01c1c1 99%); /* Opera 11.10+ */
                background: -ms-linear-gradient(45deg, #099595 2%,#01c1c1 99%); /* IE10+ */
                background: linear-gradient(45deg, #099595 2%,#01c1c1 99%); /* W3C */
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ff7b02', endColorstr='#ff8f02',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
            }

        .titrepartner
        {
            font-size: 18px;
            font-weight: 700;
        }

        .bouton
        {
            text-align: right;
            float: right;
        }

        #blocgraph
        {
            clear: both;
            width: 1060px;
            padding-top: 10px;
        }

        .btn-dashboard
        {
            float: left;
            width: 247px;
            height: 80px;
            padding: 10px;
            margin-right: 3px;
            color: #FFF;
            font-size: 20px;
            text-align: left;
            font-weight: 600;
            background: #a39c9c; /* Old browsers */
        }

        .bouton2
        {
            float: right;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="breadcrumbs" id="breadcrumbs">
        <script type="text/javascript">
            try { ace.settings.check('breadcrumbs', 'fixed') } catch (e) { }
        </script>
        <ul class="breadcrumb">
            <li><i class="icon-home home-icon"></i><a href="#">Home</a> </li>
            <li class="active">Dashboard</li>
        </ul>
        <!-- .breadcrumb -->
    </div>
    <div class="page-content">
        
                <p class="textwelcome">Welcome to the Online Reporting System (ORS) for the Sahel</p>

                <div class="blocparteners">

                    <span class="titrepartner">Partners</span><br />
                    <span>If you are a partner and want to report on your activity click here to log in</span><br />
                    <a href="../Account/Login.aspx" class="bouton">
                        <img src="assets/himages/suivant-ors.png" class="bouton" /></a>

                </div>

                <div class="bloccluster">

                    <span class="titrepartner">Cluster Coordinators</span><br />
                    <span>If you are a partner and want to report on your activity click here to log in</span><br />
                    <a href="../Account/Login.aspx" class="bouton">
                        <img src="assets/himages/suivant-ors.png" class="bouton" /></a>

                </div>

                <div id="blocgraph">
                    <div style="background: #a39c9c; width: 748px; padding: 10px; clear: both; color: #FFF; font-weight: 700; font-size: 18px;">Visitors</div>
                    <div class="btn-dashboard">
                        <a href="#" style="color: #FFF;">Dashboard</a>
                        <img src="assets/himages/activity_analysis_100px.png" style="float: left;" />
                        <a href="#" class="bouton">
                            <img src="assets/himages/suivant-ors.png" class="bouton2" /></a>

                    </div>

                    <div class="btn-dashboard">
                        <a href="#" style="color: #FFF;">Data</a>
                        <img src="assets/himages/activity_scale_operation_100px.png" style="float: left;" />
                        <a href="#" class="bouton">
                            <img src="assets/himages/suivant-ors.png" class="bouton2" /></a>

                    </div>

                    <div class="btn-dashboard">
                        <a href="#" style="color: #FFF;">3W Maps</a>
                        <img src="assets/himages/SAH_Countries2.png" style="float: left; margin-right: 5px;" />
                        <a href="#" class="bouton">
                            <img src="assets/himages/suivant-ors.png" class="bouton2" /></a>

                    </div>

                </div>
    </div>
</asp:Content>
