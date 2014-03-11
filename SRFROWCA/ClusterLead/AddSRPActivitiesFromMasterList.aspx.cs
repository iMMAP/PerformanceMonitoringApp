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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjectives();
            PopulatePririoties();
            PopulateIndicators();
        }

        private void PopulateIndicators()
        {
            gvSRPIndicators.DataSource = GetIndicators();
            gvSRPIndicators.DataBind();
        }

        private DataTable GetIndicators()
        {
            int emergencyId = 1;
            int clusterId = UserInfo.GetCluster;
            int lngId = 1;
            return DBContext.GetData("GetClusterIndicatorsToSelectSRPActivities", new object[] { emergencyId, clusterId, lngId });
        }

        protected void gvSRPIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgObj = e.Row.FindControl("imgObjective") as Image;
                if (imgObj != null)
                {
                    string txt = e.Row.Cells[0].Text;
                    if (txt.Contains("1"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so1.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 1: Track and analyse risk and vulnerability, integrating findings into humanitarian and evelopment programming.";
                    }
                    else if (txt.Contains("2"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so2.png";
                        imgObj.ToolTip = "STRATEGIC OBJECTIVE 2: Support vulnerable populations to better cope with shocks by responding earlier to warning signals, by reducing post-crisis recovery times and by building capacity of national actors.";
                    }
                    else if (txt.Contains("3"))
                    {
                        imgObj.ImageUrl = "~/images/icon/so3.png";
                        imgObj.ToolTip = " STRATEGIC OBJECTIVE 3: Deliver coordinated and integrated life-saving assistance to people affected by emergencies.";
                    }
                }
                Image imghp = e.Row.FindControl("imgPriority") as Image;
                if (imghp != null)
                {
                    string txtHP = e.Row.Cells[1].Text;
                    if (txtHP == "1")
                    {
                        imghp.ImageUrl = "~/images/icon/hp1.png";
                        imghp.ToolTip = "Addressing the humanitarian impact Natural disasters (floods, etc.)";
                    }
                    else if (txtHP == "2")
                    {
                        imghp.ImageUrl = "~/images/icon/hp2.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Conflict (IDPs, refugees, protection, etc.)";
                    }
                    else if (txtHP == "3")
                    {
                        imghp.ImageUrl = "~/images/icon/hp3.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Epidemics (cholera, malaria, etc.)";
                    }
                    else if (txtHP == "4")
                    {
                        imghp.ImageUrl = "~/images/icon/hp4.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Food insecurity";
                    }
                    else if (txtHP == "5")
                    {
                        imghp.ImageUrl = "~/images/icon/hp5.png";
                        imghp.ToolTip = "Addressing the humanitarian impact of Malnutrition";
                    }

                }
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

        protected void chkRegional_CheckedChanged(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox cb = gvSRPIndicators.Rows[index].FindControl("chkRegional") as CheckBox;
            if (cb != null)
            {
                //Label lblUserId = gvSRPIndicators.Rows[index].FindControl("lblUserId") as Label;
                int indicatorId = 0;
                int.TryParse(gvSRPIndicators.DataKeys[index].Values["ActivityDataId"].ToString(), out indicatorId);

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
            int locationId = UserInfo.GetCountry;
            DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, isAdd, userId, DBNull.Value });
        }

        private void AddRemoveRegionalIndicatorFromList(int indicatorId, bool isAdd)
        {
            Guid userId = RC.GetCurrentUserId;
            int locationId = UserInfo.GetCountry;
            DBContext.Update("InsertDeleteRegionalIndicaotr", new object[] { indicatorId, locationId, isAdd, userId, DBNull.Value });
        }

        protected void btnAddSRPActivity_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddSRPActivity.aspx");
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true);
        }

        private void PopulatePririoties()
        {
            UI.FillPriorities(cblPriorities);
        }

        

    }
}