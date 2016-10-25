using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Anonymous
{
    public partial class ExpClusterFramework : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                RC.SetFiltersFromSessionCluster(ddlCountryExport, ddlClusterExport, Session);
                SetExportFile();
            }
        }

        private bool IsMissingTarget()
        {
            int emgLocId = RC.GetSelectedIntVal(ddlCountryExport);
            int emgClusterId = RC.GetSelectedIntVal(ddlClusterExport);

            int indMissingTarget = DBContext.Update("GetIsFrameworkComplete", new object[] { emgLocId, emgClusterId, (int)RC.Year._2016, DBNull.Value });
            return indMissingTarget <= 0;
        }

        internal override void BindGridData()
        {
            PopulateDropDowns();
            SetExportFile();
        }

        private void PopulateDropDowns()
        {
            LoadCountires();
            LoadClusters();

            if (UserInfo.EmergencyCountry > 0)
            {
                ddlCountryExport.SelectedValue = UserInfo.EmergencyCountry.ToString();
                //if (!RC.IsRegionalClusterLead(this.User))
                //{
                //    ddlCountryExport.Enabled = false;
                //    ddlCountryExport.BackColor = Color.LightGray;
                //}
            }

            if (UserInfo.EmergencyCluster > 0)
            {
                ddlClusterExport.SelectedValue = UserInfo.EmergencyCluster.ToString();
                //ddlClusterExport.Enabled = false;
                //ddlClusterExport.BackColor = Color.LightGray;
            }
        }
        private void LoadCountires()
        {
            UI.FillEmergencyLocations(ddlCountryExport, RC.EmergencySahel2015);
            ListItem item = new ListItem("Select", "0");

        }
        private void LoadClusters()
        {
            UI.FillEmergnecyClusters(ddlClusterExport, RC.EmergencySahel2015);
        }
        private void SetExportFile()
        {
            //string countryId = "0" ;
            //string clusterId = "0";

            //if (!IsMissingTarget())
            //{
            //    countryId = ddlCountryExport.SelectedValue;
            //clusterId = ddlClusterExport.SelectedValue;

            //}

            string countryId = ddlCountryExport.SelectedValue;
            string clusterId = ddlClusterExport.SelectedValue;
            menueExportWord.HRef = string.Format("../Reports/DownloadReport.aspx?type=10&emgCountryId={0}&emgClusterId={1}", countryId, clusterId);
        }

        protected void ddlCountryExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetExportFile();
        }

        protected void ddlClusterExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetExportFile();
        }

        

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}