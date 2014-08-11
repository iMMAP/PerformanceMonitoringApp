using BusinessLogic.Projects;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                txtStagingSubject.Text = Convert.ToString(ConfigurationManager.AppSettings["StagingEmailSubjectText"]);
                rbListEmailSetting.SelectedValue = Convert.ToString(ConfigurationManager.AppSettings["SendEmail"]);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var webConfig = WebConfigurationManager.OpenWebConfiguration("~");
                var webSection = (AppSettingsSection)webConfig.GetSection("appSettings");
                webSection.Settings["StagingEmailSubjectText"].Value = txtStagingSubject.Text;
                webConfig.Save();

                webSection.Settings["SendEmail"].Value = rbListEmailSetting.SelectedValue;
                webConfig.Save();

                lblMessage.Text = "Settings save successfully";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Could not save settings. Error: " + ex.Message;
            }
           

        }
    }
}