using BusinessLogic.Projects;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace SRFROWCA.Admin
{
    public partial class ConfigSettings : System.Web.UI.Page
    {
        private DataTable dtConfigSettings = new DataTable();
        public string clsTab1 = "tab-pane fade in active";
        public string clsTab2 = "tab-pane fade in";
        public string liTab1 = "active";
        public string liTab2 = string.Empty;
        public string showCountry = string.Empty;
        public string countryId = string.Empty;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["editKey"])
                || !string.IsNullOrEmpty(Request.QueryString["keySave"])
                || !string.IsNullOrEmpty(Request.QueryString["delKey"]))
            {
                clsTab1 = clsTab1.Replace("active", string.Empty);
                clsTab2 += " active";
                liTab1 = liTab1.Replace("active", string.Empty);
                liTab2 = "active";

                if (!string.IsNullOrEmpty(Request.QueryString["editKey"]))
                {
                    btnAddFrameworkSettings.Text = "Update Settings";
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["keySave"]))
                {
                    if (Convert.ToBoolean(Request.QueryString["keySave"]))
                        lblFrameworkSettings.Text = "Settings saved successfully!";
                    else
                        lblFrameworkSettings.Text = "Could not save settings!";
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["delKey"]))
                {
                    if (Convert.ToBoolean(Request.QueryString["delKey"]))
                        lblFrameworkSettings.Text = "Settings deleted successfully!";
                    else
                        lblFrameworkSettings.Text = "Could not delete settings!";
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["emailSave"]))
            {
                if (Convert.ToBoolean(Request.QueryString["emailSave"]))
                    lblEmailMessage.Text = "Settings saved successfully!";
                else
                    lblEmailMessage.Text = "Could not save settings!";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ShowHideControls();
                ReadConfigKeys();
                LoadCombos();

                if (!string.IsNullOrEmpty(Request.QueryString["editKey"]))
                    FillControls(Convert.ToString("Key-" + Request.QueryString["editKey"]));
            }
        }

        private void ShowHideControls()
        {
            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCountry.Enabled = false;
                showCountry = "display:none";
                countryId = Convert.ToString(UserInfo.EmergencyCountry);
            }
        }

        private void LoadCombos()
        {
            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
        }

        private void SetKeySettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\Settings.xml";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<Settings> </Settings>");

            XmlElement elem = doc.CreateElement("EmailSettings");
            XmlAttribute key = doc.CreateAttribute("StagingEmailSubjectText");
            key.Value = txtStagingSubject.Text;
            elem.SetAttributeNode(key);

            key = doc.CreateAttribute("SendEmail");
            key.Value = rbListEmailSetting.SelectedValue;
            elem.SetAttributeNode(key);

            doc.DocumentElement.AppendChild(elem);
            doc.Save(PATH);

            Response.Redirect("~/Admin/ConfigSettings.aspx?emailSave=true");
        }

        private void SetFrameworkSettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            XmlDocument doc = new XmlDocument();

            if (!File.Exists(PATH))
                doc.LoadXml("<ChangeEndSettings> </ChangeEndSettings>");
            else
                doc.Load(PATH);

            string countryID = string.Empty;
            string country = string.Empty;
            string clusterID = string.Empty;
            string cluster = string.Empty;
            string dateLimit = string.Empty;
            string frameworkCount = string.Empty;
            string activityCount = "";
            string clusterCount = string.Empty;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
            {
                countryID = ddlCountry.SelectedValue;
                country = ddlCountry.SelectedItem.Text.Trim();
            }

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
            {
                clusterID = ddlCluster.SelectedValue;
                cluster = ddlCluster.SelectedItem.Text.Trim();
            }

            if (!string.IsNullOrEmpty(txtDate.Text.Trim()))
                dateLimit = DateTime.ParseExact(txtDate.Text.Trim(), "MM-dd-yyyy", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy"); // txtDate.Text.Trim();

            if (!string.IsNullOrEmpty(txtNoIndicatorsFramework.Text.Trim()))
                frameworkCount = txtNoIndicatorsFramework.Text.Trim();

            if (!string.IsNullOrEmpty(txtNoActivitiesFramework.Text.Trim()))
                activityCount = txtNoActivitiesFramework.Text.Trim();

            if (!string.IsNullOrEmpty(txtNoClusterIndicators.Text.Trim()))
                clusterCount = txtNoClusterIndicators.Text.Trim();

            string configKey = "Key-" + countryID + clusterID;// +dateLimit.Replace("-", string.Empty) + frameworkCount + clusterCount;
            XmlNode settingsNode = doc.DocumentElement;

            bool isExist = false;

            if (File.Exists(PATH))
            {
                XDocument delKey = XDocument.Load(PATH);
                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["editKey"])
                        && node.Name.Equals(Convert.ToString("Key-" + Request.QueryString["editKey"])))
                    {
                        delKey.Descendants(Convert.ToString("Key-" + Request.QueryString["editKey"])).Remove();
                        delKey.Save(PATH);
                        doc.Load(PATH);
                    }
                    else if (string.IsNullOrEmpty(Request.QueryString["editKey"]) &&
                            node.Name.Equals(configKey))
                    {
                        isExist = true;
                        //delKey.Descendants(configKey).Remove();
                        //delKey.Save(PATH);
                        //doc.Load(PATH);
                    }

                }
            }

            if (!isExist)
            {
                if (!configKey.Equals("Key-") &&
                    (!string.IsNullOrEmpty(dateLimit)
                    || !string.IsNullOrEmpty(frameworkCount)
                    || !string.IsNullOrEmpty(activityCount)
                    || !string.IsNullOrEmpty(clusterCount)) &&
                    (ddlCluster.SelectedIndex > -1 || ddlCountry.SelectedIndex > -1))
                {
                    XmlElement elem = doc.CreateElement(configKey);
                    XmlAttribute key;

                    if (!string.IsNullOrEmpty(countryID))
                    {
                        key = doc.CreateAttribute("CountryID");
                        key.Value = countryID;
                        elem.SetAttributeNode(key);

                        key = doc.CreateAttribute("Country");
                        key.Value = country;
                        elem.SetAttributeNode(key);
                    }

                    if (!string.IsNullOrEmpty(clusterID))
                    {
                        key = doc.CreateAttribute("ClusterID");
                        key.Value = clusterID;
                        elem.SetAttributeNode(key);

                        key = doc.CreateAttribute("Cluster");
                        key.Value = cluster;
                        elem.SetAttributeNode(key);
                    }

                    if (!string.IsNullOrEmpty(dateLimit))
                    {
                        key = doc.CreateAttribute("DateLimit");
                        key.Value = dateLimit;
                        elem.SetAttributeNode(key);
                    }

                    if (!string.IsNullOrEmpty(frameworkCount))
                    {
                        key = doc.CreateAttribute("FrameworkCount");
                        key.Value = frameworkCount;
                        elem.SetAttributeNode(key);
                    }

                    if (!string.IsNullOrEmpty(activityCount))
                    {
                        key = doc.CreateAttribute("ActivityCount");
                        key.Value = activityCount;
                        elem.SetAttributeNode(key);
                    }

                    if (!string.IsNullOrEmpty(clusterCount))
                    {
                        key = doc.CreateAttribute("ClusterCount");
                        key.Value = clusterCount;
                        elem.SetAttributeNode(key);
                    }

                    doc.DocumentElement.AppendChild(elem);
                    doc.Save(PATH);

                    Response.Redirect("~/Admin/ConfigSettings.aspx?keySave=true");
                }
            }

            Response.Redirect("~/Admin/ConfigSettings.aspx?keySave=false");
        }

        private void ReadConfigKeys()
        {
            ReadKeySettings();
            ReadFrameworkSettings();
        }

        private void ReadFrameworkSettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                dtConfigSettings.Columns.Add("ConfigKeyID");
                dtConfigSettings.Columns.Add("Country");
                dtConfigSettings.Columns.Add("Cluster");
                dtConfigSettings.Columns.Add("EndDate");
                dtConfigSettings.Columns.Add("NoOfFrameworkIndicators");
                dtConfigSettings.Columns.Add("NoOfFrameworkActivities");
                dtConfigSettings.Columns.Add("NoOfClusterIndicators");

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    DataRow row = dtConfigSettings.NewRow();

                    string configKey = "Key-";

                    if (node.Attributes["CountryID"] != null)
                    {
                        if (!string.IsNullOrEmpty(countryId) && !node.Attributes["CountryID"].Value.Equals(countryId))
                            continue;

                        configKey += Convert.ToString(node.Attributes["CountryID"].Value);
                    }
                        
                    if (node.Attributes["ClusterID"] != null)
                        configKey += Convert.ToString(node.Attributes["ClusterID"].Value);
                    //if (node.Attributes["DateLimit"] != null)
                    //    configKey += Convert.ToString(node.Attributes["DateLimit"].Value.Replace("-", string.Empty));
                    //if (node.Attributes["FrameworkCount"] != null)
                    //    configKey += Convert.ToString(node.Attributes["FrameworkCount"].Value);
                    //if (node.Attributes["ClusterCount"] != null)
                    //    configKey += Convert.ToString(node.Attributes["ClusterCount"].Value);

                    row["ConfigKeyID"] = configKey;
                    row["Country"] = node.Attributes["Country"] != null ? Convert.ToString(node.Attributes["Country"].Value) : string.Empty;
                    row["Cluster"] = node.Attributes["Cluster"] != null ? Convert.ToString(node.Attributes["Cluster"].Value) : string.Empty;
                    row["EndDate"] = node.Attributes["DateLimit"] != null ? Convert.ToString(node.Attributes["DateLimit"].Value) : string.Empty;
                    row["NoOfFrameworkIndicators"] = node.Attributes["FrameworkCount"] != null ? Convert.ToString(node.Attributes["FrameworkCount"].Value) : string.Empty;
                    row["NoOfFrameworkActivities"] = node.Attributes["ActivityCount"] != null ? Convert.ToString(node.Attributes["ActivityCount"].Value) : string.Empty;
                    row["NoOfClusterIndicators"] = node.Attributes["ClusterCount"] != null ? Convert.ToString(node.Attributes["ClusterCount"].Value) : string.Empty;

                    dtConfigSettings.Rows.Add(row);
                }

                gvConfigSettings.DataSource = dtConfigSettings;
                gvConfigSettings.DataBind();
            }
        }

        private void DeleteFrameworkSettings(string keyName)
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XDocument doc = XDocument.Load(PATH);
                doc.Descendants(keyName).Remove();

                doc.Save(PATH);
            }
        }

        private void ReadKeySettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\Settings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("Settings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name == "EmailSettings")
                    {
                        txtStagingSubject.Text = Convert.ToString(node.Attributes["StagingEmailSubjectText"].Value);
                        rbListEmailSetting.SelectedValue = Convert.ToString(node.Attributes["SendEmail"].Value);
                    }
                    //else if (node.Name == "ChangeEndSettings")
                    //{
                    //    txtNoIndicatorsFramework.Text = Convert.ToString(node.Attributes["NoOfIndicatorsFramework"].Value);
                    //    txtNoClusterIndicators.Text = Convert.ToString(node.Attributes["NoOfClusterIndicators"].Value);
                    //}
                }
            }
        }

        public static void GetKeys(out string stagingSubject, out bool sendMail)
        {
            stagingSubject = string.Empty;
            sendMail = false;

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\Settings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("Settings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name == "EmailSettings")
                    {
                        stagingSubject = Convert.ToString(node.Attributes["StagingEmailSubjectText"].Value);
                        sendMail = Convert.ToBoolean(node.Attributes["SendEmail"].Value);
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            /* string textToAdd = string.Empty;
             string valueToAdd = string.Empty;

             if (Convert.ToInt32(ddlCountry.SelectedValue) > -1
                 && Convert.ToInt32(ddlCluster.SelectedValue) > -1
                 && !string.IsNullOrEmpty(txtDate.Text))
             {
                 textToAdd = ddlCountry.SelectedItem.Text + "/";
                 valueToAdd = ddlCountry.SelectedValue + "|";
             }
             if (Convert.ToInt32(ddlCluster.SelectedValue) > -1
                 && !string.IsNullOrEmpty(txtDate.Text))
             {
                 textToAdd += ddlCluster.SelectedItem.Text + "/";
                 valueToAdd += ddlCluster.SelectedValue + "|";

                 textToAdd += txtDate.Text + "/";
                 valueToAdd += txtDate.Text + "|";
             }
             if (!string.IsNullOrEmpty(txtNoIndicatorsFramework.Text))
             {
                 textToAdd += txtNoIndicatorsFramework.Text + "/";
                 valueToAdd += txtNoIndicatorsFramework.Text + "|";
             }
             if (!string.IsNullOrEmpty(txtNoClusterIndicators.Text))
             {
                 textToAdd += txtNoClusterIndicators.Text;
                 valueToAdd += txtNoClusterIndicators.Text;
             }

             bool isExist = false;
             foreach (ListItem item in lstConfigs.Items)
             {
                 if (valueToAdd.Equals(item.Value))
                 {
                     isExist = true;
                     break;
                 }
             }

             if (!isExist && !string.IsNullOrEmpty(textToAdd))
                 lstConfigs.Items.Add(new ListItem(textToAdd, valueToAdd));*/
        }

        protected void lstConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FillDateSettings();
        }

        /*private void FillDateSettings()
        {
            string textToSet = string.Empty;
            string valueToSet = string.Empty;

            if (lstConfigs.SelectedIndex > -1)
            {
                string[] splitValues = lstConfigs.SelectedValue.Split('|');
                string[] splitText = lstConfigs.SelectedItem.Text.Split('/');
                string countryID = string.Empty;
                string clusterID = string.Empty;
                string dateLimit = string.Empty;
                string frameworkCount = string.Empty;
                string clusterCount = string.Empty;

                if (splitValues.Length > 4)
                {
                    countryID = splitValues[0];
                    clusterID = splitValues[1];
                    dateLimit = splitValues[2];
                    frameworkCount = splitValues[3];
                    clusterCount = splitValues[4];
                }
                else if (splitValues.Length > 3)
                {
                    clusterID = splitValues[0];
                    dateLimit = splitValues[1];
                    frameworkCount = splitValues[2];
                    clusterCount = splitValues[3];
                }
                else if (splitValues.Length > 1)
                {
                    frameworkCount = splitValues[0];
                    clusterCount = splitValues[1];
                }

                if (!string.IsNullOrEmpty(countryID))
                    ddlCountry.SelectedValue = countryID;

                if (!string.IsNullOrEmpty(clusterID))
                    ddlCluster.SelectedValue = clusterID;

                if (!string.IsNullOrEmpty(dateLimit))
                    txtDate.Text = dateLimit;

                if (!string.IsNullOrEmpty(frameworkCount))
                    txtNoIndicatorsFramework.Text = frameworkCount;

                if (!string.IsNullOrEmpty(clusterCount))
                    txtNoClusterIndicators.Text = clusterCount;
            }
        }*/

        protected void btnSaveEmailSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SetKeySettings();
                lblEmailMessage.Text = "Settings save successfully!";
            }
            catch (Exception ex)
            {
                lblEmailMessage.Text = "Could not save settings! Error: " + ex.Message;
            }
        }

        protected void btnAddFrameworkSettings_Click(object sender, EventArgs e)
        {
            try
            {
                SetFrameworkSettings();
                ReadFrameworkSettings();

                lblFrameworkSettings.Text = "Settings save successfully!";
            }
            catch (Exception ex)
            {
                lblFrameworkSettings.Text = "Could not save settings! Error: " + ex.Message;
            }
        }

        protected void gvConfigSettings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteButton = e.Row.FindControl("btnDelete") as LinkButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Setting?')");
                }
            }
        }

        protected void gvConfigSettings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string configKeyID = Convert.ToString(e.CommandArgument);

            if (e.CommandName == "DeleteConfig")
            {
                DeleteFrameworkSettings(configKeyID);
                ReadFrameworkSettings();
                lblFrameworkSettings.Text = "Setting deleted successfully!";

                Response.Redirect("~/Admin/ConfigSettings.aspx?delKey=true");
            }
            else if (e.CommandName == "EditConfig")
            {
                Response.Redirect("~/Admin/ConfigSettings.aspx?editKey=" + configKeyID.Replace("Key-", string.Empty));
            }
        }

        private void FillControls(string configKeyID)
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
                    if (node.Name.Equals(configKeyID))
                    {
                        if (node.Attributes["CountryID"] != null)
                            ddlCountry.SelectedValue = Convert.ToString(node.Attributes["CountryID"].Value);
                        if (node.Attributes["ClusterID"] != null)
                            ddlCluster.SelectedValue = Convert.ToString(node.Attributes["ClusterID"].Value);
                        if (node.Attributes["DateLimit"] != null)
                            txtDate.Text = Convert.ToString(node.Attributes["DateLimit"].Value);
                        if (node.Attributes["FrameworkCount"] != null)
                            txtNoIndicatorsFramework.Text = Convert.ToString(node.Attributes["FrameworkCount"].Value);

                        if (node.Attributes["ActivityCount"] != null)
                            txtNoActivitiesFramework.Text = Convert.ToString(node.Attributes["ActivityCount"].Value);

                        if (node.Attributes["ClusterCount"] != null)
                            txtNoClusterIndicators.Text = Convert.ToString(node.Attributes["ClusterCount"].Value);

                        break;
                    }
                }
            }
        }
    }
}