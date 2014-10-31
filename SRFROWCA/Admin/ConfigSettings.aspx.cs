﻿using BusinessLogic.Projects;
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
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\LimitSettings.xml";

            XmlDocument doc = new XmlDocument();

            //if (!File.Exists(PATH))
                doc.LoadXml("<LimitSettings> </LimitSettings>");
            //else
            //    doc.Load(PATH);

            XmlElement elem = doc.CreateElement("LimitSettings");
            XmlAttribute key = doc.CreateAttribute("NoOfIndicatorsFramework");
            key.Value = txtNoIndicatorsFramework.Text;
            elem.SetAttributeNode(key);

            key = doc.CreateAttribute("NoOfClusterIndicators");
            key.Value = txtNoClusterIndicators.Text;
            elem.SetAttributeNode(key);

            doc.DocumentElement.AppendChild(elem);
            doc.Save(PATH);
        }

        private void ReadConfigKeys()
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
                    else if (node.Name == "LimitSettings")
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

            if (ddlCountry.SelectedIndex > 0)
            {
                textToAdd = ddlCountry.SelectedItem.Text + "/";
                valueToAdd = ddlCountry.SelectedValue+"|";
            }
            if (ddlCountry.SelectedIndex > 0)
            {
                textToAdd += ddlCluster.SelectedItem.Text + "/";
                valueToAdd += ddlCluster.SelectedValue + "|";
            }
            if (!string.IsNullOrEmpty(txtNoIndicatorsFramework.Text))
                textToAdd += txtNoIndicatorsFramework.Text + "/";
            if (!string.IsNullOrEmpty(txtNoClusterIndicators.Text))
                textToAdd += txtNoClusterIndicators.Text;

            if(!string.IsNullOrEmpty(textToAdd))
                lstConfigs.Items.Add(new ListItem(textToAdd,valueToAdd));
        }
    }
}