﻿using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
                LoadCountry();
                DisableDropDowns();
                LoadCategories();
                LoadKeyFigures();
            }
        }

        internal override void BindGridData()
        {
            LoadCategories();
            LoadKeyFigures();
        }

        private void LoadCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
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
                ddlCountry.SelectedValue = UserInfo.EmergencyCountry.ToString();
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
            gvKeyFigures.DataSource = GetKeyFigures();
            gvKeyFigures.DataBind();
        }

        private DataTable GetKeyFigures()
        {
            int val = RC.GetSelectedIntVal(ddlCountry);
            int? emgLocationId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlCategory);
            int? catId = val > 0 ? val : (int?)null;

            val = RC.GetSelectedIntVal(ddlSubCategory);
            int? subCatId = val > 0 ? val : (int?)null;

            int? kfIndId = null;
            DateTime dateTime = DateTime.Now;
            DateTime? fromDate = null;
            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                DateTime.TryParse(txtFromDate.Text.Trim(), out dateTime);
                fromDate = dateTime == DateTime.MinValue ? (DateTime?)null : dateTime;
            }

            DateTime? toDate = null;
            if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
            {
                DateTime.TryParse(txtToDate.Text.Trim(), out dateTime);
                toDate = dateTime == DateTime.MinValue ? (DateTime?)null : dateTime;
            }

            return DBContext.GetData("GetKeyFigureListing", new object[] {emgLocationId, catId, subCatId,
                                                                                    kfIndId, fromDate, toDate, 
                                                                                    RC.SelectedSiteLanguageId, DBNull.Value});
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadKeyFigures();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            //if (categoryId > 0)
            {
                LoadSubCategories(categoryId);
            }
            LoadKeyFigures();
        }

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadKeyFigures();
        }

        protected void btnExportToExcel_ServerClick(object sender, EventArgs e) 
        {
            GridView gvExport = new GridView();
            DataTable dt = GetKeyFigures();
            gvExport.DataSource = dt;
            gvExport.DataBind();

            string fileName = "KeyFigures";
            string fileExtention = ".xls";
            ExportUtility.ExportGridView(gvExport, fileName, fileExtention, Response);
        }

        protected void btnNew_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect("AddKeyFigure.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadKeyFigures();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            if (RC.IsAdmin(this.User))
            {
                ddlCountry.SelectedIndex = 0;
            }
            ddlCategory.SelectedIndex = 0;
            ddlSubCategory.SelectedIndex = 0;

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
    }
}