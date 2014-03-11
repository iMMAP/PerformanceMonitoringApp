using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;
using System.Web.Security;
using SRFROWCA.Reports;

namespace SRFROWCA.Account
{
    public partial class Registerca : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnRegister.UniqueID;
            this.Form.DefaultFocus = this.txtUserName.UniqueID;

            PopulateCountries();
        }

        private DataTable GetUserData()
        {
            return DBContext.GetData("GetUserInfo");
        }

        private void GetUserIdFromSession()
        {
            if (Session["EditUserId"] != null)
            {
                UserId = Session["EditUseId"].ToString();
            }
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
                string message = "Account created successfully!";
                if (returnValue)
                {
                    try
                    {
                        string from = "3wopactivities@gmail.com";
                        string to = txtEmail.Text.Trim();
                        string subject = "New 3W Performace Monitoring Account!";
                        string mailBody = "";
                        if (this.User.IsInRole("Admin"))
                        {
                            mailBody = string.Format(@"3W Perfomance Monitoring admin has created your new account.
                                                        User Name: {0} \n 
                                                        Password: {1} \n
                                                        If you face any difficulty login, please contact to admin of the site!",
                                                        txtUserName.Text.Trim(), txtPassword.Text);

                            Mail.SendMail(from, txtEmail.Text, subject, mailBody);
                            message = "An email has been send to " + txtEmail.Text;
                        }
                    }
                    catch
                    {
                        message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for the inconvenience!";
                    }
                }

                ShowMessage(message, RC.NotificationType.Success, false, 500);

                ClearRegistrationControls();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        private void ClearRegistrationControls()
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            DataTable dt = RC.GetLocations(this.User, (int)RC.LocationTypes.National);
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";
            ddlLocations.DataSource = dt;
            ddlLocations.DataBind();
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
                ShowMessage(string.Format("This email {0} already exists in db!", txtEmail.Text.Trim()), RC.NotificationType.Error);
                return true;
            }
            else
            {
                membershipUser = Membership.GetUser(txtUserName.Text.Trim());

                if (membershipUser != null)
                {
                    ShowMessage(string.Format("Someone already has this, {0}, username. Try another?!", txtUserName.Text.Trim()), RC.NotificationType.Error);
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

            Guid userId = (Guid)user.ProviderUserKey;
            AddRecordInRoles(userId);

            // User details are organization and country.
            CreateUserDetails(userId);

            return true;
        }

        private void AddRecordInRoles(Guid userId)
        {
            string roleName = "CountryAdmin";
            DBContext.Add("InsertUserInRole", new object[] { userId, roleName, DBNull.Value });
        }

        // Add new user in db.
        private MembershipUser AddRecordInMembership()
        {
            // Adds a new user to the data store.
            MembershipUser user = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());
            user.IsApproved = true;

            // Updates the database with the information for the specified user.
            Membership.UpdateUser(user);
            return user;
        }

        // Add record in aspnet_User_Custom table
        // OrganizationId and LocationId i.e. country of user.
        private void CreateUserDetails(Guid userId)
        {
            object[] userValues = GetUserValues(userId);
            DBContext.Add("InsertASPNetUserCustomWithMultipleLocations", userValues);
        }

        private object[] GetUserValues(Guid userId)
        {
            int? orgId = null;
            string countryId = null;
            countryId = ReportsCommon.GetSelectedValues(ddlLocations);
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            return new object[] { userId, orgId, countryId, phone, DBNull.Value };
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            updMessage.Update();
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        public string UserId
        {
            get
            {
                if (ViewState["EditUserId"] != null)
                {
                    return ViewState["EditUserId"].ToString();
                }
                return null;
            }
            set
            {
                ViewState["EditUserId"] = value.ToString();
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "Register", this.User);
        }
    }
}