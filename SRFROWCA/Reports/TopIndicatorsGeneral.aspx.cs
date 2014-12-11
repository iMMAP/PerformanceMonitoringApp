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
    public partial class TopIndicatorsGeneral : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateClustes();
                PopulateCountry();
                PopulateObjectives();
                PopulatePriorities();

                LoadTopIndicators();
            }
        }

        private void LoadTopIndicators()
        {
            int tempId = RC.GetSelectedIntVal(ddlClusters);
            int? clusterId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlCountry);
            int? countryId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlObjectives);
            int? objId = tempId == 0 ? (int?)null : tempId;

            tempId = RC.GetSelectedIntVal(ddlPriorities);
            int? prId = tempId == 0 ? (int?)null : tempId;

            gvIndicators.DataSource = DBContext.GetData("GetTopIndicatorsGeneral", new object[] { countryId, clusterId, objId, prId });
            gvIndicators.DataBind();
        }

        protected void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTopIndicators();
        }

        private void PopulateClustes()
        {
            UI.FillClusters(ddlClusters, RC.SelectedSiteLanguageId);
            ListItem item = new ListItem("All", "0");
            ddlClusters.Items.Insert(0, item);
        }

        private void PopulateCountry()
        {
            UI.FillCountry(ddlCountry);

            ListItem item = new ListItem("All", "0");
            ddlCountry.Items.Insert(0, item);
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(ddlObjectives, true, RC.SelectedEmergencyId);
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