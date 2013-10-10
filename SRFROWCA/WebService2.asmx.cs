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
        public void GeneratePDF(int j)
        {
            string pdfpath = Server.MapPath("img2");

            if (!Directory.Exists(pdfpath + "\\" + Session.SessionID.ToString()))
            {
                Directory.CreateDirectory(pdfpath + "\\" + Session.SessionID.ToString());
            }

            string dir = pdfpath + "\\" + Session.SessionID.ToString() + "\\";

            string imagepath = "E:\\img\\" + Session.SessionID.ToString() + "\\";
            //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
            Document doc = new Document();
            doc.SetPageSize(iTextSharp.text.PageSize.A4);
            string filePath = dir + "Charts.pdf";
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
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

                for (int i = 0; i < j; i++)
                {
                    iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + Session.SessionID.ToString() + i.ToString() + ".jpg");
                    doc.Add(gif);
                }
                doc.Close();

                DirectoryInfo di = new DirectoryInfo("E:\\img\\" + Session.SessionID.ToString());
                //di.Delete(true);
                
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(filePath, @"e:\charts1.pdf");
                }
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
            string result = "Success";
            try
            {
                if (!Directory.Exists("E:\\img\\" + Session.SessionID.ToString()))
                {
                    Directory.CreateDirectory("E:\\img\\" + Session.SessionID.ToString());
                }
                string dir = "E:\\img\\" + Session.SessionID.ToString();
                string pathToSave = dir + "\\" + Session.SessionID.ToString() + i.ToString();
                string svgPath = pathToSave + ".svg";
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
                    string imagePath = pathToSave + ".jpg";
                    svgDocument.Draw().Save(imagePath);

                    memoryStream.Close();
                    memoryStream.Dispose();
                }

            }
            catch
            {
                // log error;
            }

            return result;
        }
    }
}
