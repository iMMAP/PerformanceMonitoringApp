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
    public partial class EditActivity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();           
            ShowHideControls();
            GetActivity();
        }

        internal override void BindGridData()
        {
            PopulateClusters();
            PopulateObjective();

        }
        private void GetActivity()
        {
            DataTable dt = DBContext.GetData("GetNewActivity", new object[] {Convert.ToInt32(Request.QueryString["id"]) });
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCountry.SelectedValue = dt.Rows[0]["EmergencyLocationId"].ToString();
                ddlCluster.SelectedValue = dt.Rows[0]["EmergencyClusterId"].ToString();
                ddlObjective.SelectedValue = dt.Rows[0]["EmergencyObjectiveId"].ToString();
                txtActivityEng.Text = dt.Rows[0]["Activity"].ToString();
            }
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
            int emgId = RC.SelectedEmergencyId;
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
            ddlCountry.DataValueField = "LocationId";
            ddlCountry.DataTextField = "LocationName";

            ddlCountry.DataSource = DBContext.GetData("GetEmergencyCountries", new object[]{RC.SelectedEmergencyId});
            ddlCountry.DataBind();
            ListItem item = new ListItem("Select Country", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjective()
        {
            UI.FillObjectives(ddlObjective);
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] {RC.SelectedSiteLanguageId,RC.SelectedEmergencyId});
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
                Response.Redirect("ActivityListing.aspx");
                
            }
        }

        private void SaveData()
        {
            int emgLocationId = Convert.ToInt32(ddlCountry.SelectedValue);
            int emgClusterId = Convert.ToInt32(ddlCluster.SelectedValue);
            int emgObjectiveId = Convert.ToInt32(ddlObjective.SelectedValue);
            string activity = txtActivityEng.Text;
            DBContext.Update("UpdateActivtiyNew", new object[] { Convert.ToInt32(Request.QueryString["id"]), emgLocationId, emgClusterId, emgObjectiveId, activity, RC.GetCurrentUserId,DBNull.Value });
        }

        
     

      
       

        protected void btnBackToSRPList_Click(object sender, EventArgs e)
        {
           Response.Redirect("~/ClusterLead/ActivityListing.aspx");
        }
    }
}