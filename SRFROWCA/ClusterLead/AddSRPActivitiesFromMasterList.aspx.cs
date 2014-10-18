using System;
using System.Data;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

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
            
            PopulateObjectives();
            PopulatePriorities();

            if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
            {
                PopulateClusters();
                btnAddSRPActivity.Attributes.Add("disabled", "disabled");
                btnAddIndicator.Attributes.Add("disabled", "disabled");
            }
            else
            {
                PopulateIndicators();
                ddlClusters.Visible = false;
            }

            Session["SRPCustomEditIndicator"] = null;
        }

        internal override void BindGridData()
        {
            PopulateObjectives();
            PopulatePriorities();

            if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
            {
                PopulateClusters();
                PopulateIndicators();
            }
            else
            {
                PopulateIndicators();
                ddlClusters.Visible = false;
            }
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlClusters.Visible)
            {
                if (ddlClusters.SelectedValue == "0")
                {
                    btnAddSRPActivity.Attributes.Add("disabled", "disabled");
                    btnAddIndicator.Attributes.Add("disabled", "disabled");
                }
                else
                {
                    btnAddSRPActivity.Attributes.Remove("disabled");
                    btnAddIndicator.Attributes.Remove("disabled");
                }
            }

            PopulateIndicators();
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

        protected void gvSRPIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators(true);
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

            gvSRPIndicators.DataSource = dt;
            gvSRPIndicators.DataBind();
        }

        protected void btnAddSRPActivity_Click(object sender, EventArgs e)
        {
            if (ddlClusters.Visible)
            {
                int cid = 0;
                int.TryParse(ddlClusters.SelectedValue, out cid);
                Response.Redirect("AddSRPActivity.aspx?cid=" + cid.ToString());
            }
            else
            {
                Response.Redirect("AddSRPActivity.aspx");
            }
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

        private void PopulateClusters()
        {
            int emgId = 1;
            UI.FillEmergnecyClusters(ddlClusters, emgId);
            RC.AddSelectItemInList(ddlClusters, "Select Cluster");
        }

        private void PopulateIndicators()
        {
            gvSRPIndicators.DataSource = GetIndicators(true);
            gvSRPIndicators.DataBind();
        }

        private DataTable GetIndicators(bool isFilter)
        {
            int clusterId = 0;
            if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
            {
                if (isFilter && ddlClusters.Visible)
                {
                    int.TryParse(ddlClusters.SelectedValue, out clusterId);
                }
            }
            else
            {
                clusterId = UserInfo.EmergencyCluster;
            }

            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int lngId = RC.SelectedSiteLanguageId;

            DataTable dt = new DataTable();
            if (clusterId > 0)
            {
                dt = DBContext.GetData("GetClusterIndicatorsToSelectSRPActivities", new object[] { clusterId, emgLocationId, lngId });
            }

            return dt;
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

        private void SaveData()
        {
            foreach (GridViewRow row in gvSRPIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox cb = gvSRPIndicators.Rows[row.RowIndex].FindControl("chkSRP") as CheckBox;
                    if (cb != null)
                    {
                        int indicatorId = 0;
                        int.TryParse(gvSRPIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString(), out indicatorId);

                        if (indicatorId > 0)
                        {
                            if (cb.Checked)
                            {
                                int i = 0;
                            }
                            AddRemoveSRPIndicatorFromList(indicatorId, cb.Checked);
                        }
                    }
                }
            }
        }

        private void AddRemoveSRPIndicatorFromList(int indicatorId, bool isAdd)
        {
            Guid userId = RC.GetCurrentUserId;
            int locationId = UserInfo.EmergencyCountry;

            int tempVal = 0;
            if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
            {
                if (ddlClusters.Visible)
                {
                    int.TryParse(ddlClusters.SelectedValue, out tempVal);
                }
            }
            int clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster;

            DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, clusterId, isAdd, userId, DBNull.Value });
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetIndicators(false);
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

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }
    }
}