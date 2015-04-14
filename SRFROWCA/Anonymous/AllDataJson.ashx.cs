using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace SRFROWCA.Anonymous
{
    /// <summary>
    /// Summary description for AllDataJson
    /// </summary>
    public class AllDataJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            object[] param = GetParamValues(context);
            DataTable dt = DBContext.GetData("GetAllTasksDataReport2015", param);
            RemoveColumnsFromDataTable(dt);
            string strJson = DataTableToJson.DataTableToJsonBySerializer(dt);
            context.Response.Write(strJson);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Make xml of datatable and return.
        private string GetReportData(DataTable dt)
        {
            using (var sw = new StringWriter())
            {
                dt.WriteXml(sw);
                return sw.ToString();
            }
        }

        //public bool IsReusable
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues(HttpContext context)
        {
            string monthIds = null;
            if (!string.IsNullOrEmpty(context.Request["month"]))
            {
                monthIds = context.Request["month"].ToString();
            }

            string locationIds = null;
            string clusterIds = null;
            string orgIds = null;
            string objIds = null;
            string projectIds = null;
            int? fromMonth = null;
            int? toMonth = null;
            int? funded = null;
            int? notFunded = null;
            int? isOPS = null;
            int? isApproved = null;
            int pageSize = 0;
            int pageIndex = 0;
            int langId = 1;
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