using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SRFROWCA.Common;
using System.Data;
using BusinessLogic;
using System.IO;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Web.UI.HtmlControls;

namespace SRFROWCA.ClusterLead
{
    public partial class ProjectsListing : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            PopulateControls();
            LoadProjects();
            if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
            {
                PopulateClusters();
            }
            else
            {
                ddlClusters.Visible = false;
                divClusters.Visible = false;
            }
        }

        private void PopulateControls()
        {
            if (!RC.IsDataEntryUser(User))
                PopulateOrganizations();
            else
            {
                ddlOrg.Visible = false;
                divOrganization.Visible = false;
            }


            PopulateLocations();
            //PopulateProjectStatus();
        }

        private void PopulateClusters()
        {
            int emgId = 1;
            UI.FillEmergnecyClusters(ddlClusters, emgId);
            RC.AddSelectItemInList(ddlClusters, "Select Cluster");
        }

        private void PopulateOrganizations()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

            DataTable dt = RC.GetProjectsOrganizations(UserInfo.EmergencyCountry, clusterId);
            UI.FillOrganizations(ddlOrg, dt);

            if (ddlOrg.Items.Count > 0)
            {
                ListItem item = new ListItem("All", "0");
                ddlOrg.Items.Insert(0, item);
            }
        }

        private void PopulateLocations()
        {
            UI.FillAdmin1(ddlAdmin1, UserInfo.Country);

            ListItem item = new ListItem("All", "0");
            if (ddlAdmin1.Items.Count > 0)
            {
                ddlAdmin1.Items.Insert(0, item);
            }

            //UI.FillAdmin2(ddlAdmin2, UserInfo.Country);
            //if (ddlAdmin2.Items.Count > 0)
            //{
            //    ddlAdmin2.Items.Insert(0, item);
            //}
        }

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetProjects();
            //RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, "ProjectListing", ".xls", Response, true);
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            ExportToPDF(null);
        }

        private void ExportToPDF(int? projectId)
        {
            DataTable dtResults = DBContext.GetData("uspGetReports", new object[] { projectId, null, null , null});

            if (dtResults.Rows.Count > 0)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Project-{0}-{1}.pdf", UserInfo.CountryName, DateTime.Now.ToString("yyyyMMddHHmmss")));
                Response.BinaryWrite(WriteDataEntryPDF.GeneratePDF(dtResults, projectId, null).ToArray());
            }
        }

        //private void PopulateProjectStatus()
        //{

        //}

        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void ddlAdmin1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        //protected void ddlAdmin2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjects();
        //}

        //protected void ddlProjStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void cblReportingStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        protected void gvProjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewProject")
            {
                //Session["ViewProjectId"] = e.CommandArgument.ToString();
                Response.Redirect("~/ClusterLead/ProjectDetails.aspx?pid=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName == "PrintReport")
            {
                ExportToPDF(Convert.ToInt32(e.CommandArgument));
            }
        }

        protected void gvProjects_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetProjects();
            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvProjects.DataSource = dt;
                gvProjects.DataBind();
            }
        }

        private string GetSortDirection(string column)
        {
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjects.PageIndex = e.NewPageIndex;
            gvProjects.SelectedIndex = -1;
            LoadProjects();
        }

        private void LoadProjects()
        {
            gvProjects.DataSource = GetProjects();
            gvProjects.DataBind();
        }

        private DataTable GetProjects()
        {
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }
            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;

            string projCode = txtProjectCode.Text.Trim().Length > 0 ? txtProjectCode.Text.Trim() : null;
            int? orgId = RC.GetSelectedIntVal(ddlOrg);

            if (orgId == 0)
            {
                if (RC.IsDataEntryUser(User))
                    orgId = UserInfo.Organization;
                else
                    orgId = (int?)null;
            }

            int? admin1 = RC.GetSelectedIntVal(ddlAdmin1);
            if (admin1 == 0)
            {
                admin1 = (int?)null;
            }

            //int? admin2 = RC.GetSelectedIntVal(ddlAdmin2);
            //if (admin2 == 0)
            //{
            //    admin2 = (int?)null;
            //}

            int? cbReported = cblReportingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblReportingStatus) : (int?)null;
            int? cbFunded = cblFundingStatus.SelectedIndex > -1 ? RC.GetSelectedIntVal(cblFundingStatus) : (int?)null;

            int? countryId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;

            //return DBContext.GetData("GetProjects", new object[] {countryId, clusterId,projCode, orgId, admin1, admin2, 
            //                                                                DBNull.Value, cbFunded, cbReported, 1 });

            return DBContext.GetData("GetProjects", new object[] {countryId, clusterId,projCode, orgId, admin1, DBNull.Value, 
                                                                            DBNull.Value, cbFunded, cbReported, 1 });
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }
    }
}