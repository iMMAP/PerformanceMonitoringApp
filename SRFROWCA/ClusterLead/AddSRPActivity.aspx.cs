﻿using System;
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
using System.Net.Mail;
using System.Text;
using System.Web.Security;

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
                if (RC.IsRegionalClusterLead(this.User))
                {
                    Response.Redirect("~/RegionalLead/ManageRegionalIndicators.aspx");
                }
                else
                {
                    Response.Redirect("AddSRPActivitiesFromMasterList.aspx");
                }
            }
        }

        private void SaveData()
        {
            int priorityActivityId = SaveActivity();
            if (priorityActivityId > 0)
            {
                StringBuilder strIndcators = new StringBuilder();
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
                                bool regional = RC.IsRegionalClusterLead(this.User);
                                indControl.SaveRegionalIndicators(priorityActivityId, regional);
                            }
                            TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                            TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");
                            strIndcators.AppendFormat("Indicator (English): {0}<br/>Indicator (French): {1}<br/><br/>", txtEng.Text, txtFr.Text);
                        }
                    }
                }
                SendNewIndicatorEmail(strIndcators.ToString());
            }
        }

        private void SendNewIndicatorEmail(string strIndicators)
        {
            try
            {
                using (MailMessage mailMsg = new MailMessage())
                {
                    mailMsg.From = new MailAddress("orsocharowca@gmail.com");
                    mailMsg.To.Add("orsocharowca@gmail.com");
                    mailMsg.Subject = "New Indicators Has Been Added in ORS Master List";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = string.Format(@"New Indicators Has Been Added in ORS Master List<hr/>
                                                {0}<br/>                                              
                                                <b>By Following User:</b><br/>                                                        
                                                        <b>User Name:</b> {1}<br/>
                                                        <b>Email:</b> {2}<br/>                                                        
                                                        <b>Phone:</b> {3}<b/>"
                                                            , strIndicators, Membership.GetUser().UserName, Membership.GetUser().Email, "");
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {
 
            }
        }

        private int SaveActivity()
        {
            int emergencyId = 1;
            int clusterId = UserInfo.Cluster;

            if (clusterId == 0)
            {
                if (Request.QueryString["cid"] != null)
                {
                    int.TryParse(Request.QueryString["cid"], out clusterId);
                }
            }

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