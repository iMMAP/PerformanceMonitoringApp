using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Globalization;
using BusinessLogic;

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

        public static MemoryStream GenerateProjectsListingPDF(DataTable dtProjects, bool isShowContactInfo)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);

                document.Open();
                GenerateProjectsListingDocument(document, dtProjects, isShowContactInfo);
                try
                {
                    document.Close();
                }
                catch { }

                return outputStream;
            }
        }

        public static MemoryStream GenerateSummaryPDF(DataTable dtSummary, DataTable dtCountryOrgs, string month, string country, string cluster)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);
                document.Open();
                GenerateSummaryReport(document, dtSummary, month, country, cluster);
                GenerateSummaryCountry(document, dtSummary);
                GenerateSummaryOrganization(document, dtCountryOrgs);
                try
                {
                    document.Close();
                }
                catch { }
                return outputStream;
            }
        }

        private static void GenerateProjectsListingDocument(iTextSharp.text.Document document, DataTable dt, bool isShowContactInfo)
        {
            foreach (DataRow row in dt.Rows)
            {
                ProjectGeneralInfo(document, row, isShowContactInfo);
                int projectId = 0;
                int.TryParse(row["ProjectId"].ToString(), out projectId);
                if (projectId > 0)
                {
                    DataTable dtIndicators = DBContext.GetData("GetProjectIndicators", new object[] { projectId, RC.SelectedSiteLanguageId });
                    ProjectReports(document, dtIndicators);
                }


                document.NewPage();
            }
        }

        private static void ProjectReports(iTextSharp.text.Document document, DataTable dtIndicators)
        {
            PdfPTable tbl = new PdfPTable(new float[] { 2f, 3f, 3f, 2f, 4f, 2f, 3f, 2f });
            tbl.SpacingAfter = 10f;
            document.Add(tbl);
            IndicatorTargets(document, dtIndicators);

            //else
            //{
            //    DataTable dtFiltered = new DataTable();
            //    string[] selectedColumns = new[] { "ReportID", "ReportName", "OrganizationName", "Month", "CreatedBy", 
            //                                                                    "CreatedDate", "UpdatedBy", "UpdatedDate" };
            //    try
            //    {
            //        dtFiltered = dt.DefaultView.ToTable(true, selectedColumns);
            //    }
            //    catch { }

            //    for (int i = 0; i < dtFiltered.Rows.Count; i++)
            //    {
            //        if (!string.IsNullOrEmpty(Convert.ToString(dtFiltered.Rows[i]["ReportID"])))
            //        {
            //            PdfPTable tbl = new PdfPTable(new float[] { 2f, 3f, 3f, 2f, 4f, 2f, 3f, 2f });
            //            tbl.SpacingAfter = 10f;
            //            document.Add(tbl);
            //            IndicatorTargets(document, (from projectData in dt.AsEnumerable()
            //                                        where projectData.Field<int?>("ReportID") == (int?)Convert.ToInt32(dtFiltered.Rows[i]["ReportID"])
            //                                        select projectData), false);
            //        }
            //    }
            //}
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

        private static void IndicatorTargets(Document document, DataTable dt)
        {
            var distinctObjectives = (from DataRow dRow in dt.Rows
                                      select new
                                          {
                                              ObjectiveId = dRow["ObjectiveId"]
                                          })
                                        .Distinct();

            foreach (var objective in distinctObjectives)
            {
                IEnumerable<DataRow> temp = (from ob in dt.AsEnumerable()
                                             where ob.Field<int>("ObjectiveId") == (int)objective.ObjectiveId
                                             select ob);

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

                    tbl1.SpacingBefore = 3f;
                    tbl1.SpacingAfter = 3f;
                    document.Add(tbl1);
                }


                var distinctIndicators = (from DataRow dRow in dt1.Rows
                                          select new
                                          {
                                              ActivityId = dRow["ActivityId"],
                                              IndicatorId = dRow["IndicatorId"]
                                          })
                                        .Distinct();


                foreach (var indicator in distinctIndicators)
                {
                    IEnumerable<DataRow> targets = (from ind in dt1.AsEnumerable()
                                                    where ind.Field<int>("ActivityId") == (int)indicator.ActivityId
                                                    && ind.Field<int>("IndicatorId") == (int)indicator.IndicatorId
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
                        ReportedData(document, tbl, row);
                    }

                    tbl.SpacingAfter = 10f;
                    document.Add(tbl);
                }
            }
        }

        private static void ReportedData(Document document, PdfPTable tbl, DataRow row)
        {
            tbl.AddCell(new Phrase(row["LocationName"].ToString(), TableFont));
            tbl.AddCell(new Phrase(row["Target"].ToString(), TableFont));
        }

        private static void AddLogFrameInfo(Document document, DataRow dr)
        {
            PdfPTable tbl = new PdfPTable(2);
            tbl.DefaultCell.Border = Rectangle.NO_BORDER;
            tbl.KeepTogether = true;

            float[] widths = new float[] { 1f, 3f };
            tbl.SetWidths(widths);

            tbl.AddCell(new Phrase("Activity:", TableFont));
            tbl.AddCell(new Phrase(dr["Activity"].ToString(), TableFont));
            tbl.AddCell(new Phrase("Indicator:", TableFont));
            tbl.AddCell(new Phrase(dr["Indicator"].ToString(), TableFont));

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

        private static void GenerateSummaryReport(iTextSharp.text.Document document, DataTable dt, string month, string country, string cluster)
        {

            PdfPTable tbl = new PdfPTable(2);
            tbl.KeepTogether = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 2f, 3f };
            tbl.SetWidths(widths);

            tbl.SpacingAfter = 20f;

            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Sahel ORS Projects Reporting Summary:", FontFactory.GetFont("Arial", 10)));
            cell.PaddingTop = 5;
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            string monthHeader = "Month - " + month;
            cell = new PdfPCell(new Phrase(monthHeader, FontFactory.GetFont("Arial", 10)));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            string countryHeader = "Country - " + country;
            cell = new PdfPCell(new Phrase(countryHeader, FontFactory.GetFont("Arial", 10)));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            string clusterHeader = "Cluster - " + cluster;
            cell = new PdfPCell(new Phrase(clusterHeader, FontFactory.GetFont("Arial", 10)));
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            cell = new PdfPCell();
            cell.Colspan = 2;
            cell.Border = 0;
            tbl.AddCell(cell);

            if (dt.Rows.Count > 0)
            {
                cell = new PdfPCell(new Phrase("Total Projects ", TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthTop = 1;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["Projects"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total Organizations ", TableFont));
                cell.Padding = 5;
                cell.Border = 0;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["Organizations"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);


                cell = new PdfPCell(new Phrase("Reporting Projects ", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ReportingProjects"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Reporting Organizations ", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ReportingOrganizations"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Reporting Countries ", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ReportingCountriesCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Number Of Reports ", TableFont));
                cell.Border = 0;
                cell.Padding = 5;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase(Convert.ToString(dt.Rows[0]["ReportsCount"]), TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                tbl.AddCell(cell);
            }

            document.Add(tbl);
        }

        private static void GenerateSummaryCountry(iTextSharp.text.Document document, DataTable dt)
        {
            if (dt.Rows.Count > 0 && dt.Rows[0]["Country"] != "")
            {
                PdfPTable tbl = new PdfPTable(4);

                tbl.KeepTogether = true;
                //relative col widths in proportions - 1/3 and 2/3
                float[] widths = new float[] { 1f, 2f, 2f, 2f };
                tbl.SetWidths(widths);

                tbl.SpacingAfter = 20f;

                PdfPCell cell = null;

                cell = new PdfPCell(new Phrase("Country Situation:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
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

                cell = new PdfPCell(new Phrase("Projects/Reporting", TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                tbl.AddCell(cell);

                cell = new PdfPCell(new Phrase("Organizations/Reporting", TableFont));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
                cell.Padding = 5;
                cell.Border = 0;
                cell.BorderWidthLeft = 1;
                cell.BorderWidthBottom = 1;
                cell.BorderWidthTop = 1;
                tbl.AddCell(cell);


                var distinctCountries = (from DataRow dRow in dt.Rows
                                         select new
                                         {
                                             Country = dRow["Country"],
                                             TotalProjectsCountByCountry = dRow["TotalProjectsCountByCountry"],
                                             ReportingProjectsCountByCountry = dRow["ReportingProjectsCountByCountry"],
                                             TotalOrgsCountByCountry = dRow["TotalOrgsCountByCountry"],
                                             ReportingOrgsCountByCountry = dRow["ReportingOrgsCountByCountry"],

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

                    cell = new PdfPCell(new Phrase(Convert.ToString(country.TotalProjectsCountByCountry) + " / " + Convert.ToString(country.ReportingProjectsCountByCountry), TableFont));
                    cell.Padding = 5;
                    cell.Border = 0;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthBottom = 1;
                    tbl.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Convert.ToString(country.TotalOrgsCountByCountry) + " / " + Convert.ToString(country.ReportingOrgsCountByCountry), TableFont));
                    cell.Padding = 5;
                    cell.BorderWidthLeft = 1;
                    cell.BorderWidthBottom = 1;
                    tbl.AddCell(cell);
                }
                document.Add(tbl);
            }
        }

        private static void GenerateSummaryOrganization(iTextSharp.text.Document document, DataTable dt)
        {
            PdfPTable tbl = new PdfPTable(2);

            tbl.KeepTogether = true;
            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 5f };
            tbl.SetWidths(widths);

            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Organizations Reporting:", FontFactory.GetFont("Arial", 9, Font.BOLD)));
            cell.PaddingTop = 5;
            cell.Border = 0;
            cell.Colspan = 2;
            tbl.AddCell(cell);

            var distinctCountries = (from DataRow dRow in dt.Rows
                                     select new
                                     {
                                         CountryID = dRow["CountryId"],
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
                                             where dRow.Field<int>("CountryId") == Convert.ToInt32(country.CountryID)
                                             select new
                                             {
                                                 CountryID = dRow["CountryId"],
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
        private static void ProjectGeneralInfo(Document document, DataRow dr, bool isShowContactInfo)
        {
            ProjectMainInfo(document, dr);
            ProjectDetailInfo(document, dr, isShowContactInfo);
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

            cell = new PdfPCell(new Phrase("Sub-Set Cluster: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase(Convert.ToString(dr["SubSetCluster"]), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            tbl.AddCell(cell);

            document.Add(tbl);
        }

        private static void ProjectDetailInfo(Document document, DataRow dr, bool isShowContactInfo)
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

            tbl.AddCell(new Phrase("Gender Marker:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["GenderMarker"]), TableFont));

            tbl.AddCell(new Phrase("Status:", TableFont));
            tbl.AddCell(new Phrase(Convert.ToString(dr["OPSProjectStatus"]), TableFont));

            if (isShowContactInfo)
            {
                tbl.AddCell(new Phrase("Contact Details:", TableFont));
                string contactDetails = dr["ProjectContactName"].ToString() + ", " + dr["ProjectContactEmail"].ToString() + ", " + dr["ProjectContactPhone"].ToString();
                tbl.AddCell(new Phrase(contactDetails, TableFont));
            }

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

        #region Clsuter Framewrok
        public static MemoryStream GenerateClusterFrameworkPDF(DataTable dtProjects)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, outputStream);

                document.Open();
                GenerateClsuterFrameworkDocument(document, dtProjects);
                try
                {
                    document.Close();
                }
                catch { }

                return outputStream;
            }
        }

        private static void GenerateClsuterFrameworkDocument(iTextSharp.text.Document document, DataTable dt)
        {
            //PdfPTable tbl = new PdfPTable(new float[] { 2f, 3f, 3f, 2f, 4f, 2f, 3f, 2f });
            PdfPTable tbl = new PdfPTable(2);
            tbl.SpacingAfter = 10f;
                        
            WriteFrameworkCountry(document, tbl, dt);
        }

        private static void WriteFrameworkCountry(Document document, PdfPTable tbl, DataTable dt)
        {
            DataTable dtCountryData = new DataTable();
            var countries = (from DataRow dRow in dt.Rows
                             select new
                             {
                                 CountryId = dRow["CountryId"]
                             })
                                .Distinct();

            foreach (var country in countries)
            {
                IEnumerable<DataRow> countryData = (from c in dt.AsEnumerable()
                                                    where c.Field<int>("CountryId") == (int)country.CountryId
                                                    select c);

                dtCountryData = countryData.CopyToDataTable();
                DataRow logFrame = countryData.FirstOrDefault<DataRow>();
                if (logFrame != null)
                {
                    PdfPCell cell = null;
                    var phraseColor = new BaseColor(216, 216, 216);
                    cell = new PdfPCell(new Phrase(logFrame["Country"].ToString(), TableFont));
                    cell.Border = 0;
                    cell.BackgroundColor = phraseColor;
                    tbl.AddCell(cell);
                    tbl.SpacingBefore = 3f;
                    tbl.SpacingAfter = 3f;
                    document.Add(tbl);
                    if (dtCountryData.Rows.Count > 0)
                    {
                        WriteFrameworkCluster(document, tbl, dtCountryData);                        
                    }
                }
            }
        }

        private static void WriteFrameworkCluster(Document document, PdfPTable tbl, DataTable dt)
        {
            DataTable dtClusterData = new DataTable();
            var clusters = (from DataRow dRow in dt.Rows
                             select new
                             {
                                 ClusterId = dRow["ClusterId"]
                             })
                                .Distinct();

            foreach (var cluster in clusters)
            {
                IEnumerable<DataRow> clusterData = (from c in dt.AsEnumerable()
                                                    where c.Field<int>("ClusterId") == (int)cluster.ClusterId
                                                    select c);

                dtClusterData = clusterData.CopyToDataTable();
                DataRow logFrame = clusterData.FirstOrDefault<DataRow>();
                if (logFrame != null)
                {
                    PdfPCell cell = null;
                    var phraseColor = new BaseColor(216, 216, 216);
                    cell = new PdfPCell(new Phrase(logFrame["Cluster"].ToString(), TableFont));
                    cell.Border = 0;
                    cell.BackgroundColor = phraseColor;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tbl.AddCell(cell);
                    tbl.SpacingBefore = 3f;
                    tbl.SpacingAfter = 3f;
                    document.Add(tbl);

                    if (dtClusterData.Rows.Count > 0)
                    {
                        ClusterFrameworkTarget(document, dtClusterData);
                        document.NewPage();
                    }
                }
            }
        }

        private static void ClusterFrameworkTarget(Document document, DataTable dt)
        {
            var distinctObjectives = (from DataRow dRow in dt.Rows
                                      select new
                                      {
                                          ObjectiveId = dRow["ObjectiveId"]
                                      })
                                        .Distinct();

            foreach (var objective in distinctObjectives)
            {
                IEnumerable<DataRow> temp = (from ob in dt.AsEnumerable()
                                             where ob.Field<int>("ObjectiveId") == (int)objective.ObjectiveId
                                             select ob);

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

                    tbl1.SpacingBefore = 3f;
                    tbl1.SpacingAfter = 3f;
                    document.Add(tbl1);
                }


                var distinctIndicators = (from DataRow dRow in dt1.Rows
                                          select new
                                          {
                                              ActivityId = dRow["ActivityId"],
                                              IndicatorId = dRow["IndicatorId"]
                                          })
                                        .Distinct();


                foreach (var indicator in distinctIndicators)
                {
                    IEnumerable<DataRow> targets = (from ind in dt1.AsEnumerable()
                                                    where ind.Field<int>("ActivityId") == (int)indicator.ActivityId
                                                    && ind.Field<int?>("IndicatorId") == (int?)indicator.IndicatorId
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
                        ReportedData(document, tbl, row);
                    }

                    tbl.SpacingAfter = 10f;
                    document.Add(tbl);
                }
            }
        }

        #endregion
    }
}