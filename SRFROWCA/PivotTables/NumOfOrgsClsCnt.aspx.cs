using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using BusinessLogic;
using System.Web.Script.Serialization;
using SRFROWCA.Common;

namespace SRFROWCA.PivotTables
{
    public partial class NumOfOrgsClsCnt : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            rpgvOpePresence.ExportSettings.IgnorePaging = true;
            rpgvOpePresence.ExportSettings.FileName = UserInfo.CountryName + "_" + UserInfo.OrgName + "_OperationalPresence_" + DateTime.Now.ToString();
            rpgvOpePresence.ExportSettings.OpenInNewWindow = true;
            rpgvOpePresence.ExportToExcel();
        }

        protected void RadPivotGrid1_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
        {
            DataTable dt = LoadContacts();
            rpgvOpePresence.DataSource = dt;
        }

        private DataTable LoadContacts()
        {
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int? organizationId = UserInfo.Organization > 0 ? UserInfo.Organization : (int?)null;
            return DBContext.GetData("GetNumberOfOrgsOnLocationAndCluster");
        }

        protected void CheckBoxEnableDragDrop_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvOpePresence.ConfigurationPanelSettings.EnableDragDrop = checkBox.Checked;
            this.rpgvOpePresence.Rebind();
        }
        protected void CheckBoxEnableFieldsContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            this.rpgvOpePresence.ConfigurationPanelSettings.EnableFieldsContextMenu = checkBox.Checked;
            this.rpgvOpePresence.Rebind();
        }
    }
}