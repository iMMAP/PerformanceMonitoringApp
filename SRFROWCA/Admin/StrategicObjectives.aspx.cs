using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.Admin
{
    public partial class StrategicObjectives : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            this.Form.DefaultButton = this.btnAdd.UniqueID;

            PopulateEmergencies();
            LoadStrategicObjectives();
        }

        private void PopulateEmergencies()
        {
            
            //UI.FillEmergency(ddlEmergencies, ROWCACommon.GetAllEmergencies((int)ROWCACommon.SiteLanguage.English));
        }

        private void LoadStrategicObjectives()
        {
            int? languageId = null; // (int)ROWCACommon.SiteLanguage.English;
            gvStrategicObjective.DataSource = ROWCACommon.GetStrategicObjectives(this.User, languageId);
            gvStrategicObjective.DataBind();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvStrategicObjective_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this organization?')");
                }
            }
        }

        protected void gvStrategicObjective_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //// If user click on Delete button.
            //if (e.CommandName == "DeleteOrg")
            //{
            //    int locEmgId = Convert.ToInt32(e.CommandArgument);

            //    // Check if any IP has reported on this project. If so then do not delete it.
            //    if (AnyIPReportedOnEmg(locEmgId))
            //    {
            //        lblMessage.Text = "Organization cannot be deleted! It is being used Offices and/or Reports.";
            //        lblMessage.CssClass = "error-message";
            //        lblMessage.Visible = true;

            //        return;
            //    }

            //    DeleteEmergencies(locEmgId);
            //    LoadEmergencies();
            //}

            // Edit Project.
            if (e.CommandName == "EditObjective")
            {
                hfPKId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblLocationEmergencyId = row.FindControl("lblLocationEmergencyId") as Label;

                Label lblEmergencyClusterId = row.FindControl("lblEmergencyClusterId") as Label;
                if (lblEmergencyClusterId != null)
                {
                    //ddlEmgClusters.SelectedValue = lblEmergencyClusterId.Text;
                }

                Label lblObjective = row.FindControl("lblObjective") as Label;
                if (lblObjective != null)
                {
                    txtObj.Text = lblObjective.Text;
                }
            }
        }

        private void PopulateEmergencyClusters(int emregencyId)
        {
            //ddlEmgClusters.DataValueField = "EmergencyClusterId";
            //ddlEmgClusters.DataTextField = "ClusterName";

            //ddlEmgClusters.DataSource = GetEmergencyClusters(emregencyId);
            //ddlEmgClusters.DataBind();

            //ListItem item = new ListItem("Select Cluster", "0");
            //ddlEmgClusters.Items.Insert(0, item);
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int emergencyId = 0;
            //int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            //if (emergencyId > 0)
            //{
            //    PopulateEmergencyClusters(emergencyId);
            //}
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveObjective();
            LoadStrategicObjectives();
            ClearPopupControls();
        }

        private void SaveObjective()
        {
            int emgClusterId = 0;
            //int.TryParse(ddlEmgClusters.SelectedValue, out emgClusterId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            string objectiveName = txtObj.Text.Trim();
            Server.HtmlEncode(objectiveName);
            if (!string.IsNullOrEmpty(hfPKId.Value))
            {
                int pkId = Convert.ToInt32(hfPKId.Value);
                DBContext.Update("UpdateStrategicObjective", new object[] { pkId, emgClusterId, objectiveName, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertStrategicObjective", new object[] { emgClusterId, objectiveName, userId, DBNull.Value });
            }
        }

        private void ClearPopupControls()
        {
            txtObj.Text = "";
        }
    }
}