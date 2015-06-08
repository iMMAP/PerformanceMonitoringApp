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
using System.IO;

namespace SRFROWCA.Reports
{
    public partial class LoadCountryMaps : System.Web.UI.Page
    {
        public string url;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReport();
            }
        }

        private DataTable GetReportInfo(int countryReportId)
        {
            return DBContext.GetData("GetCountryMapInfo", new object[] { countryReportId, RC.SelectedSiteLanguageId });
        }
        private void LoadReport()
        {
            DataTable dt = GetReportInfo(Convert.ToInt32(Request.QueryString["id"])); 
            if (dt != null && dt.Rows.Count > 0)
            {
                url = dt.Rows[0]["MapURL"].ToString();
                if (url.IndexOf("http://") == -1)
                {
                    url = Master.BaseURL + "/orsmaps/" + url;
                }
                //ViewState["url"] = url.Substring(0,url.LastIndexOf('/')-1);                
                ltrlFileName.Text = url.Substring(url.LastIndexOf("/")+1);
                ViewState["filename"] = ltrlFileName.Text;
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["filename"].ToString());
            Response.TransmitFile(Server.MapPath("~/ORSMaps/") + ViewState["filename"].ToString());
            Response.End(); 
        }
    }
    




}





