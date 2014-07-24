using BusinessLogic;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Reports
{
    public partial class LoadCountryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
        }

        private DataTable GetReportInfo(int countryReportId)
        {
            return DBContext.GetData("GetCountryReportInfo", new object[] { countryReportId });
        }
        private void LoadReport()
        {
            DataTable dt = GetReportInfo(Convert.ToInt32(Request.QueryString["id"]));
            if (dt != null && dt.Rows.Count > 0)
            {
                rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://54.83.26.190/Reportserver"); //new System.Uri(dt.Rows[0]["SSRSURL"].ToString());
                rvCountry.ServerReport.ReportPath = "/reports/countryreport";//dt.Rows[0]["SSRSReportName"].ToString();
                ReportParameter[] RptParameters =  new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"]);
                //rvCountry.ServerReport.ReportServerCredentials = new Microsoft.Reporting.ReportServerCredentials("uName", "PassWORD", "doMain");
                rvCountry.ServerReport.SetParameters(RptParameters);
                rvCountry.ServerReport.Refresh();
            }
        }
    }
}