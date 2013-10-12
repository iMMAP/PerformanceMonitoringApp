using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Diagnostics;
using Svg;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using PdfSharp.Drawing;
using System.Xml.Linq;
using System.Linq;

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

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private Font TitleFont
        {
            get
            {
                return iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
            }
        }

        private Font TableFont
        {
            get
            {
                return FontFactory.GetFont("Arial", 8, Font.NORMAL);
            }
        }

        private void AddProjectTitle(PdfPTable projectTitlePDFTable)
        {
            //PdfPTable projectTitlePDFTable = new PdfPTable(1);
            projectTitlePDFTable.KeepTogether = true;

            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 3f };
            projectTitlePDFTable.SetWidths(widths);

            //leave a gap before and after the table
            projectTitlePDFTable.SpacingBefore = 10f;
            projectTitlePDFTable.SpacingAfter = 10f;

            // Add funding header.
            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Cluster: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Cluster Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Objective: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Objective Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indicator: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indicator Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Activity: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Activity Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Data: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Data Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Location: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Location Name", TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            projectTitlePDFTable.AddCell(cell);

        }

        [WebMethod(EnableSession = true)]
        public void GeneratePDF2()
        {
            string pdfpath = Server.MapPath("img2");

            string imagepath = @"E:\img\";
            //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
                doc.Open();
                PdfPTable projectTitlePDFTable = new PdfPTable(2);

                AddProjectTitle(projectTitlePDFTable);
                //AddNewLineInDocument(document, 1);
                doc.Add(projectTitlePDFTable);

                PdfPTable projectMainInfoTable = new PdfPTable(2);
                projectMainInfoTable.KeepTogether = true;
                float[] widths = new float[] { 1f, 3f };
                projectMainInfoTable.SetWidths(widths);

                projectMainInfoTable.SpacingAfter = 10f;

                // Add funding header.
                PdfPCell cell = null;
                cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
                projectMainInfoTable.AddCell(cell);


                iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + "chart.png");
                doc.Add(gif);

            }

            catch (Exception ex)
            {
                //Log error;
            }
            finally
            {
                doc.Close();
            }
        }

        [WebMethod(EnableSession = true)]
        public string SaveSVG1(string svg, int i)
        {
            //string svg = '<div style="position: relative; overflow: hidden; width: 550px; height: 530px; text-align: left; line-height: normal; z-index: 0; font-family: &quot;Lucida Grande&quot;,&quot;Lucida Sans Unicode&quot;,Verdana,Arial,Helvetica,sans-serif; font-size: 12px; left: 0.0833282px; top: 0.716553px;" id="highcharts-18" class="highcharts-container"><svg height="530" width="550" version="1.1" xmlns="http://www.w3.org/2000/svg"><desc>Created with Highcharts 3.0.1</desc><defs><clipPath id="highcharts-19"><rect height="530" width="9999" y="0" x="0" fill="none" ry="0" rx="0"></rect></clipPath><clipPath id="highcharts-20"><rect height="347" width="504" y="0" x="0" fill="none"></rect></clipPath></defs><rect height="530" width="550" y="0" x="0" fill="#FFFFFF" ry="5" rx="5"></rect><g transform="translate(516,10)" stroke-linecap="round" title="Chart context menu" style="cursor:default;" class="highcharts-button"><title>Chart context menu</title><rect stroke-width="1" stroke="none" height="22" width="24" y="0.5" x="0.5" fill="white" ry="2" rx="2"></rect><path zIndex="1" stroke-width="3" stroke="#666" d="M 6 6.5 L 20 6.5 M 6 11.5 L 20 11.5 M 6 16.5 L 20 16.5" fill="#E0E0E0"></path><text zIndex="1" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:12px;color:black;fill:black;" y="13" x="0"></text></g><g zIndex="1" class="highcharts-grid"></g><g zIndex="1" class="highcharts-grid"><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 398.5 L 540 398.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 349.5 L 540 349.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 299.5 L 540 299.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 249.5 L 540 249.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 199.5 L 540 199.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 150.5 L 540 150.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 100.5 L 540 100.5" fill="none"></path><path opacity="1" zIndex="1" stroke-width="1" stroke="#C0C0C0" d="M 36 447.5 L 540 447.5" fill="none"></path></g><g zIndex="2" class="highcharts-axis"><path opacity="1" stroke-width="1" stroke="#C0D0E0" d="M 539.5 448 L 539.5 453" fill="none"></path><path opacity="1" stroke-width="1" stroke="#C0D0E0" d="M 287.5 448 L 287.5 453" fill="none"></path><path stroke-width="1" stroke="#C0D0E0" d="M 36.5 448 L 36.5 453" fill="none"></path><path visibility="visible" zIndex="7" stroke-width="1" stroke="#C0D0E0" d="M 36 447.5 L 540 447.5" fill="none"></path></g><g zIndex="2" class="highcharts-axis"></g><g zIndex="3" class="highcharts-series-group"><g clip-path="url(#highcharts-20)" style="" transform="translate(36,100) scale(1 1)" zIndex="0.1" visibility="visible" class="highcharts-series highcharts-tracker"><rect ry="0" rx="0" stroke-width="1" stroke="#FFFFFF" height="248" width="19" y="99.5" x="78.5" fill="#2f7ed8"></rect><rect ry="0" rx="0" stroke-width="1" stroke="#FFFFFF" height="298" width="19" y="49.5" x="330.5" fill="#2f7ed8"></rect></g><g transform="translate(36,100) scale(1 1)" zIndex="0.1" visibility="visible" class="highcharts-markers"></g><g clip-path="url(#highcharts-20)" style="" transform="translate(36,100) scale(1 1)" zIndex="0.1" visibility="visible" class="highcharts-series highcharts-tracker"><rect ry="0" rx="0" stroke-width="1" stroke="#FFFFFF" height="174" width="19" y="173.5" x="153.5" fill="#0d233a"></rect><rect ry="0" rx="0" stroke-width="1" stroke="#FFFFFF" height="298" width="19" y="49.5" x="405.5" fill="#0d233a"></rect></g><g transform="translate(36,100) scale(1 1)" zIndex="0.1" visibility="visible" class="highcharts-markers"></g></g><g transform="translate(192,488)" zIndex="7" class="highcharts-legend"><rect visibility="visible" stroke-width="1" stroke="#909090" height="26" width="166" y="0.5" x="0.5" fill="none" ry="5" rx="5"></rect><g clip-path="url(#highcharts-19)" zIndex="1"><g><g transform="translate(8,3)" zIndex="1" class="highcharts-legend-item"><text zIndex="2" text-anchor="start" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:12px;cursor:pointer;color:#274b6d;fill:#274b6d;" y="15" x="21"><tspan x="21">Target</tspan></text><rect zIndex="3" height="12" width="16" y="4" x="0" fill="#2f7ed8" ry="2" rx="2"></rect></g><g transform="translate(81,3)" zIndex="1" class="highcharts-legend-item"><text zIndex="2" text-anchor="start" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:12px;cursor:pointer;color:#274b6d;fill:#274b6d;" y="15" x="21"><tspan x="21">Achieved</tspan></text><rect zIndex="3" height="12" width="16" y="4" x="0" fill="#0d233a" ry="2" rx="2"></rect></g></g></g></g><g zIndex="7" class="highcharts-axis-labels"><text opacity="1" transform="rotate(-45 162 462)" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;color:#666;cursor:default;line-height:14px;fill:#666;" y="462" x="162"><tspan x="162">Gao</tspan></text><text opacity="1" transform="rotate(-45 414 462)" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;color:#666;cursor:default;line-height:14px;fill:#666;" y="462" x="414"><tspan x="414">Kayes</tspan></text></g><g zIndex="7" class="highcharts-axis-labels"><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="453.1" x="28"><tspan x="28">0</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="403.3857142857143" x="28"><tspan x="28">10</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="353.6714285714286" x="28"><tspan x="28">20</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="303.9571428571429" x="28"><tspan x="28">30</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="254.2428571428571" x="28"><tspan x="28">40</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="204.52857142857144" x="28"><tspan x="28">50</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="154.81428571428572" x="28"><tspan x="28">60</tspan></text><text opacity="1" text-anchor="end" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:11px;width:232px;color:#666;cursor:default;line-height:14px;fill:#666;" y="105.1" x="28"><tspan x="28">70</tspan></text></g><g visibility="hidden" style="cursor:default;padding:0;white-space:nowrap;" zIndex="8" class="highcharts-tooltip"><rect transform="translate(1, 1)" stroke-width="5" stroke-opacity="0.049999999999999996" stroke="black" isShadow="true" fill-opacity="0.85" height="16" width="16" y="0.5" x="0.5" fill="none" ry="3" rx="3"></rect><rect transform="translate(1, 1)" stroke-width="3" stroke-opacity="0.09999999999999999" stroke="black" isShadow="true" fill-opacity="0.85" height="16" width="16" y="0.5" x="0.5" fill="none" ry="3" rx="3"></rect><rect transform="translate(1, 1)" stroke-width="1" stroke-opacity="0.15" stroke="black" isShadow="true" fill-opacity="0.85" height="16" width="16" y="0.5" x="0.5" fill="none" ry="3" rx="3"></rect><rect fill-opacity="0.85" height="16" width="16" y="0.5" x="0.5" fill="rgb(255,255,255)" ry="3" rx="3"></rect><text zIndex="1" style="font-family:&quot;Lucida Grande&quot;, &quot;Lucida Sans Unicode&quot;, Verdana, Arial, Helvetica, sans-serif;font-size:12px;color:#333333;fill:#333333;" y="21" x="8"></text></g></svg></div>';


            string result = "Success";
            try
            {
                string dir = Server.MapPath("GeneratedChartFiles");
                dir += "\\" + Session.SessionID.ToString();

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string path = dir + "\\" + Session.SessionID.ToString() + "__" + i.ToString();

                string svgPath = path + ".svg";
                FileStream fs = new FileStream(svgPath, FileMode.Create, FileAccess.Write);
                StreamWriter s = new StreamWriter(fs);
                s.WriteLine(svg);
                s.Close();
                fs.Close();

                using (FileStream fileStream = File.OpenRead(svgPath))
                {
                    MemoryStream memoryStream = new MemoryStream();

                    memoryStream.SetLength(fileStream.Length);
                    fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);

                    SvgDocument svgDocument = SvgDocument.Open(memoryStream);
                    svgDocument.Draw().Save(path + ".jpg");

                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }
            catch
            {
                //throw;
            }

            return result;
        }

        [WebMethod(EnableSession = true)]
        public void GeneratePDF(int j)
        {
            int k = j;
            string dir = Server.MapPath("GeneratedChartFiles");

            if (!Directory.Exists(dir + "\\" + Session.SessionID.ToString()))
            {
                Directory.CreateDirectory(dir + "\\" + Session.SessionID.ToString());
            }

            string path = dir + "\\" + Session.SessionID.ToString() + "\\";

            //string imagepath = "E:\\img\\" + Session.SessionID.ToString() + "\\";

            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4);

            string filePath = path + "Charts.pdf";
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();
                PdfPTable projectTitlePDFTable = new PdfPTable(2);
                FileInfo[] imageFiles = new DirectoryInfo(path).GetFiles("*.jpg");

                XDocument logFrameDoc = XDocument.Load(path + "logframe.xml");
                string logFrameType = logFrameDoc.Element("LogFrames").Elements("LogFrame").First().Attribute("LogFrameType").Value;

                for (int i = 0; i < imageFiles.Length; i++)
                {
                    string fileName = imageFiles[i].Name;
                    string id = fileName.Substring(fileName.LastIndexOf("__") + 2, fileName.LastIndexOf('.') - fileName.LastIndexOf("__"));

                    var a = logFrameDoc.Descendants("LogFrames")
                                   .Where(x => (string)x.Attribute("Id").Value == "125")
                                   .Select(x => (string)x.Element("Objective"))
                                   .FirstOrDefault();

                    var query =
                    from e in logFrameDoc.Descendants("LogFrame")
                    where e.Element("Objective").Attribute("Id").Value == "125"
                    select new { Name = e.Element("Objective").Value };
                    foreach (var v in query)
                    { }

                    XElement d = XElement.Load(path + "logframe.xml");
                    XElement logFrame = d.Element("LogFrame");
                    var list1 =
                        from el in d.Descendants("Objective")
                        where (string)el.Attribute("Id").Value == "125"
                        select el.AncestorsAndSelf().Distinct();

                    foreach (var v in list1)
                    { }

                }

                AddProjectTitle(projectTitlePDFTable);
                //AddNewLineInDocument(document, 1);
                doc.Add(projectTitlePDFTable);

                PdfPTable projectMainInfoTable = new PdfPTable(2);
                projectMainInfoTable.KeepTogether = true;
                float[] widths = new float[] { 1f, 3f };
                projectMainInfoTable.SetWidths(widths);

                projectMainInfoTable.SpacingAfter = 10f;

                // Add funding header.
                PdfPCell cell = null;
                cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
                cell.Colspan = 2;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
                projectMainInfoTable.AddCell(cell);

                for (int i = 0; i < j; i++)
                {
                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(path + Session.SessionID.ToString() + i.ToString() + ".jpg");
                    doc.Add(gif);
                }
                doc.Close();

                PdfSharp.Pdf.PdfDocument document = new PdfSharp.Pdf.PdfDocument();

                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(filePath, @"e:\charts.pdf");
                }

                DirectoryInfo di = new DirectoryInfo(path);

                //di.Delete(true);
            }

            catch
            {
                throw;
            }
            finally
            {
                doc.Close();
            }
        }
    }
}
