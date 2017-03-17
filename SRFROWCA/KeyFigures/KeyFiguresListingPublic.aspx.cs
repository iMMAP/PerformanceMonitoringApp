using BusinessLogic;
using ClosedXML.Excel;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class KeyFiguresListingPublic : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvKeyFigures.VirtualItemCount = 200;
                LoadCountry();
                DisableDropDowns();
                LoadCategories();
                LoadKeyFigures();
            }                     
            
            if (ScriptManager.GetCurrent(this).IsInAsyncPostBack)
            {
                var c = ScriptManager.GetCurrent(this).AsyncPostBackSourceElementID;
                int j = 0;
            }
        }

        internal override void BindGridData()
        {
            LoadCategories();
            LoadKeyFigures();
        }

        private void LoadCountry()
        {
            //UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            UI.FillCountry(ddlCountry);
            ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            SetComboValues();
        }

        private void LoadCategories()
        {
            ddlCategory.DataTextField = "KeyFigureCategory";
            ddlCategory.DataValueField = "KeyFigureCategoryId";
            ddlCategory.DataSource = DBContext.GetData("GetKeyFigureCategories", new object[] { RC.SelectedSiteLanguageId });
            ddlCategory.DataBind();

            if (ddlCategory.Items.Count > 0)
            {
                ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
            }
        }

        private void SetComboValues()
        {
            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User))
            {
                //ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
                ddlCountry.SelectedValue = UserInfo.Country.ToString();
            }
        }

        private void DisableDropDowns()
        {
            if (RC.IsClusterLead(this.User) || RC.IsCountryAdmin(this.User))
            {
                RC.EnableDisableControls(ddlCountry, false);
            }
        }

        private void LoadSubCategories(int categoryId)
        {
            ddlSubCategory.DataTextField = "KeyFigureSubCategory";
            ddlSubCategory.DataValueField = "KeyFigureSubCategoryId";
            ddlSubCategory.DataSource = DBContext.GetData("GetKeyFigureSubCategories", new object[] { categoryId, RC.SelectedSiteLanguageId });
            ddlSubCategory.DataBind();

            //if (ddlSubCategory.Items.Count > 0)
            {
                ddlSubCategory.Items.Insert(0, new ListItem("Select Sub Category", "0"));
            }
        }

        private void LoadKeyFigures()
        {
            DataTable dt = GetKeyFigures(true);
            if (dt.Rows.Count > 0)
            {
                gvKeyFigures.VirtualItemCount = Convert.ToInt32(dt.Rows[0]["VirtualCount"].ToString());
            }
            gvKeyFigures.DataSource = dt;
            gvKeyFigures.DataBind();
        }

        private DataTable GetKeyFigures(bool paging)
        {
            int val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocationId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlCategory);
            int? catId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlSubCategory);
            int? subCatId = val > 0 ? val : (int?)null;

            int? kfIndId = null;

            DateTime dateTime = DateTime.MinValue;
            string fromDate = null;
            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                DateTime.TryParseExact(txtFromDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (dateTime != DateTime.MinValue)
                {
                    fromDate = dateTime.ToString("yyyyMMdd");
                }
            }

            string toDate = null;
            dateTime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                DateTime.TryParseExact(txtToDate.Text.Trim(), "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (dateTime != DateTime.MinValue)
                {
                    toDate = dateTime.ToString("yyyyMMdd");
                }
            }

            string keyFigure = (!string.IsNullOrEmpty(txtKeyFigure.Text.Trim())) ? txtKeyFigure.Text.Trim() : null;

            int isLatest = cbShowAll.Checked ? 1 : 0;
            int? pageSize = null;
            int? pageIndex = null;
            if (paging)
            {
                pageSize = gvKeyFigures.PageSize;
                pageIndex = gvKeyFigures.PageIndex;
            }

            return DBContext.GetData("GetKeyFigureListing", new object[] {emgLocationId, catId, subCatId,
                                                                                    kfIndId, fromDate, toDate, 
                                                                                    keyFigure, isLatest,
                                                                                    RC.SelectedSiteLanguageId, pageSize, pageIndex});
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvKeyFigures.PageIndex = 0;
            LoadKeyFigures();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvKeyFigures.PageIndex = 0;
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            LoadSubCategories(categoryId);
            LoadKeyFigures();
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvKeyFigures.PageIndex = 0;
            LoadKeyFigures();
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e)
        {
            GridView gvExport = new GridView();
            DataTable dt = GetKeyFigures(false);

            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt, "KeyFiguresORS");  //pass datatable and Worksheetname

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Maram_workbook_LineItems.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            //RemoveColumnsFromDataTable(dt);
            //gvExport.DataSource = dt;
            //gvExport.DataBind();

            //string fileName = "KeyFigures";
            //string fileExtention = ".xlsx";
            //ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        private void RemoveColumnsFromDataTable(DataTable dt)
        {
            try
            {
                dt.Columns.Remove("AsOfDate3");
            }
            catch { }
        }

        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("AddKeyFigure.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvKeyFigures.PageIndex = 0;
            LoadKeyFigures();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtKeyFigure.Text = "";
            if (RC.IsAdmin(this.User))
            {
                ddlCountry.SelectedIndex = 0;
            }
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;
            cbShowAll.Checked = false;
            gvKeyFigures.PageIndex = 0;

            LoadKeyFigures();
        }

        protected void gvKeyFigures_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                FormatThousandSeperator(e.Row, "lblTotalTotal");
                FormatThousandSeperator(e.Row, "lblTotalMen");
                FormatThousandSeperator(e.Row, "lblTotalWomen");
                FormatThousandSeperator(e.Row, "lblNeedTotal");
                FormatThousandSeperator(e.Row, "lblNeedlMen");
                FormatThousandSeperator(e.Row, "lblNeedWomen");
                FormatThousandSeperator(e.Row, "lblTargetedTotal");
                FormatThousandSeperator(e.Row, "lblTargetedlMen");
                FormatThousandSeperator(e.Row, "lblTargetedWomen");

                ImageButton deleteButton = e.Row.FindControl("btnDelete") as ImageButton;
                if (deleteButton != null)
                {
                    deleteButton.Attributes.Add("onclick", "javascript:return " +
                    "confirm('Are you sure you want to delete this record?')");
                }
            }
        }

        private void FormatThousandSeperator(GridViewRow row, string ctl)
        {
            string siteCulture = RC.SelectedSiteLanguageId.Equals(1) ? "en-US" : "de-DE";
            Label lbl = row.FindControl(ctl) as Label;
            if (lbl != null && !string.IsNullOrEmpty(lbl.Text))
                if (lbl.Text.Length > 1)
                {
                    lbl.Text = String.Format(new CultureInfo(siteCulture), "{0:0,0}", Convert.ToInt32(lbl.Text));
                }
        }

        protected void gvKeyFigures_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKeyFigures.PageIndex = e.NewPageIndex;
            gvKeyFigures.SelectedIndex = -1;

            LoadKeyFigures();
        }

        protected void gvKeyFigures_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = GetKeyFigures(true);
            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvKeyFigures.DataSource = dt;
                gvKeyFigures.DataBind();
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

        protected void cbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            gvKeyFigures.PageIndex = 0;
            LoadKeyFigures();
        }

        
    }
}