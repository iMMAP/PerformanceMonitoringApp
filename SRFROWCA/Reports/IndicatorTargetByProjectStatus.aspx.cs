using AjaxControlToolkit;
using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SRFROWCA.Reports
{
    public partial class IndicatorTargetByProjectStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateDropDowns();
                SetDropDownOnRole();
                PopulateIndicators();
            }
        }

        #region Events

        protected void gvActivities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UI.SetThousandSeparator(e.Row, "lblClusterTotal");
                UI.SetThousandSeparator(e.Row, "lblDraftTotal");
                UI.SetThousandSeparator(e.Row, "lblApprovedTotal");

                PopulateAdminGrid(e.Row, "hfCountryId", "gvAdmin1", "GetOPSAdmin1TargetOfIndicatorForOpsClusCooReport");
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);

                HtmlGenericControl span = e.Row.FindControl("spanProject") as HtmlGenericControl;
                if (span != null)
                {
                    int indicatorId = Convert.ToInt32(gvActivities.DataKeys[e.Row.RowIndex].Value.ToString());
                    string s = GetIndProjects(indicatorId);
                    span.Attributes["data-content"] = s;
                }
            }
        }

        protected void gvAdmin1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UI.SetThousandSeparator(e.Row, "lblAdm1ClusterTotal");
                UI.SetThousandSeparator(e.Row, "lblAdm1DraftTotal");
                UI.SetThousandSeparator(e.Row, "lblAdm1ApprovedTotal");

                PopulateAdminGrid(e.Row, "hfAdmin1Id", "gvAdmin2", "GetOPSAdmin2TargetOfIndicatorForOpsClusCooReport");
            }
        }

        protected void gvAdmin2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                UI.SetThousandSeparator(e.Row, "lblAdm2ClusterTotal");
                UI.SetThousandSeparator(e.Row, "lblAdm2DraftTotal");
                UI.SetThousandSeparator(e.Row, "lblAdm2ApprovedTotal");

            }
        }

        protected void ddl_IndexChanged(object sender, EventArgs e)
        {
            PopulateIndicators();
        }

        #endregion

        #region Methods

        //internal override void BindGridData()
        //{
        //    PopulateIndicators();
        //}

        private void PopulateDropDowns()
        {
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            ddlCluster.Items.Insert(0, new ListItem("All", "0"));

            UI.FillEmergencyLocations(ddlLocation, RC.EmergencySahel2015);
            ddlLocation.Items.Insert(0, new ListItem("All", "0"));
        }

        private void PopulateIndicators()
        {
            DataTable dt = GetIndicators();
            gvActivities.DataSource = dt;
            gvActivities.DataBind();
        }

        private void SetDropDownOnRole()
        {
            if (RC.IsClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;

                ddlLocation.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlLocation.Enabled = false;
                ddlLocation.BackColor = Color.LightGray;

            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
                ddlCluster.Enabled = false;
                ddlCluster.BackColor = Color.LightGray;
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlLocation.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlLocation.Enabled = false;
                ddlLocation.BackColor = Color.LightGray;
            }
        }

        private DataTable GetIndicators()
        {
            int val = RC.GetSelectedIntVal(ddlLocation);
            int? emgLocId = val > 0 ? val : (int?)null;
            val = RC.GetSelectedIntVal(ddlCluster);
            int? emgClusterId = val > 0 ? val : (int?)null;
            int langId = RC.SelectedSiteLanguageId;
            DataTable dt = new DataTable();
            if (emgLocId > 0 && emgClusterId > 0)
            {
                dt = DBContext.GetData("GetAllIndicatorsForOPSTargetReport", new object[] { emgLocId, emgClusterId, 
                                                                                                langId, RC.Year._2016 });
            }

            return dt;
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

        private string GetIndProjects(int indId)
        {
            DataTable dt = DBContext.GetData("GetProjectsUsingInd", new object[] { indId });

            StringBuilder b = new StringBuilder();
            b.Append("<table class='imagetable'>");

            b.Append("<tr><th width=200px>Project Code</th>");
            b.Append("<th width=200px>Organization</th>");
            b.Append("<th>Draft Total</th>");
            b.Append("<th>Approved Total</th>");

            foreach (DataRow row in dt.Rows)
            {
                b.Append("<tr>");
                b.Append("<td>" + row["ProjectCode"].ToString() + "</td>");
                b.Append("<td>" + row["OrgName"].ToString() + "</td>");
                b.Append("<td>" + row["DraftProject"].ToString() + "</td>");
                b.Append("<td>" + row["ApprovedProject"].ToString() + "</td>");
                b.Append("</tr>");
            }

            b.Append("</table>");

            return b.ToString();
        }

        #endregion
    }
}