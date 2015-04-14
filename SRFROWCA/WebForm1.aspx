<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SRFROWCA.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="//code.jquery.com/jquery-1.11.2.min.js"></script>
    <script src="http://code.highcharts.com/highcharts.js"></script>
    <script src="http://code.highcharts.com/modules/heatmap.js"></script>
    <script src="http://code.highcharts.com/modules/exporting.js"></script>

    <script>
        $(function () {

            $('#container').highcharts({

                chart: {
                    type: 'heatmap',
                    marginTop: 40,
                    marginBottom: 80
                },


                title: {
                    text: 'Sales per employee per weekday'
                },

                xAxis: {
                    categories: ['Alexander', 'Marie', 'Maximilian', 'Sophia', 'Lukas', 'Maria', 'Leon', 'Anna', 'Tim', 'Laura']
                },

                yAxis: {
                    categories: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'],
                    title: null
                },

                colorAxis: {
                    min: 0,
                    minColor: '#FFFFFF',
                    maxColor: Highcharts.getOptions().colors[0]
                },

                legend: {
                    align: 'right',
                    layout: 'vertical',
                    margin: 0,
                    verticalAlign: 'top',
                    y: 25,
                    symbolHeight: 280
                },

                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.xAxis.categories[this.point.x] + '</b> sold <br><b>' +
                            this.point.value + '</b> items on <br><b>' + this.series.yAxis.categories[this.point.y] + '</b>';
                    }
                },

                series: [{
                    name: 'Sales per employee',
                    borderWidth: 1,
                    data: [[0, 0, 10], [0, 1, 19], [0, 2, 8], [0, 3, 24], [0, 4, 67], [1, 0, 92], [1, 1, 58], [1, 2, 78], [1, 3, 117], [1, 4, 48], [2, 0, 35], [2, 1, 15], [2, 2, 123], [2, 3, 64], [2, 4, 52], [3, 0, 72], [3, 1, 132], [3, 2, 114], [3, 3, 19], [3, 4, 16], [4, 0, 38], [4, 1, 5], [4, 2, 8], [4, 3, 117], [4, 4, 115], [5, 0, 88], [5, 1, 32], [5, 2, 12], [5, 3, 6], [5, 4, 120], [6, 0, 13], [6, 1, 44], [6, 2, 88], [6, 3, 98], [6, 4, 96], [7, 0, 31], [7, 1, 1], [7, 2, 82], [7, 3, 32], [7, 4, 30], [8, 0, 85], [8, 1, 97], [8, 2, 123], [8, 3, 64], [8, 4, 84], [9, 0, 47], [9, 1, 114], [9, 2, 31], [9, 3, 48], [9, 4, 91]],
                    dataLabels: {
                        enabled: true,
                        color: '#000000'
                    }
                }]

            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <div id="container" style="height: 400px; min-width: 310px; max-width: 800px; margin: 0 auto"></div>

        <%-- <div>
            
            <script type="text/javascript">
                // Popup window code
                function newPopup(url) {
                    popupWindow = window.open(
                        url, 'popUpWindow', 'height=700,width=800,left=10,top=10,resizable=yes,scrollbars=yes,toolbar=yes,menubar=no,location=no,directories=no,status=yes')
                }
            </script>
            <a href="JavaScript:newPopup('../ops/opsdataentry.aspx?uid=1883&pid=78611&ProjectRevision=O&clname=coordinationandsupportservices&cname=chad&key=44SceGCNSjHPKW4j+yjncQ==&clname2=coordinationandsupportservices');">Mali WASH</a>
            <br />
            <a href="JavaScript:newPopup('../ops/opsdataentry.aspx?uid=1&pid=90893&clname=food security&cname=senegal');">Sahel Region</a>
            <br />
            <a href="JavaScript:newPopup('../ops/opsdataentry.aspx?uid=1&pid=75778&clname=food security&cname=senegal');">Senegal Nutrition</a>
            <br />
            <a href="JavaScript:newPopup('../ops/opsdataentry.aspx?uid=1&pid=7499975800&clname=food security&cname=mali');">Mali Food Security</a>

        </div>--%>
    </form>
</body>
</html>
