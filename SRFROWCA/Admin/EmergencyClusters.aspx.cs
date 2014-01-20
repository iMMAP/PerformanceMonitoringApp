using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Collections.Generic;

namespace SRFROWCA.Admin
{
    public partial class EmergencyClusters : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnSave.UniqueID;

            PopulateEmergencies();
            PopulateClusters();
        }

        private void PopulateEmergencies()
        {
            UI.FillLocationEmergency(ddlEmergencies, ROWCACommon.GetAllEmergencies());
        }

        private void PopulateClusters()
        {
            UI.FillClusters(cblClusters);
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                FillEmergencyClusters(emergencyId);
            }
        }

        private void FillEmergencyClusters(int emergencyId)
        {
            DataTable dt = GetEmergencyClusters(emergencyId);
            CheckClusterListBox(dt);
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
        }

        private void CheckClusterListBox(DataTable dt)
        {
            foreach (ListItem item in cblClusters.Items)
            {
                item.Selected = false;
            }

            foreach (ListItem item in cblClusters.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ClusterId"].ToString().Equals(item.Value))
                    {
                        item.Selected = true;
                        //item.Enabled = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                List<string> notDeletedItems = new List<string>();
                foreach (ListItem item in cblClusters.Items)
                {
                    if (item.Selected)
                    {
                        SaveItem(emergencyId, item.Value);
                    }
                    else
                    {
                        // If not deleted then add in list to show use message.
                        // that whihc items can't be deleted
                        if (!DeleteItem(emergencyId, item.Value))
                        {
                            notDeletedItems.Add(item.Text);
                        }
                    }
                }

                if (notDeletedItems.Count > 0)
                {
                    string msg = "You adata is saved but these items are being used and can't be removed: ";
                    lblMessage.Text = msg + string.Join(", ", notDeletedItems.ToArray());
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.CssClass = "info-message";
                    lblMessage.Text = "Saved!";
                    lblMessage.Visible = true;
                }
            }
        }

        private bool DeleteItem(int emergencyId, string itemValue)
        {
            int returnVal = DBContext.Delete("DeleteClusterFromEmergency", 
                                                new object[] { emergencyId, Convert.ToInt32(itemValue), DBNull.Value });
            // == 0 means deleted successfully
            return returnVal == 0;
        }

        private void SaveItem(int emergencyId, string itemValue)
        {
            Guid userId = ROWCACommon.GetCurrentUserId();
            DBContext.Add("InsertEmergencyClusters", new object[] { emergencyId, Convert.ToInt32(itemValue), userId, DBNull.Value });
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "EmergencyClusters", this.User);
        }
    }
}