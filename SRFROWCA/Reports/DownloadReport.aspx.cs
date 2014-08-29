using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            

          
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            rvCountry.ServerReport.ReportPath = "/reports/countryactivities";
            ReportParameter[] RptParameters = new ReportParameter[1];
            RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"].ToString());          
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            byte[] bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=CountryActivity.pdf");
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }
    }
}