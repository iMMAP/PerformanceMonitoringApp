using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Text;
using System.Data;
using BusinessLogic;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace SRFROWCA
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
     [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        [WebMethod]
        public static string GetTotalTargetAchieved()
        {
            DataTable dt = DBContext.GetData("GetMonthlyTargetAndAchieved");
            return JsonConvert.SerializeObject(dt, Formatting.None);
        }

        [WebMethod]
        public static string GetTargetAchievedByLocation()
        {
            DataTable dt = DBContext.GetData("GetTargetAndAchievedByMonthAndLocationPivot");
            //return JsonConvert.SerializeObject(dt, Formatting.None);
            return "";
        }

        [WebMethod]
        public static string SaveSVG(string svg)
        {
            string result = "Success";
            TextWriter tw = new StreamWriter("E:\\log4.txt", true);
            try
            {

                tw.Write("in log \n");
                string pathToSave = "Your path";
                FileStream fs = new FileStream(pathToSave + "temp.svg", FileMode.Create, FileAccess.Write);
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(svg);
                s.Close();
                fs.Close();

                System.Diagnostics.Process objProcess;
                objProcess = new System.Diagnostics.Process();
                objProcess.StartInfo.Arguments = "-f " + pathToSave + "temp.svg" + " -e " + pathToSave + "temp.png";
                objProcess.StartInfo.FileName = "e:\\inkscape.exe";
                objProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                objProcess.Start();
                //'Wait until it's finished
                objProcess.WaitForExit();
                //'Exitcode as String
                Console.WriteLine(objProcess.ExitCode.ToString());
                objProcess.Close();
                tw.WriteLine("end of function");

            }
            catch (Exception e)
            {
                tw.WriteLine(e.ToString());
            }

            return result;
        }
    }
}
