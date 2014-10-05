using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class CreateProject : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateClusters();
            PopulateCurrency();
            LoadProjects();

            if (rblProjects.Items.Count > 0)
            {
                rblProjects.SelectedIndex = 0;
                LoadProjectDetails();
            }

            ToggleButtons();
        }

        private void PopulateCurrency()
        {
            DataTable dt = DBContext.GetData("GetAllCurrency");
            PopulateCurrencyDropDowns(ddlRequestedAmountCurrency, dt);
            PopulateCurrencyDropDowns(ddlDonor1Currency, dt);
            PopulateCurrencyDropDowns(ddlDonor2Currency, dt);
        }

        private void PopulateCurrencyDropDowns(DropDownList ddl, DataTable dt)
        {
            ddl.DataTextField = "CurrencyTitle";
            ddl.DataValueField = "CurrencyId";
            ddl.DataSource = dt;
            ddl.DataBind();

            ListItem item = new ListItem("Select Currency", "0");
            ddl.Items.Insert(0, item);
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
            rblProjects.DataTextField = "ProjectCode";
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
            bool? isOPSProject = null;
            return DBContext.GetData("GetOrgProjectsOnLocation", new object[] { UserInfo.EmergencyCountry, UserInfo.Organization, isOPSProject });
        }

        private void ToggleButtons()
        {
            if (ProjectId > 0)
            {
                if (Convert.ToBoolean(ViewState["IsOpsProject"].ToString()))
                {
                    ToggleControlsForOPS();
                }
                else
                {
                    btnManageActivities.Enabled = true;
                    btnDeleteProject.Enabled = true;
                }
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
                txtDonor1Contributed.Text = dtProject.Rows[0]["Contribution1Amount"].ToString();
                ddlDonor1Currency.SelectedValue = dtProject.Rows[0]["Contribution1CurrencyId"].ToString();
                ddlFundingStatus.SelectedValue = dtProject.Rows[0]["FundingStatus"].ToString();
                ddlProjectSatus.SelectedValue = dtProject.Rows[0]["ProjectStatus"].ToString();
                txtImplementingPartners.Text = dtProject.Rows[0]["ProjectImplementingpartner"].ToString();
                txtRequestedAmount.Text = dtProject.Rows[0]["RequestedAmount"].ToString();
                ddlRequestedAmountCurrency.SelectedValue = dtProject.Rows[0]["RequestedAmountCurrencyId"].ToString();
                txtDonor2Name.Text = dtProject.Rows[0]["DonorName2"].ToString();
                txtDonor2Contributed.Text = dtProject.Rows[0]["Contribution2Amount"].ToString();
                ddlDonor2Currency.SelectedValue = dtProject.Rows[0]["Contribution2CurrencyId"].ToString();
                txtContactName.Text = dtProject.Rows[0]["ProjectContactName"].ToString();
                txtContactPhone.Text = dtProject.Rows[0]["ProjectContactPhone"].ToString();
                txtContactEmail.Text = dtProject.Rows[0]["ProjectContactEmail"].ToString();
                ViewState["IsOpsProject"] = dtProject.Rows[0]["IsOpsProject"].ToString();
               
                DateTime dtFrom = DateTime.Now;
                if (dtProject.Rows[0]["ProjectStartDate"] != DBNull.Value)
                {
                    dtFrom = DateTime.ParseExact(dtProject.Rows[0]["ProjectStartDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    txtFromDate.Text = dtFrom.ToString("MM/dd/yyyy");
                }
                else
                {
                    txtFromDate.Text = "";
                }

                DateTime dtTo = DateTime.Now;
                if (dtProject.Rows[0]["ProjectEndDate"] != DBNull.Value)
                {
                    dtTo = DateTime.ParseExact(dtProject.Rows[0]["ProjectEndDate"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    txtToDate.Text = dtTo.ToString("MM/dd/yyyy");
                }
                else
                {
                    txtToDate.Text = "";
                }
                
                    ToggleControlsForOPS();
                
            }
        }

        private void ToggleControlsForOPS()
        {
            if (Convert.ToBoolean(ViewState["IsOpsProject"].ToString()))
            {
                btnDeleteProject.Enabled = false;
                txtProjectTitle.Enabled = false;
                txtProjectTitle.BackColor =  Color.LightGray;
                txtProjectObjective.Enabled = false;
                txtProjectObjective.BackColor = Color.LightGray;
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtRequestedAmount.Enabled = false;
                ddlDonor1Currency.Enabled = false;
                ddlDonor2Currency.Enabled = false;
                txtDonor2Name.Enabled = false;
                txtDonorName.Enabled = false;
                txtDonor1Contributed.Enabled = false;
                txtDonor2Contributed.Enabled = false;
                txtContactName.Enabled = false;
                txtContactPhone.Enabled = false;
                txtContactEmail.Enabled = false;
                ddlRequestedAmountCurrency.Enabled = false;
                ddlFundingStatus.Enabled = false;
                ddlFundingStatus.BackColor = Color.LightGray;
            }
            else
            {
                btnDeleteProject.Enabled = true;
                txtProjectTitle.Enabled = true;
                txtProjectTitle.BackColor = Color.White;
                txtProjectObjective.Enabled = true;
                txtProjectObjective.BackColor = Color.White;
                ddlCluster.Enabled = true;
                ddlCluster.BackColor = Color.White;
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
                txtRequestedAmount.Enabled = true;
                ddlDonor1Currency.Enabled = true;
                ddlDonor2Currency.Enabled = true;
                txtDonor2Name.Enabled = true;
                txtDonorName.Enabled = true;
                txtDonor1Contributed.Enabled = true;
                txtDonor2Contributed.Enabled = true;
                txtContactName.Enabled = true;
                txtContactPhone.Enabled = true;
                txtContactEmail.Enabled = true;
                ddlRequestedAmountCurrency.Enabled = true;
                ddlFundingStatus.Enabled = true;
                ddlFundingStatus.BackColor = Color.White;
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            ClearProjectControls();
            ToggleButtons();
            ViewState["IsOpsProject"] = false;
            ToggleControlsForOPS();
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
            txtImplementingPartners.Text = "";
            txtRequestedAmount.Text = "";
            txtDonor1Contributed.Text = "";
            txtDonor2Name.Text = "";
            txtDonor2Contributed.Text = "";
            txtContactEmail.Text = "";
            txtContactName.Text = "";
            txtContactPhone.Text = "";
            ddlRequestedAmountCurrency.SelectedValue = "0";
            ddlDonor1Currency.SelectedValue = "0";
            ddlDonor2Currency.SelectedValue = "0";

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
            //using (TransactionScope scope = new TransactionScope())
            {
                //using (ORSEntities db = new ORSEntities())
                //{
                //    List<ProjectOrganization> projOrgs = db.ProjectOrganizations.Where(x => x.ProjectId == ProjectId).ToList<ProjectOrganization>();
                //    foreach (ProjectOrganization po in projOrgs)
                //    {
                //        db.ProjectOrganizations.DeleteObject(po);
                //    }

                //    Project project = db.Projects.Where(x => x.ProjectId == ProjectId).SingleOrDefault();
                //    if (project != null)
                //    {
                //        db.Projects.DeleteObject(project);
                //    }
                //    db.SaveChanges();
                //}
                DBContext.Delete("DeleteProject", new object[] { ProjectId, DBNull.Value });
                //scope.Complete();
            }
        }

        private void Save()
        {
            int locationId = UserInfo.EmergencyCountry;
            int orgId = UserInfo.Organization;
            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            string projectPartners = txtImplementingPartners.Text.Trim();
            int clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            int? fundingStatus = Convert.ToInt32(ddlFundingStatus.SelectedValue) > 0 ? Convert.ToInt32(ddlFundingStatus.SelectedValue) : (int?)null;
            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                    DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            int val = 0;

            int.TryParse(txtRequestedAmount.Text.Trim(), out val);
            int? requestedAmount = val > 0 ? val : (int?)null;
            val = 0;

            int.TryParse(ddlRequestedAmountCurrency.SelectedValue, out val);
            int? requestedCurrencyId = val > 0 ? val : (int?)null;
            val = 0;

            string donorName = !string.IsNullOrEmpty(txtDonorName.Text.Trim()) ? txtDonorName.Text.Trim() : null;
            int.TryParse(txtDonor1Contributed.Text.Trim(), out val);
            int? contribution1 = val > 0 ? val : (int?)null;
            val = 0;
            int.TryParse(ddlDonor1Currency.SelectedValue, out val);
            int? donor1CurrencyId = val > 0 ? val : (int?)null;
            val = 0;

            string donorName2 = !string.IsNullOrEmpty(txtDonor2Name.Text.Trim()) ? txtDonor2Name.Text.Trim() : null;
            int.TryParse(txtDonor2Contributed.Text.Trim(), out val);
            int? contribution2 = val > 0 ? val : (int?)null;
            val = 0;
            int.TryParse(ddlDonor2Currency.SelectedValue, out val);
            int? donor2CurrencyId = val > 0 ? val : (int?)null;
            val = 0;

            string contactName = !string.IsNullOrEmpty(txtContactName.Text.Trim()) ? txtContactName.Text.Trim() : null;
            string contactPhone = !string.IsNullOrEmpty(txtContactPhone.Text.Trim()) ? txtContactPhone.Text.Trim() : null;
            string contactEmail = !string.IsNullOrEmpty(txtContactEmail.Text.Trim()) ? txtContactEmail.Text.Trim() : null;

            Guid userId = RC.GetCurrentUserId;

            if (locationId > 0 && clusterId > 0)
            {
                if (ProjectId > 0)
                {
                    if (Convert.ToBoolean(ViewState["IsOpsProject"].ToString()))
                    {
                        DBContext.Update("UpdateOpsProjectDetail", new object[] { ProjectId, projectPartners, ddlProjectSatus.SelectedValue, userId, DBNull.Value });
                    }
                    else 
                    {
                        
                        DBContext.Update("UpdateProjectDetail", new object[] { ProjectId, clusterId, locationId, orgId, userId, title, objective, projectPartners, startDate, endDate, 
                                                                              requestedAmount, requestedCurrencyId, donorName, contribution1, donor1CurrencyId, donorName2, 
                                                                              contribution2, donor2CurrencyId, fundingStatus, contactName, contactPhone, contactEmail,ddlProjectSatus.SelectedValue, DBNull.Value });
                    }
                }
                else
                {
                    ProjectId = DBContext.Add("InsertProject", new object[] { clusterId, locationId, orgId, userId, title, objective, projectPartners, startDate, endDate, 
                                                                              requestedAmount, requestedCurrencyId, donorName, contribution1, donor1CurrencyId, donorName2, 
                                                                              contribution2, donor2CurrencyId, fundingStatus, contactName, contactPhone, contactEmail, DBNull.Value });
                    AddNotification(ProjectId);
                    SendEmailToUser(txtProjectTitle.Text.Trim(), ddlCluster.SelectedItem.Text, clusterId);
                }
            }
        }

        private void SendEmailToUser(string projectTitle, string cluster, int clusterId)
        {
            DataTable dt = DBContext.GetData("GetUserInClusterCountryAndRole", new object[] { clusterId, UserInfo.Country, "ClusterLead" });
            string toEmail = "";
            foreach (DataRow dr in dt.Rows)
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
            Guid guid = Guid.NewGuid();
            string tempString = "I8$pUs9\\";
            string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string hash = RC.GetHashString(guid + tempString + datetime);

            string notification1 = "New Project Added For " + ddlCluster.SelectedItem.Text;
            int emergencyClusterId = Convert.ToInt32(ddlCluster.SelectedValue);
            string pageURL = "~/ClusterLead/ProjectDetails.aspx?pid=" + pId.ToString();
            bool isRead = false;

            DBContext.Add("InsertNotification", new object[]{notification1, RC.GetCurrentUserId, pId, UserInfo.EmergencyCountry, emergencyClusterId,
                                                               UserInfo.Organization,  pageURL, isRead, hash, DBNull.Value});
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