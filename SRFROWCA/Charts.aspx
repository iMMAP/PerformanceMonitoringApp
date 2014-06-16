<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Charts.aspx.cs" Inherits="SRFROWCA.Charts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <script src="http://code.highcharts.com/modules/exporting.js"></script>
    <script>
        $(function () {
            var temp = 'test';
            var svg = '123';
            var logFrameId = '1';
            var durationType = '1';
            var yearId = '1';
            var chartType = '1';
            $.ajax({
                type: "POST",
                url: "../ChartSVGService.asmx/SaveSVG",
                data: "{'svg':'" + svg + "', logFrameId:'" + logFrameId + "', durationType:'" + durationType + "', yearId:'" + yearId + "', chartType:'" + chartType + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var b = data.d;
                    var obj = jQuery.parseJSON(b);
                    alert(obj[0].LocationName);
                    
                },
                error: function (msg) {
                    alert('Failure');
                }
            });
        });
    </script>
    <script>
        $(function () {
            var catMonths = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
            var data = [
                    {
                        'name': 'Achieved',
                        'data': [289.00, 540.00, 48.00, 192.00, 514.00, 971.00, 754.00]
                    }
            ];
            $('#container').highcharts({
                chart: {
                    type: 'area'
                },
                title: {
                    text: 'Monthly Average Temperature',
                    x: -20 //center
                },
                subtitle: {
                    text: 'Source: WorldClimate.com',
                    x: -20
                },
                xAxis: {
                    categories: catMonths
                },
                yAxis: {
                    title: {
                        text: 'Temperature (°C)'
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    enabled: false,
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br/>' +
                        this.x + ': ' + this.y + '°C';
                    }
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: data
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto">
    </div>
</asp:Content>
