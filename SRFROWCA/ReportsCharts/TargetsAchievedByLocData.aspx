<%@ Page Title="" Language="C#" MasterPageFile="~/NestedMasterPage1.master" AutoEventWireup="true"
    CodeBehind="TargetsAchievedByLocData.aspx.cs" Inherits="SRFROWCA.Reports.TargetsAchievedByLocData" %>

<%@ Register Assembly="DropDownCheckBoxes" Namespace="Saplin.Controls" TagPrefix="cc" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<script type="text/javascript" language="javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(PageLoadedEventHandler);
        function PageLoadedEventHandler() {
            DrawChart();

        }
    </script>--%>
    <script>
        Highcharts.getSVG = function (charts) {
            var svgArr = [],
        top = 0,
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

        function getSVG1() {
            $(".chartsclass").each(function (i, obj) {
                var svg = $(obj).html();
                var hfValue = $("#<%= hfChart.ClientID %>").val();
                $.ajax({
                    type: "POST",
                    url: "../WebService2.asmx/SaveSVG1",
                    data: "{'svg':'" + svg + "', i:'" + i + "', dataId:'" + hfValue + "' }",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        
                    },
                    error: function (msg) {
                        
                    }
                });
            });

            $.ajax({
                type: "POST",
                url: "../WebService2.asmx/GeneratePDF",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    
                },
                error: function (msg) {
                    
                }
            });
        }
        
        
    </script>
    <table width="100%" class="label1" border='0'>
        <tr>
            <td>
                Data:
            </td>
            <td colspan="5">
                <asp:DropDownList ID="ddlData" runat="server" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlData_SelectedIndexChanged">
                </asp:DropDownList>
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
                <input type="button" name="btnname" value="Generate Report" onclick="getSVG1()" />
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <div style="width: 58%; float: left;">
            <asp:Literal ID="ltrChart" runat="server" ViewStateMode="Disabled"></asp:Literal>
            <asp:HiddenField ID="hfChart" runat="server" ViewStateMode="Disabled" />
        </div>
        <div style="width: 40%; float: left;">
            <asp:Literal ID="ltrChartPercentage" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
