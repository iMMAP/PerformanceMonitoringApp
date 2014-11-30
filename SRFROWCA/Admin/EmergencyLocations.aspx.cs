using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class EmergencyLocations : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnSave.UniqueID;
            PopulateEmergencies();
            PopulateCountries();
            FillEmergencyLocations();
        }

        private void PopulateEmergencies()
        {
            int locationId = (int)RC.SelectedSiteLanguageId;
            UI.FillEmergency(ddlEmergencies, RC.GetAllEmergencies(locationId));
        }

        private void PopulateCountries()
        {
            UI.FillLocations(cblLocations, RC.GetLocations(this.User, (int)RC.LocationTypes.National));
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillEmergencyLocations();
        }

        private void FillEmergencyLocations()
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                DataTable dt = RC.GetEmergencyLocations(emergencyId);
                CheckLocationsListBox(dt);
            }
        }

        internal override void BindGridData()
        {
            PopulateEmergencies();
            PopulateCountries();
            FillEmergencyLocations();
        }

        private void CheckLocationsListBox(DataTable dt)
        {
            foreach (ListItem item in cblLocations.Items)
            {
                item.Selected = false;
            }

            foreach (ListItem item in cblLocations.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["LocationId"].ToString().Equals(item.Value))
                    {
                        item.Selected = true;                        
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
                foreach (ListItem item in cblLocations.Items)
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
                    string msg = "Your data is saved but these items are being used and can't be removed: ";
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
            int returnVal = DBContext.Delete("DeleteEmergencyLocation",
                                                new object[] { emergencyId, Convert.ToInt32(itemValue), DBNull.Value });
            // == 0 means deleted successfully
            return returnVal == 0;
        }

        private void SaveItem(int emergencyId, string itemValue)
        {
            Guid userId = RC.GetCurrentUserId;
            DBContext.Add("InsertEmergencyLocation", new object[] { emergencyId, Convert.ToInt32(itemValue), userId, DBNull.Value });
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "EmergencyLocations", this.User);
        }
    }
}