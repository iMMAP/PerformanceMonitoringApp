using SRFROWCA.Common;
using SRFROWCA.Configurations;
using System;
using System.Linq;
using System.Web.UI;

namespace SRFROWCA.OrsProject
{
    public partial class ProjectPartners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddTargetControl();
        }

        private void AddTargetControl()
        {
            int projectId = 0;
            if (Request.QueryString["pid"] != null)
            {
                int.TryParse(Request.QueryString["pid"].ToString(), out projectId);
            }

            Project project = null;
            using(ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == projectId);
            }

            if (project == null)
                return;

            string key = project.EmergencyLocationId.ToString() + project.EmergencyClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.LocationTypes.Governorate)
                {
                    ctl = (ctlORSAdmin1Partners)LoadControl("~/OrsProject/ctlORSAdmin1Partners.ascx");
                    ((ctlORSAdmin1Partners)ctl).ID = "ORSPartnerControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.District)
                {
                    ctl = (ctlORSAdmin2Targets)LoadControl("~/OrsProject/ctlORSAdmin2Targets.ascx");
                    ((ctlORSAdmin2Targets)ctl).ID = "ORSTargetControl";
                }

                if (ctl != null)
                    pnlTargets.Controls.Add(ctl);
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}