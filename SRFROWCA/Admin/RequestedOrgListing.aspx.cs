using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class RequestedOrgListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOrganizations();
            }
        }

        internal override void BindGridData()
        {
            LoadOrganizations();
        }

        private void LoadOrganizations()
        {
            DataTable dt = DBContext.GetData("GetRequestedOrganizations");
            gvOrganization.DataSource = dt;
            gvOrganization.DataBind();
        }

        protected void cbActive_Changed(object sender, EventArgs e)
        {
            GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
            int orgId = 0;
            int.TryParse(gvOrganization.DataKeys[row.RowIndex].Values["OrganizationRequestID"].ToString(), out orgId);

            if (orgId > 0)
            {
                CheckBox cbIsActive = row.FindControl("cbIsActive") as CheckBox;
                if (cbIsActive != null)
                {
                    if (cbIsActive.Checked)
                    {
                        if (!OrgAlreadyExists(orgId))
                        {
                            AddOrganization(orgId);
                            DataTable dt = DBContext.GetData("GetRequestedOrganizationInfo", new object[] { orgId });
                            EmailNotifications.SendEmailRequestOrg(dt, true);
                        }
                        else
                        {
                            lblMessage.Text = "Organization already exists with same name.";
                            lblMessage.Visible = true;
                        }
                    }
                }
            }

            LoadOrganizations();
        }

        

        private bool OrgAlreadyExists(int orgId)
        {
            return (DBContext.GetData("IsRequestedOrgAlreadyExists", new object[] { orgId }).Rows.Count > 0);
        }

        private void AddOrganization(int orgId)
        {
            DBContext.Add("AddRquestedOrgToLive", new object[] { orgId, DBNull.Value });
        }

        protected void gvOrganization_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteReqOrg")
            {
                int orgId = 0;
                int.TryParse(e.CommandArgument.ToString(), out orgId);
                if (orgId > 0)
                {
                    DataTable dt = DBContext.GetData("GetRequestedOrganizationInfo", new object[] { orgId });                    
                    DBContext.Delete("DeleteRequestedOrganization", new object[] { orgId, DBNull.Value });
                    EmailNotifications.SendEmailRequestOrg(dt, false);
                    LoadOrganizations();
                }
            }
        }

        protected void gvOrganization_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to reject this organization request?')");
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