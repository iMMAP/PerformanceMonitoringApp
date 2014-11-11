using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterDataEntry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateYears();
                PopulateMonths();
                SetDates();
                
                LoadClusterIndicators();
            }
        }

        private void SetDates()
        {
            int month = DateTime.Now.Month-1;
            int year = 11;// DateTime.Now.Year;

            if (month.Equals(12))
                month = 1;
            else if (month.Equals(11))
                month = 12;

            ddlMonth.SelectedValue = month.ToString();
            ddlYear.SelectedValue = year.ToString();
        }

        private void LoadClusterIndicators()
        {
            gvIndicators.DataSource = GetClusterIndicatros(null, null, null);
            gvIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string indicator)
        {
            int yearID = Convert.ToInt32(ddlYear.SelectedValue);
            int monthID = Convert.ToInt32(ddlMonth.SelectedValue);

            return DBContext.GetData("uspGetClusterReportDetails", new object[] { yearID, monthID, RC.SelectedSiteLanguageId });
        }

        private void PopulateYears()
        {
            ddlYear.DataValueField = "YearId";
            ddlYear.DataTextField = "Year";

            ddlYear.DataSource = GetYears();
            ddlYear.DataBind();

            var result = DateTime.Parse(DateTime.Now.ToShortDateString()).Year;
            ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(result.ToString()));
        }

        private DataTable GetYears()
        {
            DataTable dt = DBContext.GetData("GetYears");
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);

            int monthNumber = MonthNumber.GetMonthNumber(result);
            monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;
            
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void SaveClusterIndicatorDetails()
        {
            int clusterIndicatorID = 0;
            int target = 0;
            string runningSum = null;
            string achieved = null;
            int countryId = 0;
            int clusterId = 0;
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    target = Convert.ToInt32(row.Cells[3].Text);
                    
                    TextBox txtRunningSum = (TextBox)row.FindControl("txtRunningSum");
                    TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
                    Label lblCountryID = (Label)row.FindControl("lblCountryID");
                    Label lblClusterID = (Label)row.FindControl("lblClusterID");
                    Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

                    if (lblClusterIndicatorID != null)
                        clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

                    if (txtRunningSum != null)
                        runningSum = string.IsNullOrEmpty(txtRunningSum.Text)?null: Convert.ToString(txtRunningSum.Text);

                    if (txtAchieved != null)
                        achieved = string.IsNullOrEmpty(txtAchieved.Text)?null:Convert.ToString(txtAchieved.Text);

                    if (lblCountryID != null)
                        countryId = Convert.ToInt32(lblCountryID.Text);

                    if (lblClusterID != null)
                        clusterId = Convert.ToInt32(lblClusterID.Text);

                    DBContext.Add("uspInsertClusterReport", new object[] {clusterIndicatorID, clusterId, countryId, yearId, monthId, runningSum, achieved, RC.GetCurrentUserId, null });
                }
            }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveClusterIndicatorDetails();
            LoadClusterIndicators();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }
    }
}