using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.UICommon;
using System.Xml.Linq;
using System.IO;

namespace SRFROWCA.Reports
{
    public partial class GenerateReport1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            chkDuration.Attributes.Add("onclick", "radioMe(event);");
            PopulateControls();
        }

        #region Wizard Events
        protected void wzrdReport_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == (int)WizardStepIndex.First)
            {
                PopulateClusters();
            }
            else if (e.NextStepIndex == (int)WizardStepIndex.Second)
            {
                PutSelectedClustersInList();
                PopulateObjectives();
            }
            else if (e.NextStepIndex == (int)WizardStepIndex.Third)
            {
                GenerateMaps();
            }
        }

        protected void wzrdReport_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == (int)WizardStepIndex.Zero)
            {
                PutSelectedClustersInList();
            }
        }

        protected void wzrdReport_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            //if (e.CurrentStepIndex == (int)WizardStepIndex.Second)
            //{
            //    GenerateMaps();
            //}
        }

        private void GenerateMaps()
        {
            DataTable dt = GetChartData();
            grdTest.DataSource = dt;
            grdTest.DataBind();
            string html = "";
            int j = 0;

            var x = (from r in dt.AsEnumerable()
                     select r[2]).Distinct().ToList();

            XDocument doc = new XDocument();
            //XDeclaration dec = new XDeclaration("1.0", "utf-16", "true");
            //doc.Declaration = dec;
            //XProcessingInstruction pi = new XProcessingInstruction("LogFrameType", "Objective");
            //doc.Add(pi);
            //XComment comm = new XComment("test comments");
            //doc.Add(comm);

            XElement books = new XElement("LogFrames");
            doc.Add(books);            

            foreach (var item in x)
            {
                int i = (int)item;
                IEnumerable<DataRow> query =
                        from order in dt.AsEnumerable()
                        where order.Field<int>(2) == i
                        select order;

                // Create a table from the query.
                DataTable filteredTable = query.CopyToDataTable<DataRow>();
                int id = 0;
                if (filteredTable.Rows.Count > 0)
                {
                    DataRow row = filteredTable.Rows[0];
                    id = Convert.ToInt32(row["LogFrameId"].ToString());
                    WriteLogFrameInXMLFile(books, row);
                }
                html += " " + ReportsCommon.PrepareTargetAchievedChartData(filteredTable, id);
                j++;
            }

            string rootDir = Server.MapPath("~/GeneratedChartFiles");
            string dirForCurrentUser = rootDir + "\\" + Session.SessionID.ToString();
            if (!Directory.Exists(dirForCurrentUser))
            {
                Directory.CreateDirectory(dirForCurrentUser);
            }

            doc.Save(dirForCurrentUser + "\\logframe.xml");

            ltrChart.Text = html;
        }

        private void WriteLogFrameInXMLFile(XElement logFrameRoot, DataRow row)
        {
            string logFrameType = row["LogFrameType"].ToString();
            string dataId = row["DataId"].ToString();
            string dataName = row["DataName"].ToString();
            string activityId = row["ActivityId"].ToString();
            string activityName = row["ActivityName"].ToString();
            string indicatorId = row["IndicatorId"].ToString();
            string indicatorName = row["IndicatorName"].ToString();
            string objId = row["ObjectiveId"].ToString();
            string objName = row["ObjectiveName"].ToString();
            string clusterId = row["ClusterId"].ToString();
            string clusterName = row["ClusterName"].ToString();

            XElement logFrame = new XElement("LogFrame");
            logFrame.SetAttributeValue("LogFrameType", logFrameType);
            logFrameRoot.Add(logFrame);

            logFrame.Add(GetElement("Cluster", clusterId, clusterName));
            logFrame.Add(GetElement("Objective", objId, objName));
            logFrame.Add(GetElement("Indicator", indicatorId, indicatorName));
            logFrame.Add(GetElement("Activity", activityId, activityName));
            logFrame.Add(GetElement("Data", dataId, dataName));
        }

        private XElement GetElement(string name, string idValue, string nameValue)
        {
            XElement element = new XElement(name);
            element.SetAttributeValue("Id", idValue);

            if (!string.IsNullOrEmpty(nameValue))
            {
                element.Add(GetElement(nameValue));
            }
            return element;
        }

        private XElement GetElement(string text)
        {
            XElement element = new XElement("Name");
            element.Value = text;

            return element;
        }

        private DataTable GetChartData()
        {
            object[] parameters = GetUsersSelectedOptions();
            return DBContext.GetData("GetDataForChartsAndMaps", parameters);
        }

        private object[] GetUsersSelectedOptions()
        {
            LocationTypes locationType = GetLocationType();
            string locationIds = GetLocationIds();
            string clusterIds = GetClustersIds();
            string organizationIds = GetOrganizationIds();
            int? fromYear = GetDatePartFromString(txtFromDate, (int)DatePart.Year);
            int? fromMonth = GetDatePartFromString(txtFromDate, (int)DatePart.Month);
            int? toYear = GetDatePartFromString(txtToDate, (int)DatePart.Year);
            int? toMonth = GetDatePartFromString(txtToDate, (int)DatePart.Month);
            int selectedLogFrameType = GetLogFrameTypeFromOptions();
            LogFrame actualLogFrameType = GetActualLogFrameType(selectedLogFrameType);
            string logFrameIds = GetLogFrameIds(actualLogFrameType);
            int? durationType = GetDurationType();

            return new object[] { (int)locationType, locationIds, clusterIds, organizationIds,
                                    fromYear, fromMonth, toYear, toMonth, selectedLogFrameType, 
                                    (int)actualLogFrameType, logFrameIds, durationType };
        }

        private LocationTypes GetLocationType()
        {
            return rbCountry.Checked ? LocationTypes.Country : rbAdmin1.Checked ? LocationTypes.Admin1 : LocationTypes.Admin2;
        }

        private string GetLocationIds()
        {
            if (rbCountry.Checked)
            {
                return ddlCountry.SelectedItem.Value;
            }
            else if (rbAdmin1.Checked)
            {
                return ReportsCommon.GetSelectedValues(ddlAdmin1Locations);
            }
            else if (rbAdmin2.Checked)
            {
                return ReportsCommon.GetSelectedValues(ddlAdmin2Locations);
            }

            return "";
        }

        private string GetClustersIds()
        {
            return ReportsCommon.GetSelectedValues(cblClusters);
        }

        private string GetOrganizationIds()
        {
            return ReportsCommon.GetSelectedValues(ddlOrganizations);
        }

        private int? GetDatePartFromString(TextBox txtBox, int datePartIndex)
        {
            return !string.IsNullOrEmpty(txtBox.Text.Trim()) ? Convert.ToInt32((txtBox.Text.Split('/'))[datePartIndex]) : (int?)null;
        }

        private int GetLogFrameTypeFromOptions()
        {
            foreach (ListItem item in rblReportOn.Items)
            {
                if (item.Selected)
                    return Convert.ToInt32(item.Value);
            }

            return 0;
        }

        private LogFrame GetActualLogFrameType(int selectedLogFrameType)
        {
            return (LogFrame)GetLogFrameTypeFromDropDown(selectedLogFrameType);
        }

        private int GetLogFrameTypeFromDropDown(int logFrameType)
        {
            while (logFrameType > 0)
            {
                if (!string.IsNullOrEmpty(GetLogFrameIds((LogFrame)logFrameType))) break;

                logFrameType -= 1;
            }

            return logFrameType;
        }

        private int? GetSelectedDateYear(DateTime dateTime)
        {
            return dateTime.Year;
        }

        private string GetLogFrameIds(LogFrame logFrameId)
        {
            if (LogFrame.Objectives == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlObjectives);
            }
            else if (LogFrame.Indicators == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlIndicators);
            }
            else if (LogFrame.Activities == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlActivities);
            }
            else if (LogFrame.Data == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlData);
            }

            return null;
        }

        private int? GetDurationType()
        {
            foreach (ListItem item in chkDuration.Items)
            {
                if (item.Selected)
                {
                    return Convert.ToInt32(item.Value);
                }
            }

            return (int?)null;
        }

        #endregion

        #region Step1 Events & Methods.

        #region Step1 Events.
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateChildLocations();
            PopulateEmergency();
        }
        #endregion

        #region Step1 Methods.

        private void PopulateControls()
        {
            PopulateCountry();
            PopulateOrganizations();
        }

        private void PopulateEmergency()
        {
            //TODO: EXCEPTION HANDLE ON GETTING VALUE.
            int locationId = Convert.ToInt32(ddlCountry.SelectedItem.Value);
            DataTable dt = DBContext.GetData("GetEmergenciesOnLocation", new object[] { locationId });
            UI.FillLocationEmergency(ddlEmergency, dt);
        }

        private void PopulateClusters()
        {
            //TODO: EXCEPTION HANDLING.
            int locEmgId = Convert.ToInt32(ddlEmergency.SelectedItem.Value);
            DataTable dt = DBContext.GetData("GetEmergencyClusters", new object[] { locEmgId });
            UI.FillEmergnecyClusters(cblClusters, dt);
            SelectClustersList();
        }

        private void PutSelectedClustersInList()
        {
            List<string> selectedItems = new List<string>();
            foreach (ListItem item in cblClusters.Items)
            {
                if (item.Selected)
                {
                    selectedItems.Add(item.Value);
                }
            }

            SelectedClusterIds = selectedItems;
        }

        private void SelectClustersList()
        {
            if (SelectedClusterIds.Count == 0)
            {
                foreach (ListItem item in cblClusters.Items)
                {
                    item.Selected = true;
                }
            }
            else
            {
                foreach (ListItem item in cblClusters.Items)
                {
                    item.Selected = SelectedClusterIds.Contains(item.Value);
                }
            }
        }

        private void PopulateCountry()
        {
            DataTable dt = DBContext.GetData("GetCountries");
            UI.FillLocations(ddlCountry, dt);

            ListItem item = new ListItem();
            item.Text = "Select Country";
            item.Value = "0";

            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateOrganizations()
        {
            UI.FillOrganizations(ddlOrganizations);
        }

        private void PopulateAdmin1(int countryId)
        {
            DataTable dt = DBContext.GetData("GetAdmin1LocationsOfCountry", new object[] { countryId });
            UI.FillLocations(ddlAdmin1Locations, dt);
        }

        private void PopulateAdmin2(int countryId)
        {
            DataTable dt = DBContext.GetData("GetAdmin2LocationsOfCountry", new object[] { countryId });
            UI.FillLocations(ddlAdmin2Locations, dt);
        }

        private void PopulateChildLocations()
        {
            int countryId = 0;
            int.TryParse(ddlCountry.SelectedValue, out countryId);
            if (countryId > 0)
            {
                PopulateAdmin1(countryId);
                PopulateAdmin2(countryId);
            }
        }

        #endregion

        #endregion

        #region Step 2 Events & Methods.
        #region Step2 Events

        protected void ddlObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateIndicators();
            PopulateActivities();
            PopulateDataItems();
            int dropDownIndex = GetLogFrameTypeFromDropDown((int)LogFrame.Objectives);
            CheckLogFrameRadio(dropDownIndex);
        }

        protected void ddlIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
            PopulateDataItems();
            int dropDownIndex = GetLogFrameTypeFromDropDown((int)LogFrame.Indicators);
            CheckLogFrameRadio(dropDownIndex);
        }

        protected void ddlActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDataItems();
            int dropDownIndex = GetLogFrameTypeFromDropDown((int)LogFrame.Activities);
            CheckLogFrameRadio(dropDownIndex);
        }

        protected void ddlData_SelectedIndexChanged(object sender, EventArgs e)
        {
            int dropDownIndex = GetLogFrameTypeFromDropDown((int)LogFrame.Data);
            CheckLogFrameRadio(dropDownIndex);
        }

        #endregion
        #region Step2 Methods.

        private void PopulateObjectives()
        {
            DataTable dt = GetObjectives();
            UI.FillObjectives(ddlObjectives, dt);
        }

        private DataTable GetObjectives()
        {
            string clusterIds = ReportsCommon.GetSelectedValues(cblClusters);
            return DBContext.GetData("GetObjectivesOfMultipleClusters", new object[] { clusterIds });
        }

        private void PopulateIndicators()
        {
            DataTable dt = GetIndicators();
            UI.FillIndicators(ddlIndicators, dt);
        }

        private DataTable GetIndicators()
        {
            string objIds = ReportsCommon.GetSelectedValues(ddlObjectives);
            return DBContext.GetData("GetIndicatorsOfMultipleObjectives", new object[] { objIds });
        }

        private void PopulateActivities()
        {
            DataTable dt = GetActivities();
            UI.FillActivities(ddlActivities, dt);
        }

        private DataTable GetActivities()
        {
            string indicatorIds = ReportsCommon.GetSelectedValues(ddlIndicators);
            return DBContext.GetData("GetActivitiesOfMultipleIndicators", new object[] { indicatorIds });
        }

        private void PopulateDataItems()
        {
            DataTable dt = GetActivityData();
            UI.FillDataItems(ddlData, dt);
        }

        private DataTable GetActivityData()
        {
            string activityIds = ReportsCommon.GetSelectedValues(ddlActivities);
            return DBContext.GetData("GetDatItemsOfMultipleActivities", new object[] { activityIds });
        }

        private void CheckLogFrameRadio(int index)
        {

            ToggleAllList(rblReportOn, false);
            --index;
            index = index < 0 ? 0 : index;
            rblReportOn.Items[index].Selected = true;
        }

        private void ToggleAllList(ListControl lstControl, bool isSelected)
        {
            foreach (ListItem item in lstControl.Items)
                item.Selected = isSelected;
        }

        #endregion
        #endregion

        #region Enums & Properties

        enum WizardStepIndex
        {
            Zero = 0,
            First = 1,
            Second = 2,
            Third = 3,
            Last = 4,
        }

        enum LocationTypes
        {
            Country = 1,
            Admin1 = 2,
            Admin2 = 3,
        }

        enum LogFrame
        {
            Objectives = 1,
            Indicators = 2,
            Activities = 3,
            Data = 4,
        }

        enum DatePart
        {
            Day = 0,
            Month = 1,
            Year = 2,
        }

        public List<string> SelectedClusterIds
        {
            get
            {
                List<string> selectedClusterIds = new List<string>();
                if (ViewState["SelectedClusterIds"] != null)
                {
                    selectedClusterIds = (List<string>)ViewState["SelectedClusterIds"];
                }

                return selectedClusterIds;
            }
            set
            {
                ViewState["SelectedClusterIds"] = value;
            }
        }

        #endregion

        [WebMethod(EnableSession = true)]
        public void TestPDF()
        {

        }

        //[WebMethod(EnableSession = true)]
        //public void GeneratePDF(int j)
        //{
        //    string pdfpath = Server.MapPath("img2");

        //    if (!Directory.Exists(pdfpath + Session.SessionID.ToString()))
        //    {
        //        Directory.CreateDirectory(pdfpath + Session.SessionID.ToString());
        //    }

        //    string dir = pdfpath + Session.SessionID.ToString();

        //    string imagepath = "E:\\img\\" + Session.SessionID.ToString() + "\\";
        //    //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
        //    using (iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 8, 8, 14, 6))
        //    {
        //        using (MemoryStream outputStream = new MemoryStream())
        //        {
        //            try
        //            {
        //                using (PdfWriter writer = PdfWriter.GetInstance(doc, outputStream))
        //                {
        //                    //PdfWriter.GetInstance(doc, new FileStream(pdfpath + "Charts" + DateTime.Now.ToString(), FileMode.Create));
        //                    doc.Open();
        //                    PdfPTable projectTitlePDFTable = new PdfPTable(2);

        //                    //AddProjectTitle(projectTitlePDFTable);
        //                    //AddNewLineInDocument(document, 1);
        //                    //doc.Add(projectTitlePDFTable);

        //                    PdfPTable projectMainInfoTable = new PdfPTable(2);
        //                    projectMainInfoTable.KeepTogether = true;
        //                    float[] widths = new float[] { 1f, 3f };
        //                    projectMainInfoTable.SetWidths(widths);

        //                    projectMainInfoTable.SpacingAfter = 10f;

        //                    // Add funding header.
        //                    PdfPCell cell = null;
        //                    cell = new PdfPCell(new iTextSharp.text.Phrase("Project General Info", TitleFont));
        //                    cell.Colspan = 2;
        //                    cell.BackgroundColor = new iTextSharp.text.BaseColor(System.Drawing.Color.DarkGray);
        //                    projectMainInfoTable.AddCell(cell);

        //                    for (int i = 0; i < j; i++)
        //                    {
        //                        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(imagepath + Session.SessionID.ToString() + i.ToString() + ".jpg");
        //                        doc.Add(gif);
        //                    }

        //                    DirectoryInfo di = new DirectoryInfo("E:\\img\\" + Session.SessionID.ToString());
        //                    di.Delete(true);

        //                    Response.ContentType = "application/pdf";
        //                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Charts-{0}.pdf", DateTime.Now.ToString()));
        //                    Response.BinaryWrite(outputStream.ToArray());
        //                }
        //            }

        //            catch (Exception ex)
        //            {
        //                //Log error;
        //            }
        //            finally
        //            {

        //            }
        //        }
        //    }
        //}

        //private iTextSharp.text.Font TitleFont
        //{
        //    get
        //    {
        //        return iTextSharp.text.FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
        //    }
        //}

        //private iTextSharp.text.Font TableFont
        //{
        //    get
        //    {
        //        return iTextSharp.text.FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
        //    }
        //}

    }
}