function PieChart() {
    $(function () {
        $('#piechart-placeholder').highcharts({
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                height: 300
            },
            credits: {
                enabled: false
            },
            title: {
                text: ''
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.y}</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        color: '#000000',
                        connectorColor: '#000000'                        
                    }
                }
            },
            series: [{
                type: 'pie',
                name: 'Clusters',
                data: [
                ['Protection', 5739189255],
                ['Health', 4116353971],
                ['Nutrition', 2693950766],
                ['Food Security', 1136654946],
                ['Earyly Recovery', 794556139],
                ['WASH', 513598769],
                ['Emg Cluster', 432233435]
                ['Eduction', 162597949]
            ]
            }]
        });
    });
}