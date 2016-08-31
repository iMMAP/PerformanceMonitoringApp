using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SRFROWCA.api.v2.Framework
{
    /// <summary>
    /// Summary description for activitiesapi
    /// </summary>
    public class activitiesapi : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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