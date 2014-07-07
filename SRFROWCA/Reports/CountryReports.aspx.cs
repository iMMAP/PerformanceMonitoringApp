using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Reports
{
    public partial class CountryReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            lblCountryName.Text = "Mali - 2014";
            LoadReportTypes();
        }

        private void LoadReportTypes()
        {
            rptReportTypes.DataSource = GetReportTypes();
            rptReportTypes.DataBind();
        }

        private DataTable GetReportTypes()
        {
            DataTable dt = new DataTable();
            if (Request.QueryString["cid"] != null)
            {
                int countryId = 0;
                int.TryParse(Request.QueryString["cid"].ToString(), out countryId);
                if (countryId > 0)
                {
                    dt = DBContext.GetData("GetPublicCountryReportTypes", new object[] { countryId, RC.SelectedSiteLanguageId });
                }
            }

            return dt;
        }

        private DataTable GetReports(int countryId, int reportTypeId)
        {
            return DBContext.GetData("GetPublicCountryReports", new object[] { countryId, reportTypeId, RC.SelectedSiteLanguageId });
        }

        protected void rptReportTypes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField hfReportTypeId = e.Item.FindControl("hfReportTypeTitle") as HiddenField;
            int reportId = 0;
            int.TryParse(hfReportTypeId.Value, out reportId);

            Repeater rptReports = e.Item.FindControl("rptReports") as Repeater;

            if (Request.QueryString["cid"] != null)
            {
                int countryId = 0;
                int.TryParse(Request.QueryString["cid"].ToString(), out countryId);
                if (countryId > 0)
                {
                    if (rptReports != null && reportId > 0)
                    {
                        rptReports.DataSource = GetReports(countryId, reportId);
                        rptReports.DataBind();
                    }
                }
            }
        }
    }
}