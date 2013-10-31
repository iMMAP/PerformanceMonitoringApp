using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Reports;

namespace SRFROWCA.Account
{
    public partial class UpdateUser : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            GZipContents.GZipOutput();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.User.IsInRole("Admin"))
            {
                Response.Redirect("~/Default.aspx");
            }

            if (IsPostBack) return;

            this.Form.DefaultButton = this.btnUpdate.UniqueID;            

            PopulateCountries();
            PopulateOrganizations();

            if (Session["EditUserId"] != null)
            {
                GetUserInformation();
            }
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
                ddlOrganization.SelectedValue = dt.Rows[0]["OrganizationId"].ToString();

                string[] roles = Roles.GetRolesForUser(mu.UserName);
                if (roles.Length > 0)
                {
                    if (roles[0] == "CountryAdmin")
                    {
                        divCountry.Visible = false;
                        rfvCountry.Enabled = false;

                        foreach (DataRow dr in dt.Rows)
                        {
                            ListItem item = ddlLocations.Items.FindByValue(dr["LocationId"].ToString());
                            item.Selected = true;
                        }

                    }
                    else
                    {
                        ddlCountry.SelectedValue = dt.Rows[0]["LocationId"].ToString();
                        divLocation.Visible = false;
                    }
                }
            }
        }

        private DataTable GetUserDetails(Guid userId)
        {
            return DBContext.GetData("GetUserDetails", new object[] { userId });
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

            ddlLocations.DataValueField = "LocationId";
            ddlLocations.DataTextField = "LocationName";
            ddlLocations.DataSource = dt;
            ddlLocations.DataBind();
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Guid userId = new Guid(Session["EditUserId"].ToString());
            MembershipUser mu = Membership.GetUser(userId);
            mu.Email = txtEmail.Text;
            Membership.UpdateUser(mu);

            string[] roles = Roles.GetRolesForUser(mu.UserName);
            if (roles.Length > 0)
            {
                object[] userValues = GetUserValues(userId, roles[0]);
                if (roles[0] == "CountryAdmin")
                {
                    DBContext.Add("UpdateASPNetUserCustomWithMultipleLocations", userValues);
                }
                else if (roles[0] == "User")
                {
                    DBContext.Add("UpdateASPNetUserCustom", userValues);
                }
            }

            Response.Redirect("~/Admin/Users/UsersListing.aspx");
        }

        private object[] GetUserValues(Guid userId, string roleName)
        {
            int orgId = 0;
            int.TryParse(ddlOrganization.SelectedValue, out orgId);
            string countryId = null;

            if (roleName == "CountryAdmin")
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

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UpdateUser", this.User);
        }
    }
}