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
                        PlotOptionsColumn = new PlotOptionsColumn
                        {
                            PointWidth = 20
                        },
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

        public static void DrawChart(DataTable dt, int j)
        {
            PrepareTargetAchievedChartData(dt, j);
            //PreparePercentageChartData(dt);
        }

        public static string PrepareTargetAchievedChartData(DataTable dt, int j)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = GetSeries(dt);
                DataRow dr = dt.Rows[0];
                string[] categories = GetCategories(dt);
                string title = GetChartTitle(dt);
                return DrawLocaitonChart(series, categories, j, title);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                return DrawLocaitonChart(series, categories, j, string.Empty);
            }
        }

        private static string GetChartTitle(DataTable dt)
        {
            string title = "";
            title = dt.Rows[0]["ClusterName"].ToString() + "<br/> 123456 89 111213 1516171819202122 2425 27 293031323334353637 3940 414243 45";

            //title += "<br/>" + dt.Rows[0]["ObjectiveName"].ToString();
            //title += "<br/> " + dt.Rows[0]["IndicatorName"].ToString();
            //title += "<br/> " + dt.Rows[0]["ActivityName"].ToString();
            //title += "</br/> " + dt.Rows[0]["DataName"].ToString();

            return title;
        }

        public static string DrawLocaitonChart(Series[] series, string[] category, int j, string title)
        {
            Highcharts hc = new Highcharts("Chart" + j)
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Width = 550,
                    Height = 530,
                    MarginTop = 100,
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
                .SetXAxis(new XAxis
                {
                    Categories = category,
                    Labels = new XAxisLabels
                    {
                        Rotation = -45,
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

        public enum LocationType
        {
            Country = 1,
            Admin1 = 2,
            Admin2 = 3
        }
    }
}