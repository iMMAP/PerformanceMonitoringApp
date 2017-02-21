using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class CountryIndicators : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                LoadCombos();
                RC.SetFiltersFromSession(ddlCountry, ddlCluster, Session);
                DisableDropDowns(); 
                LoadClusterIndicators();
                ToggleAddIndicators();
            }
        }

        #region Events.
        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/EditOutputIndicator.aspx");
        }

        protected void cbIncudeRegional_CheckedChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void gvClusterIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 1);
                ObjPrToolTip.CountryIndicatorIcon(e, 2);
                UI.SetThousandSeparator(e.Row, "lblIndTarget");

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

                ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;

                if (!RC.IsAdmin(this.User) && (RC.GetSelectedIntVal(ddlFrameworkYear) == (int)RC.Year._2015 || RC.GetSelectedIntVal(ddlFrameworkYear) == (int)RC.Year._2016))
                {
                    if (btnDelete != null && btnEdit != null)
                    {
                        btnDelete.Visible = false;
                        btnEdit.Visible = false;
                    }
                }
                else
                {
                    if (btnDelete != null && btnEdit != null)
                    {
                        btnDelete.Attributes.Add("onclick", "javascript:return " +
                        "confirm('Are you sure you want to delete this Indicators?')");
                        //int year = 0;
                        //int.TryParse(ddlFrameworkYear.SelectedItem.Text, out year);
                        //bool IsDateExceeded = SectorFramework.DateExceeded(emgLocationId, emgClusterId, year);
                        Label lblIsDateExceeded = e.Row.FindControl("lblIsDateExceeded") as Label;
                        bool IsDateExceeded = false;
                        if (lblIsDateExceeded != null)
                            IsDateExceeded = lblIsDateExceeded.Text == "1";

                        if (!IsDateExceeded || RC.IsAdmin(this.User))
                        {
                            btnDelete.Visible = true;
                            btnEdit.Visible = true;
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
        }

        protected void gvClusterIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteIndicator")
            {
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                if (!IndicatorIsInUse(clusterIndicatorID))
                {
                    DeleteClusterIndicator(clusterIndicatorID);
                    LoadClusterIndicators();
                    ShowMessage("Indicator Deleted Successfully.");
                    ToggleAddIndicators();
                }
                else
                {
                    ShowMessage("Indicator can not be deleted. It is being used!", RC.NotificationType.Error, true, 2000);
                }
            }
            else if (e.CommandName == "EditIndicator")
            {
                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                Label lblCountryID = row.FindControl("lblCountryID") as Label;
                int cnId = 0;
                if (lblCountryID != null)
                {
                    int.TryParse(lblCountryID.Text.ToString(), out cnId);
                }

                Label lblClusterID = row.FindControl("lblClusterID") as Label;
                int cId = 0;
                if (lblClusterID != null)
                {
                    int.TryParse(lblClusterID.Text.ToString(), out cId);
                }

                int countryId = RC.GetSelectedIntVal(ddlCountry);
                cnId = countryId > 0 ? countryId : cnId;
                Response.Redirect("~/ClusterLead/EditOutputIndicator.aspx?id=" + clusterIndicatorID.ToString() + "&cid=" + cId.ToString() + "&cnid=" + cnId.ToString());
            }
        }
        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            DataTable dt = GetIndicators();
            RemoveColumnsFromDataTable(dt);
            dt.DefaultView.Sort = "Country, Cluster, Indicator, Unit";

            string fileName = "ClusterIndicators";
            ExportUtility.ExportGridView(dt, fileName, Response);
        }
        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
            //cbIncludeRegional.Visible = yearId < (int)RC.Year._2017;
            //localCBIsRegCap.Visible = yearId < (int)RC.Year._2017;

            LoadClusterIndicators();
            ToggleAddIndicators();
            RC.SaveFiltersInSession(ddlCountry, ddlCluster, Session);

        }
        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterIndicators.DataSource = dt;
                gvClusterIndicators.DataBind();
            }
        }
        protected void gvClusterindicators_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClusterIndicators.PageIndex = e.NewPageIndex;
            LoadClusterIndicators();
        }

        #endregion

        #region Methods.
        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterIndicators();
        }
        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            else
                ddlCountry.Items.Insert(0, new ListItem("Sélectionner Pays", "0"));

            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            else
                ddlCluster.Items.Insert(0, new ListItem("Sélectionner Cluster", "0"));

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
        private void LoadClusterIndicators()
        {
            gvClusterIndicators.DataSource = GetIndicators();
            gvClusterIndicators.DataBind();
        }
        private DataTable GetIndicators()
        {
            int? countryId = null;
            int? clusterId = null;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > 0)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            bool regionalIncluded = false;
            //if (cbIncludeRegional.Visible)
            //{
                regionalIncluded = cbIncludeRegional.Checked;
            //}

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("GetClusterIndicators", new object[] { clusterId, countryId, yearId, RC.SelectedSiteLanguageId,
                                                                                regionalIncluded, RC.EmergencySahel2015 });
        }
        private void ToggleAddIndicators()
        {
            // If Super Admin Enable button and return
            if (RC.IsAdmin(this.User))
            {
                btnAddIndicator.Enabled = true;
                return;
            }

            // Year is 2015 disable and return
            if (RC.GetSelectedIntVal(ddlFrameworkYear) == (int)RC.Year._2015 || RC.GetSelectedIntVal(ddlFrameworkYear) == (int)RC.Year._2016)
            {
                btnAddIndicator.Enabled = false;
                return;
            }

            int emgLocationId = RC.IsRegionalClusterLead(this.User) ? (int)RC.LocationSAH2015.SAHRegion : RC.GetSelectedIntVal(ddlCountry);
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            // if Location or Cluster is not selected disable button and return
            if (emgLocationId <= 0 || emgClusterId <= 0)
            {
                btnAddIndicator.Enabled = false;
                return;
            }

            int year = 0;
            int.TryParse(ddlFrameworkYear.SelectedItem.Text, out year);
            int indUnused = SectorFramework.OutputIndUnused(emgLocationId, emgClusterId, year);
            bool IsDateExceeded = SectorFramework.DateExceeded(emgLocationId, emgClusterId, year);
            btnAddIndicator.Enabled = !(indUnused <= 0 || IsDateExceeded);

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
        private bool IndicatorIsInUse(int clusterIndicatorID)
        {
            DataTable dt = DBContext.GetData("ClusterIndicatorInUse", new object[] { clusterIndicatorID });
            return dt.Rows.Count > 0;
        }
        private bool DeleteClusterIndicator(int indicatorID)
        {
            return Convert.ToBoolean(DBContext.Delete("uspDeleteClusterIndicator", new object[] { indicatorID, null }));
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
                if (yearId > (int)RC.Year._2016)
                {
                    dt.Columns.Remove("IsRegional");
                }
                dt.Columns.Remove("ClusterIndicatorId");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("IndicatorAlt");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("UnitId");
                dt.Columns.Remove("IsSRP");
                dt.Columns.Remove("IndicatorCalculationTypeId");

            }
            catch { }
        }

        #endregion
    }
}