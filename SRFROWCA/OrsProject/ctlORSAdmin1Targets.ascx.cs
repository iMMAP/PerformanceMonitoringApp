using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class ctlORSAdmin1Targets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetGlobalIds();
                PopulateIndicators();
            }
        }

        private void PopulateIndicators()
        {
            DataTable dtActivities = GetActivities();
            gvActivities.DataSource = dtActivities;
            gvActivities.DataBind();
        }

        #region Events.

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
                        DataTable dt = DBContext.GetData("GetProjectCountryTargetOfIndicator", new object[] { ProjectId, indicatorId });
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
                        DataTable dt1 = DBContext.GetData("GetProjectTargetOfIndicator_Admin1", new object[] { ProjectId, indicatorId });
                        rptAdmin1.DataSource = dt1;
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

                    TextBox txtAdmin2Male = e.Item.FindControl("txtAdmin1TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = e.Item.FindControl("txtAdmin1TargetFemaleProject") as TextBox;
                    TextBox txtAdmin2Target = e.Item.FindControl("txtAdmin1TargetProject") as TextBox;

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
                SaveTargets();
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

        private void SaveTargets()
        {
            //DeleteOPSReportDetails();
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
                        if (RC.IsGenderUnit(unitId))
                            SaveAdmin1GenderTargets(rptAdmin1, indicatorId, countryId);
                        else
                            SaveAdmin1Targets(rptAdmin1, indicatorId, countryId);
                    }
                }
            }
        }

        private void SaveAdmin1Targets(Repeater rptAdmin1, int indicatorId, int countryId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);

                    int? target = null;
                    TextBox txtTarget = item.FindControl("txtAdmin1TargetProject") as TextBox;
                    target = string.IsNullOrEmpty(txtTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTarget.Text.Trim());
                    if (admin1Id > 0)
                        DBContext.Update("InsertUpdateProjectIndicatorTargets", new object[] {ProjectId, indicatorId, countryId, admin1Id, admin1Id,
                                                                                        target, RC.GetCurrentUserId, DBNull.Value });

                }
            }
        }

        private void SaveAdmin1GenderTargets(Repeater rptAdmin1, int indicatorId, int countryId)
        {
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin1Male = item.FindControl("txtAdmin1TargetMaleProject") as TextBox;
                    TextBox txtAdmin1Female = item.FindControl("txtAdmin1TargetFemaleProject") as TextBox;

                    int? maleTarget = null;
                    if (txtAdmin1Male != null)
                        maleTarget = string.IsNullOrEmpty(txtAdmin1Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Male.Text.Trim());

                    int? femaleTarget = null;
                    if (txtAdmin1Female != null)
                        femaleTarget = string.IsNullOrEmpty(txtAdmin1Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin1Female.Text.Trim());

                    HiddenField hfAdmin1Id = item.FindControl("hfAdmin1Id") as HiddenField;
                    int admin1Id = 0;
                    if (hfAdmin1Id != null)
                        int.TryParse(hfAdmin1Id.Value, out admin1Id);
                    if (admin1Id > 0)
                    {
                        DBContext.Update("InsertUpdateProjectIndicatorTargetsGender", new object[] {ProjectId, indicatorId, countryId, admin1Id, admin1Id, 
                                                                                        maleTarget, femaleTarget, RC.GetCurrentUserId, DBNull.Value });
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
            int tempVal = 0;
            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                ProjectId = tempVal;
            }
        }

        private DataTable GetActivities()
        {
            int projectId = 0;
            if (Request.QueryString["pid"] != null)
                int.TryParse(Request.QueryString["pid"].ToString(), out projectId);

            Project project = null;
            using (ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            }

            int emgLocationId = project.EmergencyLocationId;
            int emgClusterId = project.EmergencyClusterId;
            int? emgSecClusterId = project.SecEmergencyClusterId;

            int yearId = (int)project.YearId;
            DataTable dt = new DataTable();
            if (emgClusterId == (int)RC.ClusterSAH2015.MS)
                dt = DBContext.GetData("GetMSRefgFrameworkForProjectTargets", new object[] { emgLocationId, emgClusterId, emgSecClusterId,
                                                                                    projectId, RC.SelectedSiteLanguageId, yearId });
            else
                dt = DBContext.GetData("GetFrameworkForProjectTargets", new object[] { emgLocationId, emgClusterId,
                                                                                    projectId, RC.SelectedSiteLanguageId, yearId });


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

        #endregion
    }
}