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
    public partial class TargetsAchievedByLocDuration : System.Web.UI.Page
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

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LastLocationType);
        }

        protected void ddlDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LastLocationType);
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateLocationDropDowns();
            LastLocationType = ReportsCommon.LocationType.Country;
            DrawChart(LastLocationType);
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

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawChart(LastLocationType);
        }

        private void PopulateDataDropDown()
        {
            ddlData.DataValueField = "ActivityDataId";
            ddlData.DataTextField = "DataName";

            ddlData.DataSource = DBContext.GetData("GetDataWithActivityIndicator");
            ddlData.DataBind();
        }

        private void PopulateCountry()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetCountries");
            ddlCountry.DataBind();
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

        private void PopulateAdmin1(int countryId)
        {
            ddlAdmin1.DataValueField = "LocationId";
            ddlAdmin1.DataTextField = "LocationName";

            ddlAdmin1.DataSource = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            ddlAdmin1.DataBind();

            ListItem item = new ListItem();
            item.Text = "Select Admin1 Location";
            item.Value = "0";
            ddlAdmin1.Items.Insert(0, item);
        }

        // Populate Locations drop down
        private void PopulateAdmin2(int countryId)
        {
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";

            ddlLocations.DataSource = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
            ddlLocations.DataBind();

            ListItem item = new ListItem();
            item.Text = "Select Admin2 Location";
            item.Value = "0";
            ddlLocations.Items.Insert(0, item);
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

            string procedureName = GetProcedureName();
            return DBContext.GetData(procedureName, param);
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

        private object[] GetParamValues(int locationTypeId)
        {
            string locationIds = null;
            if (locationTypeId == 1)
            {
                locationIds = ddlCountry.SelectedValue;
                LocationName = ddlCountry.SelectedItem.Text;
            }
            else if (locationTypeId == 2)
            {
                locationIds = ddlAdmin1.SelectedValue;// GetSelectedValues(ddlAdmin1);
                LocationName = ddlAdmin1.SelectedItem.Text;
            }
            else if (locationTypeId == 3)
            {
                locationIds = ddlLocations.SelectedValue; // GetSelectedValues(ddlLocations);
                LocationName = ddlLocations.SelectedItem.Text;
            }

            int? dataId = ReportsCommon.GetSelectedValue(ddlData);
            int? yearId = ReportsCommon.GetSelectedValue(ddlYear);
            return new object[] { locationIds, locationTypeId, dataId, yearId };
        }

        private string GetProcedureName()
        {
            if (ddlDuration.SelectedItem.Text == "Monthly")
            {
                return "GetTargetAchievedOfLocationByMonthly";
            }
            else if (ddlDuration.SelectedItem.Text == "Quarterly")
            {
                return "GetTargetAchievedOfLocationByQuarterly";
            }
            else if (ddlDuration.SelectedItem.Text == "Biannually")
            {
                return "";
            }
            else if (ddlDuration.SelectedItem.Text == "Yearly")
            {
                return "GetTargetAchievedOfLocationByYearly";
            }
            else
            {
                return "";
            }
        }

        private string GetCategoryName()
        {
            if (ddlDuration.SelectedItem.Text == "Monthly")
            {
                return "MonthName";
            }
            else if (ddlDuration.SelectedItem.Text == "Quarterly")
            {
                return "QName";
            }
            else if (ddlDuration.SelectedItem.Text == "Biannually")
            {
                return "";
            }
            else if (ddlDuration.SelectedItem.Text == "Yearly")
            {
                return "YearName";
            }
            else
            {
                return "";
            }
        }

        private string[] GetCategories(DataTable dt)
        {
            string categoryName = GetCategoryName();
            object[] categories = (from DataRow row in dt.Rows
                                   select row[categoryName]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        public void PrepareTargetAchievedChartData(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetSeries(dt);
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

        private void DrawLocaitonChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title
                {
                    Text = "Total Targets/Achieved Onn Data For " + LocationName
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

        private void DrawPercentageChart(Series[] series, string[] category)
        {
            Highcharts hc = new Highcharts("Chart1")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title
                {
                    Text = "Percentage Of Achieved Targets Of " + LocationName + " For Data"
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

        private ReportsCommon.LocationType LastLocationType
        {
            get
            {
                if (ViewState["LastLocationType"] != null)
                {
                    return (ReportsCommon.LocationType) ViewState["LastLocationType"];
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

        private string LocationName
        {
            get
            {
                if (ViewState["LocationName"] != null)
                {
                    return ViewState["LocationName"].ToString();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                ViewState["LocationName"] = value;
            }
        }
    }
}