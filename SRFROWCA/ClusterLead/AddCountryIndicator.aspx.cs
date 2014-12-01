using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Controls;

namespace SRFROWCA.ClusterLead
{
    public partial class AddCountryIndicator : BasePage
    {
        public int maxCount = 0;
        public bool applyFilter = false;
        public string countryId = string.Empty;
        public string clusterId = string.Empty;
        public string displayNone = string.Empty;

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (RC.IsClusterLead(this.User))
            {
                displayNone = "display:none";
                countryId = Convert.ToString(UserInfo.EmergencyCountry);
                clusterId = Convert.ToString(UserInfo.EmergencyCluster);

                applyFilter = true;
                SetMaxCount();

                if (maxCount == 1)
                    IndControlId = maxCount;
            }
            else
                maxCount = 1;

            //if (applyFilter)
            {
                string control = Utils.GetPostBackControlId(this);

                //if (control == "ddlCountry" || control == "ddlCluster")
                //    IndControlId = maxCount;

                if ((applyFilter && IndControlId <= maxCount && control == "btnAddIndicatorControl")
                    || control == "btnAddIndicatorControl")
                    IndControlId += 1;
                else
                    btnAddIndicatorControl.Visible = false;

                if (control == "btnRemoveIndicatorControl")
                    IndControlId -= 1;

                if (IndControlId <= 1)
                    btnRemoveIndicatorControl.Visible = false;
                else
                    btnRemoveIndicatorControl.Visible = true;

                if (applyFilter)
                {
                    if (IndControlId < maxCount)
                        btnAddIndicatorControl.Visible = true;
                    else
                        btnAddIndicatorControl.Visible = false;
                }
                else
                    btnAddIndicatorControl.Visible = true;

                for (int i = 0; i < IndControlId; i++)
                    AddIndicatorControl(i);
            }

            ShowHideControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCombos();
                ShowHideControls();
                //PopulateObjective();

                if (applyFilter)
                    SetMaxCount();

                if (maxCount > 0 && IndControlId == 0)
                {
                    AddIndicatorControl(0);
                    IndControlId = 1;
                }
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

        internal override void BindGridData()
        {
            //PopulateObjective();
            LoadCombos();
        }

        private void PopulateObjective()
        {
            LoadCombos();

            //ListItem item = new ListItem("--- Select Objective ---", "0");
            //ddlObjective.Items.Insert(0, item);
        }

        private void LoadCombos()
        {
            int emergencyId = RC.EmergencySahel2015;
            UI.FillEmergencyLocations(ddlCountry, emergencyId);
            UI.FillEmergnecyClusters(ddlCluster, emergencyId);

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));
        }

        public int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["CountryIndicatorControlId"] != null)
                {
                    int.TryParse(ViewState["CountryIndicatorControlId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["CountryIndicatorControlId"] = value.ToString();
            }
        }

        private void AddIndicatorControl(int i)
        {
            NewCountryIndicatorsControl newIndSet = (NewCountryIndicatorsControl)LoadControl("~/controls/NewCountryIndicatorsControl.ascx");
            newIndSet.ControlNumber = i + 1;
            newIndSet.ID = "countryIndicatorControlId" + i.ToString();
            pnlIndicaotrs.Controls.Add(newIndSet);
        }

        protected void btnAddIndicatorControl_ServerClick(object sender, EventArgs e)
        { }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            Response.Redirect("~/ClusterLead/CountryIndicators.aspx");
        }

        private void SaveData()
        {
            foreach (Control ctl in pnlIndicaotrs.Controls)
            {
                if (ctl != null
                    && ctl.ID != null
                    && ctl.ID.Contains("countryIndicatorControlId"))
                {
                    NewCountryIndicatorsControl indControl = ctl as NewCountryIndicatorsControl;

                    if (indControl != null)
                    {
                        TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                        TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");
                        TextBox txtTarget = (TextBox)indControl.FindControl("txtTarget");
                        DropDownList ddlUnits = (DropDownList)indControl.FindControl("ddlUnits");

                        indControl.SaveIndicators(Convert.ToInt32(ddlCountry.SelectedValue), Convert.ToInt32(ddlCluster.SelectedValue));
                    }
                }
            }
        }

        private void GetMaxCount(string configKey)
        {
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
                            maxCount = Convert.ToInt32(node.Attributes["ClusterCount"].Value);
                    }
                }
            }

            if (maxCount > 0)
            {
                //string countryId = null;
                //string clusterId = null;

                if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                    countryId = ddlCountry.SelectedValue;

                if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                    clusterId = ddlCluster.SelectedValue;

                DataTable dtCount = DBContext.GetData("GetClusterIndicatorCount", new object[] { countryId, clusterId });

                if (dtCount.Rows.Count > 0)
                    maxCount = maxCount - Convert.ToInt32(dtCount.Rows[0]["IndicatorCount"]);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetMaxCount();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SetMaxCount();
        }

        private void SetMaxCount()
        {
            //string countryId = string.Empty;
            //string clusterId = string.Empty;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = ddlCountry.SelectedValue;

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = ddlCluster.SelectedValue;

            GetMaxCount("Key-" + countryId + clusterId);
        }
    }
}