using SRFROWCA.Common;
using SRFROWCA.Configurations;
using System;
using System.Linq;
using System.Web.UI;

namespace SRFROWCA.OrsProject
{
    public partial class ProjectDataEntry : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AddTargetControl();
        }

        internal override void BindGridData()
        { }
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

            AdminTargetSettingItems items = RC.AdminTargetSettings(project.EmergencyLocationId, project.EmergencyClusterId, project.Year);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.AdminLevels.Admin1)
                {
                    ctl = (ctlORSAdmin1Report)LoadControl("~/OrsProject/ctlORSAdmin1Report.ascx");
                    ((ctlORSAdmin1Report)ctl).ID = "ORSReportControl";
                }
                else if (items.AdminLevel == RC.AdminLevels.Admin2)
                {
                    ctl = (ctlORSAdmin2Report)LoadControl("~/OrsProject/ctlORSAdmin2Report.ascx");
                    ((ctlORSAdmin2Report)ctl).ID = "ORSReportControl";
                }

                if (ctl != null)
                    pnlReport.Controls.Add(ctl);
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}