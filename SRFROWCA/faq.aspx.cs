using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;

namespace SRFROWCA
{
    public partial class faq : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        internal override void BindGridData()
        {
            if (RC.SelectedSiteLanguageId == 2)
            {
                linkAccountAns1.HRef = "~/HelpFiles/HelpDocs/HelpFrench/2_ORS_Resgistration_FR_v3.pdf";
                linkAccountAns2.HRef = "~/HelpFiles/HelpDocs/HelpFrench/6_ORS_Language_FR_v3.pdf";
                linkAccountAns3.HRef = "~/HelpFiles/HelpDocs/HelpFrench/3_ORS_Role_EN_v3.pdf";
                linkAccountAns4.HRef = "~/HelpFiles/HelpDocs/HelpFrench/4_ORS_Reporting_FR_v3.pdf";
                linkAccountAns5.HRef = "~/HelpFiles/HelpDocs/HelpFrench/5_ORS_Reports_FR_v3.pdf";
                linkAccountAns6.HRef = "~/HelpFiles/HelpDocs/HelpFrench/7_ORS_ClusterLead_FR_v3.pdf";
            }
            if (RC.SelectedSiteLanguageId == 1)
            {
                linkAccountAns1.HRef = "~/HelpFiles/HelpDocs/HelpEng/2_ORS_Resgistration_EN_v3.pdf";
                linkAccountAns2.HRef = "~/HelpFiles/HelpDocs/HelpEng/6_ORS_Language_EN_v3.pdf";
                linkAccountAns3.HRef = "~/HelpFiles/HelpDocs/HelpEng/3_ORS_Role_EN_v3.pdf";
                linkAccountAns4.HRef = "~/HelpFiles/HelpDocs/HelpEng/4_ORS_Reporting_EN_v3.pdf";
                linkAccountAns5.HRef = "~/HelpFiles/HelpDocs/HelpEng/5_ORS_Reports_EN_v3.pdf";
                linkAccountAns6.HRef = "~/HelpFiles/HelpDocs/HelpEng/7_ORS_ClusterLead_EN_v3.pdf";
            }
        }
    }
}