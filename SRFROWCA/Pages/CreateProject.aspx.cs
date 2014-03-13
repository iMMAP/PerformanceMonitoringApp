using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;
using System.Globalization;

namespace SRFROWCA.Pages
{
    public partial class CreateProject : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateClusters();
            PopulateProjects();
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlCluster, (int)RC.SiteLanguage.English);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void PopulateProjects()
        {
            rblProjects.DataValueField = "ORSProjectId";
            rblProjects.DataTextField = "ProjectTitle";
            rblProjects.DataSource = GetProjects();
            rblProjects.DataBind();
        }

        private void SelectProject()
        {
            if (ORSProjectId > 0)
            {
                rblProjects.SelectedValue = ORSProjectId.ToString();
            }
        }

        private DataTable GetProjects()
        {
            Guid userId = RC.GetCurrentUserId;
            int locationId = UserInfo.GetCountry;
            return DBContext.GetData("GetORSProjectsOfUser", new object[] { locationId, userId });
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            ORSProjectId = Convert.ToInt32(rblProjects.SelectedValue);

            DataTable dt = DBContext.GetData("GetORSProjectDetail", new object[] { ORSProjectId });

            if (dt.Rows.Count > 0)
            {
                txtProjectTitle.Text = dt.Rows[0]["ProjectTitle"].ToString();
                txtProjectObjective.Text = dt.Rows[0]["ProjectObjective"].ToString();
                ddlCluster.SelectedValue = dt.Rows[0]["ClusterId"].ToString();
                txtFromDate.Text = dt.Rows[0]["ProjectStartDate"].ToString();
                txtToDate.Text = dt.Rows[0]["ProjectEndDate"].ToString();
                ltrlProjectCode.Text = dt.Rows[0]["ProjectCode"].ToString();
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            rblProjects.ClearSelection();
            txtProjectTitle.Text = "";
            txtProjectObjective.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ltrlProjectCode.Text = "";
            ddlCluster.SelectedValue = "0";
            ORSProjectId = 0;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            Response.Redirect("~/Pages/AddActivities.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            PopulateProjects();
            SelectProject();
            SelctProjectCode();
            ShowMessage("Your Data Saved Successfuly!");
        }

        private void SelctProjectCode()
        {
            ltrlProjectCode.Text = GetProjectCode();
        }

        protected void btnManageActivities_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/ManageActivities.aspx");
        }

        protected void btnDeleteProject_Click(object sender, EventArgs e)
        {

        }

        private void Save()
        {
            int locationId = UserInfo.GetCountry;
            int orgId = UserInfo.GetOrganization;
            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            int clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                    DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;
            Guid userId = RC.GetCurrentUserId;
            //int projectId = RC.GetSelectedIntVal(rblProjects);

            if (locationId > 0 && clusterId > 0)
            {
                if (ORSProjectId > 0)
                {
                    using (ORSEntities re = new ORSEntities())
                    {
                        ORSProject project = re.ORSProjects.Single(p => p.ORSProjectId == ORSProjectId);
                        project.ProjectTitle = title;
                        project.ProjectObjective = objective;
                        project.ClusterId = clusterId;
                        project.LocationId = locationId;
                        project.ProjectStartDate = startDate;
                        project.ProjectEndDate = endDate;
                        project.UpdatedById = userId;
                        project.UpdatedDate = DateTime.Now;
                        re.SaveChanges();
                    }
                }
                else
                {
                    ORSProjectId = DBContext.Add("InsertProject", new object[] { title, objective, clusterId, locationId,
                                                             orgId, startDate, endDate, userId, DBNull.Value });
                }

            }
        }

        private string GetProjectCode()
        {
            string projectCode = "";
            using (ORSEntities re = new ORSEntities())
            {
                projectCode = re.ORSProjects.Where(p => p.ORSProjectId == ORSProjectId)
                                .Select(p => p.ProjectCode).SingleOrDefault();
            }

            return projectCode;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        public int ORSProjectId
        {
            get
            {
                int orsProjectId = 0;
                if (ViewState["ORSProjectId"] != null)
                {
                    int.TryParse(ViewState["ORSProjectId"].ToString(), out orsProjectId);
                }

                return orsProjectId;
            }
            set
            {
                ViewState["ORSProjectId"] = value.ToString();
            }
        }
    }
}