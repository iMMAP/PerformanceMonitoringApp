using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using Telerik.Web.UI;

namespace SRFROWCA.PivotTables
{
    public partial class CountryIndicatorsAchievedReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
                lblHeaderMessage.Text = UserInfo.CountryName + " " + RC.GetClusterName + " Indicators Sum";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            rpgvCountryIndicatorsAchieved.ExportSettings.IgnorePaging = true;
            rpgvCountryIndicatorsAchieved.ExportSettings.FileName = UserInfo.CountryName + "_" + RC.GetClusterName + "_CountryIndicators_" + DateTime.Now.ToString();
            rpgvCountryIndicatorsAchieved.ExportSettings.OpenInNewWindow = true;
            rpgvCountryIndicatorsAchieved.ExportToExcel();
        }

        protected void rpgvCountryIndicatorsAchieved_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            LoadData();
        }

        internal override void BindGridData()
        {
            LoadData();
            lblHeaderMessage.Text = UserInfo.CountryName + " " + RC.GetClusterName + " Indicators Sum";
        }

        private void LoadData()
        {
            DataTable dt = LoadIndicators();
            rpgvCountryIndicatorsAchieved.DataSource = dt;
        }

        private DataTable LoadIndicators()
        {
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int? emgClusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int langId = RC.SelectedSiteLanguageId;
            return DBContext.GetData("GetAllCountryIndicatorsAchievedReportPivot", new object[] { emgLocationId, emgClusterId, langId});
        }

        protected void CheckBoxEnableDragDrop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvCountryIndicatorsAchieved.ConfigurationPanelSettings.EnableDragDrop = checkBox.Checked;
            this.rpgvCountryIndicatorsAchieved.Rebind();
        }
        protected void CheckBoxEnableFieldsContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvCountryIndicatorsAchieved.ConfigurationPanelSettings.EnableFieldsContextMenu = checkBox.Checked;
            this.rpgvCountryIndicatorsAchieved.Rebind();     
        
        }

        protected void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}