using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Reports;
using Microsoft.Office.Interop.Access;

namespace SRFROWCA.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!this.User.IsInRole("Admin"))
            {
                divUserRoles.Visible = false;
                ddlLocations.Visible = false;
                ltrlLocation.Visible = false;
            }

            this.Form.DefaultButton = this.btnRegister.UniqueID;
            this.Form.DefaultFocus = this.txtUserName.UniqueID;

            PopulateCountries();
            PopulateOrganizations();
        }

        private void PopulateControlsWithUserInfo(DataTable dt)
        {
            throw new NotImplementedException();
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
                        string to = "3wopactivities@gmail.com";
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
                        else
                        {
                            mailBody = "Dear Adim! Please activitate my account " + txtUserName.Text.Trim();
                            Mail.SendMail(from, to, subject, mailBody);
                            message = @"You have been registered successfully, we will verify your
                                        credentials and activate your account in few hours!";
                        }
                    }
                    catch
                    {
                        message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for the inconvenience!";
                    }
                }


                ShowMessage(message, ROWCACommon.NotificationType.Success, false, 500);

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

            DataTable dt = ROWCACommon.GetLocations(this.User);
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();

            ListItem item = new ListItem("Select Your Country", "0");
            ddlCountry.Items.Insert(0, item);

            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";
            ddlLocations.DataSource = dt;
            ddlLocations.DataBind();
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
                ShowMessage(string.Format("This email {0} already exists in db!", txtEmail.Text.Trim()), ROWCACommon.NotificationType.Error);
                return true;
            }
            else
            {
                membershipUser = Membership.GetUser(txtUserName.Text.Trim());

                if (membershipUser != null)
                {
                    ShowMessage(string.Format("Someone already has this, {0}, username. Try another?!", txtUserName.Text.Trim()), ROWCACommon.NotificationType.Error);
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

            CreateUserOffice(userId);
            return true;
        }

        // If user office does not exists on selected organization and lcoation
        // then create one
        private void CreateUserOffice( Guid userId)
        {
            if (!IsOrganizationOfficeExists())
            {
                CreateOffice(userId);
            }
        }

        private void AddRecordInRoles(Guid userId)
        {
            string roleName = rbtnCountryAdmin.Checked ? "CountryAdmin" : "User";
            DBContext.Add("InsertUserInRole", new object[] { userId, roleName, DBNull.Value });
        }

        // Add new user in db.
        private MembershipUser AddRecordInMembership()
        {
            // Adds a new user to the data store.
            MembershipUser user = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());

            if (this.User.IsInRole("Admin"))
            {
                user.IsApproved = true;
            }
            else
            {
                user.IsApproved = false;
            }

            // Updates the database with the information for the specified user.
            Membership.UpdateUser(user);
            return user;
        }

        // Add record in aspnet_User_Custom table
        // OrganizationId and LocationId i.e. country of user.
        private void CreateUserDetails(Guid userId)
        {
            object[] userValues = GetUserValues(userId);
            if (ROWCACommon.IsAdmin(this.User))
            {
                DBContext.Add("InsertASPNetUserCustomWithMultipleLocations", userValues);
            }
            else
            {
                DBContext.Add("InsertASPNetUserCustom", userValues);
            }
        }

        private object[] GetUserValues(Guid userId)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);
            string countryId = null;

            if (ROWCACommon.IsAdmin(this.User))
            {
                countryId = ReportsCommon.GetSelectedValues(ddlLocations);
                if (countryId == null)
                {
                    countryId = ddlCountry.SelectedValue;
                }
            }
            else
            {
                countryId = ddlCountry.SelectedValue;
            }

            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;

            return new object[] { userId, orgId, countryId, phone, DBNull.Value };
        }

        // If selected organization office exists on selected locaiton
        private bool IsOrganizationOfficeExists()
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);

            int locId = 0;
            int.TryParse(ddlCountry.SelectedValue, out locId);

            if (orgId > 0 && locId > 0)
            {
                return DBContext.GetData("GetOrganizationOffices", new object[] { locId, orgId }).Rows.Count > 0;
            }

            return false;
        }

        // Create user's office on organization and location
        private void CreateOffice(Guid userId)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);

            int locId = 0;
            int.TryParse(ddlCountry.SelectedValue, out locId);

            string officeName = ddlOrganization.SelectedItem.Text + " " + ddlCountry.SelectedItem.Text;

            if (orgId > 0 && locId > 0)
            {
                DBContext.Add("InsertOffice", new object[] { orgId, locId, officeName, userId, DBNull.Value });
            }
        }

        private void ShowMessage(string message, ROWCACommon.NotificationType notificationType = ROWCACommon.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            updMessage.Update();
            ROWCACommon.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
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