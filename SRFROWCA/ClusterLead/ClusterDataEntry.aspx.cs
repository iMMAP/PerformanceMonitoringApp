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
    public partial class ClusterDataEntry : BasePage
    {
        public string CountryDisplayNone = string.Empty;
        public string ClusterDisplayNone = string.Empty;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);

            if (control.Equals("btnSaveAll"))
                ShowMessage("Achieved data saved successfully!");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                ShowHideControls();

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
                CountryDisplayNone = "display:none";
            }
            else if (RC.IsClusterLead(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible =
                        ddlCluster.Visible =
                            lblCluster.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCluster.SelectedValue = Convert.ToString(UserInfo.EmergencyCluster);

                ClusterDisplayNone = "display:none";
            } 

        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterIndicators();
        }

        private void LoadCombos()
        {
            //UI.FillCountry(ddlCountry);
            UI.FillEmergencyLocations(ddlCountry, UserInfo.Emergency, RC.SelectedSiteLanguageId);
            //UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedSiteLanguageId);

            PopulateYears();
            PopulateMonths();

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));
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
            string achieved = null;
            int countryId = 0;
            int clusterId = 0;
            int target = 0;
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            int monthId = Convert.ToInt32(ddlMonth.SelectedValue);

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //target = Convert.ToInt32(row.Cells[4].Text);

                    TextBox txtAchieved = (TextBox)row.FindControl("txtAchieved");
                    Label lblTarget = (Label)row.FindControl("lblTarget");
                    Label lblCountryID = (Label)row.FindControl("lblCountryID");
                    Label lblClusterID = (Label)row.FindControl("lblClusterID");
                    Label lblClusterIndicatorID = (Label)row.FindControl("lblClusterIndicatorID");

                    if (lblTarget != null)
                        target = Convert.ToInt32(lblTarget.Text.Replace(".", "").Replace(",", "").Trim());

                    if (lblClusterIndicatorID != null)
                        clusterIndicatorID = Convert.ToInt32(lblClusterIndicatorID.Text);

                    if (txtAchieved != null)
                    {
                        string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";
                        string cultureAchieved = txtAchieved.Text.Trim();
                        
                        if(RC.SelectedSiteLanguageId.Equals(2))
                            cultureAchieved = cultureAchieved.Replace(".", ",");

                        achieved = string.IsNullOrEmpty(txtAchieved.Text) ? null : decimal.Round(Convert.ToDecimal(cultureAchieved, new CultureInfo(siteCulture)), 0).ToString();
                    }

                    //if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                    //    countryId = Convert.ToInt32(ddlCountry.SelectedValue);
                    //else 
                        if (lblCountryID != null)
                        countryId = Convert.ToInt32(lblCountryID.Text);

                    //if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                    //    clusterId = Convert.ToInt32(ddlCluster.SelectedValue);
                    //else
                        if (lblCluster != null)
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
                ObjPrToolTip.RegionalIndicatorIcon(e,9);
                ObjPrToolTip.CountryIndicatorIcon(e, 10);

                Label lblTarget = (Label)e.Row.FindControl("lblTarget");

                if (lblTarget != null)
                {
                    string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";
                    lblTarget.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lblTarget.Text));
                }
            }
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            int? countryId = null;
            int? clusterId = null;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            DataTable dt = GetClusterIndicatros(clusterId, countryId, null);

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvIndicators.DataSource = dt;
                gvIndicators.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }
    }
}