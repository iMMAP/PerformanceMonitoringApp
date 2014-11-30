using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class LinkEmergencyObjective : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                PopulateEmergencies();
                PopulateObjectives();
                FillEmergencyObjectives();
            }
        }

        private void PopulateEmergencies()
        {
            int languageId = (int)RC.SelectedSiteLanguageId;
            UI.FillEmergency(ddlEmergencies, RC.GetAllEmergencies(languageId));
        }

        private void PopulateObjectives()
        {
            cblObjectives.DataValueField = "ObjectiveID";
            cblObjectives.DataTextField = "ShortObjectiveTitle";

            cblObjectives.DataSource = DBContext.GetData("GetAllObjectives", new object[] { RC.SelectedSiteLanguageId});
            cblObjectives.DataBind();
        }

        private void FillEmergencyObjectives()
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                DataTable dt = GetEmergencyObjectives(emergencyId);
                CheckClusterListBox(dt);
            }
        }

        internal override void BindGridData()
        {
            PopulateEmergencies();
            PopulateObjectives();
            FillEmergencyObjectives();
        }

        private DataTable GetEmergencyObjectives(int emergencyId)
        {
            return DBContext.GetData("GetEmgObjectives", new object[] { emergencyId, null, RC.SelectedSiteLanguageId });
        }

        private void CheckClusterListBox(DataTable dt)
        {
            foreach (ListItem item in cblObjectives.Items)
                item.Selected = false;

            foreach (ListItem item in cblObjectives.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ObjectiveId"].ToString().Equals(item.Value))
                        item.Selected = true;
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
                
                foreach (ListItem item in cblObjectives.Items)
                {
                    if (item.Selected)
                        SaveItem(emergencyId, item.Value);
                    else
                    {
                        if (!DeleteItem(emergencyId, item.Value))
                            notDeletedItems.Add(item.Text);
                    }
                }

                if (notDeletedItems.Count > 0)
                {
                    string msg = "Your data is saved but these items are being used and can't be removed from the selected emergency: ";
                    lblMessage.Text = msg + string.Join(", ", notDeletedItems.ToArray());
                    lblMessage.Visible = true;
                }
                else
                {
                    lblMessage.CssClass = "info-message";
                    lblMessage.Text = "Saved successfully!";
                    lblMessage.Visible = true;
                }
            }
        }

        private bool DeleteItem(int emergencyId, string itemValue)
        {
            int returnVal = DBContext.Delete("uspDeleteObjectiveFromEmergency", new object[] { emergencyId, Convert.ToInt32(itemValue), DBNull.Value });
            
            return returnVal == 0;
        }

        private void SaveItem(int emergencyId, string itemValue)
        {
            Guid userId = RC.GetCurrentUserId;
            DBContext.Add("uspInsertEmergencyObjective", new object[] { emergencyId, null, null, userId, null, null, Convert.ToInt32(itemValue) });
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillEmergencyObjectives();
        }
    }
}