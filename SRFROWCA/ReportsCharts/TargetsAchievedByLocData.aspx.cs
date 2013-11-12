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

namespace SRFROWCA.Reports
{
    public partial class TargetsAchievedByLocData : System.Web.UI.Page
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
            DataTable dt = GetLocationData(locationType);
            UpdateNullColumns(dt);
            PrepareTargetAchievedChartData(dt);
            PreparePercentageChartData(dt);
        }

        private DataTable GetLocationData(ReportsCommon.LocationType locationType)
        {
            object[] param = GetParamValues((int)locationType);
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

        private string[] GetCategories(DataTable dt)
        {
            object[] categories = (from DataRow row in dt.Rows
                                   select row["Location"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        private void DrawLocaitonChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Width = 550,
                    Height = 330,
                    //MarginTop = 10,
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

            ltrChart.Text = hc.ToHtmlString();

        }

        private void DrawPercentageChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart1")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Column,
                    Width = 550,
                    Height = 330,
                    //MarginTop = 10
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
            ltrChartPercentage.Text = hc.ToHtmlString();
        }

        public void PrepareTargetAchievedChartData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetSeries(dt);
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
                Series[] series = ReportsCommon.GetPercentageSeries(dt);
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

            int? dataId = GetSelectedValue(ddlData);
            hfChart.Value = dataId.ToString();
            return new object[] { locationIds, locationTypeId, dataId };
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