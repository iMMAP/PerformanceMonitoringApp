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
    public partial class ClusterReports : BasePage
    {
        public string CountryDisplayNone = string.Empty;
        public string ClusterDisplayNone = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                ShowHideControls();
                
                LoadClusterReports();
            }
        }

        private void LoadClusterReports()
        {
            int? countryId = null;
            int? clusterId = null;
            int? monthID = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            if (Convert.ToInt32(ddlMonth.SelectedValue) > -1)
                monthID = Convert.ToInt32(ddlMonth.SelectedValue);

            gvClusterReports.DataSource = DBContext.GetData("uspGetClusterReports", new object[] { indicator, monthID, countryId, clusterId, RC.SelectedSiteLanguageId });
            gvClusterReports.DataBind();
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterReports();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, UserInfo.Emergency, RC.SelectedSiteLanguageId);
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedSiteLanguageId);
            PopulateMonths();


            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));
            ddlMonth.Items.Insert(0, new ListItem("-- Select --", "-1"));
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

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            //var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            //result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);

            //int monthNumber = MonthNumber.GetMonthNumber(result);
            //monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;

            //ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterReports();
        }

        protected void gvClusterReports_Sorting(object sender, GridViewSortEventArgs e)
        {
            int? countryId = null;
            int? clusterId = null;
            int? monthID = null;
            string indicator = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text.Trim();

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            if (Convert.ToInt32(ddlMonth.SelectedValue) > -1)
                monthID = Convert.ToInt32(ddlMonth.SelectedValue);

            DataTable dt = DBContext.GetData("uspGetClusterReports", new object[] { indicator, monthID, countryId, clusterId, RC.SelectedSiteLanguageId });

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterReports.DataSource = dt;
                gvClusterReports.DataBind();
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
    }

}