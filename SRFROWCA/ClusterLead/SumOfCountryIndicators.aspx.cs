using Microsoft.Reporting.WebForms;
using System;

using System.Data;
using System.Web.UI.WebControls;
using BusinessLogic;
using SRFROWCA.Common;

namespace SRFROWCA.ClusterLead
{
    public partial class SumOfCountryIndicators : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RC.IsCountryAdmin(User) || RC.IsOCHAStaff(User))
                {
                    PopulateClusters();
                    LoadEmptyData();
                }
                else
                {
                    divCluster.Visible = false;
                    LoadData();
                }
            }
        }

        internal override void BindGridData()
        {
            LoadData();
        }

        private void LoadEmptyData()
        {
            gvReport.DataSource = new DataTable();
            gvReport.DataBind();
        }

        private void LoadData()
        {
            gvReport.DataSource = GetData(true);
            gvReport.DataBind();
        }

        private DataTable GetData(bool isFilter)
        {
            int tempVal = 0;
            if (isFilter && ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int yearId = 10;

            return DBContext.GetData("GetAllCountryIndicatorsAchievedReport", new object[] {emgLocationId, clusterId, yearId, RC.SelectedSiteLanguageId});
        }

        private void PopulateClusters()
        {
            int emgId = 1;
            UI.FillEmergnecyClusters(ddlClusters, emgId);
            RC.AddSelectItemInList(ddlClusters, "Select Cluster");
        }

        protected void ddlClusters_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.ObjectiveIconToolTip(e, 0);
                ObjPrToolTip.PrioritiesIconToolTip(e, 1);
            }
        }

        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetData(true);
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);

            gvReport.DataSource = dt;
            gvReport.DataBind();
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

        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = GetData(true);
            RemoveColumnsFromDataTable(dt);
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();

            ExportUtility.ExportGridView(gv, UserInfo.CountryName + "_" + RC.GetClusterName + "_Indicator_Target", ".xls", Response, true);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            dt.Columns.Remove("ObjectiveId");
            dt.Columns.Remove("ObjAndPrId");
            dt.Columns.Remove("HumanitarianPriorityId");
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            int tempVal = 0;
            if (ddlClusters.Visible)
            {
                int.TryParse(ddlClusters.SelectedValue, out tempVal);
            }

            int? clusterId = tempVal > 0 ? tempVal : UserInfo.EmergencyCluster > 0 ? UserInfo.EmergencyCluster : (int?)null;
            int? emgLocationId = UserInfo.EmergencyCountry > 0 ? UserInfo.EmergencyCountry : (int?)null;
            int yearId = 10;
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            rvCountry.ServerReport.ReportPath = "/reports/countryindicators"; 
            ReportParameter[] RptParameters = new ReportParameter[4];
            RptParameters[0] = new ReportParameter("emgLocationId", emgLocationId.ToString());
            RptParameters[1] = new ReportParameter("emgClusterId", clusterId.ToString());
            RptParameters[2] = new ReportParameter("langId", RC.SelectedSiteLanguageId.ToString());
            RptParameters[3] = new ReportParameter("yearId", yearId.ToString());
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            byte[] bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=CountryReport.pdf");
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            ExceptionUtility.LogException(exc, User);
        }
    }
}