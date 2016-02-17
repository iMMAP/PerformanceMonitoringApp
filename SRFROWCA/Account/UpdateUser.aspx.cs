using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Reports;

namespace SRFROWCA.Account
{
    public partial class UpdateUser : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.IsInRole("Admin") && !this.User.IsInRole("CountryAdmin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnUpdate.UniqueID;

            PopulateRoles();
            PopulateCountries();
            PopulateOrganizations();
            PopulateClusters();
            //PopulateEmergencies();

            if (Session["EditUserId"] != null)
            {
                GetUserInformation();
                ShowHideControls();
            }
        }

        //private void PopulateEmergencies()
        //{
        //    int locationId = (int)RC.SelectedSiteLanguageId;
        //    //UI.FillEmergency(ddlEmergency, RC.GetAllEmergencies(locationId));
        //}

        private void ShowHideControls()
        {
            if (ddlUserRole.SelectedItem.Text == "OCHA Country Admin" || ddlUserRole.SelectedItem.Text == "OCHA Country Staff")
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = false;
                divOrg.Visible = false;
                divCluster.Visible = false;

                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Region Cluster Lead")
            {
                ddlOrganization.Enabled = false;
                ddlCountry.Enabled = false;
                ddlClusters.Enabled = true;

                divOrg.Visible = false;
                divLocations.Visible = false;
                divCluster.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Country Cluster Lead")
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = true;
                divOrg.Visible = false;
                divCluster.Visible = true;
                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Data Entry/Field Officer")
            {
                ddlClusters.Enabled = false;
                ddlOrganization.Enabled = true;
                divOrg.Visible = true;
                divCluster.Visible = false;
                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
            string text = RC.SelectedSiteLanguageId == 2 ? "Sélectionner votre Cluster" : "Select Your Cluster";
            ListItem item = new ListItem(text, "0");
            ddlClusters.Items.Insert(0, item);
        }

        protected void PopulateRoles()
        {
            ddlUserRole.DataTextField = "RoleName";
            ddlUserRole.DataValueField = "RoleId";
            string userType = "Admin";
            if (RC.IsCountryAdmin(User))
            {
                userType = "CountryAdmin";
            }
            ddlUserRole.DataSource = DBContext.GetData("GetUserRolesToUpdateInUserUpdate", new object[] {userType});
            ddlUserRole.DataBind();
        }

        private void GetUserInformation()
        {
            Guid userId = new Guid(Session["EditUserId"].ToString());
            MembershipUser mu = Membership.GetUser(userId);
            txtUserName.Text = mu.UserName;
            txtEmail.Text = mu.Email;

            DataTable dt = GetUserDetails(userId);
            if (dt.Rows.Count > 0)
            {
                txtPhone.Text = dt.Rows[0]["PhoneNumber"].ToString();
                txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                ddlOrganization.SelectedValue = dt.Rows[0]["OrganizationId"].ToString();
                ddlUserRole.SelectedValue = dt.Rows[0]["RoleId"].ToString();               
                ddlCountry.SelectedValue = dt.Rows[0]["LocationId"].ToString();
                ddlClusters.SelectedValue = dt.Rows[0]["ClusterId"].ToString();
                //ddlEmergency.SelectedValue = dt.Rows[0]["EmergencyId"].ToString();
                if (RC.IsCountryAdmin(User))
                {
                    ddlCountry.Enabled = false;
                }
            }
        }

        private DataTable GetUserDetails(Guid userId)
        {
            return DBContext.GetData("GetUserAccountDetails", new object[] { userId });            
        }

        // Populate countries drop down.
        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            DataTable dt = GetCountries();
            ddlCountry.DataSource = dt;
            ddlCountry.DataBind();

            ListItem item = new ListItem("Select Your Country", "0");
            ddlCountry.Items.Insert(0, item);

            //ddlLocations.DataValueField = "LocationId";
            //ddlLocations.DataTextField = "LocationName";
            //ddlLocations.DataSource = dt;
            //ddlLocations.DataBind();
        }

        private DataTable GetCountries()
        {
            int locationType = (int)RC.LocationTypes.National;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Guid userId = new Guid(Session["EditUserId"].ToString());
                MembershipUser mu = Membership.GetUser(userId);
                mu.Email = txtEmail.Text;
                Membership.UpdateUser(mu);

                Guid roleId = new Guid(ddlUserRole.SelectedValue);
                DBContext.Update("aspnet_Roles_UpdateUserRole", new object[] { userId, roleId, DBNull.Value });

                object[] userValues = GetUserValues(userId);
                DBContext.Add("UpdateASPNetUserCustom", userValues);

                Response.Redirect("~/Admin/UsersListing.aspx");
            }
            catch
            {
                
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/UsersListing.aspx");
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Guid userId = new Guid(Session["EditUserId"].ToString());
            MembershipUser mu = Membership.GetUser(userId);
            mu.ChangePassword(mu.ResetPassword(), txtPassword.Text.Trim());
            Response.Redirect("~/Admin/UsersListing.aspx");
        }

      

        private object[] GetUserValues(Guid userId)
        {
            if (ddlUserRole.SelectedItem.Text == "Region Cluster Lead")
            {
                ddlCountry.SelectedValue = "567";
            }

            int tempVal = 0;
            int.TryParse(ddlOrganization.SelectedValue, out tempVal);
            int? orgId = tempVal > 0 ? tempVal : (int?)null;
            string countryId = null;
            countryId = ddlCountry.SelectedValue;
            int? clusterId = ddlClusters.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlClusters.SelectedValue);
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            return new object[] { userId, orgId, countryId, phone, txtFullName.Text.Trim(), clusterId, DBNull.Value };
        }

        protected void ddlUserRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOrganization.SelectedIndex = 0;
            ddlClusters.SelectedIndex = 0;
            ddlCountry.SelectedIndex = 0;

            if (ddlUserRole.SelectedItem.Text == "OCHA Country Admin" || ddlUserRole.SelectedItem.Text == "OCHA Country Staff")
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = false;
                divOrg.Visible = false;
                divCluster.Visible = false;

                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Region Cluster Lead")
            {
                ddlOrganization.Enabled = false;
                ddlCountry.Enabled = false;
                ddlClusters.Enabled = true;

                divOrg.Visible = false;
                divLocations.Visible = false;
                divCluster.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Country Cluster Lead")
            {
                ddlOrganization.Enabled = false;
                ddlClusters.Enabled = true;
                divOrg.Visible = false;
                divCluster.Visible = true;
                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }
            else if (ddlUserRole.SelectedItem.Text == "Data Entry/Field Officer")
            {
                ddlClusters.Enabled = false;
                ddlOrganization.Enabled = true;
                divOrg.Visible = true;
                divCluster.Visible = false;
                ddlCountry.Enabled = true;
                divLocations.Visible = true;
            }

            if (ddlCountry.Visible)
            {
                if (HttpContext.Current.User.IsInRole("CountryAdmin"))
                {
                    ddlCountry.SelectedValue = UserInfo.Country.ToString();
                    //ddlLocations.Enabled = false;
                }
            }
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}