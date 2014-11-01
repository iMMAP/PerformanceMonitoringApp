using BusinessLogic.Projects;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SRFROWCA.Admin
{
    public partial class ConfigSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReadConfigKeys();
                LoadCombos();
                EnableDisableControls();
            }
        }

        private void EnableDisableControls()
        {
            if (HttpContext.Current.User.IsInRole("ClusterLead"))
            {
                ddlCountry.SelectedValue = Convert.ToString(UserInfo.EmergencyCountry);
                ddlCountry.Enabled = false;
            }
        }

        private void LoadCombos()
        {
            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                SetKeySettings();
                SetDateSettings();

                lblMessage.Text = "Settings save successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Could not save settings! Error: " + ex.Message;
            }
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

            elem = doc.CreateElement("LimitSettings");
            key = doc.CreateAttribute("NoOfIndicatorsFramework");
            key.Value = txtNoIndicatorsFramework.Text;
            elem.SetAttributeNode(key);

            key = doc.CreateAttribute("NoOfClusterIndicators");
            key.Value = txtNoClusterIndicators.Text;
            elem.SetAttributeNode(key);

            doc.DocumentElement.AppendChild(elem);
            doc.Save(PATH);
        }

        private void SetDateSettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<ChangeEndSettings> </ChangeEndSettings>");

            foreach (ListItem item in lstConfigs.Items)
            {
                string[] splitValues = item.Value.Split('|');
                string[] splitText = item.Text.Split('/');
                string countryID = string.Empty;
                string country = string.Empty;
                string clusterID = string.Empty;
                string cluster = string.Empty;
                string dateLimit = string.Empty;
                string frameworkCount = string.Empty;
                string clusterCount = string.Empty;

                XmlElement elem = doc.CreateElement("Key-"+item.Value.Replace("|",string.Empty).Replace("-",string.Empty));
                XmlAttribute key;

                if (splitValues.Length > 4)
                {
                    countryID = splitValues[0];
                    country = splitText[0];
                    clusterID = splitValues[1];
                    cluster = splitText[1];
                    dateLimit = splitValues[2];
                    frameworkCount = splitValues[3];
                    clusterCount = splitValues[4];
                }
                else if (splitValues.Length > 3)
                {
                    clusterID = splitValues[0];
                    cluster = splitText[0];
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

                if (!string.IsNullOrEmpty(clusterCount))
                {
                    key = doc.CreateAttribute("ClusterCount");
                    key.Value = clusterCount;
                    elem.SetAttributeNode(key);
                }

                doc.DocumentElement.AppendChild(elem);
            }

            doc.Save(PATH);
        }

        private void ReadConfigKeys()
        {
            ReadKeySettings();
            ReadDateSettings();
        }

        private void ReadDateSettings()
        {
            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";
            lstConfigs.Items.Clear();

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    string textToAdd = string.Empty;
                    string valueToAdd = string.Empty;

                    textToAdd += Convert.ToString(node.Attributes["Country"].Value)+"/";
                    valueToAdd += Convert.ToString(node.Attributes["CountryID"].Value) + "|";
                    textToAdd += Convert.ToString(node.Attributes["Cluster"].Value) + "/";
                    valueToAdd += Convert.ToString(node.Attributes["ClusterID"].Value) + "|";
                    textToAdd += Convert.ToString(node.Attributes["DateLimit"].Value) + "/";
                    valueToAdd += Convert.ToString(node.Attributes["DateLimit"].Value) + "|";
                    textToAdd += Convert.ToString(node.Attributes["FrameworkCount"].Value) + "/";
                    valueToAdd += Convert.ToString(node.Attributes["FrameworkCount"].Value) + "|";
                    textToAdd += Convert.ToString(node.Attributes["ClusterCount"].Value);
                    valueToAdd += Convert.ToString(node.Attributes["ClusterCount"].Value);

                    lstConfigs.Items.Add(new ListItem(textToAdd, valueToAdd));
                }
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
                    else if (node.Name == "ChangeEndSettings")
                    {
                        txtNoIndicatorsFramework.Text = Convert.ToString(node.Attributes["NoOfIndicatorsFramework"].Value);
                        txtNoClusterIndicators.Text = Convert.ToString(node.Attributes["NoOfClusterIndicators"].Value);
                    }
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
            string textToAdd = string.Empty;
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
                lstConfigs.Items.Add(new ListItem(textToAdd, valueToAdd));
        }

        protected void lstConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDateSettings();
        }

        private void FillDateSettings()
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
        }
    }
}