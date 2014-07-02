using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using SRFROWCA.Common;

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
            string projectID = "0";

            if (!string.IsNullOrEmpty(Request.QueryString["pid"]))
                projectID = Convert.ToString(Request.QueryString["pid"]);

            Response.Redirect("~/ClusterLead/ProjectReports.aspx?pid=" + projectID);
        }

        protected void btExportPDF_Click(object sender, EventArgs e)
        {
            int projectID = 0;

            if (Request.QueryString["pid"] != null)
                int.TryParse(Request.QueryString["pid"].ToString(), out projectID);

            DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { Convert.ToString(projectID), null, null });

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectID, null).ToArray());
            }
        }
    }
}