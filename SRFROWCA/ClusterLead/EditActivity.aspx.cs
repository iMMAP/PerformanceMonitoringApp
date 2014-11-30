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
            DisableDropDowns();
            GetActivity(false);
        }

        private void LoadCombos()
        {
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();

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

        internal override void BindGridData()
        {
            PopulateClusters();
            PopulateObjective();
            GetActivity(true);
        }

        private void GetActivity(bool isLangChange)
        {
            DataTable dt = DBContext.GetData("GetNewActivity", new object[] {Convert.ToInt32(Request.QueryString["id"]) });
            if (dt != null && dt.Rows.Count > 0)
            {
                ddlCountry.SelectedValue = dt.Rows[0]["EmergencyLocationId"].ToString();
                ddlCluster.SelectedValue = dt.Rows[0]["EmergencyClusterId"].ToString();
                ddlObjective.SelectedValue = dt.Rows[0]["EmergencyObjectiveId"].ToString();

                if (!isLangChange)
                {
                    txtActivityEng.Text = dt.Rows[0]["Activity"].ToString();
                }
            }
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
            UI.FillObjectives(ddlObjective);
            ddlObjective.DataSource = DBContext.GetData("GetEmergencyObjectives", new object[] {RC.SelectedSiteLanguageId, RC.EmergencySahel2015});
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