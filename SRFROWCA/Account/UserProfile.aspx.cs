using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class UserProfile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnUpdate.UniqueID;

            PopulateCountries();
            GetUserInformation();

        }

        private void GetUserInformation()
        {
            Guid userId = RC.GetCurrentUserId;
            MembershipUser mu = Membership.GetUser(userId);
            txtUserName.Text = mu.UserName;
            txtEmail.Text = mu.Email;

            DataTable dt = GetUserDetails(userId);
            if (dt.Rows.Count > 0)
            {
                txtPhone.Text = dt.Rows[0]["PhoneNumber"].ToString();

                string[] roles = Roles.GetRolesForUser(mu.UserName);

                ddlCountry.SelectedValue = dt.Rows[0]["LocationId"].ToString();
            }
        }

        private DataTable GetUserDetails(Guid userId)
        {
            return RC.GetUserDetails();
        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            DataTable dt = GetCountries();
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)RC.LocationTypes.National;
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userId = RC.GetCurrentUserId;
                MembershipUser mu = Membership.GetUser(userId);
                mu.Email = txtEmail.Text;
                Membership.UpdateUser(mu);
                object[] userValues = GetUserValues(userId);
                DBContext.Add("UpdateASPNetUserInfoWithoutOrg", userValues);
                ShowMessage("You profile updated successfully!");
            }
            catch
            {
                ShowMessage("Some error occoured while updating your profile. Please contact to Admin of the site.",
                    RC.NotificationType.Error);
            }

        }

        private object[] GetUserValues(Guid userId)
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            return new object[] { userId, countryId, phone, DBNull.Value };
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UpdateUser", this.User);
        }
    }
}