using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class ctlORSAdmin1Report : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMonths();
                SetGlobalIds();
                PopulateOrganizations();
                SetReportId();
                PopulateIndicators();
            }
        }

        private void PopulateOrganizations()
        {
            DataTable dt = GetProjectPartners();

            ddlOrgs.DataValueField = "OrganizationId";
            ddlOrgs.DataTextField = "OrganizationName";
            ddlOrgs.DataSource = dt;
            ddlOrgs.DataBind();
            if (RC.IsDataEntryUser(((Page)this.Parent.Parent.Parent.Parent.Parent).User))
            {
                ddlOrgs.SelectedValue = UserInfo.Organization.ToString();
            }
        }

        private DataTable GetProjectPartners()
        {
            int orgId = OrganizationId;
            if (RC.IsDataEntryUser(((Page)this.Parent.Parent.Parent.Parent.Parent).User))
            {
                orgId = UserInfo.Organization;
            }
            return DBContext.GetData("GetProjectPartnersDataEntry", new object[] { ProjectId, orgId });
        }

        private void PopulateIndicators()
        {
            DataTable dt = GetProjectIndicators();
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        #region Events.

        protected void ddlMonth_SelectedIndexChnaged(object sender, EventArgs e)
        {
            SetReportId();
            PopulateIndicators();
        }

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfIndicatorId = e.Row.FindControl("hfIndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    Repeater rptCountry = e.Row.FindControl("rptCountry") as Repeater;
                    if (rptCountry != null)
                    {
                        int orgId = RC.GetSelectedIntVal(ddlOrgs);
                        DataTable dt = DBContext.GetData("GetProjectIndicatorDataEntryTarget_Country",
                                                  new object[] { ProjectId, ReportId, orgId, indicatorId });
                        rptCountry.DataSource = dt;
                        rptCountry.DataBind();
                    }
                }
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.ObjectiveLableToolTip(e, 0);
            }
        }

        protected void rptCountry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetCluster");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetMaleProject");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetFemaleProject");
                UI.SetThousandSeparator(e.Item, "lblCountryTargetProject");
                UI.SetThousandSeparator(e.Item, "lblCountryRunningValue");

                HiddenField hfIndicatorId = e.Item.FindControl("hfCountryIndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    HiddenField hfCountryId = e.Item.FindControl("hfCountryId") as HiddenField;
                    int countryId = 0;
                    if (hfCountryId != null)
                    {
                        int.TryParse(hfCountryId.Value, out countryId);
                    }

                    Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        int orgId = RC.GetSelectedIntVal(ddlOrgs);
                        bool isProjectOwner = orgId == OrganizationId;
                        DataTable dt = DBContext.GetData("GetProjectIndicatorDataEntryTarget_Admin1",
                                                            new object[] { ProjectId, ReportId, orgId, isProjectOwner, indicatorId });
                        rptAdmin1.DataSource = dt;
                        rptAdmin1.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1RunningValue");

                HiddenField hfIndicatorId = e.Item.FindControl("hfAdmin1IndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                    {
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);
                    }
                }

                GridViewRow row = (e.Item.Parent.Parent.Parent.Parent.Parent) as GridViewRow;
                if (row != null)
                {
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());

                    TextBox txtAdmin2Male = e.Item.FindControl("txtAdmin1ReportedMaleProject") as TextBox;
                    TextBox txtAdmin2Female = e.Item.FindControl("txtAdmin1ReportedFemaleProject") as TextBox;
                    TextBox txtAdmin2Target = e.Item.FindControl("txtAdmin1ReportedProject") as TextBox;

                    txtAdmin2Male.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Female.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");
                    txtAdmin2Target.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFE0");

                    if (RC.IsGenderUnit(unitId))
                    {
                        txtAdmin2Target.Enabled = false;
                    }
                    else
                    {
                        txtAdmin2Male.Enabled = false;
                        txtAdmin2Female.Enabled = false;
                    }
                }
            }
        }

        #region Button Click Events.

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSaved = false;
            using (TransactionScope scope = new TransactionScope())
            {
                bool isNewReport = false;
                if (ReportId == 0)
                {
                    SaveReportMainInfo();
                    isNewReport = true;
                }

                if (ReportId > 0)
                {
                    SaveReport();
                    if ((InsertUpdateReturnCode == 1 || InsertUpdateReturnCode == 2) && !isNewReport)
                        UpdateReportUpdatedDate();

                    if (InsertUpdateReturnCode == 3)
                        DeleteReport();
                }

                scope.Complete();
                isSaved = true;
                if (RC.SelectedSiteLanguageId == 1)
                    ShowMessage("Your Data Saved Successfully!");
                else
                    ShowMessage("Vos données sauvegardées avec succès");
            }
            if (isSaved)
                PopulateIndicators();
        }

        private void SaveReport()
        {
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    Repeater rptCountry = row.FindControl("rptCountry") as Repeater;
                    if (rptCountry != null)
                    {
                        CountryRepeater(rptCountry, indicatorId, unitId);
                    }
                }
            }
        }

        private void DeleteReport()
        {
            if (ReportId > 0)
                DBContext.Delete("DeleteReportDataEntry", new object[] { ReportId, DBNull.Value });
        }

        private void CountryRepeater(Repeater rptCountry, int indicatorId, int unitId)
        {
            foreach (RepeaterItem item in rptCountry.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfCountryId = item.FindControl("hfCountryId") as HiddenField;
                    int countryId = 0;
                    if (hfCountryId != null)
                        int.TryParse(hfCountryId.Value, out countryId);

                    Repeater rptAdmin1 = item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        if (RC.IsGenderUnit(unitId))
                            SaveGenderReported(rptAdmin1, indicatorId, countryId);
                        else
                            SaveTotalReported(rptAdmin1, indicatorId, countryId);
                    }
                }
            }
        }

        private void SaveTotalReported(Repeater rptAdmin1, int indicatorId, int countryId)
        {
            int returnVal = 0;
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);

                    int? reportedVal = null;
                    TextBox txtReported = item.FindControl("txtAdmin1ReportedProject") as TextBox;
                    reportedVal = string.IsNullOrEmpty(txtReported.Text.Trim()) ? (int?)null : Convert.ToInt32(txtReported.Text.Trim());
                    if (admin1Id > 0)
                        returnVal = DBContext.Update("InsertUpdateProjectIndicatorDataEntry", new object[] {ReportId, indicatorId, countryId, admin1Id, admin1Id,
                                                                                        reportedVal, RC.GetCurrentUserId, DBNull.Value });

                    if (InsertUpdateReturnCode != 1 || InsertUpdateReturnCode != 2)
                        InsertUpdateReturnCode = returnVal;

                }
            }
        }

        private void SaveGenderReported(Repeater rptAdmin1, int indicatorId, int countryId)
        {
            int returnVal = 0;
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin1Male = item.FindControl("txtAdmin1ReportedMaleProject") as TextBox;
                    TextBox txtAdmin1Female = item.FindControl("txtAdmin1ReportedFemaleProject") as TextBox;

                    int? maleAchieved = null;
                    if (txtAdmin1Male != null)
                        maleAchieved = string.IsNullOrEmpty(txtAdmin1Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Male.Text.Trim());

                    int? femaleAchieved = null;
                    if (txtAdmin1Female != null)
                        femaleAchieved = string.IsNullOrEmpty(txtAdmin1Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Female.Text.Trim());

                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);
                    if (admin1Id > 0)
                    {
                        returnVal = DBContext.Update("InsertUpdateProjectIndicatorGenderDataEntry", new object[] {ReportId, indicatorId, 
                                                                                                                    countryId, admin1Id, admin1Id, 
                                                                                                                    maleAchieved, femaleAchieved, 
                                                                                                                    RC.GetCurrentUserId, DBNull.Value });
                        if (InsertUpdateReturnCode != 1 || InsertUpdateReturnCode != 2)
                            InsertUpdateReturnCode = returnVal;
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Methods.

        // Get values from querystring and set variables.
        private void SetGlobalIds()
        {
            // Get projectid from QS
            int tempVal = 0;
            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                ProjectId = tempVal;
            }

            // Get OrgId from QS and then get ReportId
            if (Request.QueryString["orgid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["orgid"].ToString(), out tempVal);
                OrganizationId = tempVal > 0 ? tempVal : UserInfo.Organization;
            }
        }

        private void SetReportId()
        {
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int orgId = RC.GetSelectedIntVal(ddlOrgs);
            using (ORSEntities db = new ORSEntities())
            {
                Report r = db.Reports.Where(x => x.ProjectId == ProjectId
                                            && x.MonthId == monthId
                                            && x.OrganizationId == orgId).SingleOrDefault();
                ReportId = r != null ? r.ReportId : 0;
            }
        }

        private DataTable GetProjectIndicators()
        {
            Project project = null;
            using (ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == ProjectId);
            }

            int emgClusterId = 0;
            int? emgSecClusterId = null;

            if (project != null)
            {
                lblProjectCode.Text = project.ProjectCode;
                emgClusterId = project.EmergencyClusterId;
                emgSecClusterId = project.SecEmergencyClusterId;
            }

            DataTable dt = new DataTable();
            int orgId = RC.GetSelectedIntVal(ddlOrgs);
            bool isProjectOwner = orgId == OrganizationId;

            if (emgClusterId == (int)RC.ClusterSAH2015.MS)
                dt = DBContext.GetData("GetMSRefProjectIndicatorsDataEntry", new object[] { emgSecClusterId, ProjectId, orgId,
                                                                                               isProjectOwner, RC.SelectedSiteLanguageId });
            else
                dt = DBContext.GetData("GetProjectIndicatorsDataEntry", new object[] { ProjectId, orgId, isProjectOwner,
                                                                                        RC.SelectedSiteLanguageId});

            if (dt.Rows.Count <= 0)
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("There is no indicator selected for this project or no target provided for any selected indicator(s). Please close this window and add indicators targets for this project.", RC.NotificationType.Error, false, 500);
                }
                else
                {
                    ShowMessage("Il n y a pas d indicateur selectionné pour ce projet ou pas de cible renseigné dans aucun des indicateurs selectionnés. SVP fermez cette fenêtre et ajoutez des cibles d indicateurs pour ce projet.", RC.NotificationType.Error, false, 500);
                }
            }

            return dt;
        }

        private void PopulateMonths()
        {
            int i = ddlMonth.SelectedIndex;

            ddlMonth.DataValueField = "MonthId";
            ddlMonth.DataTextField = "MonthName";

            ddlMonth.DataSource = GetMonth();
            ddlMonth.DataBind();

            var result = DateTime.Now.ToString("MMMM", new CultureInfo(RC.SiteCulture));
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result);
            int monthNumber = MonthNumber.GetMonthNumber(result);
            monthNumber = monthNumber == 1 ? monthNumber : monthNumber - 1;
            ddlMonth.SelectedIndex = i > -1 ? i : ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(monthNumber.ToString()));
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void SaveReportMainInfo()
        {
            int orgId = RC.GetSelectedIntVal(ddlOrgs);
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            string reportName = lblProjectCode.Text + " (" + ddlMonth.SelectedItem.Text;
            ReportId = DBContext.Add("InsertReport2015", new object[] { monthId, ProjectId, orgId, 
                                                                        RC.GetCurrentUserId, reportName, DBNull.Value });
        }

        private void UpdateReportUpdatedDate()
        {
            DBContext.Update("UpdateReportUpdatedDate", new object[] { ReportId, RC.GetCurrentUserId, DBNull.Value });
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        #endregion

        #region Properties & Enums

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

        private int ReportId
        {
            get
            {
                int reportId = 0;
                if (ViewState["ReportId"] != null)
                    int.TryParse(ViewState["ReportId"].ToString(), out reportId);
                return reportId;
            }
            set
            {
                ViewState["ReportId"] = value.ToString();
            }
        }

        private int OrganizationId
        {
            get
            {
                int orgId = 0;
                if (ViewState["OrganizationId"] != null)
                    int.TryParse(ViewState["OrganizationId"].ToString(), out orgId);
                return orgId;
            }
            set
            {
                ViewState["OrganizationId"] = value.ToString();
            }
        }

        private int InsertUpdateReturnCode
        {
            get
            {
                int returnCode = 0;
                if (ViewState["SaveReturnCode"] != null)
                    int.TryParse(ViewState["SaveReturnCode"].ToString(), out returnCode);
                return returnCode;
            }
            set
            {
                ViewState["SaveReturnCode"] = value.ToString();
            }
        }

        #endregion

        protected void ddlOrgs_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetReportId();
            PopulateIndicators();
        }
    }
}