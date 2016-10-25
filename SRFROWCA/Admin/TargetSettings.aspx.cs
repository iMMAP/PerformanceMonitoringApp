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
using System.Xml.Linq;

namespace SRFROWCA.Admin
{
    public partial class TargetSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCombos();
            LoadSettings();
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));

            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
        }

        private void LoadSettings()
        {
            gvSettings.DataSource = GetFrameworkSettings();
            gvSettings.DataBind();
        }

        private DataTable GetFrameworkSettings()
        {
            int val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocId = val == 0 ? (int?)null : val;
            val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val == 0 ? (int?)null : val;

            int year = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("GetTargetSettings", new object[] { emgLocId, emgClusterId, year });
        }

        protected void ddlSelected(object sender, EventArgs e)
        {
            int countryId = RC.GetSelectedIntVal(ddlCountry);
            int clusterId = RC.GetSelectedIntVal(ddlCluster);
            int year = RC.GetSelectedIntVal(ddlFrameworkYear);

            if (countryId > 0 && clusterId > 0)
            {
                FillControls(countryId, clusterId);
            }

            LoadSettings();
        }

        private void FillControls(int countryId, int clusterId)
        {
            DataTable dt = GetFrameworkSettings();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                DateTime dtFrom = DateTime.Now;
                if (row["DateLimit"] != DBNull.Value)
                {
                    dtFrom = DateTime.ParseExact(row["DateLimit"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    txtDate.Text = dtFrom.ToString("dd-MM-yyyy");
                }
                else
                {
                    txtDate.Text = "";
                }

                ddlFrameworkYear.SelectedValue = row["Year"].ToString();
                txtNoClusterIndicators.Text = row["ClusterIndicatorMax"].ToString();
                txtNoActivitiesFramework.Text = row["ActivityMax"].ToString();
                txtNoIndicatorsFramework.Text = row["IndicatorMax"].ToString();
                ddlLevel.SelectedValue = row["AdminLevel"].ToString();
                ddlType.SelectedValue = row["AdminType"].ToString();


                if (row["IsTargetNeeded"].ToString() == "True")
                {
                    rbTargetYes.Checked = true;
                    rbTargetNo.Checked = false;
                }
                else
                {
                    rbTargetNo.Checked = true;
                    rbTargetYes.Checked = false;
                }

                if (row["IsTargetMandatory"].ToString() == "True")
                {
                    rbMandatoryYes.Checked = true;
                    rbMandatoryNo.Checked = false;
                }
                else
                {
                    rbMandatoryNo.Checked = true;
                    rbMandatoryYes.Checked = false;
                }

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetFrameworkSettings();
            lblFrameworkSettings.Text = "Settings save successfully!";
        }

        private void SetFrameworkSettings()
        {
            AddSettingsKey();
            LoadSettings();
            ClearControls();
        }

        private void ClearControls()
        {
            txtDate.Text = "";
            txtNoClusterIndicators.Text = "";
            txtNoActivitiesFramework.Text = "";
            txtNoIndicatorsFramework.Text = "";
            ddlLevel.SelectedIndex = 0;
            ddlType.SelectedIndex = 0;
            rbTargetYes.Checked = true;
            rbMandatoryYes.Checked = true;
        }

        private void AddSettingsKey()
        {
            List<ListItem> countriesList = RC.GetListControlItems(ddlCountry);
            List<ListItem> clustersList = RC.GetListControlItems(ddlCluster);

            foreach (ListItem country in countriesList)
            {
                foreach (ListItem cluster in clustersList)
                {
                    int year = RC.GetSelectedIntVal(ddlFrameworkYear);
                    string key = country.Value + cluster.Value + year.ToString();
                    int emgLocId = Convert.ToInt32(country.Value);
                    int emgClusterId = Convert.ToInt32(cluster.Value);
                    DateTime dateLimit = DateTime.ParseExact(txtDate.Text.Trim(),
                                                                "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    int outputInd = Convert.ToInt32(txtNoClusterIndicators.Text.Trim());
                    int activities = Convert.ToInt32(txtNoActivitiesFramework.Text.Trim());
                    int indicators = Convert.ToInt32(txtNoIndicatorsFramework.Text.Trim());
                    string adminLevel = ddlLevel.SelectedValue;
                    string adminType = ddlType.SelectedValue;
                    int isTarget = rbTargetYes.Checked ? 1 : 0;
                    int isMandatory = rbMandatoryYes.Checked ? 1 : 0;

                    DBContext.Add("InsertAdminTargetSettings", new object[]{key, year, emgLocId, emgClusterId, dateLimit, outputInd
                                                                             ,activities, indicators, adminLevel, adminType
                                                                             ,isTarget, isMandatory, DBNull.Value});
                }
            }
        }

        private void CreateSettingsAttribute(XmlDocument doc, XmlElement elem, string name, string value)
        {
            XmlAttribute key;
            key = doc.CreateAttribute(name);
            key.Value = value;
            elem.SetAttributeNode(key);
        }

        private void DeleteElement()
        {
            int val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocId = val == 0 ? (int?)null : val;
            val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val == 0 ? (int?)null : val;

            int year = RC.GetSelectedIntVal(ddlFrameworkYear);

            DBContext.Delete("DeleteAdminTargetSettings", new object[] { emgLocId, emgClusterId, year, DBNull.Value });
        }
    }
}