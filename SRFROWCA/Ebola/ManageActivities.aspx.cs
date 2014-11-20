using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Linq;
using BusinessLogic;
using SRFROWCA.Common;
using System.Transactions;
using System.Web.UI;


namespace SRFROWCA.Ebola
{
    public partial class ManageActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjects();
                PopulateObjectives();
                PopulatePriorities();

                if (rblProjects.Items.Count > 0)
                {
                    if (Request.QueryString["pid"] != null)
                    {
                        SelectedProjectId = Request.QueryString["pid"];
                    }

                    if (ProjectIdExists(SelectedProjectId))
                    {
                        rblProjects.SelectedValue = SelectedProjectId;
                    }
                    else
                    {
                        rblProjects.SelectedIndex = 0;
                    }

                    PopulateLogFrame();
                    SelectedProjectId = rblProjects.SelectedValue;
                }
            }
        }

        private bool ProjectIdExists(string SelectedProjectId)
        {
            foreach (ListItem item in rblProjects.Items)
            {
                if (item.Value == SelectedProjectId)
                {
                    return true;
                }
            }

            return false;
        }

        internal override void BindGridData()
        {
            PopulateObjectives();
            PopulatePriorities();

            if (rblProjects.Items.Count > 0)
            {
                rblProjects.SelectedValue = SelectedProjectId;
                PopulateLogFrame();
            }
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

        private void PopulateProjects()
        {
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectShortTitle";

            DataTable dt = GetUserProjects();
            rblProjects.DataSource = dt;
            rblProjects.DataBind();

            ProjectsToolTip(rblProjects, dt);

        }

        private void ProjectsToolTip(ListControl ctl, DataTable dt)
        {
            foreach (ListItem item in ctl.Items)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (item.Text == row["ProjectShortTitle"].ToString())
                    {
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                    }
                }
            }
        }

        private DataTable GetUserProjects()
        {
            bool? isOPSProject = null;
            DataTable dt = DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject });
            Session["testprojectdata"] = dt;
            return dt;
        }

        private void SaveOPSIndicator()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            int? orgId = null;
            Guid userId = RC.GetCurrentUserId;
            DBContext.Delete("DeleteIndicatorFromProject", new object[] { projectId, 0, DBNull.Value, DBNull.Value });
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());

                    CheckBox cbIsSRP = gvIndicators.Rows[row.RowIndex].FindControl("cbIsSRP") as CheckBox;
                    if (cbIsSRP == null) return;
                    if (!cbIsSRP.Checked)
                    {
                        CheckBox cbIsAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                        if (cbIsAdded == null) return;

                        int isOPS = 1;
                        int isActive = Convert.ToInt32(cbIsAdded.Checked);

                        DBContext.Update("UpdateOPSProjectIndicatorStatus", new object[] { projectId, indicatorId, isOPS, isActive, orgId, userId, DBNull.Value });
                    }
                    else
                    {
                        CheckBox cbIsAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                        if (cbIsAdded == null) return;

                        if (cbIsAdded.Checked)
                        {
                            int isActive = Convert.ToInt32(cbIsAdded.Checked);
                            int projSelectedIndId = DBContext.Add("InsertProjectIndicator2",
                                                                    new object[] { projectId, indicatorId, 0, isActive, orgId, userId, DBNull.Value });
                        }
                    }
                }
            }
        }

        private string GetSelectedIndicators()
        {
            string actIds = "";
            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString());
                    CheckBox cbIsSRP = gvIndicators.Rows[row.RowIndex].FindControl("cbIsSRP") as CheckBox;
                    if (cbIsSRP != null)
                    {
                        if (cbIsSRP.Checked)
                        {
                            CheckBox cb = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;
                            if (cb != null)
                            {
                                if (cb.Checked)
                                {
                                    if (actIds != "")
                                    {
                                        actIds += "," + indicatorId.ToString();
                                    }
                                    else
                                    {
                                        actIds += indicatorId.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return actIds;
        }

        private void PopulateLogFrame()
        {
            DataTable dtIndicators = new DataTable();

            int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            int projectClusterId = 0;
            using (ORSEntities db = new ORSEntities())
            {
                projectClusterId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyClusterId).SingleOrDefault();
            }

            if (projectClusterId > 0)
            {
                dtIndicators = DBContext.GetData("GetProjectIndicators", new object[] { projectClusterId, UserInfo.EmergencyCountry, projectId, RC.SelectedSiteLanguageId });
            }

            gvIndicators.DataSource = dtIndicators;
            gvIndicators.DataBind();
        }

        private DataTable GetProjectCluster(int projectId)
        {
            return DBContext.GetData("GetProjectCluster", new object[] { projectId });
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            PopulateLogFrame();
            SelectedProjectId = rblProjects.SelectedValue;
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.PrioritiesIconToolTip(e, 1);

                ObjPrToolTip.RegionalIndicatorIcon(e, 4);
                ObjPrToolTip.CountryIndicatorIcon(e, 5);
            }
        }

        private string GetSelectedItems(object sender)
        {
            string itemIds = "";
            foreach (ListItem item in (sender as ListControl).Items)
            {
                if (item.Selected)
                {
                    if (itemIds != "")
                    {
                        itemIds += "," + item.Value;
                    }
                    else
                    {
                        itemIds += item.Value;
                    }
                }
            }

            return itemIds;
        }

        protected void cbIsAdded_CheckedChanged(object sender, EventArgs e)
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            if (projectId == 0) return;

            int index = ((GridViewRow)((CheckBox)sender).NamingContainer).RowIndex;
            CheckBox isAdded = gvIndicators.Rows[index].FindControl("cbIsAdded") as CheckBox;
            if (isAdded == null) return;

            int indicatorId = 0;
            int.TryParse(gvIndicators.DataKeys[index].Values["ActivityDataId"].ToString(), out indicatorId);

            if (indicatorId > 0)
            {
                CheckBox cbIsSRP = gvIndicators.Rows[index].FindControl("cbIsSRP") as CheckBox;
                if (cbIsSRP == null) return;

                Guid userId = RC.GetCurrentUserId;
                int yearId = 10;
                DBContext.Add("InsertProjectIndicator", new object[] { projectId, indicatorId, UserInfo.EmergencyCountry, yearId, isAdded.Checked, userId, DBNull.Value });
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                //scope.Complete();
                ShowMessage("Data Saved Successfully!");
            }
        }

        private void SaveData()
        {
            int projectId = RC.GetSelectedIntVal(rblProjects);
            if (projectId == 0) return;

            foreach (GridViewRow row in gvIndicators.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = 0;
                    int.TryParse(gvIndicators.DataKeys[row.RowIndex].Values["ActivityDataId"].ToString(), out indicatorId);
                    CheckBox isAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;

                    if (indicatorId > 0 && isAdded != null)
                    {
                        bool isOPS = projectId > 500000 ? false : true;
                        int yearId = 10;

                        if (isAdded.Checked)
                        {
                            int j = 0;
                        }
                        DBContext.Add("InsertProjectIndicator", new object[] { projectId, indicatorId, UserInfo.EmergencyCountry, yearId,
                                                                                isAdded.Checked, isOPS, RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        public string SelectedProjectId
        {
            get
            {

                return ViewState["ManageActivitesSelectedProjectId"] != null ?
                    ViewState["ManageActivitesSelectedProjectId"].ToString() : "0";
            }
            set
            {
                ViewState["ManageActivitesSelectedProjectId"] = value.ToString();
            }
        }
    }
}