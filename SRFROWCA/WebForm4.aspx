<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm4.aspx.cs" Inherits="SRFROWCA.WebForm4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>
    <script src="Scripts/hc/highcharts.js" type="text/javascript"></script>
    <script src="http://code.highcharts.com/modules/data.js"></script>

    <script type=>
        $(function () {
            $('#container').highcharts({
                data: {
                    table: document.getElementById('GridView1')
                },
                chart: {
                    type: 'column'
                },
                title: {
                    text: 'Data extracted from a HTML table in the page'
                },
                yAxis: {
                    allowDecimals: false,
                    title: {
                        text: 'Units'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br/>' +
                    this.y + ' ' + this.x.toLowerCase();
                    }
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="container" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
