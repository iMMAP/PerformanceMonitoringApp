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
using System.Collections.Generic;

namespace SRFROWCA.Reports
{
    public partial class TargetsAchievedByLocData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //PopulateCountry();
            //PopulateDataDropDown();
            //PopulateLocationDropDowns();

            //LastLocationType = ReportsCommon.LocationType.Country;
            DrawChart(LastLocationType);
        }

        private void PopulateDataDropDown()
        {
            ddlData.DataValueField = "ActivityDataId";
            ddlData.DataTextField = "DataName";

            ddlData.DataSource = GetActiviytData();
            ddlData.DataBind();
        }

        private DataTable GetActiviytData()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            return DBContext.GetData("GetAllLogFrameDataOnLocation", new object[] { countryId });
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
            PopulateDataDropDown();
            LastLocationType = ReportsCommon.LocationType.Country;
            DrawChart(LastLocationType);
        }

        private void DrawChart(ReportsCommon.LocationType locationType)
        {
            DataTable dt = GetLocationData(locationType);
            //UpdateNullColumns(dt);

            //List<string> titles = GetChartTitles();
            //if (titles.Count > 0)
            //{
            //    divTitle.InnerHtml = titles[0];
            //    divTitle2.InnerHtml = titles[1];
            //}

            //int chartHeight = GetChartHeight(dt);

            PrepareTargetAchievedChartData(dt, 400);
            //PreparePercentageChartData(dt, chartHeight);
        }

        private int GetChartHeight(DataTable dt)
        {
            if (dt.Rows.Count <= 10)
            {
                return 400;
            }
            else if (dt.Rows.Count <= 15)
            {
                return 500;
            }
            else if (dt.Rows.Count <= 20)
            {
                return 600;
            }
            else if (dt.Rows.Count <= 25)
            {
                return 650;
            }

            return 700;
        }

        private DataTable GetLogFrameData()
        {
            int? dataId = GetSelectedValue(ddlData);
            return DBContext.GetData("GetTotalAchievedByCountry");//, new object[] { dataId });
        }

        private List<string> GetChartTitles()
        {
            DataTable dtLogFrame = GetLogFrameData();
            List<string> titles = new List<string>();
            if (dtLogFrame.Rows.Count > 0)
            {
                string countryName = ddlCountry.SelectedItem.Text;
                string clusterName = countryName + " - " + dtLogFrame.Rows[0]["ClusterName"].ToString();
                string strObjName = dtLogFrame.Rows[0]["StrategicObjectiveName"].ToString();
                string spcObjName = dtLogFrame.Rows[0]["ObjectiveName"].ToString();
                string indicatorName = dtLogFrame.Rows[0]["IndicatorName"].ToString();
                string activityName = dtLogFrame.Rows[0]["ActivityName"].ToString();
                string dataName = dtLogFrame.Rows[0]["DataName"].ToString();

                string title = string.Format("<table><tr><td colspan='2'><h1>{0}</h1></td></tr><tr><td valign='top' width='150px'>Objectif Strategique:</td><td><strong style='font-size: 16px;'>{1}</strong></td></tr><tr><td valign='top'>Objectif Specifique:</td><td><strong style='font-size: 15px;'>{2}</strong></td></tr><tr><td valign='top'>Indicateur:</td><td><strong style='font-size: 14px;'>{3}</strong></td></tr><tr><td valign='top'>Activité:</td><td><strong style='font-size: 13px;'>{4}</strong></td></tr><tr><td valign='top'>Donnée:</td><td>{5}</td></tr></table>",
                                                clusterName, strObjName, spcObjName, indicatorName, activityName, dataName);

                titles.Add(title);

                //title = string.Format("<table><tr><td colspan='2'><h3>{0}</h3></td></tr><tr><td width='150px'>Objectif Strategique:</td><td>{1}</td></tr><tr><td>Objectif Specifique:</td><td>{2}</td></tr><tr><td>Indicateur:</td><td>{3}</td></tr><tr><td>Activité:</td><td>{4}</td></tr><tr><td>Donnée:</td><td>{5}</td></tr></table>",
                //                                clusterName, strObjName, spcObjName, indicatorName, activityName, dataName);

                title = string.Format("<table><tr><td colspan='2'><h2>{0}</h2></td></tr><tr><td valign='top' width='150px'>Objectif Strategique:</td><td><strong style='font-size: 13px;'>{1}</strong></td></tr><tr><td valign='top'>Objectif Specifique:</td><td><strong style='font-size: 12px;'>{2}</strong></td></tr><tr><td valign='top'>Indicateur:</td><td><strong style='font-size: 12px;'>{3}</strong></td></tr><tr><td valign='top'>Activité:</td><td><strong style='font-size: 12px;'>{4}</strong></td></tr><tr><td valign='top'>Donnée:</td><td>{5}</td></tr></table>",
                                                clusterName, strObjName, spcObjName, indicatorName, activityName, dataName);

                titles.Add(title);
            }

            return titles;
        }

        private DataTable GetLocationData(ReportsCommon.LocationType locationType)
        {
            //object[] param = GetParamValues((int)locationType);
            return DBContext.GetData("GetTotalAchievedByCountry");//, param);
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
                                   select row["LocationName"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        private void DrawLocaitonChart(Series[] series, string[] category, int chartHeight)
        {
            Highcharts hc = new Highcharts("Chart")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Bar,
                    Height = chartHeight,
                    MarginTop = 70
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
                .SetExporting(new Exporting
                {
                    Enabled = false
                })
                .SetLegend(new Legend 
                { 
                    VerticalAlign = VerticalAligns.Top,
                    Y = 30,
                    ItemStyle = "fontSize: '9px'"
                    
                })
                .SetTitle(new Title
                {
                    Text = "Achieved Against Target",
                    //UseHTML = true,
                    //Style = "fontSize: '14px'",
                    //Align = HorizontalAligns.Left,
                    //X = 30

                })
                .SetXAxis(new XAxis
                {
                    Categories = category,
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle
                    {
                        Text = ""
                    }
                })
                .SetPlotOptions(new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        PointWidth = chartHeight == 700 ? 15 : 20,
                        Stacking = Stackings.Normal
                    }
                })
                .SetSeries(series);

            ltrChart.Text = hc.ToHtmlString();

        }

        private void DrawPercentageChart(Series[] series, string[] category, int chartHeight)
        {
            Highcharts hc = new Highcharts("Chart1")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartTypes.Bar,
                    Height = chartHeight,
                    MarginTop = 70
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
                .SetTitle(new Title
                {
                    Text = "Percentage of Target Achieved"
                })
                .SetExporting(new Exporting
                {
                    Enabled = false
                })
                .SetLegend(new Legend
                {
                    VerticalAlign = VerticalAligns.Top,
                    Y = 30,
                    ItemStyle = "fontSize: '9px'"

                })
                .SetXAxis(new XAxis
                {
                    Categories = category,
                    Title = new XAxisTitle 
                    {
                        Text = ""
                    }
                })
                .SetYAxis(new[]
                    {
                        new YAxis
                            {
                               
                                Title = new YAxisTitle
                                {
                                    Text = ""
                                }
                            }
                    })
                .SetPlotOptions(new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        DataLabels = new PlotOptionsBarDataLabels
                        {
                            Enabled = true,
                            //Rotation = -90,
                            Color = ColorTranslator.FromHtml("#FFFFFF"),
                            Align = HorizontalAligns.Right,
                            X = 0,
                            Y = 0,
                            Formatter = "function() { return this.y + '%'; }",
                            Style = "font: 'normal 8px Verdana, sans-serif'"
                        },
                        PointWidth = chartHeight > 700 ? 10 : chartHeight == 600 ? 15 : 20
                    }
                })
                .SetSeries(series);
            ltrChartPercentage.Text = hc.ToHtmlString();
        }

        public void PrepareTargetAchievedChartData(DataTable dt, int chartHeight)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetSeries(dt);
                DataRow dr = dt.Rows[0];
                string[] categories = GetCategories(dt);

                DrawLocaitonChart(series, categories, chartHeight);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                DrawLocaitonChart(series, categories, chartHeight);
            }
        }

        public void PreparePercentageChartData(DataTable dt, int chartHeight)
        {
            if (dt.Rows.Count > 0)
            {
                Series[] series = ReportsCommon.GetPercentageSeries(dt);
                string[] categories = GetCategories(dt);
                DrawPercentageChart(series, categories, chartHeight);
            }
            else
            {
                Series[] series = new[]
                {
                        new Series{ Data = new Data(new object[] {"0"})}
                };

                string[] categories = { "" };
                DrawPercentageChart(series, categories, chartHeight);
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