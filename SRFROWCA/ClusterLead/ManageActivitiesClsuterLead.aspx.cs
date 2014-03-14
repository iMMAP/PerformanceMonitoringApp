using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using BusinessLogic;

namespace SRFROWCA.ClusterLead
{
    public partial class ManageActivitiesClsuterLead : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            LoadData();
        }

        private void LoadData()
        {
            PopulateObjectives();
            PopulatePriorities();
        }

        private void PopulateObjectives()
        {
            UI.FillObjectives(rbObjectives);
        }

        private void PopulatePriorities()
        {
            UI.FillPriorities(rbPriorities);
        }

        protected void wzActivities_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            gvActivities.DataSource = DBContext.GetData("GetClusterActivitiesEngAndFrOnObjAndPriority", new object[] { 1, 1, 1, 1, 1 });
            gvActivities.DataBind();
        }

        protected void wzActivities_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
        }

        protected void wzActivities_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
        }
    }
}