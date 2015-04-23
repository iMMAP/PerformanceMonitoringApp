using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Transactions;

namespace SRFROWCA.Pages
{
    public partial class ManageActivities : BasePage
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjects();
                PopulateObjectives();

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

        internal override void BindGridData()
        {
            PopulateObjectives();

            if (rblProjects.Items.Count > 0)
            {
                rblProjects.SelectedValue = SelectedProjectId;
                PopulateLogFrame();
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

        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true, RC.SelectedEmergencyId);
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        private void PopulateProjects()
        {
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectCode";

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
                    if (item.Text == row["ProjectCode"].ToString())
                    {
                        item.Attributes["title"] = row["ProjectTitle"].ToString();
                    }
                }
            }
        }

        private DataTable GetUserProjects()
        {
            DataTable dt = RC.GetOrgProjectsOnLocation(null);
            Session["testprojectdata"] = dt;
            return dt;
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
                dtIndicators = DBContext.GetData("GetProjectIndicators2015", new object[] { projectClusterId, UserInfo.EmergencyCountry, 
                                                                                                projectId, RC.SelectedSiteLanguageId,
                                                                                                RC.SelectedEmergencyId});
            }

            gvIndicators.DataSource = dtIndicators;
            gvIndicators.DataBind();
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
                //ObjPrToolTip.PrioritiesIconToolTip(e, 1);

                //ObjPrToolTip.RegionalIndicatorIcon(e, 4);
                //ObjPrToolTip.CountryIndicatorIcon(e, 5);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                ShowMessage("Data Saved Successfully!");
                //RC.SendEmail(UserInfo.EmergencyCountry, (int?)null, "Sahel ORS: Indicators add/removed from project" + rblProjects.SelectedItem.Text, "Indicators Add/removed from project" + rblProjects.SelectedItem.Text);
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
                    int.TryParse(gvIndicators.DataKeys[row.RowIndex].Values["IndicatorId"].ToString(), out indicatorId);
                    CheckBox isAdded = gvIndicators.Rows[row.RowIndex].FindControl("cbIsAdded") as CheckBox;

                    if (indicatorId > 0 && isAdded != null)
                    {
                        bool isOPS = projectId > 500000 ? false : true;

                        DBContext.Add("InsertProjectIndicator2015", new object[] { projectId, indicatorId, UserInfo.EmergencyCountry, RC.YearsInDB.Year2015,
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