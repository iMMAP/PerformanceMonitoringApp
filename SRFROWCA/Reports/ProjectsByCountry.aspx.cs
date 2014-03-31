using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Common;

namespace SRFROWCA.Reports
{
    public partial class ProjectsByCountry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCountries();
            }
        }

        private void LoadCountries()
        {
            gvClusters.DataSource = DBContext.GetData("[GetProjectCountries]");
            gvClusters.DataBind();
        }

        protected DataTable GetProjectData(int countryId)
        {
            return DBContext.GetData("[GetProjectsOnCountry]", new object[] { countryId, RC.SelectedSiteLanguageId });
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
                int.TryParse(dr["LocationId"].ToString(), out clusterId);

                // Get all activities and bind grid.
                DataTable dt = GetProjectData(clusterId);
                gvActivities.DataSource = dt;
                gvActivities.DataBind();
            }
        }
    }
}