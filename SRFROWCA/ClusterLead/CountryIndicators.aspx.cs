using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using Microsoft.Reporting.WebForms;
using SRFROWCA.Common;

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

            if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
            {
                int maxIndicators = 0;
                DateTime endEditDate = DateTime.Now;

                int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                int emgClusterId = RC.GetSelectedIntVal(ddlCluster);
                GetIndCountAndEditDate(emgLocationId, emgClusterId, out maxIndicators, out endEditDate);
                if (maxIndicators <= 0 || DateTime.Now > endEditDate)
                {
                    btnAddIndicator.Enabled = false;
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
            UI.FillEmergnecyClusters(ddlCluster, RC.EmergencySahel2015);
            UI.FillUnits(ddlUnits);

            ddlCluster.Items.Insert(0, new ListItem("--- Select Cluster ---", "-1"));
            ddlCountry.Items.Insert(0, new ListItem("--- Select Country ---", "-1"));

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
            gvClusterIndicators.DataSource = SetDataSource();
            gvClusterIndicators.DataBind();
        }

        private DataTable SetDataSource()
        {
            string indicator = null;
            int? countryId = null;
            int? clusterId = null;

            if (!string.IsNullOrEmpty(txtIndicatorName.Text.Trim()))
                indicator = txtIndicatorName.Text;

            if (Convert.ToInt32(ddlCountry.SelectedValue) > -1)
                countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            if (Convert.ToInt32(ddlCluster.SelectedValue) > -1)
                clusterId = Convert.ToInt32(ddlCluster.SelectedValue);

            bool regionalIncluded = false;
            if (cbIncludeRegional.Visible)
            {
                regionalIncluded = cbIncludeRegional.Checked;
            }

            return DBContext.GetData("GetClusterIndicators", new object[] { clusterId, countryId, indicator,
                                                                               RC.SelectedSiteLanguageId, regionalIncluded, RC.EmergencySahel2015 });
        }

        protected void btnAddIndicator_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ClusterLead/AddCountryIndicator.aspx");
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
                    ddlCluster.SelectedValue = "-1";
                }
                if (ddlCountry.Items.Count > 0)
                {
                    ddlCountry.SelectedValue = "-1";
                }
            }

            if (RC.IsCountryAdmin(this.User))
            {
                if (ddlCluster.Items.Count > 0)
                {
                    ddlCluster.SelectedValue = "-1";
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

        private void GetIndCountAndEditDate(int emgLocationId, int emgClusterId, out int maxIndicators, out DateTime endEditDate)
        {
            string configKey = "Key-" + emgLocationId.ToString() + emgClusterId.ToString();

            maxIndicators = 0;
            endEditDate = DateTime.Now;

            string PATH = HttpRuntime.AppDomainAppPath;
            PATH = PATH.Substring(0, PATH.LastIndexOf(@"\") + 1) + @"Configurations\ChangeEndSettings.xml";

            if (File.Exists(PATH))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(PATH);

                XmlElement elem_settings = doc.GetElementById("ChangeEndSettings");
                XmlNode settingsNode = doc.DocumentElement;

                foreach (XmlNode node in settingsNode.ChildNodes)
                {
                    if (node.Name.Equals(configKey))
                    {
                        if (node.Attributes["ClusterCount"] != null)
                        {
                            maxIndicators = Convert.ToInt32(node.Attributes["ClusterCount"].Value);
                        }

                        if (node.Attributes["DateLimit"] != null)
                        {
                            endEditDate = DateTime.ParseExact(Convert.ToString(node.Attributes["DateLimit"].Value), "MM-dd-yyyy", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            if (maxIndicators > 0)
            {
                DataTable dt = DBContext.GetData("GetClusterIndicatorCount", new object[] { emgLocationId, emgClusterId });
                if (dt.Rows.Count > 0)
                {
                    int val = 0;
                    int.TryParse(dt.Rows[0]["IndicatorCount"].ToString(), out val);
                    maxIndicators -= val;
                }
            }
        }

        protected void gvClusterIndicators_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = SetDataSource();

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

                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                Label lblCountryId = e.Row.FindControl("lblCountryID") as Label;
                Label lblClusterId = e.Row.FindControl("lblClusterID") as Label;

                int emgLocationId = 0;
                int emgClusterId = 0;
                if (lblCountryId != null)
                {
                    int.TryParse(lblCountryId.Text, out emgLocationId);
                }

                if (lblClusterId != null)
                {
                    int.TryParse(lblClusterId.Text, out emgClusterId);
                }

                int maxIndicators = 0;
                DateTime endEditDate = DateTime.Now;
                GetIndCountAndEditDate(emgLocationId, emgClusterId, out maxIndicators, out endEditDate);

                if (btnDelete != null)
                {
                    btnDelete.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this Setting?')");

                    if (endEditDate < DateTime.Now)
                    {
                        if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }

                if (btnEdit != null && endEditDate < DateTime.Now)
                {
                    if (RC.IsClusterLead(this.User) || RC.IsRegionalClusterLead(this.User))
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

                int clusterIndicatorID = Convert.ToInt32(e.CommandArgument);

                ClearPopupControls();
                hfClusterIndicatorID.Value = clusterIndicatorID.ToString();

                GridViewRow row = (((Control)e.CommandSource).NamingContainer) as GridViewRow;
                hfRegionalIndicator.Value = row.Cells[1].Text;
                Label lblIndAlternate = row.FindControl("lblIndAlternate") as Label;
                Label lblUnitID = row.FindControl("lblUnitID") as Label;

                if (gvClusterIndicators.DataKeys[row.RowIndex].Value.ToString() == "1")
                {
                    txtIndicatorEng.Text = row.Cells[7].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorFr.Text = lblIndAlternate.Text;
                }
                else
                {
                    txtIndicatorFr.Text = row.Cells[7].Text;

                    if (lblIndAlternate != null)
                        txtIndicatorEng.Text = lblIndAlternate.Text;
                }

                string target = row.Cells[8].Text;
                if (!string.IsNullOrEmpty(target) && target != "&nbsp;")
                {
                    txtTarget.Text = target;
                }

                if (lblUnitID != null)
                    ddlUnits.SelectedValue = lblUnitID.Text;

                if (RC.IsAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                {
                    if (hfRegionalIndicator.Value == "True")
                    {
                        lblEditPopupHeading.Text = "Edit Indicator";
                        txtTarget.Enabled = false;
                        txtTarget.BackColor = Color.LightGray;
                    }
                    else
                    {
                        txtTarget.Enabled = true;
                        txtTarget.BackColor = Color.White;
                    }
                }
                else
                {
                    if (hfRegionalIndicator.Value == "True")
                    {
                        lblEditPopupHeading.Text = "Edit Target Of Regional Indicator";
                        EnableDisableEditControls(false);
                    }
                    else
                    {
                        lblEditPopupHeading.Text = "Edit Indicator";
                        EnableDisableEditControls(true);
                    }
                }

                mpeEditIndicator.Show();
            }
        }

        private void EnableDisableEditControls(bool isEnabled)
        {
            txtIndicatorEng.Enabled = isEnabled;
            txtIndicatorEng.BackColor = isEnabled ? Color.White : Color.LightGray;
            txtIndicatorFr.Enabled = isEnabled;
            txtIndicatorFr.BackColor = isEnabled ? Color.White : Color.LightGray;
            ddlUnits.Enabled = isEnabled;
            ddlUnits.BackColor = isEnabled ? Color.White : Color.LightGray;
        }

        private bool IndicatorIsInUse(int clusterIndicatorID)
        {
            DataTable dt = DBContext.GetData("ClusterIndicatorInUse", new object[] { clusterIndicatorID });
            return dt.Rows.Count > 0;
        }

        private void ClearPopupControls()
        {
            hfRegionalIndicator.Value = "";
            hfClusterIndicatorID.Value = "";
            txtIndicatorEng.Text = "";
            txtIndicatorFr.Text = "";
            txtTarget.Text = "";
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (hfRegionalIndicator.Value == "True")
            {
                if (RC.IsAdmin(this.User) || RC.IsRegionalClusterLead(this.User))
                {
                    SaveIndicatorWithoutTarget();
                }
                else
                {
                    SaveRegionalIndicatorTarget();
                }
            }
            else
            {
                SaveClusterIndicators();
            }

            LoadClusterIndicators();
            ClearPopupControls();
            mpeEditIndicator.Hide();
        }

        private void SaveIndicatorWithoutTarget()
        {
            if (string.IsNullOrEmpty(txtIndicatorEng.Text.Trim()))
            {
                txtIndicatorEng.Text = txtIndicatorFr.Text.Trim();
            }

            if (string.IsNullOrEmpty(txtIndicatorFr.Text.Trim()))
            {
                txtIndicatorFr.Text = txtIndicatorEng.Text.Trim();
            }

            string indicatorEng = txtIndicatorEng.Text.Trim();
            string indicatorFr = txtIndicatorFr.Text.Trim();
            int unitId = RC.GetSelectedIntVal(ddlUnits);

            int clusterIndicatorId = 0;
            int.TryParse(hfClusterIndicatorID.Value, out clusterIndicatorId);

            if (clusterIndicatorId > 0)
            {
                DBContext.Add("UpdateClusterIndicatorWithoutTarget", new object[] { clusterIndicatorId, indicatorEng, indicatorFr, unitId, RC.GetCurrentUserId, DBNull.Value, });
            }
        }

        private void SaveRegionalIndicatorTarget()
        {
            int clusterIndicatorId = 0;
            int.TryParse(hfClusterIndicatorID.Value, out clusterIndicatorId);
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int? target = !string.IsNullOrEmpty(txtTarget.Text.Trim()) ? Convert.ToInt32(txtTarget.Text.Trim()) : (int?)null;
            if (clusterIndicatorId > 0 && emgLocationId > 0)
            {
                DBContext.Add("InsertSahelIndicatorCountryTarget", new object[] { clusterIndicatorId, emgLocationId, target, RC.GetCurrentUserId, DBNull.Value });
            }
        }

        private void SaveClusterIndicators()
        {
            if (string.IsNullOrEmpty(txtIndicatorEng.Text.Trim()))
            {
                txtIndicatorEng.Text = txtIndicatorFr.Text.Trim();
            }

            if (string.IsNullOrEmpty(txtIndicatorFr.Text.Trim()))
            {
                txtIndicatorFr.Text = txtIndicatorEng.Text.Trim();
            }

            Guid userId = RC.GetCurrentUserId;
            string indicatorEng = txtIndicatorEng.Text.Trim();
            string indicatorFr = txtIndicatorFr.Text.Trim();
            int charIndex = txtTarget.Text.Trim().IndexOf('.');
            int? target = !string.IsNullOrEmpty(txtTarget.Text.Trim()) ? Convert.ToInt32(txtTarget.Text.Trim()) : (int?)null;
            int unitId = RC.GetSelectedIntVal(ddlUnits);
            int clusterIndicatorId = 0;
            int.TryParse(hfClusterIndicatorID.Value, out clusterIndicatorId);

            if (clusterIndicatorId > 0)
            {
                DBContext.Add("UpdateClusterIndicator", new object[] { clusterIndicatorId, indicatorEng, indicatorFr, target, unitId, RC.GetCurrentUserId, DBNull.Value, });
            }
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();

            DataTable dt = SetDataSource();

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
                dt.Columns.Remove("CountryID");
                dt.Columns.Remove("ClusterId");
                dt.Columns.Remove("UnitId");
                dt.Columns.Remove("IsSRP");
                dt.Columns.Remove("IsRegional");

            }
            catch { }
        }

    }
}