using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using System.Data;

namespace SRFROWCA.Reports
{
    public partial class ProjectByClusters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClusters();
            }
        }

        private void LoadClusters()
        {
            gvClusters.DataSource = DBContext.GetData("[GetProjectClusters]", new object[] { RC.SelectedSiteLanguageId });
            gvClusters.DataBind();
        }

        protected DataTable GetProjectData(int clusterId)
        {
            return DBContext.GetData("[GetProjectsOnCluster]", new object[] {clusterId, RC.SelectedSiteLanguageId });
        }

        protected void gvClusters_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Return if this is not a datarow
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            GridView gvActivities = e.Row.FindControl("gvActivities") as GridView;
            if (gvActivities != null)
            {
                DataRowView dr = e.Row.DataItem as DataRowView;
                int clusterId = 0;
                int.TryParse(dr["ClusterId"].ToString(), out clusterId);

                // Get all activities and bind grid.
                DataTable dt = GetProjectData(clusterId);
                gvActivities.DataSource = dt;
                gvActivities.DataBind();
            }
        }
    }
}