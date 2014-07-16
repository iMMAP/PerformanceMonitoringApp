using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.Controls
{
    public partial class NewIndicatorsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();
        }

        public void SaveIndicators(int priorityActivityId)
        {
            SaveIndicator(priorityActivityId);
        }
        public void SaveRegionalIndicators(int priorityActivityId)
        {
            SaveRegionalIndicator(priorityActivityId);
        }

        private void SaveIndicator(int priorityActivityId)
        {
            int unitId = 156;// RC.GetSelectedIntVal(ddlUnitsInd1);
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
                int locationId = UserInfo.EmergencyCountry;
                int clusterId = UserInfo.EmergencyCluster;
                DBContext.Update("InsertDeleteSRPIndicaotr", new object[] { indicatorId, locationId, clusterId, isAdd, userId, DBNull.Value });
               
            }
        }
        private void SaveRegionalIndicator(int priorityActivityId)
        {
            int unitId = 156;// RC.GetSelectedIntVal(ddlUnitsInd1);
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
                int locationId = UserInfo.EmergencyCountry;
                int clusterId = UserInfo.EmergencyCluster;
                DBContext.Update("InsertDeleteRegionalIndicaotr", new object[] { indicatorId, clusterId, isAdd, userId, DBNull.Value });

            }
        }


        public int ControlNumber { get; set; }
    }
}