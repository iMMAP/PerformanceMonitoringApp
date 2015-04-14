using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Globalization;

namespace SRFROWCA.Common
{
    public static class WriteDataEntryPDF
    {
        #region Properties/Enums

        private static Font TitleFont
        {
            get
            {
                return iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
            }
        }

        private static Font TableFont
        {
            get
            {
                return FontFactory.GetFont("Arial", 8, Font.NORMAL);
            }
        }

        private static bool KeepPDFTableTogether
        {
            get { return true; }
        }

        private enum ProjectFundingType
        {
            Pledges = 1,
            Commitments = 2,
            Contributions = 3,
        }

        private enum FetchStatus
        {
            Success = 1
        }

        private static int PDFExportObjId
        {
            get
            {
                int docType = 0;
                if (HttpContext.Current.Session["PDFExportObjId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["PDFExportObjId"].ToString(), out docType);
                }

                return docType;
            }
            set
            {
                HttpContext.Current.Session["PDFExportObjId"] = value.ToString();
            }
        }

        private static int PDFExportPrId
        {
            get
            {
                int docType = 0;
                if (HttpContext.Current.Session["PDFExportPrId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["PDFExportPrId"].ToString(), out docType);
                }

                return docType;
            }
            set
            {
                HttpContext.Current.Session["PDFExportPrId"] = value.ToString();
            }
        }

        private static int PDFExportActivityId
        {
            get
            {
                int docType = 0;
                if (HttpContext.Current.Session["PDFExportActivityId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["PDFExportActivityId"].ToString(), out docType);
                }

                return docType;
            }
            set
            {
                HttpContext.Current.Session["PDFExportActivityId"] = value.ToString();
            }
        }

        private static int PDFExportIndicatorId
        {
            get
            {
                int docType = 0;
                if (HttpContext.Current.Session["PDFExportIndicatorId"] != null)
                {
                    int.TryParse(HttpContext.Current.Session["PDFExportIndicatorId"].ToString(), out docType);
                }

                return docType;
            }
            set
            {
                HttpContext.Current.Session["PDFExportIndicatorId"] = value.ToString();
            }
        }

        #endregion

        public static void GenerateDocument(iTextSharp.text.Document document, DataTable dt)
        {
            List<int> projectIds = GetDistinctProjects(dt);

            for (int i = 0; i < projectIds.Count; i++)
            {
                IEnumerable<DataRow> projectDetails = GetProjectInformation(dt, projectIds[i]);
                DataRow projectGeneralInfo = projectDetails.FirstOrDefault<DataRow>();
                ProjectGeneralInfo(document, projectGeneralInfo);

                IndicatorTargets(document, projectDetails, true);
                document.NewPage();
            }
        }

        public static MemoryStream GeneratePDF(DataTable dt, int? projectID, int? reportID)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);

                if (dt.Rows.Count > 0)
                {
                    document.Open();

                    GenerateDocument(document, dt, projectID, reportID);

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }

                return outputStream;
            }
        }

        public static MemoryStream GenerateSummaryPDF(DataTable dt, string startDate, string endDate)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);

                if (dt.Rows.Count > 0)
                {
                    document.Open();
                    GenerateSummaryReport(document, dt, startDate, endDate);
                    GenerateSummaryCountry(document, dt);
                    GenerateSummaryOrganization(document, dt);

                    try
                    {
                        document.Close();
                    }
                    catch { }
                }

                return outputStream;
            }
        }

        public static void GenerateDocument(iTextSharp.text.Document document, DataTable dt, int? projectID, int? reportID)
        {
            if (projectID != null && projectID > 0)
            {
                IEnumerable<DataRow> projectDetails = GetProjectInformation(dt, (int)projectID);
                DataRow projectGeneralInfo = projectDetails.FirstOrDefault<DataRow>();

                ProjectGeneralInfo(document, projectGeneralInfo);
                ProjectReports(document, dt, reportID);


            }
            else if (projectID == null)
            {
                List<int> projectIds = GetDistinctProjects(dt);

                for (int i = 0; i < projectIds.Count; i++)
                {
                    IEnumerable<DataRow> projectDetails = GetProjectInformation(dt, projectIds[i]);
                    DataRow projectGeneralInfo = projectDetails.FirstOrDefault<DataRow>();

                    ProjectGeneralInfo(document, projectGeneralInfo);
                    ProjectReports(document, dt.Select("ProjectID='" + projectIds[i] + "'").CopyToDataTable<DataRow>(), reportID);

                    document.NewPage();
                }
            }
        }

        private static void ProjectReports(iTextSharp.text.Document document, DataTable dt, int? reportID)
        {
            if (reportID != null)
            {
                PdfPTable tbl = new PdfPTable(new float[] { 2f, 3f, 3f, 2f, 4f, 2f, 3f, 2f });
                //ProjectReportHeaders(tbl);

                DataRow[] row = dt.Select("ReportID='" + reportID.ToString() + "'");

                if (row.Length > 0)
                {
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["ReportID"]), TableFont));
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["ReportName"]), TableFont));
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["OrganizationName"]), TableFont));
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["Month"]), TableFont));
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["CreatedBy"]), TableFont));
                    //tbl.AddCell(new Phrase(!string.IsNullOrEmpty(Convert.ToString(row[0]["CreatedDate"])) ? Convert.ToDateTime(row[0]["CreatedDate"]).ToString("MM/dd/yyyy") : string.Empty, TableFont));
                    //tbl.AddCell(new Phrase(Convert.ToString(row[0]["UpdatedBy"]), TableFont));
                    //tbl.AddCell(new Phrase(!string.IsNullOrEmpty(Convert.ToString(row[0]["UpdatedDate"])) ? Convert.ToDateTime(row[0]["UpdatedDate"]).ToString("MM/dd/yyyy") : string.Empty, TableFont));
                }

                tbl.SpacingAfter = 10f;
                document.Add(tbl);

                IndicatorTargets(document, (from projectData in dt.AsEnumerable()
                                            where projectData.Field<int?>("ReportID") == (int?)Convert.ToInt32(row[0]["ReportID"])
                                            select projectData), false);

            }
            else
            {
                //DataRow[] rows = dt.Select("UserID = '" + RC.GetCurrentUserId + "'");

                //if (rows.Length > 0)
                {
                    DataTable dtFiltered = new DataTable();
                    string[] selectedColumns = new[] { "ReportID", "ReportName", "OrganizationName", "Month", "CreatedBy", "CreatedDate", "UpdatedBy", "UpdatedDate" };

                    try
                    {
                        dtFiltered = dt.DefaultView.ToTable(true, selectedColumns); //new DataView(rows.CopyToDataTable<DataRow>()).ToTable(true, selectedColumns);

                    }
                    catch { }

                    for (int i = 0; i < dtFiltered.Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dtFiltered.Rows[i]["ReportID"])))
                        {
                            PdfPTable tbl = new PdfPTable(new float[] { 2f, 3f, 3f, 2f, 4f, 2f, 3f, 2f });
                            //ProjectReportHeaders(tbl);

                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["ReportID"]), TableFont));
                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["ReportName"]), TableFont));
                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["OrganizationName"]), TableFont));
                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["Month"]), TableFont));
                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["CreatedBy"]), TableFont));
                            //tbl.AddCell(new Phrase(!string.IsNullOrEmpty(Convert.ToString(dtFiltered.Rows[i]["CreatedDate"])) ? Convert.ToDateTime(dtFiltered.Rows[i]["CreatedDate"]).ToString("MM/dd/yyyy") : string.Empty, TableFont));
                            //tbl.AddCell(new Phrase(Convert.ToString(dtFiltered.Rows[i]["UpdatedBy"]), TableFont));
                            //tbl.AddCell(new Phrase(!string.IsNullOrEmpty(Convert.ToString(dtFiltered.Rows[i]["UpdatedDate"])) ? Convert.ToDateTime(dtFiltered.Rows[i]["UpdatedDate"]).ToString("MM/dd/yyyy") : string.Empty, TableFont));

                            tbl.SpacingAfter = 10f;
                            document.Add(tbl);

                            IndicatorTargets(document, (from projectData in dt.AsEnumerable()
                                                        where projectData.Field<int?>("ReportID") == (int?)Convert.ToInt32(dtFiltered.Rows[i]["ReportID"])
                                                        select projectData), false);
                        }
                    }
                }
            }
        }

        private static void ProjectReportHeaders(PdfPTable tbl)
        {
            PdfPCell cell = null;
            var headerColor = new BaseColor(240, 240, 240);
            cell = new PdfPCell(new Phrase("Report ID", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Report Name", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Organization Name", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Month", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Created By", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Created Date", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Updated By", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Updated Date", TitleFont));
            cell.BackgroundColor = headerColor;
            tbl.AddCell(cell);
        }

        private static void IndicatorTargets(Document document, IEnumerable<DataRow> projectDetails, bool showAccum)
        {
            DataTable dt = projectDetails.CopyToDataTable();
            var distinctPriorities = (from DataRow dRow in dt.Rows
                                      select new
                                          {
                                              ObjectiveId = dRow["ObjectiveId"],
                                              PriorityId = dRow["HumanitarianPriorityId"]
                                          })
                                        .Distinct();

            foreach (var priority in distinctPriorities)
            {
                IEnumerable<DataRow> temp = (from pr in dt.AsEnumerable()
                                             where pr.Field<int>("ObjectiveId") == (int)priority.ObjectiveId
                                             && pr.Field<int>("HumanitarianPriorityId") == (int)priority.PriorityId
                                             select pr);

                DataTable dt1 = temp.CopyToDataTable();
                DataRow logFrame = temp.FirstOrDefault<DataRow>();
                if (logFrame != null)
                {
                    PdfPTable tbl1 = new PdfPTable(1);
                    PdfPCell cell1 = null;
                    var phraseColor = new BaseColor(216, 216, 216);
                    cell1 = new PdfPCell(new Phrase(logFrame["Objective"].ToString(), TableFont));
                    cell1.Border = 0;
                    cell1.BackgroundColor = phraseColor;
                    tbl1.AddCell(cell1);

                    cell1 = new PdfPCell(new Phrase(logFrame["HumanitarianPriority"].ToString(), TableFont));
                    cell1.Border = 0;
                    cell1.BackgroundColor = phraseColor;
                    tbl1.AddCell(cell1);
                    tbl1.SpacingBefore = 3f;
                    tbl1.SpacingAfter = 3f;
                    document.Add(tbl1);

                    //AddLogFrameInfo(document, logFrame);
                }


                var distinctIndicators = (from DataRow dRow in dt1.Rows
                                          select new
                                          {
                                              ActivityId = dRow["PriorityActivityId"],
                                              DataId = dRow["ActivityDataId"]
                                          })
                                        .Distinct();


                foreach (var indicator in distinctIndicators)
                {
                    IEnumerable<DataRow> targets = (from ind in dt1.AsEnumerable()
                                                    where ind.Field<int>("PriorityActivityId") == (int)indicator.ActivityId
                                                    && ind.Field<int>("ActivityDataId") == (int)indicator.DataId
                                                    select ind);


                    //PdfPTable tbl = showAccum ? new PdfPTable(4) : new PdfPTable(3);
                    PdfPTable tbl = new PdfPTable(2);
                    tbl.WidthPercentage = 39.25F;

                    DataRow drAct = targets.First<DataRow>();
                    AddLogFrameInfo(document, drAct);

                    PdfPCell cell = null;
                    var headerColor = new BaseColor(240, 240, 240);
                    cell = new PdfPCell(new Phrase("Locaiton", TitleFont));
                    cell.BackgroundColor = headerColor;
                    tbl.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Target 2015", TitleFont));
                    cell.BackgroundColor = headerColor;
                    tbl.AddCell(cell);
                    foreach (DataRow row in targets)
                    {
                        ReportedData(document, tbl, row, showAccum);
                    }

                    tbl.SpacingAfter = 10f;
                    document.Add(tbl);
                }
            }
        }

        private static void ReportedData(Document document, PdfPTable tbl, DataRow row, bool showAccum)
        {
            tbl.AddCell(new Phrase(row["Location"].ToString(), TableFont));
            //tbl.AddCell(new Phrase(row["TargetAnnual"].ToString(), TableFont));

            //if (showAccum)
            //    tbl.AddCell(new Phrase(row["Accumulative"].ToString(), TableFont));

            tbl.AddCell(new Phrase(row["Achieved"].ToString(), TableFont));
        }

        private static void AddLogFrameInfo(Document document, DataRow dr)
        {
            PdfPTable tbl = new PdfPTable(2);
            tbl.DefaultCell.Border = Rectangle.NO_BORDER;
            tbl.KeepTogether = true;

            float[] widths = new float[] { 1f, 3f };
            tbl.SetWidths(widths);

            //tbl.AddCell(new Phrase("Objective:", TableFont));
            //tbl.AddCell(new Phrase(dr["Objective"].ToString(), TableFont));
            //tbl.AddCell(new Phrase("Priority:", TableFont));
            //tbl.AddCell(new Phrase(dr["HumanitarianPriority"].ToString(), TableFont));
            tbl.AddCell(new Phrase("Activity:", TableFont));
            tbl.AddCell(new Phrase(dr["ActivityName"].ToString(), TableFont));
            tbl.AddCell(new Phrase("Activity Indicator:", TableFont));
            tbl.AddCell(new Phrase(dr["DataName"].ToString(), TableFont));

            document.Add(tbl);
        }

        private static void TargetsDetails(Document document, object indicator, DataTable dt)
        {
            //PdfPTable tbl = new PdfPTable(4);

            ////tbl.KeepTogether = true;
            //////relative col widths in proportions - 1/3 and 2/3
            ////float[] widths = new float[] { 1f, 3f };
            ////tbl.SetWidths(widths);

            //////leave a gap before and after the table
            ////tbl.SpacingBefore = 20f;
            ////tbl.SpacingAfter = 20f;

            //// Add funding header.
            //PdfPCell cell = null;

            //cell = new PdfPCell(new Phrase("Project Code: ", TitleFont));
            //cell.Border = 0;
            //cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            //tbl.AddCell(cell);

            //cell = new PdfPCell(new Phrase(dr["ProjectCode"].ToString(), TableFont));
            //cell.Border = 0;
            //cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            //tbl.AddCell(cell);
        }

        private static IEnumerable<DataRow> GetProjectInformation(DataTable dt, int projectId)
        {
            return (from projectData in dt.AsEnumerable()
                    where projectData.Field<int>("ProjectId") == projectId
                    select projectData);
        }

        private static List<int> GetDistinctProjects(DataTable dt)
        {
            return (from DataRow dRow in dt.Rows
                    select (int)dRow["ProjectId"])
                              .Distinct()
                              .ToList<int>();
        }

        private static void GenerateSummaryReport(iTextSharp.text.Document document, DataTable dt, string startDate, string endDate)
        {
            if (dt.Rows.Count > 0)
            {
                string header = string.Empty;

                if (string.IsNullOrEmpty(startDate))
                    startDate = "N/A";
                else
                    startDate = Convert.ToDateTime(startDate).ToString("MMMM dd, yyyy");

                if (string.IsNullOrEmpty(endDate))
                    endDate = DateTime.Now.ToString("MMMM dd, yyyy");
                else
                    endDate = Convert.ToDateTime(endDate).ToString("MMMM dd, yyyy");

                if (startDate.Equals("N/A"))
                    header = "Overall Progress Reports - " + endDate;
                else
                    header = "Overall Progress Reports - " + startDate + " to " + endDate;

                PdfPTable tbl = new PdfPTable(2);

                tbl.KeepTogether = true;
                //relative col widths in proportions - 1/3 and 2/3
                float[] widths = new float[] { 2f, 3f };
                tbl.SetWidths(widths);

                tbl.SpacingAfter = 20f;

                PdfPCell cell = null;

                cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 12, Font.BOLD)));
                cell.PaddingTop = 20;
                cell.Border = 0;
                //cell.BorderWidthBottom = 1;
                cell.Colspan = 2;
                tbl.AddCell(cell);

                cell = new PdfPCell();
                cell.Padding = 5;
                cell.Colspan = 2;
                cell.Border = 0;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of SRP Projects", TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthTop = 1;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["SRPProjectsCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of Organization with SRP projects ", TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["SRPOrgCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);


                cell = new PdfPCell(new Phrase("Number of SRP Projects Funded", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["SRPFundedProjects"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of SRP projects that reported", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["SRPReportedProjects"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of Organizations reporting", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ReportedOrganizationCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of Non SRP Projects", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ProjectCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of Non SRP Projects reported", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["NonSRPReportedProjects"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number of Sahel Countries Reporting", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["CountriesCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);
                

                document.Add(tbl);
            }
        }

        private static void GenerateSummaryCountry(iTextSharp.text.Document document, DataTable dt)
        {
            PdfPTable tbl = new PdfPTable(5);

            tbl.KeepTogether = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 2f, 2f, 2f, 2f };
            tbl.SetWidths(widths);

            tbl.SpacingAfter = 20f;

            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Situation by Country:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell.PaddingTop = 5;
            cell.Border = 0;
            //cell.BorderWidthBottom = 1;
            cell.Colspan = 5;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("#", TableFont));
            cell.Padding = 5;
            cell.Border = 0;
            cell.BorderWidthTop = 1;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Country", TableFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            cell.Padding = 5;
            cell.Border = 0;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthBottom = 1;
            cell.BorderWidthTop = 1;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("SRP Projects Reporting", TableFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            cell.Padding = 5;
            cell.Border = 0;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthBottom = 1;
            cell.BorderWidthTop = 1;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Non SRP Projects", TableFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            cell.Padding = 5;
            cell.Border = 0;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthBottom = 1;
            cell.BorderWidthTop = 1;
            tbl.AddCell(cell);
                       

            cell = new PdfPCell(new Phrase("Number of Reporting Organizations", TableFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            cell.Padding = 5;
            cell.Border = 0;
            cell.BorderWidthLeft = 1;
            cell.BorderWidthTop = 1;
            cell.BorderWidthRight = 1;
            cell.BorderWidthBottom = 1;
            tbl.AddCell(cell);

            var distinctCountries = (from DataRow dRow in dt.Rows
                                     select new
                                     {
                                         Country = dRow["Country"],
                                         SRPReportedProjects = dRow["SRPProjectsReportedByCountry"],
                                         NONSRPReportedProjects = dRow["NONSRPProjectsReportedByCountry"],
                                         Users = dRow["Users"],
                                         Organizations = dRow["Organizations"],
                                         SRPProjectsPerCountry = dRow["SRPProjectsPerCountry"],
                                         NONSRPProjectsPerCountry = dRow["NonSRPProjectsPerCountry"],
                                         OrgPerCountry = dRow["OrgPerCountry"]

                                     })
                                       .Distinct();

            int count = 0;
            foreach (var country in distinctCountries)
            {
                count++;
                cell = new PdfPCell(new Phrase(count.ToString(), TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(country.Country), TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthBottom = 1;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(country.SRPReportedProjects) + " / " + Convert.ToString(country.SRPProjectsPerCountry), TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthBottom = 1;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(country.NONSRPReportedProjects) + " / " + Convert.ToString(country.NONSRPProjectsPerCountry), TableFont));
                cell.Padding = 5;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthBottom = 1;
                tbl.AddCell(cell);


                cell = new PdfPCell(new Phrase(Convert.ToString(country.Organizations) + " / " + Convert.ToString(country.OrgPerCountry), TableFont));
                cell.Padding = 5;
                cell.BorderWidthLeft = 1;               
                cell.BorderWidthBottom = 1;
                cell.BorderWidthRight = 1;
                tbl.AddCell(cell);

                
            }

            document.Add(tbl);
        }

        private static void GenerateSummaryOrganization(iTextSharp.text.Document document, DataTable dt)
        {
            PdfPTable tbl = new PdfPTable(2);

            tbl.KeepTogether = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 5f };
            tbl.SetWidths(widths);

            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Organizations Name by Country:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell.PaddingTop = 5;
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            var distinctCountries = (from DataRow dRow in dt.Rows
                                     select new
                                     {
                                         CountryID = dRow["CountryID"],
                                         Country = dRow["Country"]
                                     })
                                       .Distinct();

            int count = 0;
            foreach (var country in distinctCountries)
            {
                count = 0;

                cell = new PdfPCell(new Phrase(Convert.ToString(country.Country), TableFont));
                cell.PaddingTop = 5;
                cell.Border = 0;
                cell.BorderWidthBottom = 1;
                cell.Colspan = 2;
                tbl.AddCell(cell);

                var distinctOrganizations = (from DataRow dRow in dt.Rows
                                             where dRow.Field<int>("CountryID") == Convert.ToInt32(country.CountryID)
                                             select new
                                             {
                                                 CountryID = dRow["CountryID"],
                                                 OrganizationName = dRow["OrganizationName"]
                                             })
                                        .Distinct();

                foreach (var org in distinctOrganizations)
                {
                    count++;
                    cell = new PdfPCell(new Phrase(count.ToString(), TableFont));
                    cell.Padding = 5;
                    cell.Border = 0;
                    tbl.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Convert.ToString(org.OrganizationName), TableFont));
                    cell.Padding = 5;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthRight = 1;
                    tbl.AddCell(cell);
                }
            }

            document.Add(tbl);
        }

        // Write project main info in pdf document.
        private static void ProjectGeneralInfo(Document document, DataRow dr)
        {
            ProjectMainInfo(document, dr);
            ProjectDetailInfo(document, dr);
            //ProjectDescriptions(document, dr);
        }

        private static void ProjectMainInfo(Document document, DataRow dr)
        {
            PdfPTable tbl = new PdfPTable(2);

            tbl.KeepTogether = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 3f };
            tbl.SetWidths(widths);

            //leave a gap before and after the table
            tbl.SpacingBefore = 20f;
            tbl.SpacingAfter = 10f;

            // Add funding header.
            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Appealing Agency: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["Organizationname"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Project Title: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["ProjectTitle"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Project Code: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["ProjectCode"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Country: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["Country"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Cluster: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["ClusterName"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Sec Cluster: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["SecCluster"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            document.Add(tbl);
        }

        private static void ProjectDetailInfo(Document document, DataRow dr)
        {
            PdfPTable tbl = new PdfPTable(2);
            //!SplitLate && SplitRows
            tbl.SplitLate = false;
            tbl.SplitRows = true;
            
            //tbl.KeepTogether = true;
            float[] widths = new float[] { 1f, 3f };
            tbl.SetWidths(widths);
            tbl.SpacingAfter = 5f;

            // Add funding header.
            PdfPCell cell = null;
            //cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
            //cell.Colspan = 2;
            //cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            //tbl.AddCell(cell);

            tbl.AddCell(new Phrase("Objectives:", TableFont));            
            tbl.AddCell(new Phrase(Convert.ToString(dr["ProjectObjective"]), TableFont));

            tbl.AddCell(new Phrase("Beneficiaries:", TableFont));
            string beneficiariesTotal = string.Format("Total: {0} {1}{2}",
                                                dr["BeneficiaryTotalNumber"].ToString(),
                                                dr["BeneficiariesTotalDescription"].ToString(),
                                                Environment.NewLine
                                                );
            string beneficiariesChildren = "";
            if (dr["BeneficiariesChildren"].ToString() != "" && dr["BeneficiariesChildren"].ToString() != "0")
            {
                beneficiariesChildren = string.Format("Children: {0}{1}",
                                                        dr["BeneficiariesChildren"].ToString(),
                                                        Environment.NewLine
                                                    );
            }

            string beneficiariesWomen = "";
            if (dr["BeneficiariesWomen"].ToString() != "" && dr["BeneficiariesWomen"].ToString() != "0")
            {
                beneficiariesWomen = string.Format("Women: {0}{1}",
                                                        dr["BeneficiariesWomen"].ToString(),
                                                        Environment.NewLine
                                                    );
            }

            string beneficiariesOther = "";
            if (dr["BeneficiariesOthers"].ToString() != "" && dr["BeneficiariesOthers"].ToString() != "0")
            {
                beneficiariesOther = string.Format("Other group: {0} {1}", dr["BeneficiariesOthers"].ToString(), dr["BeneficiariesDescription"].ToString());
            }

            beneficiariesTotal += beneficiariesChildren + beneficiariesWomen + beneficiariesOther;
            tbl.AddCell(new Phrase(beneficiariesTotal, TableFont));

            tbl.AddCell(new Phrase("Implementing Partners:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["ProjectImplementingpartner"]), TableFont));

            tbl.AddCell(new Phrase("Project Duration:", TableFont));
            string projectDuration = dr["ProjectStartDate"].ToString() + " - " + dr["ProjectEndDate"].ToString();
            tbl.AddCell(new Phrase(projectDuration, TableFont));

            tbl.AddCell(new Phrase("Current Funds Requested:", TableFont));
            int originalRequest = 0;
            int.TryParse(dr["OriginalRequest"].ToString(), out originalRequest);
            string requestedFunds = RC.SelectedSiteLanguageId == 1 ?
                string.Format(CultureInfo.InvariantCulture, "{0:n}", originalRequest) :
                string.Format(CultureInfo.CreateSpecificCulture("fr-FR"), "{0:n}", originalRequest);
            
            tbl.AddCell(new Phrase("$" + requestedFunds, TitleFont));

            tbl.AddCell(new Phrase("Priority/Category:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["OPSPriority"]), TableFont));

            //tbl.AddCell(new Phrase("Current Request:", TableFont));
            //tbl.AddCell(new Phrase(Convert.ToString(dr["CurrentRequest"]), TableFont));

            tbl.AddCell(new Phrase("Gender Marker:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["GenderMarker"]), TableFont));

            tbl.AddCell(new Phrase("Status:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["OPSProjectStatus"]), TableFont));

            tbl.AddCell(new Phrase("Contact Details:", TableFont));
            string contactDetails = dr["ProjectContactName"].ToString() + ", " + dr["ProjectContactEmail"].ToString() + ", " + dr["ProjectContactPhone"].ToString();
            tbl.AddCell(new Phrase(contactDetails, TableFont));

            cell = new PdfPCell(new Phrase("", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Needs", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["OPSNeeds"]), TableFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Activities Or Outputs", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["OPSActivities"]), TableFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indicators and Targets", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["OPSIndicatorOutputs"]), TableFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Activity Details", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("", TitleFont));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            document.Add(tbl);
        }

        //private static void ProjectDescriptions(Document document, DataRow dr)
        //{
        //    PdfPTable tbl = new PdfPTable(2);
        //    tbl.KeepTogether = true;
        //    float[] widths = new float[] { 1f, 3f };
        //    tbl.SetWidths(widths);
        //    tbl.SpacingAfter = 10f;

        //    // Add funding header.
        //    PdfPCell cell = null;
        //    cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
        //    cell.Colspan = 2;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    tbl.AddCell(cell);

        //    tbl.AddCell(new Phrase("Objectives:", TableFont));
        //    tbl.AddCell(new Phrase(Convert.ToString(dr["ProjectObjective"]), TableFont));

        //    PdfPCell cell = null;

        //    cell = new PdfPCell(new Phrase("Appealing Agency: ", TitleFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    tbl.AddCell(cell);

        //    cell = new PdfPCell(new Phrase(Convert.ToString(dr["Organizationname"]), TableFont));
        //    cell.Border = 0;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
        //    tbl.AddCell(cell);

        //    document.Add(tbl);
        //}

        private static void TargetsDetails(Document document, DataRow row)
        {
            PdfPTable indicatorTable = new PdfPTable(4);
            PdfPTable logFrameTable = new PdfPTable(1);

            if (PDFExportObjId != (int)row["ObjectiveId"])
            {
                document.Add(new Paragraph(6, "\u00a0"));
                PDFExportObjId = (int)row["ObjectiveId"];
                AddLogFrameItem(document, logFrameTable, row["Objective"].ToString());
            }

            if (PDFExportPrId != (int)row["HumanitarianPriorityId"])
            {
                PDFExportPrId = (int)row["HumanitarianPriorityId"];
                AddLogFrameItem(document, logFrameTable, row["HumanitarianPriority"].ToString());
            }

            if (PDFExportActivityId != (int)row["PriorityActivityId"])
            {
                document.Add(new Paragraph(3, "\u00a0"));
                PDFExportActivityId = (int)row["PriorityActivityId"];
                AddLogFrameItem(document, logFrameTable, row["ActivityName"].ToString());
            }

            if (PDFExportIndicatorId != (int)row["ActivityDataId"])
            {
                PDFExportIndicatorId = (int)row["ActivityDataId"];
                AddLogFrameItem(document, logFrameTable, row["DataName"].ToString());
                AddReportTableHeaders(indicatorTable);
            }

            document.Add(logFrameTable);


            indicatorTable.AddCell(new Phrase(row["Location"].ToString(), TableFont));
            indicatorTable.AddCell(new Phrase(row["TargetAnnual"].ToString(), TableFont));
            indicatorTable.AddCell(new Phrase(row["Achieved"].ToString(), TableFont));
            indicatorTable.AddCell(new Phrase(row["Accumulative"].ToString(), TableFont));

            document.Add(indicatorTable);
        }

        private static void AddLogFrameItem(iTextSharp.text.Document document, PdfPTable tbl, string objective)
        {
            // Add header.
            PdfPCell cell = null;
            cell = new PdfPCell(new Phrase(objective, TitleFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);
        }

        private static void AddIndicatorInTable(iTextSharp.text.Document document, PdfPTable tbl, string objective)
        {
            // Add header.
            PdfPCell cell = null;
            cell = new PdfPCell(new Phrase(objective, TitleFont));
            cell.Colspan = 4;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            tbl.AddCell(cell);
        }

        private static void AddReportTableHeaders(PdfPTable tbl)
        {
            tbl.AddCell(new Phrase("Location", TitleFont));
            tbl.AddCell(new Phrase("Annual Target", TitleFont));
            tbl.AddCell(new Phrase("Achieved", TitleFont));
            tbl.AddCell(new Phrase("Accumulative", TitleFont));
        }

        //private static void Ad2dObjective(iTextSharp.text.Document document, PdfPTable tbl, DataRow row)
        //{
        //    // Add header.
        //    PdfPCell cell = null;
        //    cell = new PdfPCell(new Phrase("" + row["Objective"].ToString(), TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    tbl.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Priority: " + row["HumanitarianPriority"].ToString(), TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    tbl.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Activity: " + row["ActivityName"].ToString(), TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    tbl.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Activ Indicator: " + row["DataName"].ToString(), TitleFont));
        //    cell.Colspan = 4;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    tbl.AddCell(cell);


        //}


        ///************************************New functions***************************************/

        //private static void IPReports(iTextSharp.text.Document document, DataRow row)
        //{
        //    IPReportsGeneralInfo(document, row);
        //    //int spReportId = Convert.ToInt32(row["ProjectSPReportid"].ToString());
        //    //WriteIPReportTargets(document, spReportId);
        //}

        //private DataTable GetReportSubSector(int spReportId)
        //{
        //    DataTable dt = new DataTable();
        //    structResult spReports = _dataContext.GetAllRecordsByID(ConnectionString, "Usp_ProjectGetReportSubSectors", "SPReports", new object[] { spReportId });

        //    if (spReports.intCode == (int)FetchStatus.Success)
        //    {
        //        if (spReports.dstResult.Tables["SPReports"].Rows.Count > 0)
        //        {
        //            dt = spReports.dstResult.Tables["SPReports"];

        //        }
        //    }

        //    return dt;
        //}

        //private DataTable GetIPReports(int projectId, int spId)
        //{
        //    DataTable dt = new DataTable();
        //    structResult spReports = _dataContext.GetAllRecordsByID(ConnectionString, "Usp_ProjectGetAllReportsOfIP", "SPReports", new object[] { projectId, spId });

        //    if (spReports.intCode == (int)FetchStatus.Success)
        //    {

        //        if (spReports.dstResult.Tables["SPReports"].Rows.Count > 0)
        //        {
        //            dt = spReports.dstResult.Tables["SPReports"];
        //        }
        //    }

        //    return dt;
        //}

        //private static void IPReportsGeneralInfo(iTextSharp.text.Document document, DataRow row)
        //{
        //    //PdfPTable reportGeneralPDFTable = new PdfPTable(4);

        //    //// Add header.
        //    ////PdfPCell cell = null;

        //    ////cell = new PdfPCell(new Phrase("Report: " + row["ReportFrequencyName"].ToString(), TitleFont));
        //    ////cell.Colspan = 4;
        //    ////cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    ////reportGeneralPDFTable.AddCell(cell);

        //    //reportGeneralPDFTable.AddCell(new Phrase("Location:", TitleFont));
        //    //reportGeneralPDFTable.AddCell(new Phrase(row["Location"].ToString(), TableFont));
        //    //reportGeneralPDFTable.AddCell(new Phrase("Location Type:", TitleFont));
        //    ////reportGeneralPDFTable.AddCell(new Phrase(row["LocationType"].ToString(), TableFont));
        //    ////reportGeneralPDFTable.AddCell(new Phrase("Start-End Date:", TitleFont));
        //    ////reportGeneralPDFTable.AddCell(new Phrase(string.Format("{0} TO {1}", row["StartDate"].ToString(), row["EndDate"].ToString()), TableFont));
        //    ////reportGeneralPDFTable.AddCell(new Phrase("Report Status:", TitleFont));
        //    ////reportGeneralPDFTable.AddCell(new Phrase(row["ReportStatusName"].ToString(), TableFont));

        //    //document.Add(reportGeneralPDFTable);
        //}

        //private void WriteIPReportTargets(iTextSharp.text.Document document, int spReportId)
        //{


        //    PdfPTable reportTargetsPDFTable = new PdfPTable(12);

        //    // Add header.
        //    PdfPCell cell = null;

        //    //cell = new PdfPCell(new Phrase("Report Targets", TitleFont));
        //    //cell.Colspan = 12;
        //    //cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    //reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Title", TitleFont));
        //    cell.Colspan = 2;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Assigned Target", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Achieved Value", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic Ind Target", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic Ind Achieved", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic Families Target", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic Families Achieved", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic HH Target", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Benefic HH Achieved", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Total Cost", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Total Cost Reported", TitleFont));
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    reportTargetsPDFTable.AddCell(cell);



        //    document.Add(reportTargetsPDFTable);

        //    DataTable ReportSubSectors = GetReportSubSector(spReportId); // Get all the subsectors used in a particular Report

        //    foreach (DataRow row in ReportSubSectors.Rows)
        //    {
        //        PdfPTable reportSubSectorTable = new PdfPTable(3);
        //        // write SubSector Name
        //        cell = new PdfPCell(new Phrase("Sub Sector: " + row["SubSector"], TitleFont));
        //        cell.Colspan = 3;
        //        cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Bisque);
        //        reportSubSectorTable.AddCell(cell);
        //        document.Add(reportSubSectorTable);

        //        int subsectorId = Convert.ToInt32(row["EmergencyPhaseSectorSubSectorID"]);

        //        DataTable dtReportTargets = GetIPReportTargets(spReportId, subsectorId);//Get Activities of a Report that belong to a particular subsector

        //        if (dtReportTargets.Rows.Count > 0)
        //        {
        //            IPReportTargets(document, dtReportTargets);
        //        }
        //    }
        //    AddNewLineInDocument(document, 1);
        //}

        //private DataTable GetIPReportTargets(int reportId, int subsectorId)
        //{
        //    DataTable dt = new DataTable();

        //    structResult spReports = _dataContext.GetAllRecordsByID(ConnectionString, "Usp_ProjectGetReportDetails", "SPReports", new object[] { reportId, subsectorId });

        //    if (spReports.intCode == (int)FetchStatus.Success)
        //    {
        //        if (spReports.dstResult.Tables["SPReports"].Rows.Count > 0)
        //        {
        //            dt = spReports.dstResult.Tables["SPReports"];
        //        }
        //    }

        //    return dt;
        //}

        //private void IPReportTargets(iTextSharp.text.Document document, DataTable dt)
        //{
        //    bool isValid;

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        BaseColor BC = new iTextSharp.text.BaseColor(System.Drawing.Color.Azure);
        //        PdfPTable reportTargetsPDFTable = new PdfPTable(12);
        //        PdfPCell cell = null;

        //        cell = new PdfPCell(new Phrase(row["Activity"].ToString(), TableFont));
        //        cell.Colspan = 2;
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["TargetValue"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["ReportedValue"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficIndividualTotal"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficIndividualTotalReported"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficFamilyTotal"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficFamilyTotalReported"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficHHTotal"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["BeneficHHTotalReported"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["ActivityCostTotal"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);

        //        cell = new PdfPCell(new Phrase(row["ActivityCostTotalReported"].ToString(), TableFont));
        //        cell.BackgroundColor = BC;
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        //        reportTargetsPDFTable.AddCell(cell);


        //        document.Add(reportTargetsPDFTable); //Add the Activity

        //        isValid = !String.IsNullOrEmpty(row["BeneficMale"].ToString()) || !String.IsNullOrEmpty(row["BeneficFemale"].ToString())
        //            || !String.IsNullOrEmpty(row["BeneficBoys"].ToString()) || !String.IsNullOrEmpty(row["BeneficGirls"].ToString())
        //            || !String.IsNullOrEmpty(row["BeneficDisabled"].ToString()) || !String.IsNullOrEmpty(row["BeneficElderly"].ToString())
        //            || !String.IsNullOrEmpty(row["BeneficInfant"].ToString()) || !String.IsNullOrEmpty(row["BeneficKid"].ToString());
        //        if (isValid)
        //        {
        //            document.Add(GetBenefHeaders());
        //            document.Add(GetBenefDetails(row));
        //        }

        //        isValid = !String.IsNullOrEmpty(row["BeneficMaleHH"].ToString()) || !String.IsNullOrEmpty(row["BeneficFemaleHH"].ToString())
        //            || !String.IsNullOrEmpty(row["BeneficChildHH"].ToString()) || !String.IsNullOrEmpty(row["BeneficDisabledHH"].ToString())
        //            || !String.IsNullOrEmpty(row["BeneficElderlyHH"].ToString());

        //        if (isValid)
        //        {
        //            document.Add(GetHHHeaders());
        //            document.Add(GetHHDetails(row));
        //        }

        //    }

        //    //document.Add(reportTargetsPDFTable);


        //}

        //private PdfPTable GetBenefHeaders()
        //{
        //    PdfPTable BenefTable = new PdfPTable(18);
        //    PdfPCell cell = null;
        //    BaseColor BenfColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Peru);

        //    cell = new PdfPCell(new Phrase("Beneficiaries Individual Targets", TitleFont));
        //    cell.Colspan = 18;
        //    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Male Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Male Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Female Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Female Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Boys Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Boys Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Girls Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Girls Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Disabled Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Disabled Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Elderly Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Elderly Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Children Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Children Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Kids Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Kids Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("PLW Target", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("PLW Reached", TableFont));
        //    cell.Rotation = -90;
        //    cell.BackgroundColor = BenfColor;
        //    BenefTable.AddCell(cell);
        //    return (BenefTable);
        //}

        //private PdfPTable GetBenefDetails(DataRow row)
        //{
        //    BaseColor BenfColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Peru);
        //    PdfPTable BenefTable = new PdfPTable(18);
        //    // new PdfPCell(new Phrase("Hello")) { BackgroundColor = color }
        //    //BenefTable.AddCell(new Phrase(row["BeneficMale"].ToString(), TableFont));
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficMale"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficMaleReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficFemale"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficFemaleReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficBoys"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficBoysReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficGirls"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficGirlsReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficDisabled"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficDisabledReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficElderly"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficElderlyReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficInfant"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficInfantReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficKid"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficKidReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficPLW"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficPLWReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

        //    return (BenefTable);
        //}

        //private PdfPTable GetHHHeaders()
        //{
        //    PdfPTable BenefTable = new PdfPTable(18);
        //    PdfPCell cell = null;
        //    BaseColor BenfColor = new iTextSharp.text.BaseColor(System.Drawing.Color.Peru);
        //    BaseColor BenfColor1 = new iTextSharp.text.BaseColor(System.Drawing.Color.PapayaWhip);
        //    BaseColor BenfColor2 = new iTextSharp.text.BaseColor(System.Drawing.Color.PaleTurquoise);


        //    cell = new PdfPCell(new Phrase("Beneficiaries HouseHold Targets", TitleFont));
        //    cell.Colspan = 10;
        //    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    BenefTable.AddCell(cell);


        //    cell = new PdfPCell(new Phrase("Cost Details", TitleFont));
        //    cell.Colspan = 8;
        //    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //    BenefTable.AddCell(cell);

        //    cell = (new PdfPCell(new Phrase("Male Target", TableFont)) { BackgroundColor = BenfColor1 });
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Male Reached", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Female Target", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Female Reported", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Child Target", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Child Reached", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Disabled Target", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Disabled Reached", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Elderly Target", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);
        //    cell = new PdfPCell(new Phrase("Elderly Reached", TableFont)) { BackgroundColor = BenfColor1 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Activity Cost", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Activity Cost Reported", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Admin Cost", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Admin Cost Reported", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Staff Cost", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Staff Cost Reported", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Input Cost", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("Input Cost Reported", TableFont)) { BackgroundColor = BenfColor2 };
        //    cell.Rotation = -90;
        //    BenefTable.AddCell(cell);

        //    return (BenefTable);
        //}

        //private PdfPTable GetHHDetails(DataRow row)
        //{

        //    PdfPTable BenefTable = new PdfPTable(18);
        //    BaseColor BenfColor = new iTextSharp.text.BaseColor(System.Drawing.Color.PapayaWhip);
        //    BaseColor BenfColor2 = new iTextSharp.text.BaseColor(System.Drawing.Color.PaleTurquoise);

        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficMaleHH"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficMaleHHReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficFemaleHH"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficFemaleHHReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficChildHH"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficChildHHReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficDisabledHH"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficDisabledHHReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficElderlyHH"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["BeneficElderlyHHReported"].ToString(), TableFont)) { BackgroundColor = BenfColor, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["ActivityCost"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["ActivityCostReported"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["AdminCost"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["AdminCostReported"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["StaffCost"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["StaffCostReported"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["ActivityOtherCost"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
        //    BenefTable.AddCell(new PdfPCell(new Phrase(row["ActivityOtherCostReported"].ToString(), TableFont)) { BackgroundColor = BenfColor2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

        //    return (BenefTable);
        //}

        ///**************************************************************************************/

        //private bool IsNew(int projectId)
        //{
        //    DataTable dt = new DataTable();

        //    structResult Project = _dataContext.GetAllRecordsByID(ConnectionString, "Usp_ProjectIsNewProject", "Project", new object[] { projectId });

        //    if (Project.intCode == (int)FetchStatus.Success)
        //    {
        //        if (Project.dstResult.Tables["Project"].Rows.Count > 0)
        //        {
        //            DataRow Row = Project.dstResult.Tables["Project"].Rows[0];
        //            if (!string.IsNullOrEmpty(Row["IsNew"].ToString()) && Convert.ToInt32(Row["IsNew"]) == 1)
        //            {
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //    }


        //    return true;
        //}

        //private void AddNewLineInDocument(iTextSharp.text.Document document, int numberOfLines)
        //{
        //    for (int i = 0; i <= numberOfLines; i++)
        //    {
        //        document.Add(new Paragraph("\n"));
        //    }
        //}
    }
}