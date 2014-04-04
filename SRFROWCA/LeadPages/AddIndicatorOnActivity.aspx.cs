﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using SRFROWCA.Controls;
using SRFROWCA.Common;
using BusinessLogic;
using System.Data;

namespace SRFROWCA.LeadPages
{
    public partial class AddIndicatorOnActivity : System.Web.UI.Page
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

        private void PopulateActivities()
        {
            UI.FillActivities(ddlActivities, GetActivites());

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivities.Items.Insert(0, item);
        }

        private DataTable GetActivites()
        {
            int objId = RC.GetSelectedIntVal(ddlObjective);
            int prId = RC.GetSelectedIntVal(ddlPriority);
            int emgId = 1;
            int clusterId = UserInfo.Cluster;
            int langId = RC.SelectedSiteLanguageId;

            return DBContext.GetData("GetActivitiesWithClusterObjAndPriority", new object[] {emgId, clusterId, objId, prId, langId});
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
                Response.Redirect("~/ClusterLead/AddSRPActivitiesFromMasterList.aspx");
            }
        }

        private void SaveData()
        {
            int priorityActivityId = RC.GetSelectedIntVal(ddlActivities);
            if (priorityActivityId > 0)
            {
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        NewIndicatorsControl indControl = ctl as NewIndicatorsControl;

                        if (indControl != null)
                        {
                            indControl.SaveIndicators(priorityActivityId);
                        }
                    }
                }
            }
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
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

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/AddSRPActivitiesFromMasterList.aspx");
        }
    }
}