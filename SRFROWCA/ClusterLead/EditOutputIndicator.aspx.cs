using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Transactions;


namespace SRFROWCA.ClusterLead
{
    public partial class EditOutputIndicator : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                DisableDropDowns();
                if (Request.QueryString["id"] != null)
                {
                    LoadIndicators();
                }
                else
                {
                    GetTargetsforRegion(0);
                }
            }
        }

        #region Events.
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTargetsforRegion(0);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!RC.IsRegionalClusterLead(this.User))
            {
                if (Request.QueryString["cnid"] != null)
                {
                    if (!(Request.QueryString["cnid"].ToString() == "11"))
                    {
                        if (IsTargetProvided())
                        {
                            ShowValidationMessage();
                            return;
                        }
                    }
                }
            }

            bool isAdded = true;
            int indicatorId = 0;
            if (Request.QueryString["id"] != null)
            {
                int.TryParse(Request.QueryString["id"].ToString(), out indicatorId);
                isAdded = false;
            }

            using (TransactionScope scope = new TransactionScope())
            {
                indicatorId = SaveIndicator(indicatorId);
                SaveAdmin1Targets(indicatorId);
                scope.Complete();
                SendEmail(isAdded);
                Response.Redirect("~/ClusterLead/CountryIndicators.aspx");
            }
        }

        private void SendEmail(bool isAdded)
        {
            int emgCountryId = RC.GetSelectedIntVal(ddlCountry);
            int? emgClsuterId = RC.GetSelectedIntVal(ddlCluster);
            string subject = "Output Indicator has been updated.";
            if (isAdded)
            {
                subject = "New Output Indicator has been Added to ORS.";
            }
            string country = ddlCountry.SelectedItem.Text;
            string cluster = ddlCluster.SelectedItem.Text;
            string user = "";
            try { user = User.Identity.Name; }
            catch { }

            string body = string.Format(@"<b>{0}</b><br/>
                                         <b>Country:</b> {1}<br/>
                                         <b>Cluster:</b> {2}<br/>
                                         <b>Ind Eng:</b> {3}<br/>
                                         <b>Ind Fr:</b> {4}<br/>
                                         <b>Added By:</b> {5}"
                                         , subject, country, cluster, txtInd1Eng.Text, txtInd1Fr.Text, user);
            RC.SendEmail(emgCountryId, emgClsuterId, subject, body);
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/CountryIndicators.aspx");

        }
        #endregion

        #region Methods.
        internal override void BindGridData()
        {
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            UI.FillUnits(ddlUnit);

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }

            if (Request.QueryString["cid"] != null)
            {
                int cid = 0;
                int.TryParse(Request.QueryString["cid"], out cid);

                if (cid > 0)
                {
                    if (ddlCluster.SelectedIndex == 0)
                    {
                        ddlCluster.SelectedValue = cid.ToString();
                    }
                }
            }

            if (Request.QueryString["cnid"] != null)
            {
                int cnid = 0;
                int.TryParse(Request.QueryString["cnid"], out cnid);

                if (cnid > 0)
                {
                    if (ddlCountry.SelectedIndex == 0)
                    {
                        ddlCountry.SelectedValue = cnid.ToString();
                    }
                }
            }
        }

        // Disable Controls on the basis of user profile
        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (Request.QueryString["cid"] != null)
            {
                int cid = 0;
                int.TryParse(Request.QueryString["cid"], out cid);

                if (cid > 0)
                {
                    if (ddlCluster.SelectedIndex > 0)
                    {
                        RC.EnableDisableControls(ddlCluster, false);
                    }
                }
            }

            if (Request.QueryString["cnid"] != null)
            {
                int cnid = 0;
                int.TryParse(Request.QueryString["cnid"], out cnid);

                if (cnid > 0)
                {
                    if (ddlCluster.SelectedIndex >= 0)
                    {
                        RC.EnableDisableControls(ddlCountry, false);
                    }
                }
            }
        }

        private void LoadIndicators()
        {
            if (Request.QueryString["id"] != null)
            {
                int indicatorId = 0;
                int.TryParse(Request.QueryString["id"].ToString(), out indicatorId);
                DataTable dt = new DataTable();
                dt = DBContext.GetData("GetClusterIndicatorById", new object[] { indicatorId, RC.SiteLanguage.English });
                if (dt.Rows.Count > 0)
                {
                    txtInd1Eng.Text = dt.Rows[0]["IndicatorEng"].ToString();
                    txtInd1Fr.Text = dt.Rows[0]["IndicatorFr"].ToString();
                    ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
                }

                hdnIsRegional.Value = dt.Rows[0]["IsRegional"].ToString();
                DisableIndicatorText(Convert.ToBoolean(dt.Rows[0]["IsRegional"].ToString()));
                GetTargetsforRegion(indicatorId);
            }
        }

        private void DisableIndicatorText(Boolean IsRegional)
        {
            if (RC.IsClusterLead(this.User) && IsRegional)
            {
                EnableDisableEditControls(false);
            }
        }

        private void GetTargetsforRegion(int indicatorId)
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            if (emgLocationId <= 0)
            {
                emgLocationId = UserInfo.EmergencyCountry;
            }
            if (!(RC.IsRegionalClusterLead(this.User) || emgLocationId == 11))
            {
                DataTable dtTargets = GetRegionalAdmin1(emgLocationId, indicatorId);
                rptAdmin1.DataSource = dtTargets;
                rptAdmin1.DataBind();
            }
        }

        private DataTable GetRegionalAdmin1(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetRegionalTargetsForOutputIndicator", new object[] { indicatorId, emgLocationId });
        }

        private void ShowValidationMessage()
        {
            if (RC.SelectedSiteLanguageId == 1)
            {
                ShowMessage("Please provide target for at least one location in all indicators.<br/> If you do not know the target at this time please put a 0 (Zero).", RC.NotificationType.Error, true, 3000);
            }
            else
            {

                ShowMessage("Se il vous plaît fournir cible pour au moins un emplacement de tous les indicateurs.<br/> Si vous ne connaissez pas la cible à ce moment se il vous plaît mettre un 0 (zéro).", RC.NotificationType.Error, true, 3000);
            }
        }

        private int SaveIndicator(int indicatorId)
        {
            string indEng = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            int unitId = RC.GetSelectedIntVal(ddlUnit);

            if (indicatorId > 0)
            {
                DBContext.Add("UpdateClusterIndicatorWithoutTarget", new object[] { indicatorId, indEng, indFr, 
                                                                                        unitId, RC.GetCurrentUserId, DBNull.Value, });
            }
            else
            {
                int countryId = RC.GetSelectedIntVal(ddlCountry);
                int clusterId = RC.GetSelectedIntVal(ddlCluster);
                indicatorId = DBContext.Add("InsertClusterIndicator", new object[] { indEng, indFr, unitId, countryId, clusterId, 
                                                                                        RC.GetCurrentUserId, DBNull.Value });
            }
            return indicatorId;
        }
        private void SaveAdmin1Targets(int indicatorId)
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int isRegional = 0;
            int.TryParse(hdnIsRegional.Value, out isRegional);

            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtTarget = (TextBox)item.FindControl("txtTarget");
                    var hdnLocationId = (HiddenField)item.FindControl("hdnLocationId");
                    int val = 0;
                    bool result = int.TryParse(txtTarget.Text.Trim(), out val);
                    int? target = null;
                    if (result)
                    {
                        target = val;
                    }

                    int locationId = 0;
                    int.TryParse(hdnLocationId.Value, out locationId);
                    if (locationId > 0)
                    {
                        DBContext.Update("InsertOutputIndicatorTarget", new object[] { indicatorId, emgLocationId,
                                            locationId, target, RC.GetCurrentUserId, isRegional, DBNull.Value });
                    }
                }
            }
        }
        private void EnableDisableEditControls(bool isEnabled)
        {
            txtInd1Eng.Enabled = isEnabled;
            txtInd1Eng.BackColor = isEnabled ? Color.White : Color.LightGray;
            txtInd1Fr.Enabled = isEnabled;
            txtInd1Fr.BackColor = isEnabled ? Color.White : Color.LightGray;
            ddlUnit.Enabled = isEnabled;
            ddlUnit.BackColor = isEnabled ? Color.White : Color.LightGray;
        }
        private bool IsTargetProvided()
        {
            bool isTargetValid = true;


            if (!string.IsNullOrEmpty(txtInd1Eng.Text.Trim()) ||
                !string.IsNullOrEmpty(txtInd1Fr.Text.Trim()))
            {
                isTargetValid = false;
                foreach (RepeaterItem item in rptAdmin1.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox txtTarget = item.FindControl("txtTarget") as TextBox;
                        if (!string.IsNullOrEmpty(txtTarget.Text.Trim()))
                        {
                            isTargetValid = true;
                            break;
                        }
                    }
                }
            }
            return !isTargetValid;
        }
        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        #endregion
    }
}