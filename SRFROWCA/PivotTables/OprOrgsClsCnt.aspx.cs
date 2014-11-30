using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic;
using Telerik.Web.UI;
using SRFROWCA.Common;

namespace SRFROWCA.PivotTables
{
    public partial class OprOrgsClsCnt : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            rpgvOpePresence.ExportSettings.IgnorePaging = true;
            rpgvOpePresence.ExportSettings.FileName = "OperationalOrganizations_" + DateTime.Now.ToString();
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
            return DBContext.GetData("GetOperationalOrganization");
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