using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin.Organization
{
    public partial class OrganizationList : BasePage
    {
        private BusinessLogic.Organization objOrganization = new BusinessLogic.Organization();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSearch.UniqueID;
            if (IsPostBack) return;
            BindData();
           
        }
        private void BindData()
        {
            LoadOrganizationTypes();
            LoadCountries();
            LoadOrganizations();
        }
        private object[] GetParameters()
        {
            string @orgnaizationName = string.IsNullOrEmpty(txtOrganizationName.Text.Trim()) ? null : txtOrganizationName.Text.Trim();
            string @organizationAcronym = string.IsNullOrEmpty(txtAcronym.Text.Trim()) ? null : txtAcronym.Text.Trim();
            int? @organizationTypeId = ddlType.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlType.SelectedValue);
            int? countryId = ddlCountry.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlCountry.SelectedValue);
            int? @status = ddlStatus.SelectedValue == "-1" ? (int?)null : Convert.ToInt32(ddlStatus.SelectedValue);

            return new object[] {DBNull.Value, @orgnaizationName, @organizationAcronym, @organizationTypeId, countryId, @status };
        }            

        private void LoadOrganizationTypes()
        {
            ddlType.DataSource = objOrganization.GetOrganizationTypes();
            ddlType.DataTextField = "OrganizationType";
            ddlType.DataValueField = "OrganizationTypeId";
            ddlType.DataBind();

        }

        private void LoadCountries()
        {
            int locTypeId = 2;
            object[] parameters = new object[] { locTypeId };
            ddlCountry.DataSource = DBContext.GetData("GetLocationOnType", parameters);
            ddlCountry.DataTextField = "LocationName";
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataBind();

        }

        private void LoadOrganizations()
        {            
            gvOrganization.DataSource = objOrganization.GetOrganizations(GetParameters());
            gvOrganization.DataBind();
        }
        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadOrganizations();
        }
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            string fileName = "Organizations";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvOrganization, fileName, fileExtention, Response);
        }
        protected void gvOrganization_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect(string.Format("{0}/Admin/Organization/AddEditOrganization.aspx?oid={1}", Master.BaseURL, Utils.EncryptQueryString(e.CommandArgument.ToString())), true);
            }
            else if (e.CommandName == "Delete")
            {
                int orgId = Convert.ToInt32(e.CommandArgument);

                // Check if any IP has reported on this project. If so then do not delete it.
                if (objOrganization.AnyUserExistsInOrganization(orgId))
                {
                    lblMessage.Text = "Organization cannot be deleted! Users are registered under this organization.";
                    lblMessage.CssClass = "error-message";
                    lblMessage.Visible = true;

                    return;
                }
                else
                {

                    objOrganization.Delete(orgId);
                    LoadOrganizations();
                }
            }
            
        }
        protected void gvOrganization_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = objOrganization.GetOrganizations(GetParameters());
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvOrganization.DataSource = dt;
                gvOrganization.DataBind();
            }
        }

        protected void gvOrganization_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button deleteButton = e.Row.FindControl("btnDelete") as Button;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this organization?')");
                }
            }
        }

        protected void gvOrganization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvOrganization.PageIndex = e.NewPageIndex;
            gvOrganization.SelectedIndex = -1;
            LoadOrganizations();
        }

        protected void gvOrganization_RowDeleting(Object sender, GridViewDeleteEventArgs e)
        {
           
        }
    }

}