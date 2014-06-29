using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectDetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            //if (Session["ViewProjectId"] != null)
            if (Request.QueryString["pid"] != null)
            {
                GetProjectDetails();
            }
        }

        private void GetProjectDetails()
        {
            //int projectId = Convert.ToInt32(Session["ViewProjectId"].ToString());
            int projectId = 0;
            int.TryParse(Request.QueryString["pid"].ToString(), out projectId);
            DataTable dt = DBContext.GetData("GetProjectDetails", new object[] { projectId, 1 });
            fvProjects.DataSource = dt;
            fvProjects.DataBind();
        }

        protected void fvProjects_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {
            
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/ProjectReports.aspx");
        }
    }
}