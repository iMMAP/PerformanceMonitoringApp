using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class CreateProject : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (Request.QueryString["pid"] != null)
            {
                int pID = 0;
                int.TryParse(Utils.DecryptQueryString(Request.QueryString["pid"]), out pID);
                ProjectId = pID;
            }

            PopulateDropDowns();

            if (ProjectId > 0)
            {
                LoadProjectDetails();
                SelectOrganization();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            SelctProjectCode();
            //ShowMessage((string)GetLocalResourceObject("CreateProjects_SaveMessageSuccess"));
        }

        private void PopulateDropDowns()
        {
            PopulateCountries();
            PopulateClusters();
            PopulateOrganizations();

            if (ProjectId <= 0)
                SetComboValues();
            DisableDropDowns();
        }

        private void PopulateCountries()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            else
                ddlCountry.Items.Insert(0, new ListItem("Sélectionner Pays", "0"));
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedEmergencyId);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void PopulateOrganizations()
        {
            ddlOrgs.DataValueField = "OrganizationId";
            ddlOrgs.DataTextField = "OrganizationAcronym";
            ddlOrgs.DataSource = DBContext.GetData("GetAllOrganizations");
            ddlOrgs.DataBind();

            if (ddlOrgs.Items.Count > 0)
            {
                ddlOrgs.Items.Insert(0, new ListItem("Select", "0"));
                ddlOrgs.SelectedIndex = 0;
            }

        }

        private void SetComboValues()
        {
            if (RC.IsDataEntryUser(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlOrgs.SelectedValue = UserInfo.Organization.ToString();
            }

            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User) || RC.IsOCHAStaff(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void DisableDropDowns()
        {
            if (RC.IsDataEntryUser(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
                RC.EnableDisableControls(ddlOrgs, false);
            }

            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
                RC.EnableDisableControls(ddlCluster, false);
            }

            if (RC.IsCountryAdmin(this.User) || RC.IsOCHAStaff(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void LoadProjectDetails()
        {
            DataTable dtProject = GetProjectDetails();
            if (dtProject.Rows.Count > 0)
            {
                ltrlProjectCode.Text = dtProject.Rows[0]["ProjectCode"].ToString();
                txtProjectTitle.Text = dtProject.Rows[0]["ProjectTitle"].ToString();
                txtProjectObjective.Text = dtProject.Rows[0]["ProjectObjective"].ToString();
                txtContactName.Text = dtProject.Rows[0]["ProjectContactName"].ToString();
                txtContactPhone.Text = dtProject.Rows[0]["ProjectContactPhone"].ToString();
                txtContactEmail.Text = dtProject.Rows[0]["ProjectContactEmail"].ToString();
                ddlCluster.SelectedValue = dtProject.Rows[0]["EmergencyClusterId"].ToString();
                ddlCountry.SelectedValue = dtProject.Rows[0]["EmergencyLocationId"].ToString();
                ddlOrgs.SelectedValue = dtProject.Rows[0]["OrganizationId"].ToString();

                DateTime dtFrom = DateTime.Now;
                if (dtProject.Rows[0]["ProjectStartDate"] != DBNull.Value)
                {
                    dtFrom = DateTime.ParseExact(dtProject.Rows[0]["ProjectStartDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    txtFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                }
                else
                {
                    txtFromDate.Text = "";
                }

                DateTime dtTo = DateTime.Now;
                if (dtProject.Rows[0]["ProjectEndDate"] != DBNull.Value)
                {
                    dtTo = DateTime.ParseExact(dtProject.Rows[0]["ProjectEndDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    txtToDate.Text = dtTo.ToString("dd-MM-yyyy");
                }
                else
                {
                    txtToDate.Text = "";
                }
            }
        }

        private void SelectOrganization()
        {
            if (Request.QueryString["oid"] != null)
            {
                int orgId = 0;
                int.TryParse(Utils.DecryptQueryString(Request.QueryString["oid"]), out orgId);

                if (orgId > 0)
                {
                    ddlOrgs.SelectedValue = orgId.ToString();;
                }
            }
        }

        internal override void BindGridData()
        {
            PopulateClusters();
        }

        private DataTable GetProjectDetails()
        {
            return DBContext.GetData("GetProjectDetails", new object[] { ProjectId, (int)RC.SelectedSiteLanguageId });
        }

        private void SelctProjectCode()
        {
            ltrlProjectCode.Text = GetProjectCode();
        }

        private void Save()
        {
            int emgLocationId = 0;
            emgLocationId = RC.GetSelectedIntVal(ddlCountry);

            int orgId = 0;
            orgId = RC.GetSelectedIntVal(ddlOrgs);

            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            int emgClusterId = RC.GetSelectedIntVal(ddlCluster);

            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                     DateTime.ParseExact(txtFromDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture) :
                                 (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            string contactName = !string.IsNullOrEmpty(txtContactName.Text.Trim()) ? txtContactName.Text.Trim() : null;
            string contactPhone = !string.IsNullOrEmpty(txtContactPhone.Text.Trim()) ? txtContactPhone.Text.Trim() : null;
            string contactEmail = !string.IsNullOrEmpty(txtContactEmail.Text.Trim()) ? txtContactEmail.Text.Trim() : null;

            Guid userId = RC.GetCurrentUserId;
            int yearId = (int)RC.Year._Current;
            int year = 2016;
            if (ProjectId > 0)
            {
                int projOrgId = 0;
                if (Request.QueryString["poid"] != null)
                {
                    int.TryParse(Utils.DecryptQueryString(Request.QueryString["poid"]), out projOrgId);
                }

                DBContext.Update("UpdateProjectDetail", new object[] { ProjectId, emgLocationId, emgClusterId, orgId, projOrgId, title, objective, 
                                                                        startDate, endDate, contactName, contactPhone, contactEmail, userId, 
                                                                        DBNull.Value });
            }
            else
            {
                ProjectId = DBContext.Add("InsertProject", new object[] { emgClusterId, emgLocationId, orgId, title, objective, startDate, endDate, 
                                                                              contactName, contactPhone, contactEmail, userId, 
                                                                              year, yearId, DBNull.Value });
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

        private int ProjectId
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