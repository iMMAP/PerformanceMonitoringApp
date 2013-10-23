using System;
using System.Web;
using System.IO.Compression;
namespace HttpModules3W
{
    public class GZIPCompress : IHttpModule
    {
        public void Init(HttpApplication httpApp)
        {
            httpApp.BeginRequest += new EventHandler(OnBeginRequest);
        }

        // Record the time of the begin request event.
        public void OnBeginRequest(Object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
        }

        public void Dispose()
        {

        }
    }
}
