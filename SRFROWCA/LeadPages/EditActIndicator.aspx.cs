﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using SRFROWCA.Common;
using System.Transactions;

namespace SRFROWCA.LeadPages
{
    public partial class EditActIndicator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["SRPCustomEditIndicator"] != null)
                {
                    DataTable dt = GetIndicatorDetails();
                    PopulateControls(dt);
                }
            }
        }        

        private DataTable GetIndicatorDetails()
        {
            DataTable dt = new DataTable();
            int indId = 0;
            int.TryParse(Session["SRPCustomEditIndicator"].ToString(), out indId);
            if (indId > 0)
            {
                dt = DBContext.GetData("GetIndicatorAndItsParentDetailsForSRPEdit", new object[] { indId, RC.SelectedSiteLanguageId });
            }

            return dt;
        }

        private void PopulateControls(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                divObjPr.InnerHtml = dt.Rows[0]["ShortObjectiveTitle"].ToString() + "<br/>" + dt.Rows[0]["ShortPriorityText"].ToString();
                txtActivity.Text = dt.Rows[0]["ActivityName"].ToString();
                ActivityId = Convert.ToInt32(dt.Rows[0]["PriorityActivityId"].ToString());
                txtIndicator.Text = dt.Rows[0]["DataName"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                Session["SRPCustomEditIndicator"] = null;
                scope.Complete();
                Response.Redirect("~/ClusterLead/AddSRPActivitiesFromMasterList.aspx");
            }
        }

        private void SaveData()
        {
            UpdateActivity();
            UpdateIndicator();
        }

        private void UpdateActivity()
        {
            if (ActivityId > 0)
            {
                DBContext.Update("UpdateActivity", new object[] { ActivityId, txtActivity.Text.Trim(), RC.SelectedSiteLanguageId, DBNull.Value });
            }
        }

        private void UpdateIndicator()
        {
            int indId = 0;
            int.TryParse(Session["SRPCustomEditIndicator"].ToString(), out indId);
            if (indId > 0)
            {
                DBContext.Update("UpdateIndicator", new object[] { indId, txtIndicator.Text.Trim(), RC.SelectedSiteLanguageId, DBNull.Value });
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Session["SRPCustomEditIndicator"] = null;
            Response.Redirect("~/ClusterLead/AddSRPActivitiesFromMasterList.aspx");
        }


        public int ActivityId
        {
            get
            {
                int activityId = 0;
                if (ViewState["ActivityId"] != null)
                {
                    int.TryParse(ViewState["ActivityId"].ToString(), out activityId);
                }

                return activityId;
            }
            set
            {
                ViewState["ActivityId"] = value.ToString();
            }
        }
    }
}