using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO; 
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SRFROWCA.ClusterLead
{
    public partial class CountryIndicators : BasePage
    {
        public bool applyFilter = false;
        public int maxCount = 0;
        public DateTime dateLimit = DateTime.Now.AddDays(1);
        public string countryId = null;
        public string clusterId = null;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (RC.IsClusterLead(this.User))
            {
                countryId = Convert.ToString(UserInfo.EmergencyCountry);
                clusterId = Convert.ToString(UserInfo.EmergencyCluster);

                applyFilter = true;
                SetMaxCount();
            }
            else
                maxCount = 1;

            if (maxCount <= 0
                || (applyFilter && DateTime.Now > dateLimit))
                btnAddIndicator.Enabled = false;
            else
                btnAddIndicator.Enabled = true;

            //if (Convert.ToInt32(ddlCountry.SelectedValue) < 0 && Convert.ToInt32(ddlCluster.SelectedValue) < 0)
            //    btnAddIndicator.Enabled = true;

            string control = Utils.GetPostBackControlId(this);
            
            if (control == "btnDelete")
                lblMessage.Text = "Setting deleted successfully!";
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

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
            }
            else if (RC.IsClusterLead(this.User))
            {
                lblCountry.Visible =
                    ddlCountry.Visible =
                        ddlCluster.Visible =
                            lblCluster.Visible = false;

                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCluster.SelectedValue = Convert.ToString(UserInfo.EmergencyCluster);
            }
        }

        private void LoadCombos()
        {
            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
            UI.FillUnits(ddlUnits);
        }

        private void LoadClusterIndicators()
        {
            //string objective = null;
            string indicator = null;
            int? countryID = null;
            int? clusterID = null;

            if(!string.IsNullOrEmpty(countryId))
                countryID = Convert.ToInt32(countryId);

            if(!string.IsNullOrEmpty(clusterId))
                clusterID = Convert.ToInt32(clusterId);

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
            return DBContext.GetData("uspGetClusterIndicators", new object[] { clusterId, countryId, indicator, RC.SelectedSiteLanguageId });
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
                countryId = ddlCountry.SelectedValue;

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = ddlCluster.SelectedValue;

            GetMaxCount("Key-" + countryId + clusterId, out maxCount, out dateLimit);
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
                    countryId = ddlCountry.SelectedValue;

                if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                    clusterId = ddlCluster.SelectedValue;

                DataTable dtCount = DBContext.GetData("uspGetIndicatorCount", new object[] { countryId, clusterId });

                if (dtCount.Rows.Count > 0)
                    maxValu = maxValu - Convert.ToInt32(dtCount.Rows[0]["IndicatorCount"]);
            }
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            //string objective = null;
            string indicator = null;
            int? countryID = Convert.ToInt32(countryId);
            int? clusterID = Convert.ToInt32(clusterId);

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

                if(applyFilter)
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
            }
        }

        protected void gvClusterIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteIndicator")
            {
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                DeleteClusterIndicator(clusterIndicatorID);
                LoadClusterIndicators();

                //lblMessage.Text = "Setting deleted successfully!";
                Response.Redirect("~/ClusterLead/CountryIndicators.aspx");
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
                    txtIndicatorEng.Text = row.Cells[4].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorFr.Text = lblIndAlternate.Text;
                }
                else
                {
                    txtIndicatorFr.Text = row.Cells[4].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorEng.Text = lblIndAlternate.Text;
                }

                txtTarget.Text = row.Cells[5].Text;

                if (lblUnitID != null)
                    ddlUnits.SelectedValue = lblUnitID.Text;

                mpeEditIndicator.Show();
            }
        }

        private void ClearPopupControls()
        {
            hfClusterIndicatorID.Value = txtIndicatorEng.Text = txtIndicatorFr.Text = string.Empty;
        }

        private void DeleteClusterIndicator(int indicatorID)
        {
            DBContext.Delete("uspDeleteClusterIndicator", new object[] { indicatorID, null });
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
            string target = txtTarget.Text.Trim();
            string unitID = ddlUnits.SelectedValue;

            if (!string.IsNullOrEmpty(hfClusterIndicatorID.Value))
                DBContext.Add("uspInsertIndicator", new object[] { indicatorEng, indicatorFr, target, unitID, null, null, RC.GetCurrentUserId, null, Convert.ToInt32(hfClusterIndicatorID.Value) });
        }

    }
}