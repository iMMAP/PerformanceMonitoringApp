using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class SelectActivities : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            
            PopulateDropDowns();
            LoadClusters();
        }

        private void PopulateDropDowns()
        {
            DataTable dt = GetUserDetails();
            int locationId = 0;
            int orgId = 0;
            if (dt.Rows.Count > 0)
            {
                locationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                orgId = Convert.ToInt32(dt.Rows[0]["OrganizationId"].ToString());
            }

            PopulateLocationEmergencies(locationId);
            PopulateOffices(locationId, orgId);
        }

        #region Events.

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusters();
        }
        protected void ddlOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusters();
        }

        protected void gvClusters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvActivities = e.Row.FindControl("gvActivities") as GridView;
            if (gvActivities != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int clusterId = 0;
                int.TryParse(dr["EmergencyClusterId"].ToString(), out clusterId);

                // Get all activities and bind grid.
                gvActivities.DataSource = GetActivities(clusterId);
                gvActivities.DataBind();

                // Attch row command event with grid.
                gvActivities.RowCommand += gvActivities_RowCommand;
            }
        }

        protected void gvActivities_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Get row of the image clicked
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

            if (e.CommandName == "AddActivity")
            {
                AddActivity(row);
                ShowMessage("Saved Successfully!", "info-message");
            }

            if (e.CommandName == "RemoveActivity")
            {
                RemoveActivity(row);
                ShowMessage("Saved Successfully!", "info-message");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow parentGridRow in gvClusters.Rows)
            {
                // Find child grid.
                GridView gvActivities = parentGridRow.FindControl("gvActivities") as GridView;
                if (gvActivities == null) return;

                foreach (GridViewRow row in gvActivities.Rows)
                {
                    CheckBox cbActivity = row.FindControl("chkActivitySelect") as CheckBox;
                    AddActivity(row, cbActivity);
                }
            }

            ShowMessage("Saved Successfully!", "info-message");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow parentGridRow in gvClusters.Rows)
            {
                GridView gvActivities = parentGridRow.FindControl("gvActivities") as GridView;
                if (gvActivities == null) return;

                foreach (GridViewRow row in gvActivities.Rows)
                {
                    CheckBox cbActivity = row.FindControl("chkActivitySelect") as CheckBox;
                    RemoveActivity(row, cbActivity);
                }
            }

            ShowMessage("Saved Successfully!", "info-message");
        }

        #endregion

        // Populate Emregency DDL
        private void PopulateLocationEmergencies(int locationId)
        {
            ddlEmergency.DataValueField = "LocationEmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";
            ddlEmergency.DataSource = GetLocationEmergencies(locationId);
            ddlEmergency.DataBind();
            if (ddlEmergency.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddlEmergency.Items.Insert(0, item);
            }
        }
        private DataTable GetLocationEmergencies(int locationId)
        {
            // If admin then return all emergencies
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                return DBContext.GetData("GetAllLocationEmergencies");
            }

            // If not admin then return emergencies of user's locations.
            return DBContext.GetData("GetLocationEmergencies", new object[] { locationId });
        }

        // Populate Office DDL
        private void PopulateOffices(int locationId, int organizationId)
        {
            ddlOffice.DataValueField = "OfficeId";
            ddlOffice.DataTextField = "OfficeName";

            ddlOffice.DataSource = GetOffices(locationId, organizationId);
            ddlOffice.DataBind();

            if (ddlOffice.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Your Office", "0");
                ddlOffice.Items.Insert(0, item);
            }
        }
        private DataTable GetOffices(int locationId, int organizationId)
        {
            // If admin then return all offices
            if (HttpContext.Current.User.IsInRole("Admin"))
            {
                return DBContext.GetData("GetAllOffices");
            }

            // If not admin then return only user's office.
            return DBContext.GetData("GetOrganizationOffices", new object[] { locationId, organizationId });
        }

        // Populate Clusters Grid.
        private void LoadClusters()
        {
            PopulateClusters();
            PreSelectAlreadyAddedActivities();
        }

        private void PopulateClusters()
        {
            DataTable dt = new DataTable();
            int emgLocationId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocationId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            if (emgLocationId > 0 && officeId > 0)
            {
                dt = GetEmergencyClusters(emgLocationId);
            }

            gvClusters.DataSource = dt;
            gvClusters.DataBind();
        }

        // Select: Use 'X' img and RemoveActivity Command Argument.
        private void PreSelectAlreadyAddedActivities()
        {
            DataTable dt = GetUserActivities();
            foreach (DataRow dr in dt.Rows)
            {
                SelectActivitiesInGrid(dr);
            }
        }

        private void SelectActivitiesInGrid(DataRow dr)
        {
            foreach (GridViewRow parentGridRow in gvClusters.Rows)
            {
                GridView gvActivities = parentGridRow.FindControl("gvActivities") as GridView;
                if (gvActivities == null) return;

                foreach (GridViewRow row in gvActivities.Rows)
                {
                    Label lblActivityId = row.FindControl("lblActivityId") as Label;
                    if (lblActivityId != null && lblActivityId.Text == dr["IndicatorActivityId"].ToString())
                    {
                        ChangeImageInfo(row, "Add");
                    }
                }
            }
        }

        // Get all activities.
        private object GetActivities(int clusterId)
        {
            return DBContext.GetData("GetEmergencyClusterActivities", new object[] { clusterId });
        }

        // Get only current (loggedin) user's activities to select from all activities.
        private DataTable GetUserActivities()
        {
            int emgLocationId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocationId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            if (emgLocationId > 0 && officeId > 0)
            {
                return DBContext.GetData("GetIPActivities", new object[] { emgLocationId, officeId, userId });
            }

            return new DataTable();
        }
        private DataTable GetUserDetails()
        {
            Guid userGuid = ROWCACommon.GetCurrentUserId();
            return DBContext.GetData("GetUserDetails", new object[] { userGuid });
        }
        private void AddActivity(GridViewRow row)
        {
            // ** Temporary checkbox **
            // When user will click on image button 
            // we will pass check box to AddActivity to make the condition true.
            CheckBox cb = new CheckBox();
            cb.Checked = true;
            AddActivity(row, cb);
        }
        private void AddActivity(GridViewRow row, CheckBox cbActivity)
        {
            // Find activity id label in grid.
            Label lblActivityId = row.FindControl("lblActivityId") as Label;
            if (lblActivityId != null && cbActivity != null && cbActivity.Checked)
            {
                int activityId = Convert.ToInt32(lblActivityId.Text);
                SaveActivity(activityId);
                ChangeImageInfo(row, "Add");
                cbActivity.Checked = false;
            }
        }
        private void RemoveActivity(GridViewRow row)
        {
            CheckBox cb = new CheckBox();
            cb.Checked = true;
            RemoveActivity(row, cb);
        }
        private void RemoveActivity(GridViewRow row, CheckBox cbActivity)
        {
            Label lblActivityId = row.FindControl("lblActivityId") as Label;
            if (lblActivityId != null && cbActivity != null && cbActivity.Checked)
            {
                int activityId = Convert.ToInt32(lblActivityId.Text);
                DeleteActivity(activityId);
                ChangeImageInfo(row, "Remove");
                cbActivity.Checked = false;
            }
        }
        private void ChangeImageInfo(GridViewRow row, string action)
        {
            // Get imagebutton from grid.
            // Change image and command name on current action i.e. Add or Remove
            ImageButton btnImg = row.FindControl("ibtnAdd") as ImageButton;
            if (btnImg != null)
            {
                if (action.Equals("Remove"))
                {
                    btnImg.ImageUrl = "~/images/add_plus.png";
                    btnImg.CommandName = "AddActivity";
                }
                else if (action.Equals("Add"))
                {
                    btnImg.ImageUrl = "~/images/error.png";
                    btnImg.CommandName = "RemoveActivity";
                }
            }
        }
        private void DeleteActivity(int dataId)
        {
            int emgLocId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);
            Guid userId = ROWCACommon.GetCurrentUserId();

            DBContext.Delete("DeleteActivityData", new object[] { emgLocId, officeId, dataId, userId, DBNull.Value });
        }
        private void SaveActivity(int activityId)
        {
            int emgLocId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocId);

            int officeId = 0;
            int.TryParse(ddlOffice.SelectedValue, out officeId);
            Guid userId = ROWCACommon.GetCurrentUserId();

            DBContext.Add("InsertActivityData", new object[] { emgLocId, officeId, activityId, userId, DBNull.Value });
        }
        private DataTable GetEmergencyClusters(int emgLocationId)
        {
            return DBContext.GetData("GetEmergencyClustersOfData", new object[] { emgLocationId });
        }
        private void ShowMessage(string message, string css)
        {
            lblMessage.CssClass = css;
            lblMessage.Text = message;
            lblMessage.Visible = true;
            updMessage.Update();
            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), UniqueID, "$('#" + lblMessage.ClientID.ToString() + "').fadeOut(3000, function() {});", true);
            return;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "SelectActivities", this.User);
        }
    }
}