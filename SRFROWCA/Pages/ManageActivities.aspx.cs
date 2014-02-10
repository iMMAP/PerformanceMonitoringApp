using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Common;

namespace SRFROWCA.Pages
{
    public partial class ManageActivities : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateProjects();
            PopulateLocations();

        }

        private void PopulateLocations()
        {
            cblLocation.DataValueField = "LocationId";
            cblLocation.DataTextField = "LocationName";
            DataTable dt = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { 2 });
            cblLocation.DataSource = dt;
            cblLocation.DataBind();

           
        }

        private void PopulateProjects()
        {
            ddlProjects.DataValueField = "ProjectId";
            ddlProjects.DataTextField = "ProjectTitle";
            DataTable dt = DBContext.GetData("GetAllProjectsTest");
            ddlProjects.DataSource = dt;
            ddlProjects.DataBind();

            Session["testprojectdata"] = dt;
            //
        }

        protected void wzrdReport_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 1)
            {
                PopulateLogFrame();
            }
            else if (e.NextStepIndex == 2)
            {
               
            }
        }

        protected void wzrdReport_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            Save();
            Response.Redirect("~/Pages/AddActivities.aspx");
        }

        private void PopulateLogFrame()
        {
            int projectId = Convert.ToInt32(ddlProjects.SelectedValue);

            int clusterId = 0;
            DataTable dt = Session["testprojectdata"] as DataTable;
            foreach(DataRow dr in dt.Rows)
            {
                if(dr["ProjectId"].ToString() == projectId.ToString())
                {
                    clusterId = Convert.ToInt32( dr["ClusterId"]);
                }
            }

            gvActivities.DataSource = DBContext.GetData("GetLogFrameOfClusterTest", new object[] { clusterId, 1 });
            gvActivities.DataBind();
        }

        protected void wzrdReport_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 0)
            {
                PopulateProjects();
                PopulateLocations();
            }
        }

        private void Save()
        {
            foreach (GridViewRow row in gvActivities.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    int pId = Convert.ToInt32(gvActivities.DataKeys[row.RowIndex].Values["PriorityActivityId"].ToString());
                    TextBox txtBale = row.FindControl("txtBale") as TextBox;
                    int? tBale = null;
                    if (txtBale != null)
                    {
                        tBale = string.IsNullOrEmpty(txtBale.Text.Trim()) ? (int?)null : Convert.ToInt32(txtBale.Text.Trim());
                    }

                    TextBox txtBanwa = row.FindControl("txtBanwa") as TextBox;
                    int? tBanwa = null;
                    if (txtBanwa != null)
                    {
                        tBanwa = string.IsNullOrEmpty(txtBanwa.Text.Trim()) ? (int?)null : Convert.ToInt32(txtBanwa.Text.Trim());
                    }
                    Guid userId = ROWCACommon.GetCurrentUserId();
                    int projectId = Convert.ToInt32(ddlProjects.SelectedValue);

                    if (tBale != null || tBanwa != null)
                    {
                        DBContext.Add("InsertUserActivityDataTest", new object[] { 2, pId, userId, projectId, DBNull.Value });
                    }
                }
            }
        }
    }
}