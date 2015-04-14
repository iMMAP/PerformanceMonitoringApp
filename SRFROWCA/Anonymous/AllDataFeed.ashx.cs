using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Anonymous
{
    /// <summary>
    /// Summary description for AllDataFeed
    /// </summary>
    public class AllDataFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            
            object[] param = GetParamValues(context);
            DataTable dt = DBContext.GetData("GetAllTasksDataReport2015", param);
            RemoveColumnsFromDataTable(dt);

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
                ds.DataSetName = "Records";
                ds.Tables[0].TableName = "Record";
                context.Response.Write(GetReportData(ds));
            }
        }

        // Make xml of datatable and return.
        private string GetReportData(DataSet dt)
        {
            using (var sw = new StringWriter())
            {
                dt.WriteXml(sw);
                return sw.ToString();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        
        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues(HttpContext context)
        {
            string monthIds = null;
            if (!string.IsNullOrEmpty(context.Request["month"]))
            {
                monthIds = context.Request["month"].ToString();
            }

            string locationIds = null;
            if (!string.IsNullOrEmpty(context.Request["country"]))
            {
                locationIds = context.Request["country"].ToString();
            }

            string clusterIds = null;
            if (!string.IsNullOrEmpty(context.Request["cluster"]))
            {
                clusterIds = context.Request["cluster"].ToString();
            }

            string orgIds = null;
            if (!string.IsNullOrEmpty(context.Request["org"] ))
            {
                orgIds = context.Request["org"].ToString();
            }

            string objIds = null;
            if (!string.IsNullOrEmpty(context.Request["obj"]))
            {
                objIds = context.Request["obj"].ToString();
            }

            string projectIds = null;
            if (!string.IsNullOrEmpty(context.Request["project"]))
            {
                projectIds = context.Request["project"].ToString();
            }

            int? fromMonth = null;
            int? toMonth = null;
            int? funded = null;
            int? notFunded = null;

            int val = 0;
            if (!string.IsNullOrEmpty(context.Request["ops"]))
            {
                int.TryParse(context.Request["ops"].ToString(), out val);
            }

            int? isOPS = val > 0 ? val : (int?)null;

            int? isApproved = 1;
            int pageSize = 0;
            int pageIndex = 0;
            int langId = 2;

            string lng = "fr";
            if (!string.IsNullOrEmpty(context.Request["lng"]))
            {
                lng = context.Request["lng"].ToString();
            }

            if (lng == "en")
            {
                langId = 1;
            }

            int emergencyId = 3;      

           return new object[] {monthIds, locationIds, clusterIds, orgIds, 
                                    objIds, projectIds,fromMonth, toMonth, funded, notFunded,
                                    isOPS, isApproved, pageIndex, pageSize, Convert.ToInt32(0), langId, emergencyId };
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("ObjectiveId");            
            dt.Columns.Remove("MonthId");
            dt.Columns.Remove("cnt");
            dt.Columns.Remove("ReportDetailId");
            dt.Columns.Remove("ActivityId");
            dt.Columns.Remove("IndicatorId");
            dt.Columns.Remove("ProjectId");
            dt.Columns.Remove("IsApproved");
            dt.Columns.Remove("Comments");

        }
    }
}