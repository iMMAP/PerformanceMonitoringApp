using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class CountryIndicators : BasePage
    {
        public bool applyFilter = false;
        public int maxCount = 0;
        public DateTime dateLimit = DateTime.Now.AddDays(1);
        public string CountryID = null;
        public string ClusterID = null;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["delete"])
                && Convert.ToBoolean(Request.QueryString["delete"]))
                ShowMessage("Indicator deleted successfully!");
            else if (!string.IsNullOrEmpty(Request.QueryString["delete"]))
                ShowMessage("Error: Indicator cannot be deleted because it is being used!", RC.NotificationType.Error, true, 1000);

            if (RC.IsClusterLead(this.User))
            {
                CountryID = Convert.ToString(UserInfo.EmergencyCountry);
                ClusterID = Convert.ToString(UserInfo.EmergencyCluster);

                applyFilter = true;
                SetMaxCount();
            }
            else if (RC.IsCountryAdmin(this.User))
            {
                CountryID = Convert.ToString(UserInfo.EmergencyCountry);
            }
            else
                maxCount = 1;

            if (maxCount <= 0
                || (applyFilter && DateTime.Now > dateLimit))
                btnAddIndicator.Enabled = false;
            else
                btnAddIndicator.Enabled = true;

            if (RC.IsAdmin(this.User))
            {
                cbIncludeRegional.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowHideControls();
                LoadClusterIndicators();
                LoadCombos();
            }
        }

        private void ShowHideControls()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible = false;

                //ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
            }
            else if (RC.IsClusterLead(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible =
                        ddlCluster.Visible =
                            lblCluster.Visible = false;

                //ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                //ddlCluster.SelectedValue = Convert.ToString(UserInfo.EmergencyCluster);
            }
        }

        private void LoadCombos()
        {
            //UI.FillEmergencyLocations(ddlCountry, 1);
            //UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);

            UI.FillEmergencyLocations(ddlCountry, UserInfo.Emergency, RC.SelectedSiteLanguageId);
            UI.FillEmergnecyClusters(ddlCluster, RC.SelectedSiteLanguageId);

            UI.FillUnits(ddlUnits);

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0,new ListItem("--- Select Country ---", "-1"));
            ddlUnits.Items.Insert(0,new ListItem("--- Select Units ---", "-1"));
        }

        private void LoadClusterIndicators()
        {
            //string objective = null;
            string indicator = null;
            int? countryID = null;
            int? clusterID = null;

            if (!string.IsNullOrEmpty(CountryID))
                countryID = Convert.ToInt32(CountryID);

            if (!string.IsNullOrEmpty(ClusterID))
                clusterID = Convert.ToInt32(ClusterID);

            //if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
            //    objective = txtObjectiveName.Text;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryID = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterID = Convert.ToInt32(ddlCluster.SelectedValue);

            gvClusterIndicators.DataSource = GetClusterIndicatros(clusterID, countryID, indicator);
            gvClusterIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string indicator)
        {
            bool regionalIncluded = cbIncludeRegional.Checked;
            int emergencyId = UserInfo.Emergency;
            if (emergencyId <= 0)
            {
                regionalIncluded = false;
            }

            return DBContext.GetData("uspGetClusterIndicators", new object[] { clusterId, countryId, indicator,
                                                                               RC.SelectedSiteLanguageId, regionalIncluded, emergencyId });
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/AddCountryIndicator.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        private void SetMaxCount()
        {
            //string countryId = string.Empty;
            //string clusterId = string.Empty;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                CountryID = ddlCountry.SelectedValue;

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                ClusterID = ddlCluster.SelectedValue;

            GetMaxCount("Key-" + CountryID + ClusterID, out maxCount, out dateLimit);
        }

        private void GetMaxCount(string configKey, out int maxValu, out DateTime maxDate)
        {
            maxValu = 0;
            maxDate = DateTime.Now.AddDays(1);

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name.Equals(configKey))
                    {
                        if (node.Attributes["ClusterCount"] != null)
                            maxValu = Convert.ToInt32(node.Attributes["ClusterCount"].Value);

                        if (node.Attributes["DateLimit"] != null)
                            maxDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    }
                }
            }

            if (maxValu > 0)
            {
                //string countryId = null;
                //string clusterId = null;

                if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                    CountryID = ddlCountry.SelectedValue;

                if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                    ClusterID = ddlCluster.SelectedValue;

                DataTable dtCount = DBContext.GetData("uspGetIndicatorCount", new object[] { CountryID, ClusterID });

                if (dtCount.Rows.Count > 0)
                    maxValu = maxValu - Convert.ToInt32(dtCount.Rows[0]["IndicatorCount"]);
            }
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            //string objective = null;
            string indicator = null;
            int? countryID = Convert.ToInt32(CountryID);
            int? clusterID = Convert.ToInt32(ClusterID);

            //if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
            //    objective = txtObjectiveName.Text;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryID = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterID = Convert.ToInt32(ddlCluster.SelectedValue);

            DataTable dt = GetClusterIndicatros(clusterID, countryID, indicator);

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterIndicators.DataSource = dt;
                gvClusterIndicators.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvClusterIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 1);
                ObjPrToolTip.CountryIndicatorIcon(e, 2);

                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                Label lblCountryID = e.Row.FindControl("lblCountryID") as Label;
                Label lblClusterID = e.Row.FindControl("lblClusterID") as Label;

                int maxVal = 0;
                DateTime maxDate = DateTime.Now.AddDays(1);
                string configKey = "Key-";

                if (lblCountryID != null && !string.IsNullOrEmpty(lblCountryID.Text))
                    configKey += lblCountryID.Text.Trim();

                if (lblClusterID != null && !string.IsNullOrEmpty(lblClusterID.Text))
                    configKey += lblClusterID.Text.Trim();

                if (applyFilter)
                    GetMaxCount(configKey, out maxVal, out maxDate);

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Setting?')");

                    if (applyFilter && maxDate < DateTime.Now)
                        btnDelete.Visible = false;
                }                

                if (btnEdit != null && applyFilter && maxDate < DateTime.Now)
                    btnEdit.Visible = false;

                string isRegional = e.Row.Cells[1].Text;
                if ((RC.IsCountryAdmin(this.User) || RC.IsClusterLead(this.User)) && isRegional == "True")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }

        protected void gvClusterIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteIndicator")
            {
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);
                string delFlag = "false";

                if (!IndicatorIsInUse(clusterIndicatorID))
                {
                    if (DeleteClusterIndicator(clusterIndicatorID))
                        delFlag = "true";
                }

                //LoadClusterIndicators();
                Response.Redirect("~/ClusterLead/CountryIndicators.aspx?delete=" + delFlag);
            }
            else if (e.CommandName == "EditIndicator")
            {
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                ClearPopupControls();
                hfClusterIndicatorID.Value = clusterIndicatorID.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblIndAlternate = row.FindControl("lblIndAlternate") as Label;
                Label lblUnitID = row.FindControl("lblUnitID") as Label;

                if (gvClusterIndicators.DataKeys[row.RowIndex].Value.ToString() == "1")
                {
                    txtIndicatorEng.Text = row.Cells[7].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorFr.Text = lblIndAlternate.Text;
                }
                else
                {
                    txtIndicatorFr.Text = row.Cells[7].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorEng.Text = lblIndAlternate.Text;
                }

                txtTarget.Text = row.Cells[8].Text;

                if (lblUnitID != null)
                    ddlUnits.SelectedValue = lblUnitID.Text;

                mpeEditIndicator.Show();
            }
        }

        private bool IndicatorIsInUse(int clusterIndicatorID)
        {
            DataTable dt = DBContext.GetData("ClusterIndicatorInUse", new object[] { clusterIndicatorID});
            return dt.Rows.Count > 0;
        }

        private void ClearPopupControls()
        {
            hfClusterIndicatorID.Value = txtIndicatorEng.Text = txtIndicatorFr.Text = string.Empty;
        }

        private bool DeleteClusterIndicator(int indicatorID)
        {
            return Convert.ToBoolean(DBContext.Delete("uspDeleteClusterIndicator", new object[] { indicatorID, null }));
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterIndicators();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            SaveClusterIndicators();
            LoadClusterIndicators();
            ClearPopupControls();
            mpeEditIndicator.Hide();
        }

        private void SaveClusterIndicators()
        {
            if (string.IsNullOrEmpty(txtIndicatorEng.Text))
                txtIndicatorEng.Text = txtIndicatorFr.Text;
            else if (string.IsNullOrEmpty(txtIndicatorFr.Text))
                txtIndicatorFr.Text = txtIndicatorEng.Text;

            Guid userId = RC.GetCurrentUserId;
            string indicatorEng = txtIndicatorEng.Text.Trim();
            string indicatorFr = txtIndicatorFr.Text.Trim();

            string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";
            string cultureTarget = txtTarget.Text.Trim();

            if (RC.SelectedSiteLanguageId.Equals(2))
                cultureTarget = cultureTarget.Replace(".", ",");

            string target = decimal.Round(Convert.ToDecimal(cultureTarget, new CultureInfo(siteCulture)), 0).ToString();
            string unitID = ddlUnits.SelectedValue;

            if (!string.IsNullOrEmpty(hfClusterIndicatorID.Value))
                DBContext.Add("uspInsertIndicator", new object[] { indicatorEng, indicatorFr, target, unitID, null, null, RC.GetCurrentUserId, null, Convert.ToInt32(hfClusterIndicatorID.Value) });
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();

            //string objective = null;
            string indicator = null;
            int? countryID = null;
            int? clusterID = null;

            if (!string.IsNullOrEmpty(CountryID))
                countryID = Convert.ToInt32(CountryID);

            if (!string.IsNullOrEmpty(ClusterID))
                clusterID = Convert.ToInt32(ClusterID);

            //if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
            //    objective = txtObjectiveName.Text;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryID = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterID = Convert.ToInt32(ddlCluster.SelectedValue);

            DataTable dt = GetClusterIndicatros(clusterID, countryID, indicator);

            RemoveColumnsFromDataTable(dt);

            dt.DefaultView.Sort = "Country, Cluster, Indicator, Unit";
            gvExport.DataSource = dt.DefaultView;
            gvExport.DataBind();

            string fileName = "ClusterIndicators";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ClusterIndicatorId");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("IndicatorAlt");
                dt.Columns.Remove("CountryID");
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("UnitId");
                dt.Columns.Remove("IsSRP");
                dt.Columns.Remove("IsRegional");

            }
            catch { }
        }

    }
}