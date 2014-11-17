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
    public partial class IndicatorsWithAdmin1TargetsControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl1stNumber.Text = " " + (ControlNumber).ToString();
            PopulateUnits();
            PopulateAdmin1();
        }

        private void PopulateUnits()
        {
            ddlUnit.DataValueField = "UnitId";
            ddlUnit.DataTextField = "Unit";

            ddlUnit.DataSource = GetUnits();
            ddlUnit.DataBind();

            ListItem item = new ListItem("Select Unit", "0");
            ddlUnit.Items.Insert(0, item);
            ddlUnit.SelectedIndex = 0;
        }

       

        private object GetUnits()
        {
            return DBContext.GetData("GetAllUnits", new object[] { RC.SelectedSiteLanguageId});
        }

        private void PopulateAdmin1()
        {
            rptAdmin1.DataSource = RC.GetAdmin1(UserInfo.Country);
            rptAdmin1.DataBind();
        }


        public void SaveIndicators(int priorityActivityId)
        {
            SaveIndicator(priorityActivityId);
        }
        public void SaveRegionalIndicators(int priorityActivityId, bool regional)
        {
            SaveRegionalIndicator(priorityActivityId, regional);
        }

        private void SaveIndicator(int ActivityId)
        {
            int unitId = Convert.ToInt32(ddlUnit.SelectedValue);// RC.GetSelectedIntVal(ddlUnitsInd1);
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            int gender = chkGender.Checked ? 1 : 0;

            int indicatorId = DBContext.Add("InsertIndicator", new object[] { ActivityId, indEn, indFr, 
                                                                unitId, userId,gender, DBNull.Value});
            if (indicatorId > 0)
            {
                SaveAdmin1Targets(indicatorId);
               
            }
        }
        private void SaveAdmin1Targets(int indicatorId)
        {
            int insertCount = 1;
           
            foreach (RepeaterItem item in rptAdmin1.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txttarget = (TextBox)item.FindControl("txtTarget");
                    var hdnLocationId = (HiddenField)item.FindControl("hdnLocationId");
                    if (!string.IsNullOrEmpty(txttarget.Text))
                    {
                        DBContext.Update("InsertIndicatorTarget", new object[] { indicatorId, Convert.ToInt32(hdnLocationId.Value), Convert.ToInt32(txttarget.Text),insertCount,RC.GetCurrentUserId, DBNull.Value });
                        insertCount++;
                    }
                   
                }
            }
        }
        private void SaveRegionalIndicator(int ActivityId, bool regional)
        {
            int unitId = Convert.ToInt32(ddlUnit.SelectedValue);// RC.GetSelectedIntVal(ddlUnitsInd1);
            string indEn = txtInd1Eng.Text.Trim();
            string indFr = txtInd1Fr.Text.Trim();
            Guid userId = RC.GetCurrentUserId;
            bool isSRP = regional;
           

            int indicatorId = DBContext.Add("InsertIndicator", new object[] { ActivityId, indEn, indFr, 
                                                                unitId, userId, DBNull.Value});
            if (indicatorId > 0)
            {
                SaveAdmin1Targets(indicatorId);

            }
        }


        public int ControlNumber { get; set; }
    }
}