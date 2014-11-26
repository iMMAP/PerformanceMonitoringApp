using System;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class AddActivity : BasePage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            UserInfo.UserProfileInfo();
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();           
            ShowHideControls();
           
        }


        internal override void BindGridData()
        {
            PopulateClusters();
            PopulateObjective();
            
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
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId,RC.SelectedSiteLanguageId });
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

            ddlCountry.DataSource = DBContext.GetData("GetEmergencyCountries", new object[]{emgId});
            ddlCountry.DataBind();
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            //UI.FillObjectives(ddlObjective);
            int emgId = UserInfo.Emergency;
            if (emgId <= 0)
            {
                emgId = 1;
            }
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] {RC.SelectedSiteLanguageId, emgId});
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
                Response.Redirect("ActivityListing.aspx");
                
            }
        }

        private void SaveData()
        {
            int ActivityId = SaveActivity();            
        }

        
        private int SaveActivity()
        {
            int emergencyId = UserInfo.Emergency;
            int clusterId = RC.IsClusterLead(this.User) ? UserInfo.Cluster : Convert.ToInt32(ddlCluster.SelectedValue);
            int emergencyLocationId = RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) ? UserInfo.EmergencyCountry : Convert.ToInt32(ddlCountry.SelectedValue);
            int emergencyClusterId = RC.IsClusterLead(this.User) ? UserInfo.EmergencyCluster : Convert.ToInt32(ddlCluster.SelectedValue); 

            if (emergencyClusterId == 0)
            {
                if (Request.QueryString["cid"] != null)
                {
                    int.TryParse(Request.QueryString["cid"], out emergencyClusterId);
                }
            }

            Guid userId = RC.GetCurrentUserId;

            int objId = RC.GetSelectedIntVal(ddlObjective);            
            string actEn = txtActivityEng.Text.Trim();
            string actFr = txtActivityFr.Text.Trim();

            return DBContext.Add("InsertActivityNew", new object[] { emergencyClusterId, objId,emergencyLocationId, 
                                                                        actEn, actFr, userId, DBNull.Value });
           
        }

      
       

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
           Response.Redirect("~/ClusterLead/ActivityListing.aspx");
        }
    }
}