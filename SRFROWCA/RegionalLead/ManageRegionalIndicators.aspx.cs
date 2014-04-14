using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.RegionalLead
{
    public partial class ManageRegionalIndicators : BasePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
            ObjPrToolTip.PrioritiesToolTip(cblPriorities);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
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
            gvIndicators.DataSource = GetIndicators();
            gvIndicators.DataBind();
        }

        private DataTable GetIndicators()
        {
            int emergencyId = 1;
            int clusterId = UserInfo.Cluster;
            return DBContext.GetData("GetClusterIndicatorsToSelectRegionalIndicators", new object[] { emergencyId, clusterId, RC.SelectedSiteLanguageId });
        }

        private void PopulateClusterName()
        {
            localizeClusterName.Text = RC.GetClusterName + " Indicators";
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e);
                ObjPrToolTip.PrioritiesIconToolTip(e);
            }
        }

        protected void chkRegional_CheckedChanged(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox cb = gvIndicators.Rows[index].FindControl("chkRegional") as CheckBox;
            if (cb != null)
            {
                //Label lblUserId = gvSRPIndicators.Rows[index].FindControl("lblUserId") as Label;
                int indicatorId = 0;
                int.TryParse(gvIndicators.DataKeys[index].Values["ActivityDataId"].ToString(), out indicatorId);

                if (indicatorId > 0)
                {
                    AddRemoveRegionalIndicatorFromList(indicatorId, cb.Checked);
                }
            }
        }

        protected void gvSRPIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators();
            
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

            gvIndicators.DataSource = dt;
            gvIndicators.DataBind();
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

        private void AddRemoveRegionalIndicatorFromList(int indicatorId, bool isAdd)
        {
            Guid userId = RC.GetCurrentUserId;
            DBContext.Update("InsertDeleteRegionalIndicaotr", new object[] { indicatorId, isAdd, userId, DBNull.Value });
        }
    }
}