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
using System.Transactions;

namespace SRFROWCA.Pages
{
    public partial class CreateProject : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateClusters();
            LoadProjects();

            if (rblProjects.Items.Count > 0)
            {
                rblProjects.SelectedIndex = 0;
                LoadProjectDetails();
            }

            ToggleButtons();
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, UserInfo.Emergency);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void LoadProjects()
        {
            rblProjects.DataValueField = "ProjectId";
            rblProjects.DataTextField = "ProjectTitle";
            rblProjects.DataSource = GetProjects();
            rblProjects.DataBind();
        }

        private void SelectProject()
        {
            if (ProjectId > 0)
            {
                rblProjects.SelectedValue = ProjectId.ToString();
            }
        }

        private DataTable GetProjects()
        {
            int isOPSProjects = 0;
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProjects});
        }

        private void ToggleButtons()
        {
            if (ProjectId > 0)
            {
                btnManageActivities.Enabled = true;
                btnDeleteProject.Enabled = true;
            }
            else
            {
                btnManageActivities.Enabled = false;
                btnDeleteProject.Enabled = false;
            }
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectDetails();
            ToggleButtons();
        }

        private void LoadProjectDetails()
        {
            ProjectId = Convert.ToInt32(rblProjects.SelectedValue);

            using (ORSEntities re = new ORSEntities())
            {
                Project p = re.Projects.Where(x => x.ProjectId == ProjectId).SingleOrDefault();
                if (p != null)
                {
                    ltrlProjectCode.Text = p.ProjectCode;
                    txtProjectTitle.Text = p.ProjectTitle;
                    txtProjectObjective.Text = p.ProjectObjective;
                    ddlCluster.SelectedValue = p.EmergencyClusterId.ToString();
                    txtFromDate.Text = p.ProjectStartDate != null ? p.ProjectStartDate.Value.ToString("MM/dd/yyyy") : "";
                    txtToDate.Text = p.ProjectEndDate != null ? p.ProjectEndDate.Value.ToString("MM/dd/yyyy") : "";
                }
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            ClearProjectControls();
            ToggleButtons();
        }

        private void ClearProjectControls()
        {
            rblProjects.ClearSelection();
            txtProjectTitle.Text = "";
            txtProjectObjective.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ltrlProjectCode.Text = "";
            ddlCluster.SelectedValue = "0";
            ProjectId = 0;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            Response.Redirect("~/Pages/AddActivities.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            LoadProjects();
            SelectProject();
            SelctProjectCode();
            ToggleButtons();
            ShowMessage("Your Data Saved Successfuly!");
        }

        private void SelctProjectCode()
        {
            ltrlProjectCode.Text = GetProjectCode();
        }

        protected void btnManageActivities_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/ManageActivities.aspx?pid=" + ProjectId.ToString());
        }

        protected void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (!IsProjectBeingUsed())
            {
                DeleteProject();
                LoadProjects();
                ClearProjectControls();
                ToggleButtons();
            }
            else
            {
                ShowMessage("This project can not be deleted becasue its being used in reports!", RC.NotificationType.Error);
            }
        }

        private bool IsProjectBeingUsed()
        {
            using (ORSEntities db = new ORSEntities())
            {
                int projCount = db.Reports.Where(x => x.ProjectId == ProjectId).Count();
                return projCount > 0;
            }
        }

        private void DeleteProject()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (ORSEntities db = new ORSEntities())
                {
                    List<ProjectOrganization> projOrgs = db.ProjectOrganizations.Where(x => x.ProjectId == ProjectId).ToList<ProjectOrganization>();
                    foreach (ProjectOrganization po in projOrgs)
                    {
                        db.ProjectOrganizations.DeleteObject(po);
                    }

                    Project project = db.Projects.Where(x => x.ProjectId == ProjectId).SingleOrDefault();
                    if (project != null)
                    {
                        db.Projects.DeleteObject(project);
                    }
                    db.SaveChanges();
                }
                scope.Complete();
            }
        }

        private void Save()
        {
            int locationId = UserInfo.EmergencyCountry;
            int orgId = UserInfo.Organization;
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

            if (locationId > 0 && clusterId > 0)
            {
                if (ProjectId > 0)
                {
                    using (ORSEntities re = new ORSEntities())
                    {
                        Project project = re.Projects.Single(p => p.ProjectId == ProjectId);
                        project.ProjectTitle = title;
                        project.ProjectObjective = objective;
                        project.EmergencyClusterId = clusterId;
                        project.EmergencyLocationId = locationId;
                        project.ProjectStartDate = startDate;
                        project.ProjectEndDate = endDate;
                        project.UpdatedById = userId;
                        project.UpdatedDate = DateTime.Now;
                        re.SaveChanges();
                    }
                }
                else
                {
                    ProjectId = DBContext.Add("InsertProject", new object[] { title, objective, clusterId, locationId,
                                                             orgId, startDate, endDate, userId, DBNull.Value });
                }
            }
        }

        private string GetProjectCode()
        {
            string projectCode = "";
            using (ORSEntities re = new ORSEntities())
            {
                projectCode = re.Projects.Where(p => p.ProjectId == ProjectId)
                                .Select(p => p.ProjectCode).SingleOrDefault();
            }

            return projectCode;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        public int ProjectId
        {
            get
            {
                int projectId = 0;
                if (ViewState["ProjectId"] != null)
                {
                    int.TryParse(ViewState["ProjectId"].ToString(), out projectId);
                }

                return projectId;
            }
            set
            {
                ViewState["ProjectId"] = value.ToString();
            }
        }
    }
}