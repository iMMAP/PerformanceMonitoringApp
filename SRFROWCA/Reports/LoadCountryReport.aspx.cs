using BusinessLogic;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Net;
using System.Security.Principal;
using SRFROWCA.Common;

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
                rvCountry.ServerReport.ReportServerUrl = zznew System.Uri("http://win-78sij2cjpjj/Reportserver");//new System.Uri(dt.Rows[0]["SSRSURL"].ToString());
                rvCountry.ServerReport.ReportPath = dt.Rows[0]["SSRSReportName"].ToString(); //"/reports/countryreport";
                ReportParameter[] RptParameters =  new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"]);
                rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
                rvCountry.ServerReport.SetParameters(RptParameters);               
                rvCountry.ServerReport.Refresh();
            }
        }
    }
    




}





