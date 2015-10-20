using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class SahelIndicatorTargets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadCombos();
            DisableDropDowns();
            LoadSahelIndicators();
        }

        private void LoadSahelIndicators()
        {
            gvSahelIndTargets.DataSource = GetSahelIndicators();
            gvSahelIndTargets.DataBind();
        }        

        private void LoadCombos()
        {
            int emergencyId = RC.EmergencySahel2015;
            UI.FillEmergencyLocations(ddlCountry, emergencyId);
            UI.FillEmergnecyClusters(ddlCluster, emergencyId);
            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "0"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "0"));
            SetComboValues();
        }

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private DataTable GetSahelIndicators()
        {
            int val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val > 0 ? val : (int?)null;
            val = 0;

            val = RC.GetSelectedIntVal(ddlCountry);
            int? emgCountryId = val > 0 ? val : (int?)null;

            return DBContext.GetData("GetSahelIndicatorsWithTargets", new object[] {RC.EmergencySahel2015,  emgClusterId, emgCountryId, RC.SelectedSiteLanguageId });            
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSahelIndicators();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveTargets();
            ShowMessage("Targets Saved Successfully");
        }

        private void SaveTargets()
        {
            foreach (GridViewRow row in gvSahelIndTargets.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int clusterIndicatorId = 0;
                    int.TryParse(gvSahelIndTargets.DataKeys[row.RowIndex].Values["ClusterIndicatorId"].ToString(), out clusterIndicatorId);
                    if (clusterIndicatorId > 0)
                    {
                        int? target = GetIndicatorTarget(row);
                        SaveIndicatorTarget(clusterIndicatorId, target);
                    }
                }
            }
        }

        private void SaveIndicatorTarget(int clusterIndicatorId, int? target)
        {
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            DBContext.Add("InsertSahelIndicatorCountryTarget", new object[] { clusterIndicatorId, emgLocationId, target, RC.GetCurrentUserId, DBNull.Value });
        }

        private int? GetIndicatorTarget(GridViewRow row)
        {
            int? target = null;
            TextBox txtTarget = row.FindControl("txtTarget") as TextBox;
            if (txtTarget != null)
            {
                if (!string.IsNullOrEmpty(txtTarget.Text.Trim()))
                {
                    target = Convert.ToInt32(txtTarget.Text.Trim());
                }
            }

            return target;
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