using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.ClusterLead
{
    public partial class CountryIndicators : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo.UserProfileInfo(RC.EmergencySahel2015);
                LoadCombos();
                DisableDropDowns();
                LoadClusterIndicators();
            }

            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                if (RC.IsRegionalClusterLead(this.User))
                {
                    emgLocationId = 11;
                }

                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
                FrameWorkSettingsCount frCount = FrameWorkUtil.GetOutputFrameworkSettings(emgLocationId, emgClusterId);
                if (frCount.ClsIndCount <= 0 || frCount.DateExcedded)
                {
                    btnAddIndicator.Enabled = false;
                }
                else
                {
                    btnAddIndicator.Enabled = true;
                }
            }
            else
            {
                btnAddIndicator.Enabled = true;
            }
        }

        // Disable Controls on the basis of user profile
        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
                RC.EnableDisableControls(ddlCountry, false);
            }

            if (RC.IsRegionalClusterLead(this.User))
            {
                RC.EnableDisableControls(ddlCluster, false);
            }

            if (RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void LoadCombos()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            else
                ddlCountry.Items.Insert(0, new ListItem("Sélectionner Pays", "0"));

            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            if (RC.SelectedSiteLanguageId == 1)
                ddlCluster.Items.Insert(0, new ListItem("Select Cluster", "0"));
            else
                ddlCluster.Items.Insert(0, new ListItem("Sélectionner Cluster", "0"));

            SetComboValues();
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCluster.SelectedValue = UserInfo.EmergencyCluster.ToString();
            }

            if (RC.IsCountryAdmin(this.User))
            {
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
            }
        }

        private void LoadClusterIndicators()
        {
            gvClusterIndicators.DataSource = GetIndicators();
            gvClusterIndicators.DataBind();
        }

        private DataTable GetIndicators()
        {
            string indicator = null;
            int? countryId = null;
            int? clusterId = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > 0)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > 0)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            bool regionalIncluded = false;
            if (cbIncludeRegional.Visible)
            {
                regionalIncluded = cbIncludeRegional.Checked;
            }

            int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);

            return DBContext.GetData("GetClusterIndicators", new object[] { clusterId, countryId, indicator,
                                                                            yearId, RC.SelectedSiteLanguageId, regionalIncluded, 
                                                                               RC.EmergencySahel2015 });
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/EditOutputIndicator.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (RC.IsAdmin(this.User))
            {
                if (ddlCluster.Items.Count > 0)
                {
                    ddlCluster.SelectedValue = "0";
                }
                if (ddlCountry.Items.Count > 0)
                {
                    ddlCountry.SelectedValue = "0";
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                if (ddlCluster.Items.Count > 0)
                {
                    ddlCluster.SelectedValue = "0";
                }
            }

            txtIndicatorName.Text = "";
            cbIncludeRegional.Checked = true;
            LoadClusterIndicators();
        }

        protected void cbIncudeRegional_CheckedChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = GetIndicators();

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvClusterIndicators.DataSource = dt;
                gvClusterIndicators.DataBind();
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

        protected void gvClusterindicators_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClusterIndicators.PageIndex = e.NewPageIndex;
            LoadClusterIndicators();
        }

        protected void gvClusterIndicators_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ObjPrToolTip.RegionalIndicatorIcon(e, 1);
                ObjPrToolTip.CountryIndicatorIcon(e, 2);
                UI.SetThousandSeparator(e.Row, "lblIndTarget");

                ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                ImageButton btnEdit = e.Row.FindControl("btnEdit") as ImageButton;

                int yearId = RC.GetSelectedIntVal(ddlFrameworkYear);
                if (yearId == 11)
                {
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                }
                else
                {
                    Label lblCountryId = e.Row.FindControl("lblCountryID") as Label;
                    Label lblClusterId = e.Row.FindControl("lblClusterID") as Label;

                    int emgLocationId = 0;

                    if (lblCountryId != null)
                    {
                        if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User))
                        {
                            emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                        }
                        else
                        {
                            int.TryParse(lblCountryId.Text, out emgLocationId);
                        }
                    }

                    int emgClusterId = 0;
                    if (lblClusterId != null)
                    {
                        int.TryParse(lblClusterId.Text, out emgClusterId);
                    }

                    FrameWorkSettingsCount frCount = FrameWorkUtil.GetOutputFrameworkSettings(emgLocationId, emgClusterId);
                    if (btnDelete != null)
                    {
                        btnDelete.Attributes.Add("onclick", "javascript:return " +
                        "confirm('Are you sure you want to delete this Indicators?')");

                        if (frCount.DateExcedded)
                        {
                            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                            {
                                btnDelete.Visible = false;
                            }
                        }
                    }

                    if (btnEdit != null && frCount.DateExcedded)
                    {
                        if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                        {
                            btnEdit.Visible = false;
                        }
                    }

                    string isRegional = e.Row.Cells[1].Text;
                    if ((RC.IsCountryAdmin(this.User) || RC.IsClusterLead(this.User)) && isRegional == "True")
                    {
                        btnDelete.Visible = false;
                    }

                    string isCountry = e.Row.Cells[2].Text;
                    if (RC.IsRegionalClusterLead(this.User) && isCountry == "True")
                    {
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
            }
        }

        protected void gvClusterIndicators_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteIndicator")
            {
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                if (!IndicatorIsInUse(clusterIndicatorID))
                {
                    DeleteClusterIndicator(clusterIndicatorID);
                    LoadClusterIndicators();
                    ShowMessage("Indicator Deleted Successfully.");
                }
                else
                {
                    ShowMessage("Indicator can not be deleted. It is being used!", RC.NotificationType.Error, true, 2000);
                }
            }
            else if (e.CommandName == "EditIndicator")
            {
                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                Label lblCountryID = row.FindControl("lblCountryID") as Label;
                int cnId = 0;
                if (lblCountryID != null)
                {
                    int.TryParse(lblCountryID.Text.ToString(), out cnId);
                }

                Label lblClusterID = row.FindControl("lblClusterID") as Label;
                int cId = 0;
                if (lblClusterID != null)
                {
                    int.TryParse(lblClusterID.Text.ToString(), out cId);
                }

                int countryId = RC.GetSelectedIntVal(ddlCountry);
                cnId = countryId > 0 ? countryId : cnId;

                Response.Redirect("~/ClusterLead/EditOutputIndicator.aspx?id=" + clusterIndicatorID.ToString() + "&cid=" + cId.ToString() + "&cnid=" + cnId.ToString());
            }
        }

        private bool IndicatorIsInUse(int clusterIndicatorID)
        {
            DataTable dt = DBContext.GetData("ClusterIndicatorInUse", new object[] { clusterIndicatorID });
            return dt.Rows.Count > 0;
        }

        private bool DeleteClusterIndicator(int indicatorID)
        {
            return Convert.ToBoolean(DBContext.Delete("uspDeleteClusterIndicator", new object[] { indicatorID, null }));
        }

        protected void ddlCluster_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

        internal override void BindGridData()
        {
            LoadCombos();
            LoadClusterIndicators();
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = GetIndicators();
            RemoveColumnsFromDataTable(dt);
            dt.DefaultView.Sort = "Country, Cluster, Indicator, Unit";
            gvExport.DataSource = dt.DefaultView;
            gvExport.DataBind();

            string fileName = "ClusterIndicators";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        protected void ExportToPDF(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes;
            ReportViewer rvCountry = new ReportViewer();
            rvCountry.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
            rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://win-78sij2cjpjj/Reportserver");
            //rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://54.83.26.190/Reportserver");
            ReportParameter[] RptParameters = null;
            // rvCountry.ServerReport.ReportServerUrl = new System.Uri("http://localhost/Reportserver");
            string indicator = null;
            string countryID = ddlCountry.SelectedValue;
            string clusterID = ddlCluster.SelectedValue;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            RptParameters = new ReportParameter[6];
            RptParameters[0] = new ReportParameter("pClusterID", clusterID, false);
            RptParameters[1] = new ReportParameter("pCountryID", countryID, false);
            RptParameters[2] = new ReportParameter("pIndicator", indicator, false);
            RptParameters[3] = new ReportParameter("pLangId", ((int)RC.SiteLanguage.English).ToString(), false);
            RptParameters[4] = new ReportParameter("includeRegional", cbIncludeRegional.Checked ? "true" : "false", false);
            RptParameters[5] = new ReportParameter("emergencyId", RC.EmergencySahel2015.ToString(), false);

            rvCountry.ServerReport.ReportPath = "/reports/outputindicators";
            string fileName = "ClusterIndicators" + DateTime.Now.ToString("yyyy-MM-dd_hh_mm_ss") + ".pdf";
            rvCountry.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "&qisW.c@Jq", "");
            rvCountry.ServerReport.SetParameters(RptParameters);
            bytes = rvCountry.ServerReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush();
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("ClusterIndicatorId");
                dt.Columns.Remove("SiteLanguageId");
                dt.Columns.Remove("IndicatorAlt");
                dt.Columns.Remove("EmergencyLocationId");
                dt.Columns.Remove("EmergencyClusterId");
                dt.Columns.Remove("UnitId");
                dt.Columns.Remove("IsSRP");
            }
            catch { }
        }

        protected void ddlYear_SelectedIndexChnaged(object sender, EventArgs e)
        {
            LoadClusterIndicators();
        }

    }
}