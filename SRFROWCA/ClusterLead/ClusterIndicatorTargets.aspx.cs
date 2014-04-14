using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class ClusterIndicatorTargets : BasePage
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
        }

        internal override void BindGridData()
        {
            PopulateObjectives();
            PopulatePriorities();
            PopulateIndicators();
            PopulateClusterName();
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

        private void PopulateIndicators()
        {
            gvClusterIndTargets.DataSource = GetIndicators();
            gvClusterIndTargets.DataBind();
        }

        private DataTable GetIndicators()
        {
            int lngId = RC.SelectedSiteLanguageId;
            return DBContext.GetData("GetClusterIndicatorsToForTargets", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster, lngId });
        }

        private void PopulateClusterName()
        {
            localizeClusterName.Text = RC.GetClusterName + " Indicators";
        }

        protected void gvClusterIndTargets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e);
                ObjPrToolTip.PrioritiesIconToolTip(e);                
                ObjPrToolTip.RegionalIndicatorIcon(e, 5);
                ObjPrToolTip.CountryIndicatorIcon(e, 6);
            }
        }

        protected void gvClusterIndTargets_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = new DataTable();
            //GetIndicators();
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

            gvClusterIndTargets.DataSource = dt;
            gvClusterIndTargets.DataBind();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                ShowMessage("Data Saved Successfully!");
            }
        }

        private void SaveData()
        {
            using (ORSEntities db = new ORSEntities())
            {
                var clusterIndTargetIds = db.ClusterIndicatorTargets
                                            .Where(x => x.EmergencyLocationId == UserInfo.EmergencyCountry && x.EmergencyClusterId == UserInfo.EmergencyCluster)
                                            .Select(y => y.ClusterIndicatorTargetId).ToList();

                foreach (GridViewRow row in gvClusterIndTargets.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        int clusterIndTargetId = 0;
                        int.TryParse(gvClusterIndTargets.DataKeys[row.RowIndex].Values["ClusterIndicatorTargetId"].ToString(), out clusterIndTargetId);

                        int indicatorId = 0;
                        int.TryParse(gvClusterIndTargets.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString(), out indicatorId);

                        if (indicatorId > 0)
                        {
                            int midYearTargetTemp = 0;
                            TextBox txtMidYearTarget = row.FindControl("txtMidYearTarget") as TextBox;
                            if (txtMidYearTarget != null)
                            {
                                int.TryParse(txtMidYearTarget.Text.Trim(), out midYearTargetTemp);
                            }

                            int fullYearTargetTemp = 0;
                            TextBox txtFullYearTarget = row.FindControl("txtFullYearTarget") as TextBox;
                            if (txtFullYearTarget != null)
                            {
                                int.TryParse(txtFullYearTarget.Text.Trim(), out fullYearTargetTemp);
                            }

                            int? midYearTarget = midYearTargetTemp > 0 ? midYearTargetTemp : (int?)null;
                            int? fullYearTarget = fullYearTargetTemp > 0 ? fullYearTargetTemp : (int?)null;
                            if (midYearTarget != null || fullYearTarget != null)
                            {
                                SaveTargets(clusterIndTargetId, indicatorId, midYearTarget, fullYearTarget);
                                clusterIndTargetIds.Remove(clusterIndTargetId);
                            }
                        }
                    }
                }

                DeleteAllRemainingTarget(clusterIndTargetIds);
            }
        }

        private void DeleteAllRemainingTarget(List<int> clusterIndTargetIds)
        {
            string itemIds = "";

            foreach (int i in clusterIndTargetIds)
            {

                if (itemIds != "")
                {
                    itemIds += "," + i.ToString();
                }
                else
                {
                    itemIds += i.ToString();
                }
            }

            if (!string.IsNullOrEmpty(itemIds))
            {
                DBContext.Delete("DeleteClustersNotBeingUsed", new object[] { itemIds, DBNull.Value });
            }
        }

        private void SaveTargets(int clusterIndTargetId, int indicatorId, int? midYearTarget, int? fullYearTarget)
        {
            int yearId = 10; // TODO: Get year from datetime or from frontend
            DBContext.Add("InsertTargetInClustrIndicatorTargets", new object[] {clusterIndTargetId, UserInfo.EmergencyCountry, UserInfo.EmergencyCluster,
                                                                                    indicatorId, yearId, midYearTarget, fullYearTarget, RC.GetCurrentUserId, DBNull.Value });
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetIndicators();
            RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, UserInfo.CountryName + "_" + RC.GetClusterName + "_Indicator_Target", ".xls", Response, true);
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

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }
    }
}