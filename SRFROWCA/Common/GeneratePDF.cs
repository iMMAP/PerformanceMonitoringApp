using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;

namespace SRFROWCA.Common
{
    public static class WriteDataEntryPDF
    {
        #region Generate PDF Document

        public static void GenerateDocument(iTextSharp.text.Document document, DataTable dt)
        {
            var distinctRows = (from DataRow dRow in dt.Rows
                                select new
                                {
                                    ProjectId = dRow["ProjectId"]
                                }).Distinct();

            foreach (var item in distinctRows)
            {
                int projectId = Convert.ToInt32(item.ProjectId);
                IEnumerable<DataRow> query =
                        from projectData in dt.AsEnumerable()
                        where projectData.Field<int>("ProjectId") == projectId
                        select projectData;
                DataRow dr = query.FirstOrDefault<DataRow>();
                ProjectGeneralInfo(document, dr);
                foreach (var row in query)
                {
                    ServiceProviderInfo(document, row);
                }                
                
                PDFExportObjId = 0;
                PDFExportPrId = 0;
                PDFExportActivityId = 0;

                document.NewPage();
            }
        }

        // Write project main info in pdf document.
        private static void ProjectGeneralInfo(iTextSharp.text.Document document, DataRow dr)
        {
            PdfPTable titleTable = new PdfPTable(2);

            AddProjectTitle(dr, titleTable);
            document.Add(titleTable);

            PdfPTable infoTable = new PdfPTable(2);
            infoTable.KeepTogether = true;
            float[] widths = new float[] { 1f, 3f };
            infoTable.SetWidths(widths);

            infoTable.SpacingAfter = 10f;

            // Add funding header.
            PdfPCell cell = null;
            cell = new PdfPCell(new Phrase("Project General Info", TitleFont));
            cell.Colspan = 2;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            infoTable.AddCell(cell);

            AddProjectInfoInPDFTable(dr, infoTable);
            document.Add(infoTable);
        }

        private static void AddProjectTitle(DataRow dr, PdfPTable pdfTable)
        {
            //PdfPTable projectTitlePDFTable = new PdfPTable(1);
            pdfTable.KeepTogether = true;

            //relative col widths in proportions - 1/3 and 2/3
            float[] widths = new float[] { 1f, 3f };
            pdfTable.SetWidths(widths);

            //leave a gap before and after the table
            pdfTable.SpacingBefore = 20f;
            pdfTable.SpacingAfter = 20f;

            // Add funding header.
            PdfPCell cell = null;

            cell = new PdfPCell(new Phrase("Project Code: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(dr["ProjectCode"].ToString(), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Project Title: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(dr["ProjectTitle"].ToString(), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase("Country: ", TitleFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);

            cell = new PdfPCell(new Phrase(dr["Country"].ToString(), TableFont));
            cell.Border = 0;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.LightGray);
            pdfTable.AddCell(cell);
        }

        private static void AddProjectInfoInPDFTable(DataRow dr, PdfPTable projectTable)
        {
            projectTable.AddCell(new Phrase("Organization:", TableFont));
            projectTable.AddCell(new Phrase(dr["Organizationname"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Start Date:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectStartDate"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("End Date:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectEndDate"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Original Request:", TableFont));
            projectTable.AddCell(new Phrase(dr["OriginalRequest"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Current Request:", TableFont));
            projectTable.AddCell(new Phrase(dr["CurrentRequest"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiaries Children:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiariesChildren"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiaries Women:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiariesWomen"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiaries Others:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiariesOthers"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiary Total:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiaryTotalNumber"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiaries Description:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiariesDescription"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Beneficiaries Total Description:", TableFont));
            projectTable.AddCell(new Phrase(dr["BeneficiariesTotalDescription"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Status:", TableFont));
            projectTable.AddCell(new Phrase(dr["Status_LongDs"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Implementing Partners:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectImplementingpartner"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Contact Name:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectContactName"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Contact Email:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectContactEmail"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Contact Phone:", TableFont));
            projectTable.AddCell(new Phrase(dr["ProjectContactPhone"].ToString(), TableFont));

            projectTable.AddCell(new Phrase("Related URL:", TableFont));
            projectTable.AddCell(new Phrase(dr["RelatedURL"].ToString(), TableFont));
        }

        private static void ServiceProviderInfo(iTextSharp.text.Document document, DataRow row)
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

        private static void Ad2dObjective(iTextSharp.text.Document document, PdfPTable tbl, DataRow row)
        {
            // Add header.
            PdfPCell cell = null;
            cell = new PdfPCell(new Phrase("" + row["Objective"].ToString(), TitleFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Priority: " + row["HumanitarianPriority"].ToString(), TitleFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Activity: " + row["ActivityName"].ToString(), TitleFont));
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            tbl.AddCell(cell);

            cell = new PdfPCell(new Phrase("Indicator: " + row["DataName"].ToString(), TitleFont));
            cell.Colspan = 4;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
            tbl.AddCell(cell);


        }


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

        #endregion
    }
}