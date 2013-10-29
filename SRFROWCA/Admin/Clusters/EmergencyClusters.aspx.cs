using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using BusinessLogic;

namespace SRFROWCA.Admin.Clusters
{
    public partial class EmergencyClusters : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateEmergencies();
            PopulateClusters();
        }

        private void PopulateEmergencies()
        {
            ddlEmergencies.DataValueField = "LocationEmergencyId";
            ddlEmergencies.DataTextField = "EmergencyName";

            ddlEmergencies.DataSource = GetEmergencies();
            ddlEmergencies.DataBind();

            ListItem item = new ListItem("Select Emergency", "0");
            ddlEmergencies.Items.Insert(0, item);
        }

        private DataTable GetEmergencies()
        {
            return DBContext.GetData("GetALLLocationEmergencies");
        }

        private void PopulateClusters()
        {
            cblClusters.DataValueField = "ClusterId";
            cblClusters.DataTextField = "ClusterName";

            cblClusters.DataSource = GetClusters();
            cblClusters.DataBind();
        }

        private DataTable GetClusters()
        {
            return DBContext.GetData("GetAllClusters");
        }

        protected void ddlEmergencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            int emergencyId = 0;
            int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

            if (emergencyId > 0)
            {
                FillEmergencyClusters(emergencyId);
            }
        }

        private void FillEmergencyClusters(int emergencyId)
        {
            DataTable dt = GetEmergencyClusters(emergencyId);
            CheckClusterListBox(dt);
        }       

        private DataTable GetEmergencyClusters(int emergencyId)
        {
            return DBContext.GetData("GetEmergencyClusters", new object[] { emergencyId });
        }

        private void CheckClusterListBox(DataTable dt)
        {
            foreach (ListItem item in cblClusters.Items)
            {
                item.Selected = false;
            }

            foreach (ListItem item in cblClusters.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ClusterId"].ToString().Equals(item.Value))
                    {
                        item.Selected = true;
                        item.Enabled = false;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int emergencyId = 0;
                int.TryParse(ddlEmergencies.SelectedValue, out emergencyId);

                foreach (ListItem item in cblClusters.Items)
                {
                    if (item.Selected)
                    {
                        SaveItem(emergencyId, item.Value);
                    }
                    else
                    {
                        DeleteItem(emergencyId, item.Value);
                    }
                }

                lblMessage.Text = "Saved!";
                lblMessage.Visible = true;
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.ToString();
                lblMessage.Visible = true;
            }
        }

        private void DeleteItem(int emergencyId, string itemValue)
        {
            DBContext.Delete("DeleteClusterFromEmergency", new object[] { emergencyId, Convert.ToInt32(itemValue), DBNull.Value });
        }

        private void SaveItem(int emergencyId, string itemValue)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            DBContext.Add("InsertEmergencyClusters", new object[] { emergencyId, Convert.ToInt32(itemValue), userId, DBNull.Value });
        }
    }
}