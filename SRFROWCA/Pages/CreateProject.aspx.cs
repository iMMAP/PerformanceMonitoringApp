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
            UserInfo.UserProfileInfo();
            PopulateClusters();
            PopulateProjects();
        }

        private void PopulateClusters()
        {
            UI.FillClusters(ddlCluster, (int)RC.SiteLanguage.English);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }

        private void PopulateProjects()
        {
            rblProjects.DataValueField = "ORSProjectId";
            rblProjects.DataTextField = "ProjectTitle";
            rblProjects.DataSource = GetProjects();
            rblProjects.DataBind();
        }

        private void SelectProject()
        {
            if (ORSProjectId > 0)
            {
                rblProjects.SelectedValue = ORSProjectId.ToString();
            }
        }

        private DataTable GetProjects()
        {
            Guid userId = RC.GetCurrentUserId;
            int locationId = UserInfo.GetCountry;
            return DBContext.GetData("GetORSProjectsOfUser", new object[] { locationId, userId });
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
                ltrlProjectCode.Text = dt.Rows[0]["ProjectCode"].ToString();
            }
        }

        protected void btnCreateProject_Click(object sender, EventArgs e)
        {
            rblProjects.ClearSelection();
            txtProjectTitle.Text = "";
            txtProjectObjective.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ltrlProjectCode.Text = "";
            ddlCluster.SelectedValue = "0";
            ORSProjectId = 0;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Save();
            Response.Redirect("~/Pages/AddActivities.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();
            PopulateProjects();
            SelectProject();
            ShowMessage("Your Data Saved Successfuly!");
        }

        protected void btnManageActivities_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/ManageActivities.aspx");
        }

        private void Save()
        {
            int locationId = UserInfo.GetCountry;
            int orgId = UserInfo.GetOrganization;
            string title = txtProjectTitle.Text.Trim();
            string objective = txtProjectObjective.Text.Trim();
            int clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            string startDate = txtFromDate.Text.Trim();
            string endDate = txtToDate.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            int projectId = RC.GetSelectedIntVal(rblProjects);

            if (locationId > 0 && clusterId > 0)
            {
                if (projectId > 0)
                {
                    DBContext.Add("UpdateProject", new object[] { projectId, title, objective, 
                                                            clusterId, locationId, startDate,
                                                            endDate, userId, DBNull.Value });
                }
                else
                {
                    ORSProjectId = DBContext.Add("InsertProject", new object[] { title, objective, clusterId, locationId,
                                                             orgId, startDate, endDate, userId, DBNull.Value });
                }
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success)
        {
            RC.ShowMessage(this.Page, typeof(Page), UniqueID, message, notificationType, true, 500);
        }

        public int ORSProjectId
        {
            get
            {
                int orsProjectId = 0;
                if (ViewState["ORSProjectId"] != null)
                {
                    int.TryParse(ViewState["ORSProjectId"].ToString(), out orsProjectId);
                }

                return orsProjectId;
            }
            set
            {
                ViewState["ORSProjectId"] = value.ToString();
            }
        }
    }
}