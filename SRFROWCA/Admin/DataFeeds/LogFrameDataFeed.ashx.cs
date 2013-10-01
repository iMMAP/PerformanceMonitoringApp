using System.Data;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Admin.DataFeeds
{
    /// <summary>
    /// Summary description for LogFrameDataFeed
    /// </summary>
    public class LogFrameDataFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("GetAllLogFrameData");
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