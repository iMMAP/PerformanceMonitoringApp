using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNet.Highcharts.Options;
using System.Data;
using DotNet.Highcharts.Helpers;
using System.Collections;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using System.Drawing;

namespace SRFROWCA.Reports
{
    public static class ReportsCommon
    {
        public static Series[] GetSeries(DataTable dt)
        {
            Series[] series = new Series[2];
            int i = 0;

            foreach (DataColumn column in dt.Columns)
            {
                if (column.Caption == "Target" || column.Caption == "Achieved")
                {
                    object[] locationSeries = (from DataRow row in dt.Rows
                                               select row[column.Caption]).ToArray();

                    series[i] = new Series
                    {
                        Name = column.Caption,
                        Data = new Data(locationSeries.ToArray()),
                        Color = column.Caption.Equals("Target") ? ColorTranslator.FromHtml("#1F77B4") : ColorTranslator.FromHtml("#FF7F0E")
                        
                        //PlotOptionsColumn = new PlotOptionsColumn{
                        //    PointWidth = 10
                        //}
                    };

                    i++;
                }
            }

            return series;
        }

        public static Series[] GetPercentageSeries(DataTable dt)
        {
            Series[] series = new Series[1];

            foreach (DataColumn column in dt.Columns)
            {
                if (column.Caption == "WorkDone")
                {
                    object[] locationSeries = (from DataRow row in dt.Rows
                                               select row[column.Caption]).ToArray();
                    series[0] = new Series
                    {
                        Name = "Percentage",
                        Data = new Data(locationSeries.ToArray()),

                        Color = ColorTranslator.FromHtml("#595959"),
                        PlotOptionsColumn = new PlotOptionsColumn
                        {
                            PointWidth = 20
                        }
                    };
                }
            }

            return series;
        }

        // Get multiple selected values from drop down checkbox.
        public static string GetSelectedValues(object sender)
        {
            string ids = GetSelectedItems(sender);
            ids = !string.IsNullOrEmpty(ids) ? ids : null;
            return ids;
        }

        public static int? GetSelectedValue(DropDownList ddl)
        {
            int val = 0;
            int.TryParse(ddl.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        private static string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

        public static void UpdateNullColumns(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (!((column.Caption == "Location") || (column.Caption == "LocationId")))
                    {
                        if (row[column.Caption] == DBNull.Value)
                            row[column.Caption] = '0';
                    }
                }
            }
        }

        private static string[] GetCategories(DataTable dt)
        {
            object[] categories = (from DataRow row in dt.Rows
                                   select row["Location"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        public static string PrepareTargetAchievedChartData(DataTable dt, int uniqueVal1, int? uniqueVal2, int? uniqueVal3, int chartType)
        {
            string returnVal = "";
            Series[] series = GetSeries(dt);
            string[] categories = GetCategories(dt);

            Series[] pSeries = GetPercentageSeries(dt);
            string[] pCategories = GetCategories(dt);

            returnVal = DrawLocaitonChart(series, categories, uniqueVal1, uniqueVal2, uniqueVal3, chartType);
            returnVal += " " + DrawPercentageChart(pSeries, pCategories, uniqueVal1, uniqueVal2, uniqueVal3, chartType);

            return returnVal;
        }

        private static string DrawLocaitonChart(Series[] series, string[] category, int uniqueVal1, int? uniqueVal2, int? uniqueVal3, int chartType)
        {
            string chartName = uniqueVal3 != null ? "Chart" + uniqueVal1 + "_" + uniqueVal2 + "deys" + uniqueVal3 + "ye" :
                "Chart" + uniqueVal1;

            ChartTypes typeOfChart = (ChartTypes)chartType;

            Highcharts hc = new Highcharts(chartName)
                .InitChart(new Chart
                {
                    DefaultSeriesType = typeOfChart,
                    Width = 550,
                    Height = 380,
                    MarginTop = 10,
                })
                .SetLegend(new Legend
                {
                    Margin = 5
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
                .SetTitle(new Title
                {
                    Text = ""
                })
                .SetExporting(new Exporting
                {
                    Enabled = false
                })
                .SetTooltip(new Tooltip 
                {
                    Enabled = false
                })
                .SetXAxis(new XAxis
                {
                    Categories = category,
                    Labels = new XAxisLabels
                    {
                        Rotation = -30,
                        Align = HorizontalAligns.Right
                    }
                })
                .SetYAxis(new[]
                    {
                        new YAxis
                            {
                                Title = new YAxisTitle { Text = "" },                                
                            }
                    })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointWidth = 20,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = false
                        },
                    }
                })
                .SetSeries(series);

            return hc.ToHtmlString();

        }

        private static string DrawPercentageChart(Series[] series, string[] category, int uniqueVal1, int? uniqueVal2, int? uniqueVal3, int chartType)
        {
            string chartName = uniqueVal3 != null ? "Charp" + uniqueVal1 + "_" + uniqueVal2 + "deys" + uniqueVal3 + "ye" :
                "Charp" + uniqueVal1;

            ChartTypes typeOfChart = (ChartTypes)chartType;

            Highcharts hc = new Highcharts(chartName)
                .InitChart(new Chart
                {
                    DefaultSeriesType = typeOfChart,
                    Width = 550,
                    Height = 280,
                    MarginTop = 10
                })
                .SetLegend(new Legend
                {
                    Margin = 5
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
                .SetTitle(new Title
                {
                    Text = ""
                })
                .SetExporting(new Exporting
                {
                    Enabled = false
                })
                .SetTooltip(new Tooltip 
                {
                    Enabled = false
                })

                .SetXAxis(new XAxis
                {
                    Categories = category,
                    Labels = new XAxisLabels
                    {
                        Rotation = -30,
                        Align = HorizontalAligns.Right
                    }
                })
                .SetYAxis(new[]
                    {
                        new YAxis
                            {
                                Title = new YAxisTitle { Text = "" },
                                Max = 100,
                            }
                    })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointWidth = 20,
                        DataLabels = new PlotOptionsColumnDataLabels
                        {
                            Enabled = true,
                            Rotation = -90,
                            Color = ColorTranslator.FromHtml("#FFFFFF"),
                            Align = HorizontalAligns.Right,
                            X = 3,
                            Y = 2,
                            Formatter = "function() { return this.y; }",
                            Style = "font: 'normal 13px Verdana, sans-serif'"
                        },
                    }
                })
                .SetSeries(series);
            return  hc.ToHtmlString();
        }

        public enum LocationType
        {
            Country = 1,
            Admin1 = 2,
            Admin2 = 3,
            None = -1
        }
    }
}