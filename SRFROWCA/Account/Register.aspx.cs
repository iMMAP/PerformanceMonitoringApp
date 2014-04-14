using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Reports;

namespace SRFROWCA.Account
{
    public partial class Register : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!this.User.IsInRole("Admin"))
            {
                this.Form.DefaultButton = this.btnRegister.UniqueID;
            }

            PopulateCountries();
            PopulateOrganizations();
            PopulateClusters();
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, 1);
            ListItem item = new ListItem("Select Your Cluster", "0");
            ddlClusters.Items.Insert(0, item);
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
            string message = "";
            try
            {
                if (!Valid()) return;
                // If user already exists return.
                if (IsUserAlreadyExits()) return;

                // Create New 1)Rider, 2) Places, 3) Route,
                // 4) Send Email to user to verify account.
                returnValue = CreateUserAccount();
                message = "Account created successfully!";
                if (returnValue)
                {
                    try
                    {
                        string from = txtEmail.Text.Trim();
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
                            mailBody = "Dear Admin! Please activitate my account Username: " + txtUserName.Text.Trim() + ", Full Name: " + txtFullName.Text.Trim();
                            Mail.SendMail(from, to, subject, mailBody);
                            message = @"You have been registered successfully, we will verify your credentials and activate your account in few hours!";
                        }
                    }
                    catch
                    {
                        message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for the inconvenience!";
                    }
                }

                //ShowMessage(message, RC.NotificationType.Success, false, 500);
                

                //ClearRegistrationControls();
            }
            catch (Exception ex)
            {
                message = ex.Message;
                ShowMessage(ex.Message);
            }

            Session["RegisterMessage"] = message;
            Response.Redirect("../RegisterSuccess.aspx");
        }

        private bool Valid()
        {
            //string countryIds = RC.GetSelectedValues(ddlLocations);
            string clusterIds = RC.GetSelectedValues(ddlClusters);

            string message = "";
            //if (string.IsNullOrEmpty(countryIds))
            //{
            //    message = "Please select at least one country.";
            //}

            if (ddlUserRole.SelectedValue.Equals("2") && string.IsNullOrEmpty(clusterIds))
            {
                message += " Please select your cluster(s).";
            }

            if (!string.IsNullOrEmpty(message))
            {
                ShowMessage(message, RC.NotificationType.Error, false);
                return false;
            }

            return true;
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

            DataTable dt = RC.GetLocations(this.User, (int)RC.LocationTypes.National);
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();

            ListItem item = new ListItem("Select Your Country", "0");
            ddlCountry.Items.Insert(0, item);

            //ddlLocations.DataValueField = "LocationId";
            //ddlLocations.DataTextField = "LocationName";
            //ddlLocations.DataSource = dt;
            //ddlLocations.DataBind();
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
            string roleName = "User";

            if (ddlUserRole.SelectedValue.Equals("4"))
            {
                roleName = "OCHA";
            }
            else if (ddlUserRole.SelectedValue.Equals("3"))
            {
                roleName = "RegionalClusterLead";
            }
            else if (ddlUserRole.SelectedValue.Equals("2"))
            {
                roleName = "ClusterLead";
            }
            else if (ddlUserRole.SelectedValue.Equals("1"))
            {
                roleName = "User";
            }

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
            InsertLocationsAndOrg(userId);
            InsertClusters(userId);
        }

        private void InsertClusters(Guid userId)
        {
            if (ddlUserRole.SelectedValue.Equals("2") || ddlUserRole.SelectedValue.Equals("3"))
            {
                int clusterId = Convert.ToInt32(ddlClusters.SelectedValue);
                DBContext.Add("InsertASPNetUserCluster", new object[] { userId, clusterId, DBNull.Value });
            }
        }

        private void InsertLocationsAndOrg(Guid userId)
        {
            object[] userValues = GetUserValues(userId);
            DBContext.Add("InsertASPNetUserCustom", userValues);
        }

        private object[] GetUserValues(Guid userId)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);
            string countryId = null;
            countryId = ddlCountry.SelectedValue;
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            string fullName = txtFullName.Text.Trim();

            return new object[] { userId, orgId, countryId, phone, fullName, DBNull.Value };
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            //updMessage.Update();
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