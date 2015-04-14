using BusinessLogic;
using SRFROWCA.Admin.DataFeeds;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.DataFeeds
{
    /// <summary>
    /// Summary description for Clusters
    /// </summary>
    public class Clusters : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetClustersWithEmgIds", param);
            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
            {
                format = context.Request["format"].ToString();
            }

            if (format == "json")
            {
                context.Response.ContentType = "text/plain";
                string strJson = DataTableToJson.DataTableToJsonBySerializer(dt);
                context.Response.Write(strJson);
            }
            else
            {
                context.Response.ContentType = "text/xml";            
                DataSet ds = new DataSet();
                ds = dt.DataSet;
                ds.DataSetName = "Clusters";
                ds.Tables[0].TableName = "Cluster";
                context.Response.Write(GetReportData(ds));
            }
        }

        private string GetReportData(DataSet ds)
        {
            using (var sw = new StringWriter())
            {
                ds.WriteXml(sw);
                return sw.ToString();
            }
        }

        private object[] GetReportParam(HttpContext context)
        {
            int val = 0;
            if (context.Request["emg"] != null)
            {
                int.TryParse(context.Request["emg"].ToString(), out val);
            }
            int emg = val > 0 && val < 4 ? val : 3;

            return new object[] { emg };
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}