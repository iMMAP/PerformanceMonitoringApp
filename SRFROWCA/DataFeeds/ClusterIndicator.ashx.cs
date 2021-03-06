﻿using BusinessLogic;
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
    /// Summary description for ClusterIndicator
    /// </summary>
    public class ClusterIndicator : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("ClusterIndicatorFeed", param);

            string format = "xml";
            if (!string.IsNullOrEmpty(context.Request["format"]))
            {
                format = context.Request["format"].ToString();
            }

            if (format == "json")
            {
                context.Response.ContentType = "text/plain";
                string strJson = SRFROWCA.Common.DataTableToJson.DataTableToJsonBySerializer(dt);
                context.Response.Write(strJson);
            }
            else
            {
                context.Response.ContentType = "text/xml";
                ds = dt.DataSet;
                ds.DataSetName = "OutputIndicators";
                ds.Tables[0].TableName = "OutputIndicator";
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
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                int.TryParse(context.Request["country"].ToString(), out val);
            }
            int? countryId = val > 0 ? val : (int?)null;

            val = 0;
            if (!string.IsNullOrEmpty(context.Request["cluster"]))
            {
                int.TryParse(context.Request["cluster"].ToString(), out val);
            }
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            int? isReg = null;
            if (context.Request["reg"] != null)
            {
                string queryVal = context.Request["reg"].ToString();
                if (queryVal == "No" || queryVal == "no" || queryVal == "n" || queryVal == "NO" || queryVal == "0")
                    isReg = 0;

                if (queryVal == "Yes" || queryVal == "yes" || queryVal == "y" || queryVal == "YES" || queryVal == "1")
                    isReg = 1;
            }

            int? yearId = (int)RC.Year._Current;
            if (context.Request["year"] != null)
            {
                val = 0;
                int.TryParse(context.Request["year"].ToString(), out val);

                RC.Year yearEnum;
                if (Enum.TryParse("_" + val.ToString(), out yearEnum))
                    yearId = (int)yearEnum;

                if (yearId <= 0)
                    yearId = (int)RC.Year._Current;
            }

            return new object[] { countryId, clusterId, yearId, isReg};
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