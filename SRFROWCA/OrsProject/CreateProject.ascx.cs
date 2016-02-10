using AjaxControlToolkit;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Pages;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class CreateProject1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;            

            PopulateDropDowns();
            DataTable dt = GetOrganizations();
            PopulateAppealingAgency(ddlOrgs, dt);
            PopulateAppealingAgency(ddlOrgs2, dt);
            PopulateOrganizations(dt);
            int projectId = ((ManageProject)this.Page).ProjectId;
            if (projectId > 0)
            {
                LoadProjectDetails();
                SelectProjectPartners();
                SelectAppealingAgencies();
                HideDisableControls();
            }

            ToggleButtons();
        }

        private void HideDisableControls()
        {
            if (IsOPSProject == "True")
            {
                RC.EnableDisableControls(ddlOrgs, false);
            }

            lblAppealingAgency2.Visible = false;
            ddlOrgs2.Visible = false;
            lblCountry.Visible = false;
            ddlCountry.Visible = false;
        }

        private void SelectAppealingAgencies()
        {
            if (Request.QueryString["oid"] != null)
            {
                int orgId = 0;
                int.TryParse(Utils.DecryptQueryString(Request.QueryString["oid"]), out orgId);

                if (orgId > 0)
                {
                    ddlOrgs.SelectedValue = orgId.ToString();
                }
            }
        }

        private void PopulateDropDowns()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            SetComboValues();
            DisableDropDowns();
            PopulateClusters();
            PopulateCurrency();
        }

        private void PopulateAppealingAgency(DropDownList ddl, DataTable dt)
        {
            ddl.DataValueField = "OrganizationId";
            ddl.DataTextField = "OrganizationAcronym";
            ddl.DataSource = dt;
            ddl.DataBind();

            if (ddl.Items.Count > 0)
            {
                ddl.Items.Insert(0, new ListItem("Select", "0"));
                ddl.SelectedIndex = 0;
            }

        }

        private DataTable GetOrganizations()
        {
            return DBContext.GetData("GetAllOrganizations");
        }

        private void SetComboValues()
        {
            if (!RC.IsAdmin(this.Page.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
            
            if (RC.IsClusterLead(this.Page.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }
        }

        private void DisableDropDowns()
        {
            if (!RC.IsAdmin(this.Page.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsClusterLead(this.Page.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            SelctProjectCode();
            //ShowMessage((string)GetLocalResourceObject("CreateProjects_SaveMessageSuccess"));
            ShowMessage("Project Saved!");
            TabPanel pnlPartners = this.Parent.Parent.NamingContainer.FindControl("tpnlPartners") as TabPanel;
            ProjectPartners ctlProjectParnters = pnlPartners.FindControl("ctlProjectPartnes") as ProjectPartners;
            ctlProjectParnters.PopulateActivities();
        }

        protected void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (!IsProjectBeingUsed())
            {
                DeleteProject();
                //LoadProjects();
                //ClearProjectControls();
                ToggleButtons();
            }
            else
            {
                ShowMessage("This project can not be deleted becasue its being used in reports!", RC.NotificationType.Error);
            }
        }



        private void PopulateOrganizations(DataTable dt)
        {
            gvOrgs.DataSource = dt;
            gvOrgs.DataBind();
        }



        private void PopulateCurrency()
        {
            DataTable dt = DBContext.GetData("GetAllCurrency");
            PopulateCurrencyDropDowns(ddlRequestedAmountCurrency, dt);
            PopulateCurrencyDropDowns(ddlDonor1Currency, dt);
            PopulateCurrencyDropDowns(ddlDonor2Currency, dt);
        }

        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedEmergencyId);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void SelectProjectPartners()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            if (projectId > 0)
            {
                DataTable dt = DBContext.GetData("GetProjectPartnerOrganizations", new object[] { projectId });
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (GridViewRow row in gvOrgs.Rows)
                    {
                        string orgId = gvOrgs.DataKeys[row.RowIndex].Values["OrganizationId"].ToString();
                        if (orgId == dr["OrganizationId"].ToString())
                        {
                            CheckBox cbOrg = row.FindControl("cbOrg") as CheckBox;
                            if (cbOrg != null)
                            {
                                cbOrg.Checked = true;
                            }
                        }
                    }
                }
            }
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

        //internal override void BindGridData()
        //{
        //    PopulateClusters();
        //}

        private void ToggleButtons()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            if (projectId > 0)
            {
                if (IsOPSProject == "True")
                {
                    ToggleControlsForOPS(false);
                }
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
                ddlCluster.SelectedValue = dtProject.Rows[0]["EmergencyClusterId"].ToString();
                txtDonorName.Text = dtProject.Rows[0]["DonorName"].ToString();
                txtDonor1Contributed.Text = dtProject.Rows[0]["Contribution1Amount"].ToString();
                ddlDonor1Currency.SelectedValue = dtProject.Rows[0]["Contribution1CurrencyId"].ToString();
                ddlFundingStatus.SelectedValue = dtProject.Rows[0]["FundingStatus"].ToString();
                ddlProjectSatus.SelectedValue = dtProject.Rows[0]["ProjectStatus"].ToString();
                txtRequestedAmount.Text = dtProject.Rows[0]["RequestedAmount"].ToString();
                ddlRequestedAmountCurrency.SelectedValue = dtProject.Rows[0]["RequestedAmountCurrencyId"].ToString();
                txtDonor2Name.Text = dtProject.Rows[0]["DonorName2"].ToString();
                txtDonor2Contributed.Text = dtProject.Rows[0]["Contribution2Amount"].ToString();
                ddlDonor2Currency.SelectedValue = dtProject.Rows[0]["Contribution2CurrencyId"].ToString();
                txtContactName.Text = dtProject.Rows[0]["ProjectContactName"].ToString();
                txtContactPhone.Text = dtProject.Rows[0]["ProjectContactPhone"].ToString();
                txtContactEmail.Text = dtProject.Rows[0]["ProjectContactEmail"].ToString();
                IsOPSProject = dtProject.Rows[0]["IsOpsProject"].ToString();

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

                ToggleControlsForOPS(!(IsOPSProject == "True"));

            }
        }

        private DataTable GetProjectDetails()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            return DBContext.GetData("GetProjectDetails", new object[] { projectId, (int)RC.SelectedSiteLanguageId });
        }

        private void ToggleControlsForOPS(bool isEnabled)
        {
            {
                //btnDeleteProject.Enabled = isEnabled;
                txtProjectTitle.Enabled = isEnabled;
                txtProjectObjective.Enabled = isEnabled;
                ddlCluster.Enabled = isEnabled;
                txtFromDate.Enabled = isEnabled;
                txtToDate.Enabled = isEnabled;
                txtRequestedAmount.Enabled = isEnabled;
                ddlDonor1Currency.Enabled = isEnabled;
                ddlDonor2Currency.Enabled = isEnabled;
                txtDonor2Name.Enabled = isEnabled;
                txtDonorName.Enabled = isEnabled;
                txtDonor1Contributed.Enabled = isEnabled;
                txtDonor2Contributed.Enabled = isEnabled;
                txtContactName.Enabled = isEnabled;
                txtContactPhone.Enabled = isEnabled;
                txtContactEmail.Enabled = isEnabled;
                ddlRequestedAmountCurrency.Enabled = isEnabled;
                ddlFundingStatus.Enabled = isEnabled;
            }

            //{
            //    btnDeleteProject.Enabled = isEnabled;
            //    txtProjectTitle.Enabled = isEnabled;
            //    txtProjectTitle.BackColor = Color.White;
            //    txtProjectObjective.Enabled = isEnabled;
            //    txtProjectObjective.BackColor = Color.White;
            //    ddlCluster.Enabled = isEnabled;
            //    ddlCluster.BackColor = Color.White;
            //    txtFromDate.Enabled = isEnabled;
            //    txtToDate.Enabled = isEnabled;
            //    txtRequestedAmount.Enabled = isEnabled;
            //    ddlDonor1Currency.Enabled = isEnabled;
            //    ddlDonor1Currency.BackColor = Color.White;
            //    ddlDonor2Currency.Enabled = isEnabled;
            //    ddlDonor2Currency.BackColor = Color.White;
            //    txtDonor2Name.Enabled = isEnabled;
            //    txtDonorName.Enabled = isEnabled;
            //    txtDonor1Contributed.Enabled = isEnabled;
            //    txtDonor2Contributed.Enabled = isEnabled;
            //    txtContactName.Enabled = isEnabled;
            //    txtContactPhone.Enabled = isEnabled;
            //    txtContactEmail.Enabled = isEnabled;
            //    ddlRequestedAmountCurrency.Enabled = isEnabled;
            //    ddlRequestedAmountCurrency.BackColor = Color.White;
            //    ddlFundingStatus.Enabled = isEnabled;
            //    ddlFundingStatus.BackColor = Color.White;
            //}

            //if (Convert.ToBoolean(ViewState["IsOpsProject"].ToString()))
            if (isEnabled)
            {
                txtProjectTitle.BackColor = Color.White;
                txtProjectObjective.BackColor = Color.White;
                ddlCluster.BackColor = Color.White;
                ddlDonor1Currency.BackColor = Color.White;
                ddlDonor2Currency.BackColor = Color.White;
                ddlRequestedAmountCurrency.BackColor = Color.White;
                ddlFundingStatus.BackColor = Color.White;
            }
            else
            {
                txtProjectTitle.BackColor = Color.LightGray;
                txtProjectObjective.BackColor = Color.LightGray;
                ddlCluster.BackColor = Color.LightGray;
                ddlDonor1Currency.BackColor = Color.LightGray;
                ddlDonor2Currency.BackColor = Color.LightGray;
                ddlRequestedAmountCurrency.BackColor = Color.LightGray;
                ddlFundingStatus.BackColor = Color.LightGray;
            }
        }

        private void SelctProjectCode()
        {
            ltrlProjectCode.Text = GetProjectCode();
        }

        private bool IsProjectBeingUsed()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            return ((DBContext.GetData("IsReportExistsForProject", new object[] { projectId }).Rows.Count > 0));
        }

        private void DeleteProject()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            DBContext.Delete("DeleteProject", new object[] { projectId, DBNull.Value });
        }

        private void Save()
        {
            int emgLocationId = 0;
            emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : RC.GetSelectedIntVal(ddlCountry);
            int orgId = 0;
            orgId = UserInfo.Organization > 0 ? UserInfo.Organization : RC.GetSelectedIntVal(ddlOrgs);

            int val = RC.GetSelectedIntVal(ddlOrgs2);
            int? org2Id = val > 0 ? val : (int?)null;

            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            string projectPartners = ""; // txtImplementingPartners.Text.Trim();
            int clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            int? fundingStatus = Convert.ToInt32(ddlFundingStatus.SelectedValue) > 0 ? Convert.ToInt32(ddlFundingStatus.SelectedValue) : (int?)null;
            DateTime? startDate = txtFromDate.Text.Trim().Length > 0 ?
                                    DateTime.ParseExact(txtFromDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            DateTime? endDate = txtToDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtToDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture) :
                                (DateTime?)null;

            val = 0;
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

            if (emgLocationId > 0 && clusterId > 0 && orgId > 0)
            {
                int projectId = ((ManageProject)this.Page).ProjectId;
                if (projectId > 0)
                {
                    SaveProjectPartners();
                    if ((IsOPSProject == "True"))
                    {
                        DBContext.Update("UpdateOpsProjectDetail", new object[] { projectId, projectPartners, ddlProjectSatus.SelectedValue, userId, DBNull.Value });
                    }
                    else
                    {
                        int projOrgId = 0;
                        if (Request.QueryString["poid"] != null)
                        {
                            int.TryParse(Utils.DecryptQueryString(Request.QueryString["poid"]), out projOrgId);
                        }
                        DBContext.Update("UpdateProjectDetail", new object[] { projectId, clusterId, orgId, projOrgId, userId, title, objective, projectPartners, startDate, endDate, 
                                                                              requestedAmount, requestedCurrencyId, donorName, contribution1, donor1CurrencyId, donorName2, 
                                                                              contribution2, donor2CurrencyId, fundingStatus, contactName, contactPhone, contactEmail,ddlProjectSatus.SelectedValue, DBNull.Value });
                    }
                }
                else
                {

                    ((ManageProject)this.Page).ProjectId = DBContext.Add("InsertProject", new object[] { clusterId, emgLocationId, orgId, org2Id, userId, title, objective, projectPartners, startDate, endDate, 
                                                                              requestedAmount, requestedCurrencyId, donorName, contribution1, donor1CurrencyId, donorName2, 
                                                                              contribution2, donor2CurrencyId, fundingStatus, contactName, contactPhone, contactEmail, DBNull.Value });
                    SaveProjectPartners();

                    TabPanel pnlActivities = this.Parent.Parent.NamingContainer.FindControl("tpnlActivities") as TabPanel;
                    ProjectActivities ctlProjectActivites = pnlActivities.FindControl("ctlProjectActivities") as ProjectActivities;
                    ctlProjectActivites.PopulateLogFrame();

                    TabPanel pnlPartners = this.Parent.Parent.NamingContainer.FindControl("tpnlPartners") as TabPanel;
                    ProjectPartners ctlProjectParnters = pnlPartners.FindControl("ctlProjectPartnes") as ProjectPartners;
                    ctlProjectParnters.PopulateLocations();
                }
            }
        }

        private void SaveProjectPartners()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            DBContext.Delete("DeleteProjectPartnerOrganizations", new object[] { projectId, DBNull.Value });

            foreach (GridViewRow row in gvOrgs.Rows)
            {
                CheckBox cbOrg = row.FindControl("cbOrg") as CheckBox;
                if (cbOrg != null)
                {
                    if (cbOrg.Checked)
                    {
                        int orgId = 0;
                        int.TryParse(gvOrgs.DataKeys[row.RowIndex].Values["OrganizationId"].ToString(), out orgId);
                        DBContext.Add("InsertProjectPartners", new object[] { projectId, orgId, RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
        }

        private string GetProjectCode()
        {
            int projectId = ((ManageProject)this.Page).ProjectId;
            string projectCode = "";
            using (ORSEntities re = new ORSEntities())
            {
                projectCode = re.Projects.Where(p => p.ProjectId == projectId)
                                .Select(p => p.ProjectCode).SingleOrDefault();
            }

            return projectCode;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        private string IsOPSProject
        {
            get
            {
                if (ViewState["IsOpsProject"] != null)
                {
                    return ViewState["IsOpsProject"].ToString();
                }
                return "false";
            }
            set
            {
                ViewState["IsOpsProject"] = value.ToString();
            }
        }
    }
}