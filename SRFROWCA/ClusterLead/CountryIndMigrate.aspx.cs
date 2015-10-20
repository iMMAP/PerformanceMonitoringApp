using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class CountryIndMigrate : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                LoadCombos();
                DisableDropDowns();
                LoadClusterIndicators();
            }
        }

        // Disable Controls on the basis of user profile
        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void LoadClusterIndicators()
        {
            gvClusterIndicators.DataSource = SetDataSource();
            gvClusterIndicators.DataBind();
        }

        private DataTable SetDataSource()
        {
            string indicator = null;
            int? countryId = null;
            int? clusterId = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            bool regionalIncluded = false;
            if (cbIncludeRegional.Visible)
            {
                regionalIncluded = cbIncludeRegional.Checked;
            }

            return DBContext.GetData("GetClusterIndicators", new object[] { clusterId, countryId, indicator,
                                                                               RC.SelectedSiteLanguageId, regionalIncluded, RC.EmergencySahel2015 });
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsAdmin(this.User))
            {
                if (ddlCluster.Items.Count > 0)
                {
                    ddlCluster.SelectedValue = "-1";
                }
                if (ddlCountry.Items.Count > 0)
                {
                    ddlCountry.SelectedValue = "-1";
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                if (ddlCluster.Items.Count > 0)
                {
                    ddlCluster.SelectedValue = "-1";
                }
            }

            txtIndicatorName.Text = "";
            cbIncludeRegional.Checked = true;
            LoadClusterIndicators();
        }

        protected void cbIncudeRegional_CheckedChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        
        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = SetDataSource();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterIndicators.DataSource = dt;
                gvClusterIndicators.DataBind();
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

        protected void gvClusterindicators_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClusterIndicators.PageIndex = e.NewPageIndex;
            LoadClusterIndicators();
        }

        protected void gvClusterIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 1);
                ObjPrToolTip.CountryIndicatorIcon(e, 2);

                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                Label lblCountryId = e.Row.FindControl("lblCountryID") as Label;
                Label lblClusterId = e.Row.FindControl("lblClusterID") as Label;

                int emgLocationId = 0;

                if (lblCountryId != null)
                {
                    if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User))
                    {
                        emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                    }
                    else
                    {
                        int.TryParse(lblCountryId.Text, out emgLocationId);
                    }
                }

                int emgClusterId = 0;
                if (lblClusterId != null)
                {
                    int.TryParse(lblClusterId.Text, out emgClusterId);
                }

                FrameWorkSettingsCount frCount = FrameWorkUtil.GetOutputFrameworkSettings(emgLocationId, emgClusterId);

                //GetIndCountAndEditDate(emgLocationId, emgClusterId, out maxIndicators, out endEditDate);

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Setting?')");

                    if (frCount.DateExcedded)
                    {
                        if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }

                if (btnEdit != null && frCount.DateExcedded)
                {
                    if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                    {
                        btnEdit.Visible = false;
                    }
                }

                string isRegional = e.Row.Cells[1].Text;
                if ((RC.IsCountryAdmin(this.User) || RC.IsClusterLead(this.User)) && isRegional == "True")
                {
                    btnDelete.Visible = false;
                }

                string isCountry = e.Row.Cells[2].Text;
                if (RC.IsRegionalClusterLead(this.User) && isCountry == "True")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterIndicators();
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvClusterIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    //int activityId = Convert.ToInt32(gvActivity.DataKeys[row.RowIndex].Values["ActivityId"].ToString());
                    //int indicatorId = Convert.ToInt32(gvActivity.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    //CheckBox cb = gvActivity.Rows[row.RowIndex].FindControl("cbIsSelected") as CheckBox;
                    //if (cb != null)
                    //{
                    //    if (cb.Checked && cb.Enabled)
                    //    {
                    //        DBContext.Add("Insert2016Framework", new object[] { activityId, indicatorId, 12, RC.GetCurrentUserId, DBNull.Value });
                    //    }
                    //}
                }
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}