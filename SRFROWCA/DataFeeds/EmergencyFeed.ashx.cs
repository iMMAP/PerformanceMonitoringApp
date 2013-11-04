using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BusinessLogic;
using System.IO;

namespace SRFROWCA.Admin.DataFeeds
{
    /// <summary>
    /// Summary description for EmergencyFeed
    /// </summary>
    public class EmergencyFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("GetAllEmergenciesName");
            // reutrn xml of datatable.
            context.Response.Write(FeedXML.GetData(dt));
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