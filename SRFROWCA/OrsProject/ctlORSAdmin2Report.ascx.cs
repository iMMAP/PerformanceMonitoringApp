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
    public partial class ctlORSAdmin2Report : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateMonths();
                SetGlobalIds();
                PopulateIndicators();
            }
        }


        #region Events.

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

        protected void ddlMonth_SelectedIndexChnaged(object sender, EventArgs e)
        {
            SetGlobalIds();
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
                        DataTable dt = DBContext.GetData("GetProjectIndicatorDataEntryTarget_Country",
                                                  new object[] { ProjectId, ReportId, OrganizationId, indicatorId });
                        rptCountry.DataSource = dt;
                        rptCountry.DataBind();

                        if (dt.Rows.Count == 0)
                        {
                            Label lblNoTarget = e.Row.FindControl("lblNoTarget") as Label;
                            if (lblNoTarget != null)
                                lblNoTarget.Visible = true;
                        }
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
                        DataTable dt = DBContext.GetData("GetProjectIndicatorDataEntryTargetAdmin1_Admin2",
                                                            new object[] { ProjectId, ReportId, OrganizationId, indicatorId });
                        rptAdmin1.DataSource = dt;
                        rptAdmin1.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin1Gender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetCluster");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetMaleProject");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetFemaleProject");
                UI.SetThousandSeparator(e.Item, "lblAdmin1TargetProject");

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

                    Repeater rptAdmin2 = e.Item.FindControl("rptAdmin2") as Repeater;
                    if (rptAdmin2 != null)
                    {
                        DataTable dt = DBContext.GetData("GetProjectIndicatorDataEntryTarget_Admin2",
                                                           new object[] {admin1Id, ProjectId, ReportId, OrganizationId, indicatorId });
                        rptAdmin2.DataSource = dt;
                        rptAdmin2.DataBind();
                    }
                }
            }
        }

        protected void rptAdmin2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GridViewRow row = (e.Item.Parent.Parent.Parent.Parent.Parent.Parent.Parent) as GridViewRow;
                if (row != null)
                {
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());

                    TextBox txtAdmin2Male = e.Item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = e.Item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;
                    TextBox txtAdmin2Target = e.Item.FindControl("txtAdmin2TargetProject") as TextBox;

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

        private void DeleteReport()
        {
            if (ReportId > 0)
                DBContext.Delete("DeleteReportDataEntry", new object[] { ReportId, DBNull.Value });
        }
             

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
                if (OrganizationId > 0)
                {
                    SetReportId();
                    using (ORSEntities db = new ORSEntities())
                    {
                        var org = db.Organizations.FirstOrDefault(x => x.OrganizationId == OrganizationId);
                        lblReportingOrg.Text = "Reporting Organization: " + org.OrganizationAcronym;
                    }
                }
            }
        }

        private void SetReportId()
        {
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            using (ORSEntities db = new ORSEntities())
            {
                Report r = db.Reports.Where(x => x.ProjectId == ProjectId
                                            && x.YearId == (int)RC.Year._Current
                                            && x.MonthId == monthId
                                            && x.OrganizationId == OrganizationId).SingleOrDefault();
                ReportId = r != null ? r.ReportId : 0;
            }
        }

        private void PopulateIndicators()
        {
            DataTable dt = GetProjectIndicators();
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
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

            if (emgClusterId == (int)RC.ClusterSAH2015.MS)
                dt = DBContext.GetData("GetMSRefProjectIndicatorsDataEntry", new object[] { emgSecClusterId, ProjectId, RC.SelectedSiteLanguageId });
            else
                dt = DBContext.GetData("GetProjectIndicatorsDataEntry", new object[] { ProjectId, RC.SelectedSiteLanguageId });

            if (dt.Rows.Count <= 0)
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("The framework for your cluster (i.e. activties and indicators) has not been uploaded yet.<br/> Please contact your Cluster (Sector) coordinator for more information.<br/>Please click on this message to go back to the main window.", RC.NotificationType.Error, false, 500);
                }
                else
                {
                    ShowMessage("le cadre de travail sectoriel (les activités et les indicateurs) ne sont pas encore enregistrés.<br/> Merci de contacter votre coordinateur de cluster (secteur) pour plus de renseignements.<br/>Veuillez cliquer sur ce message pour retourner à l apage principale.", RC.NotificationType.Error, false, 500);
                }
            }

            return dt;
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
                        Admin1Repeater(rptAdmin1, indicatorId, unitId, countryId);
                    }
                }
            }
        }

        private void Admin1Repeater(Repeater rptAdmin1, int indicatorId, int unitId, int countryId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);

                    Repeater rptAdmin2 = item.FindControl("rptAdmin2") as Repeater;
                    if (rptAdmin2 != null)
                    {
                        if (RC.IsGenderUnit(unitId))
                            SaveGenderReported(rptAdmin2, indicatorId, countryId, admin1Id);
                        else
                            SaveTotalReported(rptAdmin2, indicatorId, countryId, admin1Id);
                    }
                }
            }
        }

        private void SaveGenderReported(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            int returnVal = 0;
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin2Male = item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;

                    int? maleAchieved = null;
                    if (txtAdmin2Male != null)
                        maleAchieved = string.IsNullOrEmpty(txtAdmin2Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Male.Text.Trim());

                    int? femaleAchieved = null;
                    if (txtAdmin2Female != null)
                        femaleAchieved = string.IsNullOrEmpty(txtAdmin2Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Female.Text.Trim());

                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);
                    if (admin2Id > 0)
                    {
                        returnVal = DBContext.Update("InsertUpdateProjectIndicatorGenderDataEntry", new object[] {ReportId, indicatorId, 
                                                                                                                    countryId, admin1Id, admin2Id, 
                                                                                                                    maleAchieved, femaleAchieved, 
                                                                                                                    RC.GetCurrentUserId, DBNull.Value });
                        if (InsertUpdateReturnCode != 1 || InsertUpdateReturnCode != 2)
                            InsertUpdateReturnCode = returnVal;
                    }
                }
            }
        }

        private void SaveTotalReported(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            int returnVal = 0;
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);

                    int? reportedVal = null;
                    TextBox txtTarget = item.FindControl("txtAdmin2TargetProject") as TextBox;
                    reportedVal = string.IsNullOrEmpty(txtTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTarget.Text.Trim());
                    if (admin2Id > 0)
                    {
                        returnVal = DBContext.Update("InsertUpdateProjectIndicatorDataEntry", new object[] {ReportId, indicatorId, countryId, admin1Id, admin2Id,
                                                                                        reportedVal, RC.GetCurrentUserId, DBNull.Value });

                        if (InsertUpdateReturnCode != 1 || InsertUpdateReturnCode != 2)
                            InsertUpdateReturnCode = returnVal;
                    }
                }
            }
        }

        private DataTable GetMonth()
        {
            DataTable dt = DBContext.GetData("GetMonths", new object[] { RC.SelectedSiteLanguageId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private void SaveReportMainInfo()
        {
            int yearId = (int)RC.Year._Current;
            int monthId = RC.GetSelectedIntVal(ddlMonth);
            int reportingYear = 2016;
            string reportName = lblProjectCode.Text + " (" + ddlMonth.SelectedItem.Text + "16)";
            ReportId = DBContext.Add("InsertReport2015", new object[] { yearId, monthId, ProjectId, OrganizationId, 
                                                                        RC.GetCurrentUserId, reportName, reportingYear, DBNull.Value });
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
    }
}