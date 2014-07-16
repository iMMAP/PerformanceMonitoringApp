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
using SRFROWCA.Controls;

namespace SRFROWCA.ClusterLead
{
    public partial class AddSRPActivity : BasePage
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);
            if (control == "btnAddIndicatorControl")
            {
                IndControlId += 1;
            }

            if (control == "btnRemoveIndicatorControl")
            {
                IndControlId -= 1;
            }

            if (IndControlId <= 1)
            {
                btnRemoveIndicatorControl.Visible = false;
            }
            else
            {
                btnRemoveIndicatorControl.Visible = true;
            }

            //if (control == "btnSave" && IndControlId > 1)
            //{
            //    IndControlId -= 1;
            //}
            for (int i = 0; i < IndControlId; i++)
            {
                AddIndicatorControl(i);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjective();
            PopulatePriority();
            AddIndicatorControl(0);
            IndControlId = 1;
            if (RC.IsClusterLead(this.User))
            {
                btnBackToSRPList.Text = "Back To SRP List";
            }
            else
            {
                btnBackToSRPList.Text = "Back To Indicators List";
            }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                if (RC.IsClusterLead(this.User))
                {
                    Response.Redirect("AddSRPActivitiesFromMasterList.aspx");
                }
                else
                {
                    Response.Redirect("~/RegionalLead/ManageRegionalIndicators.aspx");
                }
            }
        }

        private void SaveData()
        {
            int priorityActivityId = SaveActivity();
            if (priorityActivityId > 0)
            {
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        NewIndicatorsControl indControl = ctl as NewIndicatorsControl;

                        if (indControl != null)
                        {
                            if (RC.IsClusterLead(this.User))
                            {
                                indControl.SaveIndicators(priorityActivityId);
                            }
                            else
                            {
                                indControl.SaveRegionalIndicators(priorityActivityId);
                            }
                        }
                    }
                }
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

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        private void AddIndicatorControl(int i)
        {
            NewIndicatorsControl newIndSet = (NewIndicatorsControl)LoadControl("~/controls/NewIndicatorsControl.ascx");
            newIndSet.ControlNumber = i + 1;
            newIndSet.ID = "indicatorControlId" + i.ToString();
            pnlAdditionalIndicaotrs.Controls.Add(newIndSet);
        }

        public int IndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["IndicatorControlId"] != null)
                {
                    int.TryParse(ViewState["IndicatorControlId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["IndicatorControlId"] = value.ToString();
            }
        }

        //public int IndControlId
        //{
        //    get
        //    {
        //        int indControlId = 0;
        //        if (Session["IndicatorControlId"] != null)
        //        {
        //            int.TryParse(Session["IndicatorControlId"].ToString(), out indControlId);
        //        }

        //        return indControlId;
        //    }
        //    set
        //    {
        //        Session["IndicatorControlId"] = value.ToString();
        //    }
        //}

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            if (RC.IsClusterLead(this.User))
            {
                Response.Redirect("AddSRPActivitiesFromMasterList.aspx");
            }
            else
            {
                Response.Redirect("~/RegionalLead/ManageRegionalIndicators.aspx");
            }
        }
    }
}