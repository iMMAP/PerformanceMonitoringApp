using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;

namespace SRFROWCA.Reports
{
    public partial class DownloadReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ExportToPDF();
        }

        private void ExportToPDF()
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            int tempVal = 0;


            string reportType = Request.QueryString["Type"].ToString();
            string fileName = "";
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            //rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://54.83.26.190/Reportserver");
            ReportParameter[] RptParameters = null;
            if (reportType == "3W")
            {
                RptParameters = new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"].ToString());          
                rvCountry.ServerReport.ReportPath = "/reports/countryactivities";
                fileName = "ORS3W-" + Request.QueryString["cName"].ToString() + ".pdf";
            }
            else if (reportType == "4W")
            {
                RptParameters = new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"].ToString());          
                rvCountry.ServerReport.ReportPath = "/reports/countryactivities4w";
                fileName = "ORS4W-" + Request.QueryString["cName"].ToString() + ".pdf";
            }
            else if (reportType == "5")
            {
                DataTable dt = GetReportInfo(Convert.ToInt32(Request.QueryString["cid"]));
                RptParameters = new ReportParameter[4];
                RptParameters[0] = new ReportParameter("emgLocationId", dt.Rows[0]["EmergencyLocationId"].ToString());
                RptParameters[1] = new ReportParameter("emgClusterId", dt.Rows[0]["EmergencyClusterId"].ToString());
                RptParameters[2] = new ReportParameter("langId", ((int)RC.SiteLanguage.English).ToString());
                RptParameters[3] = new ReportParameter("yearId", "10");
                rvCountry.ServerReport.ReportPath = "/reports/countryindicators";
                fileName = "CountryIndicators-" + Request.QueryString["cName"].ToString() + "-" + GetClusterName((int)dt.Rows[0]["EmergencyClusterId"]) + ".pdf";
            }
            else
            {
                DataTable dt = GetReportInfo(Convert.ToInt32(Request.QueryString["cid"]));
                rvCountry.ServerReport.ReportPath = dt.Rows[0]["SSRSReportName"].ToString(); //"/reports/countryreport";
                RptParameters = new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["countryId"]);
                fileName = dt.Rows[0]["ReportTitle"].ToString() + Request.QueryString["cName"].ToString() + ".pdf";
            }
            
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            byte[] bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);         
            

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }

        private DataTable GetReportInfo(int countryReportId)
        {
            return DBContext.GetData("GetCountryReportInfo", new object[] { countryReportId });
        }

        private string GetClusterName(int emergencyClusterId)
        {
            DataTable dt = DBContext.GetData("GetClusterNameByEmgClusterId", new object[] { emergencyClusterId });
            return dt.Rows[0]["ClusterName"].ToString();
        }
    }
}