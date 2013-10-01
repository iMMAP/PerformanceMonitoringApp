using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.UICommon;

namespace SRFROWCA.Reports
{
    public partial class GenerateReport1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateControls();
        }

        #region Wizard Events
        protected void wzrdReport_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == (int)WizardStepIndex.First)
            {
                PopulateClusters();
                //PopulateOrganizations();
            }
            else if (e.NextStepIndex == (int)WizardStepIndex.Second)
            {
                PutSelectedClustersInList();
                PopulateObjectives();
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
            if (e.CurrentStepIndex == (int)WizardStepIndex.Second)
            {
                GenerateMaps();
            }
        }

        private void GenerateMaps()
        {
            object[] parameters = GetUsersSelectedOptions();
            DataTable dt = GetChartData(parameters);
        }

        private DataTable GetChartData(object[] parameters)
        {
            return DBContext.GetData("", parameters);
        }

        private object[] GetUsersSelectedOptions()
        {
            string locationIds = GetLocationIds();
            LocationTypes locationType = rbCountry.Checked ? LocationTypes.Country : rbAdmin1.Checked ? LocationTypes.Admin1 : LocationTypes.Admin2;
            string clusterIds = GetClustersIds();
            string organizationIds = GetOrganizationIds();
            DateTime dateFrom = fromDate.SelectedDate;
            DateTime dateTo = toDate.SelectedDate;
            int logFrameId = GetLogFrameType();
            string logFrameIds = GetLogFrameIds(logFrameId);

            return new object[] { locationIds, (int)locationType, clusterIds, organizationIds, fromDate, toDate };
        }

        private string GetLogFrameIds(int logFrameId)
        {
            if ((int)LogFrame.Objectives == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlObjectives);
            }
            else if ((int)LogFrame.Indicators == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlIndicators);
            }
            else if ((int)LogFrame.Activities == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlActivities);
            }
            else if ((int)LogFrame.Data == logFrameId)
            {
                return ReportsCommon.GetSelectedValues(ddlData);
            }
            
            return null;
        }

        private int GetLogFrameType()
        {
            foreach (ListItem item in rblReportOn.Items)
            {
                if (item.Selected)
                {
                    return Convert.ToInt32(item.Value);
                }
            }

            return 0;
        }

        private string GetOrganizationIds()
        {
            return ReportsCommon.GetSelectedValues(ddlOrganizations);
        }

        private string GetClustersIds()
        {
            return ReportsCommon.GetSelectedValues(cblClusters);
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

        #endregion

        #region Step1

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

        #region ------ Step 2 -----
        #region Step2 Events

        protected void ddlObjectives_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateIndicators();
        }

        protected void ddlIndicators_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
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

        #endregion
        #endregion

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
            Country = 2,
            Admin1 = 3,
            Admin2 = 4,
        }

        enum LogFrame
        {
            Objectives = 1,
            Indicators = 2,
            Activities = 3,
            Data = 4,
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
    }
}