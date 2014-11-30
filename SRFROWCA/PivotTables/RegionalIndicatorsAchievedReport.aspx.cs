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
    public partial class RegionalIndicatorsAchievedReport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblHeaderMessage.Text = RC.GetClusterName + " Indicators Sum";
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            rpgvRegionalIndicatorsAchieved.ExportSettings.IgnorePaging = true;
            rpgvRegionalIndicatorsAchieved.ExportSettings.FileName = UserInfo.CountryName + "_" + RC.GetClusterName + "_RegionalIndicators_" + DateTime.Now.ToString();
            rpgvRegionalIndicatorsAchieved.ExportSettings.OpenInNewWindow = true;
            rpgvRegionalIndicatorsAchieved.ExportToExcel();
        }

        protected void rpgvRegionalIndicatorsAchieved_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            DataTable dt = LoadIndicators();
            rpgvRegionalIndicatorsAchieved.DataSource = dt;            
        }

        internal override void BindGridData()
        {
            DataTable dt = LoadIndicators();
            rpgvRegionalIndicatorsAchieved.DataSource = dt;
            lblHeaderMessage.Text = RC.GetClusterName + " Indicators Sum";
        }

        private DataTable LoadIndicators()
        {
            int? emgClusterId = UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int langId = RC.SelectedSiteLanguageId;
            return DBContext.GetData("GetAllRegionalIndicatorsAchievedReport", new object[] { emgClusterId, langId });
        }

        protected void CheckBoxEnableDragDrop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvRegionalIndicatorsAchieved.ConfigurationPanelSettings.EnableDragDrop = checkBox.Checked;
            this.rpgvRegionalIndicatorsAchieved.Rebind();
        }
        protected void CheckBoxEnableFieldsContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvRegionalIndicatorsAchieved.ConfigurationPanelSettings.EnableFieldsContextMenu = checkBox.Checked;
            this.rpgvRegionalIndicatorsAchieved.Rebind();
        }

    }
}