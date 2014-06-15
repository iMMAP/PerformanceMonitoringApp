using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.Account
{
    public partial class Register2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            //DataTable dt = DBContext.GetData("GetOPSUsersTemp");
            //foreach (DataRow dr in dt.Rows)
            //{
            //    CreateUser(dr);
            //}
        }

        private void CreateUser(DataRow dr)
        {
            try
            {
                int organizationId = Convert.ToInt32(dr["OrganizationId"].ToString());
                int locationId = Convert.ToInt32(dr["LocationId"].ToString());
                string userName = dr["Email"].ToString();
                string email = userName;
                string phone = dr["Phone"].ToString();

                if (!IsUserAlreadyExits(userName, email))
                {
                    MembershipUser user = null;

                    // Add user in aspnet membership tables.
                    user = AddRecordInMembership(userName, "abc123", email);

                    Guid userId = (Guid)user.ProviderUserKey;
                    AddRecordInRoles(userId);

                    // User details are organization and country.
                    CreateUserDetails(userId, organizationId, locationId, phone);
                }
            }
            catch
            {

            }
        }

        private bool IsUserAlreadyExits(string userName, string email)
        {
            MembershipUser membershipUser = Membership.GetUser(email);

            if (membershipUser != null)
            {
                return true;
            }
            else
            {
                membershipUser = Membership.GetUser(userName);

                if (membershipUser != null)
                {
                    return true;
                }
            }

            return false;
        }

        // Add new user in db.
        private MembershipUser AddRecordInMembership(string userName, string password, string email)
        {
            // Adds a new user to the data store.
            MembershipUser user = Membership.CreateUser(userName, password, email);
            user.IsApproved = true;

            // Updates the database with the information for the specified user.
            Membership.UpdateUser(user);
            return user;
        }

        private void AddRecordInRoles(Guid userId)
        {
            string roleName = "User";
            DBContext.Add("InsertUserInRole", new object[] { userId, roleName, DBNull.Value });
        }

        private void CreateUserDetails(Guid userId, int orgId, int countryId, string phone)
        {
            DBContext.Add("InsertASPNetUserCustom", new object[] { userId, orgId, countryId, phone, DBNull.Value });
        }
    }
}