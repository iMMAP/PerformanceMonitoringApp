using System;
using System.Collections;
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

namespace SRFROWCA.Pages
{
    public partial class TargetsAchievedOfLocationByDuration : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateCountry();
            PopulateLocationDropDowns();
            DrawChart(LocationType.Country);
        }

        protected void ddlDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LocationType.Admin1);
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
            DrawChart(LocationType.Country);
        }

        private void DrawChart(LocationType locationType)
        {
            DataTable dt = GetLocationData(locationType);
            UpdateNullColumns(dt);
            PrepareTargetAchievedChartData(dt);
            PreparePercentageChartData(dt);
        }

        private DataTable GetLocationData(LocationType locationType)
        {
            object[] param = GetParamValues((int)locationType);
            return DBContext.GetData("GetTargetAchievedOfLocationByMonthly", param);
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastLocationType = LocationType.Admin2;
            DrawChart(LocationType.Admin1);
        }

        protected void ddlLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LocationType.Admin2);
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

        private Series[] GetSeries(DataTable dt)
        {
            Series[] series = new Series[dt.Columns.Count - 5];
            int i = 0;

            foreach (DataColumn column in dt.Columns)
            {
                if (!((column.Caption == "Location") || (column.Caption == "LocationId") || (column.Caption == "WorkDone")
                    || (column.Caption == "MonthId")|| (column.Caption == "MonthName")))
                {
                    object[] locationSeries = (from DataRow row in dt.Rows
                                               select row[column.Caption]).ToArray();
                    string columnName = column.Caption.Remove(column.Caption.Length - 2, 2);
                    series[i] = new Series
                    {
                        Name = columnName,
                        Data = new Data(locationSeries.ToArray())
                    };

                    i++;
                }
            }

            return series;
        }

        private Series[] GetPercentageSeries(DataTable dt)
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

        private string[] GetCategories(DataTable dt)
        {
            object[] categories = (from DataRow row in dt.Rows
                                   select row["MonthName"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        private void DrawLocaitonChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title
                {
                    Text = "Total Target/Achieved on Data"
                })

                .SetXAxis(new XAxis
                {
                    Categories = category,
                    //Labels = new XAxisLabels
                    //{
                    //    Rotation = -45,
                    //    Align = HorizontalAligns.Right
                    //}
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

            ltrChart.Text = hc.ToHtmlString();
        }

        private void DrawPercentageChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart1")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Areaspline })
                .SetTitle(new Title
                {
                    Text = "Total Target/Achieved on Data"
                })

                .SetXAxis(new XAxis
                {
                    Categories = category,
                    //Labels = new XAxisLabels
                    //{
                    //    Rotation = -45,
                    //    Align = HorizontalAligns.Right
                    //}
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
                            X = 4,
                            Y = 10,
                            Formatter = "function() { return this.y; }",
                            Style = "font: 'normal 13px Verdana, sans-serif'"
                        },
                    }
                })
                .SetSeries(series);
            ltrChartPercentage.Text = hc.ToHtmlString();
        }

        public void PrepareTargetAchievedChartData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = GetSeries(dt);
                DataRow dr = dt.Rows[0];
                string[] categories = GetCategories(dt);
                DrawLocaitonChart(series, categories);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                DrawLocaitonChart(series, categories);
            }
        }

        public void PreparePercentageChartData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = GetPercentageSeries(dt);
                string[] categories = GetCategories(dt);
                DrawPercentageChart(series, categories);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                DrawPercentageChart(series, categories);
            }
        }

        private object[] GetParamValues(int locationTypeId)
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

            return new object[] { locationIds, locationTypeId };
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

        private LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return LastLocationType;
                }
                else
                {
                    return LocationType.Country;
                }
            }
            set
            {
                ViewState["LastLocationType"] = value;
            }
        }

        enum LocationType
        {
            Country = 1,
            Admin1 = 2,
            Admin2 = 3
        }
    }
}