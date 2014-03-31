using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;

namespace SRFROWCA.Reports
{
    public partial class TopIndicaotrsRegional : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo();
                PopulateOrganizations();
                PopulateCountry();
                PopulateObjectives();
                PopulatePriorities();
                PopulateClusterName();
                

                LoadTopIndicators();
            }
        }

        private void PopulateClusterName()
        {
            try
            {
                lblClusterName.Text = RC.GetClusterName + " Mostly Used Regional Indicators";
            }
            catch { }
        }

        private void PopulateOrganizations()
        {
            ddlOrganizations.DataTextField = "OrganizationName";
            ddlOrganizations.DataValueField = "OrganizationId";
            ddlOrganizations.DataSource = DBContext.GetData("GetAllOrganizations");
            ddlOrganizations.DataBind();

            ListItem item = new ListItem("All", "0");
            ddlOrganizations.Items.Insert(0, item);
        }

        private void LoadTopIndicators()
        {
            int tempId = RC.GetSelectedIntVal(ddlOrganizations);
            int? orgId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlCountry);
            int? countryId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlObjectives);
            int? objId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlPriorities);
            int? prId = tempId == 0 ? (int?)null : tempId;

            gvIndicators.DataSource = DBContext.GetData("GetTopIndicatorsRegional", new object[] { UserInfo.EmergencyCluster, countryId, orgId, objId, prId });
            gvIndicators.DataBind();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTopIndicators();
        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);

            ListItem item = new ListItem("All", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(ddlObjectives, true);
            ListItem item = new ListItem("All", "0");
            ddlObjectives.Items.Insert(0, item);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(ddlPriorities);
            ListItem item = new ListItem("All", "0");
            ddlPriorities.Items.Insert(0, item);
        }
    }
}