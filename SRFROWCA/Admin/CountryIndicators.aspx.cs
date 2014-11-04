﻿using BusinessLogic;
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

namespace SRFROWCA.Admin
{
    public partial class CountryIndicators : BasePage
    {
        public int maxCount = 0;
        public DateTime dateLimit = DateTime.Now.AddDays(1);

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            SetMaxCount();

            if (maxCount <= 0)
                btnAddIndicator.Enabled = false;
            else
                btnAddIndicator.Enabled = true;

            if (Convert.ToInt32(ddlCountry.SelectedValue) < 0 && Convert.ToInt32(ddlCluster.SelectedValue) < 0)
                btnAddIndicator.Enabled = true;

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
        }

        private void LoadClusterIndicators()
        {
            string objective = null;
            string indicator = null;
            int? countryId = null;
            int? clusterId = null;

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            gvClusterIndicators.DataSource = GetClusterIndicatros(clusterId, countryId, objective, indicator);
            gvClusterIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string objective, string indicator)
        {
            return DBContext.GetData("uspGetClusterIndicators", new object[] { clusterId, countryId, objective, indicator, RC.SelectedSiteLanguageId });
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AddCountryIndicator.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        private void SetMaxCount()
        {
            string countryId = string.Empty;
            string clusterId = string.Empty;

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
                            maxDate = Convert.ToDateTime(node.Attributes["DateLimit"].Value);
                    }
                }
            }

            if (maxValu > 0)
            {
                string countryId = null;
                string clusterId = null;

                if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                    countryId = ddlCountry.SelectedValue;

                if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                    clusterId = ddlCluster.SelectedValue;

                DataTable dtCount = DBContext.GetData("uspGetIndicatorCount", new object[] { countryId, clusterId });

                if (dtCount.Rows.Count > 0)
                    maxValu = maxValu - Convert.ToInt32(dtCount.Rows[0]["IndicatorCount"]);
            }
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

                GetMaxCount(configKey, out maxVal, out maxDate);

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Setting?')");

                    if (maxDate < DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture))
                        btnDelete.Visible = false;
                }

                if (btnEdit != null && maxDate < DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"), "MM-dd-yyyy", CultureInfo.InvariantCulture))
                    btnEdit.Visible = false;
            }
        }

        protected void gvClusterIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "DeleteIndicator")
            {
                DeleteClusterIndicator(clusterIndicatorID);
                LoadClusterIndicators();

                //lblMessage.Text = "Setting deleted successfully!";
                Response.Redirect("~/Admin/CountryIndicators.aspx");
            }
            else if (e.CommandName == "EditIndicator")
            {

            }
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

    }
}