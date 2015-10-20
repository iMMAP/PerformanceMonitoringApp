using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SRFROWCA.ClusterLead
{
    public partial class IndicatorListing2016 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                LoadClustersFilter();
                LoadCountry();
                SetDropDownOnRole(true);
                LoadIndicators();
            }
        }

        private void SetDropDownOnRole(bool bindAll)
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                    ddlCountry.Enabled = false;
                    ddlCountry.BackColor = Color.LightGray;
                }
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                if (bindAll)
                {
                    ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                    ddlCountry.Enabled = false;
                    ddlCountry.BackColor = Color.LightGray;
                }
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            if (RC.GetSelectedIntVal(ddlCountry) == 0)
            {
                ShowMessage("Please select a country to export data!", RC.NotificationType.Warning, true, 1000);
                return;
            }
            DataTable dt = GetActivitiesForExcel();
            RemoveColumnsFromDataTable(dt);
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "Indicators";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        internal override void BindGridData()
        {
            LoadClustersFilter();
            SetDropDownOnRole(false);
            LoadIndicators();
        }

        protected void gvActivity_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetActivities();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvActivity.DataSource = dt;
                gvActivity.DataBind();
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

        protected void btnBackToFramework_Click(object sender, EventArgs e)
        {
            Response.Redirect("IndicatorListing.aspx");
        }

        private void LoadIndicators()
        {
            gvActivity.DataSource = GetActivities();
            gvActivity.DataBind();
        }

        private DataTable GetActivities()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            return DBContext.GetData("GetAll2016Indicators", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                                    (int)RC.SelectedSiteLanguageId });
        }

        private void LoadCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "0"));
        }

        private void LoadClustersFilter()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "0"));
        }

        private DataTable GetClusters()
        {
            int emergencyId = RC.SelectedEmergencyId;
            if (emergencyId == 0)
            {
                emergencyId = 1;
            }

            return DBContext.GetData("GetClusters", new object[] { (int)RC.SelectedSiteLanguageId, emergencyId });
        }

        private DataTable GetActivitiesForExcel()
        {
            int? emergencyClusterId = ddlCluster.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCluster.SelectedValue);
            int? emergencyLocationId = ddlCountry.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            DataTable dt = new DataTable();
            if (emergencyLocationId > 0)
            {
                dt = DBContext.GetData("GetAllIndicatorsNew2WithTargets", new object[] { emergencyLocationId, emergencyClusterId, 
                                                                                        (int)RC.SelectedSiteLanguageId });
            }

            return dt;
        }

        protected void ddl_IndexChanged(object sender, EventArgs e)
        {
            LoadIndicators();
        }
        
        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }

        protected void gvActivity_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvActivity.PageIndex = e.NewPageIndex;
            LoadIndicators();
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ActivityId");
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("IndicatorId");
                dt.Columns.Remove("IndicatorDetailId");
                dt.Columns.Remove("ClusterName");
                dt.Columns.Remove("ShortObjective");
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("ShortObjective");

            }
            catch { }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }
    }
}