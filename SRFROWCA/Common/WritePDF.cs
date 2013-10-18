using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace SRFROWCA.Common
{
    public class WritePDF
    {
        public LogFrameValues LogFrameValues { get; set; }
        public string ImageFilePath { get; set; }
        public string LogFrameType { get; set; }

        public WritePDF(LogFrameValues logFrameValues, string imageFilePath)
        {
            LogFrameValues = logFrameValues;
            ImageFilePath = imageFilePath;
        }

        public void WriteInPDFDocument(iTextSharp.text.Document document)
        {
            ChartTitle(document);
            ChartImage(document);
            document.NewPage();
            //ServiceProviderInfo(document, projectId);
        }

        private void ChartImage(Document document)
        {
            string imageName = ImageFilePath.Substring(ImageFilePath.LastIndexOf('\\') + 2);
            string imagePath = ImageFilePath.Substring(0, ImageFilePath.LastIndexOf('\\'));
            imagePath += "\\t" + imageName;
            iTextSharp.text.Image percentageGif = iTextSharp.text.Image.GetInstance(imagePath);
            document.Add(percentageGif);
            iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(ImageFilePath);
            document.Add(gif);

            
        }

        private void ChartTitle(iTextSharp.text.Document document)
        {
            PdfPTable chartTitleTable = new PdfPTable(2);

            chartTitleTable.KeepTogether = true;

            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 3f };
            chartTitleTable.SetWidths(widths);

            //leave a gap before and after the table
            chartTitleTable.SpacingBefore = 20f;
            chartTitleTable.SpacingAfter = 20f;

            if (!string.IsNullOrEmpty(LogFrameValues.Cluster))
            {
                ChartTitleCell(chartTitleTable, "Cluster", LogFrameValues.Cluster);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.Objective))
            {
                ChartTitleCell(chartTitleTable, "Objective", LogFrameValues.Objective);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.Indicator))
            {
                ChartTitleCell(chartTitleTable, "Indicator", LogFrameValues.Indicator);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.Activity))
            {
                ChartTitleCell(chartTitleTable, "Activity", LogFrameValues.Activity);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.Data))
            {
                ChartTitleCell(chartTitleTable, "Data", LogFrameValues.Data);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.MonthName))
            {
                ChartTitleCell(chartTitleTable, "Month", LogFrameValues.MonthName);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.QName))
            {
                ChartTitleCell(chartTitleTable, "Quarter", LogFrameValues.QName);
            }
            if (!string.IsNullOrEmpty(LogFrameValues.YearName))
            {
                ChartTitleCell(chartTitleTable, "Year", LogFrameValues.YearName);
            }

            document.Add(chartTitleTable);
        }

        private void ChartTitleCell(PdfPTable chartTitleTable, string caption, string text)
        {
            // Add funding header.
            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase(caption + ": ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            chartTitleTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(text, TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            chartTitleTable.AddCell(cell);
        }

        //private void ChartTitle(iTextSharp.text.Document document, int projectId)
        //{
        //    PdfPTable projectTitlePDFTable = new PdfPTable(2);

        //    AddProjectTitle(, projectTitlePDFTable);
        //    //AddNewLineInDocument(document, 1);
        //    document.Add(projectTitlePDFTable);

        //    PdfPTable projectMainInfoTable = new PdfPTable(2);
        //    projectMainInfoTable.KeepTogether = true;
        //    float[] widths = new float[] { 1f, 3f };
        //    projectMainInfoTable.SetWidths(widths);

        //    projectMainInfoTable.SpacingAfter = 10f;

        //    // Add funding header.
        //    PdfPCell cell = null;
        //    cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
        //    cell.Colspan = 2;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    projectMainInfoTable.AddCell(cell);

        //    AddProjectInfoInPDFTable(drProjectMainInfo, projectMainInfoTable);
        //    document.Add(projectMainInfoTable);
        //}

        //private void AddProjectTitle(DataRow dr, PdfPTable projectTitlePDFTable)
        //{
        //    //PdfPTable projectTitlePDFTable = new PdfPTable(1);
        //    projectTitlePDFTable.KeepTogether = true;

        //    //relative col widths in proportions - 1/3 and 2/3
        //    float[] widths = new float[] { 1f, 3f };
        //    projectTitlePDFTable.SetWidths(widths);

        //    //leave a gap before and after the table
        //    projectTitlePDFTable.SpacingBefore = 20f;
        //    projectTitlePDFTable.SpacingAfter = 20f;

        //    // Add funding header.
        //    PdfPCell cell = null;

        //    cell = new PdfPCell(new Phrase("Project Code: ", TitleFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase(dr["ProjectCode"].ToString(), TableFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Project Title: ", TitleFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase(dr["ProjectTitle"].ToString(), TableFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    projectTitlePDFTable.AddCell(cell);
        //}
        //private void AddProjectInfoInPDFTable(DataRow dr, PdfPTable projectTable)
        //{
        //    projectTable.AddCell(new Phrase("Emergency:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["EmergencyTitle"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Appeal:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["AppealTitle"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Parent Project Code:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ANProjectCode"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Parent Project Title:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ANProjectTitle"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("ProjectCode:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectCode"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("ProjectTitle:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectTitle"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Start Date:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectStartDate"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("End Date:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectEndDate"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Currency:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["CurrencyTitle"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Admin Cost:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectOperationalCost"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Staff Cost:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectOverheadsCost"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Activity Cost:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectProgramCost"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Input Cost:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectAdditionalCost"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Project Cost:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectCost"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Project Status:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectStatusTypeName"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("PO Name:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectOwnerName"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("PO Cell No:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["MemberCellNo"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("PO Email:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["MemberEmail"].ToString(), TableFont));

        //    projectTable.AddCell(new Phrase("Comments:", TableFont));
        //    projectTable.AddCell(new Phrase(dr["ProjectDescription"].ToString(), TableFont));
        //}

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
    }
}