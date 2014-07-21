using System;
using System.Data;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Account
{
    public partial class Register : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!User.IsInRole("Admin"))
            {
                Form.DefaultButton = btnRegister.UniqueID;
            }

            PopulateCountries();
            PopulateOrganizations();
            PopulateClusters();
        }

        internal override void BindGridData()
        {
            PopulateCountries();
            PopulateOrganizations();
            PopulateClusters();
        }

        #region Events.

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string message = "";
            try
            {
                if (!Valid()) return;
                // If user already exists return.
                if (IsUserAlreadyExits()) return;

                // Create New 1)Rider, 2) Places, 3) Route,
                // 4) Send Email to user to verify account.

                bool returnValue = CreateUserAccount();
                //message = "Account created successfully!";
                if (returnValue)
                {
                    try
                    {
                        DataTable dtEmails = DBContext.GetData("uspGetUserEmails", new object[] { ddlCountry.SelectedValue, 0 });
                        string emails = string.Empty;
                        emails = "orsocharowca@gmail.com";
                        if (dtEmails.Rows.Count > 0)
                        {
                            using (MailMessage mailMsg = new MailMessage())
                            {
                                for (int i = 0; i < dtEmails.Rows.Count; i++)
                                    emails += "," + Convert.ToString(dtEmails.Rows[i]["Email"]);

                                mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                                //mailMsg.To.Add(new MailAddress("orsocharowca@gmail.com"));
                                mailMsg.To.Add(emails.TrimEnd(','));
                                mailMsg.Subject = "New ORS Account Created By User!";
                                mailMsg.IsBodyHtml = true;
                                mailMsg.Body = string.Format(@"New {0} Registered with following credentials<hr/>
                                                        <b>Name:</b> {1}<br/>
                                                        <b>User Name:</b> {2}<br/>
                                                        <b>Email:</b> {3}<br/>                                                        
                                                        <b>Cluster:</b> {4}<br/>
                                                        <b>Organization:</b> {5}<br/>
                                                        <b>Country:</b> {6}", ddlUserRole.SelectedItem.Text, txtFullName.Text.Trim(), txtUserName.Text.Trim(),
                                                                           txtEmail.Text.Trim(), ddlClusters.SelectedItem.Text, ddlOrganization.SelectedItem.Text,
                                                                           ddlCountry.SelectedItem.Text);
                                Mail.SendMail(mailMsg);
                                message = @"You have been registered successfully, we will verify your credentials and activate your account in few hours!";
                            }
                        }

                    }
                    catch
                    {
                        message = "You have been registered successfully but some error occoured on sending email to site admin. Contact admin and ask for the verification! We apologies for the inconvenience!";
                    }
                }

            }
            catch (Exception ex)
            {
                Session["RegisterMessage"] = null;
                message = ex.Message;
                ShowMessage(ex.Message, RC.NotificationType.Error, false);
                return;
            }

            Session["RegisterMessage"] = message;
            Response.Redirect("../RegisterSuccess.aspx");
        }

        #endregion

        // Populate countries drop down.
        private void PopulateOrganizations()
        {
            ddlOrganization.DataValueField = "OrganizationId";
            ddlOrganization.DataTextField = "OrganizationAcronym";

            ddlOrganization.DataSource = GetOrganizations();
            ddlOrganization.DataBind();

            string text = RC.SelectedSiteLanguageId == 2 ? "Séléctionner votre Organisation" : "Select Your Organization";
            ListItem item = new ListItem(text, "0");
            ddlOrganization.Items.Insert(0, item);
        }

        private DataTable GetOrganizations()
        {
            DataTable dt = DBContext.GetData("GetOrganizations");

            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            DataTable dt = RC.GetLocations(User, (int)RC.LocationTypes.National);
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();

            string text = RC.SelectedSiteLanguageId == 2 ? "Sélectionner votre Pays" : "Select Your Country";
            ListItem item = new ListItem(text, "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
            string text = RC.SelectedSiteLanguageId == 2 ? "Sélectionner votre Cluster" : "Select Your Cluster";
            ListItem item = new ListItem(text, "0");
            ddlClusters.Items.Insert(0, item);
        }

        private bool Valid()
        {
            string clusterIds = RC.GetSelectedValues(ddlClusters);

            string message = "";
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

        // Check if user (email address) is already in db.
        // If not null then user already exists otherwise not.
        private bool IsUserAlreadyExits()
        {
            MembershipUser membershipUser = Membership.GetUser(txtUserName.Text.Trim());
            if (membershipUser != null)
            {
                ShowMessage(string.Format("Another user already exists in the system with the same username ({0}). Try another!", txtUserName.Text.Trim()), RC.NotificationType.Error, false);
                return true;
            }

            //string userName = Membership.GetUserNameByEmail(txtEmail.Text.Trim());

            //if (!string.IsNullOrEmpty(userName))
            //{
            //    ShowMessage(string.Format("Another user already exists in the system with the same email ({0})", txtEmail.Text.Trim()), RC.NotificationType.Error, false);
            //    return true;
            //}

            return false;
        }

        // Create new user, places and route.
        private bool CreateUserAccount()
        {
            // Add user in aspnet membership tables.
            MembershipUser user = AddRecordInMembership();
            if (user.ProviderUserKey != null)
            {
                Guid userId = (Guid)user.ProviderUserKey;
                AddRecordInRoles(userId);

                // User details are organization and country.
                CreateUserDetails(userId);
            }
            return true;
        }

        private void AddRecordInRoles(Guid userId)
        {
            string roleName = "User";

            if (ddlUserRole.SelectedValue.Equals("5"))
            {
                roleName = "CountryAdmin";
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
            else if (ddlUserRole.SelectedValue.Equals("6"))
            {
                roleName = "OCHACountryStaff";
            }

            DBContext.Add("InsertUserInRole", new object[] { userId, roleName, DBNull.Value });
        }

        // Add new user in db.
        private MembershipUser AddRecordInMembership()
        {
            // Adds a new user to the data store.
            MembershipUser user = Membership.CreateUser(txtUserName.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());

            if (User.IsInRole("Admin"))
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

        private void InsertLocationsAndOrg(Guid userId)
        {
            object[] userValues = GetUserValues(userId);
            DBContext.Add("InsertASPNetUserCustom", userValues);
        }

        private void InsertClusters(Guid userId)
        {
            if (ddlUserRole.SelectedValue.Equals("2") || ddlUserRole.SelectedValue.Equals("3"))
            {
                int clusterId = Convert.ToInt32(ddlClusters.SelectedValue);
                DBContext.Add("InsertASPNetUserCluster", new object[] { userId, clusterId, DBNull.Value });
            }
        }

        private object[] GetUserValues(Guid userId)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);
            string countryId = ddlCountry.SelectedValue;
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            string fullName = txtFullName.Text.Trim();

            return new object[] { userId, ddlUserRole.SelectedValue == "1" ? orgId : (int?)null, countryId, phone, fullName, DBNull.Value };
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            //updMessage.Update();
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        #region Properties & Enums.
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
                ViewState["EditUserId"] = value;
            }
        }
        #endregion

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, "Register", User);
        }
    }
}