using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Anonymous
{
    /// <summary>
    /// Summary description for AllDataFeed
    /// </summary>
    public class AllDataFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            DataTable dt = GetData();
            RemoveColumnsFromDataTable(dt);
            context.Response.Write(GetReportData(dt));
        }

        private DataTable GetData()
        {
            object[] paramValue = GetParamValues();
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Get data from db.
        private DataTable GetReportData()
        {
            object[] paramValue = GetParamValues();
            return DBContext.GetData("GetAllTasksDataReport", paramValue);
        }

        // Get filter criteria and create an object with parameter values.
        private object[] GetParamValues()
        {
            string monthIds = null;//RC.GetSelectedValues(ddlMonth);
            string locationIds = null; //GetLocationIds();
            //if (string.IsNullOrEmpty(locationIds))
            //{
            //    if (!string.IsNullOrEmpty(lblCountry.Text.Trim()))
            //    {
            //        locationIds = UserInfo.Country.ToString();
            //    }
            //}

            string clusterIds = null; //RC.GetSelectedValues(ddlClusters);
            string orgIds = null; //RC.GetSelectedValues(ddlOrganizations);
            string objIds = null; //RC.GetSelectedValues(ddlObjectives);
            string prIds = null; //RC.GetSelectedValues(ddlPriority);
            string actIds = null; //RC.GetSelectedValues(ddlActivities);
            string indIds = null; //RC.GetSelectedValues(ddlIndicators);
            string projectIds = null; //RC.GetSelectedValues(ddlProjects);
            int? fromMonth = null; //!string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? Convert.ToInt32(txtFromDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? toMonth = null; //!string.IsNullOrEmpty(txtToDate.Text.Trim()) ? Convert.ToInt32(txtToDate.Text.Trim().Substring(0, 2)) : (int?)null;
            int? regionalInd = null; //cbRegional.Checked ? 1 : (int?)null;
            int? countryInd = null; //cbCountry.Checked ? 1 : (int?)null;
            int? funded = null; //cbFunded.Checked ? 1 : (int?)null;
            int? notFunded = null; //cbNotFunded.Checked ? 1 : (int?)null;
            int? isOPS = null; //cbOPSProjects.Checked && cbORSProjects.Checked ? null : cbOPSProjects.Checked ? 1 : cbORSProjects.Checked ? 0 : (int?)null;

            int? isApproved = 0;
            int pageSize = 0;
            int pageIndex = 0;
            int langId = 1;
            //SetHFQueryString(monthIds, locationIds, clusterIds, orgIds);

            return new object[] {monthIds, locationIds, clusterIds, orgIds, 
                                    objIds, prIds, actIds, indIds, projectIds,
                                    fromMonth, toMonth, regionalInd, countryInd, funded, notFunded,
                                    isOPS, isApproved, pageIndex, pageSize, Convert.ToInt32(0), langId };
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            dt.Columns.Remove("rnumber");
            dt.Columns.Remove("ObjectiveId");
            dt.Columns.Remove("PriorityId");
            dt.Columns.Remove("MonthId");
            dt.Columns.Remove("cnt");
            dt.Columns.Remove("ReportDetailId");
            dt.Columns.Remove("ActivityId");
            dt.Columns.Remove("IndicatorId");
            dt.Columns.Remove("ProjectId");
            dt.Columns.Remove("IsApproved");

        }
    }
}