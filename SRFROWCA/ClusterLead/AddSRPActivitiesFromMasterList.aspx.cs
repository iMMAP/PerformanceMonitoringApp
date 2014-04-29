using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.ClusterLead
{
    public partial class AddSRPActivitiesFromMasterList : BasePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
            ObjPrToolTip.PrioritiesToolTip(cblPriorities);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjectives();
            PopulatePriorities();
            PopulateIndicators();
            PopulateClusterName();

            Session["SRPCustomEditIndicator"] = null;
        }

        internal override void BindGridData()
        {
            PopulateObjectives();
            PopulatePriorities();
            PopulateIndicators();
            PopulateClusterName();
        }

        private void PopulateIndicators()
        {
            gvSRPIndicators.DataSource = GetIndicators();
            gvSRPIndicators.DataBind();
        }

        private DataTable GetIndicators()
        {
            int emergencyId = 1;
            int clusterId = UserInfo.Cluster;
            int emgLocationId = UserInfo.EmergencyCountry;
            int lngId = RC.SelectedSiteLanguageId;
            return DBContext.GetData("GetClusterIndicatorsToSelectSRPActivities", new object[] { emergencyId, clusterId, emgLocationId, lngId });
        }

        private void PopulateClusterName()
        {
            localizeClusterName.Text = RC.GetClusterName + " Indicators";
        }

        protected void gvSRPIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.PrioritiesIconToolTip(e, 1);
                ObjPrToolTip.RegionalIndicatorIcon(e, 5);
            }
        }

        protected void gvSRPIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditIndicator")
            {
                Session["SRPCustomEditIndicator"] = e.CommandArgument.ToString();
                Response.Redirect("~/LeadPages/EditActIndicator.aspx");
            }
        }

        protected void chkSRP_CheckedChanged(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox cb = gvSRPIndicators.Rows[index].FindControl("chkSRP") as CheckBox;
            if (cb != null)
            {
                //Label lblUserId = gvSRPIndicators.Rows[index].FindControl("lblUserId") as Label;
                int indicatorId = 0;
                int.TryParse(gvSRPIndicators.DataKeys[index].Values["ActivityDataId"].ToString(), out indicatorId);

                if (indicatorId > 0)
                {
                    AddRemoveSRPIndicatorFromList(indicatorId, cb.Checked);
                }
            }
        }

        protected void gvSRPIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators();
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

            gvSRPIndicators.DataSource = dt;
            gvSRPIndicators.DataBind();
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

        private void AddRemoveSRPIndicatorFromList(int indicatorId, bool isAdd)
        {
            Guid userId = RC.GetCurrentUserId;
            int locationId = UserInfo.EmergencyCountry;
            int clusterId = UserInfo.EmergencyCluster;
            DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, clusterId, isAdd, userId, DBNull.Value });
        }

        protected void btnAddSRPActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddSRPActivity.aspx");
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/LeadPages/AddIndicatorOnActivity.aspx");
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(cblPriorities);
            ObjPrToolTip.PrioritiesToolTip(cblPriorities);
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetIndicators();
            RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, UserInfo.CountryName + "_" + RC.GetClusterName + "_Indicators", ".xls", Response, true);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            dt.Columns.Remove("ObjectiveId");
            dt.Columns.Remove("ObjAndPrId");
            dt.Columns.Remove("PriorityActivityId");
            dt.Columns.Remove("ActivityDataId");
            dt.Columns.Remove("HumanitarianPriorityId");
            dt.Columns.Remove("ActivityDataId1");
        }
    }
}