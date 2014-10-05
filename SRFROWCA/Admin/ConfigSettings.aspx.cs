using BusinessLogic.Projects;
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
                //txtStagingSubject.Text = Convert.ToString(ConfigurationManager.AppSettings["StagingEmailSubjectText"]);
                //rbListEmailSetting.SelectedValue = Convert.ToString(ConfigurationManager.AppSettings["SendEmail"]);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                /*var webConfig = WebConfigurationManager.OpenWebConfiguration("~");
                var webSection = (AppSettingsSection)webConfig.GetSection("appSettings");
                webSection.Settings["StagingEmailSubjectText"].Value = txtStagingSubject.Text;
                webConfig.Save();

                webSection.Settings["SendEmail"].Value = rbListEmailSetting.SelectedValue;
                webConfig.Save();*/

                string PATH = HttpRuntime.AppDomainAppPath;
                PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\Settings.xml";

                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Settings> </Settings>");

                XmlElement elem = doc.CreateElement("KeySettings");
                XmlAttribute key = doc.CreateAttribute("StagingEmailSubjectText");
                key.Value = txtStagingSubject.Text;
                elem.SetAttributeNode(key);

                key = doc.CreateAttribute("SendEmail");
                key.Value = rbListEmailSetting.SelectedValue;
                elem.SetAttributeNode(key);

                doc.DocumentElement.AppendChild(elem);
                doc.Save(PATH);

                lblMessage.Text = "Settings save successfully!";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Could not save settings! Error: " + ex.Message;
            }
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
                    if (node.Name == "KeySettings")
                    {
                        txtStagingSubject.Text = Convert.ToString(node.Attributes["StagingEmailSubjectText"].Value);
                        rbListEmailSetting.SelectedValue = Convert.ToString(node.Attributes["SendEmail"].Value);
                    }
                }
            }
        }
    }
}