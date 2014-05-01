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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnRegister.UniqueID;
            this.Form.DefaultFocus = this.txtUserName.UniqueID;

            PopulateCountries();
            PopulateOrganizations();
            PopulateClusters();
        }

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

        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
            string text = RC.SelectedSiteLanguageId == 2 ? "Sélectionner votre Cluster" : "Select Your Cluster";
            ListItem item = new ListItem(text, "0");
            ddlClusters.Items.Insert(0, item);
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
                if (!DataIsValid()) return;
                // If user already exists return.
                if (IsUserAlreadyExits()) return;

                // Create New 1)Rider, 2) Places, 3) Route,
                // 4) Send Email to user to verify account.
                returnValue = CreateUserAccount();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message, RC.NotificationType.Error, false);
            }

            if (returnValue)
            {   
                Response.Redirect("~/Admin/UsersListing.aspx");
            }
        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            DataTable dt = RC.GetLocations(this.User, (int)RC.LocationTypes.National);
            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";
            ddlLocations.DataSource = dt;
            ddlLocations.DataBind();

            ListItem item = new ListItem("Select Country", "0");
            ddlLocations.Items.Insert(0, item);

            if (HttpContext.Current.User.IsInRole("CountryAdmin"))
            {
                ddlLocations.SelectedValue = UserInfo.Country.ToString();
                ddlLocations.Enabled = false;
            }
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

            if (ddlUserRole.SelectedValue.Equals("5"))
            {
                roleName = "CountryAdmin";
            }
            else if (ddlUserRole.SelectedValue.Equals("4"))
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
            user.IsApproved = true;

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
            if (ddlUserRole.SelectedValue.Equals("3"))
            {
                ddlLocations.SelectedValue = "567";
            }

            int? orgId = null;
            orgId = ddlOrganization.SelectedValue != "0"? Convert.ToInt32(ddlOrganization.SelectedValue) : (int?)null;
            string countryId = null;
            countryId = ddlLocations.SelectedValue;
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            return new object[] { userId, orgId, countryId, phone, txtFullName.Text, DBNull.Value };
        }

        protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOrganization.SelectedIndex = 0;
            ddlClusters.SelectedIndex = 0;
            ddlLocations.SelectedIndex = 0;

            if (ddlUserRole.SelectedValue.Equals("0"))
            {
                ddlOrganization.Enabled = true;
                ddlClusters.Enabled = true;
                divOrg.Visible = true;
                divCluster.Visible = true;
            }
            else if (ddlUserRole.SelectedValue.Equals("5"))
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = false;
                divOrg.Visible = false;
                divCluster.Visible = false;
            }
            else if (ddlUserRole.SelectedValue.Equals("4"))
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = false;
                divOrg.Visible = false;
                divCluster.Visible = false;
            }
            else if (ddlUserRole.SelectedValue.Equals("3"))
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = true;
                divOrg.Visible = false;
                divCluster.Visible = true;
            }
            else if (ddlUserRole.SelectedValue.Equals("2"))
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = true;
                divOrg.Visible = false;
                divCluster.Visible = true;
            }
            else if (ddlUserRole.SelectedValue.Equals("1"))
            {
                ddlClusters.Enabled = false;
                ddlOrganization.Enabled = true;
                divOrg.Visible = true;
                divCluster.Visible = false;
            }
        }

        private bool DataIsValid()
        {
            //if (ddlUserRole.SelectedValue.Equals("5") || ddlUserRole.SelectedValue.Equals("4"))
            //{
            //    //roleName = "CountryAdmin";
            //}

            if (ddlUserRole.SelectedValue.Equals("3") || ddlUserRole.SelectedValue.Equals("2"))
            {
                if (ddlClusters.SelectedIndex == 0)
                    return false;
            }
            else if (ddlUserRole.SelectedValue.Equals("1"))
            {
                if (ddlOrganization.SelectedIndex == 0)
                    return false;
            }

            return true;
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
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