using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.OrsProject
{
    public partial class ProjectPartners : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSearch.UniqueID;
            if (IsPostBack) return;
            GetProjectId();
            hfProjectId.Value = ProjectId.ToString();
            SetProjectCode();
            LoadPartnerOrgs();
            LoadOrganizations();

        }

        private void SetProjectCode()
        {
            Project project = null;
            using (ORSEntities db = new ORSEntities())
            {
                project = db.Projects.FirstOrDefault(x => x.ProjectId == ProjectId);
            }

            if (project != null)
            {
                lblProjectCode.Text = "Project: " + project.ProjectCode;
            }
        }

        private void LoadPartnerOrgs()
        {
            DataTable dt = DBContext.GetData("GetProjectPartners", new object[] { ProjectId });
            gvPartnerOrgs.DataSource = dt;
            gvPartnerOrgs.DataBind();
        }

        private void LoadOrganizations()
        {
            gvOrganization.DataSource = DBContext.GetData("GetOrganizationsToAddAsPartner", new object[] { ProjectId });
            gvOrganization.DataBind();
        }

        protected void gvPartnerOrgs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeletePartner")
            {
                int orgId = Convert.ToInt32(e.CommandArgument);
                DeleteProjectPartner(orgId);
            }
        }

        protected void gvOrganization_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddPartner")
            {
                int orgId = Convert.ToInt32(e.CommandArgument);
                AddProjectPartner(orgId);
                LoadPartnerOrgs();
                LoadOrganizations();
            }
        }

        private void AddProjectPartner(int orgId)
        {
            DBContext.Add("InsertProjectPartner", new object[] { ProjectId, orgId, RC.GetCurrentUserId, DBNull.Value });
        }

        private void DeleteProjectPartner(int orgId)
        {
            if (IsPartnerReportsExists(orgId))
            {
                ShowMessage("This partner can not be deleted. Reported data exists for this partner.", RC.NotificationType.Error, true, 3000);
            }
            else
            {
                DeletePartner(orgId);
                LoadPartnerOrgs();
                LoadOrganizations();
            }
        }

        private void DeletePartner(int orgId)
        {
            DBContext.Delete("DeleteProjectPartners", new object[] { ProjectId, orgId, DBNull.Value });
        }

        private bool IsPartnerReportsExists(int orgId)
        {
            return (DBContext.GetData("IsPartnerReportsExists", new object[] { ProjectId, orgId })).Rows.Count > 0;
        }
       
        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        private void GetProjectId()
        {
            int tempVal = 0;
            if (Request.QueryString["pid"] != null)
            {
                tempVal = 0;
                int.TryParse(Request.QueryString["pid"].ToString(), out tempVal);
                ProjectId = tempVal;
            }
        }

        private int ProjectId
        {
            get
            {
                int projectId = 0;
                if (ViewState["ProjectId"] != null)
                {
                    int.TryParse(ViewState["ProjectId"].ToString(), out projectId);
                }

                return projectId;
            }
            set
            {
                ViewState["ProjectId"] = value.ToString();
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