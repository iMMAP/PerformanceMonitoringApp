using System;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Transactions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using SRFROWCA.Controls;

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
            LoadCombos();
            AddIndicatorControl(0);
            IndControlId = 1;
            //ShowHideControls();
            DisableDropDowns();
        }

        private void LoadCombos()
        {
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();
            PopulateActivities();

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void PopulateActivities()
        {
            int emergencyLocationId = RC.GetSelectedIntVal(ddlCountry);
            int emergencyClusterId = RC.GetSelectedIntVal(ddlCluster);
            int emergencyObjectiveId = RC.GetSelectedIntVal(ddlObjective);

            ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, RC.SelectedSiteLanguageId });
            ddlActivity.DataTextField = "Activity";
            ddlActivity.DataValueField = "ActivityId";
            ddlActivity.DataBind();

            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
        }

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        //private void ShowHideControls()
        //{
        //    if (RC.IsClusterLead(this.User))
        //    {
        //        ddlCluster.Visible = false;
        //        rfvCluster.Enabled = false;
        //        dvcluster.Visible = false;
        //        ddlCountry.Visible = false;
        //        rfvCountry.Visible = false;
        //        dvCountry.Visible = false;
        //    }
        //    else if (RC.IsCountryAdmin(this.User))
        //    {
        //        ddlCluster.Visible = true;
        //        rfvCluster.Enabled = true;
        //        dvcluster.Visible = true;
        //        ddlCountry.Visible = false;
        //        rfvCountry.Visible = false;
        //        dvCountry.Visible = false;
        //    }
        //    else if (RC.IsAdmin(this.User) || RC.IsOCHAStaff(this.User))
        //    {
        //        ddlCluster.Visible = true;
        //        rfvCluster.Enabled = true;
        //        dvcluster.Visible = true;
        //        ddlCountry.Visible = true;
        //        rfvCountry.Visible = true;
        //        dvCountry.Visible = true;
        //    }
        //}
        private void PopulateClusters()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ListItem item = new ListItem("Select Cluster", "0");
            ddlCluster.Items.Insert(0, item);
        }
       
        private void PopulateCountries()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] { RC.SelectedSiteLanguageId, RC.EmergencySahel2015 });
            ddlObjective.DataTextField = "Objective";
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
                        //if (RC.IsClusterLead(this.User))
                        //{
                            indControl.SaveIndicators(ActivityId);
                        //}
                        //else
                        //{
                        //    bool regional = RC.IsRegionalClusterLead(this.User);
                        //    indControl.SaveRegionalIndicators(ActivityId, regional);
                        //}
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
                    mailMsg.Subject = "Framework 2015 - New Indicators Has Been Added in ORS Master List";
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = string.Format(@"New Indicators Has Been Added in ORS 2015 Framework<hr/>
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


        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }

        private void AddIndicatorControl(int i)
        {
            IndicatorsWithAdmin1TargetsControl newIndSet = (IndicatorsWithAdmin1TargetsControl)LoadControl("~/controls/IndicatorsWithAdmin1TargetsControl.ascx");
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            if (emgLocationId > 0)
            {
                int indicatorId = 0;
                //newIndSet.PopulateAdmin1(emgLocationId);
                DataTable dtTargets = GetAdmin1ForIndicatorAndLocation(emgLocationId, indicatorId);
                newIndSet.rptAdmin1.DataSource = dtTargets;
                newIndSet.rptAdmin1.DataBind();
            }
            newIndSet.ControlNumber = i + 1;
            newIndSet.ID = "indicatorControlId" + i.ToString();
            pnlAdditionalIndicaotrs.Controls.Add(newIndSet);
        }

        private DataTable GetAdmin1ForIndicatorAndLocation(int emgLocationId, int indicatorId)
        {
            return DBContext.GetData("GetAdmin1ForIndicator2", new object[] { indicatorId, emgLocationId });
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

        internal override void BindGridData()
        {
            PopulateClusters();
            PopulateObjective();
            PopulateActivities();

        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateActivities();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CountryIndicators.aspx");
        }
    }
}