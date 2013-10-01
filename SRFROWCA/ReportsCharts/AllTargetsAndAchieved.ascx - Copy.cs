using System;
using System.Collections;
using System.Data;
using System.Linq;
using BusinessLogic;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using SRFROWCA.Controls;


namespace SRFROWCA.Reports
{
    public partial class AllTargetsAndAchieved : System.Web.UI.UserControl
    {
        Highcharts hc = new Highcharts("Chart");
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string ChartType { get; set; }

        protected void ddlChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChartType = ddlChartType.SelectedValue;
            FilterCriteria ctrlFilter = this.Parent.FindControl("FilterCriteria1") as FilterCriteria;
            if (ctrlFilter != null)
            {
                ctrlFilter.LoadData();
            }
        }

        private DataTable GetLocationSeries(object[] param)
        {
            return DBContext.GetData("GetTargetAndAchievedByMonthAndLocationPivot", param);
        }

        private void UpdateNullColumns(DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (!(column.Caption == "Month"))
                    {
                        if (row[column.Caption] == DBNull.Value)
                            row[column.Caption] = '0';
                    }
                }
            }
        }

        private Series[] GetSeries(DataTable dt)
        {
            Series[] series = new Series[dt.Columns.Count - 1];
            int i = 0;

            foreach (DataColumn column in dt.Columns)
            {
                if (!(column.Caption == "Month"))
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

        private string[] GetCategories(DataTable dt)
        {
            object[] categories = (from DataRow row in dt.Rows
                                       select row["Month"]).ToArray();
            return ((IEnumerable)categories).Cast<object>()
                         .Select(x => x.ToString())
                         .ToArray();
        }

        private void DrawLocaitonChart(Series[] series, string[] category)
        {
            hc.SetSeries(series);

            Chart c = new Chart();
            if (string.IsNullOrEmpty(ChartType) || ChartType == "Column")
            {
                c.Type = ChartTypes.Column;
            }
            else if(ChartType == "Bar")
            {
                c.Type = ChartTypes.Bar;
            }
            else if (ChartType == "Line")
            {
                c.Type = ChartTypes.Line;
            }
            
            hc.InitChart(c);
            XAxis xAxis = new XAxis();
            xAxis.Categories = category;
            hc.SetXAxis(xAxis);
            //PlotOptions po = new PlotOptions();
            //PlotOptionsColumn poc = new PlotOptionsColumn();
            //poc.Stacking = Stackings.Normal;
            //po.Column = poc;
            //hc.SetPlotOptions(po);
            Legend l = new Legend();
            l.Enabled = false;
            hc.SetLegend(l);
            ltrChart.Text = hc.ToHtmlString();
        }

        public void DrawChart(object[] param)
        {
            DataTable dt = GetLocationSeries(param);
            if (dt.Rows.Count > 1)
            {
                UpdateNullColumns(dt);
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
    }
}