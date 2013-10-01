using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace SRFROWCA
{
    /// <summary>
    /// Summary description for ToPNG
    /// </summary>
    public class ToPNG : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/png";

            string PngRelativeDirectory = "E:\\img2\\";

            String svgXml = "E:\\img\\drawing.svg"; // GetSvgImageXml(context);
            string svgFileName = "E:\\img\\drawing.svg";  //GetSvgFileName(context);
            using (StreamWriter writer = new StreamWriter(svgFileName, false))
            {
                writer.Write(svgXml);
            }

            string pngFileName = "abc.png";// GetPngFileName(context);

            string inkscapeArgs =
             "-f " + svgFileName + " -e " + PngRelativeDirectory + pngFileName;

            Process inkscape = Process.Start(
              new ProcessStartInfo(
               "e:\\Inkscape\\inkscape.exe",
               inkscapeArgs));
            inkscape.WaitForExit(3000);
            context.Server.Transfer(PngRelativeDirectory + pngFileName);
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