<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" ValidateRequest="false"
    CodeBehind="ChartReport.aspx.cs" Inherits="SRFROWCA.Reports.ChartReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%@ register assembly="DropDownCheckBoxes" namespace="Saplin.Controls" tagprefix="cc" %>
    <style type="text/css">
        .ddlwidth1 { width:100%;}
    </style>        
    <script>
        function getSVG1() {
            var svg = $('#Chart1_container').html();
            alert(svg);
            $.ajax({                
                type: "POST",
                url: "http://localhost:50464/WebService2.asmx/SaveSVG1",
                data: "{'svg':'" + svg + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d);
                }
            });
        }

        Highcharts.getSVG = function (charts) {
            var svgArr = [],
        top = 10,
        width = 0;

            $.each(charts, function (i, chart) {
                var svg = chart.getSVG();
                svg = svg.replace('<svg', '<g transform="translate(0,' + top + ')" ');
                svg = svg.replace('</svg>', '</g>');

                top += chart.chartHeight;
                width = Math.max(width, chart.chartWidth);

                svgArr.push(svg);
            });

            return '<svg height="' + top + '" width="' + width + '" version="1.1" xmlns="http://www.w3.org/2000/svg">' + svgArr.join('') + '</svg>';
        };

        /**
        * Create a global exportCharts method that takes an array of charts as an argument,
        * and exporting options as the second argument
        */
        Highcharts.exportCharts = function (charts, options) {
            var form
            svg = Highcharts.getSVG(charts);

            // merge the options
            options = Highcharts.merge(Highcharts.getOptions().exporting, options);

            // create the form
            form = Highcharts.createElement('form', {
                method: 'post',
                action: options.url
            }, {
                display: 'none'
            }, document.body);

            // add the values
            Highcharts.each(['filename', 'type', 'width', 'svg'], function (name) {
                Highcharts.createElement('input', {
                    type: 'hidden',
                    name: name,
                    value: {
                        filename: options.filename || 'chart',
                        type: options.type,
                        width: options.width,
                        svg: svg
                    }[name]
                }, null, form);
            });
            //console.log(svg); return;
            // submit
            form.submit();

            // clean up
            form.parentNode.removeChild(form);
        };

        function MyJueryFunction() {

            var list = new Array();
            $(".chartsclass").each(function (i, obj) {
                list.push($(obj).highcharts());
            });

            //            var chart = $('#Chart_container').highcharts();
            //            var chart2 = $('#Chart1_container').highcharts();
            Highcharts.exportCharts(list, {
                type: 'application/pdf',
                filename: 'my-pdf'
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" class="label1" border='1'>
        <tr>
            <td>
                Data:
            </td>
            <td colspan="5">
                <cc:DropDownCheckBoxes ID="ddlData" runat="server" CssClass="ddlwidth1"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlData_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Data" />
                </cc:DropDownCheckBoxes>                
            </td>
        </tr>
        <tr>
            <td>
                Country:
            </td>
            <td>
                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Width="200px"
                    OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td>
                Admin1:
            </td>
            <td>
                <cc:DropDownCheckBoxes ID="ddlAdmin1" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlAdmin1_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
            <td>
                Admin2:
            </td>
            <td>
                <cc:DropDownCheckBoxes ID="ddlLocations" runat="server" CssClass="ddlWidth" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlLocations_SelectedIndexChanged" AddJQueryReference="True"
                    meta:resourcekey="checkBoxes2Resource1" UseButtons="False" UseSelectAllNode="True">
                    <Style SelectBoxWidth="" DropDownBoxBoxWidth="" DropDownBoxBoxHeight=""></Style>
                    <Texts SelectBoxCaption="Select Location" />
                </cc:DropDownCheckBoxes>
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" name="btnname" value="Generate Report" onclick="MyJueryFunction()" />
                <input type="button" name="btnname" value="Generate Report" onclick="getSVG1()" />
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <div style="width: 100%; float: left;">
            <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
        </div>
        <%--<div style="width: 40%; float: left;">
            <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
        </div>--%>
    </div>
</asp:Content>
