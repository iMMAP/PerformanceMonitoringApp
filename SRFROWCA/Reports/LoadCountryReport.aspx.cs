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
                rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver"); //new System.Uri(dt.Rows[0]["SSRSURL"].ToString());
                rvCountry.ServerReport.ReportPath = "/reports/countryreport";//dt.Rows[0]["SSRSReportName"].ToString();
                ReportParameter[] RptParameters =  new ReportParameter[1];
                RptParameters[0] = new ReportParameter("CountryId", Request.QueryString["cid"]);
                rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
                rvCountry.ServerReport.SetParameters(RptParameters);
                rvCountry.ServerReport.Refresh();
            }
        }
    }
    


public class ReportServerCredentials : IReportServerCredentials
{
private string reportServerUserName;
private string reportServerPassword;
private string reportServerDomain;

public ReportServerCredentials(string userName, string password, string domain)
{
reportServerUserName = userName;
reportServerPassword = password;
reportServerDomain = domain;
}

public WindowsIdentity ImpersonationUser
{
get
{
// Use default identity.
return null;
}
}

public ICredentials NetworkCredentials
{
get
{
// Use default identity.
return new NetworkCredential(reportServerUserName, reportServerPassword, reportServerDomain);
}
}

public void New(string userName, string password, string domain)
{
reportServerUserName = userName;
reportServerPassword = password;
reportServerDomain = domain;
}

public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
{
// Do not use forms credentials to authenticate.
authCookie = null;
user = null;
password = null;
authority = null;

return false;
} 
}

}





