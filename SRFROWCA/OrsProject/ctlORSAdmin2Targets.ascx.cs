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
    public partial class ctlORSAdmin2Targets : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetGlobalIds();
                PopulateIndicators();
            }
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
                    Repeater rptCountry = e.Row.FindControl("rptCountryGender") as Repeater;
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

        protected void rptCountryGender_ItemDataBound(object sender, RepeaterItemEventArgs e)
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
                        DataTable dt = DBContext.GetData("GetProjectTargetOfIndicatorAdmin1_ForAdmin2", new object[] { ProjectId, indicatorId });
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
                        DataTable dt = DBContext.GetData("[GetProjectTargetOfIndicator_Admin2]", new object[] {admin1Id, ProjectId, indicatorId });
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
                //var a = e.Item.Parent.FindControl("");
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
                SaveProjectTargets();
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

        private bool TargetProvided(Repeater rpt, bool isGender)
        {
            bool isTargetValid = false;
            foreach (RepeaterItem item in rpt.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rptAdmin1 = (Repeater)item.FindControl("rptAdmin1");
                    foreach (RepeaterItem admin1Item in rptAdmin1.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Repeater rptAdmin2 = (Repeater)admin1Item.FindControl("rptAdmin2");
                            foreach (RepeaterItem admin2Item in rptAdmin2.Items)
                            {
                                if (isGender)
                                {
                                    TextBox txtTargetMale = admin2Item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                                    TextBox txtTargetFemale = admin2Item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;
                                    if (!string.IsNullOrEmpty(txtTargetMale.Text.Trim()) || !string.IsNullOrEmpty(txtTargetFemale.Text.Trim()))
                                    {
                                        isTargetValid = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    TextBox txtTarget = admin2Item.FindControl("txtAdmin2TargetProject") as TextBox;
                                    if (!string.IsNullOrEmpty(txtTarget.Text.Trim()))
                                    {
                                        isTargetValid = true;
                                        break;
                                    }
                                }
                            }

                            if (isTargetValid) break;
                        }
                    }

                    if (isTargetValid) break;
                }
            }

            return isTargetValid;
        }

        private bool IsTargetProvided()
        {
            bool isTargetValid = false;
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());

                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        if (RC.IsGenderUnit(unitId))
                            isTargetValid = TargetProvided(rptCountry, true);
                        else
                            isTargetValid = TargetProvided(rptCountry, false);
                    }
                    if (isTargetValid) break;
                }
            }

            if (!isTargetValid)
            {
                if (RC.SelectedSiteLanguageId == 1)
                {
                    ShowMessage("Please provide target for at least one location.", RC.NotificationType.Error, true, 3000);
                }
                else
                {

                    ShowMessage("Se il vous plaît fournir cible pour au moins un emplacement.", RC.NotificationType.Error, true, 3000);
                }
            }

            return isTargetValid;
        }

        #endregion

        #region Methods.

        // Get values from querystring and set variables.
        private void SetGlobalIds()
        {
            int tempVal = 0;
            // ProjectId
            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                ProjectId = tempVal;
            }
        }

        private void PopulateIndicators()
        {
            DataTable dtActivities = GetActivities();
            gvActivities.DataSource = dtActivities;
            gvActivities.DataBind();
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

        private void SaveProjectTargets()
        {
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    int unitId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["UnitId"].ToString());
                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
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
                            SaveAdmin2GenderTargets(rptAdmin2, indicatorId, countryId, admin1Id);
                        else
                            SaveAdmin2Targets(rptAdmin2, indicatorId, countryId, admin1Id);


                    }
                }
            }
        }

        private void SaveAdmin2GenderTargets(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtAdmin2Male = item.FindControl("txtAdmin2TargetMaleProject") as TextBox;
                    TextBox txtAdmin2Female = item.FindControl("txtAdmin2TargetFemaleProject") as TextBox;

                    int? maleTarget = null;
                    if (txtAdmin2Male != null)
                        maleTarget = string.IsNullOrEmpty(txtAdmin2Male.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Male.Text.Trim());

                    int? femaleTarget = null;
                    if (txtAdmin2Female != null)
                        femaleTarget = string.IsNullOrEmpty(txtAdmin2Female.Text.Trim()) ? (int?)null : Convert.ToInt32(txtAdmin2Female.Text.Trim());

                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);
                    if (admin2Id > 0)
                    {
                        DBContext.Update("InsertUpdateProjectIndicatorTargetsGender", new object[] {ProjectId, indicatorId, countryId, admin1Id , 
                                                                                        admin2Id, maleTarget, femaleTarget,
                                                                                         RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
        }

        private void SaveAdmin2Targets(Repeater rptAdmin2, int indicatorId, int countryId, int admin1Id)
        {
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);

                    int? target = null;
                    TextBox txtTarget = item.FindControl("txtAdmin2TargetProject") as TextBox;
                    target = string.IsNullOrEmpty(txtTarget.Text.Trim()) ? (int?)null : Convert.ToInt32(txtTarget.Text.Trim());
                    if (admin2Id > 0)
                    {
                        DBContext.Update("InsertUpdateProjectIndicatorTargets", new object[] {ProjectId, indicatorId, countryId, admin1Id, admin2Id,
                                                                                        target, RC.GetCurrentUserId, DBNull.Value });
                    }
                }
            }
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