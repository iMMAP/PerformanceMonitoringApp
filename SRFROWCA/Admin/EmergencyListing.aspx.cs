using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Admin
{
    public partial class EmergencyListing : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnAdd.UniqueID;

            LoadEmergencies();
            PopulateDisasterTypes();
        }

        // Add delete confirmation message with all delete buttons.
        protected void gvEmergency_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Emergency?')");
                }
            }
        }

        // Execute row commands like Edit, Delete etc. on Grid.
        protected void gvEmergency_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // If user click on Delete button.
            if (e.CommandName == "DeleteEmergency")
            {
                int emgId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (!EmgIsBeingUsed(emgId))
                {
                    ShowMessage("Emergency cannot be deleted! It is being used.", ROWCACommon.NotificationType.Error, false, 500);
                    return;
                }

                DeleteEmergency(emgId);
                LoadEmergencies();
            }

            // Edit Project.
            if (e.CommandName == "EditEmergency")
            {
                ClearPopupControls();

                hfLocEmgId.Value = e.CommandArgument.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                Label lblEmgTypeId = row.FindControl("lblEmergencyTypeId") as Label;
                if (lblEmgTypeId != null)
                {
                    ddlEmgType.SelectedValue = lblEmgTypeId.Text;
                }

                txtEmgNameEng.Text = row.Cells[2].Text;                
                mpeAddOrg.Show();
            }
        }

        private bool EmgIsBeingUsed(int emgId)
        {
            DataTable dt = DBContext.GetData("GetIsEmergencyBeingUsed", new object[] { emgId });
            return !(dt.Rows.Count > 0);
        }

        private void DeleteEmergency(int emgId)
        {
            DBContext.Delete("DeleteEmergency", new object[] { emgId, DBNull.Value });
        }

        protected void gvEmergency_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            int? locationId = null;
            DataTable dt = ROWCACommon.GetAllEmergencies(locationId);
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvEmergency.DataSource = dt;
                gvEmergency.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void LoadEmergencies()
        {
            int? languageId = null;
            gvEmergency.DataSource = ROWCACommon.GetAllEmergencies(languageId);
            gvEmergency.DataBind();
        }

        private void PopulateDisasterTypes()
        {
            ddlEmgType.DataValueField = "EmergencyTypeId";
            ddlEmgType.DataTextField = "EmergencyType";
            ddlEmgType.DataSource = GetDisasterTypes();
            ddlEmgType.DataBind();

            ListItem item = new ListItem("Select Disaster Type", "0");
            ddlEmgType.Items.Insert(0, item);
        }

        private DataTable GetDisasterTypes()
        {
            return DBContext.GetData("GetAllDisasterTypes");
        }

        protected void btnAddEmergency_Click(object sender, EventArgs e)
        {
            ClearPopupControls();
            mpeAddOrg.Show();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SaveEmergency();
            LoadEmergencies();
            mpeAddOrg.Hide();
            ClearPopupControls();
        }

        private void ClearPopupControls()
        {
            if (ddlEmgType.Items.Count > 0)
            {
                ddlEmgType.SelectedIndex = 0;
            }

            hfLocEmgId.Value = txtEmgNameEng.Text = txtEmgNameFr.Text = "";
        }

        private void SaveEmergency()
        {
            int emgTyepId = 0;
            int.TryParse(ddlEmgType.SelectedValue, out emgTyepId);

            int locId = 0;

            string emgNameEng = txtEmgNameEng.Text.Trim();
            string emgNameFr = txtEmgNameFr.Text.Trim();
            Guid userId = ROWCACommon.GetCurrentUserId();

            if (!string.IsNullOrEmpty(hfLocEmgId.Value))
            {
                int emgLocId = Convert.ToInt32(hfLocEmgId.Value);
                DBContext.Update("UpdateEmergency", new object[] { emgLocId, emgTyepId, emgNameEng, emgNameFr, locId, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertEmergency", new object[] { emgTyepId, emgNameEng, emgNameFr, locId, userId, DBNull.Value });
            }
        }

        //private void UpdateEmergency()
        //{
        //    int emgTyepId = 0;
        //    int.TryParse(ddlEmgType.SelectedValue, out emgTyepId);

        //    int locId = 0;
        //    int.TryParse(ddlLocations.SelectedValue, out locId);

        //    string emgName = txtEmgName.Text.Trim();
        //    Guid userId = ROWCACommon.GetCurrentUserId();

        //    if (!string.IsNullOrEmpty(hfLocEmgId.Value))
        //    {
        //        int emgLocId = Convert.ToInt32(hfLocEmgId.Value);
        //        DBContext.Update("UpdateEmergency", new object[] { emgLocId, emgTyepId, emgName, locId, userId, DBNull.Value });
        //    }
        //}

        private void ShowMessage(string message, ROWCACommon.NotificationType notificationType = ROWCACommon.NotificationType.Success, bool fadeOut = true, int animationTime = 0)
        {
            updMessage.Update();
            ROWCACommon.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        enum LocationTypes
        {
            Country = 2,
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "EmergencyListing", this.User);
        }
    }
}