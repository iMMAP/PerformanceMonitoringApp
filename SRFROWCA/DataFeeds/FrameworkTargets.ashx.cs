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
    /// Summary description for FrameworkTargets
    /// </summary>
    public class FrameworkTargets : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            object[] param = GetReportParam(context);
            DataTable dt = DBContext.GetData("GetIndicatorTargets", param);

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
                ds.DataSetName = "IndicatorTargets";
                ds.Tables[0].TableName = "IndicatorTarget";
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
            if (context.Request["cluster"] != null)
            {
                int.TryParse(context.Request["cluster"].ToString(), out val);
            }
            int? clusterId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["obj"] != null)
            {
                int.TryParse(context.Request["obj"].ToString(), out val);
            }
            int? objId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["act"] != null)
            {
                int.TryParse(context.Request["act"].ToString(), out val);
            }
            int? actId = val > 0 ? val : (int?)null;

            val = 0;
            if (context.Request["admin1"] != null)
            {
                int.TryParse(context.Request["admin1"].ToString(), out val);
            }
            int? admin1Id = val > 0 ? val : (int?)null;

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

            return new object[] { countryId, clusterId, objId, actId, admin1Id, yearId };
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