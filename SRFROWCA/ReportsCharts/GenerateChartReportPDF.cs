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
using System.Web.UI;

namespace SRFROWCA.ReportsCharts
{
    public class GenerateChartReportPDF
    {
        public string GeneratePDF(string path)
        {
            string filePath = "";
            if (File.Exists(path + "logframe.xml"))
            {
                filePath = path + "Charts.pdf";

                iTextSharp.text.Document document = CreatePDFDocument(filePath);
                document.Open();

                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles("*.jpg").OrderBy(p => p.CreationTime).ToArray();

                XDocument xmlDoc = XDocument.Load(path + "logframe.xml");
                string durationTypeName = GetAttributeValue(xmlDoc.Element("LogFrames"), "LogFrame", "DurationTypeName");

                foreach (FileInfo file in files)
                {
                    string fileName = file.Name;
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
                            WritePDF generatePDF = new WritePDF(logFrameValues, file.FullName);
                            generatePDF.WriteInPDFDocument(document);

                        }
                    }
                }

                document.Close();
            }
            return filePath;
        }

        private Document CreatePDFDocument(string filePath)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4, 8, 8, 14, 6);
            FileStream outputStream = new FileStream(filePath, FileMode.Create);
            PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
            return document;
        }

        private string GetAttributeValue(XElement xElement, string childElement, string attribute)
        {
            return xElement.Elements(childElement).First().Attribute(attribute).Value;
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

        private string GetElementValue(IEnumerable<XElement> logFrameElements, string elementName)
        {
            return logFrameElements.Elements(elementName).FirstOrDefault().Value;
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
    }
}