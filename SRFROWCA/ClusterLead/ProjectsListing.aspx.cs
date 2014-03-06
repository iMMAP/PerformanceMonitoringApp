using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectsListing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            UserInfo.UserProfileInfo();
            LoadProjects();

        }

        protected void gvProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewProject")
            {
                Session["ViewProjectId"] = e.CommandArgument.ToString();
                Response.Redirect("~/ClusterLead/ProjectDetails.aspx");
            }
        }

        private void LoadProjects()
        {
            DataTable dt = DBContext.GetData("GetProjects", new object[] {UserInfo.GetCountry, UserInfo.GetCluster, 1});
            gvProjects.DataSource = dt;
            gvProjects.DataBind();
        }
    }
}