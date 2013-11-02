using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.ReportsCharts;

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
                PopulateObjectives();
            }
            else if (e.NextStepIndex == (int)WizardStepIndex.Second)
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

        private void GenerateMaps()
        {
            int chartType = Convert.ToInt32(ddlChartType.SelectedValue);
            DataTable dt = GetChartData();
            if (dt.Rows.Count == 0)
            {
                divMessage.InnerHtml = "Your selected options did not produce any report. Please contact to admin of this site for futher details!";
                return;
            }
            else
            {
                divMessage.InnerHtml = @"<b>You have successfully selected all the options. Please click on 'Generate Report' button.
                        <br />It will take a while to generate your report, depending on the options you haveselected!</b>";
            }
            string html = "";

            XDocument doc = new XDocument();
            XElement books = new XElement("LogFrames");
            doc.Add(books);

            var distinctRows = (from DataRow dRow in dt.Rows
                                select new
                                {
                                    LogFrameId = dRow["LogFrameId"],
                                    DurationType = dRow["DurationType"],
                                    YearId = dRow["YearId"]
                                }).Distinct();

            foreach (var item in distinctRows)
            {
                int logFrameId = (int)item.LogFrameId;
                int? durationType = string.IsNullOrEmpty(item.DurationType.ToString()) ? (int?)null : Convert.ToInt32(item.DurationType.ToString());
                int? yearId = string.IsNullOrEmpty(item.YearId.ToString()) ? (int?)null : Convert.ToInt32(item.YearId.ToString());

                IEnumerable<DataRow> query =
                        from chartData in dt.AsEnumerable()
                        where chartData.Field<int>("LogFrameId") == logFrameId
                        && chartData.Field<int?>("DurationType") == durationType
                        && chartData.Field<int?>("YearId") == yearId
                        select chartData;

                // Create a table from the query.
                DataTable filteredTable = query.CopyToDataTable<DataRow>();

                if (filteredTable.Rows.Count > 0)
                {
                    DataRow row = filteredTable.Rows[0];
                    WriteLogFrameInXMLFile(books, row);
                }

                html += " " + ReportsCommon.PrepareTargetAchievedChartData(filteredTable, logFrameId, durationType, yearId, chartType);
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
            //string logFrameType = row["LogFrameType"].ToString();
            string durationTypeId = row["DurationType"].ToString();
            string durationTypeName = row["DurationTypeName"].ToString();
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
            string monthId = row["MonthId"].ToString();
            string monthName = row["MonthName"].ToString();
            string qId = row["QNumber"].ToString();
            string qName = row["QName"].ToString();
            string yearId = row["YearId"].ToString();
            string yearName = row["YearName"].ToString();

            XElement logFrame = new XElement("LogFrame");
            //logFrame.SetAttributeValue("LogFrameType", logFrameType);
            logFrame.SetAttributeValue("DurationTypeName", durationTypeName);
            logFrameRoot.Add(logFrame);

            logFrame.Add(GetElement("DurationType", durationTypeId, durationTypeName));
            logFrame.Add(GetElement("Month", monthId, monthName));
            logFrame.Add(GetElement("Quarter", qId, qName));
            logFrame.Add(GetElement("Year", yearId, yearName));
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
            LogFrame actualLogFrameType = GetActualLogFrameType();
            string logFrameIds = GetLogFrameIds(actualLogFrameType);
            int? durationType = GetDurationType();

            return new object[] { (int)locationType, locationIds, clusterIds, organizationIds,
                                    fromYear, fromMonth, toYear, toMonth, 
                                    (int)actualLogFrameType, logFrameIds, durationType };
        }

        private LocationTypes GetLocationType()
        {
            return rbCountry.Checked ? LocationTypes.Country : rbAdmin1.Checked ? LocationTypes.Admin1 : LocationTypes.Admin2;
        }

        private string GetLocationIds()
        {
            string ids = null;
            if (rbCountry.Checked)
            {
                ids = ddlCountry.SelectedItem.Value;
            }
            else if (rbAdmin1.Checked)
            {
                ids = ReportsCommon.GetSelectedValues(ddlAdmin1Locations);
                if (string.IsNullOrEmpty(ids))
                {
                    ids = ddlCountry.SelectedItem.Value;
                }
            }
            else if (rbAdmin2.Checked)
            {
                ids = ReportsCommon.GetSelectedValues(ddlAdmin2Locations);
                if (string.IsNullOrEmpty(ids))
                {
                    ids = ReportsCommon.GetSelectedValues(ddlAdmin1Locations);
                    if (string.IsNullOrEmpty(ids))
                    {
                        ids = ddlCountry.SelectedItem.Value;
                    }
                }
            }

            return ids;
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

        //private int GetLogFrameTypeFromOptions()
        //{
        //    foreach (ListItem item in rblReportOn.Items)
        //    {
        //        if (item.Selected)
        //            return Convert.ToInt32(item.Value);
        //    }

        //    return 0;
        //}

        private LogFrame GetActualLogFrameType()
        {
            return (LogFrame)GetLogFrameTypeFromDropDown();
        }

        private int GetLogFrameTypeFromDropDown()
        {
            int logFrameType = (int)LogFrame.Data;
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
            PopulateEmergencyClusters();
        }

        protected void ddlEmergency_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateEmergencyClusters();
        }

        #endregion

        #region Step1 Methods.

        private void PopulateControls()
        {
            PopulateCountry();
            PopulateOrganizations();
            PopulateClusters();
        }

        private void PopulateEmergency()
        {
            //TODO: EXCEPTION HANDLE ON GETTING VALUE.
            int locationId = Convert.ToInt32(ddlCountry.SelectedItem.Value);
            DataTable dt = DBContext.GetData("GetEmergenciesOnLocation", new object[] { locationId });
            UI.FillLocationEmergency(ddlEmergency, dt);
        }

        private void PopulateEmergencyClusters()
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergency.SelectedValue, out emergencyId);

            //if (emergencyId > 0)
            {
                FillEmergencyClusters(emergencyId);
            }

            //DataTable dt = DBContext.GetData("GetEmergencyClusters", new object[] { locEmgId });
            //UI.FillEmergnecyClusters(cblClusters, dt);
            //SelectClustersList();
        }

        private void FillEmergencyClusters(int emergencyId)
        {
            DataTable dt = GetEmergencyClusters(emergencyId);
            CheckClusterListBox(dt);
        }

        private void CheckClusterListBox(DataTable dt)
        {
            foreach (ListItem item in cblClusters.Items)
            {
                item.Selected = false;
            }

            foreach (ListItem item in cblClusters.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ClusterId"].ToString().Equals(item.Value))
                    {
                        item.Selected = true;                        
                    }
                }
            }
        }

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
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

        private void PopulateClusters()
        {
            UI.FillClusters(cblClusters);
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
        }

        protected void ddlIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
            PopulateDataItems();
        }

        protected void ddlActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateDataItems();
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            string dir = Server.MapPath("~/GeneratedChartFiles");
            string sessionId = Session.SessionID.ToString();
            string path = (ROWCACommon.CreateFolderForFiles(dir, sessionId)) + "\\";

            GenerateChartReportPDF pdf = new GenerateChartReportPDF();
            string filePath = pdf.GeneratePDF(path);

            try
            {
                DownloadPDF(filePath);
            }
            finally
            {
                DeleteUserFolder(path);               
            }
        }

        private void DownloadPDF(string filePath)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("content-disposition",
                    "attachment; filename=3WReport" + DateTime.Now.ToString("dd-MM-yyyy--hh-mm-ss") + ".pdf");
            Response.TransmitFile(filePath);
            Response.Flush();
        }

        private void DeleteUserFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            di.Delete(true);
        }
    }
}