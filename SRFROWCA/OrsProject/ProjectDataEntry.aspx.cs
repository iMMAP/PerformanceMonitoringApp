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

            string key = project.EmergencyLocationId.ToString() + project.EmergencyClusterId.ToString();
            AdminTargetSettingItems items = RC.AdminTargetSettings(key);
            if (items.IsTarget)
            {
                UserControl ctl = null;
                if (items.AdminLevel == RC.LocationTypes.Governorate)
                {
                    ctl = (ctlORSAdmin1Report)LoadControl("~/OrsProject/ctlORSAdmin1Report.ascx");
                    ((ctlORSAdmin1Report)ctl).ID = "ORSReportControl";
                }
                else if (items.AdminLevel == RC.LocationTypes.District)
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