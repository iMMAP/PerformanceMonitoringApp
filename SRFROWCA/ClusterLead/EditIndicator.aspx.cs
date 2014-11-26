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
    public partial class EditIndicator : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();
            PopulateActivities();
            PopulateUnits();
            ShowHideControls();
            PopulateIndicator(Convert.ToInt32(Request.QueryString["id"]));
           
        }
        internal override void BindGridData()
        {
            PopulateObjective();
            PopulateClusters();           
            PopulateActivities();
            PopulateUnits();

        }
        private void PopulateIndicator(int indicatorDetailId)
        {
            DataTable dt = DBContext.GetData("GetNewIndicator", new object[] { indicatorDetailId });
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCountry.SelectedValue = dt.Rows[0]["EmergencyLocationId"].ToString();
                ddlCluster.SelectedValue = dt.Rows[0]["EmergencyClusterId"].ToString();
                ddlObjective.SelectedValue = dt.Rows[0]["EmergencyObjectiveId"].ToString();
                ddlActivity.SelectedValue = dt.Rows[0]["ActivityId"].ToString();
                txtInd1Eng.Text = dt.Rows[0]["Indicator"].ToString();
                ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
                chkGender.Checked = Convert.ToBoolean(dt.Rows[0]["IsGender"].ToString());
                GetAdmin1ForIndicatorAndLocation(indicatorDetailId);
            }
        }

        private void GetAdmin1ForIndicatorAndLocation(int indicatorDetailId)
        {
            DataTable dt = DBContext.GetData("GetAdmin1ForIndicator", new object[] { indicatorDetailId, UserInfo.Country == 0 ? Convert.ToInt32(ddlCountry.SelectedValue) : UserInfo.Country });
            if (dt != null && dt.Rows.Count > 0)
            {
                rptAdmin1.DataSource = dt;
                rptAdmin1.DataBind();
            }
        }
        private void PopulateActivities()
        {           
            int? emergencyLocationId = RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) ? UserInfo.EmergencyCountry : Convert.ToInt32(ddlCountry.SelectedValue);
            int? emergencyClusterId = RC.IsClusterLead(this.User) ? UserInfo.EmergencyCluster : Convert.ToInt32(ddlCluster.SelectedValue);

            if (emergencyLocationId <= 0)
            {
                emergencyLocationId = null;
            }

            if (emergencyClusterId <= 0)
            {
                emergencyClusterId = null;
            }

            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null :  Convert.ToInt32(ddlObjective.SelectedValue);
            
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
            if (emgId <= 0)
            {
                emgId = 1;
            }

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
            int emgId = UserInfo.Emergency;
            if (emgId <= 0)
            {
                emgId = 1;
            }

            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetEmergencyCountries", new object[] { emgId });
            ddlCountry.DataBind();
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            int emgId = UserInfo.Emergency;
            if (emgId <= 0)
            {
                emgId = 1;
            }

            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] { RC.SelectedSiteLanguageId, emgId });
            ddlObjective.DataTextField = "ShortObjectiveTitle";
            ddlObjective.DataValueField = "EmergencyObjectiveId";
            ddlObjective.DataBind();
            ListItem item = new ListItem("Select Objective", "0");
            ddlObjective.Items.Insert(0, item);
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
                        DBContext.Update("InsertIndicatorTarget", new object[] { indicatorId, Convert.ToInt32(hdnLocationId.Value), Convert.ToInt32(txttarget.Text),insertCount, RC.GetCurrentUserId, DBNull.Value });
                        insertCount++;
                    }

                }
            }
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
            return DBContext.GetData("GetAllUnits", new object[] { RC.SelectedSiteLanguageId });
        }

       
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                UpdateIndicator();
                scope.Complete();                
                Response.Redirect("IndicatorListing.aspx");
               
            }
        }

        private void UpdateIndicator()
        {
            int indicatorDetailId = Convert.ToInt32(Request.QueryString["id"]);
            int activityId = Convert.ToInt32(ddlActivity.SelectedValue);
            int UnitId = Convert.ToInt32(ddlUnit.SelectedValue);
            string indicator = txtInd1Eng.Text;
            Guid userId = RC.GetCurrentUserId;
            int gender = chkGender.Checked ? 1 : 0;
            int indicatorId = DBContext.Update("UpdateIndicatorNew", new object[] { indicatorDetailId,activityId,UnitId,indicator,userId,gender, DBNull.Value});
            SaveAdmin1Targets(indicatorId);
        }

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/IndicatorListing.aspx");
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