using System;
using System.Data;
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
                txtFullName.Text = dt.Rows[0]["FullName"].ToString();
                ddlOrganization.SelectedValue = dt.Rows[0]["OrganizationId"].ToString();

                //string[] roles = Roles.GetRolesForUser(mu.UserName);
                //if (roles.Length > 0)
                //{
                //    if (roles[0] == "CountryAdmin")
                //    {
                //        divCountry.Visible = false;
                //        rfvCountry.Enabled = false;

                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            ListItem item = ddlLocations.Items.FindByValue(dr["LocationId"].ToString());
                //            item.Selected = true;
                //        }

                //    }
                //    else
                //    {
                ddlCountry.SelectedValue = dt.Rows[0]["LocationId"].ToString();
                //        divLocation.Visible = false;
                //    }
                //}
            }
        }

        private DataTable GetUserDetails(Guid userId)
        {
            return DBContext.GetData("GetUserDetails", new object[] { 1, userId });
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
            Guid userId = new Guid(Session["EditUserId"].ToString());
            MembershipUser mu = Membership.GetUser(userId);
            mu.Email = txtEmail.Text;
            Membership.UpdateUser(mu);

            object[] userValues = GetUserValues(userId);
            DBContext.Add("UpdateASPNetUserCustom", userValues);

            Response.Redirect("~/Admin/UsersListing.aspx");
        }

        private object[] GetUserValues(Guid userId)
        {
            int tempVal = 0;
            int.TryParse(ddlOrganization.SelectedValue, out tempVal);
            int? orgId = tempVal > 0 ? tempVal : (int?)null;
            string countryId = null;
            countryId = ddlCountry.SelectedValue;
            string phone = txtPhone.Text.Trim().Length > 0 ? txtPhone.Text.Trim() : null;
            return new object[] { userId, orgId, countryId, phone, txtFullName.Text.Trim(), DBNull.Value };
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            SRFROWCA.Common.ExceptionUtility.LogException(exc, "UpdateUser", this.User);
        }
    }
}