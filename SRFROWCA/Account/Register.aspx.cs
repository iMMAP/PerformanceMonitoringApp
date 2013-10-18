using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BusinessLogic;
using System.Data;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateCountries();
            PopulateOrganizations();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            bool returnValue = false;
            try
            {
                // If user already exists return.
                if (IsUserAlreadyExits()) return;

                // Create New 1)Rider, 2) Places, 3) Route,
                // 4) Send Email to user to verify account.
                returnValue = CreateUserAccount();
                string message = "You have been registered successfully!";
                if (returnValue)
                {
                    try
                    {
                        Mail.SendMail("3wopactivities@gmail.com", "3wopactivities@gmail.com", "New 3W Account Request!", "Dear Adim! Please activitate my account " + txtUserName.Text.Trim());
                        message = "You have been registered successfully, we will verify your credentials and activate your account in few hours!";
                    }
                    catch
                    {
                        message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for inconvenience!";
                    }
                }

                lblMessage.CssClass = "info-message";
                lblMessage.Text = message;
                lblMessage.Visible = true;

                ClearRegistrationControls();
            }
            catch
            {
                lblMessage.Text = "Some Error Occoured. Please check your internet or try again!";
                lblMessage.Visible = true;
            }
        }

        private void ClearRegistrationControls()
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";

            if (ddlOrganization.Items.Count > 0)
            {
                ddlOrganization.SelectedIndex = 0;
            }

            if (ddlCountry.Items.Count > 0)
            {
                ddlCountry.SelectedIndex = 0;
            }

        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = GetCountries();
            ddlCountry.DataBind();

            ListItem item = new ListItem("Select Your Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private DataTable GetCountries()
        {
            int locationType = (int)ROWCACommon.LocationTypes.National;
            DataTable dt = DBContext.GetData("GetLocationOnType", new object[] { locationType });

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate countries drop down.
        private void PopulateOrganizations()
        {

            ddlOrganization.DataValueField = "OrganizationId";
            ddlOrganization.DataTextField = "OrganizationAcronym";

            ddlOrganization.DataSource = GetOrganizations();
            ddlOrganization.DataBind();

            ListItem item = new ListItem("Select Your Organization", "0");
            ddlOrganization.Items.Insert(0, item);
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Check if user (email address) is already in db.
        // If not null then user already exists otherwise not.
        private bool IsUserAlreadyExits()
        {
            MembershipUser membershipUser = Membership.GetUser(txtEmail.Text.Trim());

            if (membershipUser != null)
            {
                lblMessage.Text = string.Format("This email {0} already exists in db!", txtEmail.Text.Trim());
                lblMessage.Visible = true;
                return true;
            }
            else
            {
                membershipUser = Membership.GetUser(txtUserName.Text.Trim());

                if (membershipUser != null)
                {
                    lblMessage.Text = string.Format("Someone already has this, '{0}', username. Try another?!", txtUserName.Text.Trim());
                    lblMessage.Visible = true;
                    return true;
                }
            }

            return false;
        }

        // Create new user, places and route.
        private bool CreateUserAccount()
        {
            MembershipUser user = null;

            // Add user in aspnet membership tables.
            user = AddRecordInMembership();

            AddRecordInRoles((Guid)user.ProviderUserKey);

            // User details are organization and country.
            CreateUserDetails((Guid)user.ProviderUserKey);
            return true;
        }

        private void AddRecordInRoles(Guid userId)
        {
            DBContext.Add("InsertUserInRole", new object[] { userId, DBNull.Value });
        }

        // Add new user in db.
        private MembershipUser AddRecordInMembership()
        {
            // Adds a new user to the data store.
            MembershipUser user = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());
            user.IsApproved = false;

            // Updates the database with the information for the specified user.
            Membership.UpdateUser(user);
            return user;
        }

        // Add record in aspnet_User_Custom table
        // OrganizationId and LocationId i.e. country of user.
        private void CreateUserDetails(Guid userId)
        {
            object[] userValues = GetUserValues(userId);
            SaveUserDetails(userValues);
        }

        private void SaveUserDetails(object[] userValues)
        {
            DBContext.Add("InsertASPNetUserCustom", userValues);
        }

        private object[] GetUserValues(Guid userId)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;

            return new object[] { userId, orgId, countryId, phone, DBNull.Value };
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}