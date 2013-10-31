using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using SRFROWCA.Controls;
using System.Web.Services;
using System.IO;
using System.Diagnostics;

namespace SRFROWCA.Reports
{
    public partial class ChartReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateDataDropDown();
            PopulateCountry();
            PopulateLocationDropDowns();

            LastLocationType = ReportsCommon.LocationType.Country;
            DrawChart(LastLocationType);
        }

        private void PopulateDataDropDown()
        {
            ddlData.DataValueField = "ActivityDataId";
            ddlData.DataTextField = "DataName";

            ddlData.DataSource = DBContext.GetData("GetDataWithActivityIndicator");
            ddlData.DataBind();
        }

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LastLocationType);            
        }

        private void PopulateLocationDropDowns()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            if (countryId > 0)
            {
                PopulateAdmin1(countryId);
                PopulateAdmin2(countryId);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLocationDropDowns();
            LastLocationType = ReportsCommon.LocationType.Country;
            DrawChart(LastLocationType);
        }

        private void DrawChart(ReportsCommon.LocationType locationType)
        {
            List<string> items = new List<string>();
            List<string> itemText = new List<string>();
            string c1 = null;
            int i = 1;
            foreach (ListItem item in (ddlData as ListControl).Items)
            {
                if (item.Selected)
                {
                    string[] logFrame = item.Text.Split(new string[] { " -- > ", "   ->   ", "   -->   " }, StringSplitOptions.RemoveEmptyEntries);
                    DataTable dt = GetLocationData(locationType, item.Value);
                    UpdateNullColumns(dt);
                    string title = string.Format("Cluster: {0}<br/>Indicator: <b>{1}</b><br/>Activity: <b>{2}</b><br/>Data: {3}",
                                                 logFrame[0], logFrame[1], logFrame[2], logFrame[3]);
                    c1 += " " + PrepareTargetAchievedChartData(dt, i, title);
                    ++i;
                    c1 += " " + PreparePercentageChartData(dt, i);
                    ++i;
                }
            }


            foreach (string id in items)
            {

                //DataTable dt = GetLocationData(locationType, id);
                //UpdateNullColumns(dt);
                //c1 += " " + PrepareTargetAchievedChartData(dt, i);
                //++i;
                //c1 += " " + PreparePercentageChartData(dt, i);
                //++i;
            }

            ltrChart.Text = c1;
        }

        private DataTable GetLocationData(ReportsCommon.LocationType locationType, string dataId)
        {
            object[] param = GetParamValues((int)locationType, dataId);
            return DBContext.GetData("GetTargetAchievedByLocation", param);
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin1;
            DrawChart(LastLocationType);
        }

        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = ReportsCommon.LocationType.Admin2;
            DrawChart(LastLocationType);
        }

        private void PopulateCountry()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetCountries");
            ddlCountry.DataBind();
        }

        private void PopulateAdmin1(int countryId)
        {
            ddlAdmin1.DataValueField = "LocationId";
            ddlAdmin1.DataTextField = "LocationName";

            ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin1.DataBind();
        }

        // Populate Locations drop down
        private void PopulateAdmin2(int countryId)
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
            ddlLocations.DataBind();
        }

        public string ChartType { get; set; }

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterCriteria ctrlFilter = this.Parent.FindControl("FilterCriteria1") as FilterCriteria;
            if (ctrlFilter != null)
            {
                ctrlFilter.LoadData();
            }
        }

        private void UpdateNullColumns(DataTable dt)
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

        //private Series[] GetSeries(DataTable dt)
        //{
        //    Series[] series = new Series[dt.Columns.Count - 3];
        //    int i = 0;

        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        if (!((column.Caption == "Location") || (column.Caption == "LocationId") || (column.Caption == "WorkDone")))
        //        {
        //            object[] locationSeries = (from DataRow row in dt.Rows
        //                                       select row[column.Caption]).ToArray();
        //            //string columnName = column.Caption.Remove(column.Caption.Length - 2, 2);
        //            series[i] = new Series
        //            {
        //                Name = column.Caption,
        //                Data = new Data(locationSeries.ToArray())
        //            };

        //            i++;
        //        }
        //    }

        //    return series;
        //}

        //private Series[] GetPercentageSeries(DataTable dt)
        //{
        //    Series[] series = new Series[1];

        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        if (column.Caption == "WorkDone")
        //        {
        //            object[] locationSeries = (from DataRow row in dt.Rows
        //                                       select row[column.Caption]).ToArray();
        //            series[0] = new Series
        //            {
        //                Name = "Percentage",
        //                Data = new Data(locationSeries.ToArray()),
        //                PlotOptionsColumn = new PlotOptionsColumn
        //                {
        //                    PointWidth = 20
        //                },
        //            };
        //        }
        //    }

        //    return series;
        //}

        private string[] GetCategories(DataTable dt)
        {
            object[] categories = (from DataRow row in dt.Rows
                                   select row["Location"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        private string DrawLocaitonChart(Series[] series, string[] category, int i, string title)
        {
            Highcharts hc = new Highcharts("Chart" + i.ToString())
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Height = 500,
                    Width = 1000

                })
                .SetExporting(new Exporting
                {
                    Scale = 2,
                    Width = 1500
                })

                .SetTitle(new Title
                {
                    Text = title,
                    Margin = 70


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
            //ltrChart.Text = hc.ToHtmlString();
        }


        private string DrawPercentageChart(Series[] series, string[] category, int i)
        {
            Highcharts hc = new Highcharts("Chart" + i.ToString())
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title
                {
                    Text = "Percentage Achieved Of Target On Data By Location"
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
                            Y = 10,
                            Formatter = "function() { return this.y; }",
                            Style = "font: 'normal 13px Verdana, sans-serif'"
                        },
                    }
                })
                .SetSeries(series);

            return hc.ToHtmlString();
            //ltrChartPercentage.Text = hc.ToHtmlString();
        }

        public string PrepareTargetAchievedChartData(DataTable dt, int i, string title)
        {
            string c = null;
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetSeries(dt);
                DataRow dr = dt.Rows[0];
                string[] categories = GetCategories(dt);
                c = DrawLocaitonChart(series, categories, i, title);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                c = DrawLocaitonChart(series, categories, i, title);
            }

            return c;
        }

        public string PreparePercentageChartData(DataTable dt, int i)
        {
            string c = null;
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetPercentageSeries(dt);
                string[] categories = GetCategories(dt);
                c = DrawPercentageChart(series, categories, i);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                c = DrawPercentageChart(series, categories, i);
            }

            return c;
        }

        private object[] GetParamValues(int locationTypeId, string dataId)
        {
            string locationIds = null;
            if (locationTypeId == 1)
            {
                locationIds = ddlCountry.SelectedValue;
            }
            else if (locationTypeId == 2)
            {
                locationIds = GetSelectedValues(ddlAdmin1);
            }
            else if (locationTypeId == 3)
            {
                locationIds = GetSelectedValues(ddlLocations);
            }

            //int? dataId = GetSelectedValues(ddlData);
            return new object[] { locationIds, locationTypeId, Convert.ToInt32(dataId) };
        }

        // Get multiple selected values from drop down checkbox.
        private string GetSelectedValues(object sender)
        {
            string ids = GetSelectedItems(sender);
            ids = !string.IsNullOrEmpty(ids) ? ids : null;
            return ids;
        }

        private int? GetSelectedValue(DropDownList ddl)
        {
            int val = 0;
            int.TryParse(ddl.SelectedValue, out val);
            return val > 0 ? val : (int?)null;
        }

        private string GetSelectedItems(object sender)
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

        private ReportsCommon.LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return (ReportsCommon.LocationType)ViewState["LastLocationType"];
                }
                else
                {
                    return ReportsCommon.LocationType.Country;
                }
            }
            set
            {
                ViewState["LastLocationType"] = value;
            }
        }
    }
}