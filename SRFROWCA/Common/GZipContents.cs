using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO.Compression;
using System.Text;
using System.IO;

namespace SRFROWCA.Common
{
    public static class GZipContents
    {
        public static void GZipOutput()
        {
            HttpContext context = HttpContext.Current;
            context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
            HttpContext.Current.Response.AppendHeader("Content-encoding", "gzip");
            HttpContext.Current.Response.Cache.VaryByHeaders["Accept-encoding"] = true;
            HttpContext.Current.Response.Charset = "utf-8";
        }
    }
}