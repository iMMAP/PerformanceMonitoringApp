using System.Data;
using System.IO;
using System.Web;
using BusinessLogic;

namespace SRFROWCA.Admin.DataFeeds
{
    /// <summary>
    /// Summary description for OrganizationsFeed
    /// </summary>
    public class OrganizationsFeed : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            DataTable dt = DBContext.GetData("GetAllOrganizations");

            // reutrn xml of datatable.
            context.Response.Write(GetData(dt));
        }

        private string GetData(DataTable dt)
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
    }
}