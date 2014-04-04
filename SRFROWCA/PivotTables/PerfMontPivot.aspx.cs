using System;
using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;
using Telerik.Web.UI;

namespace SRFROWCA.PivotTables
{
    public partial class PerfMontPivot : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            rpgvCustomReport.ExportSettings.IgnorePaging = true;
            rpgvCustomReport.ExportSettings.FileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_PivotReport_" + DateTime.Now.ToString();
            rpgvCustomReport.ExportSettings.OpenInNewWindow = true;
            rpgvCustomReport.ExportToExcel();
        }

        protected void RadPivotGrid1_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            DataTable dt = LoadIndicators();
            rpgvCustomReport.DataSource = dt;
        }

        private DataTable LoadIndicators()
        {
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int? emgClusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? organizationId = UserInfo.Organization > 0 ? UserInfo.Organization : (int?)null;            
            int langId = RC.SelectedSiteLanguageId;
            return DBContext.GetData("GetDataForPivotReport", new object[] {emgLocationId, emgClusterId, organizationId,langId});
        }

        protected void CheckBoxEnableDragDrop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvCustomReport.ConfigurationPanelSettings.EnableDragDrop = checkBox.Checked;
            this.rpgvCustomReport.Rebind();
        }
        protected void CheckBoxEnableFieldsContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvCustomReport.ConfigurationPanelSettings.EnableFieldsContextMenu = checkBox.Checked;
            this.rpgvCustomReport.Rebind();
        }
       
    }
}