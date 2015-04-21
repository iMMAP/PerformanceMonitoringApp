using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class AddKeyFigure : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!this.User.Identity.IsAuthenticated || RC.IsDataEntryUser(this.User))
                {
                    Response.Redirect("~/Default.aspx");
                }
                IsPopulation = 0;
                LoadCountry();
                DisableDropDowns();
                LoadCategories();                
                txtFromDate.BackColor = ColorTranslator.FromHtml("#FFE6E6");
                if (Request.QueryString["d"] == null && Request.QueryString["u"] == null)
                {
                    SetFiltersFromSession();
                }

                if (Request.QueryString["d"] != null)
                {
                    LoadReportData();
                }
                else
                {
                    LoadData();
                }
            }
        }

        private void SetFiltersFromSession()
        {
            if (Session["KeyFigureFilterCountry"] != null)
            {
                int emgLocationId = 0;
                int.TryParse(Session["KeyFigureFilterCountry"].ToString(), out emgLocationId);
                if (emgLocationId > 0)
                {
                    try
                    {
                        ddlCountry.SelectedValue = emgLocationId.ToString();
                    }
                    catch { }
                }
            }

            if (Session["KeyFigureFilterCategory"] != null)
            {
                int catId = 0;
                int.TryParse(Session["KeyFigureFilterCategory"].ToString(), out catId);
                if (catId > 0)
                {
                    try
                    {
                        ddlCategory.SelectedValue = catId.ToString();
                        LoadSubCategories(catId);
                    }
                    catch { }
                }
            }

            if (Session["KeyFigureFilterSubCategory"] != null)
            {
                int subCatId = 0;
                int.TryParse(Session["KeyFigureFilterSubCategory"].ToString(), out subCatId);
                if (subCatId > 0)
                {
                    try
                    {
                        ddlSubCategory.SelectedValue = subCatId.ToString();
                    }
                    catch { }
                }
            }
        }

        private void LoadReportData()
        {
            DateTime date = DateTime.MinValue;
            if (Request.QueryString["d"] != null)
            {
                date = DateTime.ParseExact(Request.QueryString["d"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }

            int emgLocId = 0;
            if (Request.QueryString["l"] != null)
            {
                int.TryParse(Request.QueryString["l"].ToString(), out emgLocId);
            }

            int catId = 0;
            if (Request.QueryString["c"] != null)
            {
                int.TryParse(Request.QueryString["c"].ToString(), out catId);
                if (catId > 0)
                    LoadSubCategories(catId);
            }

            int subCatId = 0;
            if (Request.QueryString["s"] != null)
            {
                int.TryParse(Request.QueryString["s"].ToString(), out subCatId);
                if (subCatId > 0)
                    UpdateIsPopulationFlag(subCatId);
            }

            if (date != DateTime.MinValue && emgLocId > 0 && catId > 0 && subCatId > 0)
            {
                txtFromDate.Text = date.ToString("MM/dd/yyyy");
                ddlCountry.SelectedValue = emgLocId.ToString();
                ddlCategory.SelectedValue = catId.ToString();
                ddlSubCategory.SelectedValue = subCatId.ToString();

                if (Request.QueryString["u"] == null)
                {
                    RC.EnableDisableControls(txtFromDate, false);
                }
                else
                {
                    IsDuplicate = 1;
                }
                RC.EnableDisableControls(ddlCountry, false);
                RC.EnableDisableControls(ddlCategory, false);
                RC.EnableDisableControls(ddlSubCategory, false);

                LoadData();
            }

        }

        private void UpdateIsPopulationFlag(int subCatId)
        {
            DataTable dt = DBContext.GetData("GetIsPopulationFlagFromKeyFigSubCat", new object[] { subCatId });
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["IsPopulation"].ToString() == "True")
                {
                    IsPopulation = 1;
                }
                else
                {
                    IsPopulation = 0;
                }
            }
        }

        internal override void BindGridData()
        {
            LoadCategories();
        }

        private void LoadCountry()
        {
            UI.FillEmergencyLocations(ddlCountry, RC.EmergencySahel2015);
            ddlCountry.Items.Insert(0, new ListItem("All", "0"));
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

        private void LoadSubCategories(int categoryId)
        {
            ddlSubCategory.DataTextField = "KeyFigureSubCategory";
            ddlSubCategory.DataValueField = "KeyFigureSubCategoryId";
            ddlSubCategory.DataSource = DBContext.GetData("GetKeyFigureSubCategories", new object[] { categoryId, RC.SelectedSiteLanguageId });
            ddlSubCategory.DataBind();

            if (ddlSubCategory.Items.Count > 0)
            {
                ddlSubCategory.Items.Insert(0, new ListItem("Select Sub Category", "0"));
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

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            if (categoryId > 0)
            {
                LoadSubCategories(categoryId);
                UpdateIsPopulationFlag(categoryId);
                LoadData();
            }
        }

        public void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int subCategoryId = RC.GetSelectedIntVal(ddlSubCategory);
            UpdateIsPopulationFlag(subCategoryId);
            LoadData();
        }

        private void LoadData()
        {
            DateTime date = DateTime.MinValue;
            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
            {
                date = DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (Request.QueryString["u"] != null)
                {
                    txtFromDate.Text = "";
                }
            }
            int subCatId = RC.GetSelectedIntVal(ddlSubCategory);
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            DataTable dt = DBContext.GetData("GetKeyFigureReport", new object[] { date, emgLocationId, subCatId, RC.SelectedSiteLanguageId });
            rptKeyFigure.DataSource = dt;
            rptKeyFigure.DataBind();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveKeyFigures())
                Response.Redirect("KeyFiguresListing.aspx");
        }

        private void ShowMessage(string message, RC.NotificationType notificationType = RC.NotificationType.Success, bool fadeOut = true, int animationTime = 500)
        {
            RC.ShowMessage(Page, typeof(Page), UniqueID, message, notificationType, fadeOut, animationTime);
        }

        private bool SaveKeyFigures()
        {
            bool returnVal = true;
            DateTime date = txtFromDate.Text.Trim().Length > 0 ?
                                DateTime.ParseExact(txtFromDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture) :
                                DateTime.MinValue;

            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int kfSubCategoryId = RC.GetSelectedIntVal(ddlSubCategory);
            if (!IsSourceProvided())
            {
                ShowMessage("Error Saving! Please provide source of the reported Key Figure(s).", RC.NotificationType.Error, true, 3000);
                returnVal = false;
            }
            else
            {
                if (!IsExistsInCaseOfDuplicate(date, emgLocationId, kfSubCategoryId))
                {
                    foreach (RepeaterItem item in rptKeyFigure.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            int kfIndicatorId = GetValueFromHiddenField(item, "hfKeyFigureIndicatorId");
                            int kfReportId = GetValueFromHiddenField(item, "hfKeyFigureReportId");

                            if (kfReportId > 0 && IsDuplicate == 0)
                            {
                                DeleteKeyFigure(kfReportId);
                            }

                            SaveKeyFigure(item, date, emgLocationId, kfSubCategoryId, kfIndicatorId);
                            SaveAdmin1Values(item, date, emgLocationId, kfSubCategoryId, kfIndicatorId);
                        }
                    }
                    LoadData();
                }
                else
                {
                    ShowMessage("The data is already exists for this date and key figures.", RC.NotificationType.Error, true, 2000);
                    returnVal = false;
                }
            }

            return returnVal;
        }

        private bool IsSourceProvided()
        {
            bool returnVal = true;
            foreach (RepeaterItem item in rptKeyFigure.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    returnVal = KeyFigureSource(item);
                    if (returnVal)
                    {
                        returnVal = IsAdmin1SourceProvided(item);
                        if (!returnVal)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return returnVal;
        }

        private bool IsAdmin1SourceProvided(RepeaterItem parentRepeater)
        {
            bool returnVal = true;
            Repeater rptAdmin1 = parentRepeater.FindControl("rptAdmin1") as Repeater;
            if (rptAdmin1 != null)
            {
                foreach (RepeaterItem item in rptAdmin1.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        returnVal = KeyFigureSource(item);
                        if (!returnVal) 
                            break;
                    }
                }
            }

            return returnVal;
        }

        private bool KeyFigureSource(RepeaterItem item)
        {
            bool returnVal = true;
            int? totalTotal = KeyFigureReported(item, "txtTotalTotal");
            int? totalMen = KeyFigureReported(item, "txtTotalMen");
            int? totalWomen = KeyFigureReported(item, "txtTotalWomen");
            int? needTotal = KeyFigureReported(item, "txtNeedTotal");
            int? needMen = KeyFigureReported(item, "txtNeedMen");
            int? needWomen = KeyFigureReported(item, "txtNeedWomen");
            int? targatedTotal = KeyFigureReported(item, "txtTargetedTotal");
            int? targatedMen = KeyFigureReported(item, "txtTargetedMen");
            int? targatedWomen = KeyFigureReported(item, "txtTargetedWomen");

            string kfSource = null;
            TextBox txtKfSource = item.FindControl("txtKFSouce") as TextBox;
            if (txtKfSource != null)
            {
                if (!string.IsNullOrEmpty(txtKfSource.Text.Trim()))
                    kfSource = txtKfSource.Text.Trim();
            }

            bool valueProvied = (totalTotal != null || totalMen != null || totalWomen != null ||
                                 needTotal != null || needMen != null || needWomen != null ||
                                 targatedTotal != null || targatedMen != null || targatedWomen != null);
            if (valueProvied)
            {
                if (string.IsNullOrEmpty(txtKfSource.Text.Trim()))
                {
                    returnVal = false;
                }
            }

            return returnVal;
        }

        private bool IsExistsInCaseOfDuplicate(DateTime date, int emgLocationId, int kfSubCategoryId)
        {
            if (IsDuplicate == 1)
                return (DBContext.GetData("IsKeyFiguresExistsInReports", new object[] { date, emgLocationId, kfSubCategoryId }).Rows.Count > 0);
            return false;
        }

        private void DeleteKeyFigure(int kfReportId)
        {
            DBContext.Delete("DeleteKeyFigure", new object[] { kfReportId, DBNull.Value });
        }

        private void SaveAdmin1Values(RepeaterItem mainRepeaterItem, DateTime date, int emgLocId, int kfSubCatId, int kfIndId)
        {
            Repeater rptAdmin1 = mainRepeaterItem.FindControl("rptAdmin1") as Repeater;
            if (rptAdmin1 != null)
            {
                foreach (RepeaterItem item in rptAdmin1.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        SaveKeyFigure(item, date, emgLocId, kfSubCatId, kfIndId);
                    }
                }
            }
        }
        private int GetValueFromHiddenField(RepeaterItem item, string controlName)
        {
            int value = 0;
            HiddenField hf = item.FindControl(controlName) as HiddenField;
            if (hf != null)
            {
                int.TryParse(hf.Value, out value);
            }

            return value;
        }

        private int? KeyFigureReported(RepeaterItem item, string controlName)
        {
            int? reportedValue = null;
            TextBox txt = item.FindControl(controlName) as TextBox;
            if (!string.IsNullOrEmpty(txt.Text.Trim()))
            {
                reportedValue = Convert.ToInt32(txt.Text.Trim());
            }

            return reportedValue;
        }

        private DataTable GetAdmin1Locations(int parentLocationId)
        {
            string procedure = "GetSecondLevelChildLocations";

            if (parentLocationId == 567)
            {
                procedure = "GetSecondLevelChildLocationsAndCountry";
            }

            DataTable dt = DBContext.GetData(procedure, new object[] { parentLocationId });
            return dt.Rows.Count > 0 ? dt : new DataTable();
        }

        private int SaveReportMain(int kfIndicatorId)
        {
            DateTime date = Convert.ToDateTime(txtFromDate.Text);
            int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
            int kfSubCategoryId = RC.GetSelectedIntVal(ddlSubCategory);

            return DBContext.Add("InsertKeyFigureReport", new object[] { emgLocationId, kfSubCategoryId, kfIndicatorId, date, RC.GetCurrentUserId, DBNull.Value });
        }

        internal void SaveKeyFigure(RepeaterItem item, DateTime date, int emgLocationId, int kfSubCatId, int kfIndId)
        {
            int locationId = GetValueFromHiddenField(item, "hfLocationId");

            int? totalTotal = KeyFigureReported(item, "txtTotalTotal");
            int? totalMen = KeyFigureReported(item, "txtTotalMen");
            int? totalWomen = KeyFigureReported(item, "txtTotalWomen");
            int? needTotal = KeyFigureReported(item, "txtNeedTotal");
            int? needMen = KeyFigureReported(item, "txtNeedMen");
            int? needWomen = KeyFigureReported(item, "txtNeedWomen");
            int? targatedTotal = KeyFigureReported(item, "txtTargetedTotal");
            int? targatedMen = KeyFigureReported(item, "txtTargetedMen");
            int? targatedWomen = KeyFigureReported(item, "txtTargetedWomen");

            string fromLocation = null;
            TextBox txt = item.FindControl("txtFromLocation") as TextBox;
            if (!string.IsNullOrEmpty(txt.Text.Trim()))
            {
                fromLocation = txt.Text.Trim();
            }

            string kfSource = null;
            TextBox txtKfSource = item.FindControl("txtKFSouce") as TextBox;
            if (txtKfSource != null)
            {
                if (!string.IsNullOrEmpty(txtKfSource.Text.Trim()))
                {
                    kfSource = txtKfSource.Text.Trim();
                }
            }


            bool valueProvied = (totalTotal != null || totalMen != null || totalWomen != null ||
                                 needTotal != null || needMen != null || needWomen != null ||
                                 targatedTotal != null || targatedMen != null || targatedWomen != null
                                 );
            if (locationId > 0 && valueProvied)
            {
                DBContext.Add("InsertKeyFigureReportDetails", new object[] {date, emgLocationId, kfSubCatId, kfIndId, locationId, 
                                                                            totalTotal, totalMen, totalWomen,
                                                                            needTotal, needMen, needWomen,
                                                                            targatedTotal, targatedMen, targatedWomen,
                                                                            fromLocation, kfSource, RC.GetCurrentUserId, DBNull.Value});
            }
        }

        protected void btnClick_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void rptKeyFigure_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (IsPopulation == 0)
                {
                    HtmlTableCell th = e.Item.FindControl("thFromLocTop") as HtmlTableCell;
                    if (th != null)
                        th.Visible = false;

                    th = e.Item.FindControl("thFromLoc") as HtmlTableCell;
                    if (th != null)
                        th.Visible = false;
                }
            }
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (IsPopulation == 0)
                {
                    HtmlTableCell td = e.Item.FindControl("tdFromLocTop") as HtmlTableCell;
                    if (td != null)
                        td.Visible = false;

                    td = e.Item.FindControl("tdFromLoc") as HtmlTableCell;
                    if (td != null)
                        td.Visible = false;

                }
                Repeater rptAdmin1 = e.Item.FindControl("rptAdmin1") as Repeater;
                if (rptAdmin1 != null)
                {
                    int kfReportId = 0;
                    HiddenField hfKeyFigureReportId = e.Item.FindControl("hfKeyFigureReportId") as HiddenField;
                    if (hfKeyFigureReportId != null)
                    {
                        int.TryParse(hfKeyFigureReportId.Value, out kfReportId);
                    }
                    int emgLocationId = RC.GetSelectedIntVal(ddlCountry);
                    DataTable dtTargets = DBContext.GetData("GetAdmin1ForKeyFigure", new object[] { kfReportId, emgLocationId });
                    rptAdmin1.DataSource = dtTargets;
                    rptAdmin1.DataBind();
                }
            }
        }

        protected void rptAdmin1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //int catId = RC.GetSelectedIntVal(ddlCategory);
                if (IsPopulation == 0)
                {
                    HtmlTableCell td = e.Item.FindControl("tdFromLocTop") as HtmlTableCell;
                    if (td != null)
                        td.Visible = false;

                    td = e.Item.FindControl("tdFromLoc") as HtmlTableCell;
                    if (td != null)
                        td.Visible = false;

                }
            }
        }

        public int IsPopulation
        {
            get
            {
                int id = 0;
                if (ViewState["IsPopulation"] != null)
                {
                    int.TryParse(ViewState["IsPopulation"].ToString(), out id);
                }

                return id;
            }
            set
            {
                ViewState["IsPopulation"] = value.ToString();
            }
        }

        public int IsDuplicate
        {
            get
            {
                int id = 0;
                if (ViewState["IsDuplicate"] != null)
                {
                    int.TryParse(ViewState["IsDuplicate"].ToString(), out id);
                }

                return id;
            }
            set
            {
                ViewState["IsDuplicate"] = value.ToString();
            }
        }
    }
}