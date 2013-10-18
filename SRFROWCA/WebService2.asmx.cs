using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Svg;
using System.Collections.Generic;
using SRFROWCA.Common;


namespace SRFROWCA
{
    /// <summary>
    /// Summary description for WebService2
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService2 : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public string SaveSVGOnDisk(string svg, string logFrameId, string durationType, string yearId, string chartType)
        {
            string result = "Success";

            string dir = CreateFolderForFiles();

            // Use LogFrameId to save
            string fileName = dir + "\\" + chartType +  logFrameId;

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
            using (FileStream fs = new FileStream(path + ".svg", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(svg);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void GeneratePDF()
        {
            string path = CreateFolderForFiles() + "\\";

            string filePath = path + "Charts.pdf";

            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 8, 8, 14, 6);
            //MemoryStream outputStream = new MemoryStream()
            FileStream outputStream = new FileStream(filePath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
            document.Open();
            

            FileInfo[] imageFiles = new DirectoryInfo(path).GetFiles("*.jpg");

            XDocument logFrameDoc = XDocument.Load(path + "logframe.xml");
            //string logFrameType = GetAttributeValue(logFrameDoc.Element("LogFrames"), "LogFrame", "LogFrameType");
            string durationTypeName = GetAttributeValue(logFrameDoc.Element("LogFrames"), "LogFrame", "DurationTypeName");

            for (int i = 0; i < imageFiles.Length; i++)
            {
                string fileName = imageFiles[i].Name;
                if (!fileName.Contains('t'))
                {
                    int lastIndexOfUnderScore = fileName.LastIndexOf("__");
                    int lastIndexofHyphen = fileName.LastIndexOf('-');
                    int lastIndexOfPeriod = fileName.LastIndexOf(".");
                    int length = lastIndexOfUnderScore == -1 ? lastIndexOfPeriod : lastIndexOfUnderScore;

                    string logFrameId = fileName.Substring(1, length - 1);
                    string durationTypeId = lastIndexOfUnderScore == -1 ? null :
                        fileName.Substring(lastIndexOfUnderScore + 2, lastIndexofHyphen - (lastIndexOfUnderScore + 2));

                    string yearTyepId = lastIndexOfUnderScore == -1 ? null :
                        fileName.Substring(lastIndexofHyphen + 1, lastIndexOfPeriod - (lastIndexofHyphen + 1));

                    XElement xmlDoc = XElement.Load(path + "logframe.xml");
                    var logFrameElements =
                        from le in xmlDoc.Descendants("Data")
                        where (string)le.Attribute("Id").Value == logFrameId
                        select le.AncestorsAndSelf().Distinct();

                    LogFrameValues logFrameValues = null;
                    IEnumerable<XElement> elementsToUseInPDF = logFrameElements.Count() > 0 ? logFrameElements.FirstOrDefault() : null;
                    if (elementsToUseInPDF != null)
                    {
                        if (string.IsNullOrEmpty(durationTypeName))
                        {
                            logFrameValues = GetLogFrameValues(elementsToUseInPDF);
                        }
                        else
                        {
                            elementsToUseInPDF = GetElementChunk(logFrameElements, durationTypeName, durationTypeId, yearTyepId);
                            if (elementsToUseInPDF != null)
                            {
                                logFrameValues = GetLogFrameValues(elementsToUseInPDF);
                            }
                        }
                    }

                    if (logFrameValues != null)
                    {

                        WritePDF generatePDF = new WritePDF(logFrameValues, imageFiles[i].FullName);
                        generatePDF.GeneratePDF(document);

                    }
                }
            }
            
            document.Close();

            //DownLoadFile(filePath);

            Context.Response.ContentType = "Application/pdf";
            Context.Response.AppendHeader("content-disposition",
                    "attachment; filename=" + filePath);
            Context.Response.TransmitFile(filePath);
            Context.Response.End();            

            DeleteUserFolder(path);
        }

        private void GenerateChartsReport(LogFrameValues logFrameValues, string fileName)
        {
            using (iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 8, 8, 14, 6))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, outputStream))
                    {
                        document.Open();

                        //WritePDF generatePDF = new WritePDF(logFrameValues, fileName, logf);
                        //generatePDF.GeneratePDF(document);

                        document.Close();
                    }
                }
            }
        }

        private IEnumerable<XElement> GetElementChunk(IEnumerable<IEnumerable<XElement>> logFrameElements, string durationTypeName, string durationTypeId, string yearTypeId)
        {
            foreach (var result in logFrameElements)
            {
                string yearId = result.Elements("Year").First().Attribute("Id").Value;
                string monthOrQuarterId = null;
                if (durationTypeName.Equals("Month"))
                {
                    monthOrQuarterId = result.Elements("Month").First().Attribute("Id").Value;
                }
                else if (durationTypeName.Equals("Quarter"))
                {
                    monthOrQuarterId = result.Elements("Quarter").First().Attribute("Id").Value;
                }

                if (monthOrQuarterId == durationTypeId && yearId == yearTypeId)
                {
                    return result;
                }
            }

            return null;
        }

        private LogFrameValues GetLogFrameValues(IEnumerable<XElement> logFrameElements)
        {
            LogFrameValues lfv = new LogFrameValues();
            lfv.Cluster = GetElementValue(logFrameElements, "Cluster");
            lfv.Objective = GetElementValue(logFrameElements, "Objective");
            lfv.Indicator = GetElementValue(logFrameElements, "Indicator");
            lfv.Activity = GetElementValue(logFrameElements, "Activity");
            lfv.Data = GetElementValue(logFrameElements, "Data");
            lfv.MonthName = GetElementValue(logFrameElements, "Month");
            lfv.QName = GetElementValue(logFrameElements, "Quarter");
            lfv.YearName = GetElementValue(logFrameElements, "Year");

            return lfv;
        }

        private string GetAttributeValue(XElement xElement, string childElement, string attribute)
        {
            return xElement.Elements(childElement).First().Attribute(attribute).Value;
        }

        private void DownLoadFile(string filePath)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(filePath, @"e:\charts.pdf");
            }
        }

        private void DeleteUserFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Delete(true);
        }

        private string GetElementValue(IEnumerable<XElement> logFrameElements, string elementName)
        {
            return logFrameElements.Elements(elementName).FirstOrDefault().Value;
        }

        //private Font TitleFont
        //{
        //    get
        //    {
        //        return iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
        //    }
        //}

        //private Font TableFont
        //{
        //    get
        //    {
        //        return FontFactory.GetFont("Arial", 8, Font.NORMAL);
        //    }
        //}

        //private void AddProjectTitle(PdfPTable projectTitlePDFTable, string phraseTitle, string phraseText)
        //{
        //    //PdfPTable projectTitlePDFTable = new PdfPTable(1);
        //    projectTitlePDFTable.KeepTogether = true;

        //    //relative col widths in proportions - 1/3 and 2/3
        //    float[] widths = new float[] { 1f, 3f };
        //    projectTitlePDFTable.SetWidths(widths);

        //    //leave a gap before and after the table
        //    projectTitlePDFTable.SpacingBefore = 10f;
        //    projectTitlePDFTable.SpacingAfter = 10f;

        //    // Add funding header.
        //    PdfPCell cell = null;

        //    cell = new PdfPCell(new Phrase(phraseTitle, TitleFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase(phraseText, TableFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);
        //}

        // Add funding header.
        //PdfPTable projectMainInfoTable = new PdfPTable(2);
        //projectMainInfoTable.KeepTogether = true;
        //float[] widths = new float[] { 1f, 3f };
        //projectMainInfoTable.SetWidths(widths);

        //PdfPCell cell = null;
        //cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
        //cell.Colspan = 2;
        //cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //projectMainInfoTable.AddCell(cell);

        //projectMainInfoTable.SpacingAfter = 10f;

        //PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
        //doc.Open();
        //PdfPTable projectTitlePDFTable = new PdfPTable(2);


    }
}
