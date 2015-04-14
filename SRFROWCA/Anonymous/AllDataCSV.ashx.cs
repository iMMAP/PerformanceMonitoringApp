using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BusinessLogic;
using System.Text;
using SRFROWCA.Common;


namespace SRFROWCA.Anonymous
{
    /// <summary>
    /// Summary description for AllDataCSV
    /// </summary>
    public class AllDataCSV : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/ms-excel";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedData.csv;");
            object[] param = GetParamValues(context);
            DataTable dt = DBContext.GetData("GetAllTasksDataReport2015", param);
            RemoveColumnsFromDataTable(dt);
            string strCSV = DataTableToCSV.ToCSV(dt);
            context.Response.Write(strCSV);
        }

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
        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}