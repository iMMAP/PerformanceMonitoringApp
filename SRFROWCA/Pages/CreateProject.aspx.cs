using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.Pages
{
    public partial class CreateProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateClusters();
            PopulateProjects();
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlCluster, (int)ROWCACommon.SiteLanguage.English);
        }

        private void PopulateProjects()
        {
            int locationId = 0;
            Guid userId = new Guid();
            DataTable dt = ROWCACommon.GetUserDetails();
            if (dt.Rows.Count > 0)
            {
                locationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
                userId = new Guid(dt.Rows[0]["UserId"].ToString());
            }

            rblProjects.DataValueField = "ORSProjectId";
            rblProjects.DataTextField = "ProjectTitle";
            rblProjects.DataSource = DBContext.GetData("GetORSProjectsOfUser", new object[] { locationId, userId });
            rblProjects.DataBind();
        }

        protected void rblProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            int projectId = Convert.ToInt32(rblProjects.SelectedValue);
            DataTable dt = DBContext.GetData("GetORSProjectDetail", new object[] { projectId });

            if (dt.Rows.Count > 0)
            {
                txtProjectTitle.Text = dt.Rows[0]["ProjectTitle"].ToString();
                txtProjectObjective.Text = dt.Rows[0]["ProjectObjective"].ToString();
                ddlCluster.SelectedValue = dt.Rows[0]["ClusterId"].ToString();
                txtFromDate.Text = dt.Rows[0]["ProjectStartDate"].ToString();
                txtToDate.Text = dt.Rows[0]["ProjectEndDate"].ToString();
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            rblProjects.ClearSelection();
            txtProjectTitle.Text = "";
            txtProjectObjective.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            Response.Redirect("~/Pages/AddActivities.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            ShowMessage("Your Data Saved Successfuly!");
        }

        protected void btnManageActivities_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/ManageActivities.aspx");
        }


        private void Save()
        {
            int locationId = 0;
            DataTable dt = ROWCACommon.GetUserDetails();
            if (dt.Rows.Count > 0)
            {
                locationId = Convert.ToInt32(dt.Rows[0]["LocationId"].ToString());
            }
            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            int clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            DateTime startDate = Convert.ToDateTime(txtFromDate.Text);
            string endDate = txtToDate.Text;

            Guid userId = ROWCACommon.GetCurrentUserId();
            int projectId = 0;
            int.TryParse(rblProjects.SelectedValue, out projectId);
            //string projectCode = "ORS";
            if (projectId > 0)
            {
                DBContext.Add("UpdateProject", new object[] { projectId, title, objective, clusterId, locationId, startDate, endDate, userId, DBNull.Value });
            }
            else
            {
                DBContext.Add("InsertProject", new object[] { title, objective, clusterId, locationId, startDate, endDate, userId, DBNull.Value });
            }
        }

        private void ShowMessage(string message, ROWCACommon.NotificationType notificationType = ROWCACommon.NotificationType.Success)
        {
            ROWCACommon.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }
    }
}