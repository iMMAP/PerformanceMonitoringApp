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
using System.Net.Mail;

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

        internal override void BindGridData()
        {
            PopulateClusters();
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
            DataTable dtProject = DBContext.GetData("GetProjectDetails", new object[] { ProjectId, (int)RC.SelectedSiteLanguageId });
            if (dtProject != null && dtProject.Rows.Count > 0)
            {
                ltrlProjectCode.Text = dtProject.Rows[0]["ProjectCode"].ToString();
                txtProjectTitle.Text = dtProject.Rows[0]["ProjectTitle"].ToString();
                txtProjectObjective.Text = dtProject.Rows[0]["ProjectObjective"].ToString();
                ddlCluster.SelectedValue = dtProject.Rows[0]["EmergencyClusterId"].ToString();
                txtDonorName.Text = dtProject.Rows[0]["DonorName"].ToString();
                ddlFundingStatus.SelectedValue = dtProject.Rows[0]["FundingStatus"].ToString();
                txtFromDate.Text = dtProject.Rows[0]["ProjectStartDate"] != null ? Convert.ToDateTime(dtProject.Rows[0]["ProjectStartDate"]).ToString("MM/dd/yyyy") : "";
                txtToDate.Text = dtProject.Rows[0]["ProjectEndDate"] != null ? Convert.ToDateTime(dtProject.Rows[0]["ProjectEndDate"]).ToString("MM/dd/yyyy") : "";
            }
            //using (ORSEntities re = new ORSEntities())
            //{
            //    Project p = re.Projects.Where(x => x.ProjectId == ProjectId).SingleOrDefault();
            //    if (p != null)
            //    {
            //        ltrlProjectCode.Text = p.ProjectCode;
            //        txtProjectTitle.Text = p.ProjectTitle;
            //        txtProjectObjective.Text = p.ProjectObjective;
            //        ddlCluster.SelectedValue = p.EmergencyClusterId.ToString();
            //        //txtDonorName.Text = p.DonorName != null ? p.DonorName : string.Empty;
            //        //ddlFundingStatus.SelectedValue = p.FundingStatus != null ? p.FundingStatus.ToString() : "-1";
            //        txtFromDate.Text = p.ProjectStartDate != null ? p.ProjectStartDate.Value.ToString("MM/dd/yyyy") : "";
            //        txtToDate.Text = p.ProjectEndDate != null ? p.ProjectEndDate.Value.ToString("MM/dd/yyyy") : "";
            //    }
            //}
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
            txtDonorName.Text = string.Empty;
            ddlFundingStatus.SelectedValue = "-1";
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
            ShowMessage((string)GetLocalResourceObject("CreateProjects_SaveMessageSuccess"));
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
            string donorName = !string.IsNullOrEmpty(txtDonorName.Text.Trim()) ? txtDonorName.Text.Trim() : null;
            int? fundingStatus = Convert.ToInt32(ddlFundingStatus.SelectedValue) > 0 ? Convert.ToInt32(ddlFundingStatus.SelectedValue) : (int?)null;
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
                    //using (ORSEntities re = new ORSEntities())
                    //{
                    //    Project project = re.Projects.Single(p => p.ProjectId == ProjectId);
                    //    project.ProjectTitle = title;
                    //    project.ProjectObjective = objective;
                    //    project.EmergencyClusterId = clusterId;
                    //    project.EmergencyLocationId = locationId;
                    //    project.ProjectStartDate = startDate;
                    //    project.ProjectEndDate = endDate;
                    //    project.UpdatedById = userId;
                    //    project.UpdatedDate = DateTime.Now;
                    //    //project.DonorName = donorName;
                    //    //project.FundingStatus = fundingStatus;
                    //    re.SaveChanges();
                    //}

                    DBContext.Update("UpdateProjectDetail", new object[] { ProjectId,title, objective, clusterId, locationId,
                                                             orgId, startDate, endDate, userId,donorName,fundingStatus, DBNull.Value });
                }
                else
                {
                    ProjectId = DBContext.Add("InsertProject", new object[] { title, objective, clusterId, locationId,
                                                             orgId, startDate, endDate, userId,donorName,fundingStatus, DBNull.Value });
                    AddNotification(ProjectId);
                    SendEmailToUser(txtProjectTitle.Text.Trim(), ddlCluster.SelectedItem.Text, clusterId);
                }
            }
        }

        private void SendEmailToUser(string projectTitle, string cluster, int clusterId)
        {
            DataTable dt = DBContext.GetData("GetUserInClusterCountryAndRole", new object[] { clusterId, UserInfo.Country, "ClusterLead" });
            string toEmail = "";
            foreach(DataRow dr in dt.Rows)
            {
                if (string.IsNullOrEmpty(toEmail))
                {
                    toEmail = dr["Email"].ToString();
                }
                else
                {
                    toEmail += "," + dr["Email"].ToString();
                }
            }

            DataTable dtCA = DBContext.GetData("GetCountryAdmins", new object[] { UserInfo.Country, "CountryAdmin" });
            foreach (DataRow dr in dtCA.Rows)
            {
                if (string.IsNullOrEmpty(toEmail))
                {
                    toEmail = dr["Email"].ToString();
                }
                else
                {
                    toEmail += "," + dr["Email"].ToString();
                }
            }

            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                    mailMsg.To.Add(toEmail);
                    mailMsg.Subject = "ORS Project Created/Updated!";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = "New ORS project created with these details Project Title: " + projectTitle + " Cluster: " + cluster.ToString();
                    Mail.SendMail(mailMsg);
                }

            }
            catch
            {
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

        private void AddNotification(int pId)
        {
            using (ORSEntities db = new ORSEntities())
            {
                Notification notification = db.Notifications.CreateObject();
                notification.Notification1 = "New Project Added For " + ddlCluster.SelectedItem.Text;
                notification.SourceUserId = RC.GetCurrentUserId;
                notification.ProjectId = pId;
                notification.EmergencyLocationId = UserInfo.EmergencyCountry;
                notification.EmergencyClusterId = Convert.ToInt32(ddlCluster.SelectedValue);
                notification.OrganizationId = UserInfo.Organization;
                notification.PageURL = "~/ClusterLead/ProjectDetails.aspx?pid=" + pId.ToString();
                notification.IsRead = false;
                db.Notifications.AddObject(notification);
                db.SaveChanges();
            }
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