using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;
using System.Transactions;

namespace SRFROWCA.ClusterLead
{
    public partial class AddSRPActivity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjective();
            PopulatePriority();
            PopulateUnits();
        }

        private void PopulateObjective()
        {
            UI.FillObjectives(ddlObjective);

            ListItem item = new ListItem("Select Objective", "0");
            ddlObjective.Items.Insert(0, item);
        }

        private void PopulatePriority()
        {
            UI.FillPriorities(ddlPriority);

            ListItem item = new ListItem("Select Priority", "0");
            ddlPriority.Items.Insert(0, item);
        }

        private void PopulateUnits()
        {
            UI.FillUnits(ddlUnitsInd1);
            UI.FillUnits(ddlUnitsInd2);

            ListItem item = new ListItem("Select Unit", "0");
            ddlUnitsInd1.Items.Insert(0, item);
            ddlUnitsInd2.Items.Insert(0, item);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
            }
        }

        private void SaveData()
        {
            int priorityActivityId = SaveActivity();
            if (priorityActivityId > 0)
            {
                SaveIndicators(priorityActivityId);
            }
        }

        private int SaveActivity()
        {
            int emergencyId = 1;
            int clusterId = UserInfo.Cluster;
            Guid userId = RC.GetCurrentUserId;

            int objId = RC.GetSelectedIntVal(ddlObjective);
            int priorityId = RC.GetSelectedIntVal(ddlPriority);
            string actEn = txtActivityEng.Text.Trim();
            string actFr = txtActivityFr.Text.Trim();

            return DBContext.Add("InsertPriorityActivity", new object[] { emergencyId, clusterId, objId, 
                                                                        priorityId, actEn, actFr, userId, DBNull.Value });
        }

        private void SaveIndicators(int priorityActivityId)
        {
            SaveIndicator1(priorityActivityId);
            SaveIndicaotr2(priorityActivityId);
        }

        private void SaveIndicator1(int priorityActivityId)
        {
            int unitId = RC.GetSelectedIntVal(ddlUnitsInd1);
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            bool isSRP = true;
            bool isPriorityInd = false;

            int indicatorId = DBContext.Add("InsertOutPutIndicator", new object[] { priorityActivityId, indEn, indFr, 
                                                                unitId, isSRP, isPriorityInd, userId, DBNull.Value});
            if (indicatorId > 0)
            {
                bool isAdd = true;
                int locationId = UserInfo.Country;
                DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, isAdd, userId, DBNull.Value });
            }
        }

        private void SaveIndicaotr2(int priorityActivityId)
        {
            if (!Ind2IsValid()) return;

            int unitId = RC.GetSelectedIntVal(ddlUnitsInd2);
            string indEn = txtInd2Eng.Text.Trim();
            string indFr = txtInd2Fr.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            bool isSRP = true;
            bool isPriorityInd = false;

            int indicatorId = DBContext.Add("InsertOutPutIndicator", new object[] { priorityActivityId, indEn, indFr, 
                                                                unitId, isSRP, isPriorityInd, userId, DBNull.Value});

            if (indicatorId > 0)
            {
                bool isAdd = true;
                int locationId = UserInfo.Country;
                DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, isAdd, userId, DBNull.Value });
            }
        }

        private bool Ind2IsValid()
        {
            string txtIndEn = txtInd2Eng.Text.Trim();
            string txtIndFr = txtInd2Fr.Text.Trim();
            int unitId = RC.GetSelectedIntVal(ddlUnitsInd2);

            if (!string.IsNullOrEmpty(txtIndEn) && !string.IsNullOrEmpty(txtIndFr) && unitId > 0) return true;

            if (!string.IsNullOrEmpty(txtIndEn) && !string.IsNullOrEmpty(txtIndFr) && unitId == 0)
            {
                string message = "Please select Unit for Indicator 2.";
                return false;
            }

            if (!string.IsNullOrEmpty(txtIndEn) && string.IsNullOrEmpty(txtIndFr))
            {
                string message = "Please provide French version of Indicator 2. ";
                return false;
            }

            if (string.IsNullOrEmpty(txtIndEn) && !string.IsNullOrEmpty(txtIndFr))
            {
                string message = "Please provide English version of Indicator 2. ";
                return false;
            }

            return false;
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddSRPActivitiesFromMasterList.aspx");
        }
    }
}