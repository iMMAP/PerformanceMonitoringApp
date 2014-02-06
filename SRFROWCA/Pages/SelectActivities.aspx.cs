using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class SelectActivities : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string languageChange = "";
            if (Session["SiteChanged"] != null)
            {
                languageChange = Session["SiteChanged"].ToString();
            }
            //Session["SiteLanguage"]
            if (IsPostBack && string.IsNullOrEmpty(languageChange)) return;

            PopulateDropDowns();
            LoadClusters();
            Session["SiteChanged"] = null;
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
        }

        #region Events.

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void gvClusters_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType != DataControlRowType.DataRow) return;

            //if (e.Row.RowIndex == 0)
            //{
            //    CollapsiblePanelExtender cpe = e.Row.FindControl("cpeExpandCollapseActivities") as CollapsiblePanelExtender;
            //    if (cpe != null)
            //    {
            //        cpe.Collapsed = false;
            //    }
            //}
        }

        protected void gvActivities_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Get row of the image clicked
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);

            if (e.CommandName == "AddActivity")
            {
                AddActivity(row);
                ShowMessage("Activity Added In Your List.");
            }

            if (e.CommandName == "RemoveActivity")
            {
                RemoveActivity(row);
                ShowMessage("Activity Removed From Your List.");
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

            ShowMessage("All Selected Activities Added In Your List.");
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

            ShowMessage("All Selected Activities Removed From Your List.");
        }

        #endregion

        // Populate Emregency DDL
        private void PopulateLocationEmergencies(int locationId)
        {
            ddlEmergency.DataValueField = "EmergencyId";
            ddlEmergency.DataTextField = "EmergencyName";
            ddlEmergency.DataSource = GetLocationEmergencies(locationId);
            ddlEmergency.DataBind();
            if (ddlEmergency.Items.Count > 1)
            {
                ListItem item = new ListItem("Select Emergency", "0");
                ddlEmergency.Items.Insert(0, item);
            }
            else
            {
                divEmergency.Visible = false;
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
            return DBContext.GetData("GetEmergencyOnLocation", new object[] { locationId, ROWCACommon.SelectedSiteLanguageId });
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

            if (emgLocationId > 0)
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
                    if (lblActivityId != null && lblActivityId.Text == dr["PriorityActivityId"].ToString())
                    {
                        ChangeImageInfo(row, "Add");
                    }
                }
            }
        }

        // Get all activities.
        private object GetActivities(int clusterId)
        {
            return DBContext.GetData("GetEmergencyClusterActivities", new object[] { clusterId, ROWCACommon.SelectedSiteLanguageId });
        }

        // Get only current (loggedin) user's activities to select from all activities.
        private DataTable GetUserActivities()
        {
            int emgLocationId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocationId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            if (emgLocationId > 0)
            {
                return DBContext.GetData("GetIPActivities", new object[] { emgLocationId, userId });
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
        private void DeleteActivity(int activityId)
        {
            int emgLocId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            DBContext.Delete("DeleteActivityData", new object[] { emgLocId, activityId, userId, DBNull.Value });
        }
        private void SaveActivity(int activityId)
        {
            int emgLocId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emgLocId);

            Guid userId = ROWCACommon.GetCurrentUserId();
            DBContext.Add("InsertActivityData", new object[] { emgLocId, activityId, userId, DBNull.Value });
        }

        private DataTable GetEmergencyClusters(int emgId)
        {
            return DBContext.GetData("GetEmergencyClustersOfData", new object[] { emgId, ROWCACommon.SelectedSiteLanguageId });
        }

        private void ShowMessage(string message, ROWCACommon.NotificationType notificationType = ROWCACommon.NotificationType.Success)
        {
            updMessage.Update();
            ROWCACommon.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            ShowMessage("<b>Some Error Occoured. Admin Has Notified About It</b>.<br/> Please Try Again.", ROWCACommon.NotificationType.Error);

            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "SelectActivities", this.User);
        }
    }
}