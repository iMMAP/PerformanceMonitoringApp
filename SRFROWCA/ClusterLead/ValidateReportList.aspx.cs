using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ValidateReportList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
                LoadReports();
            }
        }

        private void LoadReports()
        {
            gvReports.DataSource = DBContext.GetData("GetCountryReports", new object[] { UserInfo.EmergencyCountry, UserInfo.EmergencyCluster });
            gvReports.DataBind();
        }

        protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewReport")
            {
                int rowIndex = int.Parse(e.CommandArgument.ToString());

                int reportId = 0;
                int.TryParse(this.gvReports.DataKeys[rowIndex]["ReportId"].ToString(), out reportId);

                Response.Redirect("ValidateIndicators.aspx?rid=" + reportId);
            }
        }
    }
}