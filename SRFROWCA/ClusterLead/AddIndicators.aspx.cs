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
using System.Net.Mail;
using System.Text;
using System.Web.Security;

namespace SRFROWCA.ClusterLead
{
    public partial class AddIndicators : BasePage
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
            PopulateClusters();
            PopulateCountries();
            PopulateActivities();
            AddIndicatorControl(0);
            IndControlId = 1;
            ShowHideControls();
            
        }
        private void PopulateActivities()
        {           
            int emergencyLocationId = RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) ? UserInfo.EmergencyCountry : Convert.ToInt32(ddlCountry.SelectedValue);
            int emergencyClusterId = RC.IsClusterLead(this.User) ? UserInfo.EmergencyCluster : Convert.ToInt32(ddlCluster.SelectedValue); 
            int emergencyObjectiveId = Convert.ToInt32(ddlObjective.SelectedValue);
            
                ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, RC.SelectedSiteLanguageId });
                ddlActivity.DataTextField = "Activity";
                ddlActivity.DataValueField = "ActivityId";
                ddlActivity.DataBind();
            
            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
        }
        private void ShowHideControls()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCluster.Visible = false;
                rfvCluster.Enabled = false;
                dvcluster.Visible = false;
                ddlCountry.Visible = false;
                rfvCountry.Visible = false;
                dvCountry.Visible = false;
            }
            else if (RC.IsCountryAdmin(this.User))
            {
                ddlCluster.Visible = true;
                rfvCluster.Enabled = true;
                dvcluster.Visible = true;
                ddlCountry.Visible = false;
                rfvCountry.Visible = false;
                dvCountry.Visible = false;
            }
            else if (RC.IsAdmin(this.User) || RC.IsOCHAStaff(this.User))
            {
                ddlCluster.Visible = true;
                rfvCluster.Enabled = true;
                dvcluster.Visible = true;
                ddlCountry.Visible = true;
                rfvCountry.Visible = true;
                dvCountry.Visible = true;
            }
        }
        private void PopulateClusters()
        {
            int emgId = UserInfo.Emergency;
            ddlCluster.DataValueField = "EmergencyClusterId";
            ddlCluster.DataTextField = "ClusterName";

            ddlCluster.DataSource = GetEmergencyClusters(emgId);
            ddlCluster.DataBind();

            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }
        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId, RC.SelectedSiteLanguageId });
        }

        private void PopulateCountries()
        {
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetEmergencyCountries", new object[] { UserInfo.Emergency });
            ddlCountry.DataBind();
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            UI.FillObjectives(ddlObjective);
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] {RC.SelectedSiteLanguageId,UserInfo.Emergency});
            ddlObjective.DataTextField = "ShortObjectiveTitle";
            ddlObjective.DataValueField = "EmergencyObjectiveId";
            ddlObjective.DataBind();
            ListItem item = new ListItem("Select Objective", "0");
            ddlObjective.Items.Insert(0, item);
        }

      
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                SaveData();
                scope.Complete();
               
                Response.Redirect("IndicatorListing.aspx");
                
            }
        }

        private void SaveData()
        {
            int ActivityId = Convert.ToInt32(ddlActivity.SelectedValue);
           
                StringBuilder strIndcators = new StringBuilder();
                foreach (Control ctl in pnlAdditionalIndicaotrs.Controls)
                {
                    if (ctl != null && ctl.ID != null && ctl.ID.Contains("indicatorControlId"))
                    {
                        IndicatorsWithAdmin1TargetsControl indControl = ctl as IndicatorsWithAdmin1TargetsControl;

                        if (indControl != null)
                        {
                            if (RC.IsClusterLead(this.User))
                            {
                                indControl.SaveIndicators(ActivityId);
                            }
                            else
                            {
                                bool regional = RC.IsRegionalClusterLead(this.User);
                                indControl.SaveRegionalIndicators(ActivityId, regional);
                            }
                            TextBox txtEng = (TextBox)indControl.FindControl("txtInd1Eng");
                            TextBox txtFr = (TextBox)indControl.FindControl("txtInd1Fr");
                            strIndcators.AppendFormat("Indicator (English): {0}<br/>Indicator (French): {1}<br/><br/>", txtEng.Text, txtFr.Text);
                        }
                    }
                }
                SendNewIndicatorEmail(strIndcators.ToString());
           
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
                                                            , strIndicators, Membership.GetUser().UserName, Membership.GetUser().Email, RC.GetUserDetails().Rows[0]["PhoneNumber"].ToString());
                    Mail.SendMail(mailMsg);
                }
            }
            catch
            {
 
            }
        }
       

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        private void AddIndicatorControl(int i)
        {
            IndicatorsWithAdmin1TargetsControl newIndSet = (IndicatorsWithAdmin1TargetsControl)LoadControl("~/controls/IndicatorsWithAdmin1TargetsControl.ascx");
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
            
                Response.Redirect("IndicatorListing.aspx");
           
        }

        protected void ddlObjective_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }
    }
}