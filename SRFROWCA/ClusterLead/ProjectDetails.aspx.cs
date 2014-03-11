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

            if (Session["ViewProjectId"] != null)
            {
                GetProjectDetails();
            }
        }

        private void GetProjectDetails()
        {
            int projectId = Convert.ToInt32(Session["ViewProjectId"].ToString());
            DataTable dt = DBContext.GetData("GetProjectDetails", new object[] { projectId, 1 });
            fvProjects.DataSource = dt;
            fvProjects.DataBind();
        }
    }
}