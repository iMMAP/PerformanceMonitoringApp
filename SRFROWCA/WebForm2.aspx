<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="SRFROWCA.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.2/jquery.min.js"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/jquery.dataTables.min.js"
        type="text/javascript"></script>
    <script src="Scripts/hc/highcharts.js" type="text/javascript"></script>
    <script src="http://code.highcharts.com/modules/data.js"></script>
    <script type="text/javascript" charset="utf-8">
        /* Data set - can contain whatever information you want */
        var aDataSet = [
				['Jan', 150, 100],
                ['Feb', 150, 150],
                ['Mar', 70, 50],
                ['Apr', 110, 90]
			];

        $(document).ready(function () {
            $('#dynamic').html('<table cellpadding="0" cellspacing="0" border="0" class="display" id="example"></table>');
            $('#example').dataTable({
                "aaData": aDataSet,
                "aoColumns": [
						{ "sTitle": "MonthName" },
						{ "sTitle": "Target" },
						{ "sTitle": "Achieved" }
					]
            });

            $('#container').highcharts({
                title: {
                    text: 'Cluster: 1037 EDUIndicator: % d'' écoles d''accueil et d''écoles du Nord recevant des kits enseignantsActivity: Distribution de kits enseignants>Data: Nombre d''écoles au Nord (source Ministère)'
                },
                data: {
                    table: document.getElementById('example')
                }
            });

            $("thead td").click(function () {
                alert('1');
                $('#container').highcharts({
                    data: {
                        table: document.getElementById('example')
                    }
                });
            });
        });
    </script>
</head>
<body id="dt_example">
    <form id="form1" runat="server">
    <div id="container" style="min-width: 400px; height: 400px; margin: 0 auto">
    </div>
    <div id="dynamic">
    </div>
    </form>
</body>
</html>
