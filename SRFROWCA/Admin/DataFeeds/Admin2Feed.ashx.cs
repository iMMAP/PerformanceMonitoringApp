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
    /// Summary description for Admin2Feed
    /// </summary>
    public class Admin2Feed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("GetAllAdmin1Locations");

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