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
                LoadCombos();
                ShowHideControls();

                PopulateYears();
                PopulateMonths();
                SetDates();
                
                LoadClusterIndicators();
            }
        }

        private void ShowHideControls()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);

                if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                {
                    lblCountryClusterTitle.Text = "Country:";
                    lblCountryCluster.Text = UserInfo.CountryName;
                }
            }
            else if (RC.IsClusterLead(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible =
                        ddlCluster.Visible =
                            lblCluster.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCluster.SelectedValue = Convert.ToString(UserInfo.EmergencyCluster);

                if (Convert.ToInt32(ddlCountry.SelectedValue) > 0
                    && Convert.ToInt32(ddlCluster.SelectedValue) > 0)
                {
                    lblCountryClusterTitle.Text = "Country/Cluster:";
                    lblCountryCluster.Text = UserInfo.CountryName + "-" + ddlCluster.SelectedItem.Text;
                }
            }
            
        }

        private void LoadCombos()
        {
            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
        }

        private void SetDates()
        {
            int month = DateTime.Now.Month - 1;
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
            int? countryId = null;
            int? clusterId = null;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            gvIndicators.DataSource = GetClusterIndicatros(clusterId, countryId, null);
            gvIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string indicator)
        {
            int yearID = Convert.ToInt32(ddlYear.SelectedValue);
            int monthID = Convert.ToInt32(ddlMonth.SelectedValue);
          
            return DBContext.GetData("uspGetClusterReportDetails", new object[] { yearID, monthID, RC.SelectedSiteLanguageId, countryId, clusterId });
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
            string achieved = null;
            int countryId = 0;
            int clusterId = 0;
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    target = Convert.ToInt32(row.Cells[4].Text);

                    TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
                    Label lblCountryID = (Label)row.FindControl("lblCountryID");
                    Label lblClusterID = (Label)row.FindControl("lblClusterID");
                    Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

                    if (lblClusterIndicatorID != null)
                        clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

                    if (txtAchieved != null)
                        achieved = string.IsNullOrEmpty(txtAchieved.Text) ? null : Convert.ToString(txtAchieved.Text);

                    if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                        countryId = Convert.ToInt32(ddlCountry.SelectedValue);
                    else if (lblCountryID != null)
                        countryId = Convert.ToInt32(lblCountryID.Text);

                    if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                        clusterId = Convert.ToInt32(ddlCluster.SelectedValue);
                    else if (lblCluster != null)
                        clusterId = Convert.ToInt32(lblClusterID.Text);

                    DBContext.Add("uspInsertClusterReport", new object[] { clusterIndicatorID, clusterId, countryId, yearId, monthId, achieved, RC.GetCurrentUserId, null });
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

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 8);
                ObjPrToolTip.CountryIndicatorIcon(e, 9);
            }
        }
    }
}