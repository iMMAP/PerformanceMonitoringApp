using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace SRFROWCA.Reports
{
    public partial class IndicatorTargetByProjectStatus : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                PopulateIndicators();
            }
        }

        private void PopulateDropDowns()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("All", "0"));

            UI.FillEmergencyLocations(ddlLocation, RC.EmergencySahel2015);
            ddlLocation.Items.Insert(0, new ListItem("All", "0"));
        }

        #region Events

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PopulateAdminGrid(e.Row, "hfCountryId", "gvAdmin1", "GetOPSAdmin1TargetOfIndicatorForOpsClusCooReport");
                ObjPrToolTip.ObjectiveIconToolTip(e, 2);
            }
        }

        protected void gvAdmin1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PopulateAdminGrid(e.Row, "hfAdmin1Id", "gvAdmin2", "GetOPSAdmin2TargetOfIndicatorForOpsClusCooReport");
            }
        }

        protected void ddl_IndexChanged(object sender, EventArgs e)
        {
            PopulateIndicators();
        }

        #endregion

        #region Methods

        internal override void BindGridData()
        {
            PopulateIndicators();
        }
        private void PopulateIndicators()
        {
            DataTable dt = GetIndicators();
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        private DataTable GetIndicators()
        {
            int val = RC.GetSelectedIntVal(ddlLocation);
            int? emgLocId = val > 0 ? val : (int?) null;
            val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val > 0 ? val : (int?)null;
            int langId = RC.SelectedSiteLanguageId;
            int yearId = 12;
            return DBContext.GetData("GetAllIndicatorsForOPSTargetReport", new object[] { emgLocId, emgClusterId, langId, yearId });
        }

        private void PopulateAdminGrid(GridViewRow row, string hfLocName, string gvName, string procName)
        {
            HiddenField hfIndicatorId = row.FindControl("hfIndicatorId") as HiddenField;
            int indicatorId = 0;
            if (hfIndicatorId != null)
            {
                int.TryParse(hfIndicatorId.Value, out indicatorId);
            }
            if (indicatorId > 0)
            {
                if (indicatorId > 0)
                {
                    HiddenField hfLocId = row.FindControl(hfLocName) as HiddenField;
                    int locId = 0;
                    if (hfLocId != null)
                    {
                        int.TryParse(hfLocId.Value, out locId);
                    }
                    if (locId > 0)
                    {
                        GridView gv = row.FindControl(gvName) as GridView;
                        if (gv != null)
                        {
                            DataTable dt = DBContext.GetData(procName, new object[] { locId, indicatorId });
                            gv.DataSource = dt;
                            gv.DataBind();
                        }
                    }
                }
            }
        }

        #endregion
    }
}