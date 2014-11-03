using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.Admin
{
    public partial class CountryIndicators : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClusterIndicators();
                LoadCombos();
            }
        }

        private void LoadCombos()
        {
            UI.FillCountry(ddlCountry);
            UI.FillClusters(ddlCluster, RC.SelectedSiteLanguageId);
        }

        private void LoadClusterIndicators()
        {
            string objective = null;
            string indicator = null;
            int? countryId = null;
            int? clusterId = null;

            if (!string.IsNullOrEmpty(txtObjectiveName.Text.Trim()))
                objective = txtObjectiveName.Text;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            gvClusterIndicators.DataSource = GetClusterIndicatros(clusterId, countryId, objective, indicator);
            gvClusterIndicators.DataBind();
        }

        private DataTable GetClusterIndicatros(int? clusterId, int? countryId, string objective, string indicator)
        {
            return DBContext.GetData("uspGetCluserIndicators", new object[] { clusterId, countryId, objective, indicator, RC.SelectedSiteLanguageId });
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/AddCountryIndicator.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }
    }
}