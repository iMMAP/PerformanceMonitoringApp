using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Transactions;
using AjaxControlToolkit;

namespace SRFROWCA.Pages
{
    public partial class ProjectActivities : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateObjectives();
                PopulateLogFrame();
            }
        }

        
        private void PopulateObjectives()
        {
            UI.FillObjectives(cblObjectives, true, RC.SelectedEmergencyId);
            ObjPrToolTip.ObjectivesToolTip(cblObjectives);
        }

        internal void PopulateLogFrame()
        {
            DataTable dtIndicators = new DataTable();
            int projectId = ((ManageProject)this.Page).ProjectId;
            int projectClusterId = 0;
            int projectEmgLocationId = 0;

            using (ORSEntities db = new ORSEntities())
            {
                projectClusterId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyClusterId).SingleOrDefault();
                projectEmgLocationId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyLocationId).SingleOrDefault();
            }

            if (projectClusterId > 0 && projectEmgLocationId > 0)
            {
                dtIndicators = DBContext.GetData("GetProjectIndicators2015", new object[] { projectClusterId, projectEmgLocationId,
                                                                                                projectId, RC.SelectedSiteLanguageId,
                                                                                                RC.SelectedEmergencyId});
            }

            gvIndicators.DataSource = dtIndicators;
            gvIndicators.DataBind();
        }

        protected void gvIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                ShowMessage("Data Saved Successfully!");                
            }

            TabPanel pnlActivities = this.Parent.Parent.NamingContainer.FindControl("tpnlPartners") as TabPanel;
            ProjectPartners ctlProjectPartners = pnlActivities.FindControl("ctlProjectPartnes") as ProjectPartners;
            ctlProjectPartners.PopulateActivities();
        }

        private void SaveData()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            if (projectId == 0) return;

            int projectEmgLocationId = 0;

            using (ORSEntities db = new ORSEntities())
            {
                projectEmgLocationId = db.Projects.Where(x => x.ProjectId == projectId).Select(y => y.EmergencyLocationId).SingleOrDefault();
            }

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

                        DBContext.Add("InsertProjectIndicator2015", new object[] { projectId, indicatorId, projectEmgLocationId, RC.Year._2015,
                                                                                isAdded.Checked, isOPS, RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }
    }
}