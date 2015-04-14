using BusinessLogic;
using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SRFROWCA.KeyFigures
{
    public partial class AddKeyFigureIndicator : BasePage
    {
        protected void Page_PreLoad(object sender, EventArgs e)
        {
            string control = Utils.GetPostBackControlId(this);
            if (control == "btnAddIndicatorControl")
            {
                KFIndControlId += 1;
            }

            if (control == "btnRemoveIndicatorControl")
            {
                KFIndControlId -= 1;
            }

            if (KFIndControlId <= 1)
            {
                btnRemoveIndicatorControl.Visible = false;
            }
            else
            {
                btnRemoveIndicatorControl.Visible = true;
            }

            for (int i = 0; i < KFIndControlId; i++)
            {
                AddIndicatorControl(i);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                AddIndicatorControl(0);
                KFIndControlId += 1;
            }
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

        internal override void BindGridData()
        {
            LoadCategories();
            LoadSubCategories(0);
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

        protected void btnAddIndiatorControl_Click(object sender, EventArgs e)
        {

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Control ctl in pnlKeyFigCategory.Controls)
            {
                if (ctl != null && ctl.ID != null && ctl.ID.Contains("ctlCategoryId"))
                {
                    AddKeyFigureIndicatorCtrl ctlCategory = ctl as AddKeyFigureIndicatorCtrl;
                    if (ctlCategory != null)
                    {
                        int subCatId = RC.GetSelectedIntVal(ddlSubCategory);
                        if (subCatId > 0)
                        {
                            ctlCategory.Save(subCatId);
                        }
                    }
                }
            }

            Response.Redirect("KeyFigureIndicatorListing.aspx");
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = RC.GetSelectedIntVal(ddlCategory);
            if (categoryId > 0)
            {
                LoadSubCategories(categoryId);
            }
        }

        private void AddIndicatorControl(int i)
        {
            AddKeyFigureIndicatorCtrl ctlCategory = (AddKeyFigureIndicatorCtrl)LoadControl("~/KeyFigures/AddKeyFigureIndicatorCtrl.ascx");
            ctlCategory.ID = "ctlCategoryId" + i.ToString();
            pnlKeyFigCategory.Controls.Add(ctlCategory);
        }

        public int KFIndControlId
        {
            get
            {
                int indControlId = 0;
                if (ViewState["KFIndicatorId"] != null)
                {
                    int.TryParse(ViewState["KFIndicatorId"].ToString(), out indControlId);
                }

                return indControlId;
            }
            set
            {
                ViewState["KFIndicatorId"] = value.ToString();
            }
        }
    }
}