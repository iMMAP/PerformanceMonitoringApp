using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class UploadSRP : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //PopulateDropDowns();
            }
        }

        private void PopulateDropDowns()
        {
            LoadCountires();
            LoadClusters();
        }
        private void LoadCountires()
        {
            int emergencyId = UserInfo.Emergency;
            UI.FillEmergencyLocations(ddlCountry, emergencyId);

            if (ddlCountry.Items.Count > 0)
            {
                ListItem item = new ListItem("Select", "0");
                ddlCountry.Items.Insert(0, item);
            }
        }
        private void LoadClusters()
        {
            int emergencyId = UserInfo.Emergency;
            UI.FillEmergnecyClusters(ddlCluster, emergencyId);

            if (ddlCountry.Items.Count > 0)
            {
                ListItem item = new ListItem("Select", "0");
                ddlCountry.Items.Insert(0, item);
            }
        }

        #region Upload

        private bool IsValidFile()
        {
            if (fuSRP.HasFile)
            {
                string fileExt = Path.GetExtension(fuSRP.PostedFile.FileName);
                if (fileExt != ".xls" && fileExt != ".xlsx" && fileExt != ".xlsb")
                {
                    ShowMessage("Pleae use Excel files with 'xls' OR 'xlsx' extentions.");
                    return false;
                }
            }
            else
            {
                ShowMessage("Please select file to upload!");
                return false;
            }

            return true;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UploadActivities", this.User);
        }

        #endregion
    }
}