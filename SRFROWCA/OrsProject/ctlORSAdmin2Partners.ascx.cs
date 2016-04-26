using BusinessLogic;
using Saplin.Controls;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class ctlORSAdmin2Partners : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetProjectId();
                PageIndex = 0;
                PopulateIndicators();
            }
        }


        #region Events.

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int indicatorId = 0;
                int.TryParse(gvActivities.DataKeys[e.Row.RowIndex]["IndicatorId"].ToString(), out indicatorId);
                if (indicatorId > 0)
                {
                    Repeater rptCountry = e.Row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        DataTable dt = GetCountryTable(indicatorId);
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
            }
        }

        private DataTable GetCountryTable(int indicatorId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("LocationName", typeof(string));
            table.Columns.Add("IndicatorId", typeof(string));

            // Here we add five DataRows.
            table.Rows.Add(CountryName, indicatorId);
            return table;
        }

        protected void rptCountryGender_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hfIndicatorId = e.Item.FindControl("hfCountryIndicatorId") as HiddenField;
                int indicatorId = 0;
                if (hfIndicatorId != null)
                {
                    int.TryParse(hfIndicatorId.Value, out indicatorId);
                }
                if (indicatorId > 0)
                {
                    Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                    if (rptAdmin1 != null)
                    {
                        //GetProjectTargetOfIndicatorAdmin1_ForAdmin2
                        DataTable dt = DBContext.GetData("GetProjectIndicatorsAdmin1ForAdmin2", new object[] { ProjectId, indicatorId });
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
                        DataTable dt = DBContext.GetData("[GetProjectIndicatorsAdmin2]", new object[] { admin1Id, ProjectId, indicatorId });
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
                DropDownCheckBoxes ddlOrgs = e.Item.FindControl("ddlOrganizations") as DropDownCheckBoxes;
                if (ddlOrgs != null)
                {
                    HiddenField hfIndicatorId = e.Item.FindControl("hfAdmin2IndicatorId") as HiddenField;
                    int indicatorId = 0;
                    if (hfIndicatorId != null)
                    {
                        int.TryParse(hfIndicatorId.Value, out indicatorId);
                    }
                    if (indicatorId > 0)
                    {
                        HiddenField hfAdmin1Id = e.Item.FindControl("hfAdmin2Id") as HiddenField;
                        int admin1Id = 0;
                        if (hfAdmin1Id != null)
                        {
                            int.TryParse(hfAdmin1Id.Value, out admin1Id);
                        }
                        LoadProjectPartners(ddlOrgs, indicatorId, admin1Id);
                    }
                }
            }
        }

        private DataTable GetProjectPartnersWithIndicators(int indicatorId, int admin1Id)
        {
            return DBContext.GetData("GetProjectPartnersForIndicatorLocation", new object[] { ProjectId, indicatorId, admin1Id });
        }

        private void LoadProjectPartners(DropDownCheckBoxes ddlOrgs, int indicatorId, int admin1Id)
        {
            DataTable dt = GetProjectPartnersWithIndicators(indicatorId, admin1Id);
            ddlOrgs.DataValueField = "OrganizationId";
            ddlOrgs.DataTextField = "OrganizationName";
            ddlOrgs.DataSource = dt;
            ddlOrgs.DataBind();

            SelectIndicatorOrgs(ddlOrgs, dt);
        }

        private void SelectIndicatorOrgs(DropDownCheckBoxes ddlOrgs, DataTable dt)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (ListItem item in ddlOrgs.Items)
                {
                    if (item.Value == row["OrganizationId"].ToString())
                    {
                        if (row["IsExists"].ToString() == "1")
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSaved = false;
            //using (TransactionScope scope = new TransactionScope())
            {
                SavePartners();
                //scope.Complete();
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
        private void SetProjectId()
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
            //int projectId = 0;
            //if (Request.QueryString["pid"] != null)
            //    int.TryParse(Request.QueryString["pid"].ToString(), out projectId);

            Project project = null;
            using (ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == ProjectId);
            }

            int emgLocationId = project.EmergencyLocationId;
            int emgClusterId = project.EmergencyClusterId;
            int? emgSecClusterId = project.SecEmergencyClusterId;

            using (ORSEntities db = new ORSEntities())
            {
                CountryName = (from el in db.EmergencyLocations
                               join l in db.Locations on el.LocationId equals l.LocationId
                               where el.EmergencyLocationId == emgLocationId
                               select l.LocationName).SingleOrDefault();
            }

            DataTable dt = new DataTable();
            if (emgClusterId == (int)RC.ClusterSAH2015.MS)
                dt = DBContext.GetData("GetMSRefgFrameworkForProjectPartners", new object[] { emgSecClusterId, ProjectId, RC.SelectedSiteLanguageId, PageIndex });
            else
                dt = DBContext.GetData("GetFrameworkForProjectPartners", new object[] { ProjectId, RC.SelectedSiteLanguageId, PageIndex});

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
            else
            {
                int numbOfRecords = 0;
                int.TryParse(dt.Rows[0]["NumberOfRecords"].ToString(), out numbOfRecords);
                EnableDisableWizardButtons(numbOfRecords);
            }

            return dt;
        }

        private void EnableDisableWizardButtons(int numOfRecords)
        {
            btnPrevious.Enabled = PageIndex > 0;
            if (numOfRecords / (PageIndex + 1) > 6)
                btnNext.Text = "Next >>";
            else
                btnNext.Text = "Finish";

            int pageTotal = numOfRecords % 6 == 0 ? numOfRecords / 6 : (numOfRecords / 6) + 1;
            lblPageNumber.Text = "Page " + (PageIndex + 1).ToString() + " Of " + pageTotal.ToString();
        }

        private void SavePartners()
        {

            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int indicatorId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["IndicatorId"].ToString());
                    Repeater rptCountry = row.FindControl("rptCountryGender") as Repeater;
                    if (rptCountry != null)
                    {
                        CountryRepeater(rptCountry, indicatorId);
                    }
                }
            }
        }

        private void DeleteProjectPartners(int indicatorId, int locationId)
        {
            DBContext.Delete("DeleteProjectPartnerIndicators", new object[] { ProjectId, indicatorId, locationId, DBNull.Value });
        }

        private void CountryRepeater(Repeater rptCountry, int indicatorId)
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
                        Admin1Repeater(rptAdmin1, indicatorId);
                    }
                }
            }
        }

        private void Admin1Repeater(Repeater rptAdmin1, int indicatorId)
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
                        SaveAdmin2Targets(rptAdmin2, indicatorId);
                    }
                }
            }
        }

        private void SaveAdmin2Targets(Repeater rptAdmin2, int indicatorId)
        {
            foreach (RepeaterItem item in rptAdmin2.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    HiddenField hfAdmin2Id = item.FindControl("hfAdmin2Id") as HiddenField;
                    int admin2Id = 0;
                    if (hfAdmin2Id != null)
                        int.TryParse(hfAdmin2Id.Value, out admin2Id);

                    DeleteProjectPartners(indicatorId, admin2Id);
                    DropDownCheckBoxes ddlOrgs = item.FindControl("ddlOrganizations") as DropDownCheckBoxes;
                    string orgs = RC.GetSelectedValues(ddlOrgs);
                    if (orgs != null)
                    {
                        List<int> selectedOrgs = orgs.Split(',').Select(int.Parse).ToList();
                        foreach (int orgId in selectedOrgs)
                        {
                            DBContext.Add("InsertProjectPartners", new object[] { ProjectId, orgId, indicatorId, admin2Id, (int)RC.Year._Current,
                                                                                RC.GetCurrentUserId, DBNull.Value });
                        }
                    }
                }
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            SavePartners();
            if (((Button)sender).Text == "Finish")
            {
                Page.ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
            else
            {
                PageIndex += 1;
                PopulateIndicators();
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            SavePartners();
            PageIndex -= 1;
            PopulateIndicators();
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

        private string CountryName
        {
            get
            {
                string countryName = "";
                if (ViewState["CountryName"] != null)
                {
                    countryName = ViewState["CountryName"].ToString();
                }

                return countryName;
            }
            set
            {
                ViewState["CountryName"] = value.ToString();
            }
        }

        private int PageIndex
        {
            get
            {
                int pageIndex = 0;
                if (ViewState["PageIndex"] != null)
                {
                    int.TryParse(ViewState["PageIndex"].ToString(), out pageIndex);
                }

                return pageIndex;
            }
            set
            {
                ViewState["PageIndex"] = value.ToString();
            }
        }
    }
}