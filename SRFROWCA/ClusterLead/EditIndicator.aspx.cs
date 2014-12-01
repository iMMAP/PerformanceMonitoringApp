using System;
using System.Data;
using System.Transactions;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class EditIndicator : BasePage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadCombos();
            DisableDropDowns();
            PopulateIndicator(Convert.ToInt32(Request.QueryString["id"]));
           
        }
        internal override void BindGridData()
        {
            PopulateObjective();
            PopulateClusters();           
            PopulateActivities();
            PopulateUnits();

        }

        private void LoadCombos()
        {
            PopulateObjective();
            PopulateClusters();
            PopulateCountries();
            PopulateActivities();
            PopulateUnits();

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
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            DataTable dt = DBContext.GetData("GetAdmin1ForIndicator", new object[] { indicatorDetailId,  emgLocationId});
            if (dt != null && dt.Rows.Count > 0)
            {
                rptAdmin1.DataSource = dt;
                rptAdmin1.DataBind();
            }
        }
        private void PopulateActivities()
        {           
            int? emergencyLocationId = RC.GetSelectedIntVal(ddlCountry);
            int? emergencyClusterId = RC.GetSelectedIntVal(ddlCluster);

            if (emergencyLocationId <= 0)
            {
                emergencyLocationId = null;
            }

            if (emergencyClusterId <= 0)
            {
                emergencyClusterId = null;
            }

            int? emergencyObjectiveId = ddlObjective.SelectedValue == "0" ? (int?)null :  RC.GetSelectedIntVal(ddlObjective);
            
                ddlActivity.DataSource = DBContext.GetData("GetActivitiesNew", new object[] { emergencyLocationId, emergencyClusterId, emergencyObjectiveId, RC.SelectedSiteLanguageId });
                ddlActivity.DataTextField = "Activity";
                ddlActivity.DataValueField = "ActivityId";
                ddlActivity.DataBind();
            
            ListItem item = new ListItem("Select Activity", "0");
            ddlActivity.Items.Insert(0, item);
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