using System.IO;
using System.Web.Services;
using Svg;

namespace SRFROWCA
{
    /// <summary>
    /// Save Chart svg (passed as argument in SaveSVG folder) and create an image on that svg
    /// </summary>
    [WebService(Namespace = "http://rowca.oasiswebservice.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ChartSVGService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public string SaveSVG(string svg, string logFrameId, string durationType, string yearId, string chartType)
        {
            string result = "Success";

            string dir = CreateFolderForFiles();

            // Use LogFrameId to save
            string fileName = dir + "\\" + chartType + logFrameId;

            if (!string.IsNullOrEmpty(durationType) && !string.IsNullOrEmpty(yearId))
            {
                fileName += "__" + durationType + "-" + yearId;
            }

            CreateSVG(svg, fileName);
            CreateImage(fileName);

            return result;
        }

        private string CreateFolderForFiles()
        {
            string dir = Server.MapPath("~/GeneratedChartFiles");

            // Concat sessionid with path to generate seperate
            // folder for each user.
            dir += "\\" + Session.SessionID.ToString();

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        // Read svg file from specified location and
        // create image using svg.dll
        private void CreateImage(string path)
        {
            string imagePath = path + ".jpg";
            if (File.Exists(imagePath)) return;
            using (FileStream fileStream = File.OpenRead(path + ".svg"))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.SetLength(fileStream.Length);
                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);

                    SvgDocument svgDocument = SvgDocument.Open(memoryStream);
                    svgDocument.Draw().Save(path + ".jpg");
                }
            }
        }

        private void CreateSVG(string svg, string path)
        {
            string filePath = path + ".svg";

            // If file exists then no need to create again.
            if (File.Exists(filePath)) return;

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(svg);
                }
            }
        }
    }
}
